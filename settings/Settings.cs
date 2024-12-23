using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SpaceWar3.settings
{
    enum SettingTypes
    {
        STRING,INT
    }
    class SettingBar
    {
        public string name;
        public readonly Control control;
        public string value;
        public readonly SettingTypes type;
        public bool isReady = false;
        public SettingBar(string name, Control control, SettingTypes type)
        {
            this.name = name;
            this.control = control;
            this.type = type;
        }
        public void setValue(string val)
        {
            this.value = val;
            if(this.control is Button){
                ((Button)this.control).Content = val; 
            }
            else ((TextBox)this.control).Text = val;
            this.isReady = true;
        }
    }
    internal class Settings
    {
        private Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private List<SettingBar> settingBar;
        public Settings(List<SettingBar> settingBar) 
        {
            this.settingBar = settingBar;
            foreach (SettingBar item in settingBar)
            {
                item.name.Replace(" ", "_");
            }
            //MessageBox.Show(config.AppSettings.Settings["BOSS_SPAWN_SCORE"].Value);
        }
        public void intializeButtons()
        {
            foreach (SettingBar item in settingBar)
            {
                item.setValue(ConfigurationManager.AppSettings[item.name]);
            }
        }
        public void setNotReady(Control control)
        {
            this.settingBar.Find(e => e.control == control).isReady = false;
        }
        public void setReady(Control control)
        {
            this.settingBar.Find(e => e.control == control).isReady = true;
        }
        public void setValue(Control control, string val)
        {
            SettingBar sb = this.settingBar.Find(e => e.control == control);
            if (control is Button)
            {
                string temp = sb.value;
                foreach (SettingBar item in this.settingBar)
                {
                    if (item.value == val) item.setValue(temp);
                }
                sb.setValue(val);
            }
            sb.setValue(val);
        }
        public bool save()
        {
            foreach (SettingBar item in this.settingBar)
            {
                config.AppSettings.Settings[item.name].Value = item.value;
                if(!item.isReady){ return false; }
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            Controller.refresh();
            return true;
        }
        public void setDefault()
        {
            string[,] nameValue =
            {
                {"RIGHT","D" },
                {"LEFT","A" },
                {"UP","W" },
                {"DOWN","S" },
                {"SHOOT","Space" },
                {"PAUSE","P" },
                {"FAST_SUPPLY","U" },
                {"HEAL_SUPPLY","I" },
                {"STRONG_SUPPLY","T" },
                {"GHOST_SUPPLY","Y" },
                {"BOSS_SPAWN_SCORE","200" },
                {"ASTEROID_SPAWN_TIME","20" },
                {"ENEMY_SPAWN_TIME_FIRST","4" },
                {"ENEMY_SPAWN_TIME_SECOND","3" },
                {"ENEMY_SPAWN_TIME_THIRD","2" },
                {"ENEMY_SPAWN_TIME_FOURTH","1" },
            };
            for (int i = 0; i<nameValue.GetLength(0); i++)
            {
                config.AppSettings.Settings[nameValue[i,0]].Value = nameValue[i,1];
            }
            //config.AppSettings.Settings["RIGHT"].Value = "D";
            //config.AppSettings.Settings["LEFT"].Value = "A";
            //config.AppSettings.Settings["UP"].Value = "W";
            //config.AppSettings.Settings["DOWN"].Value = "S";
            //config.AppSettings.Settings["SHOOT"].Value = "Space";
            //config.AppSettings.Settings["PAUSE"].Value = "P";
            //config.AppSettings.Settings["FAST_SUPPLY"].Value = "U";
            //config.AppSettings.Settings["HEAL_SUPPLY"].Value = "I";
            //config.AppSettings.Settings["STRONG_SUPPLY"].Value = "T";
            //config.AppSettings.Settings["GHOST_SUPPLY"].Value = "Y";
            //config.AppSettings.Settings["BOSS_SPAWN_SCORE"].Value = "20";
            //config.AppSettings.Settings["ASTEROID_SPAWN_TIME"].Value = "20";
            //config.AppSettings.Settings["ENEMY_SPAWN_TIME_FIRST"].Value = "4";
            //config.AppSettings.Settings["ENEMY_SPAWN_TIME_SECOND"].Value = "3";
            //config.AppSettings.Settings["ENEMY_SPAWN_TIME_THIRD"].Value = "2";
            //config.AppSettings.Settings["ENEMY_SPAWN_TIME_FOURTH"].Value = "1";

            for (int i = 0; i < nameValue.GetLength(0); i++)
            {
                this.settingBar.Find(e => e.name == nameValue[i, 0]).setValue(nameValue[i, 1]);
            }
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
