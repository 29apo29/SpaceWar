using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SpaceWar3.supply
{
    internal class StrongSupply : Supply
    {
        private Timer timer;
        private int counter = 0;
        public StrongSupply(SpaceShip ship, int x, int y) : base(Types.STRONG, ship, x, y)
        {
            string STRONG_SUPPLY_IMAGE = "pack://application:,,,/Resources/img/" + ConfigurationManager.AppSettings["STRONG_SUPPLY_IMAGE"];
            Source = new BitmapImage(new Uri(STRONG_SUPPLY_IMAGE));
        }
        public override void effect()
        {
            timer = new Timer(updateTimer, null, 0, 3000);
        }
        private void updateTimer(object state)
        {
            if (counter == 0)
            {
                counter++;
                return;
            }
            this.effectStop();
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            timer = null;
        }
    }
}
