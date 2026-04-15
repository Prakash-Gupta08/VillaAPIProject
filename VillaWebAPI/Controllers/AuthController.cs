using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using VillaWebAPI.DTO;
using VillaWebAPI.Services;

namespace VillaWebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService; 
        }

        [HttpPost("Register")]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody]RegistrationRequestDto registrationRequestDto)
        {
            try
            {

                if (registrationRequestDto == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Registration data is required"));
                }
                if (await _authService.IsEmailExistsAsync(registrationRequestDto.Email))
                {
                    return Conflict(ApiResponse<object>.Conflict($"User with email '{registrationRequestDto.Email}', already exists"));
                }
                var user = await _authService.RegisterAsync(registrationRequestDto);
                if (user == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa data is required.."));
                }
                // auth service
                var response = ApiResponse<UserDto>.Ok(user, "User created successfully");
                return CreatedAtAction(nameof(Register), response);
            }

             catch (Exception ex)
            {
                var res = ApiResponse<object>.Error(500, "An error occured during registration : ", ex.Message);
                return StatusCode(500, res);

            }
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LoginResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            try
            {

                if (loginRequestDto == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Login data is required"));
                }
               
                var loginResponse = await _authService.LoginAsync(loginRequestDto);
                if (loginResponse == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Login failed"));
                }
                // auth service
                var response = ApiResponse<LoginResponseDto>.Ok(loginResponse, "Login successfully");
                return Ok(response);
            }

            catch (Exception ex)
            {
                var res = ApiResponse<object>.Error(500, "An error occured during registration : ", ex.Message);
                return StatusCode(500, res);

            }
        }


    }
}
