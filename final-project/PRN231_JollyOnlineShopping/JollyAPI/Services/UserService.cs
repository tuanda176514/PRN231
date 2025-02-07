using JollyAPI.DAO;
using JollyAPI.Models;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.DTOS.User;
using JollyAPI.Models.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;

namespace JollyAPI.Services
{
    public class UserService
    {
		private readonly UserDAO _userDao;
		private readonly CartDAO _cartDao;
		private readonly WishListDAO _wishListDao;

		public UserService(UserDAO userDao, CartDAO cartDao, WishListDAO wishListDao)
		{
			_userDao = userDao;
			_cartDao = cartDao;
			_wishListDao = wishListDao;
		}
		
		public List<UserDTO> GetAllUser() => _userDao.GetUser();

		public User CreateUserWithCartAndWishlist(UserDTO userDTO)
		{
			
			var user = new User
			{
				FullName = userDTO.FullName,
				Email = userDTO.Email,
				Password = userDTO.Password,
				Phone = userDTO.Phone,
                Status = userDTO.Status ?? true,
                Role = userDTO.Role ?? "user"
            };

			if (CheckUserExist(userDTO.Email))
			{
				throw new Exception("Duplicate");
			}


			var createdUser = _userDao.CreateUser(user);

			if (createdUser != null)
			{
				var cart = new Cart { UserId = createdUser.Id };
				var wishlist = new WishList { UserId = createdUser.Id };

				_cartDao.CreateCart(cart);
				_wishListDao.CreateWishList(wishlist);

				createdUser.WishlistId = wishlist.Id;
				createdUser.CartId = cart.Id;

				_userDao.UpdateUser(createdUser);

				return createdUser;
			}
			else
			{
				return null;
			}
		}

		public Boolean CheckUserExist (string email)
		{
			return _userDao.CheckUserExist(email);
		}

		public User GetUserById(int id)
		{
			return _userDao.GetUserById(id);
		}

		public void UpdateUser(int userId, UserDTO userDTO)
		{
			var user = _userDao.GetUserById(userId);

			if (user == null)
			{
				throw new Exception("User not exist");
			}

			user.FullName = userDTO.FullName;
			user.Email = userDTO.Email;
			user.Password = userDTO.Password;
			user.Phone = userDTO.Phone;
			//user.Status = userDTO.Status;
			user.Role = userDTO.Role;

			_userDao.UpdateUser(user);
		}

		public void ChangeStatus(User user)
		{
			_userDao.ChangeStatus(user);
		}

		public UserResponseDTO Login(string email, string password)
		{
            if (!_userDao.IsEmailExist(email))
            {
                return null;
            }

            var user = _userDao.GetUserByEmail(email);
			
			if (user != null && CheckPassword(user, password))
			{
				return MapUserToUserResponseDTO(user);

			}
			return null; 
		}

		private bool CheckPassword(User user, string password)
		{

			return user.Password == password;
		}

		private UserResponseDTO MapUserToUserResponseDTO(User user)
		{
			return new UserResponseDTO
			{
				Id = user.Id,
				Email = user.Email,
				FullName = user.FullName,
				Role = user.Role,
				Status = user.Status
			};
		}

		public bool UpdatePassword(int userId, PasswordUpdateDTO passwordUpdate)
		{
			var user = _userDao.GetUserById(userId);

			if (user == null)
			{
				return false;
			}

			if (passwordUpdate.NewPassword != passwordUpdate.ConfirmPassword)
			{
				return false;
			}

			if (passwordUpdate.CurrentPassword != user.Password)
			{
				return false;
			}

			user.Password = passwordUpdate.NewPassword;

			return _userDao.UpdatePassword(user);
		}

		public List<Address> GetAddressesWithUser(int userId)
		{
			return _userDao.GetAddressesByUserId(userId);
		}

        public Address GetAddressesWithUserIdAndId(int userId, int addressId)
        {
            return _userDao.GetAddressByUserIdAndId(userId, addressId);
        }

        public void AddAddressToUser(int userId, Address address)
		{
			_userDao.AddAddressToUser(userId, address);
		}

        public void CreateAddress(AddressesDTO addressesDTO)
        {
            var address = new Address
            {
				Ward = addressesDTO.Ward,
				Street = addressesDTO.Street,
                City = addressesDTO.City,
                District = addressesDTO.District,
                UserId = addressesDTO.UserId
            };

            _userDao.CreateAddress(address);
        }

        public AddressesDTO UpdateAddressByUserIdAndAddressId(int userId, int addressId, AddressesDTO updatedAddress)
        {
            return _userDao.UpdateAddressByUserIdAndId(userId, addressId, updatedAddress);
        }

        public async Task<string> ResetPasswordAsync(string email)
        {
            var user = await _userDao.GetUserByEmailAsync(email);
            if (user == null)
            {
                return null; 
            }

            string randomPassword = GenerateRandomPassword(); 
            await _userDao.ResetUserPasswordAsync(user, randomPassword);

            return randomPassword;
        }

        private string GenerateRandomPassword()
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            int passwordLength = 8; 
            var random = new Random();

            string newPassword = new string(Enumerable.Repeat(characters, passwordLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return newPassword;
        }
    }
}
