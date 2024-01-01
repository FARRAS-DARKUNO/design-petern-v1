using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using final_project_be.Models;
using final_project_be.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace final_project_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserClassController : ControllerBase
    {
        private readonly UserClassRepository _userClassRepository;

        public UserClassController(UserClassRepository userClassRepository)
        {
            _userClassRepository = userClassRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetSchedule()
        {
            string userId = User.FindFirstValue(ClaimTypes.Sid);
            List<UserClassModel> result = _userClassRepository.GetUserClass(int.Parse(userId));

            return Ok(result);
        }
    }
}