using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SpaceWar3.bullet;
using SpaceWar3.enemy;
using SpaceWar3.enemy.boss;
using SpaceWar3.supply;
using WpfAnimatedGif;

namespace SpaceWar3
{
    class GUIControl
    {
        private GameGUI form;
        private SpaceShip ship;
        private Canvas panel;
        private Canvas starPanel;
        private System.Threading.Timer timer;
        List<Enemy> enemies;
        private byte timeCounter = 0;
        private Star[] stars = new Star[120];
        private Game game;
        private List<Bullet> notOwnerBullets;
        private Canvas topBar;
        private ProgressBar progressBar;
        private Label stopLabel;
        private Label scoreLabel;
        private List<Supply> supplies;
        private Image[] imgs = new Image[5];
        private List<Boss> boss;
        private Canvas[] supplyBox = new Canvas[4];
        private Label[] supplyLabels = new Label[4];
        private Asteroid asteroid;
        public GUIControl(GameGUI form, SpaceShip ship,List<Bullet> notOwnerBullets, List<Enemy> enemies,List<Supply> supplies,List<Boss> boss,Asteroid asteroid,Game game) 
        {
            this.game = game;
            this.form = form;
            this.ship = ship;
            this.supplies = supplies;
            this.boss = boss;
            this.panel = this.form.guiGrid;
            this.starPanel = this.form.starGrid;
            this.notOwnerBullets = notOwnerBullets;
            this.enemies = enemies;
            this.createStars();
            this.initialize();
            timer = new System.Threading.Timer(this.updateTimer, null, 0, 1000/144);
            this.form.KeyDown += this.game.keyDownListener;
            this.form.KeyUp += this.game.keyUpListener;
            this.asteroid = asteroid;
        }
        private void updateTimer(object state) 
        {
            this.update();
            this.timeCounter++;
            if(this.timeCounter == 1)
            {
                this.timeCounter = 0;
                this.updateStars();
            }
        }
        public void timerControl(bool status)
        {
            if (!status){
                this.timer.Change(Timeout.Infinite, Timeout.Infinite);
                this.panel.Children.Add(this.stopLabel);
            }
            else{
                this.timer.Change(0, 1000 / 64);
                this.panel.Children.Remove(this.stopLabel);
            }
        }
        public void gameOver()
        {
            this.timer.Change(Timeout.Infinite, Timeout.Infinite);
            this.addGameOver();
        }
        private void addGameOver()
        {
            Application.Current.Dispatcher.Invoke(() => {
                Label usernameScore = new Label();
                usernameScore.Content = $"{this.game.Username}: {Game.Score}";
                usernameScore.FontSize = 30;
                usernameScore.Foreground = new SolidColorBrush(Colors.White);
                Label gameOverLabel = new Label();
                gameOverLabel.Content = "GAME OVER !!!";
                gameOverLabel.FontSize = 50;
                gameOverLabel.Foreground = new SolidColorBrush(Colors.DarkRed);
                Panel.SetZIndex(gameOverLabel, 10);
                Panel.SetZIndex(usernameScore, 10);
                Canvas.SetLeft(gameOverLabel, Game.maxX / 2 - 100);
                Canvas.SetTop(gameOverLabel, Game.maxY / 2 - 50);
                Canvas.SetLeft(usernameScore, Game.maxX / 2 - 100);
                Canvas.SetTop(usernameScore, Game.maxY / 2 + 50);
                this.panel.Children.Add(gameOverLabel);
                this.panel.Children.Add(usernameScore);

            });
        }
        private void stopLabelInitialize()
        {
            this.stopLabel = new Label();
            this.stopLabel.Content = "PAUSED";
            this.stopLabel.FontSize = 50;
            this.stopLabel.Foreground = new SolidColorBrush(Colors.White);
        }
        private void progressBarInitialize()
        {
            this.progressBar = new ProgressBar();
            this.progressBar.Width = 100;
            this.progressBar.Height = 10;
            this.progressBar.Foreground = new SolidColorBrush(Colors.Red);
        }
        private void supplyBarInitialize()
        {
            for (int i = 0; i < 4; i++)
            {
                string imgSource;
                switch (i)
                {
                    case 0:
                        imgSource = "pack://application:,,,/Resources/img/" + ConfigurationManager.AppSettings["FAST_SUPPLY_IMAGE"];
                        break;
                    case 1:
                        imgSource = "pack://application:,,,/Resources/img/" + ConfigurationManager.AppSettings["HEAL_SUPPLY_IMAGE"];
                        break;
                    case 2:
                        imgSource = "pack://application:,,,/Resources/img/" + ConfigurationManager.AppSettings["SHIELD_SUPPLY_IMAGE"];
                        break;
                    case 3:
                        imgSource = "pack://application:,,,/Resources/img/" + ConfigurationManager.AppSettings["STRONG_SUPPLY_IMAGE"];
                        break;
                    default:
                        imgSource = "pack://application:,,,/Resources/img/" + ConfigurationManager.AppSettings["FAST_SUPPLY_IMAGE"];
                        break;
                }
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(imgSource, UriKind.Absolute));
                img.Width = 25;
                img.Height = 25;
                this.supplyBox[i] = new Canvas();
                this.supplyBox[i].Children.Add(img);
                Label label = new Label();
                label.Content = "0";
                this.supplyBox[i].Children.Add(label);
                label.Foreground = Brushes.White;
                Canvas.SetLeft(this.supplyBox[i], 150 + i * 50);
                Canvas.SetTop(this.supplyBox[i], 10);
                Canvas.SetLeft(label, 30);
                Canvas.SetTop(label, 0);
                this.panel.Children.Add(this.supplyBox[i]);
                this.supplyLabels[i] = label;
            }
        }
        private void topBarInitialize()
        {
            this.progressBarInitialize();
            this.topBar = new Canvas();
            this.topBar.Width = this.panel.Width;
            this.topBar.Height = 100;
            this.topBar.Children.Add(this.progressBar);
            this.scoreLabel = new Label();
            this.scoreLabel.Foreground = new SolidColorBrush(Colors.Blue);
            this.scoreLabel.Content = "Score: 0";
            this.scoreLabel.FontSize = 18;
            this.scoreLabel.Padding = new Thickness(0);
            this.topBar.Children.Add(this.scoreLabel);
            Canvas.SetRight(this.scoreLabel, 50);
            Canvas.SetTop(this.scoreLabel,-3);
            this.supplyBarInitialize();
        }
    
