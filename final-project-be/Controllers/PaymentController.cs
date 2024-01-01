
using final_project_be.DTO;
using final_project_be.Models;
using final_project_be.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace final_project_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentRepository _paymentRepository;
        private readonly IWebHostEnvironment _webHostEnvirontment;
        public PaymentController(PaymentRepository payment, IWebHostEnvironment webHostEnvirontment)
        {
            _paymentRepository = payment;
            _webHostEnvirontment = webHostEnvirontment;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromForm] PaymentDTO data)
        {
            IFormFile image = data.Image!;

            var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
            string fileName = Guid.NewGuid().ToString() + ext;

            string uploadDir = "uploads";
            string physicalPath = $"wwwroot/{uploadDir}";
            var filePath = Path.Combine(_webHostEnvirontment.ContentRootPath, physicalPath, fileName);
            using var stream = System.IO.File.Create(filePath);
            await image.CopyToAsync(stream);

            string fileUrlPath = $"{uploadDir}/{fileName}";

            bool payment = _paymentRepository.InsertPayment(data, fileUrlPath);
            if (payment)
            {
                return Ok(new
                {
                    status = "Success",
                });
            }

            return BadRequest(new
            {
                status = "Failed",
            });
        }

        [HttpGet]
        public IActionResult GetPayment([FromQuery] bool? status)
        {
            List<PaymentModel> result = _paymentRepository.GetListPayment(status);

            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById([FromForm] PaymentDTO data, int id)
        {
            PaymentModel result = _paymentRepository.GetListPaymentById(id);

            string fileUrlPath = result.Image!;

            data.Name ??= result.Name;

            if (data.Image != null)
            {
                IFormFile image = data.Image!;

                var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
                string fileName = Guid.NewGuid().ToString() + ext;

                string uploadDir = "uploads";
                string physicalPath = $"wwwroot/{uploadDir}";
                var filePath = Path.Combine(_webHostEnvirontment.ContentRootPath, physicalPath, fileName);
                using var stream = System.IO.File.Create(filePath);
                await image.CopyToAsync(stream);

                fileUrlPath = $"{uploadDir}/{fileName}";
            }


            bool payment = _paymentRepository.UpdatePayment(data, fileUrlPath, id);
            if (payment)
            {
                return Ok(new
                {
                    status = "Success",
                });
            }
            return BadRequest(new
            {
                status = "Failed",
            });
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut]
        public IActionResult UpdateStatus([FromQuery] int id, bool status)
        {


            bool payment = _paymentRepository.UpdateStatusPayment(status, id);
            if (payment)
            {
                return Ok(new
                {
                    status = "Success",
                });
            }
            return BadRequest(new
            {
                status = "Failed",
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetPayment(int id)
        {
            PaymentModel result = _paymentRepository.GetListPaymentById(id);

            return Ok(result);
        }
    }
}