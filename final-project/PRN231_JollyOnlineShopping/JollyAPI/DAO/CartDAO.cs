using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace JollyAPI.DAO
{
    public class CartDAO
    {
		private readonly JollyShoppingOnlineContext _context;
		private readonly ProductDAO _productDAO;

		public CartDAO(JollyShoppingOnlineContext context, ProductDAO productDAO)
		{
			_productDAO = productDAO;
			_context = context;
		}
		public Cart CreateCart(Cart cart)
		{
			_context.Carts.Add(cart);
			_context.SaveChanges();
			return cart;
		}

		public void AddToCart(CartDTO cartDTO)
		{
			Cart cartExist = _context.Carts.Where(x => x.UserId == cartDTO.UserId).FirstOrDefault();
			if(cartExist != null)
			{
                Item itemExist = _context.Items
					.Where(x => x.Color == cartDTO.Items.First().Color)
					.Where(x => x.Size == cartDTO.Items.First().Size)
					.FirstOrDefault(x => x.ProductId == cartDTO.Items.First().ProductCartDTO.Id);
				if(itemExist != null)
				{
                    itemExist.Quantity += cartDTO.Items.First().Quantity;
                    _context.SaveChanges();
				}
				else
				{
                    foreach (var cartItemDTO in cartDTO.Items)
                    {
                        var cartItem = new Item
                        {
                            CartId = cartExist.Id,
                            Size = cartItemDTO.Size,
                            Color = cartItemDTO.Color,
                            Quantity = cartItemDTO.Quantity,
                            Product = _context.Products.Where(p => p.Id == cartItemDTO.ProductCartDTO.Id).FirstOrDefault()
                        };
                        _context.Items.Add(cartItem);
                    }

                    _context.SaveChanges();
                }


			}
			else
			{
                var cart = new Cart
                {
                    UserId = cartDTO.UserId
                };
                _context.Carts.Add(cart);
                _context.SaveChanges();

                int cartId = cart.Id;

                foreach (var cartItemDTO in cartDTO.Items)
                {
                    var cartItem = new Item
                    {
                        CartId = cartId,
                        Size = cartItemDTO.Size,
                        Color = cartItemDTO.Color,
                        Quantity = cartItemDTO.Quantity,
                        Product = _context.Products.Where(p => p.Id == cartItemDTO.ProductCartDTO.Id).FirstOrDefault()
                    };
                    _context.Items.Add(cartItem);
                }

                _context.SaveChanges();
            }
            
		}

        public int? GetTotalCartByUserId(int userId)
        {
            int? totalQuantity = _context.Items
                .Where(x => x.Cart.UserId == userId)
                .Sum(x => x.Quantity);

            return totalQuantity;
        }

        public List<CartItemData> getAllCartItem(int userId)
        {
            return _context.Items
        .Where(x => x.Cart.UserId == userId)
        .Select(item => new CartItemData
        {
            Id = item.Id,
            ProductId = item.ProductId,
            Color = item.Color.Trim(),
            Size = item.Size.Trim(),
            Quantity = item.Quantity,
            Name = item.Product.Name,
            Image = item.Product.Images.Select(image => image.Url).FirstOrDefault(),
            Price = item.Product.Price
        })
        .ToList();
        }

        public void DeleteCartItem(int itemId)
        {
            Item item = _context.Items.Where(i => i.Id == itemId).FirstOrDefault();
            _context.Items.Remove(item);
            _context.SaveChanges();
        }

        public void PlusQuantity(int itemId)
        {
            Item item = _context.Items.Where(i => i.Id == itemId).FirstOrDefault();
            item.Quantity++;
            _context.SaveChanges();
        }

        public void MinusQuantity(int itemId)
        {
            Item item = _context.Items.Where(i => i.Id == itemId).FirstOrDefault();
            item.Quantity--;
            _context.SaveChanges();
        }

    }
}
