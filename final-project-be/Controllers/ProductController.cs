
using final_project_be.DTO;
using final_project_be.Models;
using final_project_be.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace final_project_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository _productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(ProductRepository Product, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = Product;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductDTO data)
        {
            IFormFile image = data.Image!;

            var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
            string fileName = Guid.NewGuid().ToString() + ext;


            string uploadDir = "uploads";
            string physicalPath = $"wwwroot/{uploadDir}";
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, physicalPath, fileName);
            using var stream = System.IO.File.Create(filePath);
            await image.CopyToAsync(stream);

            string fileUrlPath = $"{uploadDir}/{fileName}";

            bool product = _productRepository.InsertProduct(data, fileUrlPath);

            if (product)
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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateData([FromForm] ProductDTO data, int id)
        {
            ProductDetailModel productById = _productRepository.GetDetailProduct(id);

            string fileUrlPath = productById.Image!;
            data.DescriptionProduct ??= productById.Description;
            data.TitleProduct ??= productById.Name;
            data.Price ??= productById.Price;
            data.CategoryId ??= productById.CategoryId;

            if (data.Image != null)
            {
                IFormFile image = data.Image!;

                var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
                string fileName = Guid.NewGuid().ToString() + ext;


                string uploadDir = "uploads";
                string physicalPath = $"wwwroot/{uploadDir}";
                var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, physicalPath, fileName);
                using var stream = System.IO.File.Create(filePath);
                await image.CopyToAsync(stream);

                fileUrlPath = $"{uploadDir}/{fileName}";
            }

            bool product = _productRepository.UpdateProduct(data, fileUrlPath, id);


            if (product)
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
        [HttpPut]
        public IActionResult UpdateStatus([FromQuery] bool isActive, int id)
        {
            bool product = _productRepository.UpdateStatus(isActive, id);


            if (product)
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

        [HttpGet]
        public IActionResult GetListProduct([FromQuery] int? limit, int? categoryId, int? productId)
        {
            List<ProductModel> result = _productRepository.GetProduct(limit, categoryId, productId);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetDetailProduct(int id)
        {
            ProductDetailModel result = _productRepository.GetDetailProduct(id);
            return Ok(result);
        }
    }
}