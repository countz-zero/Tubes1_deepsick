using System;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class RandomBot : Bot
{
    // Store the last scanned enemy's relative bearing and distance
    private double lastEnemyBearing = double.NaN;
    private double lastEnemyDistance = double.NaN;

    private Random random = new Random();

    // Bot constructor: load configuration from RandomBot.json (make sure it exists)
    public RandomBot() : base(BotInfo.FromFile("RandomBot.json")) { }

    // Entry point for the bot
    public static void Main()
    {
        new RandomBot().Start();
    }

    public override void Run()
    {
        // Main loop
        while (IsRunning)
        {
            // If we've recently scanned an enemy, do a "greedy" move away from them
            if (!double.IsNaN(lastEnemyBearing))
            {
                // Candidate turning angles (relative to current heading)
                double[] candidates = { -90, -45, 0, 45, 90 };

                double bestScore = double.MinValue;
                double bestAngle = 0;

                // Our "ideal" turn is 180° away from the enemy bearing
                double idealTurn = NormalizeAngle(lastEnemyBearing + 180);

                foreach (double candidate in candidates)
                {
                    double candidateNorm = NormalizeAngle(candidate);
                    double diff = Math.Abs(NormalizeAngle(candidateNorm - idealTurn));

                    // We want to maximize the difference from the enemy's bearing
                    if (diff > bestScore)
                    {
                        bestScore = diff;
                        bestAngle = candidate;
                    }
                }

                // Execute the chosen movement
                TurnRight(bestAngle);
                Forward(100);
            }
            else
            {
                // No enemy seen recently; move randomly
                double randomTurn = random.Next(-90, 91); // angle between -90 and 90
                TurnRight(randomTurn);
                Forward(100);
            }

            // Always scan continuously
            TurnRadarRight(360);
        }
    }

    // When an enemy bot is scanned, compute bearing/distance and fire
    public override void OnScannedBot(ScannedBotEvent e)
    {
        // Calculate delta x and delta y relative to our current position
        double dx = e.X - X;   // Changed from e.ScannedBot.X to e.X
        double dy = e.Y - Y;   // Changed from e.ScannedBot.Y to e.Y

        // Calculate the absolute angle to the target in degrees
        double angleToTarget = Math.Atan2(dy, dx) * 180.0 / Math.PI;

        // Bearing is difference between angleToTarget and our Direction
        double bearing = angleToTarget - Direction;
        bearing = NormalizeAngle(bearing); // normalize to [-180,180)

        // Distance is Euclidean distance between the two points
        double distance = Math.Sqrt(dx * dx + dy * dy);

        // Store bearing/distance for our "greedy" movement logic
        lastEnemyBearing = bearing;
        lastEnemyDistance = distance;

        // Example: Fire with power inversely proportional to distance (max 3)
        double firePower = Math.Min(3, 500 / distance);
        Fire(firePower);

        // Keep radar locked on the target
        TurnRadarRight(bearing);
    }

    // Utility method: normalize angle to [-180, 180)
    private double NormalizeAngle(double angle)
    {
        angle %= 360;
        if (angle > 180)
            angle -= 360;
        else if (angle <= -180)
            angle += 360;
        return angle;
    }
}
