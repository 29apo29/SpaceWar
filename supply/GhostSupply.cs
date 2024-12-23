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
    internal class GhostSupply : Supply
    {
        private Timer timer;
        private int counter = 0;
        private BitmapImage GhostImage;
        private BitmapImage GhostlessImage;
        public GhostSupply(SpaceShip ship, int x, int y) : base(Types.GHOST, ship, x, y)
        {
            string SHIELD_SUPPLY_IMAGE = "pack://application:,,,/Resources/img/" + ConfigurationManager.AppSettings["SHIELD_SUPPLY_IMAGE"];
            string SPACESHIP_GHOST_IMAGE = "pack://application:,,,/Resources/img/" + ConfigurationManager.AppSettings["SPACESHIP_GHOST_IMAGE"];
            string SPACESHIP_IMAGE = "pack://application:,,,/Resources/img/" + ConfigurationManager.AppSettings["SPACESHIP_IMAGE"];
            Source = new BitmapImage(new Uri(SHIELD_SUPPLY_IMAGE));
            GhostImage = new BitmapImage(new Uri(SPACESHIP_GHOST_IMAGE));
            GhostlessImage = new BitmapImage(new Uri(SPACESHIP_IMAGE));
        }
        public override void effect()
        {
            timer = new Timer(updateTimer, null, 0, 3000);
            Application.Current.Dispatcher.Invoke(() =>
            {
                ship.Source = GhostImage;
            });
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
        protected override void effectStop()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ship.Source = GhostlessImage;
            });
            base.effectStop();
        }
    }
}
