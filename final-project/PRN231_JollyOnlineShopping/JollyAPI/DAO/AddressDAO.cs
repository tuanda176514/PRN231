using JollyAPI.Models.Entity;

namespace JollyAPI.DAO
{
	public class AddressDAO
	{
		private readonly JollyShoppingOnlineContext _context;

		public AddressDAO(JollyShoppingOnlineContext context)
		{
			_context = context;
		}

		public void AddAddress(Address address)
		{
			_context.Addresses.Add(address);
			_context.SaveChanges();
		}

		public bool CanUserDeleteAddress(int addressId, int userId)
		{
			var address = _context.Addresses.FirstOrDefault(a => a.Id == addressId && a.UserId == userId);
			return address != null;
		}

		public void RemoveAddress(int addressId, int userId)
		{
			var address = _context.Addresses.FirstOrDefault(a => a.Id == addressId && a.UserId == userId);
            if (address != null)
			{
				_context.Addresses.Remove(address);
				_context.SaveChanges();
			}
		}
	}
}
