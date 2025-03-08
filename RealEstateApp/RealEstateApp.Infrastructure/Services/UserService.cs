using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RealEstateApp.Application.DTOs;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Infrastructure.Identity.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealEstateApp.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _mapper = mapper;
    }
    
    public async Task<bool> RegisterUserAsync(UserDto userDto)
    {
        // بررسی اینکه آیا کاربر با این نام کاربری یا ایمیل وجود دارد
        var existingUser = await _userManager.FindByNameAsync(userDto.UserName);
        if (existingUser != null)
        {
            LogMessage($"User with username '{userDto.UserName}' already exists.");
            return false;
        }

        var existingEmail = await _userManager.FindByEmailAsync(userDto.Email);
        if (existingEmail != null)
        {
            LogMessage($"User with email '{userDto.Email}' already exists.");
            return false;
        }

        // ایجاد کاربر جدید
        var user = _mapper.Map<ApplicationUser>(userDto);
        var registrationResult = await _userManager.CreateAsync(user, userDto.Password);

        if (!registrationResult.Succeeded)
        {
            LogErrors(registrationResult.Errors, "Registration");
            return false;
        }

        // اضافه کردن کاربر به نقش
        var roleResult = await _userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
        {
            LogErrors(roleResult.Errors, "Role Assignment");
            return false;
        }

        // ثبت‌نام با موفقیت انجام شد
        LogMessage("User registered successfully.");
        return true;
    }

    // متد برای لاگ پیام‌های ساده
    private void LogMessage(string message)
    {
        Console.WriteLine($"[INFO] {message}");
    }

    public async Task<string> AuthenticateUserAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.UserName);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);
        return GenerateJwtToken(user, roles);
    }

    public async Task<UserDetailsDto> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);
        var userDetails = _mapper.Map<UserDetailsDto>(user);
        userDetails.Roles = roles.ToList();

        return userDetails;
    }
    public async Task<List<UserDetailsDto>> GetAllUsersAsync()
    {
        var users = _userManager.Users.ToList();  // گرفتن تمام کاربران
        if (users == null || !users.Any()) return new List<UserDetailsDto>();

        var userDtos = new List<UserDetailsDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userDetailsDto = _mapper.Map<UserDetailsDto>(user);
            userDetailsDto.Roles = roles.ToList();
            userDtos.Add(userDetailsDto);
        }

        return userDtos;
    }
    public async Task<List<string>> GetAllRolesAsync()
    {
        return _roleManager.Roles.Select(r => r.Name).ToList();
    }

    public async Task<bool> AssignRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || !await _roleManager.RoleExistsAsync(roleName))
        {
            return false;
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded;
    }

    public async Task<List<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user == null ? null : (await _userManager.GetRolesAsync(user)).ToList();
    }

    private string GenerateJwtToken(ApplicationUser user, IList<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(double.Parse(_configuration["JwtSettings:TokenLifetime"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private void LogErrors(IEnumerable<IdentityError> errors, string context)
    {
        foreach (var error in errors)
        {
            Console.WriteLine($"[{context}] Error: {error.Description}");
        }
    }
}
