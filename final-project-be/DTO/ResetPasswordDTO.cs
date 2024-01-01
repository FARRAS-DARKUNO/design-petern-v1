using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace final_project_be.DTO
{
    public class ResetPasswordDTO
    {
        public string Email { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}