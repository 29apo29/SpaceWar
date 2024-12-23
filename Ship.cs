using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SpaceWar3.bullet;

namespace SpaceWar3
{
    abstract class Ship : Subject , IShip
    {
        protected int health;
        protected int damage;
        protected List<Bullet> bullets = new List<Bullet>();

        public List<Bullet> Bullets
        {
            get => this.bullets;
        }

        public int Health
        {
            get { return health; }
        }
        public virtual int Damage
        {
            get { return damage; }
        }

        public abstract void shoot();
        public virtual void takeDamage(Bullet bullet)
        {
            this.health -= bullet.Damage;
        }
        public void updateLocation(int x, int y)
        {
            this.locationX += x;
            this.locationY += y;
        }

    }
}