        private void initialize()
        {
            this.topBarInitialize();
            this.stopLabelInitialize();
            Canvas.SetLeft(this.ship, this.ship.LocationXWrite);
            Canvas.SetTop(this.ship, this.ship.LocationYWrite);
            Canvas.SetLeft(this.topBar, 10);
            Canvas.SetTop(this.topBar, 10);
            //Image img = new Image();
            //img.Source = new BitmapImage(new Uri("C:\\Users\\polis\\source\\repos\\SpaceWar3\\SpaceWar3\\img\\faster.png"));
            //img.Width = 40;
            //img.Height = 40;
            //Canvas.SetLeft(img, 500);
            //Canvas.SetTop(img, 500);
            this.panel.Children.Add(this.ship);
            //this.panel.Children.Add(img);
            this.panel.Children.Add(this.topBar);
            //for (int i = 4; i>=0;i--)
            //{
            //    imgs[i] = new Image();
            //    imgs[i].Source = new BitmapImage(new Uri("C:\\Users\\polis\\source\\repos\\SpaceWar3\\SpaceWar3\\img\\" + i + ".png"));
            //    imgs[i].Width = (i+1) * 10;
            //    imgs[i].Height = (i+1) * 10;
            //    Canvas.SetLeft(imgs[i], this.ship.LocationX- i * 10 / 2);
            //    Canvas.SetTop(imgs[i], this.ship.LocationY- i * 10 / 2);
            //    this.panel.Children.Add(imgs[i]);
            //}
        }
        private void supplyBarUpdate()
        {
            for (int i = 0; i < 4; i++)
            {
                int count = 0;
                switch (i)
                {
                    case 0:
                        count = this.ship.supplies.Find(e => e.type == Types.FAST).count;
                        break;
                    case 1:
                        count = this.ship.supplies.Find(e => e.type == Types.HEAL).count;
                        break;
                    case 2:
                        count = this.ship.supplies.Find(e => e.type == Types.GHOST).count;
                        break;
                    case 3:
                        count = this.ship.supplies.Find(e => e.type == Types.STRONG).count;
                        break;
                    default:
                        count = this.ship.supplies.Find(e => e.type == Types.FAST).count;
                        break;
                }
                this.panel.Dispatcher.Invoke(() =>
                {
                    this.supplyLabels[i].Content = count;
                });
            }
        }
        private void suppliesUpdate()
        {
            List<Supply> forRemove = new List<Supply>();
            foreach (Supply supply in this.supplies.ToArray())
            {
                this.panel.Dispatcher.Invoke(() =>
                {
                    if (!supply.IsFinished)
                    {
                        if (supply.IsShowing == false)
                        {
                            this.panel.Children.Add(supply);
                            supply.IsShowing = true;
                        }
                        Canvas.SetLeft(supply, supply.LocationXWrite);
                        Canvas.SetTop(supply, supply.LocationYWrite);
                    }
                    else
                    {
                        this.panel.Children.Remove(supply);
                        forRemove.Add(supply);
                    }
                });
            }
            game.removeSupplies(forRemove);
        }
        private void bossUpdate()
        {
            List<Boss> forRemove = new List<Boss>();
            List<Enemy> forRemoveEnemy = new List<Enemy>();
            foreach (Boss boss in this.boss.ToArray())
            {
                this.panel.Dispatcher.Invoke(() =>
                {
                    if (!boss.IsFinished)
                    {
                        if (boss.IsShowing == false)
                        {
                            this.panel.Children.Add(boss);
                            boss.IsShowing = true;
                        }
                        Canvas.SetLeft(boss, boss.LocationXWrite);
                        Canvas.SetTop(boss, boss.LocationYWrite);
                    }
                    else
                    {
                        this.panel.Children.Remove(boss);
                        forRemove.Add(boss);
                    }
                    foreach (Bullet bullet in boss.Bullets.ToArray())
                    {
                        bulletUpdate(bullet, boss);
                    }
                });
                this.panel.Dispatcher.Invoke(() =>
                {
                    foreach (Enemy enemy in boss.Enemies.ToArray())
                    {
                        if (enemy.IsFinished)
                        {
                            this.panel.Children.Remove(enemy);
                            forRemoveEnemy.Add(enemy);
                            continue;
                        }
                        if (enemy.IsShowing == false) this.panel.Children.Add(enemy);
                        enemy.IsShowing = true;
                        Canvas.SetLeft(enemy, enemy.LocationXWrite);
                        Canvas.SetTop(enemy, enemy.LocationYWrite);
                        foreach (Bullet bullet in enemy.Bullets.ToArray())
                        {
                            bulletUpdate(bullet, enemy);
                        }
                    }
                });
            }
            this.game.removeBoss(forRemove,forRemoveEnemy);
        }
        private void update() 
        {
            this.shipUpdate();
            this.enemyUpdate();
            this.notOwnerBulletsUpdate();
            this.suppliesUpdate();
            this.bossUpdate();
            this.supplyBarUpdate();
            this.asteroidUpdate();
            //this.panel.Dispatcher.Invoke(() =>{
            //    Canvas.SetLeft(this.ship, this.ship.LocationXWrite);
            //    Canvas.SetTop(this.ship, this.ship.LocationYWrite);
            //    foreach (Bullet bullet in this.ship.Bullets.ToArray())
            //    {
            //        if (bullet.IsShowing == false) this.panel.Children.Add(bullet);
            //        bullet.IsShowing = true;
            //        Canvas.SetLeft(bullet, bullet.LocationXWrite);
            //        Canvas.SetTop(bullet, bullet.LocationYWrite);
            //    }
            //    foreach (Enemy enemy in game.getEnemies())
            //    {
            //        if (enemy.IsShowing == false) this.panel.Children.Add(enemy);
            //        enemy.IsShowing = true;
            //        Canvas.SetLeft(enemy, enemy.LocationXWrite);
            //        Canvas.SetTop(enemy, enemy.LocationYWrite);
            //        foreach (Bullet bullet in enemy.Bullets)
            //        {
            //            if (bullet.IsShowing == false) this.panel.Children.Add(bullet);
            //            bullet.IsShowing = true;
            //            Canvas.SetLeft(bullet, bullet.LocationXWrite);
            //            Canvas.SetTop(bullet, bullet.LocationYWrite);
            //        }
            //    }          
            //});
        }
        private void asteroidUpdate()
        {
            this.panel.Dispatcher.Invoke(() =>
            {
                Canvas.SetLeft(this.asteroid,this.asteroid.LocationXWrite);
                Canvas.SetTop(this.asteroid, this.asteroid.LocationYWrite);
                if (!this.asteroid.IsShowing)
                {
                    this.panel.Children.Add(this.asteroid);
                    this.asteroid.IsShowing = true;
                }
            });

        }
        private void notOwnerBulletsUpdate()
        {
            foreach (Bullet bullet in this.notOwnerBullets.ToArray())
            {

                this.panel.Dispatcher.Invoke(() =>
                {
                    if (bullet.IsFinished)
                    {
                        this.panel.Children.Remove(bullet);
                        this.notOwnerBullets.Remove(bullet);
                    }
                    else
                    {
                        if (!bullet.IsShowing)
                        {
                            this.panel.Children.Add(bullet);
                            bullet.IsShowing = true;
                        }
                        Canvas.SetLeft(bullet, bullet.LocationXWrite);
                        Canvas.SetTop(bullet, bullet.LocationYWrite);
                    }
                });
            }
        }
        private void enemyUpdate()
        {
            this.panel.Dispatcher.Invoke(() =>
            {
                List<Enemy> forRemove = new List<Enemy>();
                foreach (Enemy enemy in this.enemies)
                {
                    if (enemy.IsFinished)
                    {
                        this.panel.Children.Remove(enemy);
                        forRemove.Add(enemy);
                        continue;
                    }
                    if (enemy.IsShowing == false) this.panel.Children.Add(enemy);
                    enemy.IsShowing = true;
                    Canvas.SetLeft(enemy, enemy.LocationXWrite);
                    Canvas.SetTop(enemy, enemy.LocationYWrite);
                    foreach (Bullet bullet in enemy.Bullets.ToArray())
                    {
                        bulletUpdate(bullet,enemy);
                    }
                }
                this.game.removeEnemies(forRemove);
            });
        }

