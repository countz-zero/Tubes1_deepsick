using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

using System.Drawing;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;


public class Adam : Bot
{
    // The main method starts our bot
    static void Main(string[] args)
    {
        // Read configuration file from current directory
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("Adam.json");

        // Read the configuration into a BotInfo instance
        var config = builder.Build();
        var botInfo = BotInfo.FromConfiguration(config);

        // Create and start our bot based on the bot info
        new Adam(botInfo).Start();
    }

    // Constructor taking a BotInfo that is forwarded to the base class
    private Adam(BotInfo botInfo) : base(botInfo) {}

    // Called when a new round is started -> initialize and do some movement
    public override void Run()
    {
        BodyColor = Color.Red;
        TurretColor = Color.Blue;
        RadarColor = Color.Black;
        ScanColor = Color.Yellow;

        // Repeat while the bot is running
        while (IsRunning)
        {
            // Tell the game that when we take move, we'll also want to turn right... a lot
            SetTurnLeft(10_000);
            // Limit our speed to 5
            MaxSpeed = 5;
            // Start moving (and turning)
            Forward(10_000);
        }
    }

    // We scanned another bot -> fire hard!
    public override void OnScannedBot(ScannedBotEvent evt)
    {
        Fire(3);
    }

    // We hit another bot -> if it's our fault, we'll stop turning and moving,
    // so we need to turn again to keep spinning.
    public override void OnHitBot(HitBotEvent e)
    {
        var bearing = BearingTo(e.X, e.Y);
        if (bearing > -10 && bearing < 10)
        {
            Fire(4);
        }
        if (e.IsRammed)
        {
            TurnLeft(8);
        }
    }
}