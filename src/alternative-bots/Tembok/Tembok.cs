using System.Drawing;
using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;
using System;
public class Tembok : Bot
{
    bool peek = false;
    //Bool untuk kepastian apakah boleh berhenti untuk menembak
    // The main method starts our bot
    static void Main(string[] args)
    {
        new Tembok().Start();
    }

    // Constructor, which loads the bot config file
    Tembok() : base(BotInfo.FromFile("Tembok.json")) { }

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

        //Hadapi 0 Derajat
        if (Direction <= 180 ) {
            TurnRight(Direction);
        } else {
            TurnLeft(360 - Direction);
        }

        //Jarak jaga agar tidak menabrak tembok
        double WALL_MARGIN = 30;

        //Maju sampai tembok, nemun beri sedikit jarak agak tidak terhitung menabrak
        Forward(ArenaWidth - X - WALL_MARGIN);

        //Putar senapan agar menghadapi arena
        TurnGunLeft(90);

        TurnLeft(90);
        Forward(ArenaHeight - Y - WALL_MARGIN);
        // Repeat while the bot is running
        while (IsRunning)
        {
            //Ulangi putar-putar arena
            peek = false;
            TurnLeft(90);
            peek = true;
            Forward((ArenaWidth - 2*WALL_MARGIN));
            peek = false;
            TurnLeft(90);
            peek = true;
            Forward((ArenaHeight - 2*WALL_MARGIN));
            peek = false;
            TurnLeft(90);
            peek = true;
            Forward((ArenaWidth - 2*WALL_MARGIN));
            peek = false;
            TurnLeft(90);
            peek = true;
            Forward((ArenaHeight - 2*WALL_MARGIN));
        }
    }

    // We saw another bot -> fire!
    public override void OnScannedBot(ScannedBotEvent evt)
    {
        Fire(2);
        if (peek) {
            SetStop();
            SetStop();
            Rescan();
        }

        SetResume();
    }


    public override void OnHitBot(HitBotEvent e) {
        double bearing = BearingTo(e.X, e.Y);
        //Jika di depan kita, mundur sedikit.
        if (bearing > -90 && bearing < 90) {
			Back(100);
		} // Jika di belakang, maju sedikit
		else {
			Forward(100);
		}
    }
}
