using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VillaWebAPI.Data;
using VillaWebAPI.DTO;
using VillaWebAPI.Models;

namespace VillaWebAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly MyDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        
        public AuthService(MyDBContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }


        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(s => s.Email.ToLower()== email.ToLower());
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            try
            {
                var email = loginRequestDto.Email?.ToLower();
                var user = await _context.Users.FirstOrDefaultAsync(s => s.Email.ToLower() == loginRequestDto.Email.ToLower());
                if (user == null || user.Password != loginRequestDto.Password)
                {
                    return null;
                }
                //generate token
                var token = GenerateJwtToken(user);
                return new LoginResponseDto
                {
                    UserDto = _mapper.Map<UserDto>(user),
                    Token = token,
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

        private string GenerateJwtToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtSettings")["Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token); 

        }
    }
}
