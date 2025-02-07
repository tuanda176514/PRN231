using JollyAPI.Models.DTO.User;
using JollyAPI.Models.Entity;
using JollyAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JollyAPI.Controller
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userServices;

        public UserController(UserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet("get-all")]
        public ActionResult<IList<User>> GetAllUsers()
        {
            var users = _userServices.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userServices.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("GetUserByEmail/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            var user = _userServices.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser([FromBody] Models.Entity.User user)
        {
            var updatedUser = _userServices.UpdateUser(user);
            return Ok(updatedUser);
        }

        [HttpDelete("DeleteUser")]
        public IActionResult DeleteUser(int id)
        {
            _userServices.DeleteUser(id);
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login user)
        {
            var userLogin = _userServices.GetUserByEmailAndPassword(user.Email, user.Password);
            if (userLogin == null)
            {
                return NotFound();
            }
            return Ok(userLogin);
        }

        [HttpPost("register")]
        public ActionResult<User> Register(UserDTO userDTO)
        {
            try
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
                var newUser = _userServices.RegisterUser(userDTO);
                return Ok(newUser);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
