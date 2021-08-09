using AuthenticationService.Models;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        

        private readonly ILogger<UserController> _logger;
        private readonly IJwtService _jwtService;

        public UserController(ILogger<UserController> logger, IJwtService jwtService)
        {
            _logger = logger;
            _jwtService = jwtService;
        }

        [HttpPost]
        public string GetToken(User user)
        {
            return _jwtService.GenerateToken(user);
        }

        [HttpPost]
        public bool AuthorizeRequest(AuthorizeRequest request)
        {
            return true;
        }
    }
}
