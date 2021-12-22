using System;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ServicesInterfaces.Companies;
using ServicesInterfaces.Users;
using System.Threading.Tasks;
using ConstData;
using TestBaseDto;

namespace TestNHibernate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRequestClient<IUsersData> _usersRequestClient;
        private readonly IRequestClient<IUserPrimaryData> _userRequestClient;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public UserController(IUserService userService,
            IRequestClient<IUsersData> usersRequestClient,
            IRequestClient<IUserPrimaryData> userRequestClient,
            ISendEndpointProvider sendEndpointProvider)
        {
            _userService = userService;
            _usersRequestClient = usersRequestClient;
            _userRequestClient = userRequestClient;
            _sendEndpointProvider = sendEndpointProvider;
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
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{ConstStringForQueues.UpdateUser}"));

            await endpoint.Send<IUserData>(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.CompanyId
            });

            return Ok();
        }

        [HttpPut("CreateUser")]
        public async Task<IActionResult> CreateUser(UserDto user)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{ConstStringForQueues.CreateUser}"));

            await endpoint.Send<IUserData>(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.CompanyId
            });

            return Ok();
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(UserDto user)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{ConstStringForQueues.DeleteUser}"));

            await endpoint.Send<IUserPrimaryData>(new{user.Id});

            return Ok();
        }
    }
}