using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static SpaceWar3.ScoreBoard;

namespace SpaceWar3
{
    /// <summary>
    /// ScoreBoardWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class ScoreBoardWindow : Window
    {
        private ScoreBoard scoreBoard;
        private MainWindow mw;
        public ScoreBoardWindow(MainWindow mw)
        {
            InitializeComponent();
            this.scoreBoard = new ScoreBoard();
            this.mw = mw;
        }

        private void ScoreBoardWindow_Load(object sender, RoutedEventArgs e)
        {
            writeScoreBoard();
        }
        private void writeScoreBoard()
        {
            User[] users = this.scoreBoard.getForRender();
            if (users.Length == 0)
            {

                var nameLabel = new Label
                {
                    Content = "There is no score!!",
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                var scoreLabel = new Label
                {
                    Content = "",
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Right
                };

                Grid.SetRow(nameLabel, ScoreBoardTable.RowDefinitions.Count - 1);
                Grid.SetColumn(nameLabel, 0);
                ScoreBoardTable.Children.Add(nameLabel);
                Grid.SetRow(scoreLabel, ScoreBoardTable.RowDefinitions.Count - 1);
                Grid.SetColumn(scoreLabel, 1);
                ScoreBoardTable.Children.Add(scoreLabel);
                ScoreBoardTable.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                return;
            }
            foreach (User user in users)
            {
                ScoreBoardTable.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                Label nameLabel = new Label
                {
                    Content = user.Username,
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                Label scoreLabel = new Label
                {
                    Content = user.Score.ToString(),
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Right
                };

                Grid.SetRow(nameLabel, ScoreBoardTable.RowDefinitions.Count - 1);
                Grid.SetColumn(nameLabel, 0);
                ScoreBoardTable.Children.Add(nameLabel);

                Grid.SetRow(scoreLabel, ScoreBoardTable.RowDefinitions.Count - 1);
                Grid.SetColumn(scoreLabel, 1);
                ScoreBoardTable.Children.Add(scoreLabel);
                ScoreBoardTable.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }
        }
        private void SBW_Closed(object sender, EventArgs e)
        {
            this.mw.IsSbwOpen = false;
            this.mw.windowClosed(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.mw.IsSbwOpen = false;
            this.mw.windowClosed(this);
            this.Close();
        }
    }
}
