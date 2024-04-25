using Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repo.AuthModels;
using Repo.Helpers;

namespace APITask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthservice _Auth;
        private readonly SignInManager<Users> _signInManager;

                
        public AuthController(IAuthservice Auth, SignInManager<Users> signInManager)
        {
            _Auth = Auth;
            _signInManager = signInManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] Register Reg)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _Auth.Registerasync(Reg);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoles ARL)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _Auth.AddRoleAsync(ARL);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(ARL);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Loginasync([FromBody] Login log)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _Auth.Loginasync(log);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            var SO = _signInManager.SignOutAsync();
            return Ok(SO);
        }
    }
}
