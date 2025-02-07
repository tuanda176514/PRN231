using JollyAPI.DAO;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.Entity;
using System;

namespace JollyAPI.Services
{
    public class CartService
    {
		private readonly CartDAO cartDAO;
		public List<Item> items { get; set; }
		public CartService(CartDAO cartDAO)
		{
			this.cartDAO = cartDAO;
		}
		public int? GetTotalCartByUserId(int userId) => cartDAO.GetTotalCartByUserId(userId);

		public void AddToCart(CartDTO cartDTO) => cartDAO.AddToCart(cartDTO);
		public List<CartItemData> GetAllCartItem(int userId) => cartDAO.getAllCartItem(userId);
		public void DeleteCartItem(int itemId) => cartDAO.DeleteCartItem(itemId);
		public void PlusQuantity(int itemId) => cartDAO.PlusQuantity(itemId);
		public void MinusQuantity(int itemId) => cartDAO.MinusQuantity(itemId);

		private Boolean CheckExist(int id)
		{
			foreach (Item item in items)
			{
				if (item.Product.Id == id)
				{
					return true;
				}
			}
			return false;
		}

		private Item GetItemById(int id)
		{
			foreach(Item item in items)
			{
				if(item.Product.Id == id)
				{
					return item;
				}
			}
			return null;
		}
		public void AddItem(Item newItem)
		{
			if(CheckExist(newItem.Product.Id))
			{
				Item oldItem = GetItemById(newItem.Product.Id);
				oldItem.Quantity = oldItem.Quantity + newItem.Quantity;
			}
			items.Add(newItem);
		}

		public void RemoveItem(int id)
		{
			if(GetItemById(id) != null)
			{
				items.Remove(GetItemById(id));
			}
		}

		public decimal GetTotalMoney()
		{
			decimal t = 0;
			foreach(Item item in items)
			{
				t += (decimal) (item.Quantity * item.Product.Price);
			}
			return t;
		}
	}
}
