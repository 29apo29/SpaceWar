using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SpaceWar3.settings;

namespace SpaceWar3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool isGameOpened { get; set; }
        private GameGUI gameGUI;
        private SettingsWindow sw;
        private ScoreBoardWindow sbw;
        private bool isSwOpen = false;
        private bool isSbwOpen = false;

        public bool IsSwOpen
        {
            set
            {
                this.sw = value?new SettingsWindow(this):null;
                this.isSwOpen = value; 
            }
        }
        public bool IsSbwOpen
        {
            set
            {
                this.sbw = value?new ScoreBoardWindow(this):null;
                this.isSbwOpen = value; 
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            this.MinWidth = 500;
            this.MinHeight = 550;
            this.Width = 500;
            this.Height = 550;
            this.isGameOpened = false;
            this.sw = null;
            this.sbw = null;
        }
        private void Github(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/29apo29/SpaceWar",
                UseShellExecute = true
            });
        }
        public void windowClosed(Window a)
        {
            if (a == this.sw) this.sw = null;
            if (a == this.sbw) this.sbw = null;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.isGameOpened) this.gameGUI.shutdown();
            if(this.isSwOpen) this.sw.Close();
            if(this.isSbwOpen) this.sbw.Close();
            this.sw = null;
            this.sbw = null;
            Application.Current.Shutdown();
        }

        private void username_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = username.Text;
            startButtonEnableControl(text);
        }
        private void startButtonEnableControl(string text)
        {
            if (this.isGameOpened)
            {
                writeWarn(false);
                startButton.IsEnabled = false;
                return;
            }
            if (text.Length >= 3 && text.Length <= 16)
            {
                startButton.IsEnabled = true;
                writeWarn(true, true);
            }
            else
            {
                startButton.IsEnabled = false;
                writeWarn(true);
            }
        }
        private void writeWarn(bool which, bool status = false)
        {
            if (!status) warnLabel.Content = which? "The length of the username must be between 3 and 16 characters!" : "The game is already opened!";
            else warnLabel.Content = "";
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            this.isGameOpened = true;
            startButtonEnableControl(this.username.Text);
            this.gameGUI = new GameGUI(this,this.username.Text);
            this.gameGUI.Show();
        }

        public void guiClosed()
        {
            this.isGameOpened=false;
            startButtonEnableControl(this.username.Text);
            writeWarn(false, true);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!this.isSwOpen)
            {
                this.sw = new SettingsWindow(this);
                Application.Current.Dispatcher.Invoke(() => { 
                    this.sw.Show();
                    this.isSwOpen = true;
                });
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!this.isSbwOpen)
            {
                this.sbw = new ScoreBoardWindow(this);
                Application.Current.Dispatcher.Invoke(() => {
                    this.sbw.Show();
                    this.isSbwOpen = true;
                });
            }

        }
    }
}