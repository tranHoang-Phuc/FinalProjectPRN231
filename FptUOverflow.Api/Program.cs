using CloudinaryDotNet;
using FptUOverflow.Api.Mapper;
using FptUOverflow.Api.Services;
using FptUOverflow.Api.Services.IServices;
using FptUOverflow.Core.CoreObjects;
using FptUOverflow.Core.Exceptions;
using FptUOverflow.Infra.EfCore.DataAccess;
using FptUOverflow.Infra.EfCore.Models;
using FptUOverflow.Infra.EfCore.Repositories;
using FptUOverflow.Infra.EfCore.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                
            }, ServiceLifetime.Singleton);

            #region Mapper
            builder.Services.AddAutoMapper(typeof(ProfileMapper));
            #endregion

            #region Configure Options
            builder.Services.Configure<JwtOptions>(
                builder.Configuration.GetSection("ApiSetting:JwtOptions")
                );

            builder.Services.Configure<CloudinarySettings>(
               builder.Configuration.GetSection("Cloudinary")
               );
                
            builder.Services.Configure<OAuth2>(
                builder.Configuration.GetSection("OAuth2")
                );
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<AppDbContext>()
               .AddDefaultTokenProviders();

            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #endregion

            #region Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
               options =>
               {
                   options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = builder.Configuration["ApiSetting:JwtOptions:Issuer"],
                       ValidAudience = builder.Configuration["ApiSetting:JwtOptions:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                           .GetBytes(builder.Configuration["ApiSetting:JwtOptions:Secret"]))
                   };

                   options.Events = new JwtBearerEvents
                   {
                       OnAuthenticationFailed = async context =>
                       {
                           if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                           {
                               throw new AppException(ErrorCode.Unauthorized);
                           }
                       },

                   };
               });
            #endregion

            #region Cors Policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    policy => policy.WithOrigins("http://localhost:3000")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                    );
            });
            #endregion

            #region Repositories
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
            builder.Services.AddScoped<IAnswerVoteRepository, AnswerVoteRepository>();
            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
            builder.Services.AddScoped<IQuestionVoteRepository, QuestionVoteRepository>();
            builder.Services.AddScoped<IQuestionTagRepository, QuestionTagRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<ITagUserRepository, TagUserRepository>();
            builder.Services.AddScoped<ICloudinaryRepository, CloudinaryRepository>();
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            builder.Services.AddScoped<IImageUploadRepository, ImageUploadRepository>();
            #endregion

            #region Services
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IQuestionService, QuestionService>();
            builder.Services.AddScoped<IAnswerService, AnswerService>();
            #endregion

            #region Helpers
            builder.Services.AddSingleton(provider =>
            {
                var settings = provider.GetRequiredService<IOptions<CloudinarySettings>>().Value;
                return new CloudinaryDotNet.Cloudinary(new Account(
                    settings.CloudName,
                    settings.ApiKey,
                    settings.ApiSecret
                ));
            });
            #endregion

            var app = builder.Build();

            #region GlocalExceptionHandler
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var exceptionHandler = app.Services.GetRequiredService<ILogger<GlobalExceptionHandler>>();
                    var handler = new GlobalExceptionHandler(exceptionHandler);
                    await handler.InvokeAsync(context);
                });
            });
            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            await AddDefault(app);
            app.Run();
        }

        private static async Task AddDefault(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    if(!roleManager.Roles.Any())
                    {
                        await roleManager.CreateAsync(new IdentityRole("USER"));
                    }
                }
                catch (Exception ex)
                {
                    throw new AppException(ErrorCode.UncategorizedException);
                }
            }
        }
    }
}
