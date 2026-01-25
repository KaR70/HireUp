using HireUp.Authentication;
using HireUp.Database;
using HireUp.Services;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using FluentValidation.AspNetCore;
using HireUp.Mapping;
using HireUp.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace HireUp;

public static class DependencyInjection
{
    public static IServiceCollection AddDependecies(this IServiceCollection services, IConfiguration configuration)
    {
        #region System Services
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        #endregion

        #region Add Database
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

        // services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        #endregion

        #region Database Retry Logic
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString,
        // --- THIS IS THE FIX ---
        // This tells EF Core to be resilient and retry connecting to the database
        // if it encounters a transient (temporary) error, like the DB container
        // not being ready yet.
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5, // It will try up to 5 times
                maxRetryDelay: TimeSpan.FromSeconds(30), // With up to a 30-second delay between tries
                errorNumbersToAdd: null);
        })); 
        #endregion

        
        services
            .AddMapsterConf()
            .AddAuthConfig(configuration)
            .AddFluentValidationConfig();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<UrlBuilderService>();

        services.Configure<ApplicationSettings>(configuration.GetSection(ApplicationSettings.SectionName));
        services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        return services;
    }

    private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddSingleton<IJwtProvider, JwtProvider>();

        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
                    ValidIssuer = jwtSettings?.Issuer,
                    ValidAudience = jwtSettings?.Audience,
                };
            });
        
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
        });

        return services;
    }

    private static IServiceCollection AddMapsterConf(this IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMapper>(new Mapper(mappingConfig));

        return services;
    }
    
    private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
