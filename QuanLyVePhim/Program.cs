
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuanLyVePhim.Controllers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


ConfigurationManager configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(op =>
{
    op.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
    op.DefaultScheme=JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(ops =>
{
    ops.SaveToken = true;
    ops.RequireHttpsMetadata = false;
    ops.TokenValidationParameters = new
        Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:ValidAudience"],
        ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))

    };
});
builder.Services.AddDbContext<ApplicationDbContext>(op => op.UseSqlServer("name:ConnectionStrings:Connect"));



var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    });

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
