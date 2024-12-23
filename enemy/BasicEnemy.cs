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
    internal class BasicEnemy : Enemy
    {
        private string BASIC_ENEMY_URI = "pack://application:,,,/Resources/img/" + ConfigurationManager.AppSettings["BASIC_ENEMY_IMAGE"];
        private byte counter = 0;
        public BasicEnemy(int spawnX, int spawnY, SpaceShip ship)
        {
            health = 20;
            this.spawnX = spawnX;
            this.spawnY = spawnY;
            width = 50;
            height = 50;
            Width = width;
            Height = height;
            locationX = this.spawnX;
            locationY = this.spawnY + 70;
            Source = new BitmapImage(new Uri(BASIC_ENEMY_URI));
            this.ship = ship;
            speed = 2;
            score = 5;
            chance = 10;
            damage = 1;
        }
        public override void update()
        {
            updateSpeedX();
            try
            {
                move();
            }
            catch(OutOfTheWindowException)
            {
                this.destroy();
            }
            moveBullets();
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

        //public override void takeDamage()
        //{

        //}
    }
}
