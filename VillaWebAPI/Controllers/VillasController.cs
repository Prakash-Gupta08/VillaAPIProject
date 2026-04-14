using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using VillaWebAPI.Data;
using VillaWebAPI.DTO;
using VillaWebAPI.Models;

namespace VillaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillasController : ControllerBase
    {
        private readonly MyDBContext _context;
        private readonly IMapper _mapper;
        public VillasController(MyDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;   
        }

        [HttpGet("GetAllVilla")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable>>> GetAllVillas()
        {
            var data = await _context.Villa.ToListAsync();
            //return Ok(_mapper.Map<List<VillaDto>>(data));
            var dtoResponseVilla = _mapper.Map<List<VillaDto>>(data);
            var response = ApiResponse<IEnumerable<VillaDto>>.Ok(dtoResponseVilla, "Villa retrieve successfully");
            return Ok(response);
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<VillaDto>>> GetVillaById(int id)
        {
            try
            {
                if(id<=0)
                {
                    return NotFound(ApiResponse < object>.NotFound("Villa ID must be greater than 0"));
                    //return new ApiResponse<VillaDto>()
                    //{
                    //    StatusCode = 400,
                    //    Errors = "Valid ID must be greater than 0",
                    //    Success = false,
                    //    Message = "Bad Request"
                    //};
                    

                }
                var villa = await _context.Villa.FirstOrDefaultAsync(u => u.Id == id);
                if(villa == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa with ID {id} was not found"));
                }
                return Ok(ApiResponse<object>.Ok(_mapper.Map<VillaDto>(villa), "Data found successfully"));
                //return new ApiResponse<VillaDto>()
                //{
                //    StatusCode = 200,
                //    Errors = "Valid ID must be greater than 0",
                //    Success = true,
                //    Message = "Data found successfully",
                //    Data = _mapper.Map<VillaDto>(villa)
                //};
               

            }
            catch (Exception ex)
            {
                var res = ApiResponse<object>.Error(500, "An error occured while creating villa with ID : ", ex.Message);
                return StatusCode(500, res);

            }
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(ApiResponse<VillaDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<VillaDto>>> CreateVilla(VillaCreateDto villaDTO)
        {
            try
            {
                if (villaDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa data is required"));

                }
                var duplicateVilla = await _context.Villa.FirstOrDefaultAsync(s => s.Name.ToLower() == villaDTO.Name.ToLower());
                if (duplicateVilla != null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"A villa with the name '{villaDTO}' already exist"));
                }
                Villa villa = _mapper.Map<Villa>(villaDTO);

                await _context.Villa.AddAsync(villa);
                await _context.SaveChangesAsync();
                var res = ApiResponse<VillaDto>.CreatedAt(_mapper.Map<VillaDto>(villa), "Villa created successfully");
                return CreatedAtAction(nameof(CreateVilla), new { id = villa.Id }, res);

            }
            catch (Exception ex)
            {
                var res = ApiResponse<object>.Error(500, "An error occured while creating villa with ID : ", ex.Message);
                return StatusCode(500, res);
                   
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<VillaDto>>> UpdateVilla(int id, VillaUpdateDto villaDTO)
        {
            try
            {
                if (villaDTO == null)
                {
                    //return BadRequest("Villa must required");
                    return BadRequest(ApiResponse<object>.BadRequest("Villa data is required"));

                }
                if(id != villaDTO.Id)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa data is required"));
                    //return BadRequest("Villa ID in URL does not match villa ID in request body");
                }

                var existingvilla = await _context.Villa.FirstOrDefaultAsync(u => u.Id == id);
                if(existingvilla == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa with {id} was not found"));
                }
                var duplicateVilla = await _context.Villa.FirstOrDefaultAsync(s => s.Name.ToLower() == villaDTO.Name.ToLower() && s.Id != id);
                if (duplicateVilla != null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"A villa with the name '{villaDTO.Name}' already exist"));
                }
                _mapper.Map(villaDTO,existingvilla);
                existingvilla.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                var res = ApiResponse<VillaDto>.Ok(_mapper.Map<VillaDto>(villaDTO), "Villa updates successfully");
                return Ok(villaDTO);

            }
            catch (Exception ex)
            {
                var res = ApiResponse<object>.Error(500, "An error occured while creating villa with ID : ", ex.Message);
                return StatusCode(500, res);

            }
        }

    }
}
