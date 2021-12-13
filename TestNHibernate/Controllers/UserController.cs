using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServicesInterfaces;
using TestBaseDto;

namespace TestNHibernate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserAsync(id);
            return Ok(user);
        }

        [HttpPost("UpdateUSer")]
        public async Task<IActionResult> UpdateUser(UserDto user)
        {
            await _userService.UpdateUserAsync(user);
            return Ok();
        }

        [HttpPut("CreateUser")]
        public async Task<IActionResult> CreateUser(UserDto user)
        {
            await _userService.CreateUserAsync(user);
            return Ok();
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(UserDto user)
        {
            await _userService.DeleteUserAsync(user.Id);
            return Ok();
        }
    }
}