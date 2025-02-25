using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.DTOs;
using RealEstateApp.Application.Interfaces;
using System.Net;

namespace RealEstateApp.Presentation.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// ثبت‌نام کاربر جدید
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RegisterUserAsync(userDto);

            if (!result)
            {
                return BadRequest("خطا در ثبت‌نام کاربر.");
            }

            return Ok("کاربر با موفقیت ثبت شد.");
        }

        /// <summary>
        /// ورود کاربر و دریافت توکن JWT
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _userService.AuthenticateUserAsync(loginDto);

            if (token == null)
            {
                return Unauthorized("نام کاربری یا رمز عبور اشتباه است.");
            }

            return Ok(new { Token = token });
        }

        /// <summary>
        /// دریافت اطلاعات یک کاربر با شناسه
        /// </summary>
        [HttpGet("{userId}")]
        [Authorize] // نیاز به احراز هویت
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound("کاربر یافت نشد.");
            }

            return Ok(user);
        }
        /// <summary>
        /// دریافت تمام کاربران
        /// </summary>
        [HttpGet]
        [Authorize] // تنها مدیران می‌توانند تمام کاربران را مشاهده کنند
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            if (users == null || !users.Any())
            {
                return NotFound("هیچ کاربری یافت نشد.");
            }

            return Ok(users);
        }


        /// <summary>
        /// اختصاص یک نقش به کاربر
        /// </summary>
        [HttpPost("{userId}/assign-role")]
        [Authorize(Roles = "Admin")] // تنها مدیران می‌توانند نقش‌ها را تخصیص دهند
        public async Task<IActionResult> AssignRole(string userId, [FromBody] string roleName)
        {
            var result = await _userService.AssignRoleAsync(userId, roleName);

            if (!result)
            {
                return BadRequest("خطا در اختصاص نقش به کاربر.");
            }

            return Ok("نقش با موفقیت تخصیص داده شد.");
        }

        /// <summary>
        /// دریافت نقش‌های کاربر
        /// </summary>
        [HttpGet("{userId}/roles")]
        [Authorize] // نیاز به احراز هویت
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var roles = await _userService.GetUserRolesAsync(userId);

            if (roles == null || !roles.Any())
            {
                return NotFound("هیچ نقشی برای این کاربر یافت نشد.");
            }

            return Ok(roles);
        }
    }
}
