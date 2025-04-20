using Models.DTOs.Auth;
using System.Threading.Tasks;

namespace IdentityManager.Services.ControllerService.IControllerService
{
    public interface IAuthService
    {
        Task<object> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<object> RegisterAsync(RegisterRequestDTO registerRequestDTO);
        Task<string> ForgotPasswordAsync(ForgotPasswordRequestDTO forgotPasswordRequestDTO);
        Task<string> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
    }
}