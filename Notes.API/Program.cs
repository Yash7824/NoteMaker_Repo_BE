
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Notes.API.DL;
using Notes.API.Repositories;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using Notes.API.BL;

namespace Notes.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                var issuer = builder.Configuration["AppSettings:Jwt:Issuer"];
                var audience = builder.Configuration["AppSettings:Jwt:Audience"];
                var key = builder.Configuration["AppSettings:Jwt:Key"];

                if (issuer != null && audience != null && key != null)
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };
                }
                else
                {
                    throw new ApplicationException("JWT configuration values are missing.");
                }
            });



            builder.Services.AddScoped<INoteRepository, NotesDL>();
            builder.Services.AddScoped<IUserRepository, UsersDL>();
            builder.Services.AddScoped<ITokenRepository, LoginAuthBL>();

            // builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}