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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using JingleBellsGame.Properties;
using static JingleBellsGame.App;


namespace JingleBellsGame
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public sealed partial class MenuWindow : Window
    {
        //public MenuWindow WindowMenu;//= new MenuWindow();
        //TextBlock _volume = new TextBlock();
        public MenuWindow()
        {
            Loaded += OnLoaded;
            InitializeComponent();
            App.OpenedMainWindow.LocationChanged += OpenedMainWindow_LocationChanged;

            //_volume.VerticalAlignment = VerticalAlignment.Bottom;
            //_volume.HorizontalAlignment = HorizontalAlignment.Left;
            //_volume.Text = App.Settings.RestVolume.ToString(CultureInfo.InvariantCulture) + " " +
            //              App.Settings.MainMuseVolume;
            //_volume.Foreground = Brushes.Red;
            //_volume.FontSize = 30;
            //GridMenu.Children.Add(_volume);
        }

        private void OpenedMainWindow_LocationChanged(object sender, EventArgs e)
        {
            UpdateLocationMenu();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateLocationMenu();
            SliderValue.Value = App.Settings.MainMuseVolume;
            //if (Math.Abs(App.Settings.MenuLeft) > 0.00001 && Math.Abs(App.Settings.MenuTop) > 0.00001)
            //{
            //    Left = App.Settings.MenuLeft;
            //    Top = App.Settings.MenuTop;
            //}
        }

        private void UpdateLocationMenu()
        {
            Top = App.OpenedMainWindow.Top + (App.OpenedMainWindow.ActualHeight - ActualHeight) / 2;
            Left = App.OpenedMainWindow.Left + (App.OpenedMainWindow.ActualWidth - ActualWidth) / 2;
        }


        private Grid _gridMenu = new Grid();
        //private double _volumeMainMuse = App.MainMuse.Volume;
        //private double _volumeHoho = App.Hoho.Volume;
        //private double _volumeBell = App.Bell.Volume;
        //private double _lava = App.LavaMedia.Volume;

        //private double _volume = 0.4;

        public void MenuParameters()
        {
            //Background = Brushes.BlueViolet;
            //Opacity = 0.5;
            //Width = _gridMenu.Width = 650;
            //Height = _gridMenu.Height = 350;
            //AllowsTransparency = true;
            //WindowStyle = WindowStyle.None;
            //WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public void ButtonsMenu()
        {
            
            //var volume = new Button();
            //var language = new Button();
            //language.Click += Click_Language;
            //var stackPan = new StackPanel();
            //stackPan.Children.Add(volume);
            //stackPan.Children.Add(language);

            //volume.Width = language.Width = 120;
            //volume.Height = language.Height = 90;
            //volume.Background = language.Background = Brushes.Aquamarine;
            //volume.Content = "Volume";
            //language.Content = "Language";
            //AddChild(_gridMenu);
            //_gridMenu.Children.Add(stackPan);
            //stackPan.VerticalAlignment = VerticalAlignment.Top;
            //stackPan.HorizontalAlignment = HorizontalAlignment.Center; 

        }

        
        private void Language_OnClick(object sender, RoutedEventArgs e)
        {
            ////PanelLanguage.Opacity = Math.Abs(PanelLanguage.Opacity - 1) < 0.0001 ? 0 : 1;
            //BorderLanguage.Opacity = Math.Abs(BorderLanguage.Opacity - 1) < 0.0001 ? 0 : 1;
            BorderLanguage.Visibility = BorderLanguage.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Volume_OnClick(object sender, RoutedEventArgs e)
        {
            //if (Math.Abs(OffVolume.Opacity - 1) < 0.0001 && Math.Abs(SliderValue.Opacity - 1) < 0.0001)
            //    OffVolume.Opacity = SliderValue.Opacity = 0;
            //else
            //{
            //    OffVolume.Opacity = SliderValue.Opacity = 1;
            //    //SliderValue.Value = App.Settings.MainMuseVolume;
            //}

            if (OffVolume.Visibility == Visibility.Collapsed && SliderValue.Visibility == Visibility.Collapsed)
            {
                OffVolume.Visibility = Visibility.Visible;
                SliderValue.Visibility = Visibility.Visible;
            }
            else
            {
                OffVolume.Visibility = Visibility.Collapsed;
                SliderValue.Visibility = Visibility.Collapsed;
            }
        }

        private void OffVolume_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(OffVolume.Content is string)) return;
            switch ((string)OffVolume.Content)
            {
                case "Off":

                    MainMuse.Stop();
                    App.Settings.RestVolume = 0;
                    SliderValue.Value = 0;
                    OffVolume.Content = "On";
                    break;
                case "On":

                    App.Settings.MainMuseVolume = 0.4;
                    MainMuse.Volume = App.Settings.MainMuseVolume;
                    MainMuse.Play();
                    App.Settings.RestVolume = App.Settings.MainMuseVolume * 1.4;// 0.7;
                    SliderValue.Value = 0.4;
                    OffVolume.Content = "Off";
                    break;
            }

        }
        private void SliderValue_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            App.Settings.MainMuseVolume = MainMuse.Volume = e.NewValue;
            //App.Settings.RestVolume = e.NewValue + 0.3;
            App.Settings.RestVolume = e.NewValue * 1.4;
            //if (App.Settings.RestVolume <= 0.7) App.Settings.RestVolume = e.NewValue + 0.3;
            //else App.Settings.RestVolume = 1;
            //MainMuse.Volume = App.Settings.MainMuseVolume;
            if ((string) OffVolume.Content == "Off" && e.NewValue <= 0)
            {
                OffVolume.Content = "On";
                App.Settings.RestVolume =App.Settings.MainMuseVolume =  0;
                //MainMuse.Stop();
            }
            if ((string) OffVolume.Content == "On" && e.NewValue > 0)
            {
                OffVolume.Content = "Off";
               
                MainMuse.Play();
            }
            //_volume.Text = App.Settings.RestVolume.ToString(CultureInfo.InvariantCulture) + " " +
            //               App.Settings.MainMuseVolume;
        }
        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            App.Settings.Save();
            OpenedMainWindow.Close();
        }

        //private void Russian_OnClick(object sender, RoutedEventArgs e)
        //{
        //    App.Settings.Language = LanguageGame.Russian;
        //    Russian.Background = Brushes.Red;
        //    English.Background = Brushes.Chartreuse;
        //}

        //private void English_OnClick(object sender, RoutedEventArgs e)
        //{
        //    App.Settings.Language = LanguageGame.English;
        //    English.Background = Brushes.LightCoral;
        //    Russian.Background = Brushes.Chartreuse;
        //}

        private void Restart_OnClick(object sender, RoutedEventArgs e)
        {
            OpenedMainWindow.NextGame();
        }

        private void Resume_OnClick(object sender, RoutedEventArgs e)
        {
            switch ((string)Resume.Content)
            {
                case "Resume":
                    App.OpenedMainWindow.OpenedMenu = false;
                    Close();
                    break;
                case "Restart":
                   OpenedMainWindow.NextGame();
                    break;
            }

            //App.OpenedMainWindow.OpenedMenu = false;
        }
    }

}
