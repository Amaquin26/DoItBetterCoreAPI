
using DoItBetterCoreAPI.Extensions;
using DoItBetterCoreAPI.Middlewares;
using DoItBetterCoreAPI.Models;
using DoItBetterCoreAPI.Repositories;
using DoItBetterCoreAPI.Services;
using Microsoft.OpenApi.Models;

namespace DoItBetterCoreAPI
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
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Fill in the JWT token",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<String>()
                    }
                });
            });

            builder.Services.AddHttpContextAccessor();

            // Services
            builder.Services.AddScoped<ITodoTaskRepository, TodoTaskRepository>();
            builder.Services.AddScoped<ITodoTaskService, TodoTaskService>();
            builder.Services.AddScoped<ITodoSubtaskRepository, TodoSubtaskRepository>();
            builder.Services.AddScoped<ITodoSubtaskService, TodoSubtaskService>();

            // Services from Identity Core
            builder.Services
                .AddIdentityHandlersAndStores()
                .ConfigureIdentityOptions()
                .InjectDbContext(builder.Configuration)
                .AddIdentityAuth(builder.Configuration);

            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

            var app = builder.Build();

            // Middlewares
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            #region Config.CORS
            app.UseCors(options =>
                options.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            #endregion

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            app
                .MapGroup("/api")
                .MapIdentityApi<AppUser>();

            app.Run();

        }
    }
}
