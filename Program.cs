using Microsoft.EntityFrameworkCore;
using CoinPusherServer.Models;
using SignalRChat.Hubs;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<CoinUserContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("CoinUserContext")));
}
else
{
    builder.Services.AddDbContext<CoinUserContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("CoinUserContext")));
}

// builder.Services.AddDbContext<CoinUserContext>(opt =>
//     opt.UseInMemoryDatabase("CoinUserList"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.MapHub<ChatHub>("/chatHub");

app.Run();
