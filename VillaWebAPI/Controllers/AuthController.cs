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

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
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


    }
}
