using BenchmarkDotNet.Attributes;
using Cathei.LinqGen;

namespace SourceGenerators.Benchmarks;

[MemoryDiagnoser]
public class LinqBenchmarker
{
    public class Player
    {
        public int Score { get; set; }
    }
    
    private const int PlayersAmount = 100;
    private static readonly Player[] _players = new Player[PlayersAmount];

    public IEnumerable<Player[]> Lists
    {
        get
        {
            yield return _players;
        }
    }

    static LinqBenchmarker()
    {
        var random = new Random(123);

        for (var i = 0; i < PlayersAmount; i++)
        {
            _players[i] = new Player { Score = random.Next(0, 10000) };
        }
    }
    
    // We want to show a leaderboard of players ordered from top score
    // on a second page, where each page shows 30 players.

    [Benchmark]
    [ArgumentsSource(nameof(Lists))]
    public void Linq(Player[] players)
    {
        var leaderboardPlayers = players.OrderByDescending(p => p.Score) // Sorting the players
            .Skip(30) // Skipping the first page
            .Take(30) // Taking the players from the second page
            .ToList();
    }
    
    
    [Benchmark]
    [ArgumentsSource(nameof(Lists))]
    public void GenLinq(Player[] players)
    {
        var leaderboardPlayers = players.Gen() // Telling the source generator to start its magic
            .OrderByDescending(p => p.Score) // Sorting the players
            .Skip(30) // Skipping the first page
            .Take(30) // Taking the players from the second page
            .ToList();
    }
}