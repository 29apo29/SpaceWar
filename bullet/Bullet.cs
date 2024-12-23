using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWar3.bullet
{
    internal abstract class Bullet : Subject, IBullet
    {
        protected bool isExplosive;
        protected int damage;
        protected bool isShowing;
        protected Ship owner;

        public Bullet(Ship owner)
        {
            this.owner = owner;
        }
        public Ship Owner
        {
            get => owner;
        }
        public bool IsShowing
        {
            get => isShowing;
            set => isShowing = value;
        }

        public bool IsExplosive
        {
            get => isExplosive;
            set => isExplosive = value;
        }
        public int Damage
        {
            get => damage;
            set => damage = value;
        }

        public void onHit()
        {
            this.remove();
        }
        public virtual void onHit(Ship ship)
        {
            ship.takeDamage(this);
            this.remove();
        }

        public void remove()
        {
            isFinished = true;
        }
    }
}
