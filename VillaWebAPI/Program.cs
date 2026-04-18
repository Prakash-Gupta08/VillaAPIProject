using Microsoft.EntityFrameworkCore;
using RoyalVilla.DTO;
using VillaWebAPI.Controllers;
using VillaWebAPI.Data;
using VillaWebAPI.DTO;
using VillaWebAPI.Models;
using VillaWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext (IMPORTANT: configure connection string)
builder.Services.AddDbContext<MyDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("MySqlConn")
    )
);
//builder.Services.AddAutoMapper(typeof(Program));
//builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddAutoMapper(o =>
{
    o.CreateMap<Villa, VillaCreateDto>().ReverseMap();
    o.CreateMap<Villa, VillaUpdateDto>().ReverseMap(); // Reverse map is used for the automapping with the data or model classes
    o.CreateMap<Villa, VillaDto>().ReverseMap();
    o.CreateMap<VillaUpdateDto, VillaDto>().ReverseMap();
    o.CreateMap<User, UserDto>().ReverseMap();
    o.CreateMap<VillaAmenities, VillaAmenitiesCreateDTO>().ReverseMap();
    o.CreateMap<VillaAmenities, VillaAmenitiesUpdateDTO>().ReverseMap();
    o.CreateMap<VillaAmenities, VillaAmenitiesDTO>()
    .ForMember(dest => dest.VillaName, opt => opt.MapFrom(src =>src.Villa!=null? src.Villa.Name : null));
    o.CreateMap<VillaAmenitiesDTO, VillaAmenities>();

});
builder.Services.AddScoped<IAuthService, AuthService>();
var app = builder.Build();

//migration
await SeedDataAsync(app);

// Enable Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student API v1");
    });
}
app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("*"));
app.UseHttpsRedirection();
app.UseAuthorization();
//app.UseAuthorization(); 
app.MapControllers();
app.Run();

static async Task SeedDataAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MyDBContext>();
    await context.Database.MigrateAsync();
}