        private void shipUpdate()
        {
            this.panel.Dispatcher.Invoke(() =>
            {
                Canvas.SetLeft(this.ship, this.ship.LocationXWrite);
                Canvas.SetTop(this.ship, this.ship.LocationYWrite);
                this.progressBar.Value = this.ship.Health;
                this.scoreLabel.Content = "Score: "+Game.Score;
                foreach (Bullet bullet in this.ship.Bullets.ToArray())
                {
                    bulletUpdate(bullet,this.ship);
                }
            });

        }
        private void bulletUpdate(Bullet bullet,Ship ship)
        {
            if (bullet == null) return;
            Canvas.SetLeft(bullet, bullet.LocationXWrite);
            Canvas.SetTop(bullet, bullet.LocationYWrite);
            if (bullet.IsFinished)
            {
                this.panel.Children.Remove(bullet);
                ship.Bullets.Remove(bullet);
                return;
            }
            if (!bullet.IsShowing)
            {
                this.panel.Children.Add(bullet);
                bullet.IsShowing = true;
            }
        }
        private void updateStars()
        {
            foreach (Star star in this.stars)
            {
                star.setY();
                this.panel.Dispatcher.Invoke(() =>
                {
                    Canvas.SetLeft(star, star.x);
                    Canvas.SetTop(star, star.y);
                });
            }
        }

        private void createStars()
        {
            Star.maxX = (int)this.panel.Width;
            Star.maxY = (int)this.panel.Height;
            Random rand = new Random();
            for (int i = 0; i < 120; i++)
            {
                Star star = new Star();
                star.Foreground = rand.Next(0, 10) == 0? rand.Next(0, 10)>=5?Brushes.CadetBlue:Brushes.IndianRed : Brushes.White;
                this.stars[i] = star;
                Canvas.SetLeft(star, star.y);
                Canvas.SetTop(star, star.x);
                Canvas.SetZIndex(star, 1);
                this.starPanel.Children.Add(star);
            }
        }
        public void quickClose()
        {
            this.StopTimer();
        }
        private void StopTimer()
        {
            this.timer.Dispose();
        }
        public void shutdown()
        {
            this.quickClose();
        }
    }
}
