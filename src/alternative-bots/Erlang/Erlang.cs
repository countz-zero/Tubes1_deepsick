using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Erlang : Bot
{
    private int orbitDirection = 1;

    static void Main(string[] args) => new Erlang().Start();

    Erlang() : base(BotInfo.FromFile("Erlang.json")) { }

    public override void Run()
    {
        // White-grey color theme
        BodyColor = Color.FromArgb(0xF0, 0xF0, 0xF0);
        TurretColor = Color.FromArgb(0x80, 0x80, 0x80);
        RadarColor = Color.FromArgb(0x40, 0x40, 0x40);
        BulletColor = Color.FromArgb(0x20, 0x20, 0x20);
        ScanColor = Color.FromArgb(0xC0, 0xC0, 0xC0);

        while (IsRunning)
        {
        Forward(1000);
        TurnLeft(90);
        SetTurnRadarRight(Double.PositiveInfinity);
        }
    }


    public override void OnScannedBot(ScannedBotEvent e)
    {
        double bearing = BearingTo(e.X, e.Y);
        double distance = DistanceTo(e.X, e.Y);
        
        // Menjauh
        if (distance < 250)
        {
            orbitDirection *= -1;
            TurnLeft(90);
            Back(100);
        }

        // Targeting
        double absoluteBearing = Direction + bearing;
        double gunAdjust = NormalizeBearing(absoluteBearing - GunDirection);
        TurnGunRight(gunAdjust);

        // Menembak dengan energi yang cukup kecil jika jaraknya dekat
        var distance2 = DistanceTo(e.X, e.Y);
        if(distance2<200){
        Fire(1);
        }
        // Rescan
        Rescan();

        // Menjaga jarak aman
        TurnRight(NormalizeBearing(bearing + 90 * orbitDirection));
    }

    public override void OnHitBot(HitBotEvent e)
    {
        // Emergency evasion protocol
        Back(200);
        TurnLeft(45);
        orbitDirection *= -1;
        Rescan();
    }

    public override void OnHitByBullet(HitByBulletEvent e)
    {
        // Menghindari peluru
        double bulletBearing = NormalizeBearing(e.Bullet.Direction - Direction);
        TurnLeft(90 - bulletBearing);
        MaxSpeed = 8;
        Forward(150);
        MaxSpeed = 5;
    }
   public override void OnHitWall(HitWallEvent evnt)
        {
            SetTurnLeft(45);
            orbitDirection *= -1;
            SetForward(2000 * orbitDirection);
        }

    private double NormalizeBearing(double bearing)
    {
        return (bearing + 360) % 360;
    }
}