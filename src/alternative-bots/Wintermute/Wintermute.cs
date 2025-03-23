using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;
using System;

public class Wintermute : Bot
{
    int count = 0;
    double gunTurnAmt;
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
            gunTurnAmt = 60;

            if (count > 2) {
                TurnLeft(120);
                count = 0;
            }

            TurnRadarLeft(gunTurnAmt);
            TurnRadarRight(gunTurnAmt*2);
            TurnRadarLeft(gunTurnAmt);
            count++;
        }
    }

    public override void OnScannedBot(ScannedBotEvent evt)
    {
        bool killed = false;
        double trackedDir;
        double rotAtm;
        if(DistanceTo(evt.X, evt.Y) > 100) {
            trackedDir = DirectionTo(evt.X, evt.Y);

            rotAtm = CalcDeltaAngle(trackedDir, Direction);
            TurnLeft(rotAtm);
            Forward(100);
            Fire(2);
        }

        else {
            Back(50);
            trackedDir = DirectionTo(evt.X, evt.Y);

            rotAtm = CalcDeltaAngle(trackedDir, Direction);
            TurnLeft(rotAtm);
            int timesFire = (int)evt.Energy / 3;
            while(timesFire > 0) {
                Fire(3);
                timesFire--;
            }
        }
    }

    //We were hit by a bullet -> turn perpendicular to the bullet
    public override void OnHitByBullet(HitByBulletEvent evt)
    {
        SetTurnLeft(90);
        Back(100);
    }

    public void onHitBot(HitBotEvent e) {
		double collisionDir;
        double turnAmt;
        collisionDir = DirectionTo(e.X, e.Y);
        turnAmt = CalcDeltaAngle(collisionDir, Direction);

        TurnLeft(turnAmt);
        Fire(2);
	}

    public void OnHitWall(HitWallEvent evt) {
        TurnRight(180);
        Back(500);
    }
}
