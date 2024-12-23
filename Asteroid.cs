using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SpaceWar3
{
    internal class Asteroid : Subject
    {
        private int radius;
        private bool isActive = false;
        private bool isShowing = false;

        public bool IsActive
        {
            get => this.isActive;
        }
        public bool IsShowing
        {
            get => this.isShowing;
            set => this.isShowing = value;
        }
        public int Radius
        {
            get => this.radius;
        }
        public Asteroid(byte radius) 
        {
            this.radius = radius*50;
            this.width = this.radius;
            this.height = this.radius;
            this.Width = this.width;
            this.Height = this.height;
            this.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/img/" + ConfigurationManager.AppSettings["ASTEROID_IMAGE"]));
            Random rand = new Random();
            this.locationX = rand.Next(0, 2) == 0 ? -1 * this.width / 2 : Game.maxX ;
            this.locationY = rand.Next(0, 2) == 0 ? -1 * this.width / 2 : Game.maxY + this.width / 2;
            this.speedX = this.locationX == -1 * this.width / 2 ? 3 : -3;
            this.speedY = this.locationY == -1 * this.width / 2 ? 3 : -3;
            this.isShowing = false;
        }
        public void reCreate(byte radius)
        {
            this.radius = radius * 100;
            this.width = this.radius;
            this.height = this.radius;
            this.Width = this.width;
            this.Height = this.height;
            Random rand = new Random();
            this.locationX = rand.Next(0, 2) == 0 ? -1*this.width/2 : Game.maxX + this.width / 2;
            this.locationY = rand.Next(0, 2) == 0 ? -1 * this.width / 2 : Game.maxY;
            this.speedX = this.locationX == -1 * this.width / 2 ? 3 : -3;
            this.speedY = this.locationY == -1 * this.width / 2 ? 3 : -3;
            this.isActive = true;
        }
        public override void move()
        {
            if (!this.isActive) return;
            try
            {
                base.move();
            }
            catch(OutOfTheWindowException e)
            {
                this.isActive = false;
            }
        }
    }
}
