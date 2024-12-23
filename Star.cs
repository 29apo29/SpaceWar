using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace SpaceWar3
{
    internal class Star : Label
    {
        public int x;
        public int y;
        public static int maxX;
        public static int maxY;

        public Star()
        {
            this.Content = ".";
            this.Foreground = Brushes.White;
            Random rand = new Random();
            this.x = rand.Next(0, maxX);
            this.y = rand.Next(0, maxY);
        }
        public void setY()
        {
            if (this.y + 1 > maxY)
            {
                this.y = 0;
                Random rand = new Random();
                this.x = rand.Next(0, maxX);
            }
            else this.y++;
        }
    }
}
