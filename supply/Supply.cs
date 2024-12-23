using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SpaceWar3.supply
{
    internal abstract class Supply : Subject
    {
        protected Types type;
        protected SpaceShip ship;
        protected bool isShowing;


        public bool IsShowing
        {
            get => isShowing;
            set => isShowing = value;
        }
        public Types Type
        {
            get => type;
        }
        public Supply(Types type, SpaceShip ship, int x, int y)
        {
            this.ship = ship;
            this.type = type;
            width = 30;
            height = 30;
            Width = width;
            Height = height;
            locationX = x;
            locationY = y;
            speedY = 3;
        }
        public void removeFromUI()
        {
            isFinished = true;
        }
        protected virtual void effectStop()
        {
            isFinished = true;
            ship.supplies.Find(e => e.type == this.type).status = false;
        }
        public abstract void effect();
    }
}
