using VillaWebAPI.DTO;

namespace VillaWebAPI.Services
{
    public interface IAuthService
    {
        Task<UserDto?>RegisterAsync(RegistrationRequestDto registrationRequestDto);
        Task<LoginRequestDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<bool> IsEmailExistsAsync(string email);
    }
}
