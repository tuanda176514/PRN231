using JollyAPI.DAO;
using JollyAPI.Models.DTO.User;
using JollyAPI.Models.Entity;

namespace JollyAPI.Service
{
    public class UserServices
    {
        private readonly UserDAO _userDAO;
        private readonly CartDAO _cartDAO;
        private readonly WishListDAO _wishListDAO;
        public UserServices(UserDAO userDAO, CartDAO cartDAO, WishListDAO wishListDAO)
        {
            _userDAO = userDAO;
            _cartDAO = cartDAO;
            _wishListDAO = wishListDAO;
        }
        public User GetUserById(int id)
        {
            return _userDAO.GetUserById(id);
        }
        public User GetUserByEmail(string email)
        {
            return _userDAO.GetUserByEmail(email);
        }
        public User RegisterUser(UserDTO userDTO)
        {
            var user = new User
            {
                FullName = userDTO.FullName,
                Email = userDTO.Email,
                Password = userDTO.Password,
                Phone = userDTO.Phone,
                Role = userDTO.Role ?? "user",
                Status = userDTO.Status ?? true
            };

            if (_userDAO.GetUserByEmail(userDTO.Email) == null)
            {
                throw new Exception("Email already exists");
            }

            var newUser = _userDAO.CreateUser(user);

            if (newUser != null)
            {
                var cart = new Cart
                {
                    UserId = newUser.Id
                };
                var wishlist = new WishList
                {
                    UserId = newUser.Id
                };

                _cartDAO.CreateCart(cart);
                _wishListDAO.CreateWishList(wishlist);

                newUser.WishlistId = wishlist.Id;
                newUser.CartId = cart.Id;

                _userDAO.UpdateUser(newUser);

                return newUser;

            }
            else
            {
                return null;
            }

        }
        public User UpdateUser(User user)
        {
            return _userDAO.UpdateUser(user);
        }
        public void DeleteUser(int id)
        {
            _userDAO.DeleteUser(id);
        }
        public List<UserDTO> GetAllUsers()
        {
            return _userDAO.GetAllUsers();
        }

        public User GetUserByEmailAndPassword(string email, string password)
        {
            return _userDAO.GetUserByEmailAndPassword(email, password);
        }
    }
}
