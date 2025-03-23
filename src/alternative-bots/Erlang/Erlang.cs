using System;
using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

public class Erlang : Bot
{
    private int orbitDirection = 1;
    private double lastEnergy = 100;

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
            // Defensive patrol pattern
            ExecuteSurvivalMovement();
            ExecuteEnergyConservation();
        }
    }

    private void ExecuteSurvivalMovement()
    {
        if (DistanceRemaining < 10)
        {
            TurnRight(10 * orbitDirection);
            Forward(50);
            
            // Wall avoidance
            if (X < 100 || X > ArenaWidth - 100 || 
                Y < 100 || Y > ArenaHeight - 100)
            {
                orbitDirection *= -1;
                SetBack(100);
                SetTurnRight(90);
            }
        }
    }

    private void ExecuteEnergyConservation()
    {
        // Energy regeneration monitoring
        if (Energy >= lastEnergy)
        {
            TurnRadarRight(45);
            lastEnergy = Energy;
        }
    }

    public override void OnScannedBot(ScannedBotEvent e)
    {
        double bearing = BearingTo(e.X, e.Y);
        double distance = DistanceTo(e.X, e.Y);
        
        // Survival-first engagement rules
        if (distance < 250)
        {
            orbitDirection *= -1;
            Back(100);
        }

        // Conservative targeting
        double absoluteBearing = Direction + bearing;
        double gunAdjust = NormalizeBearing(absoluteBearing - GunDirection);
        TurnGunRight(gunAdjust);

        // Energy-efficient firing
       SetFire(0.5);

        // Maintain safe distance
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
        // Advanced bullet dodging
        double bulletBearing = NormalizeBearing(e.Bullet.Direction - Direction);
        TurnLeft(90 - bulletBearing);
        MaxSpeed = 8;
        Forward(150);
        MaxSpeed = 5;
    }

    private double NormalizeBearing(double bearing)
    {
        return (bearing + 360) % 360;
    }
}