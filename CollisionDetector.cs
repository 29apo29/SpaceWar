using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceWar3.bullet;
using SpaceWar3.enemy;
using SpaceWar3.enemy.boss;
using SpaceWar3.supply;

namespace SpaceWar3
{
    internal class CollisionDetector
    {
        private SpaceShip ship;
        private List<Enemy> enemies;
        private List<Bullet> notOwnerBullets;
        private List<Supply> supplies;
        private List<Boss> boss;
        private Asteroid asteroid;

        public List<Enemy> Enemies
        {
            set => this.enemies = value;
        }
        public CollisionDetector(List<Enemy> enemies, SpaceShip ship, List<Bullet> notOwnerBullets, List<Supply> supplies, List<Boss> boss,Asteroid asteroid) 
        {
            this.enemies = enemies;
            this.ship = ship;
            this.notOwnerBullets = notOwnerBullets;
            this.supplies = supplies;
            this.boss = boss;
            this.asteroid = asteroid;
        }
        public void detect()
        {
            shipDetect();
            enemiesDetect();
            notOwnerBulletsDetect();
            supplyDetect();
            bossDetect();
            bossEnemyDetect();
            asteroidDetect();
        }
        private void asteroidDetect()
        {
            if (!asteroid.IsActive) return;
            foreach (Enemy enemy in this.enemies.ToArray())
            {
                foreach (Bullet bullet in enemy.Bullets)
                {
                    if (this.asteroidAndSubject(this.asteroid, bullet))
                    {
                        bullet.onHit();
                    }
                }
                if (this.asteroidAndSubject(this.asteroid,enemy))
                {
                    enemy.takeDamage(this.asteroid.Radius / 5);
                    enemy.updateLocation((int)(this.asteroid.SpeedX / 3 * this.asteroid.Radius / 2.5), (int)(this.asteroid.SpeedY / 3 * this.asteroid.Radius / 2.5));
                }
            }
            foreach (Bullet bullet in this.ship.Bullets.ToArray())
            {
                if (this.asteroidAndSubject(this.asteroid, bullet))
                {
                    bullet.onHit();
                }
            }
            if (this.asteroidAndSubject(this.asteroid, this.ship) && !this.ship.supplies.Find(e=>e.type == Types.GHOST).status)
            {
                this.ship.takeDamage(this.asteroid.Radius/5);
                this.ship.updateLocation((int)(this.asteroid.SpeedX / 3 * this.asteroid.Radius / 2.5) , (int)(this.asteroid.SpeedY / 3 * this.asteroid.Radius / 2.5));
            }
            foreach (Boss boss in this.boss)
            {
                foreach (Enemy enemy in boss.Enemies)
                {
                    foreach (Bullet bullet in enemy.Bullets)
                    {
                        if (this.asteroidAndSubject(this.asteroid, bullet))
                        {
                            bullet.onHit();
                        }
                    }
                    if (this.asteroidAndSubject(this.asteroid, enemy))
                    {
                        enemy.takeDamage(this.asteroid.Radius / 5);
                        enemy.updateLocation((int)(this.asteroid.SpeedX / 3 * this.asteroid.Radius / 2.5), (int)(this.asteroid.SpeedY / 3 * this.asteroid.Radius / 2.5));
                    }
                }
                if (this.asteroidAndSubject(this.asteroid, boss))
                {
                    boss.takeDamage(this.asteroid.Radius / 5);
                    boss.updateLocation((int)(this.asteroid.SpeedX / 3 * this.asteroid.Radius / 2.5), (int)(this.asteroid.SpeedY / 3 * this.asteroid.Radius / 2.5));
                }
            }
        }
        private void bossDetect()
        {
            foreach (Boss boss in this.boss.ToArray())
            {
                bulletsDetect(boss.Bullets, this.ship.Bullets);
                bulletsDetectForSpaceShip(boss.Bullets);
                if (this.shipAndShip(this.ship, boss) && !this.ship.supplies.Find(e => e.type == Types.GHOST).status)
                {
                    int h1 = boss.Health;
                    boss.takeDamage(this.ship.Health);
                    this.ship.takeDamage(h1);
                }
                foreach (Bullet bullet in this.ship.Bullets.ToArray())
                {
                    if (this.shipAndBullet(boss, bullet))
                    {
                        bullet.onHit(boss);
                    }
                }
                foreach (Enemy enemy in this.enemies.ToArray())
                {
                    if (this.shipAndShip(boss, enemy))
                    {
                        int h1 = enemy.Health;
                        enemy.takeDamage(boss.Health);
                        boss.takeDamage(h1);
                    }
                }
            }
        }
        private void bossEnemyDetect()
        {
            foreach (Boss boss in this.boss.ToArray())
            {
                foreach (Enemy enemy in boss.Enemies.ToArray())
                {
                    bulletsDetect(enemy.Bullets, this.ship.Bullets);
                    bulletsDetectForSpaceShip(enemy.Bullets);
                    if (this.shipAndShip(this.ship, enemy) && !this.ship.supplies.Find(e => e.type == Types.GHOST).status)
                    {
                        int h1 = enemy.Health;
                        enemy.takeDamage(this.ship.Health);
                        this.ship.takeDamage(h1);
                    }
                    foreach (Bullet bullet in this.ship.Bullets.ToArray())
                    {
                        if (this.shipAndBullet(enemy, bullet))
                        {
                            bullet.onHit(enemy);
                        }
                    }
                    foreach (Enemy enemy2 in boss.Enemies.ToArray())
                    {
                        if (enemy == enemy2) continue;
                        if (this.shipAndShip(enemy, enemy2))
                        {
                            int h1 = enemy.Health;
                            enemy.takeDamage(enemy2.Health);
                            enemy2.takeDamage(h1);
                        }
                    }
                }
            }
        }

