using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HouseRentingAPI.Data;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using HouseRentingAPI.Interface;
using HouseRentingAPI.Model;
using System.ComponentModel.DataAnnotations;

namespace HouseRentingAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Landlord")]
    [ApiController]
    public class HousesController : ControllerBase
    {
        private readonly HouseRentingDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHouseService _houseService;

        public HousesController(HouseRentingDbContext context, IMapper mapper, IHouseService houseService)
        {
            _context = context;
            _mapper = mapper;
            _houseService = houseService;
        }

        // GET: api/Houses
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetHouseDto>>> GetHouses()
        {
            var houses = await _houseService.GetAllAsync();
            var record = _mapper.Map<List<GetHouseDto>>(houses);
            return Ok(record);
        }

        // GET: api/Houses/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetHouseByIdDto>> GetHouseById(Guid id)
        {
            var house = await _houseService.GetAsync(id);

            if (house == null)
            {
                return NotFound();
            }

            return Ok(house);
        }

        [AllowAnonymous]
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<GetHouseDto>>> SearchHouses([FromQuery][Required] string keyword)
        {
            try
            {
                var result = await _houseService.SearchHouses(keyword);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Houses
        [HttpPost]
        public async Task<ActionResult<HouseAddDto>> CreateHouse(HouseAddDto houseAddDto)
        {
            var house = _mapper.Map<House>(houseAddDto);
            _context.Houses.Add(house);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHouseById", new { id = house.HouseID }, _mapper.Map<GetHouseByIdDto>(house));
        }

        // PUT: api/Houses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHouse(Guid id, UpdateHouseDto updateHouseDto)
        {
            if (id != updateHouseDto.HouseID)
            {
                return BadRequest("Invalid Record Id");
            }

            var house = await _houseService.GetAsync(id);

            if (house == null)
            {
                return NotFound();
            }

            _mapper.Map(updateHouseDto, house);

            try
            {
                await _houseService.UpdateAsync(house);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await HouseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { Message = "資料已更新" });
        }

        // DELETE: api/Houses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHouse(Guid id)
        {
            var house = await _houseService.GetAsync(id);
            if (house == null)
            {
                return NotFound();
            }

            await _houseService.DeleteAsync(id);

            return Ok(new { Message = "房屋資料已刪除" });
        }

        private async Task<bool> HouseExists(Guid id)
        {
            return await _houseService.Exists(id);
        }
    }
}
