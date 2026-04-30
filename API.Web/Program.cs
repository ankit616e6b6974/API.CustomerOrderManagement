using API.Web.Common;
using API.Web.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));

            //var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy", policy =>
            //    {
            //        policy.WithOrigins(allowedOrigins!)
            //              .AllowAnyHeader()
            //              .AllowAnyMethod();
            //    });
            //});

            // 1. Add CORS services
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            //MediatR
            builder.Services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Order Maintainer API",
                    Version = "v1" // Good practice to include the version string here too
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseCors("CorsPolicy");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

    }


}
