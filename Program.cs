using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddRateLimiter(options =>
//{
//    options.AddPolicy("swaggerPolicy", context =>
//        RateLimitPartition.GetFixedWindowLimiter(
//            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
//            factory: key => new FixedWindowRateLimiterOptions
//            {
//                PermitLimit = 5,
//                Window = TimeSpan.FromMinutes(1),
//                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
//                QueueLimit = 2
//            }));
//});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.UseWhen(context => context.Request.Path.StartsWithSegments("/swagger"), appBuilder =>
//{
//    appBuilder.UseRateLimiter(new RateLimiterOptions
//    {
//        GlobalLimiter = RateLimitPartition.GetFixedWindowLimiter(
//            key => "swagger",
//            _ => new FixedWindowRateLimiterOptions
//            {
//                PermitLimit = 5,
//                Window = TimeSpan.FromMinutes(1),
//                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
//                QueueLimit = 1
//            })
//    });
//});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
