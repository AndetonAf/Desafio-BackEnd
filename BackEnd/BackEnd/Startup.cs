using BackEnd.Configurations;
using BackEnd.Services.Interfaces;
using Data;
using Google.Apis.Auth.OAuth2;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BackEnd
{
    public class Startup(IConfiguration configuration)
    {

        private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        public void ConfigureServices(IServiceCollection services)
        {
            var settings = new ConfigurationBuilder()
                .AddConfiguration(_configuration)
                .AddJsonFile("config/appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddSubstitution()
                .Build()
                .Get<Settings>() ?? throw new InvalidOperationException("Configuração não encontrada.");

            GlobalConfigurations.Initialize(settings);

            string credential_path_gcp = GlobalConfigurations.Settings.Gcp.PathRelative ? AppDomain.CurrentDomain.BaseDirectory + GlobalConfigurations.Settings.Gcp.PathJson : GlobalConfigurations.Settings.Gcp.PathJson;

            if (File.Exists(credential_path_gcp))
            {
                System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path_gcp);
            }

            services.AddControllers().AddJsonOptions(options => {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = @"JWT Authorization header using the Bearer scheme, Example: ""Bearer eyJhbGciOiJIU...""",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                         Array.Empty<string>()
                    }
                });
            });

            services.AddHttpContextAccessor();

            services.AddDbContext<Context>(options =>
            {
                options.UseNpgsql(settings.ConnectionStrings.Db, options =>
                {
                    options.CommandTimeout(300);
                }
             );
                #if DEBUG 
                    options.EnableSensitiveDataLogging();
                #endif
            });

            services.AddHttpContextAccessor();
            services.AddTransient<IMessagingService, Services.MessagingService>();
            services.AddTransient<IStorageService, Services.StorageService>();
            services.AddTransient<IMotorcycleService, Services.MotorcycleService>();
            services.AddTransient<IUserService, Services.UserService>();
            services.AddTransient<IRentalService, Services.RentalService>();
            services.AddTransient<IFacadeService, Services.FacadeService>();

            services.AddHealthChecks();

            services.AddHostedService<PubSubBackgroundService>();

            SetAuthentication(ref services, settings);

            services.AddCors();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true));

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { 
                endpoints.MapHealthChecks("/health", new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
                endpoints.MapGet("/", () => JsonSerializer.Serialize(GlobalConfigurations.Settings));
                endpoints.MapControllers();
            });
        }

        public static void SetAuthentication(ref IServiceCollection services, Settings settings)
        {
            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, config =>
            {
                var secrectBytes = Encoding.ASCII.GetBytes(settings.Auth.SecretJwt);
                var key = new SymmetricSecurityKey(secrectBytes);
                config.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = key,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
