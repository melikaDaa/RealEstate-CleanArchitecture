using Microsoft.AspNetCore.Identity;
using RealEstateApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RealEstateApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(UserDto userDto);
        Task<string> AuthenticateUserAsync(LoginDto loginDto);
        Task<UserDetailsDto> GetUserByIdAsync(string userId);
        Task<bool> AssignRoleAsync(string userId, string roleName);
        Task<List<string>> GetUserRolesAsync(string userId);
         Task<List<UserDetailsDto>> GetAllUsersAsync();
        Task<List<string>> GetAllRolesAsync();
    }
}
