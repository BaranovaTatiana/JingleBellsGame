using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JingleBellsGame
{
    class Bonuses: VisibleBlocks, IGameObject
    {
        //public FrameworkElement ShapeBlock { get; set; }
        //public ShapeParameters ShapeParam { get; set; }
        //public bool IsRemoved { get; set; }

        private MediaPlayer _hohoMediaPlayer = App.Hoho;

        private Uri _hohoMediaSource1 = new Uri("music/bonusHo.mp3", UriKind.RelativeOrAbsolute);
        private Uri _hohoMediaSource2 = new Uri("music/bonusWAW2.mp3", UriKind.RelativeOrAbsolute);
        public Bonuses(FrameworkElement bonus, ShapeParameters bonusParameters) :base(bonus, bonusParameters) 
        {
            ShapeBlock = bonus;
            ShapeParam = bonusParameters;
        }

        
        public new void Create()
        {
            if (!(ShapeBlock is Image localBonus)) return;
            var rnd3 = App.Rnd.Next(1, 3);
            switch (rnd3)
            {
                case 1:
                    localBonus.Source = GameResources.BigBlock;
                    localBonus.Width = 50;
                    localBonus.Height = 70;
                    break;
                case 2:
                    localBonus.Source = GameResources.Bonus2;
                    localBonus.Width = 55;
                    localBonus.Height = 65;
                    break;
            }
        }

        public new void Collision()
        {
            IsRemoved = CollisionBonusBlock(ShapeBlock, ShapeParam);
        }

        private bool CollisionBonusBlock(FrameworkElement bonus, ShapeParameters bonusParameters)
        {
            if (!App.OpenedMainWindow.CheckCollision(bonus)) return false;
            var rnd = App.Rnd.Next(1, 3);
            _hohoMediaPlayer.Volume = App.Settings.RestVolume;
            switch (rnd)
            {
                case 1:
                    _hohoMediaPlayer.Open(_hohoMediaSource1);
                    _hohoMediaPlayer.Play();
                    break;
                case 2:
                    _hohoMediaPlayer.Open(_hohoMediaSource2);
                    _hohoMediaPlayer.Play();
                    break;
            }
            App.OpenedMainWindow.BCanvas.Children.Remove(bonus);
            App.OpenedMainWindow.BlockGameObjects.Remove(this);
            var brick = new VisibleBlocks(null, null);
            
            brick.Create(bonusParameters.InvisibleBlocks); 
            ShapeParameters parametersFrame = new ShapeParameters
            {
                X = bonusParameters.X,
                Y = bonusParameters.Y,
                Type = TypeShapeParameters.BonusMessage,
            };
            
            var frame = new Frames(parametersFrame);
            frame.Create();
            return true;
        }



    }
}
