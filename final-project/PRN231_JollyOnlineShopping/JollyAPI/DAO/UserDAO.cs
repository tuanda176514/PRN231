using JollyAPI.Models;
using JollyAPI.Models.DTOS;
using JollyAPI.Models.DTOS.User;
using JollyAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace JollyAPI.DAO
{
    public class UserDAO
    {
        private readonly JollyShoppingOnlineContext _context;

        public UserDAO(JollyShoppingOnlineContext context)
        {
            _context = context;
        }

        public User CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public void UpdateUser(User user)
        {
            var existingUser = _context.Users.Find(user.Id);

            if (existingUser != null)
            {
                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                //existingUser.Password = user.Password;
                existingUser.Phone = user.Phone;
                //existingUser.Status = user.Status;
                existingUser.Role = user.Role;
                //existingUser.WishlistId = user.WishlistId;
                //existingUser.CartId = user.CartId;

                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Error");
            }
        }

        public User GetUserById(int id)
        {
            return _context.Users
                    .Include(u => u.Addresses)
                    .FirstOrDefault(u => u.Id == id);
        }

        public Boolean CheckUserExist(string email)
        {
            return _context.Users.FirstOrDefault(x => x.Email.ToLower().Equals(email.ToLower())) != null ? true : false;
        }


        public void UpdateUserById(int userId, UserDTO userDTO)
        {
            var existingUser = _context.Users.Find(userId);

            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            existingUser.FullName = userDTO.FullName;
            existingUser.Email = userDTO.Email;
            existingUser.Password = userDTO.Password;
            existingUser.Phone = userDTO.Phone;
            existingUser.Status = userDTO.Status;
            existingUser.Role = userDTO.Role;

            _context.SaveChanges();
        }

        public void ChangeStatus(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email.Equals(email));
        }

        public bool IsEmailExist(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool UpdatePassword(User user)
        {
            try
            {
                _context.Update(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Address> GetAddressesByUserId(int userId)
        {
            return _context.Addresses
                .Include(a => a.User)
                .Where(a => a.UserId == userId)
                .ToList();
        }
        public Address GetAddressByUserIdAndId(int userId, int addressId)
        {
            return _context.Addresses
              .Include(a => a.User)
              .Where(a => a.UserId == userId && a.Id == addressId)
              .FirstOrDefault();
        }

        public void AddAddressToUser(int userId, Address address)
        {
            var user = _context.Users.Find(userId);

            if (user != null)
            {
                user.Addresses.Add(address);
                _context.SaveChanges();
            }
            else
            {
                throw new("User not found");
            }
        }

        public void CreateAddress(Address address)
        {
            _context.Addresses.Add(address);
            _context.SaveChanges();
        }
        public AddressesDTO UpdateAddressByUserIdAndId(int userId, int addressId, AddressesDTO updatedAddress)
        {
            var address = GetAddressByUserIdAndId(userId, addressId);

            if (address == null)
            {
                return null; 
            }

            address.City = updatedAddress.City;
            address.District = updatedAddress.District;
            address.Ward = updatedAddress.Ward;
            address.Street = updatedAddress.Street;

            _context.SaveChanges(); 

            var updatedAddressDTO = new AddressesDTO
            {
                City = address.City,
                District = address.District,
                Ward = address.Ward,
                Street = address.Street,

            };

            return updatedAddressDTO; 
        }


        public List<UserDTO> GetUser()
        {
            return _context.Users.Where(x => x.Role.Equals("user")).Select(u => new UserDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Phone = u.Phone,
                Status = (bool)u.Status
            })
                .ToList();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task ResetUserPasswordAsync(User user, string newPassword)
        {
            user.Password = newPassword;
            await _context.SaveChangesAsync();
        }

    }
}
