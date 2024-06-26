using NLog.Web;
using NLog;
using seaway.API.Manager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using seaway.API.Configurations;
using Microsoft.Extensions.Options;
using seaway.API.Models;
using Microsoft.Extensions.Configuration;

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
    builder.Services.AddScoped<CustomerManager>();
    builder.Services.AddScoped<RoomManager>();
    builder.Services.AddScoped<OfferManager>();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseCors(policy => policy.AllowAnyHeader()
                                .AllowAnyMethod()
                                .SetIsOriginAllowed(origin => true)
                                .AllowCredentials());

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

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



