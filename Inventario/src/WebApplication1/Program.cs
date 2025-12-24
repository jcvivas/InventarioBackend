using Api.IoC;
using Infraestructura.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Infraestructura.Extensiones;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.AddSwaggerCustom();
builder.SetCorsCustom();
builder.SetRegisterDependencies();








var jwtSection = builder.Configuration.GetSection("Jwt");
var secret = jwtSection["Secret"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; 
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!)),

            ValidateIssuer = true,
            ValidIssuer = jwtSection["Issuer"],

            ValidateAudience = true,
            ValidAudience = jwtSection["Audience"],

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SoloAdmin", p => p.RequireRole("ADMIN"));
    options.AddPolicy("SoloComprador", p => p.RequireRole("COMPRADOR"));
    options.AddPolicy("AdminOComprador", p => p.RequireRole("ADMIN", "COMPRADOR"));
});

builder.Services.AddEndpointsApiExplorer();




var app = builder.Build();
app.ConfigureExceptionHandler();



app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseHttpPipelineCustom();
app.UseCorsCustom();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
