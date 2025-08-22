using BlogTalks.API;
using BlogTalks.API.Middlewares;
using BlogTalks.Application;
using BlogTalks.EmailSenderAPI.Models;
using BlogTalks.Infrastructure;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(@"c:\logs\myapp.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();



builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName);

    // Add JWT Bearer token support
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Correct and properly formatted security requirement
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                In = ParameterLocation.Header,
                Name = "Authorization",
                Scheme = "Bearer"
            },
            Array.Empty<string>()
        }
    });
});


builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));

//builder.Services.AddScoped<IEmailService, EmailHttpService>();

builder.Services.AddHttpClient("EmailSenderApi", client =>
{
    var config = builder.Configuration.GetSection("Services:EmailSenderApi");
    client.BaseAddress = new Uri(config["Url"]);
});

builder.Services.AddFeatureManagement();



//builder.Services.AddMediatR(cfg =>
//{
//    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
//    cfg.RegisterServicesFromAssembly(typeof(GetAllResponse).Assembly);
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//app.MapPost("/email", (EmailDto request, IEmailService emailService) =>
//{
//    emailService.SendEmailAsync(request);

//    return Results.Ok();
//});

app.Run();
