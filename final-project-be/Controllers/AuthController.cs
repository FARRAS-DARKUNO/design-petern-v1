using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using final_project_be.DTO;
using final_project_be.Helpers;
using final_project_be.Models;
using final_project_be.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace final_project_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthRepository _authRepository;
        public AuthController(AuthRepository auth)
        {
            _authRepository = auth;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDTO data)
        {
            string hashedPassword = PasswordHelper.EncryptPassword(data.Password);

            UserModel? user = _authRepository.GetByEmailAndPassword(data.Email, hashedPassword, false);

            if (user == null || user.IsActive == false)
            {
                return NotFound();
            }

            //create token
            string token = JWTHelper.Generate(user.Id, user.Role);

            return Ok(new
            {
                tokenUser = token,
                role = user.Role
            });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO data, string? link)
        {
            string hashedPassword = PasswordHelper.EncryptPassword(data.Password);

            UserModel? user = _authRepository.GetByEmailAndPassword(data.Email, hashedPassword, true);

            bool register = false;

            if (user == null)
            {
                register = _authRepository.CreateRegister(data, hashedPassword);
                if (data.IsActive == false)
                {
                    string resetLink = link + data.Email;
                    Console.WriteLine(resetLink);

                    await MailHelper.Send(data.Email, "Active Account", "Hello " + data.Name + ", Please press this link to avtive your account " + resetLink);
                }
            }


            if (register)
            {
                return Ok(new
                {
                    status = "Succsess",
                });
            }

            return BadRequest(new
            {
                status = "Failid",
            });
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO data)
        {
            //check email valid
            UserModel? user = _authRepository.GetByEmail(data.Email);
            if (user == null)
            {
                return NotFound();
            }

            string key = DateTime.UtcNow.Ticks.ToString() + data.Email;
            string resetToken = PasswordHelper.EncryptPassword(key);

            //update reset token
            bool isSuccess = _authRepository.InsertResetPasswordToken(user.Id, resetToken);

            if (!isSuccess)
            {
                return Problem();
            }

            string resetLink = data.Link + data.Email + "&token=" + resetToken;

            await MailHelper.Send(data.Email, "Forgot password", "Hello " + user.Email + ", there was recently a request to change the password for your account." + "If you requested this password change, please reset your password here: " + resetLink);

            return Ok();
        }

        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDTO data)
        {
            //check if email valid
            UserModel? user = _authRepository.GetByEmailAndResetToken(data.Email, data.Token);
            if (user == null)
            {
                return NotFound();
            }

            string hashedPassword = PasswordHelper.EncryptPassword(data.NewPassword);

            bool isResetSuccess = _authRepository.UpdatePassword(user.Id, hashedPassword);

            if (!isResetSuccess)
            {
                return NotFound(new
                {
                    status = "Failed"
                });
            }

            return Ok(new
            {
                status = "Success"
            });
        }
        [HttpPut]
        public IActionResult ActiveAuth([FromQuery] string email)
        {
            bool active = _authRepository.ActiveUser(email);

            if (!active)
            {
                return NotFound(new
                {
                    status = "Failed"
                });
            }

            return Ok(new
            {
                status = "Success"
            });
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public IActionResult ChangeStatusAuth(int id, [FromQuery] bool status)
        {
            bool active = _authRepository.ChangeStatus(status, id);

            if (!active)
            {
                return NotFound(new
                {
                    status = "Failed"
                });
            }

            return Ok(new
            {
                status = "Success"
            });
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult GetDataUser()
        {
            List<UserModel> userData = _authRepository.getDataUser();

            return Ok(userData);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("{id}")]
        public IActionResult GetDataUserById(int id)
        {
            UserModel userData = _authRepository.GetDataUserById(id);

            return Ok(userData);
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateDataUser(int id, [FromBody] RegisterDTO data)
        {
            UserModel userData = _authRepository.GetDataUserById(id);

            data.Name = data.Name == null || data.Name == "" ? userData.Name : data.Name;
            data.Email = data.Email == null || data.Email == "" ? userData.Email : data.Email;
            data.Role = data.Role == null || data.Role == "" ? userData.Role : data.Role;

            bool active = _authRepository.UpdateUser(data, id);

            if (!active)
            {
                return NotFound(new
                {
                    status = "Failed"
                });
            }

            return Ok(new
            {
                status = "Success"
            });
        }
    }
}