using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;
using JollyAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JollyAPI.Controllers
{
    [Route("api/[cart]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService cartService;

        public CartController(CartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpPost("/cart/add")]
        public IActionResult AddToCart(CartDTO cartDTO)
        {
            try
            {
                cartService.AddToCart(cartDTO);
                return Ok(new { Message = "Cart added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing the request.", Error = ex.Message });
            }
        }
        [HttpPost("/cart/item/delete/{itemId}")]
        public IActionResult DeleteCartItem(int itemId)
        {
            try
            {
                cartService.DeleteCartItem(itemId);
                return Ok(new { Message = "Cart item deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing the request.", Error = ex.Message });
            }
        }
        [HttpPost("/cart/item/plus/{itemId}")]
        public void PlusQuantity(int itemId) => cartService.PlusQuantity(itemId);
        [HttpPost("/cart/item/minus/{itemId}")]
        public void MinusQuantity(int itemId) => cartService.MinusQuantity(itemId);

        [HttpGet("/cart/total/{userId}")]
        public ActionResult<int> GetCartByUserId(int userId) => cartService.GetTotalCartByUserId(userId);

		[HttpGet("/cart/{userId}")]
		public ActionResult<List<CartItemData>> GetCartItemByUserId(int userId) => cartService.GetAllCartItem(userId);

	}
}
