using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoinPusherServer.Models;
using StackExchange.Redis;



namespace CoinPusherServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinUserRoomController : ControllerBase
    {
        private readonly CoinUserContext _context;

        private readonly RedisService _redisService;

       

        public CoinUserRoomController(CoinUserContext context,RedisService redisService)
        {
            _context = context;
            _redisService=redisService;
        }


        //ホストがルームのidを取得するAPI
        [HttpGet("{id}")]
        public async Task<int> GetRoomId(long id)
        {

            if (_context.CoinUsers == null)
            {
                return -1;
            }
            var coinUser = await _context.CoinUsers.FindAsync(id);

            if (coinUser == null)
            {
                return -1;
            }

            var roomId=await _redisService.SetRoomIdAsync(coinUser.Id);
            return roomId;
        }

        private bool CoinUserExists(long id)
        {
            return (_context.CoinUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }

}