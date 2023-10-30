using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoinPusherServer.Models;

namespace CoinPusherServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinUserssController : ControllerBase
    {
        private readonly CoinUserContext _context;

        public CoinUserssController(CoinUserContext context)
        {
            _context = context;
        }

        // GET: api/CoinUserss
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoinUser>>> GetCoinUsers()
        {
          if (_context.CoinUsers == null)
          {
              return NotFound();
          }
            return await _context.CoinUsers.ToListAsync();
        }

        // GET: api/CoinUserss/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CoinUser>> GetCoinUser(long id)
        {
          if (_context.CoinUsers == null)
          {
              return NotFound();
          }
            var coinUser = await _context.CoinUsers.FindAsync(id);

            if (coinUser == null)
            {
                return NotFound();
            }

            return coinUser;
        }

        // PUT: api/CoinUserss/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCoinUser(long id, CoinUser coinUser)
        {
            if (id != coinUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(coinUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoinUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CoinUserss
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CoinUser>> PostCoinUser(CoinUser coinUser)
        {
          if (_context.CoinUsers == null)
          {
              return Problem("Entity set 'CoinUserContext.CoinUsers'  is null.");
          }
            _context.CoinUsers.Add(coinUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCoinUser), new { id = coinUser.Id }, coinUser);
        }

        // DELETE: api/CoinUserss/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoinUser(long id)
        {
            if (_context.CoinUsers == null)
            {
                return NotFound();
            }
            var coinUser = await _context.CoinUsers.FindAsync(id);
            if (coinUser == null)
            {
                return NotFound();
            }

            _context.CoinUsers.Remove(coinUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //コインのランキングを取得するAPI
        [HttpGet("ranking")]
        public async Task<ActionResult<IEnumerable<UserRanking>>> GetRanking()
        {
          if (_context.CoinUsers == null)
          {
              return NotFound();
          }
            var coinUsers = await _context.CoinUsers.ToListAsync();
            var ranking = coinUsers.OrderByDescending(x => x.HaveCoin).Select(x => new UserRanking { Name = x.Name, HaveCoin = x.HaveCoin }).Take(5).ToList();
            return ranking;
        }

        private bool CoinUserExists(long id)
        {
            return (_context.CoinUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
