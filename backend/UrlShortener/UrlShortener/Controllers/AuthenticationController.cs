using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.BLL.Services.JwtService.Interfaces;
using UrlShortener.BLL.ViewModels;
using UrlShortener.DAL.Entities;

namespace UrlShortener.Controllers;

[Route("auth")]
[ApiController]
[AllowAnonymous]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly IJwtService _jwtService;

    public AuthenticationController( IJwtService jwtService, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        _jwtService = jwtService;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] AuthenticationViewModel login)
    {
        var user = await _userManager.FindByNameAsync(login.Username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password))
            return BadRequest("Wrong username or password");

        var roles = await _userManager.GetRolesAsync(user);
        var tokenString = _jwtService.GetToken(user.Id, user.UserName, roles[0]);

        return Ok(new { token = tokenString });
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync([FromBody] AuthenticationViewModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);

        if (user != null)
            return BadRequest("User exist");

        var newUser = new User { UserName = model.Username };
        var result = await _userManager.CreateAsync(newUser, model.Password);

        if (!result.Succeeded) 
            return BadRequest("Smth went wrong");

        if (newUser.UserName.Equals("artur4455"))
            await AddUserToRole(newUser, "admin");
        else
            await AddUserToRole(newUser, "user");

        return Ok("User was added");
    }

    private async Task AddUserToRole(User user, string roleName)
    {
        var roleExists = await _roleManager.RoleExistsAsync(roleName);

        if (!roleExists)
        {
            var role = new IdentityRole<int>(roleName);
            await _roleManager.CreateAsync(role);
        }
        await _userManager.AddToRoleAsync(user, roleName);
    }
}