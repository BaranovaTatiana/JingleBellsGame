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
    class Prizes:  Bonuses, IGameObject
    {

        //public FrameworkElement ShapeBlock { get; set; }
        //public ShapeParameters ShapeParam { get; set; }
        //public bool IsRemoved { get; set; }
        
        private MediaPlayer _bell = App.Bell;
        private Uri _bellSource = new Uri("music/bells.mp3", UriKind.RelativeOrAbsolute);
        public Prizes(FrameworkElement prize, ShapeParameters prizeParameters) : base(prize, prizeParameters)
        {
            ShapeBlock = prize;
            ShapeParam = prizeParameters;
        }

        public new void Create()
        {
            var imagePrize = (Image)ShapeBlock;
            var rnd2 = App.Rnd.Next(1, 4);
            switch (rnd2)
            {
                case 1:
                    imagePrize.Source = GameResources.Prize4;
                    imagePrize.Height = 65; //45
                    imagePrize.Width = 90;
                    break;
                case 2:
                    imagePrize.Source = GameResources.Prize5;
                    imagePrize.Height = 80; //45
                    imagePrize.Width = 120;
                    break;
                case 3:
                    imagePrize.Source = GameResources.Prize7;
                    imagePrize.Height = 75; //45
                    imagePrize.Width = 50;
                    break;
            }
        }

        public new void Collision()
        {
            if (CollisionPrizes(/*ShapeBlock, ShapeParam*/))
            {
                IsRemoved = true;
                App.OpenedMainWindow.ScoreInGame.StepSwingImage = 1;
            }
            else IsRemoved = false;
        }

        public bool CollisionPrizes(/*FrameworkElement prize, ShapeParameters prizeParameters*/)
        {
            if (!App.OpenedMainWindow.CheckCollision(ShapeBlock)) return false;
            _bell.Volume = App.Settings.RestVolume;
            _bell.Open(_bellSource);
            _bell.Play();
            
            
            App.OpenedMainWindow.BCanvas.Children.Remove(ShapeBlock);


            for (int i = 0; i < App.OpenedMainWindow.BlockGameObjects.Count; i++)
            {
                if (this == App.OpenedMainWindow.BlockGameObjects[i]) App.OpenedMainWindow.BlockGameObjects.Remove(this);
            }
            
            
            ShapeParameters parametersFrame = new ShapeParameters
            {
                X = ShapeParam.X,
                Y = ShapeParam.Y,
                Type = TypeShapeParameters.Frame,
            };
            
            var frame = new Frames(parametersFrame);
            frame.Create();
            
            App.OpenedMainWindow.ScoreInGame.AddScore();
            return true;
        }
    }
}
