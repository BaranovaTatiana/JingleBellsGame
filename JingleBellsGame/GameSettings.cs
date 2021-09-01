using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace JingleBellsGame
{
    [Serializable]
    public class GameSettings
    {
        
        public double MainMuseVolume { get; set; } = 0.5;

        public double RestVolume { get; set; } = 0;
        public GameSettings()
        {
        }

        //public GameSettings(double mainVolume, double restVolume)
        //{
        //    MainMuseVolume = mainVolume;
        //    RestVolume = restVolume;
        //}
        XmlSerializer _serializer = new XmlSerializer(typeof(GameSettings));
        private FileStream _fs;
        public void Save()
        {
            using (_fs = new FileStream("data.xml", FileMode.Create))
            {
                _serializer.Serialize(_fs, App.Settings);
            }
        }

        //private int _num = 0;
        public void Load()
        {
            using (_fs = new FileStream("data.xml", FileMode.OpenOrCreate))
            {
                var settings = (GameSettings)_serializer.Deserialize(_fs);
                if (settings != null)
                {
                    MainMuseVolume = settings.MainMuseVolume;
                    RestVolume = settings.RestVolume;
                }
            }
        }
    }
}
