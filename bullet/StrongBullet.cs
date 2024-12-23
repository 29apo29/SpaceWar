using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SpaceWar3.bullet
{
    internal class StrongBullet : Bullet
    {
        public StrongBullet(Ship owner) : base(owner)
        {
            string BULLET_URI = "pack://application:,,,/Resources/img/" + (owner is SpaceShip ? ConfigurationManager.AppSettings["STRONG_BULLET_IMAGE"] : ConfigurationManager.AppSettings["STRONG_BULLET_REVERSE_IMAGE"]);
            IsExplosive = false;
            Damage = 10 * owner.Damage;
            locationX = owner.LocationX;
            locationY = owner.LocationY;
            speedX = 0;
            speedY = owner is SpaceShip ? -7 + owner.SpeedY / 2 : 7;
            speedX = owner is SpaceShip ? owner.SpeedX / 2 : 0;
            width = 10;
            height = 10;
            Width = 10;
            Height = 10;
            Source = new BitmapImage(new Uri(BULLET_URI));
        }
    }
}
