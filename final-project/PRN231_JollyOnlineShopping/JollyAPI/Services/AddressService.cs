using JollyAPI.DAO;
using JollyAPI.Models;
using JollyAPI.Models.Entity;

namespace JollyAPI.Services
{
	public class AddressService
	{
		private readonly AddressDAO _addressDao;
		

		public AddressService(AddressDAO addressDao)
		{
			_addressDao = addressDao;
		}

		public void AddAddress(Address address)
		{
			_addressDao.AddAddress(address);
		}

		public bool CanUserDeleteAddress(int addressId, int userId)
		{
			return _addressDao.CanUserDeleteAddress(addressId, userId);
		}

		public void RemoveAddress(int addressId, int userId)
		{
			_addressDao.RemoveAddress(addressId, userId);
		}
	}
}
