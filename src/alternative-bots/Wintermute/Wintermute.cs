using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;
using System;

public class Wintermute : Bot
{
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

        double initAngle = BearingTo(ArenaWidth, ArenaHeight);
        Console.WriteLine(initAngle);

        TurnRadarRight(360);
        Forward(100);
        TurnRadarRight(360);
        Back(100);

        double gunTurnAmt = 10;
        int count = 0;
        int trackedId;
        // Repeat while the bot is running
        while (IsRunning)
        {
            TurnGunRight(gunTurnAmt);
			// Keep track of how long we've been looking
			count++;
			// If we've haven't seen our target for 2 turns, look left
			if (count > 2) {
				gunTurnAmt = -10;
			}
			// If we still haven't seen our target for 5 turns, look right
			if (count > 5) {
				gunTurnAmt = 10;
			}
			// If we still haven't seen our target after 10 turns, find another target
			if (count > 11) {
				trackName = null;
			}
        }
    }

    // We saw another bot -> fire!
    public override void OnScannedBot(ScannedBotEvent evt)
    {
        
		// If we have a target, and this isn't it, return immediately
		// so we can get more ScannedRobotEvents.
		if (trackName != null && !evt.ScannedBotId.equals(trackName)) {
			return;
		}

		// If we don't have a target, well, now we do!
		if (trackName == null) {
			trackName = evt.ScannedBotId();
			Console.WriteLine("Tracking " + trackName);
		}
		// This is our target.  Reset count (see the run method)
		count = 0;
		// If our target is too far away, turn and move toward it.
		if (DistanceTo(evt.X, evt.Y) > 150) {
			gunTurnAmt = NormalizedRelativeAngle(BearingTo(evt.X, evt.Y) + (Direction - RadarDirection));

			TurnGunRight(gunTurnAmt); // Try changing these to setTurnGunRight,
			TurnRight(BearingTo(evt.X, evt.Y)); // and see how much Tracker improves...
			// (you'll have to make Tracker an AdvancedRobot)
			Forward(DistanceTo(evt.X, evt.Y) - 140);
			return;
		}

		// Our target is close.
		gunTurnAmt = NormalizeRelativeAngle(BearingTo(evt.X, evt.Y) + (Direction - RadarDirection));
		TurnGunRight(gunTurnAmt);
		Fire(3);

		// Our target is too close!  Back up.
		if (DistanceTo(evt.X, evt.Y) < 100) {
			if (BearingTo(evt.X, evt.Y) > -90 && BearingTo(evt.X, evt.Y) <= 90) {
				Back(40);
			} else {
				Forward(40);
			}
		}
		Rescan();
    }

    // We were hit by a bullet -> turn perpendicular to the bullet
    public override void OnHitByBullet(HitByBulletEvent evt)
    {
        // Calculate the bearing to the direction of the bullet
        var bearing = CalcBearing(evt.Bullet.Direction);

        // Turn 90 degrees to the bullet direction based on the bearing
        TurnLeft(90 - bearing);
    }
}
