using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Immutable;
using SpaceWar3.supply;
using SpaceWar3.bullet;

namespace SpaceWar3
{
    class SupplyController
    {
        public readonly Types type;
        public bool status { get; set; }
        public int count { get; set; } 
        public readonly Stack<Supply> items = new Stack<Supply>();
        public SupplyController(Types type, bool status, int count)
        {
            this.type = type;
            this.status = status;
            this.count = count;
        }
        public void use()
        {
            if(this.count != 0 && !this.status)
            {
                this.status = true;
                this.count--;
                Supply item = this.items.Pop();
                item.effect();
                //if(this.type == Types.STRONG) Debugger.Break();
            }
        }
        public void add(Supply supply)
        {
            this.items.Push(supply);
            this.count++;
        }
    }
    class SpaceShip : Ship
    {
        private string SPACESHIP_URI = "pack://application:,,,/Resources/img/" + ConfigurationManager.AppSettings["SPACESHIP_IMAGE"];
        private HashSet<string> clicking = new HashSet<string>();
        private int maxX;
        private int maxY;
        private string[] keys = 
        {
            ConfigurationManager.AppSettings["LEFT"],
            ConfigurationManager.AppSettings["RIGHT"],
            ConfigurationManager.AppSettings["DOWN"],
            ConfigurationManager.AppSettings["UP"],
            ConfigurationManager.AppSettings["SHOOT"],
            ConfigurationManager.AppSettings["FAST_SUPPLY"],
            ConfigurationManager.AppSettings["HEAL_SUPPLY"],
            ConfigurationManager.AppSettings["STRONG_SUPPLY"],
            ConfigurationManager.AppSettings["GHOST_SUPPLY"],
        };
        private byte counter = 0;
        bool isShootable = true;
        public readonly ImmutableList<SupplyController> supplies = ImmutableList.Create(
            new SupplyController(Types.FAST, false, 0),
            new SupplyController(Types.HEAL, false, 0),
            new SupplyController(Types.GHOST, false, 0),
            new SupplyController(Types.STRONG, false, 0)
        );

        public SpaceShip(int locationX, int locationY)
        {
            this.health = 100;
            this.damage = 1;
            this.speedX = 0;
            this.speedY = 0;
            this.Source = new BitmapImage(new Uri(SPACESHIP_URI, UriKind.Absolute));
            this.width = 100;
            this.height = 100;
            this.locationX = locationX;
            this.locationY = locationY;
            this.Width = 100;
            this.Height = 100;
        }
        public void heal(int add)
        {
            this.health = this.health + add >= 100 ? 100 : this.health+add;
        }
        public void keyDownListener(string which)
        {
            clicking.Add(which);
            this.updateKey();
        }
        public void keyUpListener(string which)
        {
            clicking.Remove(which);
            this.updateKey();
        }
        private void updateKey()
        {
            this.useSupply();
            int speedMultiplication = this.supplies.Find(e => e.type == Types.FAST).status ? 2:1 ;
            this.SpeedY = 0;
            this.SpeedX = (clicking.Contains(keys[0]) && !clicking.Contains(keys[1]) ? -5: clicking.Contains(keys[1]) && !clicking.Contains(keys[0])? 5:0) * speedMultiplication;
            this.SpeedY = (clicking.Contains(keys[3]) && !clicking.Contains(keys[2]) ? -5: clicking.Contains(keys[2]) && !clicking.Contains(keys[3])? 5:0) * speedMultiplication;
        }
        public override void move()
        {
            if (this.LocationX + SpeedX >= Game.maxX || this.LocationX + this.SpeedX <= 0) SpeedX = 0;
            if (this.LocationY + SpeedY >= Game.maxY-50 || this.LocationY + this.SpeedY <= 0) SpeedY = 0;
            //if(this.LocationY + SpeedY >= this.maxY) Debugger.Break();
            try
            {
                base.move();
            }
            catch (OutOfTheWindowException)
            {
                this.locationX = Game.maxX / 2;
                this.locationY = Game.maxY / 2;
            }
        }
        public override void shoot()
        {
            //if (this.Health <= 10) this.heal(100);
            this.counter += 1;
            if (this.counter  == 10)
            {
                this.counter = 0;
                this.isShootable = true;
            }
            if (clicking.Contains(keys[4]) && this.isShootable)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    bullets.Add(this.supplies.Find(e => e.type == Types.STRONG).status ? new StrongBullet(this) : new SimpleBullet(this));
                });
                this.isShootable = false;
            }
        }
        public void takeDamage(int health)
        {
            if (!this.supplies.Find(e => e.type == Types.GHOST).status) this.health -= health;
            //if (this.Health <= 10) this.heal(100);
            //if (this.health <= 0) this.destroy();
        }
        public void moveBullets()
        {
            foreach (Bullet bullet in this.bullets.ToArray())
            {
                if (bullet == null) continue;
                try
                {
                    bullet.move();
                }
                catch (OutOfTheWindowException)
                {
                    bullet.remove();
                }
            }
        }
        public void addSupply(Supply supply)
        {
            //var foundItem = supplies.FirstOrDefault(e => e.type == Types.HEAL);
            SupplyController supplyCont = supplies.Find(e => e.type == supply.Type);
            supplyCont.add(supply);
        }
        private void useSupply()
        {
            if (clicking.Contains(keys[5]))
            {
                supplies.Find(e => e.type == Types.FAST).use();
            }
            if (clicking.Contains(keys[6]))
            {
                supplies.Find(e => e.type == Types.HEAL).use();
            }
            if (clicking.Contains(keys[8]))
            {
                supplies.Find(e => e.type == Types.GHOST).use();
            }
            if (clicking.Contains(keys[7]))
            {
                supplies.Find(e => e.type == Types.STRONG).use();
            }
        }
    }
}
