using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows;

namespace SpaceWar3.enemy.boss
{
    internal class StrongBoss:Boss
    {
        public StrongBoss(SpaceShip ship) : base(ship)
        {
            string STRONG_BOSS_URI = "pack://application:,,,/Resources/img/" + ConfigurationManager.AppSettings["STRONG_BOSS_IMAGE"];
            this.health = 500;
            this.initialHealth = 500;
            this.spawnX = Game.maxX / 2;
            this.spawnY = -100;
            width = 200;
            height = 200;
            Width = width;
            Height = height;
            locationX = this.spawnX;
            locationY = this.spawnY;
            Source = new BitmapImage(new Uri(STRONG_BOSS_URI));
            speed = 2f;
            score = 100;
            chance = 100;
            damage = 2;
            this.score = 150;
            this.minHRate = 10;
        }
        public override void addEnemy()
        {
            if (this.initialHealth - this.initialHealth * this.minHRate / 100 >= this.health)
            {
                this.minHRate += 10;
                Random rand = new Random();
                int x = rand.Next(0, 2) == 0 ? this.LocationX + 100 : this.LocationX - 100;
                x = x >= Game.maxX ? x - 200 : x <= 0 ? x + 200 : x;
                int y = this.LocationY + rand.Next(50, 150);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.enemies.Add(new StrongEnemy(x, y, this.ship));
                });
            }
        }
    }
}
