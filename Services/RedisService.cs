using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using CoinPusherServer.Logics;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Common;




public class RedisService
{
    private IDatabase _database;
    private RoomIdLogics roomIdLogics=new RoomIdLogics();

    private TokenService tokenService;

    private readonly string _secretKey;

    public RedisService(IConnectionMultiplexer multiplexer,RoomIdLogics roomIdLogics, IConfiguration configuration)
    {
        _database = multiplexer.GetDatabase();
        // this.roomIdLogics = roomIdLogics;
        this._secretKey = configuration["Jwt:Key"];;
        this.tokenService = new TokenService();

    }
    public async Task<int> SetRoomIdAsync(long key)
    {

        var roomId=roomIdLogics.DecideroomId();
        // var roomId=1;
        var token=tokenService.GenerateToken(key.ToString());
        // await _database.StringSetAsync(key.ToString(), token);
        var roomInfo="roomId"+roomId+":token"+token;
        _database = _database.Multiplexer.GetDatabase(1);
        await _database.StringSetAsync(key.ToString(), roomInfo);
        return roomId;
    }

    // public async Task<string> GetStringAsync(string key)
    // {
    //     return await _database.StringGetAsync(key);
    // }
}
