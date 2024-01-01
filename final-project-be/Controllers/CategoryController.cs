using final_project_be.DTO;
using final_project_be.Models;
using final_project_be.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace final_project_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoryController(CategoryRepository category, IWebHostEnvironment webHostEnvironment)
        {
            _categoryRepository = category;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryDTO data)
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

            bool category = _categoryRepository.InsertCategory(data, fileUrlPath);
            if (category)
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

        public IActionResult GetAll([FromQuery] bool? status)
        {
            List<CategoryModel> result = _categoryRepository.GetListCategoty(status);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id, [FromQuery] string? type)
        {
            CategoryModel result = _categoryRepository.GetCategory(id, type);

            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById([FromForm] CategoryDTO data, int id)
        {
            CategoryModel categoryById = _categoryRepository.GetCategory(id, "all");

            string fileUrlPath = categoryById.Image!;
            data.Name ??= categoryById.Name;
            data.Description ??= categoryById.Description;

            if (data.Image != null)
            {
                IFormFile image = data.Image;

                var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
                string fileName = Guid.NewGuid().ToString() + ext;


                string uploadDir = "uploads";
                string physicalPath = $"wwwroot/{uploadDir}";
                var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, physicalPath, fileName);
                using var stream = System.IO.File.Create(filePath);
                await image.CopyToAsync(stream);

                fileUrlPath = $"{uploadDir}/{fileName}";
            }

            bool category = _categoryRepository.UpdateCategory(data, fileUrlPath, id);
            if (category)
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
        public IActionResult UpdateStatus([FromQuery] int id, bool status)
        {
            bool category = _categoryRepository.UpdateStatusCategory(id, status);
            if (category)
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
    }
}