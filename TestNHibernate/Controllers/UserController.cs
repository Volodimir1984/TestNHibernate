using ConstData;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ServicesInterfaces.Users;
using System;
using System.Threading.Tasks;
using TestBaseDto;

namespace TestNHibernate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IRequestClient<IUsersData> _usersRequestClient;
        private readonly IRequestClient<IUserPrimaryData> _userRequestClient;
        private readonly IRequestClient<IUserUpdate> _userUpdateRequestClient;
        private readonly IRequestClient<IUserCreate> _userCreateRequestClient;
        private readonly IRequestClient<IUserDelete> _userDeleteRequestClient;

        public UserController(IRequestClient<IUsersData> usersRequestClient,
            IRequestClient<IUserPrimaryData> userRequestClient, 
            IRequestClient<IUserUpdate> userUpdateRequestClient, 
            IRequestClient<IUserCreate> userCreateRequestClient, IRequestClient<IUserDelete> userDeleteRequestClient)
        {
            _usersRequestClient = usersRequestClient;
            _userRequestClient = userRequestClient;
            _userUpdateRequestClient = userUpdateRequestClient;
            _userCreateRequestClient = userCreateRequestClient;
            _userDeleteRequestClient = userDeleteRequestClient;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _usersRequestClient.GetResponse<IUsersData>(new { });
            return Ok(users.Message);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRequestClient.GetResponse<IUserData>(new {id});
            return Ok(user.Message);
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserDto user)
        {
            var userId = await _userUpdateRequestClient.GetResponse<IUserPrimaryData>(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.CompanyId
            });

            return Ok(userId.Message);
        }

        [HttpPut("CreateUser")]
        public async Task<IActionResult> CreateUser(UserDto user)
        {
            var userData = await _userCreateRequestClient.GetResponse<IUserData>(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.CompanyId
            });

            return Ok(userData.Message);
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(UserDto user)
        {
            var userId = await _userDeleteRequestClient.GetResponse<IUserPrimaryData>(new
            {
                user.Id
            });

            return Ok(userId.Message);
        }
    }
}