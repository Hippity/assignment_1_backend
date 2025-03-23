using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using MovieBackend.Configurations;
using MovieBackend.Contexts;
using MovieBackend.Interaces;
using MovieBackend.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

//AUTH
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options => 
{
    options.Cookie.SameSite = SameSiteMode.Lax; // Important for cross-site redirects
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Google:CLIENT_ID"];
    options.ClientSecret = builder.Configuration["Google:CLIENT_SECRET"];
    options.CallbackPath = "/api/account/GoogleResponse";
});
builder.Services.AddScoped<IAccountService, AccountService>();


// MEMORY CACHE
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IMemoryCacheService, MemoryCacheService>();

// TMDB REST SERVICE
builder.Services.Configure<TmdbSettings>(builder.Configuration.GetSection("TMDB"));

builder.Services.AddScoped<ITmdbService, TmdbService>();

// MY MOVIES DATABASE SERVICE
builder.Services.AddDbContext<MyMovieContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLITE") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

// ALLOW ANY ORIGIN TO REMOVE CORS ERROR  
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
});


var app = builder.Build();

// CREATE SWAGGER INTERFACE
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.UseCors("AllowAnyOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
