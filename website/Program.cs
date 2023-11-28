using Microsoft.Extensions.Options;
using Microsoft.Extensions.FileProviders;
using Lithium.Website;
using website.Services;
using Serilog;
using CorrelationId.DependencyInjection;
using CorrelationId;
using Microsoft.AspNetCore.HttpOverrides;
using System.Reflection;
using System.Diagnostics;
using Lithium.Website.Domain;
using Lithium.Website.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables(prefix: "Bronekfoto_");

builder.Services.AddMvc();
builder.Services.AddRazorPages();
builder.Services.Configure<WebsiteConfiguration>(builder.Configuration.GetSection("Website"));
builder.Services.Configure<SmtpConfiguration>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddScoped<MyTransformer>();
builder.Services.AddSingleton<IPageRepository, PageRepository>();
builder.Services.AddSingleton<IPageService, PageService>();
builder.Services.AddSingleton<CreateNewPage>();
builder.Services.AddSingleton<AmendPage>();
builder.Services.AddSingleton<IFileSystem, PhysicalFileSystem>();
builder.Services.AddSingleton<IEmailService>(_ =>
{
    var cfg = _.GetRequiredService<IOptions<WebsiteConfiguration>>().Value;
    var smtp = _.GetRequiredService<IOptions<SmtpConfiguration>>().Value;
    var loggerFactory = _.GetRequiredService<ILoggerFactory>();
    return new EmailService(
        cfg.Administrator.Email,
        cfg.Name,
        smtp.SmtpServer,
        smtp.Port,
        smtp.SslEnabled,
        smtp.Username,
        smtp.Password,
        loggerFactory.CreateLogger<EmailService>());
});
builder.Services.AddScoped<IMenuRepository, MenuRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(options =>
    options.ConstraintMap.Add("pageexist", typeof(PageExistsRouteConstraint)));

builder.Services.AddDefaultCorrelationId();
builder.Services.AddHttpLogging(_ => { });
builder.Services.AddHttpContextAccessor();

builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

Console.WriteLine($"Environment={app.Environment.EnvironmentName}");
var config = app.Configuration.AsEnumerable();
foreach (var i in config)
{
    Console.WriteLine($"{i.Key}={i.Value}");
}

var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();
logger.LogInformation("Starting... Software version: {softwareVersion}", GetProductVersion());

// app.UseResponseCompression();

app.UseCorrelationId();
app.UseHttpLogging();
app.UseSerilogRequestLogging();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

var imagesPath = app.Services.GetRequiredService<IOptions<WebsiteConfiguration>>().Value.ImagesPath;
logger.LogInformation("Configuring static files for serving images from directory {imagesPath}", imagesPath);
app.UseStaticFiles(
    new StaticFileOptions
    {
        RequestPath = "/images",
        FileProvider = new PhysicalFileProvider(imagesPath)
    });

app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.MapRazorPages();
app.MapDynamicPageRoute<MyTransformer>("{**slug:pageexist}");

app.Run();

string GetProductVersion() => FileVersionInfo
    .GetVersionInfo(Assembly.GetExecutingAssembly().Location)
    .ProductVersion;
