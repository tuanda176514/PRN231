using JollyAPI.DAO;

namespace JollyAPI.Services
{
	public class WishListService
	{
		private readonly WishListDAO dao;
		public WishListService(WishListDAO dao)
		{
			this.dao = dao;
		}

		public int TotalItem(int uid)
		{
			return dao.TotalItem(uid);
		}
		public bool ModifyItem(int uid, int pid)
		{
			return dao.ModifyItem(uid, pid);
		}

	}
}
