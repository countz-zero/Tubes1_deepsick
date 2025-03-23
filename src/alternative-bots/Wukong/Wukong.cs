using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Wukong : Bot
{
    private int moveDirection = 1;

    static void Main(string[] args)
    {
        new Wukong().Start();
    }

    Wukong() : base(BotInfo.FromFile("Wukong.json")) { }

    public override void Run()
    {
        // Red and gold color theme
        BodyColor = Color.FromArgb(0xB3, 0x00, 0x00);    // Dark Red
        TurretColor = Color.FromArgb(0xFF, 0xD7, 0x00);   // Gold
        RadarColor = Color.FromArgb(0xFF, 0x8C, 0x00);    // Dark Orange
        BulletColor = Color.FromArgb(0xFF, 0x45, 0x00);   // Orange-Red
        ScanColor = Color.FromArgb(0xFF, 0x99, 0x00);     // Amber
        TracksColor = Color.FromArgb(0x99, 0x33, 0x00);   // Brown
        GunColor = Color.FromArgb(0xCC, 0x55, 0x00);      // Orange

        // Radar rotation

        while (IsRunning)
        {
            // Zig-zag movement
            Forward(100 * moveDirection);
            TurnRight(45 * moveDirection);
            moveDirection *= -1;
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        if (IsTeammate(e.ScannedBotId)) return;

        double bearing = BearingTo(e.X, e.Y);
        double BodyHeading = Direction;
        double GunHeading = GunDirection;
        double absoluteBearing = BodyHeading + bearing;
        double gunAdjust = NormalizeBearing(absoluteBearing - GunHeading);

        TurnGunRight(gunAdjust);
        var distance = DistanceTo(e.X, e.Y);
        if (Math.Abs(gunAdjust) < 5 && e.Energy > 10)
        {
            Fire(Math.Min(3.0, 500 / distance));
        }

        TurnRight(bearing + 10);
    }

    public override void OnHitByBullet(HitByBulletEvent e)
    {   double BodyHeading = Direction;
        double bulletBearing = NormalizeBearing(e.Bullet.Direction - BodyHeading);
        TurnLeft(90 - bulletBearing);
        Forward(150);
        Rescan();
    }

    private double NormalizeBearing(double bearing)
    {
        while (bearing > 180) bearing -= 360;
        while (bearing < -180) bearing += 360;
        return bearing;
    }
}