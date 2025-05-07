
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using server.BL;
using server.BL.Interface;
using server.DAL;
using server.DAL.Interface;
using server.MiddleWere;
using server.Models.DTO;
using System.Text;
namespace server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IStatusDal, StatusDal>();

            builder.Services.AddScoped<IGiftDal, GiftDal>();
            builder.Services.AddScoped<IGiftService, GiftService>();

            builder.Services.AddScoped<IDonorDal, DonorDal>();
            builder.Services.AddScoped<IDonorService, DonorService>();

            builder.Services.AddScoped<ICategoryDal, CategoryDal>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            builder.Services.AddScoped<IAuthDal, AuthDal>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<IMannagerDal, MannagerDal>();
            builder.Services.AddScoped<IManageService, ManageService>();

            builder.Services.AddScoped<ITicketDalMannager, TicketDalMannager>();
            builder.Services.AddScoped<ITicketManagerService, TicketMannagerService>();

            builder.Services.AddScoped<ITicketDalUser, TicketDalUser>();
            builder.Services.AddScoped<ITicketUserService, TicketUserService>();


            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            var key = Encoding.ASCII.GetBytes("ph/MB07NAZWbwLwYb/kCzDWJHIHMJra/KZ1+nrTRym4="); 

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false 
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireSetStatus", policy =>
                    policy.Requirements.Add(new SystemStatusRequirement(SystemStatus.SET)));
                options.AddPolicy("RequireOnStatus", policy =>
                    policy.Requirements.Add(new SystemStatusRequirement(SystemStatus.ON)));
                options.AddPolicy("RequireOffStatus", policy =>
                    policy.Requirements.Add(new SystemStatusRequirement(SystemStatus.OFF)));
                options.AddPolicy("ManagerOnly", policy => policy.RequireRole("MANAGER"));
                options.AddPolicy("ManagerOnly1", policy => policy.RequireClaim("role", "MANAGER"));

            });

            builder.Services.AddScoped<IAuthorizationHandler, SystemStatusHandler>();

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CustomCorsPolicy",
                    builder => builder.WithOrigins("http://localhost:4200", "http://localhost:4201")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            builder.Services.AddControllers();
            builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    );
            builder.Services.AddDbContext<PDbContext>(option => option.UseSqlServer("Server=DESKTOP-DFD7ELQ;Database=Project;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;"));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }, new string[] {} }
    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("CustomCorsPolicy");

            app.UseHttpsRedirection();

            app.UseLoggerMiddlere();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
