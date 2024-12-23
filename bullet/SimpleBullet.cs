using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SpaceWar3.bullet
{
    internal class SimpleBullet : Bullet
    {
        public SimpleBullet(Ship owner) : base(owner)
        {
            string BULLET_URI = "pack://application:,,,/Resources/img/" + (owner is SpaceShip ? ConfigurationManager.AppSettings["SIMPLE_BULLET_IMAGE"] : ConfigurationManager.AppSettings["SIMPLE_BULLET_REVERSE_IMAGE"]);
            IsExplosive = false;
            Damage = 5 * owner.Damage;
            locationX = owner.LocationX;
            locationY = owner.LocationY;
            speedX = 0;
            speedY = owner is SpaceShip ? -7 + owner.SpeedY / 2 : 7;
            speedX = owner.SpeedX / 2;
            width = 10;
            height = 10;
            Width = 10;
            Height = 10;
            Source = new BitmapImage(new Uri(BULLET_URI));
        }

    }
}
