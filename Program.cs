using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Student_GPA.Models;
using Student_GPA.Repositories;
using Student_GPA.Services;
using System.Text.Json.Serialization;

namespace Student_GPA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddControllersWithViews();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            builder.Services.AddOpenApi();


            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });


            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IStudentService, StudentService>();

            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<ICourseService, CourseService>();

            builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
            builder.Services.AddScoped<ISemesterService, SemesterService>();

            builder.Services.AddScoped<IStudentCourseRepository, StudentCourseRepository>();
            builder.Services.AddScoped<IStudentCourseService, StudentCourseService>();



            var app = builder.Build();

            app.UseCors("AllowAll");

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();

                app.MapGet("/", async context =>
                {
                    context.Response.Redirect("/scalar/v1");
                    await Task.CompletedTask;
                });
            }

            app.UseAuthentication();
            app.UseAuthorization();




            app.MapControllers();

            app.Run();
        }
    }
}
