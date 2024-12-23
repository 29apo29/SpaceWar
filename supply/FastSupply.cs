using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SpaceWar3.supply
{
    internal class FastSupply : Supply
    {
        private Timer timer;
        private int counter = 0;
        public FastSupply(SpaceShip ship, int x, int y) : base(Types.FAST, ship, x, y)
        {
            string FAST_SUPPLY_IMAGE = "pack://application:,,,/Resources/img/" + ConfigurationManager.AppSettings["FAST_SUPPLY_IMAGE"];
            Source = new BitmapImage(new Uri(FAST_SUPPLY_IMAGE));
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
            effectStop();
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            timer = null;
        }
    }
}
