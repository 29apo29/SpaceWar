using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Configuration;
using SpaceWar3.supply;
using SpaceWar3.enemy;
using SpaceWar3.bullet;
using SpaceWar3.enemy.boss;

namespace SpaceWar3
{
    public static class Controller
    {
        public static int BOSS_SPAWN_SCORE = Int16.Parse(ConfigurationManager.AppSettings["BOSS_SPAWN_SCORE"]);
        public static int ASTEROID_SPAWN_TIME = Int16.Parse(ConfigurationManager.AppSettings["ASTEROID_SPAWN_TIME"]);
        public static int ENEMY_SPAWN_TIME_FIRST = Int16.Parse(ConfigurationManager.AppSettings["ENEMY_SPAWN_TIME_FIRST"]);
        public static int ENEMY_SPAWN_TIME_SECOND = Int16.Parse(ConfigurationManager.AppSettings["ENEMY_SPAWN_TIME_SECOND"]);
        public static int ENEMY_SPAWN_TIME_THIRD = Int16.Parse(ConfigurationManager.AppSettings["ENEMY_SPAWN_TIME_THIRD"]);
        public static int ENEMY_SPAWN_TIME_FOURTH = Int16.Parse(ConfigurationManager.AppSettings["ENEMY_SPAWN_TIME_FOURTH"]);
        public static void refresh()
        {
            BOSS_SPAWN_SCORE = Int16.Parse(ConfigurationManager.AppSettings["BOSS_SPAWN_SCORE"]);
            ASTEROID_SPAWN_TIME = Int16.Parse(ConfigurationManager.AppSettings["ASTEROID_SPAWN_TIME"]);
            ENEMY_SPAWN_TIME_FIRST = Int16.Parse(ConfigurationManager.AppSettings["ENEMY_SPAWN_TIME_FIRST"]);
            ENEMY_SPAWN_TIME_SECOND = Int16.Parse(ConfigurationManager.AppSettings["ENEMY_SPAWN_TIME_SECOND"]);
            ENEMY_SPAWN_TIME_THIRD = Int16.Parse(ConfigurationManager.AppSettings["ENEMY_SPAWN_TIME_THIRD"]);
            ENEMY_SPAWN_TIME_FOURTH = Int16.Parse(ConfigurationManager.AppSettings["ENEMY_SPAWN_TIME_FOURTH"]);
        }
    }
    class Game
    {
        private SpaceShip ship;
        private System.Threading.Timer timer;
        private GUIControl gui;
        private byte tickCounter = 0;
        private int counter = 0;
        List<Enemy> enemies = new List<Enemy>();
        public static int maxX;
        public static int maxY;
        private CollisionDetector collisionDetector;
        private List<Bullet> notOwnerBullet = new List<Bullet>();
        private List<Supply> supplies = new List<Supply>();
        private static int score = 0;
        private string pauseKey = ConfigurationManager.AppSettings["PAUSE"];
        private bool timerStatus = true;
        private List<Boss> boss = new List<Boss>();
        private int bossCounter = 0;
        private byte asteroidCounter = 0;
        private readonly Asteroid asteroid = new Asteroid(0);
        private byte bulletCounter = 0;
        private bool isGameOver = false;
        private string username;
        private GameGUI form;

