using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using VillaWebAPI.Data;
using VillaWebAPI.DTO;
using VillaWebAPI.Models;

namespace VillaWebAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly MyDBContext _context;
        private readonly IMapper _mapper;
        
        public AuthService(MyDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(s => s.Email.ToLower()== email.ToLower());
        }

        public async Task<LoginRequestDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(s => s.Email.ToLower() == loginRequestDto.Email.ToLower());
                if (user == null || user.Password != loginRequestDto.Password)
                {
                    return null;
                }
                //generate token
                return new LoginResponseDto
                {
                    UserDto = _mapper.Map<UserDto>(user),
                    Token = ""
                };
               
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("An unexpected error occured during user registration", ex);
            }
        }

        public async Task<UserDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {

            try
            {
                if (await IsEmailExistsAsync(registrationRequestDto.Email))
                {
                    return null;
                }
                User user = new()
                {
                    Email = registrationRequestDto.Email,
                    Name = registrationRequestDto.Name,
                    Password = registrationRequestDto.Password,
                    Role = string.IsNullOrEmpty(registrationRequestDto.Role) ? "Customer" : registrationRequestDto.Role,
                    CreatedDate = DateTime.Now,
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("An unexpected error occured during user registration", ex);
            }
        }
    }
}
