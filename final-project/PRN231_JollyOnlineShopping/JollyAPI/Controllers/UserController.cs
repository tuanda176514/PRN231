using JollyAPI.Models.DTOS;
using JollyAPI.Models.DTOS.User;
using JollyAPI.Models.Entity;
using JollyAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JollyAPI.Controllers
{
	[Route("users")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly UserService _userService;
		private readonly AddressService _addressService;

		public UserController(UserService userService, AddressService addressService)
		{
			_userService = userService;
			_addressService = addressService;
		}


		[HttpGet("get-all")]
		public ActionResult<IList<UserDTO>> GetAllUser() => _userService.GetAllUser();

		[HttpPost("register")]
		public ActionResult<User> CreateUserWithCartAndWishlist(UserDTO userDTO)
		{
			try
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
				var createdUser = _userService.CreateUserWithCartAndWishlist(userDTO);
				return Ok(createdUser);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}


		[HttpGet("{id}")]
		public IActionResult GetUserById(int id)
		{
			var user = _userService.GetUserById(id);

			if (user == null)
			{
				return NotFound();
			}

			return Ok(user);
		}

		[HttpPut("{id}")]
		public IActionResult UpdateUser(int id, [FromBody] UserDTO updatedUser)
		{
			try
			{
				_userService.UpdateUser(id, updatedUser);
				return Ok();
			}
			catch (Exception)
			{
				return BadRequest("Error");
			}
		}

		[HttpPut("changeStatus/{id}")]
		public IActionResult ChangeUserStatus(int id)
		{
			var existingUser = _userService.GetUserById(id);

			if (existingUser == null)
			{
				return NotFound();
			}

			existingUser.Status = !existingUser.Status;

			_userService.ChangeStatus(existingUser);

			return Ok();
		}

		[HttpPost("login")]
		public IActionResult LoginUser([FromBody] LoginDTO loginDTO)
		{
			var userResponse = _userService.Login(loginDTO.Email, loginDTO.Password);

			if (userResponse == null)
			{
				return Unauthorized("Unauthorized");
			}
			if(userResponse.Status == false)
			{
                return BadRequest("Deactive");
            }
			return Ok(userResponse);
		}

		[HttpPut("password/{userId}")]
		public IActionResult UpdatePassword(int userId, PasswordUpdateDTO passwordUpdate)
		{
			var result = _userService.UpdatePassword(userId, passwordUpdate);

			if (result)
			{
				return Ok("Password updated successfully");
			}
			else
			{
				return BadRequest("Failed to update password");
			}
		}
		[Route("address/{userId}")]
		[HttpGet]
		public IActionResult GetAddressesByUserId(int userId)
		{
			try
			{
				var addresses = _userService.GetAddressesWithUser(userId);

				if (addresses == null)
				{
					return NotFound("User not found");
				}

				return Ok(addresses);
			}
			catch (Exception ex)
			{
				return BadRequest("Failed to retrieve addresses: " + ex.Message);
			}
		}
        [Route("address/{userId}/{addressId}")]
        [HttpGet]
        public IActionResult GetAddressesByUserIdAndId(int userId, int addressId)
        {
            try
            {
                var addresses = _userService.GetAddressesWithUserIdAndId(userId, addressId);

                if (addresses == null)
                {
                    return NotFound("User not found");
                }

                return Ok(addresses);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to retrieve addresses: " + ex.Message);
            }
        }

        [Route("address/{userId}")]
		[HttpPost]
		public IActionResult AddAddressToUser(int userId, [FromBody] Address address)
		{
			try
			{
				_userService.AddAddressToUser(userId, address);
				return Ok("Address added successfully.");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        [Route("address")]
        [HttpPost]
        public IActionResult CreateAddress(AddressesDTO addressesDTO)
        {
            _userService.CreateAddress(addressesDTO);
            return Ok();
        }


        [HttpPut("address/{userId}/{addressId}")]
        public IActionResult UpdateAddress(int userId, int addressId, [FromBody] AddressesDTO updatedAddress)
        {
            var updated = _userService.UpdateAddressByUserIdAndAddressId(userId, addressId, updatedAddress);

            if (updated == null)
            {
                return NotFound(); 
            }

            return Ok(updated);
        }

        [Route("resetpassword")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] string email)
        {
            var randomPassword = await _userService.ResetPasswordAsync(email);
            if (!string.IsNullOrEmpty(randomPassword))
            {
                return Ok($"{randomPassword}");
            }
            else
            {
                return BadRequest("User not found or password reset failed.");
            }
        }

        [Route("address/{id}/{userId}")]
		[HttpDelete]
		public IActionResult RemoveAddress(int id, int userId)
		{
			try
			{
				if (!_addressService.CanUserDeleteAddress(id, userId))
				{
					return Forbid("You do not have permission to delete this address.");
				}

				_addressService.RemoveAddress(id, userId);

				return Ok("Address removed successfully.");
			}
			catch (Exception ex)
			{
				return BadRequest("Failed to remove address: " + ex.Message);
			}
		}

    }

}
