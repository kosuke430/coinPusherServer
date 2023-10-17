namespace CoinPusherServer.Models;

public class CoinUser
{
    public long Id { get; set; }
    public string? Name { get; set; }

    public int HaveCoin { get; set; }

    public long Ranking { get; set; }

}