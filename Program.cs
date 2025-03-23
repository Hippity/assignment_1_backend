using Microsoft.EntityFrameworkCore;
using MovieBackend.Configurations;
using MovieBackend.Contexts;
using MovieBackend.Interaces;
using MovieBackend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Register services
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IMemoryCacheService, MemoryCacheService>();
builder.Services.Configure<TmdbSettings>(builder.Configuration.GetSection("TMDB"));
builder.Services.AddScoped<ITmdbService, TmdbService>();
builder.Services.AddScoped<IMyMoviesService, MyMoviesService>();

// Database context
builder.Services.AddDbContext<MyMovieContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLITE") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options => { options.DocumentPath = "/openapi/v1.json"; });
}

app.UseRouting();
app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();