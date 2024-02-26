using HotelListing.API.Constract;
using HouseRentingAPI.Configuration;
using HouseRentingAPI.Constract;
using HouseRentingAPI.Controllers;
using HouseRentingAPI.Data;
using HouseRentingAPI.Interface;
using HouseRentingAPI.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("HouseRentingDbString");
builder.Services.AddDbContext<HouseRentingDbContext>(options => { options.UseSqlServer(connectionString); });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCorsPolicy", builder =>
    {
        builder.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

//Authorizaion
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("LandlordPolicy", policy =>
    {
        policy.RequireRole("Landlord");
    });
});

builder.Services.AddAutoMapper(typeof(MapperConfig));
builder.Services.AddScoped<UsersController>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericService<>));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<LandlordsController>();
builder.Services.AddScoped<ILandlordService, LandLordService>();
builder.Services.AddScoped<HousesController>();
builder.Services.AddScoped<IHouseService, HouseService>();
builder.Services.AddScoped<FavoritesController>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<ICommentService, CommentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("DevCorsPolicy");

app.MapControllers();

app.Run();