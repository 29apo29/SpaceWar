using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using SpaceWar3.bullet;

namespace SpaceWar3.enemy
{
    internal class FastEnemy : Enemy
    {
        private string FAST_ENEMY_URI = "pack://application:,,,/Resources/img/" + ConfigurationManager.AppSettings["FAST_ENEMY_IMAGE"];
        private byte counter = 0;
        public FastEnemy(int spawnX, int spawnY, SpaceShip ship)
        {
            health = 10;
            this.spawnX = spawnX;
            this.spawnY = spawnY;
            width = 50;
            height = 50;
            Width = width;
            Height = height;
            locationX = this.spawnX;
            locationY = this.spawnY + 70;
            Source = new BitmapImage(new Uri(FAST_ENEMY_URI));
            this.ship = ship;
            speed = 7;
            score = 10;
            chance = 20;
            damage = 1;
        }
        public override void update()
        {
            updateSpeed();
            try
            {
                move();
            }
            catch (OutOfTheWindowException)
            {
                this.destroy();
            }
            moveBullets();
        }
        private void updateSpeed()
        {
            double angle = Math.Atan2(ship.LocationY - LocationY, ship.LocationX - LocationX);
            SpeedX = speed * Math.Cos(angle);
            SpeedY = speed * Math.Sin(angle);
        }

        public override void shoot()
        {
            counter += 1;
            if (counter >= 40 && Math.Abs(LocationX - ship.LocationX) <= 50)
            {
                counter = 0;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    bullets.Add(new SimpleBullet(this));
                }); ;
            }
        }
    }
}
