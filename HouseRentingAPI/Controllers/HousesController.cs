﻿using System;
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
        private readonly ICommentService _commentService;

        public HousesController(HouseRentingDbContext context, IMapper mapper, IHouseService houseService, ICommentService commentService)
        {
            this._context = context;
            this._mapper = mapper;
            this._houseService = houseService;
            this._commentService = commentService;
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

        // GET: api/Houses/{id}
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

        // House Searching
        // Get:api/Houses/Search/{keyword}  
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
            await _houseService.AddAsync(house);

            return CreatedAtAction("GetHouseById", new { id = house.HouseID }, _mapper.Map<GetHouseByIdDto>(house));
        }

        // PUT: api/Houses/{id}
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

        // DELETE: api/Houses/{id}
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


        //-----------Comment-------------------(Add/Update/Delete)
        // POST: api/Houses/{houseId}/comments
        [HttpPost("{houseId}/comments")]
        public async Task<IActionResult> AddComment(Guid houseId, [FromBody] string content, [FromRoute] Guid userId)
        {
            try
            {
                await _houseService.AddCommentAsync(houseId, content, userId);
                return Ok(new { Message = "Comment added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Failed to add comment.", Details = ex.Message });
            }
        }

        // PUT: api/Houses/{houseId}/comments/{commentId}
        [HttpPut("{houseId}/comments/{commentId}")]
        public async Task<IActionResult> UpdateComment(Guid houseId, Guid commentId, [FromBody] Comment comment)
        {
            try
            {
                if (commentId != comment.CommentId || houseId != comment.HouseId)
                    return BadRequest("Invalid comment ID or house ID.");

                await _houseService.UpdateCommentAsync(comment);
                return Ok(new { Message = "Comment updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Failed to update comment.", Details = ex.Message });
            }
        }

        // DELETE: api/Houses/{houseId}/comments/{commentId}
        [HttpDelete("{houseId}/comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid houseId, Guid commentId)
        {
            try
            {
                await _houseService.DeleteCommentAsync(commentId);
                return Ok(new { Message = "Comment deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Failed to delete comment.", Details = ex.Message });
            }
        }

        //---------------------------------------------------------
    }
}