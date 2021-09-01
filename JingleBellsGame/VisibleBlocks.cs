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
    class VisibleBlocks : IGameObject
    {
        public FrameworkElement ShapeBlock { get; set; }
        public ShapeParameters ShapeParam { get; set; }

        public bool IsRemoved { get; set; }
        
        public VisibleBlocks(FrameworkElement visBlock, ShapeParameters visParameters)
        {
            ShapeBlock = visBlock;
            ShapeParam = visParameters;
        }

        
        public void Create()
        {
            var localVis =ShapeBlock as Image;
            if (localVis!= null)
            {
                switch ( ShapeParam.Size)
                {
                    case SizeShapeParameters.Big:
                        var rnd1 = App.Rnd.Next(1, 5); //Debug.WriteLine(rnd);
                        switch (rnd1)
                        {
                            case 1:
                                localVis.Source = GameResources.BigB1;
                                break;
                            case 2:
                                localVis.Source = GameResources.BigB2;
                                break;
                            case 3:
                                localVis.Source = GameResources.BigB3;
                                break;
                            case 4:
                                localVis.Source = GameResources.BigB4;
                                break;
                        }

                        localVis.Width = 130;
                        localVis.Height = 120; //if (Level==1) block.Fill = Brushes.Green;
                        break;
                    case SizeShapeParameters.Small:
                        rnd1 = App.Rnd.Next(1, 4); //Debug.WriteLine(rnd);
                        switch (rnd1)
                        {
                            case 1:
                                localVis.Source = GameResources.SmallB1;
                                break;            
                            case 2:               
                                localVis.Source = GameResources.SmallB2;
                                break;            
                            case 3:               
                                localVis.Source = GameResources.SmallB3;
                                break;
                        }

                        localVis.Width = 90;
                        localVis.Height = 40; //if (Level == 1) block.Fill = Brushes.Red;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            //GameStart = false;

        }
        public void Collision()
        {
            double xShape = Canvas.GetLeft(ShapeBlock);
            double yShape = Canvas.GetTop(ShapeBlock);
            double yHeroEllipse = Hero.Y;
            if (yHeroEllipse > yShape - Hero.HeroShape.ActualHeight
                && yHeroEllipse < yShape + ShapeBlock.ActualHeight + 8) //гравитация
            {
                if (Hero.X > xShape - Hero./*HeroShape.Actual*/Width &&
                    Hero.X < xShape + App.OpenedMainWindow.CollisionValueX - Hero./*HeroShape.Actual*/Width)
                    Hero.X = xShape - Hero./*HeroShape.Actual*/Width;

                if (Hero.X > xShape + ShapeBlock.ActualWidth - App.OpenedMainWindow.CollisionValueX &&
                    Hero.X < xShape + ShapeBlock.ActualWidth)
                    Hero.X = xShape + ShapeBlock.ActualWidth;
            }

            if (Hero.X > xShape - Hero./*HeroShape.Actual*/Width &&
                Hero.X < xShape + ShapeBlock.ActualWidth)
            {
                if (
                    Hero.Y > yShape +
                    ShapeBlock.ActualHeight /
                    3 && //сделано пересечение областей, снизу область проверки больше тк в момент прыжка скорость высокая и проскакивает тонкие блоки
                    Hero.Y < yShape + ShapeBlock.ActualHeight+10)
                {
                    Hero.Y = yShape + ShapeBlock.ActualHeight +10;

                }

                if (ShapeParam.Type == TypeShapeParameters.Invisible)
                {
                    CollisionInvisibleBlocks(yShape);
                }

                if (Hero.Y > yShape - Hero.HeroShape.ActualHeight + 10 && //шарик внутри блока
                    Hero.Y < yShape - Hero.HeroShape.ActualHeight + /*GameObject.Shape*/ShapeBlock.ActualHeight / 2)
                {
                    Hero.Y = yShape - Hero.HeroShape.ActualHeight + 10;
                }
            }
        }
        public void Create(List<ShapeParameters> invisibleBlocks)
        {
            foreach (var invisibleBlock in invisibleBlocks)
            {
                var block = new Image { Stretch = Stretch.Fill };

                switch (invisibleBlock.Name)
                {
                    case NameShapeParameters.NewLevel:
                        block.Source = GameResources.NewLevel;
                        block.Width = block.Height = 80;
                        break;
                    case NameShapeParameters.Brick:
                        block.Source = GameResources.SmallBlock1;
                        block.Width = 70;
                        block.Height = 25;
                        break;
                }
                //var visibleBlockObjAndParam = new ObjectAndParameters(block, invisibleBlock);
                var visibleBlock = new VisibleBlocks(block, invisibleBlock/*visibleBlockObjAndParam*/);
                App.OpenedMainWindow.BCanvas.Children.Add(block);
                App.OpenedMainWindow.BlockGameObjects.Add(visibleBlock);
                Canvas.SetLeft(block, invisibleBlock.X + App.OpenedMainWindow.TotalShiftBlocks);
                Canvas.SetTop(block, invisibleBlock.Y);
            }
            App.OpenedMainWindow.ScoreInGame.RenovationScore();
        }
        
        private void CollisionInvisibleBlocks(double yShape)
        {
            if (Hero.Y > yShape - Hero.HeroShape.ActualHeight &&
                Hero.Y < yShape + ShapeBlock.ActualHeight)// HeroEllipse.ActualHeight + invis.Shape.ActualHeight / 2)
            {
                if (ShapeParam.Name == NameShapeParameters.NewLevel)
                    App.OpenedMainWindow.NextGame();
                else
                    Hero.Y = yShape - Hero.HeroShape.ActualHeight;// /2;
            }
        }

    }
}

