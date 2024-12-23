using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SpaceWar3.supply
{
    internal class HealSupply : Supply
    {
        public HealSupply(SpaceShip ship, int x, int y) : base(Types.HEAL, ship, x, y)
        {
            string HEAL_SUPPLY_IMAGE = "pack://application:,,,/Resources/img/" + ConfigurationManager.AppSettings["HEAL_SUPPLY_IMAGE"];
            Source = new BitmapImage(new Uri(HEAL_SUPPLY_IMAGE));
        }
        public override void effect()
        {
            ship.heal(10);
            effectStop();
        }
    }
}
