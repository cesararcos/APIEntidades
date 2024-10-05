using APIEntidades.Application.Contracts;
using APIEntidades.Application;
using APIEntidades.Domain.Dto;
using APIEntidades.Utilities.Validators;
using APIEntidades.Utilities;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using APIEntidades.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agregar el servicio de memoria caché
builder.Services.AddMemoryCache();

// Añadir el servicio de autenticación JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]!)) // Clave secreta para firmar los tokens
    };
});

// Conexión BBDD
builder.Services.AddDbContext<EntidadesDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("database")));

// Inyección Dependencia
builder.Services.AddTransient<IUsuarioAppService, UsuarioAppService>();
builder.Services.AddTransient<IVideoJuegosAppService, VideoJuegosAppService>();

// Inyección de JWT
builder.Services.AddTransient<IUtilities, Utilities>();

// Inyección de validador
builder.Services.AddTransient<IValidator<UsuarioDto>, UsuarioValidator>();
builder.Services.AddTransient<IValidator<IngresoDto>, IngresoValidator>();

var app = builder.Build();

app.UseAuthentication(); // Middleware para autenticar usuarios
app.UseAuthorization();  // Middleware para verificar las autorizaciones

app.UseCors(options =>
{
    //options.WithOrigins("http://localhost:3000");
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
