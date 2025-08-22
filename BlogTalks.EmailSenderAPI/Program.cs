using BlogTalks.EmailSenderAPI.Models;
using BlogTalks.EmailSenderAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddHostedService<RabbitMqBackgroundEmailService>();
builder.Services.Configure<RabbitMqSettingsEmailSender>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton(builder.Configuration
    .GetSection("RabbitMQ")
    .Get<RabbitMqSettingsEmailSender>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.MapPost("/email", (EmailDto request, IEmailSender emailService) =>
{
    emailService.SendEmailAsync(request);

    return Results.Ok();
});
app.Run();