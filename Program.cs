using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MovieBackend.Configurations;
using MovieBackend.Contexts;
using MovieBackend.Interaces;
using MovieBackend.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();


// Add JWT configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// Configure authentication with JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero
    };
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Google:CLIENT_ID"];
    options.ClientSecret = builder.Configuration["Google:CLIENT_SECRET"];
    options.CallbackPath = "/api/account/callback";
    options.SaveTokens = true; // Important to save tokens for later use
    
    // Map additional claims from Google
    options.Scope.Add("profile");
    options.ClaimActions.MapJsonKey("picture", "picture", "url");
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

app.UseAuthentication();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
