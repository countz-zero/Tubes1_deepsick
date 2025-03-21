using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;
using System;

public class Wintermute : Bot
{
    int count = 0;
    // The main method starts our bot
    static void Main(string[] args)
    {
        new Wintermute().Start();
    }

    // Constructor, which loads the bot config file
    Wintermute() : base(BotInfo.FromFile("Wintermute.json")) { }

    // Called when a new round is started -> initialize and do some movement
    public override void Run()
    {

        BodyColor = Color.FromArgb(0xFF, 0x8C, 0x00);   // Dark Orange
        TurretColor = Color.FromArgb(0xFF, 0xA5, 0x00); // Orange
        RadarColor = Color.FromArgb(0xFF, 0xD7, 0x00);  // Gold
        BulletColor = Color.FromArgb(0xFF, 0x45, 0x00); // Orange-Red
        ScanColor = Color.FromArgb(0xFF, 0xFF, 0x00);   // Bright Yellow 
        TracksColor = Color.FromArgb(0x99, 0x33, 0x00); // Dark Brownish-Orange
        GunColor = Color.FromArgb(0xCC, 0x55, 0x00);    // Medium Orange

        
        double directionToCenter = DirectionTo(ArenaWidth/2, ArenaHeight/2);
        double rotAtm = CalcDeltaAngle(directionToCenter, Direction);
        TurnLeft(rotAtm);
        // Repeat while the bot is running
        while (IsRunning)
        {
            if(Direction != DirectionTo(ArenaWidth/2, ArenaHeight/2)) {
                rotAtm = CalcDeltaAngle(directionToCenter, Direction);
                TurnLeft(rotAtm);
            }

            if (count > 5) {
                TurnLeft(180);
                count = 0;
            }
            TurnRadarLeft(90);
            TurnRadarRight(180);
            TurnRadarLeft(90);
            count++;
        }
    }


    // We saw another bot -> fire!
    public override void OnScannedBot(ScannedBotEvent evt)
    {
        double trackedDir;
        double rotAtm;
        if(DistanceTo(evt.X, evt.Y) > 300) {
            trackedDir = DirectionTo(evt.X, evt.Y);

            rotAtm = CalcDeltaAngle(Direction, trackedDir);
            TurnLeft(rotAtm);
            Forward(100);
            Fire(1);
        }
    }

    //We were hit by a bullet -> turn perpendicular to the bullet
    public override void OnHitByBullet(HitByBulletEvent evt)
    {
        // Calculate the bearing to the direction of the bullet
        var bearing = CalcBearing(evt.Bullet.Direction);

        // Turn 90 degrees to the bullet direction based on the bearing
        TurnLeft(90 - bearing);
    }

    public void onHitBot(HitBotEvent e) {
		// Only print if he's not already our target.
		// Set the target
		int trackName = e.VictimId;
		// Back up a bit.
		// Note:  We won't get scan events while we're doing this!
		// An AdvancedRobot might use setBack(); execute();
		gunTurnAmt = NormalizeRelativeAngle(BearingTo(e.X, e.Y) + Direction - RadarDirection);
		TurnGunRight(gunTurnAmt);
		Fire(3);
		Back(50);
	}

    public void OnHitWall(HitWallEvent evt) {
        TurnRight(180);
        Back(500);
    }
}
