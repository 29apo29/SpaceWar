using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SpaceWar3
{
    public class User
    {
        public string Username { get; set; }
        public int Score { get; set; }
    }
    internal class ScoreBoard
    {
        private User[] users;
        private User user;
        private string username;
        private int score;
        private string csv;
        private const string FILE_NAME = "./scoreboard.csv";
        private Grid panel;

        public ScoreBoard()
        {
            this.getFromStorage();
        }
        public ScoreBoard(string username, int score)
        {
            this.username = username;
            this.score = score;
            this.getFromStorage();
        }

        public void update()
        {
            this.getFromStorage();
        }


        private void getFromStorage()
        {
            try
            {
                this.csv = File.ReadAllText(FILE_NAME, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                this.csv = "null";
            }
            this.ConvertCSVToArray();
        }
        private void ConvertCSVToArray()
        {
            if (this.csv == "null")
            {
                this.users = new User[0];
                return;
            }
            string[] lines = ArrayUtils.removeLastItem(this.csv.Split(';'));
            User[] users = new User[0];
            foreach (string line in lines)
            {
                string[] splited = line.Split(',');
                string username = splited[0];
                int score = int.Parse(splited[1]);
                users = ArrayUtils.add(users, new User { Username = username, Score = score });
            }
            this.users = users;
        }
        public void add()
        {
            int location = this.findLocation();
            if (location == -1) return;
            this.user = new User { Username = this.username, Score = this.score };
            this.users = ArrayUtils.addWithLocation(this.users, user, location);
            this.updateStorage();
        }
        private void updateStorage()
        {
            string csv = "";
            foreach (User user in this.users)
            {
                csv += user.Username + "," + user.Score + ";";
            }
            File.WriteAllText(FILE_NAME, csv);
        }

        private int findLocation()
        {
            if (this.users.Length == 0) return 0;
            int i = 0;
            foreach (User user in this.users)
            {
                if (user.Score <= this.score)
                {
                    return i;
                }
                i++;
            }
            if (this.users.Length < 10) return this.users.Length;
            return -1;
        }
        public User[] getForRender()
        {
            return this.users;
        }
        private Grid[] getPanelsForRender()
        {
            Grid[] panels = new Grid[0];
            int i = 0;
            foreach (User user in this.users)
            {
                Label label1 = new Label();
                label1.Content = user.Username;
                Label label2 = new Label();
                label2.Content = user.Score.ToString();
                Canvas.SetLeft(label2, 100);
                Grid panel = new Grid();
                panel.Children.Add(label1);
                panel.Children.Add(label2);
                Canvas.SetTop(panel, i * 20);
                panel.Height = 20;
                panels = ArrayUtils.add(panels, panel);
                i++;
            }
            return panels;
        }
    }
}