        public static int Score
        {
            get => score; 
            set => score += value;
        }
        public string Username
        {
            get => this.username;
        }
        public Game(GameGUI form, string username) 
        {
            this.username = username;
            this.form = form;
        }
        public void startGame()
        {
            this.ship = new SpaceShip((int)form.Width / 2, (int)form.Height - 200);
            this.gui = new GUIControl(form, this.ship, this.notOwnerBullet, this.enemies, this.supplies, this.boss, this.asteroid, this);
            this.timer = new System.Threading.Timer(this.updateTimer, null, 0, 1000 / 64);
            this.collisionDetector = new CollisionDetector(this.enemies, this.ship, this.notOwnerBullet, this.supplies, this.boss, this.asteroid);
            form.Closed += Window_Closed;
            maxX = (int)form.guiGrid.Width;
            maxY = (int)form.guiGrid.Height;
            score = 0;
        }
        public void keyDownListener(object sender, KeyEventArgs e)
        {
            this.ship.keyDownListener(e.Key.ToString());
            if (e.Key.ToString() == this.pauseKey && !this.isGameOver) this.timerControl();
        }
        public void keyUpListener(object sender, KeyEventArgs e)
        {
            this.ship.keyUpListener(e.Key.ToString());
        }
        private void timerControl()
        {
            this.timerStatus = !this.timerStatus;
            if (!this.timerStatus) this.timer.Change(Timeout.Infinite, Timeout.Infinite);
            else this.timer.Change(0, 1000 / 64);
            this.gui.timerControl(this.timerStatus);
        }
        private void updateTimer(object state)
        {
            this.update();
            this.tickCounter++;
            if (this.tickCounter == 64)
            {
                this.tickCounter = 0;
                this.counter++;
                this.asteroidCounter++;
                this.bulletCounter++;
            }
            this.addEnemy();
            this.updateSupply();

        }
        private void asteroidUpdate()
        {
            if(this.tickCounter == 0 && this.asteroidCounter >= Controller.ASTEROID_SPAWN_TIME && !this.asteroid.IsActive)
            {
                Random rand = new Random();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.asteroid.reCreate((byte)rand.Next(1, 6));
                });
                this.asteroidCounter = 0;
            }
            this.asteroid.move();
        }
        private void addSupply(int x, int y)
        {
            Random rand = new Random();
            Application.Current.Dispatcher.Invoke(() =>
            {
                int num = rand.Next(0, 4);
                Supply sup = num == 0 ? new FastSupply(this.ship, x, y) : num == 1?  new HealSupply(this.ship, x, y):num == 2?new StrongSupply(this.ship,x,y):new GhostSupply(this.ship, x, y);
                this.supplies.Add(sup);
            });
        }
        private void updateSupply()
        {
            foreach (Supply supply in this.supplies.ToArray())
            {
                try
                {
                    supply.move();
                }
                catch(OutOfTheWindowException e)
                {
                    supply.removeFromUI();
                }
            }
        }
        private void addEnemy()
        {
            int counterS = bossCounter == 0 ? Controller.ENEMY_SPAWN_TIME_FIRST :
                bossCounter == 1 ? Controller.ENEMY_SPAWN_TIME_SECOND :
                bossCounter == 2 ? Controller.ENEMY_SPAWN_TIME_THIRD : Controller.ENEMY_SPAWN_TIME_FOURTH;
            if (this.tickCounter == 0 && this.counter % counterS == 0 && this.boss.Count == 0)
            {
                Random rand = new Random();
                int status = rand.Next(0, (bossCounter+1) >=3 ? 3 : bossCounter + 1);
                bool which = rand.Next(0, 10) < 5;
                bool which2 = rand.Next(0, 10) < 5;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Enemy enemy = status == 0 ?
                        new BasicEnemy(which ? which2 ? 0 : maxX : rand.Next(0, maxX), which ? rand.Next(0, maxY / 2) : 0, this.ship)
                        :status ==1? new FastEnemy(which ? which2 ? 0 : maxX : rand.Next(0, maxX), which ? rand.Next(0, maxY / 2) : 0, this.ship)
                        : new StrongEnemy(which ? which2 ? 0 : maxX : rand.Next(0, maxX), which ? rand.Next(0, maxY / 2) : 0, this.ship);
                    this.enemies.Add(enemy);
                });
            }
        }

        public void removeEnemies(List<Enemy> enemies)
        {
            Random rand = new Random();
            foreach (Enemy enemy in enemies.ToArray())
            {
                if (rand.Next(0, 100) + 1 <= enemy.Chance) this.addSupply(enemy.LocationX,enemy.LocationY);
                score += enemy.Score;
                enemy.remove(this.notOwnerBullet);
                this.enemies.Remove(enemy);
            }
        }
        public void removeSupplies(List<Supply> supplies)
        {
            foreach (Supply supply in supplies.ToArray())
            {
                this.supplies.Remove(supply);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.quickClose();
        }
        private void quickClose()
        {
            this.StopTimer();
            this.gui.quickClose();
        }
        private void StopTimer()
        {
            this.timer.Dispose();
        }
        private void update()
        {
            this.isGameOverControl();
            if (this.isGameOver) return;
            this.notOwnerBulletUpdate();
            this.ship.move();
            this.ship.shoot();
            this.ship.moveBullets();
            this.enemyUpdate();
            this.bossUpdate();
            this.asteroidUpdate();
            this.checkCollisions();
        }
        private void checkCollisions()
        {
            this.collisionDetector.detect();
        }
        private void isGameOverControl()
        {
            this.isGameOver = this.ship.Health <= 0;
            if (!this.isGameOver) return;
            this.timer.Change(Timeout.Infinite, Timeout.Infinite);
            this.gui.gameOver();
            ScoreBoard sb = new ScoreBoard(this.username,score);
            sb.add();
        }
        private void bossUpdate()
        {
            foreach (Boss boss in this.boss.ToArray())
            {
                boss.update();
                boss.shoot();
                foreach (Enemy enemy in boss.Enemies.ToArray())
                {  
                    enemy.update();
                    enemy.shoot();
                }
            }
            if (score/Controller.BOSS_SPAWN_SCORE != 0 && score/ Controller.BOSS_SPAWN_SCORE >= this.bossCounter+1 && this.boss.Count == 0)
            {
                this.bossCounter++;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Boss boss = this.bossCounter % 3 == 1? new SimpleBoss(this.ship) : this.bossCounter % 3 == 2 ? new FastBoss(this.ship) : new StrongBoss(this.ship);
                    this.boss.Add(boss);
                });
            }
        }
        public void removeBoss(List<Boss> bosss, List<Enemy> enemiess)
        {
            Random rand = new Random();
            foreach (Boss boss in bosss.ToArray())
            {
                if (rand.Next(0, 100) + 1 <= boss.Chance) this.addSupply(boss.LocationX, boss.LocationY);
                this.boss.Remove(boss);
                boss.remove(this.notOwnerBullet, this.enemies);
                score += boss.Score * (this.bossCounter/3+1);
            }
            foreach (Boss boss in this.boss.ToArray())
            {
                foreach (Enemy enemy in enemiess)
                {
                    if (rand.Next(0, 100) + 1 <= enemy.Chance) this.addSupply(enemy.LocationX, enemy.LocationY);
                    boss.Enemies.Remove(enemy);
                    enemy.remove(this.notOwnerBullet);
                    score += enemy.Score;
                }
            }
        }
        private void notOwnerBulletUpdate()
        {   
            foreach (Bullet bullet in this.notOwnerBullet.ToArray())
            {
                if (bullet.IsFinished)
                {
                    this.notOwnerBullet.Remove(bullet);
                }
                if(bullet is HomingBullet)
                {
                    ((HomingBullet)bullet).updateSpeeds();
                }
                try
                {
                    bullet.move();
                }
                catch (OutOfTheWindowException e)
                {
                    bullet.remove();
                }
            }
        }
        private void enemyUpdate()
        {
            foreach (Enemy enemy in enemies.ToArray())
            {
                enemy.update();
                enemy.shoot();
            }
        }
        public void shutdown()
        {
            this.gui.shutdown();
            this.quickClose();
        }
    }
}
