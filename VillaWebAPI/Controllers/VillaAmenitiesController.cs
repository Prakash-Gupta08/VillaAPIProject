using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla.DTO;
using System.Collections;
using VillaWebAPI.Data;
using VillaWebAPI.DTO;
using VillaWebAPI.Models;

namespace VillaWebAPI.Controllers
{
    [Route("api/villa-amenities")]
    [ApiController]
    public class VillaAmenitiesController : ControllerBase
    {
        private readonly MyDBContext _context;
        private readonly IMapper _mapper; 
        public VillaAmenitiesController(MyDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetAllVilla")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaAmenitiesDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable>>> GetAllVillaAmenities()
        {
            var data = await _context.VillaAmenities.ToListAsync();
            //return Ok(_mapper.Map<List<VillaAmenitiesDTO>>(data));
            var dtoResponseVilla = _mapper.Map<List<VillaAmenitiesDTO>>(data);
            var response = ApiResponse<IEnumerable<VillaAmenitiesDTO>>.Ok(dtoResponseVilla, "VillaAmenities retrieve successfully");
            return Ok(response);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVillaAmenitiesById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound(ApiResponse<object>.NotFound("VillaAmenities ID must be greater than 0"));
                    //return new ApiResponse<VillaAmenitiesDTO>()
                    //{
                    //    StatusCode = 400,
                    //    Errors = "Valid ID must be greater than 0",
                    //    Success = false,
                    //    Message = "Bad Request"
                    //};


                }
                var villaAmenities = await _context.VillaAmenities.FirstOrDefaultAsync(u => u.Id == id);
                if (villaAmenities == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"VillaAmenities with ID {id} was not found"));
                }
                return Ok(ApiResponse<object>.Ok(_mapper.Map<VillaAmenitiesDTO>(villaAmenities), "Data found successfully"));

            }
            catch (Exception ex)
            {
                var res = ApiResponse<object>.Error(500, "An error occured while creating villa with ID : ", ex.Message);
                return StatusCode(500, res);

            }
        }
        //[HttpGet("{id:int}")]
        //[ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetVillaAmenitiesById(int id)
        //{
        //    try
        //    {
        //        if (id <= 0)
        //        {
        //            return NotFound(ApiResponse<VillaAmenitiesDTO>
        //                .NotFound("VillaAmenities ID must be greater than 0"));
        //        }

        //        var villaAmenities = await _context.VillaAmenities
        //            .FirstOrDefaultAsync(u => u.Id == id);

        //        if (villaAmenities == null)
        //        {
        //            return NotFound(ApiResponse<VillaAmenitiesDTO>
        //                .NotFound($"VillaAmenities with ID {id} was not found"));
        //        }

        //        var result = _mapper.Map<VillaAmenitiesDTO>(villaAmenities);

        //        return Ok(ApiResponse<VillaAmenitiesDTO>
        //            .Ok(result, "Data found successfully"));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500,
        //            ApiResponse<VillaAmenitiesDTO>.Error(
        //                500,
        //                "An error occurred while fetching villa amenities",
        //                new List<string> { ex.Message }
        //            ));
        //    }
        //}


        [HttpPost("Create")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateVillaAmenities(VillaAmenitiesCreateDTO villaAmentiesDTO)
        {
            try
            {
                if (villaAmentiesDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("VillaAmenities data is required"));

                }
                var duplicateVilla = await _context.VillaAmenities.FirstOrDefaultAsync(s => s.Id == villaAmentiesDTO.VillaId);
                if (duplicateVilla != null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"A villa with the ID '{villaAmentiesDTO.VillaId}' does not exist"));
                }
                VillaAmenities villaAmenities = _mapper.Map<VillaAmenities>(villaAmentiesDTO);
                villaAmenities.CreatedDate = DateTime.Now;
                
                await _context.VillaAmenities.AddAsync(villaAmenities);
                await _context.SaveChangesAsync();
                var res = ApiResponse<VillaAmenitiesDTO>.CreatedAt(_mapper.Map<VillaAmenitiesDTO>(villaAmenities), "VillaAmenities created successfully");
                return CreatedAtAction(nameof(CreateVillaAmenities), new { id = villaAmenities.Id }, res);

            }
            catch (Exception ex)
            {
                var res = ApiResponse<object>.Error(500, "An error occured while creating villa with ID : ", ex.Message);
                return StatusCode(500, res);

            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, VillaAmenitiesUpdateDTO villaAmenitiesDTO)
        {
            try
            {
                if (villaAmenitiesDTO == null)
                {
                   
                    return BadRequest(ApiResponse<object>.BadRequest("Villa Amenities data is required"));

                }
                if (id != villaAmenitiesDTO.Id)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("VillaAmenities ID in URL does not match VillaAmenities ID in request body"));
                  
                }

                var existingvilla = await _context.Villa.FirstOrDefaultAsync(u => u.Id == villaAmenitiesDTO.VillaId);
                if (existingvilla == null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"Villa Amenities with ID {villaAmenitiesDTO.VillaId} does not exist"));
                }

                var existingVillaAmenities = await _context.VillaAmenities.FirstOrDefaultAsync(s => s.Id == id);
                if (existingVillaAmenities == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa Amenities with ID {id} was not found"));
                }
                _mapper.Map(villaAmenitiesDTO, existingVillaAmenities);
                existingVillaAmenities.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                var res = ApiResponse<VillaAmenitiesDTO>.Ok(_mapper.Map<VillaAmenitiesDTO>(villaAmenitiesDTO), "VillaAmenities updates successfully");
                return Ok(villaAmenitiesDTO);



            }
            catch (Exception ex)
            {
                var res = ApiResponse<object>.Error(500, "An error occured while creating villa amenities : ", ex.Message);
                return StatusCode(500, res);

            }
        }


    }
}
