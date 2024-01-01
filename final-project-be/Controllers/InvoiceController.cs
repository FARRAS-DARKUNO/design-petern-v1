
using System.Security.Claims;
using final_project_be.DTO;
using final_project_be.Models;
using final_project_be.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace final_project_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceRepository _invoiceRepository;
        public InvoiceController(InvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        [Authorize]
        [HttpPost]
        public IActionResult InsertInvoice([FromBody] InvoiceDTO data)
        {
            string userId = User.FindFirstValue(ClaimTypes.Sid);
            bool invoice = _invoiceRepository.InsertInvoice(data, int.Parse(userId));

            if (invoice)
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

        [Authorize]
        [HttpGet("User")]
        public IActionResult GetInvoiceById()
        {
            string userId = User.FindFirstValue(ClaimTypes.Sid);
            List<InvoiceModel> invoices = _invoiceRepository.GetInvoiceById(int.Parse(userId));

            return Ok(invoices);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetDetailInvoice([FromQuery] string? number)
        {
            if (number == null)
            {
                List<InvoiceListModel> invoice = _invoiceRepository.GetAdminData();
                return Ok(invoice);
            }
            DetailInvoiceModel detail = _invoiceRepository.GetDetailInvoice(number!);

            return Ok(detail);
        }

        // [Authorize]
        [HttpGet("admin")]
        public IActionResult GetInvoice()
        {
            List<InvoiceModel> invoices = _invoiceRepository.GetInvoiceAdmin();

            return Ok(invoices);
        }
    }
}