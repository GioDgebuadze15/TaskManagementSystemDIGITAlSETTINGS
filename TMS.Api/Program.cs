using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using TMS.Api;
using TMS.Data.Forms.Validation;
using TMS.Database.DatabaseRepository;
using TMS.Database.EntityFramework;
using TMS.Services.AppServices;
using TMS.Services.AppServices.AdminAppService;
using TMS.Services.AppServices.Middleware;
using TMS.Services.AppServices.TaskAppService;
using TMS.Services.AppServices.UserAppService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("Dev"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new RsaSecurityKey(RsaKey.GetRsaKey()),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });
// builder.Services.AddAuthorization(options =>
// {
// options.AddPolicy(TmsConstants.Policies.Admin, policy => policy
// .RequireAuthenticatedUser()
// .RequireClaim(TmsConstants.Claims.Role,
// TmsConstants.Roles.Admin)
// );
// });

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITaskService, TaskService>();
builder.Services.AddTransient<IAdminService, AdminService>();

builder.Services.AddControllers();

// Automatic Fluent Validation 
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(typeof(LoginUserFormValidation).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(CreateUserFormValidation).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(CreateTaskFormValidation).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(UpdateTaskFormValidation).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(AssignRolesFormValidation).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo {Title = "TMS API", Version = "v1"});

    // Define the JWT bearer scheme
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    // Require JWT bearer authorization for all endpoints
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
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    if (app.Environment.IsDevelopment())
    {
        // Todo: change this and make it more secure
        // This is only for development
        new IdentityUserBuilder()
            .WithName("admin")
            .WithPassword("Admin12345")
            .WithClaim(TmsConstants.Claims.Role, TmsConstants.Roles.Admin)
            .Build(userManager);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TMS API V1");
        options.EnableDeepLinking();
        options.DisplayRequestDuration();
        options.DocExpansion(DocExpansion.None);
        options.DocumentTitle = "TMS API";

        // Pass the JWT token to Swagger 
        options.InjectJavascript("/swagger/authorize.js");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.UseMiddleware<LoggingMiddleware>();

app.MapControllers();

app.Run();