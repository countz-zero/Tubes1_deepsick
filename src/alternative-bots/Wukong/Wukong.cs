using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Wukong : Bot
{  
    private int moveDirection = 1;
    private int consecutiveHits;
    private double lastEnemyDistance;

    static void Main(string[] args) => new Wukong().Start();

    Wukong() : base(BotInfo.FromFile("Wukong.json")) { }

    public override void Run()
    {
        // Red and gold color theme
        BodyColor = Color.FromArgb(0xB3, 0x00, 0x00);
        TurretColor = Color.FromArgb(0xFF, 0xD7, 0x00);
        RadarColor = Color.FromArgb(0xFF, 0x8C, 0x00);
        BulletColor = Color.FromArgb(0xFF, 0x45, 0x00);
        ScanColor = Color.FromArgb(0xFF, 0x99, 0x00);
        TracksColor = Color.FromArgb(0x99, 0x33, 0x00);
        GunColor = Color.FromArgb(0xCC, 0x55, 0x00);

        while (IsRunning)
        { 
            SetTurnRight(45);
            SetForward(100);
            Go();
        }

    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
         TurnToFaceTarget(e.X, e.Y);
        lastEnemyDistance = DistanceTo(e.X, e.Y);
        double bearing = BearingTo(e.X, e.Y);
        double absoluteBearing = Direction + bearing;
        
        // Targeting
        double gunAdjust = NormalizeBearing(absoluteBearing - GunDirection);
        TurnGunRight(gunAdjust);

        // Optimisasi damage
        Fire(3);

        // Menargetkan ulang musuh yang terakhir terdeteksi
        if (lastEnemyDistance > 200)
        {
            TurnRight(bearing);
            Forward(100);
        }
        Rescan();
    }
    private void TurnToFaceTarget(double x, double y)
    {
        var bearing = BearingTo(x, y);
        if (bearing >= 0)
            moveDirection = 1;
        else
            moveDirection = -1;

        TurnLeft(bearing);
    }
    public override void OnHitByBullet(HitByBulletEvent e)
    {
        // Mundur ketika terkena serangan lebih dari 2 kali
        if (consecutiveHits > 2)
        {
            Back(150);
            TurnRight(180 - Direction);
            consecutiveHits = 0;
        }
        else
        { // Respon pertama ketika terkena serangan
            double bulletBearing = NormalizeBearing(e.Bullet.Direction - Direction);
            TurnLeft(90 - bulletBearing);
            Forward(100);
        }
        Rescan();
    }
public override void OnHitWall(HitWallEvent evnt)
        {
            SetForward(200*moveDirection);
            Go();
        }
    private double NormalizeBearing(double bearing)
    {
        return (bearing + 360) % 360;
    }
}