using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Application.DTOs
{
    public class UserDto
    {
        public string UserName { get; set; }  // نام کاربری
        public string Email { get; set; }     // ایمیل
        public string Password { get; set; }  // رمز عبور
        public string FullName { get; set; }  // نام کامل
    }

    public class LoginDto
    {
        public string UserName { get; set; }  // نام کاربری برای ورود
        public string Password { get; set; }  // رمز عبور برای ورود
    }
    public class UserDetailsDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
    }
}
