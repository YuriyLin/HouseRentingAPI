﻿using HotelListing.API.Constract;
using HouseRentingAPI.Data;
using HouseRentingAPI.Model;

namespace HouseRentingAPI.Interface
{
    public interface IHouseService:IGenericRepository<House>
    {
        Task<List<GetHouseDto>> GetAllHouses();
        Task<List<House>> SearchHouses(string keyword);
        Task<CommentDto> AddCommentAsync(Guid houseId, string content, Guid userId);
        Task<List<Comment>> GetCommentsByHouseIdAsync(Guid houseId);
        Task UpdateCommentAsync(Guid commentId, string comment);
        Task DeleteCommentAsync(Guid commentId);
    }
}