        private void supplyDetect() 
        {
            foreach (Supply supply in this.supplies.ToArray())
            {
                if (this.check(this.ship, supply,50))
                {
                    if (!supply.IsFinished)
                    {
                        supply.removeFromUI();
                        this.ship.addSupply(supply);
                    }
                }
            }
        }
        private void notOwnerBulletsDetect()
        {
            this.bulletsDetect(this.notOwnerBullets,this.ship.Bullets);
            foreach (Bullet bullet in this.notOwnerBullets.ToArray())
            {
                if (this.shipAndBullet(this.ship, bullet))
                {
                    bullet.onHit(this.ship);
                }
            }
        }
        private void shipDetect()
        {
            foreach (Enemy enemy in this.enemies.ToArray())
            {
                if(this.shipAndShip(this.ship, enemy) && !this.ship.supplies.Find(e=>e.type==Types.GHOST).status)
                {
                    int h1 = enemy.Health;
                    enemy.takeDamage(this.ship.Health);
                    this.ship.takeDamage(h1);
                }
                bulletsDetect(this.ship.Bullets, enemy.Bullets);
            }
        }
        private void enemiesDetect()
        {
            foreach (Enemy enemy1 in this.enemies.ToArray())
            {
                foreach (Enemy enemy2 in this.enemies.ToArray())
                {
                    if (enemy1 == enemy2) continue;
                    if(this.shipAndShip(enemy1, enemy2))
                    {
                        int h1 = enemy1.Health;
                        enemy1.takeDamage(enemy2.Health);
                        enemy2.takeDamage(h1);
                    }
                    bulletsDetect(enemy1.Bullets, enemy2.Bullets);
                }
                this.bulletsDetectForSpaceShip(enemy1.Bullets);
            }
        }
        private void bulletsDetectForSpaceShip(List<Bullet> bullets)
        {
            foreach (Bullet bullet in bullets.ToArray())
            {
                if (this.shipAndBullet(this.ship, bullet) && !this.ship.supplies.Find(e => e.type == Types.GHOST).status)
                {
                    bullet.onHit(this.ship);
                }
            }
        }
        private void bulletsDetect(List<Bullet> bullets1, List<Bullet> bullets2)
        {
            foreach (Bullet bullet1 in bullets1.ToArray())
            {
                if (bullet1 == null) continue;
                foreach (Bullet bullet2 in bullets2.ToArray())
                {
                    if (bullet2 == null) continue;
                    if ((bullet1 == bullet2) || (bullet1.Owner is Enemy && bullet2.Owner is Enemy)) continue;
                    if (this.bulletAndBullet(bullet1, bullet2) && !bullet1.IsFinished && !bullet2.IsFinished)
                    {
                        bullet1.remove();
                        bullet2.remove();
                    }
                }
                if (bullet1.Owner is Enemy && !bullet1.IsExplosive) continue;
                foreach (Enemy enemy in this.enemies.ToArray())
                {
                    if (this.shipAndBullet(enemy, bullet1))
                    {
                        bullet1.onHit(enemy);
                    }
                }
            }
        }
        private bool shipAndBullet(Ship ship, Bullet bullet)
        {
            if(bullet == null) return false;
            if(ship.Bullets.Contains(bullet)) return false;
            return this.check(ship, bullet, ship is Boss?80:50);
        }
        private bool shipAndShip(Ship ship1, Ship ship2)
        {
            return this.check(ship1,ship2,(ship1 is Enemy)?40:80);
        }
        private bool bulletAndBullet(Bullet bullet1, Bullet bullet2)
        {
            return this.check(bullet1, bullet2,20);
        }
        private bool asteroidAndSubject(Asteroid asteroid,Subject subj)
        {
            if(subj == null) return false;
            return this.check(asteroid,subj,asteroid.Radius/2+subj.getWidth()/3);
        }

        private bool check(Subject sub1, Subject sub2,int dist)
        {
            int x = Math.Abs(sub1.LocationX - sub2.LocationX);
            int y = Math.Abs(sub1.LocationY - sub2.LocationY);
            int distance = (int)Math.Sqrt(x * x + y * y);
            return distance <= dist;
        }
    }
}
