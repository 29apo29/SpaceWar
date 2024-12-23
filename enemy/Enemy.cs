using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SpaceWar3.bullet;

namespace SpaceWar3.enemy
{
    internal abstract class Enemy : Ship
    {
        protected int spawnX;
        protected int spawnY;
        protected SpaceShip ship;
        protected bool isShowing;
        protected float speed;
        protected byte score;
        protected byte chance;

        public bool IsShowing
        {
            get => isShowing;
            set => isShowing = value;
        }

        public byte Score
        {
            get => score;
        }
        public byte Chance
        {
            get => chance;
        }


        public abstract void update();
        protected void updateSpeedX()
        {
            SpeedX = Math.Abs(ship.LocationX - LocationX) <= 10
                ? 0 :
                (ship.LocationX - LocationX) / Math.Abs(ship.LocationX - LocationX) * speed;
        }
        protected void updateSpeedY()
        {
            SpeedY = Math.Abs(ship.LocationY - LocationY) <= 10 ? 0 : (ship.LocationY - LocationY) / Math.Abs(ship.LocationY - LocationY) * speed;
        }

        public override void takeDamage(Bullet bullet)
        {
            base.takeDamage(bullet);
            if (health <= 0) destroy();
        }
        public void takeDamage(int health)
        {
            this.health -= health;
            if (this.health <= 0) destroy();
        }
        protected void destroy()
        {
            isFinished = true;
        }
        public void remove(List<Bullet> notOwner)
        {
            foreach (Bullet item in bullets)
            {
                notOwner.Add(item);
            }
            //this.bullets.Clear();
        }

        public void moveBullets()
        {
            List<Bullet> forDelete = new List<Bullet>();
            foreach (Bullet bullet in bullets.ToArray())
            {
                if (bullet == null) continue;
                if(bullet is HomingBullet)
                {
                    Application.Current.Dispatcher.Invoke(() => {
                        ((HomingBullet)bullet).updateSpeeds();
                    });
                }
                try
                {
                    bullet.move();
                }
                catch (OutOfTheWindowException)
                {
                    bullet.remove();
                }
            }
        }
    }
}
