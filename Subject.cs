using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SpaceWar3
{
    abstract class Subject : Image , IMovable
    {
        protected double speedX;
        protected double speedY;
        protected int locationX;
        protected int locationY;
        protected int width;
        protected int height;
        protected bool isFinished = false;


        public bool IsFinished
        {
            get => this.isFinished;
        }
        public int LocationXWrite
        {
            get => this.locationX;
        }
        public int LocationYWrite
        {
            get => this.locationY;
        }
        public int LocationX
        {

            get
            {
                return this.locationX + (this.width / 2); 
            }
        }
        public int LocationY
        {
            get
            {
                return this.locationY + (this.height / 2);
            }

        }
        public double SpeedX
        {
            get { return speedX; }
            set
            {
                if (speedY == 0) this.speedX = value;
                else
                {
                    if(value != 0)
                    {
                        this.speedY = speedY / Math.Sqrt(2);
                        this.speedX = value / Math.Sqrt(2);
                    }
                    else this.speedX = value;
                    
                }
            }
        }
        public double SpeedY
        {
            get { return speedY; }
            set
            {
                if (speedX == 0) this.speedY = value;
                else
                {
                    if (value != 0)
                    {
                        this.speedX = speedX / Math.Sqrt(2);
                        this.speedY = value / Math.Sqrt(2);
                    }
                    else this.speedY = value;
                }
            }
        }

        public int getWidth()
        {
            return this.width;
        }
        public virtual void move()
        {
            this.locationX += (int)speedX;
            this.locationY += (int)speedY;
            if (this.LocationX >= Game.maxX + this.width || this.LocationX <= -1*this.width || this.LocationY >= Game.maxY + this.height || this.LocationY <= -1 * this.height)
                throw new OutOfTheWindowException(this);
        }
    }
}
