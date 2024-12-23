using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWar3.bullet
{
    internal interface IBullet
    {
        void onHit(Ship ship);
    }
}
