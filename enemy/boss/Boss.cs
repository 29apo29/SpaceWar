using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SpaceWar3.bullet;

namespace SpaceWar3.enemy.boss
{
    internal abstract class Boss : Enemy
    {
        private byte counter = 0;
        protected List<Enemy> enemies = new List<Enemy>();
        private byte counterE = 0;
        protected int initialHealth;
        protected int minHRate;
        private byte homingCounter;
        public Boss(SpaceShip ship) 
        {
            this.ship = ship;
            this.chance = 100;
        }
        public List<Enemy> Enemies
        {
            get => this.enemies;
        }

        public override void shoot()
        {
            this.counter += 1;
            this.homingCounter += 1;
            if(this.counter >= 120)
            {
                this.counter = 0;
                this.homingCounter = 0;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.bullets.Add(new HomingBullet(this,this.ship));
                });
            }
            if (this.counter >= 30 && Math.Abs(LocationX - ship.LocationX) <= 200)
            {
                this.counter = 0;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.bullets.Add(new SimpleBullet(this));
                });
            }
        }
        public abstract void addEnemy();
        public override void update()
        {
            this.counterE += 1;
            if (this.LocationY < 170) this.speedY = 2;
            else this.speedY = 0;
            updateSpeedX();
            try
            {
                move();
            }
            catch (OutOfTheWindowException)
            {
                this.destroy();
            }
            moveBullets();
            if (this.counterE == 255)
            {
                this.addEnemy();
                this.counterE = 0;
            }
        }
        public override void takeDamage(Bullet bullet)
        {
            base.takeDamage(bullet);
        }
        public void remove(List<Bullet> notOwner,List<Enemy> enemies)
        {
            base.remove(notOwner);
            foreach (Enemy enem in this.enemies.ToArray())
            {
                enemies.Add(enem);
            }
        }
    }
}
