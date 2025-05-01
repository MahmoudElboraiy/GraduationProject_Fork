using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Auth;
using IdentityManager.Services.ControllerService.IControllerService;
using System.Threading.Tasks;

namespace IdentityManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthUserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthUserController(IAuthService authService)
        {
            _authService = authService;
        }

        // Login
        [HttpPost("AuthenticateUser")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var result = await _authService.LoginAsync(loginRequestDTO);
            return Ok(result);
        }

        // Register
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var result = await _authService.RegisterAsync(registerRequestDTO);
            return Ok(result);
        }

        // Forgot-Password
        [HttpPost("RequestPasswordReset")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDTO forgotPasswordRequestDTO)
        {
            var result = await _authService.ForgotPasswordAsync(forgotPasswordRequestDTO);
            return Ok(new { message = "Token generated", token = result });
        }

        // Reset-Password
        [HttpPost("ResetUserPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            var result = await _authService.ResetPasswordAsync(resetPasswordDTO);
            return Ok(new { message = result });
        }
    }
}