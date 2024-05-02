namespace HouseRentingAPI.Model;
using System.Collections.Generic;

public class PagedList<T>
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }

    public PagedList(List<T> items, int totalCount, int pageNumber, int pageSize, int totalPages)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = totalPages;
    }
}