using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SpaceWar3.settings
{
    /// <summary>
    /// SettingsWindow.xaml etkileşim mantığı
    /// </summary>
    
    public partial class SettingsWindow : Window
    {
        private List<SettingBar> settingBars = new List<SettingBar>();
        private Settings settings;
        private MainWindow mw;
        public SettingsWindow(MainWindow mw)
        {
            InitializeComponent();
            this.mw = mw;
            this.settingBars = new List<SettingBar>() 
            {
                new SettingBar(UP.Name, UP, SettingTypes.STRING),
                new SettingBar(DOWN.Name, DOWN, SettingTypes.STRING),
                new SettingBar(LEFT.Name, LEFT, SettingTypes.STRING),
                new SettingBar(RIGHT.Name, RIGHT, SettingTypes.STRING),
                new SettingBar(SHOOT.Name, SHOOT, SettingTypes.STRING),
                new SettingBar(PAUSE.Name, PAUSE, SettingTypes.STRING),
                new SettingBar(FAST_SUPPLY.Name, FAST_SUPPLY, SettingTypes.STRING),
                new SettingBar(HEAL_SUPPLY.Name, HEAL_SUPPLY, SettingTypes.STRING),
                new SettingBar(STRONG_SUPPLY.Name, STRONG_SUPPLY, SettingTypes.STRING),
                new SettingBar(GHOST_SUPPLY.Name, GHOST_SUPPLY, SettingTypes.STRING),
                new SettingBar(BOSS_SPAWN_SCORE.Name, BOSS_SPAWN_SCORE, SettingTypes.STRING),
                new SettingBar(ASTEROID_SPAWN_TIME.Name, ASTEROID_SPAWN_TIME, SettingTypes.INT),
                new SettingBar(ENEMY_SPAWN_TIME_FIRST.Name, ENEMY_SPAWN_TIME_FIRST, SettingTypes.INT),
                new SettingBar(ENEMY_SPAWN_TIME_SECOND.Name, ENEMY_SPAWN_TIME_SECOND, SettingTypes.INT),
                new SettingBar(ENEMY_SPAWN_TIME_THIRD.Name, ENEMY_SPAWN_TIME_THIRD, SettingTypes.INT),
                new SettingBar(ENEMY_SPAWN_TIME_FOURTH.Name, ENEMY_SPAWN_TIME_FOURTH, SettingTypes.INT),
            };
            this.settings = new Settings(this.settingBars);
            this.intializeButtons();
            this.PreviewKeyDown += Window_KeyDown;
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            object focusedElement = Keyboard.FocusedElement;
            if (focusedElement is Button)
            {
                this.settings.setValue((Control)focusedElement,e.Key.ToString());
            }
        }
        private void intializeButtons()
        {
            this.settings.intializeButtons();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox)) return;
            TextBox textBox = sender as TextBox;
            int number;
            bool success = Int32.TryParse(textBox.Text, out number);
            if (!success){
                textBox.Background = Brushes.Red;
                this.settings?.setNotReady(textBox);
                return;
            }
            else
            { 
                textBox.Background = Brushes.White;
                this.settings?.setReady(textBox);
            }
            if(textBox == BOSS_SPAWN_SCORE || textBox == ASTEROID_SPAWN_TIME)
            {
                if (textBox == BOSS_SPAWN_SCORE)
                {
                    if (number >= 200 && number <= 1000)
                    {
                        textBox.Background = Brushes.White;
                        this.settings.setValue(textBox, textBox.Text);
                    }
                    else
                    {
                        textBox.Background = Brushes.Red;
                        this.settings.setNotReady(textBox);
                    }

                }
                if (textBox == ASTEROID_SPAWN_TIME)
                {
                    if (number >= 10 && number <= 100)
                    {
                        textBox.Background = Brushes.White;
                        this.settings.setValue(textBox, textBox.Text);
                    }
                    else
                    {
                        textBox.Background = Brushes.Red;
                        this.settings.setNotReady(textBox);
                    }

                }
                return;
            }
                int[] values;
            try
            {
                values = intMinMaxControl();
            }
            catch (IntConvertException)
            {
                return;
            }
            if (values[0] < 1 || values[0] < values[1] || values[0] > 10)
            {
                ENEMY_SPAWN_TIME_FIRST.Background = Brushes.Red;
                this.settings.setNotReady(ENEMY_SPAWN_TIME_FIRST);
            }
            else
            {
                ENEMY_SPAWN_TIME_FIRST.Background = Brushes.White;
                this.settings.setValue(ENEMY_SPAWN_TIME_FIRST, ENEMY_SPAWN_TIME_FIRST.Text);
            }
            if (values[1] < 1 ||  values[1] < values[2] ||  values[1] > values[0] ||  values[1] > 10)
            {
                ENEMY_SPAWN_TIME_SECOND.Background = Brushes.Red;
                this.settings.setNotReady(ENEMY_SPAWN_TIME_SECOND);
            }
            else
            {
                ENEMY_SPAWN_TIME_SECOND.Background = Brushes.White;
                this.settings.setValue(ENEMY_SPAWN_TIME_SECOND, ENEMY_SPAWN_TIME_SECOND.Text);
            }
            if (values[2] < 1 ||  values[2] < values[3] ||  values[2] > values[1] ||  values[2] > 10)
            {
                ENEMY_SPAWN_TIME_THIRD.Background = Brushes.Red;
                this.settings.setNotReady(ENEMY_SPAWN_TIME_THIRD);
            }
            else
            {
                ENEMY_SPAWN_TIME_THIRD.Background = Brushes.White;
                this.settings.setValue(ENEMY_SPAWN_TIME_THIRD, ENEMY_SPAWN_TIME_THIRD.Text);
            }
            if (values[3] < 1 ||  values[3] > values[2] ||  values[3] > 10)
            {
                ENEMY_SPAWN_TIME_FOURTH.Background = Brushes.Red;
                this.settings.setNotReady(ENEMY_SPAWN_TIME_FOURTH);
            }
            else
            {
                ENEMY_SPAWN_TIME_FOURTH.Background = Brushes.White;
                this.settings.setValue(ENEMY_SPAWN_TIME_FOURTH, ENEMY_SPAWN_TIME_FOURTH.Text);
            }


        }
        private int[] intMinMaxControl()
        {
            bool result = true;
            int n1,n2,n3,n4;
            try
            {
                n1 = Int16.Parse(ENEMY_SPAWN_TIME_FIRST.Text);
            }
            catch (Exception ex)
            {
                n1 = 0;
                ENEMY_SPAWN_TIME_FIRST.Background = Brushes.Red;
                this.settingBars.Find(e => e.name == "ENEMY_SPAWN_TIME_FIRST").isReady = false;
                result = false;
            }
            try
            {
                n2 = Int16.Parse(ENEMY_SPAWN_TIME_SECOND.Text);
            }
            catch (Exception ex)
            {
                n2 = 0;
                ENEMY_SPAWN_TIME_SECOND.Background = Brushes.Red;
                this.settingBars.Find(e => e.name == "ENEMY_SPAWN_TIME_SECOND").isReady = false;
                result = false;
            }
            try
            {
                n3 = Int16.Parse(ENEMY_SPAWN_TIME_THIRD.Text);
            }
            catch (Exception ex)
            {
                n3 = 0;
                ENEMY_SPAWN_TIME_THIRD.Background = Brushes.Red;
                this.settingBars.Find(e => e.name == "ENEMY_SPAWN_TIME_THIRD").isReady = false;
                result = false;
            }
            try
            {
                n4 = Int16.Parse(ENEMY_SPAWN_TIME_FOURTH.Text);
            }
            catch (Exception ex)
            {
                n4 = 0;
                ENEMY_SPAWN_TIME_FOURTH.Background = Brushes.Red;
                this.settingBars.Find(e => e.name == "ENEMY_SPAWN_TIME_FOURTH").isReady = false;
                result = false;
            }
            if (result == false) throw new IntConvertException();
            int[] a = { n1, n2, n3, n4 };
            return a;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!this.settings.save()) MessageBox.Show("Ther is a problem !!!");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.mw.IsSwOpen = false;
            this.mw.windowClosed(this);
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.settings.setDefault();
        }
        private void SW_Closed(object sender, EventArgs e)
        {
            this.mw.IsSwOpen = false;
            this.mw.windowClosed(this);
        }
    }
}
