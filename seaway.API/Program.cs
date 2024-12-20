using NLog.Web;
using NLog;
using seaway.API.Manager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using seaway.API.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using seaway.API.Models;
using System.Web.Http;
using seaway.API.Models.ViewModels;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.

    builder.Services.AddScoped<PicDocumentManager>();
    builder.Services.AddScoped<ActivityManager>();
    builder.Services.AddScoped<LoginManager>();
    builder.Services.AddScoped<LogHandler>();
    builder.Services.AddScoped<PasswordHelper>();
    builder.Services.AddScoped<CustomerManager>();
    builder.Services.AddScoped<RoomCategoryManager>();
    builder.Services.AddScoped<OfferManager>();
    builder.Services.AddScoped<AdminManager>();
    builder.Services.AddScoped<RoomManager>();
    builder.Services.AddScoped<EmailManager>();
    builder.Services.AddScoped<BookingManager>();
    builder.Services.AddScoped<PackageManager>();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.SaveToken = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                //ValidIssuer = builder.Configuration["JWTSettings:Issuer"],
                //ValidAudience = builder.Configuration["JWTSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:Key"]))
            };
        });


    builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseCors(policy => policy.AllowAnyHeader()
                                .AllowAnyMethod()
                                .WithOrigins("http://localhost:4200"));

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    //app.MapPost("api/Login", async (LoginManager loginManager, [FromBody] LoginModel login) =>
    //{

    //});

    app.Run();

}
catch (Exception ex)
{
    logger.Error(ex);
}
finally
{
    LogManager.Shutdown();
}



