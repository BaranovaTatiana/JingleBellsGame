using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;
using JingleBellsGame.Properties;

namespace JingleBellsGame
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MainWindow OpenedMainWindow = null;
        public static Random Rnd = new Random(Environment.TickCount);

        public static bool IsLava = false;
        public static bool IsJumped = false;
        public static double CounterStepJump = 0;
        public static double Speed = 0;
        public int Level = 1;
        public static bool IsFirstPrize = true;
        public static bool IsFirstBonus = true;
        public static MediaPlayer MainMuse = new MediaPlayer();

        public static MediaPlayer Hoho = new MediaPlayer();
        public static MediaPlayer Bell = new MediaPlayer();
        public static MediaPlayer LavaMedia = new MediaPlayer();

        public static GameSettings Settings = new GameSettings();

        
    public App()
        {


        }
    }
}
