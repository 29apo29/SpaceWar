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

namespace SpaceWar3
{
    /// <summary>
    /// GameGUI.xaml etkileşim mantığı
    /// </summary>
    public partial class GameGUI : Window
    {
        private MainWindow mainWindow;
        private Game game;
        public GameGUI(MainWindow mainWindow,string username)
        {
            InitializeComponent();
            this.Top = 0;
            this.mainWindow = mainWindow;
            this.Width = 1500;
            this.Height = 1000;
            this.guiGrid.Width = this.Width;
            this.guiGrid.Height = this.Height;
            Game game = new Game(this,username);
            this.game = game;
            this.game.startGame();
        }
        private void On_Closed(object sender, EventArgs e)
        {
            this.mainWindow.guiClosed();
            game.shutdown();
        }
        public void shutdown() 
        {        
            this.game.shutdown();
        }
    }
}
