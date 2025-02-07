using JollyAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace JollyAPI.DAO
{
	public class WishListDAO
	{

		private readonly JollyShoppingOnlineContext _context;

		public WishListDAO(JollyShoppingOnlineContext context)
		{
			_context = context;
		}

		public WishList CreateWishList(WishList wishlist)
		{
			_context.WishLists.Add(wishlist);
			_context.SaveChanges();
			return wishlist;
		}
		public int TotalItem(int uid)
		{
			var user = _context.Users.Where(x => x.Id == uid).Include(x => x.Wishlist).ThenInclude(x => x.WishItems).FirstOrDefault();
			if (user == null) return 0;
			return user.Wishlist.WishItems.Count;
		}

		public bool ModifyItem(int uid, int pid)
		{
			var user = _context.Users.Find(uid);
			if (user == null)
			{
				throw new Exception();
			}

			var wishItem = _context.WishItems.Where(x => x.WishlistId == user.WishlistId && pid == x.ProductId).FirstOrDefault();
			if (wishItem != null)
			{
				_context.Remove(wishItem);
				_context.SaveChanges();
				return false;
			}
			else
			{
				wishItem = new WishItem();
				wishItem.WishlistId = user.WishlistId;
				wishItem.ProductId = pid;
				_context.WishItems.Add(wishItem);
				_context.SaveChanges();
				return true;
			}

		}

	}
}
