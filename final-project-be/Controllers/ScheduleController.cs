using final_project_be.DTO;
using final_project_be.Models;
using final_project_be.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace final_project_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleRepository _scheduleRepository;

        public ScheduleController(ScheduleRepository schedule)
        {
            _scheduleRepository = schedule;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult InsertSchedule([FromBody] ScheduleDTO data)
        {
            bool schedule = _scheduleRepository.InsertSchedule(data);

            if (schedule)
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
        
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult GetSchedule ()
        {
            List<ScheduleModel> result = _scheduleRepository.GetSchedule();

            return Ok(result);
        }

    }
}