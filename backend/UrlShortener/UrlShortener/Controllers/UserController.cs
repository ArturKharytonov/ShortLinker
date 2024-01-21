using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.BLL.Services.UserContext.Interfaces;
using UrlShortener.BLL.ViewModels;

namespace UrlShortener.Controllers
{
    [Route("user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserContext _userContext;

        public UserController(IUserContext userContext)
        {
            _userContext = userContext;
        }

        [HttpGet("claims")]
        public IActionResult GetClaims()
        {
            return Ok(new ClaimsViewModel { Id = _userContext.GetUserId()!, Role = _userContext.GetUserRole()! });
        }
    }
}
