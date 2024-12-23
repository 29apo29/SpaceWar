using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWar3
{
    interface IMovable
    {
        double SpeedX { get; }
        double SpeedY { get; }
        int LocationX { get; }
        int LocationY { get; }

        public void move();
    }
}
