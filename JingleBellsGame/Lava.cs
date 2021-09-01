using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JingleBellsGame
{
    class Lava : VisibleBlocks, IGameObject
    {
        //public FrameworkElement ShapeBlock { get; set; }
        //public ShapeParameters ShapeParam { get; set; }
        //public bool IsRemoved { get; set; }

        
        private MediaPlayer _lavaMediaPlayer = App.LavaMedia;
        private Uri _lavaMediaSource = new Uri("music/lava1.mp3", UriKind.RelativeOrAbsolute);
        public Lava(FrameworkElement lava, ShapeParameters lavaParameters) : base(lava, lavaParameters)
        {
            ShapeBlock = lava;
            ShapeParam = lavaParameters;
        }
        public new void Create()
        {
            if (!(ShapeBlock is Image localLava)) return;
            localLava.Source = GameResources.Lava1;
            localLava.Height = 60;
            if (ShapeParam.Size == SizeShapeParameters.Small) localLava.Width = 310; //210
            else localLava.Width = 770; //690
        }
        
        public new void Collision()
        {
            if (App.IsLava) return;

            double x = Canvas.GetLeft(ShapeBlock);
            double y = Canvas.GetTop(ShapeBlock);
            if (Hero.X >= x - Hero.Width / 2 &&
                Hero.X <= x + ShapeBlock.Width - Hero.Width / 2 &&
                Hero.Y >= y - Hero.HeroShape.ActualHeight / 2)
            {
                _lavaMediaPlayer.Volume = App.Settings.RestVolume;
                _lavaMediaPlayer.Open(_lavaMediaSource);
                _lavaMediaPlayer.Play();
                
                App.IsLava = true;
                App.OpenedMainWindow.GameOver();
            }
        }

      

    }
}
