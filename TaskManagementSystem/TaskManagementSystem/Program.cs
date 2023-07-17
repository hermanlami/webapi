using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Newtonsoft.Json.Serialization;
using System.Text;
using TaskManagementSystem.BLL;
using TaskManagementSystem.Middlewares;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementSystem.BLL.BackgroundJobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterBllServices(builder.Configuration);

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("C:\\Users\\Lenovo\\Documents\\TMSLogs\\TmsLogs.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), "Logs", autoCreateSqlTable: true)
            .CreateLogger();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog();
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

builder.Services.AddScoped<CustomErrorMiddleware>();
//builder.Services.AddHostedService<BackgroundJobsService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSingleton<AuthenticationMiddleware>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "apiWithAuthBackend",
            ValidAudience = "apiWithAuthBackend",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("!SomethingSecretAndLongEnoughToBeUseful!")
            ),
        };
    });
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseMiddleware(typeof(AuthenticationMiddleware));

//app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapControllerRoute(
        name: "errorHandler",
        pattern: "/api/errorHandler",
        defaults: new { controller = "ErrorHandler", action = "HandleError" }
    );
});

app.UseMiddleware<CustomErrorMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
