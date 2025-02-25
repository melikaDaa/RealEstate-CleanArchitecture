using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Application.DTOs;
using RealEstateApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Infrastructure.Services;

namespace RealEstateApp.Presentation.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstateController : ControllerBase
    {
        private readonly IEstateService _estateService;

        public EstateController(IEstateService estateService)
        {
            _estateService = estateService;
        }

        // Get all estates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstateDto>>> GetAllEstates()
        {
            var estates = await _estateService.GetAllEstatesAsync();
            return Ok(estates);
        }

        // Add a new estate
        [HttpPost]
        public async Task<ActionResult<EstateDto>> AddEstate([FromForm] EstateCreateDto estateCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newEstate = await _estateService.AddEstateAsync(estateCreateDto);
            return CreatedAtAction(nameof(GetAllEstates), new { id = newEstate.Id }, newEstate);
        }

        // Update an existing estate
        [HttpPut("{id}")]
        public async Task<ActionResult<EstateDto>> UpdateEstate(int id, [FromForm] EstateCreateDto estateUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedEstate = await _estateService.UpdateEstateAsync(id, estateUpdateDto);
                return Ok(updatedEstate); // Return updated EstateDto
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Estate not found" });
            }
        }
        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<EstateDto>>> SearchEstates([FromBody] SearchDto searchDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var estates = await _estateService.SearchEstatesAsync(searchDto);
                return Ok(estates);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        // Delete an estate
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstate(int id)
        {
            try
            {
                await _estateService.DeleteEstateAsync(id);
                return NoContent(); // 204 No Content indicates successful deletion
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Estate not found" });
            }
        }
    }
}
