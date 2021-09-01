using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace JingleBellsGame
{
    class Frames : IGameObject
    {
        public FrameworkElement ShapeBlock { get; set; }
        public ShapeParameters ShapeParam { get; set; }
        public bool IsRemoved { get; set; }
        //private bool _isFirstPrize = true;
        public Frames(ShapeParameters frameParameters)
        {

            ShapeParam = frameParameters;
        }
       
        public  void Collision(){}

        public  void Create()
        {
            var canvas = new Canvas();
            var block = new Image();
            var text = new TextBlock();
            canvas.Children.Add(block);
            canvas.Children.Add(text);
            ShapeBlock = canvas;

            //GameObject = new ObjectAndParameters(canvas, GameObject.ShapeParam);

            //var textInFrameBlock = new ObjectAndParameters(text, GameObject.ShapeParam);
            //App.OpenedMainWindow.AllBlocks.Add(textInFrameBlock);
            //App.OpenedMainWindow.AllBlocks.Add(frameBlock);
            
            block.Stretch = Stretch.Fill;
            block.Opacity = 0.8;
            text.FontFamily = new FontFamily(new Uri("pack://application:,,,/Fonts/"), "./#Miama Nueva");
            text.TextAlignment = TextAlignment.Center;
            switch (ShapeParam.Type)
            {
                case TypeShapeParameters.Frame:
                    if (App.IsFirstPrize)
                    {
                        block.Source = GameResources.Mess1;
                        block.Width = 150;
                        block.Height = 160;
                        text.Width = block.Width - 30;
                        text.TextWrapping = TextWrapping.Wrap;
                        text.Text = "Welcome! We are glad to see you in our New Years Game! " +
                                    "This is the Prizes! Collect them to get balls in Score";
                        Canvas.SetLeft(text, 11);
                        //GameObject.ShapeParam.Y = CollisionFramesInCanvas(GameObject.ShapeParam, block);
                        Canvas.SetTop(text, 55);
                        App.IsFirstPrize = false;
                    }
                    else
                    {
                        block.Source = GameResources.Mess3;
                        text.TextWrapping = TextWrapping.Wrap;
                        block.Width = 90;
                        block.Height = 80;
                        text.Width = block.Width - 20;
                        var rnd = App.Rnd.Next(1, 3);
                        switch (rnd)
                        {
                            case 1:
                                text.Text = "Вravo!";
                                break;
                            case 2:
                                text.Text = "Cool!";
                                break;
                        }

                        Canvas.SetLeft(text, 9);
                        //GameObject.ShapeParam.Y = CollisionFramesInCanvas(GameObject.ShapeParam, block);
                        Canvas.SetTop(text, 40);
                    }

                    break;
                case TypeShapeParameters.BonusMessage:
                    if (App.IsFirstBonus)
                    {
                        block.Source = GameResources.Mess2;
                        block.Width = 140;
                        block.Height = 110;
                        text.Width = block.Width - 10;
                        text.TextWrapping = TextWrapping.Wrap;
                        text.Text = "This present open the invisible breaks!";
                        Canvas.SetLeft(text, 9);
                        //GameObject.ShapeParam.Y = CollisionFramesInCanvas(GameObject.ShapeParam, block);
                        Canvas.SetTop(text, 53);
                        App.IsFirstBonus = false;
                    }
                    else
                    {
                        block.Source = GameResources.Mess2;
                        block.Width = 140;
                        block.Height = 110;
                        text.Width = block.Width - 10;
                        text.TextWrapping = TextWrapping.Wrap;
                        text.Text = "Take the break!";
                        Canvas.SetLeft(text, 9);
                        //GameObject.ShapeParam.Y = CollisionFramesInCanvas(GameObject.ShapeParam, block);
                        Canvas.SetTop(text, 53);
                    }
                    break;
            }

            canvas.Height = block.Height;
            Canvas.SetLeft(canvas, ShapeParam.X + App.OpenedMainWindow.TotalShiftBlocks);
            ShapeParam.Y = CollisionFramesInCanvas(ShapeParam, canvas);
            Canvas.SetTop(canvas, ShapeParam.Y);
            //var txt = textInFrameBlock as IGameObject;
            App.OpenedMainWindow.BlockGameObjects.Add(this);
            //if (txt != null) App.OpenedMainWindow.BlockGameObjects.Add(txt);
            App.OpenedMainWindow.BCanvas.Children.Add(canvas);
            //App.OpenedMainWindow.BCanvas.Children.Add(text);
            LifetimeMess();
        }
        

       
        private double CollisionFramesInCanvas(ShapeParameters param, Canvas canvasImage)
        {
            if (param.Y > App.OpenedMainWindow.BCanvas.ActualHeight - canvasImage.Height - 10)
                return App.OpenedMainWindow.BCanvas.ActualHeight - canvasImage.Height - 10;
            return param.Y;
            
        }

        private async void LifetimeMess()
        {
            while (ShapeBlock.Opacity > 0)
            {
                await Task.Delay(5);
                ShapeBlock.Opacity -= 0.005;
            }
            App.OpenedMainWindow.BCanvas.Children.Remove(ShapeBlock);
            App.OpenedMainWindow.BlockGameObjects.Remove(this);
        }

    }
}
