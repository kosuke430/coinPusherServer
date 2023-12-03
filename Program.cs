using Microsoft.EntityFrameworkCore;
using CoinPusherServer.Models;
using SignalRChat.Hubs;
using StackExchange.Redis;
using CoinPusherServer.Logics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<RoomIdLogics>();

builder.Services.AddSingleton<RedisService>();
// builder.Services.AddSingleton<RedisService>(sp =>
// {
//     var multiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
//     var roomIdLogics = sp.GetRequiredService<RoomIdLogics>();
//     var secretKey = builder.Configuration["Jwt:Key"];
//     var redisConnectionString = builder.Configuration.GetConnectionString("Redis");

//     if (string.IsNullOrEmpty(secretKey))
//     {
//         throw new ArgumentNullException(nameof(secretKey), "Jwt:Key is null or empty in configuration");
//     }

//     return new RedisService(multiplexer, roomIdLogics, secretKey);
// });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<CoinUserContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("CoinUserContext")));

    builder.Services.AddSingleton<IConnectionMultiplexer>(x => 
    ConnectionMultiplexer.Connect("localhost"));

    builder.Services.AddSingleton<RedisService>();

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

var Configuration = builder.Configuration;

//アクセストークン発行の設定
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Configuration["Jwt:Issuer"],
        ValidAudience = Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
    };
});


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

//redis接続
var multiplexer = app.Services.GetRequiredService<IConnectionMultiplexer>();
var redis = multiplexer.GetDatabase();

app.UseAuthentication();

app.Run();
