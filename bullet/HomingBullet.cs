using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpaceWar3.bullet
{
    internal class HomingBullet : Bullet
    {
        private SpaceShip ship;
        private int speed;
        private BitmapImage source = new BitmapImage(new Uri("pack://application:,,,/Resources/img/" + ConfigurationManager.AppSettings["HOMING_BULLET_IMAGE"]));
        public HomingBullet(Ship owner, SpaceShip ship) : base(owner)
        {
            this.owner = owner;
            this.ship = ship;
            this.isExplosive = true;
            this.damage = 20;
            this.speed = 10;
            this.width = 100;
            this.height = 40;
            this.Width = this.width;
            this.Height = this.height;
            this.Source = this.source;
            locationX = owner.LocationX;
            locationY = owner.LocationY+100;
        }
        public override void onHit(Ship ship)
        {
            ship.takeDamage(this);
            this.remove();
        }
        public void updateSpeeds()
        {
            double angle = Math.Atan2(this.ship.LocationY - LocationY, this.ship.LocationX - LocationX);
            RotateTransform rotateTransform = new RotateTransform
            {
                Angle = -1*(90-(angle*180/Math.PI)),
                CenterX = this.width/2,
                CenterY = this.height/2
            };
            this.RenderTransform = rotateTransform;
            SpeedX = this.speed * Math.Cos(angle);
            SpeedY = this.speed * Math.Sin(angle);
        }
    }
}
