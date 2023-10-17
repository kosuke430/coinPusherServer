using Microsoft.EntityFrameworkCore;

namespace CoinPusherServer.Models;

public class CoinUserContext : DbContext
{
    public CoinUserContext(DbContextOptions<CoinUserContext> options)
        : base(options)
    {
    }

    public DbSet<CoinUser> CoinUsers { get; set; } = null!;
}