namespace shareme_backend.Startup;

using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using shareme_backend.Data;
using shareme_backend.Utils;

public static partial class ServiceInitializer
{
    private static IConfiguration _configuration { get; set; }

    private static string _policyName = "CorsPolicy";

    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        _configuration = configuration;

        RegisterEnv();
        RegisterSqlServer(services);
        RegisterControllers(services);
        RegisterSwagger(services);
        RegisterCustomDependencies(services);
        RegisterCors(services);
        RegisterAuthentication(services);
        RegisterAutoMapper(services);

        return services;
    }

    private static void RegisterSqlServer(IServiceCollection services)
    {
        var connectionString = _configuration["ConnectionStrings:SharemeDb"];
        if (connectionString is null)
        {
            throw new ApplicationException("Database connection string not found");
        }

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        });
    }

    private static void RegisterControllers(IServiceCollection services)
    {
        services.AddControllers();
    }

    private static void RegisterCustomDependencies(IServiceCollection services)
    {
        services.AddServices("shareme_backend.Services", Assembly.GetExecutingAssembly());
    }

    private static void RegisterSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.OperationFilter<SwaggerDefaultValues>();

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                },
            });
        });
    }

    private static void RegisterCors(IServiceCollection services)
    {
        var frontendUrl = _configuration["Application:FrontendUrl"];
        if (frontendUrl is null)
        {
            throw new ApplicationException("Cors URL not found");
        }

        services.AddCors(opt => opt.AddPolicy(_policyName, policy =>
            policy
                .WithOrigins(frontendUrl)
                .AllowAnyHeader()
                .AllowAnyMethod()));
    }

    private static void RegisterAuthentication(IServiceCollection services)
    {
        var key = Encoding.ASCII.GetBytes(Env.Secret);

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        });
    }

    private static void RegisterAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ModelToDTOMapping));
    }

    private static void RegisterEnv()
    {
        Env.Initialize(_configuration);
    }
}
