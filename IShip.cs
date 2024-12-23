using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceWar3.bullet;

namespace SpaceWar3
{
    interface IShip
    {
        int Health { get; }
        int Damage { get; }

        void shoot();
        void takeDamage(Bullet bullet);
    }
}
