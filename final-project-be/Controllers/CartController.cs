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
    public class CartController : ControllerBase
    {
        private readonly CartRepository _cartReposirory;
        public CartController(CartRepository cartReposirory)
        {
            _cartReposirory = cartReposirory;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateCart([FromBody] CartDTO data)
        {
            string userId = User.FindFirstValue(ClaimTypes.Sid);

            CartModel checkByUser = _cartReposirory.CheckCartByUser(int.Parse(userId), data.IdSchedule);

            if (checkByUser != null)
            {
                return BadRequest(new
                {
                    status = "Sudah ada dalam cart",
                });
            }

            Console.WriteLine(userId);

            bool cart = _cartReposirory.InsertCart(data, int.Parse(userId));

            if (cart)
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
        [HttpGet]
        public IActionResult GetCart()
        {
            string userId = User.FindFirstValue(ClaimTypes.Sid);

            List<CartModel> result = _cartReposirory.GetCart(int.Parse(userId));

            return Ok(result);

        }

        [Authorize]
        [HttpDelete]
        public IActionResult DelleteCart([FromQuery] int IdChart)
        {
            bool cart = _cartReposirory.DelleteCartById(IdChart);

            if (cart)
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