using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace JingleBellsGame
{
    public static class Hero
    {
        private static Image HeroImage = new Image();
        public static Grid HeroShape = new Grid();//если приватБ то недоступен в главном классе 
        public static double X;
        public static double Y;
        public static double OldY;

        public static double Height
        {
            get { return HeroShape.ActualHeight; }
            set { HeroShape.Height = value; }
        }
        public static double Width
        {
            get { return HeroShape.ActualWidth; }
            set { HeroShape.Width= value; }
        }

        //public static double Height { get; set; }
        //public static double Width { get; set; }
        public static double X_Canvas = 0;
        private static int Angle = 0;
        private const int CenterX = 25;
        private const int CenterY = 25;

        public static bool FirstDown;
        public static bool FirstUp;
       
        public static RotateTransform RotateTransformHero;
        public static ScaleTransform ScaleTransformHeroShape;

        private static MediaPlayer _up = new MediaPlayer();
        private static MediaPlayer _down = new MediaPlayer();
        private static Uri _upSource = new Uri("music/Up.mp3", UriKind.RelativeOrAbsolute);
        private static Uri _downSource = new Uri("music/Down.mp3", UriKind.RelativeOrAbsolute);

        public static void CreateSnow()
        {
            //if (HeroShape is Image snow)
            //{
            //    snow.Source = GameResources.Snow;
            //    snow.Stretch = Stretch.Fill;
            //    snow.Width = snow.Height = 50;
            //    HeroX = 0;
            //    HeroY = 370;
            //    RotateTransformHero = new RotateTransform(Angle, CenterX, CenterY);
            //    snow.RenderTransform = RotateTransformHero;
            //}
            HeroShape.Children.Add(HeroImage);
            //HeroShape.Background = Brushes.Red;
            //HeroShape.Opacity = 0.5;
            HeroImage.Source = GameResources.Snow;
            HeroImage.Stretch = Stretch.Fill;
            Height = Width/* = HeroShape.Width = HeroShape.Height *//*= HeroImage.Height = HeroImage.Width*/ = 50;
            X = 0;
            Y = 367;//370;
            //Panel.SetZIndex(HeroShape, 2);

            HeroShape.RenderTransformOrigin = new Point(0, 1);

            ScaleTransformHeroShape = new ScaleTransform();
            ScaleTransformHeroShape.ScaleY = 1;
            HeroShape.RenderTransform = ScaleTransformHeroShape;

            RotateTransformHero = new RotateTransform(Angle, CenterX, CenterY);
            HeroImage.RenderTransform = RotateTransformHero;
        }

        private static void UpDown()
        {
            if (FirstDown)
            {
                _down.Volume = App.Settings.RestVolume*0.42;
                _down.Open(_downSource);
                _down.Play();
            }
            else
            {
                _up.Volume = App.Settings.RestVolume * 0.7;
                _up.Open(_upSource);
                _up.Play();
            }
        }
        //public static void AnimationHeroDown()
        //{
        //    Storyboard story = new Storyboard();
        //    DoubleAnimation heroAnimation = new DoubleAnimation();
        //    heroAnimation.From = 1;
        //    heroAnimation.To = 0.7;
        //    //heroAnimation.AccelerationRatio = 0.2;
        //    heroAnimation.Duration = TimeSpan.FromSeconds(0.2);
        //    heroAnimation.AutoReverse = true;
        //    story.Children.Add(heroAnimation);

        //    Storyboard.SetTargetProperty(heroAnimation, new PropertyPath("RenderTransform.ScaleY"));
        //    Storyboard.SetTarget(heroAnimation, HeroShape);

        //    story.Begin();

        //    //////DoubleAnimation buttonAnimation = new DoubleAnimation();
        //    //////buttonAnimation.From = ScaleTransformHeroShape.ScaleY;
        //    //////buttonAnimation.To = ScaleTransformHeroShape.ScaleY / 2;
        //    //////buttonAnimation.Duration = TimeSpan.FromSeconds(0.5);
        //    //////buttonAnimation.AutoReverse = true;
        //    //////HeroShape.BeginAnimation(FrameworkElement.HeightProperty, buttonAnimation);

        //}

        //public static void AnimationHeroUp()
        //{
        //    Storyboard story = new Storyboard();
        //    DoubleAnimation heroAnimation = new DoubleAnimation();
        //    heroAnimation.From = 1;
        //    heroAnimation.To = 1.2;
        //    //heroAnimation.AccelerationRatio = 0.2;
        //    heroAnimation.Duration = TimeSpan.FromSeconds(0.3);
        //    heroAnimation.AutoReverse = true;
        //    story.Children.Add(heroAnimation);

        //    Storyboard.SetTargetProperty(heroAnimation, new PropertyPath("RenderTransform.ScaleY"));
        //    Storyboard.SetTarget(heroAnimation, HeroShape);

        //    story.Begin();
        //}

        public static void AnimationHero()
        {
            Storyboard story = new Storyboard();
            DoubleAnimation heroAnimation = new DoubleAnimation();
            if (FirstDown)
            {
                heroAnimation.From = 1;
                heroAnimation.To = 0.7;
                heroAnimation.Duration = TimeSpan.FromSeconds(0.1);
            }
            else 
            {
                heroAnimation.From = 1;
                heroAnimation.To = 1.2;
                heroAnimation.Duration = TimeSpan.FromSeconds(0.2);
            }
            heroAnimation.AutoReverse = true;
            story.Children.Add(heroAnimation);

            Storyboard.SetTargetProperty(heroAnimation, new PropertyPath("RenderTransform.ScaleY"));
            Storyboard.SetTarget(heroAnimation, HeroShape);

            story.Begin();
        }
        public static void UpdateCoordinatesSnow()
        {
            X = Canvas.GetLeft(HeroShape);
            Y = Canvas.GetTop(HeroShape);
            App.OpenedMainWindow.X_Canvas = Canvas.GetLeft(App.OpenedMainWindow.BCanvas); // важная
        }

       
        public static void UpdateEllipse()
        {

            if (X < 0) X = 0;
            if (!App.IsLava && Y > App.OpenedMainWindow.BCanvas.ActualHeight - /*HeroShape.*/Height)
            {
                Y = App.OpenedMainWindow.BCanvas.ActualHeight - /*HeroShape.*/Height;
            }

            if (Y < 8)  Y = 8;
            App.Speed = Canvas.GetTop(HeroShape) - Y;
            if (Math.Abs(App.Speed) < 0.001)
            {
                App.IsJumped = false;
            }

            if (Math.Abs(OldY - Y) > 0.01) FirstDown = true; // шарик падает

            if (Math.Abs(OldY - Y) <= 0.01 && FirstDown) //шарик стоит на месте
            {
                AnimationHero();
                UpDown();
                FirstDown = false;
            }


            Canvas.SetTop(HeroShape, Y);
            Canvas.SetLeft(HeroShape, X);
        }
        public static void Jump()
        {
            if (App.IsJumped)
            {
                if (App.CounterStepJump < 25)
                {
                    if (App.OpenedMainWindow != null)
                    {
                        var shift = MainWindow.Shift * 4.5 - App.CounterStepJump;
                        Y -= shift;
                        if (FirstUp)
                        {
                            AnimationHero();
                            UpDown();
                            FirstUp = false;
                        }
                    }

                    App.CounterStepJump += 1;
                }
            }
            else
            {
                if (App.CounterStepJump > 0.1) App.CounterStepJump = 0;
                FirstUp = true;
            }

        }
        public static void Gravity()
        {
            OldY = Y;
            if (App.IsLava && Y <= App.OpenedMainWindow.BCanvas.ActualHeight)Y += 1;
            else Y += 8;
        }
    }
}
