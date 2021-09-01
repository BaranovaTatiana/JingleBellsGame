using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Mime;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Serialization;
using WpfAnimatedGif;
using JingleBellsGame.Annotations;

namespace JingleBellsGame
{
    public sealed partial class MainWindow : Window
    {
        //public double X_HeroEllipse = 0;
        //public double Y_HeroEllipse = 0;
        //private bool IsJumped = false;
        //private double CounterStepJump = 0;
        //private double Speed = 0;
        // public TextBlock ScoreBox = new TextBlock();
        //public Image ScoreImage = new Image();
        //public double AngleScore = 0;
        //public int Score = 0;
        
        
        private Uri _winMediaSource = new Uri("music/win.mp3", UriKind.RelativeOrAbsolute);
        public Uri MainMuseMediaSource = new Uri("music/mainmuse.mp3", UriKind.RelativeOrAbsolute);

         public List<ShapeParameters> InfoAboutBlocks = new List<ShapeParameters>();
        public List<IGameObject> BlockGameObjects = new List<IGameObject>();
         private List<Key> ListOfKeys = new List<Key>();
         public List<FrameworkElement> SnowFlakes = new List<FrameworkElement>();

        public const int Shift = 5;
        public int CollisionValueX = Shift * 2;
       
        public double X_Canvas = 0;
       
        
        private bool MoveRight = false;
        private bool MoveLeft = false;

        private double LookForMaxCoordinateForEllipse_Right = (double) 2 / 3; // коэффициент для того чтобы шарик не ушел в противополжный край экрана сильно далеко
        private double LookForMaxCoordinateForEllipse_Left = (double) 1 / 3; // обратный коэффициент
        
        
        public TimeSpan EndMuse = new TimeSpan(0, 0, 1, 16);
        public TimeSpan BeginMuse = new TimeSpan(0, 0, 0, 1);

        public bool IsWin = false;
        public bool GameStart = true;
        private bool IsFirstInitSnowFlakes = true;
        
        
         //public MediaPlayer MainMuse = new MediaPlayer();
         //public MediaPlayer LavaMedia = new MediaPlayer();
         //public MediaPlayer Hoho = new MediaPlayer();
         //public MediaPlayer Bell = new MediaPlayer();

         private MediaPlayer _mainMuse = App.MainMuse;


        private TextBlock TextWin = new TextBlock();

        public int TotalShiftBlocks = 0;
        public Score ScoreInGame;
        public MenuWindow Menu = null;// = new MenuWindow();
        private bool _isGameOver = false;
        private TextBlock _isGameOverText = new TextBlock();

       
        public MainWindow()
        {
           

            InitializeComponent();
            //UpdateCoordinatesSnow(); //обновление координат

            App.Settings.Load();
            App.OpenedMainWindow = this;
            
            InitInfoAboutBlocks(); //инициализация, заполнение полей
            Hero.CreateSnow();
            BCanvas.Children.Add(Hero.HeroShape);

            Canvas.SetLeft(Hero.HeroShape, Hero.X);
            Canvas.SetTop(Hero.HeroShape, Hero.Y);
            Hero.UpdateCoordinatesSnow();
            CreateBlocKs();
            
            CreateButtonMenu();

            ScoreInGame = new Score(0, null);
            ScoreInGame.Create();
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
            MouseDown += MouseClick_OnMainWindow;
            MouseLeftButtonDown += OnMouseLeftButtonDown;
            MainMusic();
            TimerStart();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pointLeft = Left;
            var pointTop = Top; 
            DragMove();
            e.Handled = Math.Abs(pointLeft - Left) > 20 || Math.Abs(pointTop - Top) > 20;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Menu = new MenuWindow {Owner = this};
            OpenedMenu = true;
            Menu.Show();
        }

        private void MouseClick_OnMainWindow(object sender, MouseEventArgs e)
        {
            if (Menu == null) return;
            Menu.Close();
            OpenedMenu = false;
        }
        public void CreateButtonMenu()
        {
            
            var imageMenu = new Image {Source = GameResources.MenuIm, Width = 130, Height = 110};

            //var buttonMenu = new Button {Width = 102, Height = 35, /*Opacity = 0*/};
            RotateTransform button = new RotateTransform(12, ButtonMenu.Width / 2, 0);
            ButtonMenu.RenderTransform = button;
            ButtonMenu.Click += ButtonMenu_Click;

            var textMenu = new TextBlock();
            textMenu.Width = 48;
            textMenu.Height = 12;
            textMenu.Text = "MENU";
            RotateTransform menu = new RotateTransform(12, ButtonMenu.Width / 2, 0);
            textMenu.RenderTransform = menu;
            textMenu.FontFamily = new FontFamily(new Uri("pack://application:,,,/Fonts/"), "./#Miama Nueva");
            
            ButtonMenu.Background = Brushes.AliceBlue;
            ButtonMenu.IsCancel = true;
            BCanvas.Children.Add(imageMenu);
            Canvas.SetLeft(imageMenu, 0);//BCanvas.ActualWidth / 10);
            Canvas.SetTop(imageMenu, 0); //BCanvas.ActualHeight / 10);

            BCanvas.Children.Add(textMenu);
            Canvas.SetLeft(textMenu, 55);//BCanvas.ActualWidth / 10);
            Canvas.SetTop(textMenu, 60); //BCanvas.ActualHeight / 10);

            //BCanvas.Children.Add(buttonMenu);
            Canvas.SetLeft(ButtonMenu, 26);//BCanvas.ActualWidth / 10);   26 14
            Canvas.SetTop(ButtonMenu, 41); //BCanvas.ActualHeight / 10);    41 27
        }

        public bool OpenedMenu = true;
        public void ButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            if (OpenedMenu)
            {
                Menu.Close();
                OpenedMenu = false;
            }
            else
            {
                
                Menu = new MenuWindow {Owner = this};
                OpenedMenu = true;
                if (_menuForLava) Menu.BigRestart.Visibility = Visibility.Visible;
                Menu.Show();
            }
        }
        private void InitInfoAboutBlocks()
        {
            int x = 250;
            int y = 325;
            int stepSmall = 140;
            int stepBig = 200;
            int stepBonusBlock = 70;

            ShapeParameters block1 = new ShapeParameters();
            block1.X = x;
            block1.Y = y;
            block1.Size = SizeShapeParameters.Small;
            block1.Type = TypeShapeParameters.Visible;
            InfoAboutBlocks.Add(block1);

            ShapeParameters block2 = new ShapeParameters();
            block2.X = x += stepSmall;
            block2.Y = y -= 35;
            block2.Size = SizeShapeParameters.Small;
            block2.Type = TypeShapeParameters.Visible;
            InfoAboutBlocks.Add(block2);

            ShapeParameters block3 = new ShapeParameters();
            block3.X = x += stepSmall;
            block3.Y = y -= 65;
            block3.Size = SizeShapeParameters.Big;
            block3.Type = TypeShapeParameters.Visible;
            InfoAboutBlocks.Add(block3);

            ShapeParameters prize1 = new ShapeParameters
            {
                X = 578,
                Y = 120,
                Type = TypeShapeParameters.Prize,
            };
            InfoAboutBlocks.Add(prize1);

            ShapeParameters prize1a = new ShapeParameters
            {
                X = 770,
                Y = 280,
                Type = TypeShapeParameters.Prize,
            };
            InfoAboutBlocks.Add(prize1a);

            ShapeParameters block4 = new ShapeParameters();
            block4.X = x += stepBig;
            block4.Y = y -= 65;
            block4.Size = SizeShapeParameters.Small;
            block4.Type = TypeShapeParameters.Visible;
            InfoAboutBlocks.Add(block4);

            ShapeParameters block5 = new ShapeParameters();
            block5.X = x += stepSmall; //870
            block5.Y = y -= 40; //120
            block5.Size = SizeShapeParameters.Small;
            block5.Type = TypeShapeParameters.Visible;
            InfoAboutBlocks.Add(block5);

            ShapeParameters prize2 = new ShapeParameters
            {
                X = x + 28,
                Y = 30,
                Type = TypeShapeParameters.Prize,
            };
            InfoAboutBlocks.Add(prize2);

            ShapeParameters block6 = new ShapeParameters();
            block6.X = x += stepSmall;
            block6.Y = y += 40;
            block6.Size = SizeShapeParameters.Big;
            block6.Type = TypeShapeParameters.Visible;
            InfoAboutBlocks.Add(block6);

            ShapeParameters block7 = new ShapeParameters();
            block7.X = x += stepBig; //1210
            block7.Y = y += 80; //240
            block7.Size = SizeShapeParameters.Small;
            block7.Type = TypeShapeParameters.Visible;
            InfoAboutBlocks.Add(block7);

            ShapeParameters block8 = new ShapeParameters();
            block8.X = x += stepSmall;
            block8.Y = y += 90;
            block8.Size = SizeShapeParameters.Small;
            block8.Type = TypeShapeParameters.Visible;
            InfoAboutBlocks.Add(block8);

            ShapeParameters block9 = new ShapeParameters();
            block9.X = x += stepBig;
            block9.Y = y -= 96;
            block9.Size = SizeShapeParameters.Big;
            block9.Type = TypeShapeParameters.Visible;
            InfoAboutBlocks.Add(block9);

            ShapeParameters prize1b = new ShapeParameters
            {
                X = 1598,
                Y = 25,
                Type = TypeShapeParameters.Prize,
            };
            InfoAboutBlocks.Add(prize1b);

            ShapeParameters block10 = new ShapeParameters();
            block10.X = x += stepBig;
            block10.Y = y -= 50;
            block10.Size = SizeShapeParameters.Small;
            block10.Type = TypeShapeParameters.Visible;
            InfoAboutBlocks.Add(block10);

            ShapeParameters block11bonus1 = new ShapeParameters();
            block11bonus1.X = x += stepBonusBlock;
            block11bonus1.Y = y += 160;
            block11bonus1.Type = TypeShapeParameters.Invisible; //невидимый при видимости размер сделать меньше(stepBonusBlock) и желтым цветом - лестница
            block11bonus1.Name = NameShapeParameters.Brick;

            ShapeParameters block12bonus1 = new ShapeParameters();
            block12bonus1.X = x += stepBonusBlock;
            block12bonus1.Y = y -= 60;
            block12bonus1.Type = TypeShapeParameters.Invisible; //невидимый
            block12bonus1.Name = NameShapeParameters.Brick;

            ShapeParameters block13bonus1 = new ShapeParameters();
            block13bonus1.X = x += stepBonusBlock;
            block13bonus1.Y = y -= 60;
            block13bonus1.Type = TypeShapeParameters.Invisible; //невидимый
            block13bonus1.Name = NameShapeParameters.Brick;

            ShapeParameters block14bonus1 = new ShapeParameters();
            block14bonus1.X = x += stepBonusBlock;
            block14bonus1.Y = y -= 60;
            block14bonus1.Type = TypeShapeParameters.Invisible; //невидимый
            block14bonus1.Name = NameShapeParameters.Brick;

            ShapeParameters bonus1 = new ShapeParameters
            {
                X = 1750,
                Y = 300,
                Type = TypeShapeParameters.Bonus,
                Name = NameShapeParameters.HelpMessage,
                InvisibleBlocks = new List<ShapeParameters>

                {
                    block11bonus1,
                    block12bonus1,
                    block13bonus1,
                    block14bonus1
                }
            };
            InfoAboutBlocks.Add(bonus1);

            ShapeParameters prize3 = new ShapeParameters
            {
                X = 2148,
                Y = 20,
                Type = TypeShapeParameters.Prize
            };
            InfoAboutBlocks.Add(prize3);

            ShapeParameters block15 = new ShapeParameters();
            block15.X = x += stepBonusBlock;
            block15.Y = y -= 60;
            block15.Size = SizeShapeParameters.Big;
            block15.Type = TypeShapeParameters.Visible;
            InfoAboutBlocks.Add(block15);

            ShapeParameters block16 = new ShapeParameters();
            block16.X = x += stepBig;
            block16.Y = y += 70;
            block16.Size = SizeShapeParameters.Small;
            block16.Type = TypeShapeParameters.Visible;
            InfoAboutBlocks.Add(block16);

            ShapeParameters block1bonus2 = new ShapeParameters
            {
                X = 2950,
                Y = 250,
                Type = TypeShapeParameters.Invisible,
                Name = NameShapeParameters.Brick
            };

            ShapeParameters bonus2 = new ShapeParameters
            {
                X = x,
                Y = y + 150,
                Type = TypeShapeParameters.Bonus,
                InvisibleBlocks = new List<ShapeParameters> {block1bonus2}
            };
            InfoAboutBlocks.Add(bonus2);

            ShapeParameters prize10 = new ShapeParameters
            {
                X = 2968,
                Y = 170,
                Type = TypeShapeParameters.Prize
            };
            InfoAboutBlocks.Add(prize10);

            ShapeParameters block17 = new ShapeParameters();
            block17.X = x += stepSmall;
            block17.Y = y += 55;
            block17.Size = SizeShapeParameters.Small;
            block17.Type = TypeShapeParameters.Visible;
            InfoAboutBlocks.Add(block17);

            ShapeParameters block18 = new ShapeParameters();
            block18.X = x += stepSmall;
            block18.Y = y -= 45;
            block18.Size = SizeShapeParameters.Small;
            block18.Type = TypeShapeParameters.Visible;
            InfoAboutBlocks.Add(block18);

            ShapeParameters prize4 = new ShapeParameters
            {
                X = x + 30,
                Y = y - 120,
                Type = TypeShapeParameters.Prize
            };
            InfoAboutBlocks.Add(prize4);

            ShapeParameters prize4a = new ShapeParameters
            {
                X = 2500,
                Y = 260,
                Type = TypeShapeParameters.Prize
            };
            InfoAboutBlocks.Add(prize4a);

            ShapeParameters lava = new ShapeParameters
            {
                X = 2800, //2835, //2850
                Y = 370, //360
                Size = SizeShapeParameters.Small,
                Type = TypeShapeParameters.Lava
            };
            InfoAboutBlocks.Add(lava);

            ShapeParameters block19 = new ShapeParameters
            {
                X = x += stepSmall,
                Y = y += 150,
                Size = SizeShapeParameters.Big,
                Type = TypeShapeParameters.Visible,
            };
            InfoAboutBlocks.Add(block19);

            ShapeParameters block20 = new ShapeParameters
            {
                X = x += stepBig + stepSmall,
                Y = y,
                Size = SizeShapeParameters.Big,
                Type = TypeShapeParameters.Visible,
            };
            InfoAboutBlocks.Add(block20);
            y += 60;
            ShapeParameters block21 = new ShapeParameters
            {
                X = x += stepBig,
                Y = y -= 70,
                Size = SizeShapeParameters.Small,
                Type = TypeShapeParameters.Visible,
            };
            InfoAboutBlocks.Add(block21);

            ShapeParameters block22bonus4 = new ShapeParameters
            {
                X = 3340,
                Y = 250,
                Type = TypeShapeParameters.Invisible,
                Name = NameShapeParameters.Brick
            };

            ShapeParameters bonus4 = new ShapeParameters
            {
                X = 3568,
                Y = 250,
                Type = TypeShapeParameters.Bonus,
                InvisibleBlocks = new List<ShapeParameters> {block22bonus4}
            };
            InfoAboutBlocks.Add(bonus4);

            ShapeParameters prize5 = new ShapeParameters
            {
                X = 3568,
                Y = 50,
                Type = TypeShapeParameters.Prize
            };
            InfoAboutBlocks.Add(prize5);

            x += stepBonusBlock;
            y -= 70;
            x += stepBonusBlock;

            ShapeParameters block23 = new ShapeParameters
            {

                X = x += stepSmall,
                Y = y -= 100,
                Size = SizeShapeParameters.Small,
                Type = TypeShapeParameters.Visible,
            };
            InfoAboutBlocks.Add(block23);

            ShapeParameters block24 = new ShapeParameters
            {
                X = x += stepBig,
                Y = y += 80,
                Size = SizeShapeParameters.Big,
                Type = TypeShapeParameters.Visible,
            };
            InfoAboutBlocks.Add(block24);

            ShapeParameters block25 = new ShapeParameters
            {
                X = x += stepBig,
                Y = y += 27,
                Size = SizeShapeParameters.Small,
                Type = TypeShapeParameters.Visible,
            };
            InfoAboutBlocks.Add(block25);

            ShapeParameters block26 = new ShapeParameters
            {
                X = x += stepSmall,
                Y = y -= 75,
                Size = SizeShapeParameters.Small,
                Type = TypeShapeParameters.Visible,
            };
            InfoAboutBlocks.Add(block26);

            x += stepSmall;
            y -= 75;

            ShapeParameters block28bb = new ShapeParameters // бонус лестница вниз
            {
                X = x += stepBonusBlock,
                Y = y += 60,
                Type = TypeShapeParameters.Invisible,
                Name = NameShapeParameters.Brick
            };

            ShapeParameters block29bb = new ShapeParameters // бонус лестница вниз
            {
                X = x += stepBonusBlock,
                Y = y += 60,
                Type = TypeShapeParameters.Invisible,
                Name = NameShapeParameters.Brick
            };

            ShapeParameters block30bb = new ShapeParameters // бонус лестница вниз
            {
                X = x += stepBonusBlock,
                Y = y += 60,
                Type = TypeShapeParameters.Invisible,
                Name = NameShapeParameters.Brick
            };

            ShapeParameters bonus6 = new ShapeParameters
            {
                X = 4248, //x + 28,
                Y = 60, //y -45,
                Type = TypeShapeParameters.Bonus,
                InvisibleBlocks = new List<ShapeParameters>
                {
                    block28bb,
                    block29bb,
                    block30bb
                }
            };
            InfoAboutBlocks.Add(bonus6);

            ShapeParameters block27 = new ShapeParameters // сделать лаву растаявший лед в озере 
            {
                X = 4080 + stepSmall, //x += stepSmall, //4080+stepSmall
                Y = 186 - 75, //y-=75,  //186-75
                Size = SizeShapeParameters.Small,
                Type = TypeShapeParameters.Visible,
            };
            InfoAboutBlocks.Add(block27);

            ShapeParameters lava2 = new ShapeParameters
            {
                X = 4545, //130
                Y = 371, //y += 20, //371
                Size = SizeShapeParameters.Big,
                Type = TypeShapeParameters.Lava
            };
            InfoAboutBlocks.Add(lava2);

            ShapeParameters block31 = new ShapeParameters //островок
            {
                X = x += stepBonusBlock,
                Y = y += 60,
                Size = SizeShapeParameters.Big,
                Type = TypeShapeParameters.Visible,
            };
            InfoAboutBlocks.Add(block31);

            y += 20;
            x += 110;
            ShapeParameters block32a = new ShapeParameters //льдина
            {
                X = x += 90, //70
                Y = y -= 40,
                Size = SizeShapeParameters.Small,
                Type = TypeShapeParameters.Visible,
            };
            InfoAboutBlocks.Add(block32a);

            ShapeParameters block32b = new ShapeParameters //льдина
            {
                X = x += stepSmall,
                Y = y -= 30,
                Size = SizeShapeParameters.Small,
                Type = TypeShapeParameters.Visible,
            };
            InfoAboutBlocks.Add(block32b);

            ShapeParameters block32c = new ShapeParameters //льдина
            {
                X = x += stepSmall,
                Y = y -= 30,
                Size = SizeShapeParameters.Small,
                Type = TypeShapeParameters.Visible,
            };
            InfoAboutBlocks.Add(block32c);

            ShapeParameters block32dbonus1 = new ShapeParameters //льдина
            {
                X = 4990 + stepSmall,
                Y = 255,
                //Size = Size_ShapeParameters.Small,
                Type = TypeShapeParameters.Invisible,
                Name = NameShapeParameters.Brick
            };
            InfoAboutBlocks.Add(block32dbonus1);

            ShapeParameters bonus7 = new ShapeParameters
            {
                X = x += 28,
                Y = y - 180,
                Type = TypeShapeParameters.Bonus,
                InvisibleBlocks = new List<ShapeParameters> {block32dbonus1}
            };
            InfoAboutBlocks.Add(bonus7);

            y += 20;
            ShapeParameters block33 = new ShapeParameters //островок
            {
                X = x += stepBig + 20,
                Y = y += 15,
                Size = SizeShapeParameters.Big,
                Type = TypeShapeParameters.Visible,
            };
            InfoAboutBlocks.Add(block33);

            ShapeParameters block33a = new ShapeParameters
            {
                X = x += stepBig,
                Y = y += 15,
                Size = SizeShapeParameters.Small,
                Type = TypeShapeParameters.Visible,
            };
            InfoAboutBlocks.Add(block33a);

            ShapeParameters block34 = new ShapeParameters
            {
                X = x += stepBig,
                Y = y -= 80,
                Size = SizeShapeParameters.Big,
                Type = TypeShapeParameters.Visible,
            };
            InfoAboutBlocks.Add(block34);

            ShapeParameters prize8 = new ShapeParameters
            {
                X = x + 48,
                Y = y - 160,
                Type = TypeShapeParameters.Prize
            };
            InfoAboutBlocks.Add(prize8);

            ShapeParameters block35 = new ShapeParameters
            {
                X = x += stepBig,
                Y = y -= 90,
                Size = SizeShapeParameters.Small,
                Type = TypeShapeParameters.Visible,
            };
            InfoAboutBlocks.Add(block35);

            ShapeParameters block36 = new ShapeParameters
            {
                X = x += stepBig,
                Y = y -= 90,
                Size = SizeShapeParameters.Small,
                Type = TypeShapeParameters.Visible,
            };
            InfoAboutBlocks.Add(block36);

            ShapeParameters newLevel = new ShapeParameters
            {
                X = 6400,
                Y = 280,
                Type = TypeShapeParameters.Invisible,
                Name = NameShapeParameters.NewLevel
            };

            ShapeParameters portal = new ShapeParameters
            {
                X = x += 170,
                Y = y + 30,
                Type = TypeShapeParameters.Portal,
                InvisibleBlocks = new List<ShapeParameters> {newLevel}
            };
            InfoAboutBlocks.Add(portal);

            ShapeParameters block1dbonus8 = new ShapeParameters
            {
                X = 5820,
                Y = 320,
                //Size = Size_ShapeParameters.Small,
                Type = TypeShapeParameters.Invisible,
                Name = NameShapeParameters.Brick
            };

            ShapeParameters bonus8 = new ShapeParameters
            {
                X = 6048, //x,//6048
                Y = 241, //y + 180,//61
                Type = TypeShapeParameters.Bonus,
                InvisibleBlocks = new List<ShapeParameters>
                {
                    block1dbonus8
                }
            };
            InfoAboutBlocks.Add(bonus8);


        }

        private void DeleteBlocks()
        {
            while ( BlockGameObjects.Count > 0)
            {
                var obj = BlockGameObjects[0];
                BlockGameObjects.Remove(obj);
                BCanvas.Children.Remove(obj.ShapeBlock);
            }
        }

        private void CreateBlocKs()
        {
            TotalShiftBlocks = 0;
            if (GameStart)
            {
                foreach (var blockParameters in InfoAboutBlocks)
                {
                    var block = new Image();
                    BCanvas.Children.Add(block);
                    Canvas.SetLeft(block, blockParameters.X);
                    Canvas.SetTop(block, blockParameters.Y);
                    block.Stretch = Stretch.Fill;
                    IGameObject gameObj = null;
                    switch (blockParameters.Type)
                    {
                        case TypeShapeParameters.Lava:
                            gameObj = new Lava(block, blockParameters);
                            break;
                        case TypeShapeParameters.Bonus:
                            gameObj = new Bonuses(block, blockParameters);
                            break;
                        case TypeShapeParameters.Prize:
                            gameObj = new Prizes(block, blockParameters);
                            break;
                        case TypeShapeParameters.Portal:
                            gameObj = new Portals(block, blockParameters);
                            break;
                        case TypeShapeParameters.Visible:
                            gameObj = new VisibleBlocks(block, blockParameters);
                            break;
                        case TypeShapeParameters.Invisible:
                            break;
                        case TypeShapeParameters.Frame:
                            break;
                        case TypeShapeParameters.BonusMessage:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();

                    }
                    if (gameObj != null)
                    {
                        BlockGameObjects.Add(gameObj);
                        gameObj.Create();
                    }
                }

                GameStart = false;
            }
        }

        private void InitSnowFlakes()
        {
            while (SnowFlakes.Count < 150)
            {
                Image snowFlake = new Image();
                var rnd = App.Rnd.Next(1, 4);
                switch (rnd)
                {
                    case 1:
                        snowFlake.Source = GameResources.SnowFlake1Picture;
                        break;
                    case 2:
                        snowFlake.Source = GameResources.SnowFlake2Picture;
                        break;
                    case 3:
                        snowFlake.Source = GameResources.SnowFlake3Picture;
                        break;
                }

                snowFlake.Height = snowFlake.Width = App.Rnd.Next(15, 60);
                SnowFlakes.Add(snowFlake);
                if (snowFlake.Height < 30) BCanvas.Children.Insert(0, snowFlake);
                else BCanvas.Children.Add(snowFlake);
                Canvas.SetLeft(snowFlake,
                    App.Rnd.Next((int) -(snowFlake.Height + BCanvas.ActualWidth), (int) BCanvas.ActualWidth * 2));
                if (IsFirstInitSnowFlakes)
                    Canvas.SetTop(snowFlake, App.Rnd.Next((int) -snowFlake.Width, (int) BCanvas.ActualHeight));
                else
                    Canvas.SetTop(snowFlake, -snowFlake.Width);
            }

            IsFirstInitSnowFlakes = false;
        }

        private void MoveSnowFlakes()
        {
            for (var i = 0; i < SnowFlakes.Count; i++)
            {
                var snowFlake = SnowFlakes[i];
                double xSnowFlake = Canvas.GetLeft(snowFlake);
                double ySnowFlake = Canvas.GetTop(snowFlake);
                if (ySnowFlake > BCanvas.ActualHeight ||
                    xSnowFlake < -snowFlake.ActualWidth - BCanvas.ActualWidth ||
                    xSnowFlake > BCanvas.ActualWidth * 2)
                {
                    BCanvas.Children.Remove(snowFlake);
                    SnowFlakes.Remove(snowFlake);
                }

                ySnowFlake += snowFlake.Width / 20;
                Canvas.SetTop(snowFlake, ySnowFlake);
            }
            //if (Y_SnowFlakes1 > BCanvas.ActualHeight || 
            //    X_SnowFlakes1 < -SnowFlake1.Width || 
            //    X_SnowFlakes1 > BCanvas.ActualWidth)
            //{

            //    SnowFlake1.Width = Rnd.Next(30,120);
            //    SnowFlake1.Height = SnowFlake1.Width;
            //    Y_SnowFlakes1 = -SnowFlake1.ActualHeight;
            //    X_SnowFlakes1 = Rnd.Next((int)-SnowFlake1.Width, (int)BCanvas.ActualWidth);
            //}
            //Y_SnowFlakes1 += Shift - 3;
        }

        private void TimerStart()
        {
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimerOnTick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);
            dispatcherTimer.Start();
        }

        
        private void MainMusic()
        {
            _mainMuse.Open(MainMuseMediaSource);
            _mainMuse.Position = BeginMuse;
            _mainMuse.Volume = App.Settings.MainMuseVolume;
            _mainMuse.Play();
            _mainMuse.MediaEnded += MainMuseOnMediaEnded;
        }

        private void MainMuseOnMediaEnded(object sender, EventArgs e)
        {
            _mainMuse.Position = IsWin ? TimeSpan.Zero : BeginMuse;
            _mainMuse.Play();
        }

        private void CollisionBlockGameObjects()
        {
            for (int i = 0; i < BlockGameObjects.Count; i++)
            {
                var blockGameObject = BlockGameObjects[i];
                blockGameObject.Collision();
                if (blockGameObject.IsRemoved) i--;
            }
            //for (var i = 0; i < AllBlocks.Count; i++)
            //{
            //    var objAndParam = AllBlocks[i];

            //    switch (objAndParam.ShapeParam.Type)
            //    {
            //        case TypeShapeParameters.Visible:
            //            Collision();
            //            //CollisionShape(objAndParam);
            //            break;
            //        case TypeShapeParameters.Bonus:
            //            if (CollisionBonusBlock(objAndParam)) i--;
            //            break;
            //        case TypeShapeParameters.Lava:
            //            if (IsLava) break;
            //            else
            //            {
            //                Collision();
            //                IsLava = true;
            //            }
            //            //CollisionLava(objAndParam);
            //            break;
            //        case TypeShapeParameters.Prize:
            //            if (CollisionPrizes(objAndParam))
            //            {
            //                StepSwingImage = 1;
            //                i--;
            //            }
            //            break;
            //        case TypeShapeParameters.Portal:
            //            Collision();
            //            break;
            //        case TypeShapeParameters.Invisible:
            //            if (objAndParam.ShapeParam.Name== NameShapeParameters.Welcome) OpacityTextBlock(objAndParam);
            //            else Collision();//CollisionShape(objAndParam);
            //            break;
            //        case TypeShapeParameters.Frame:
            //        {

            //            OpacityTextBlock(objAndParam);
            //        }
            //            break;
            //        case TypeShapeParameters.BonusMessage:
            //            OpacityTextBlock(objAndParam);
            //            break;
            //    }
            //}
        }

        private bool _isOpacityGameOverText = true;
        private void DispatcherTimerOnTick(object sender, EventArgs e)
        {
            if (!_isOpacityGameOverText) OpacityText();
            if (_isGameOver) GameOverInLava();
            
            Keys();
            Hero.Jump();
            Hero.Gravity();
            CollisionBlockGameObjects();
            ScoreInGame.SwingScoreImage();
            Hero.UpdateEllipse();
            InitSnowFlakes();
            MoveSnowFlakes();
        }

        private void GameOverInLava()
        {
            Menu.Opacity += 0.005;
            GameOverText.Opacity -= 0.01;
            if (Menu.Opacity >= 1)
            {
                _isGameOver = false;
                Menu.Opacity = 1;
            }
        }
        
        
        
        public bool CheckCollision( FrameworkElement specialShape)
        {
            double xShape = Canvas.GetLeft(specialShape);
            double yShape = Canvas.GetTop(specialShape);
            return Hero.X > xShape - Hero.HeroShape.ActualWidth &&
                   Hero.X < xShape + specialShape.ActualWidth &&
                   Hero.Y < yShape + specialShape.ActualHeight && 
                   Hero.Y > yShape - Hero.HeroShape.ActualHeight + specialShape.ActualHeight / 2;
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!ListOfKeys.Contains(e.Key)) ListOfKeys.Add(e.Key);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            ListOfKeys.Remove(e.Key);
            MoveRight = false;
            MoveLeft = false;
        }

        private void Keys()
        {
            if (!App.IsLava)
            {
                foreach (var key in ListOfKeys)
                {
                    switch (key)
                    {

                        case Key.Left:
                            {
                                MoveLeft = true;
                                Hero.RotateTransformHero.Angle -= 360 / (Hero.HeroShape.ActualHeight * 3.14) * Shift;
                                if (Hero.X > BCanvas.ActualWidth * LookForMaxCoordinateForEllipse_Left)
                                    Hero.X -= Shift;
                                else MoveShapes();
                                break;
                            }

                        case Key.Right:
                            {
                                MoveRight = true;
                                Hero.RotateTransformHero.Angle += 360 / (Hero.HeroShape.ActualHeight * 3.14) * Shift;
                                if (Hero.X < BCanvas.ActualWidth * LookForMaxCoordinateForEllipse_Right -
                                    Hero.HeroShape.ActualWidth)
                                    Hero.X += Shift;
                                else MoveShapes();
                                break;
                            }

                        case Key.Up:
                            {
                                if (Math.Abs(App.Speed) < 0.001) App.IsJumped = true;
                                
                                break;
                            }
                    }

                }
            }

        }
        private void MoveShapes()
        {
            foreach (var snowFlake in SnowFlakes)
            {
                double xShape = Canvas.GetLeft(snowFlake);
                if (MoveRight)
                {
                    xShape -= Shift;
                }
                if (MoveLeft)
                {
                    xShape += Shift;
                }
                UpdateShapes(snowFlake, xShape);
            }

            if (MoveRight) TotalShiftBlocks -= Shift;
            if (MoveLeft) TotalShiftBlocks += Shift;
            foreach (var gameObject in BlockGameObjects)
            {
                double xShape = Canvas.GetLeft(gameObject.ShapeBlock);
                if (MoveRight)
                {
                    xShape -= Shift;
                }
                if (MoveLeft)
                {
                    xShape += Shift;
                }
                UpdateShapes(gameObject.ShapeBlock, xShape);
            }

            MoveRight = false;
            MoveLeft = false;
        }
        private void UpdateShapes(FrameworkElement shape, double xShape)
        {
            Canvas.SetLeft(shape, xShape);
        }

        private int StepSnowDown = 1;
        private int StepSnowUp = 5;


        private bool _menuForLava = false;
        public void OpacityText()
        {
            //_isGameOverText.Opacity += 0.009;
            //if (_isGameOverText.Opacity >= 0.7)
            //{
            //    _isGameOver = true;
            //    _isOpacityGameOverText = true;
            //}
            GameOverText.Opacity += 0.009;
            if (GameOverText.Opacity >= 0.7)
            {
                _isGameOver = true;
                _isOpacityGameOverText = true;
            }
        }

        public void GameOver1()
        {
        }

        public void GameOver()
        {
            //_isGameOver = true;
            _isOpacityGameOverText = false;

            //BCanvas.Children.Add(_isGameOverText);
            //Canvas.SetLeft(_isGameOverText, BCanvas.ActualWidth / 2 - 150);
            //Canvas.SetTop(_isGameOverText, BCanvas.ActualHeight / 2 - 100);
            //_isGameOverText.FontSize = 50;
            //_isGameOverText.FontFamily = new FontFamily(new Uri("pack://application:,,,/Fonts/"), "./#Miama Nueva");
            //_isGameOverText.Opacity = 0;
            //_isGameOverText.Text = "Game over !!!";
            GameOverText.Visibility = Visibility.Visible;

            Menu = new MenuWindow {Owner = this, BigRestart = {Visibility = Visibility.Visible}, Opacity = 0};
            Menu.Show();
            OpenedMenu = true;
            _menuForLava = true;
        }


        public void Win()
        {
            if (IsWin) return;
            _mainMuse.Stop();
            _mainMuse.Open(_winMediaSource);
            //App.MainMuse.Volume = 1;
            _mainMuse.Play();


            //TextWin.Foreground = Brushes.DarkBlue;
            //TextWin.FontFamily = new FontFamily(new Uri("pack://application:,,,/Fonts/"), "./#Miama Nueva");
            //BCanvas.Children.Add(TextWin);
            //Canvas.SetLeft(TextWin, BCanvas.ActualWidth / 4 - 130);
            //Canvas.SetTop(TextWin, BCanvas.ActualHeight - 200);
            //TextWin.FontSize = 40;
            //TextWin.Text = "YOU WIN !!! FANTASTIC !!!";

            Menu = new MenuWindow
            {
                Owner = this,
                Opacity = 0,
                ForWinner = {Visibility = Visibility.Visible},
                Resume = {Content = "Restart"}
            };

            //Menu.BigRestart.Width = Menu.Resume.Width;
            //Menu.BigRestart.Height = Menu.Resume.Height;
            //Menu.BigRestart.Visibility = Visibility.Visible;
            Menu.Show();
            _isGameOver = true;
            IsWin = true;
        }

        
        public void NextGame()
        {
            if (GameOverText.Visibility == Visibility.Visible) GameOverText.Visibility = Visibility.Collapsed;
            if (Menu.ForWinner.Visibility == Visibility.Visible) Menu.ForWinner.Visibility = Visibility.Collapsed;
            if ((string) Menu.Resume.Content == "Restart") Menu.Resume.Content = "Resume";
            Menu.Close();
            App.IsFirstPrize = true;
            App.IsFirstBonus = true;
            Menu.BigRestart.Visibility = Visibility.Collapsed;
            GameStart = true;
            IsWin = true;
            App.IsLava = false;
            BCanvas.Children.Remove(_isGameOverText);
            DeleteBlocks();
            Hero.X = 0;
            Hero.Y = 367;
            BCanvas.Children.Remove(TextWin);
            CreateBlocKs();
            MainMusic();
            ScoreInGame.AddScore();
            ScoreInGame.RenovationScore();
            ListOfKeys.Clear();
            OpenedMenu = false;
            MoveRight = false;
            MoveLeft = false;
            IsWin = false;
            GameStart = false;
            _isGameOver = false;
            _menuForLava = false;
            Hero.FirstDown = false;
            Hero.FirstUp = false;
        }
    }

    //private void CreateMenu()
    //{
    //    //var imageMenu = new Image();
    //    //imageMenu.Source = GameResources.MenuIm;
    //    //imageMenu.Width = 130;
    //    //imageMenu.Height = 110;
    //    //var buttonMenu = new Button();

    //    //buttonMenu.Width = 100;
    //    //buttonMenu.Height = 35;
    //    //buttonMenu.Opacity = 0;
    //    //RotateTransform button = new RotateTransform(12, buttonMenu.Width / 2, 0);
    //    //buttonMenu.RenderTransform = button;
    //    //buttonMenu.Click += ButtonMenu_Click;


    //    //var textMenu = new TextBlock();
    //    //textMenu.Width = 48;
    //    //textMenu.Height = 12;
    //    //textMenu.Text = "MENU";
    //    //RotateTransform menu = new RotateTransform(12, buttonMenu.Width / 2, 0);
    //    //textMenu.RenderTransform = menu;
    //    //textMenu.FontFamily = new FontFamily("Miama Nueva");

    //    //BCanvas.Children.Add(imageMenu);
    //    //Canvas.SetLeft(imageMenu, 0); //BCanvas.ActualWidth / 10);
    //    //Canvas.SetTop(imageMenu, 0); //BCanvas.ActualHeight / 10);

    //    //BCanvas.Children.Add(textMenu);
    //    //Canvas.SetLeft(textMenu, 55); //BCanvas.ActualWidth / 10);
    //    //Canvas.SetTop(textMenu, 60); //BCanvas.ActualHeight / 10);

    //    //BCanvas.Children.Add(buttonMenu);
    //    //Canvas.SetLeft(buttonMenu, 26); //BCanvas.ActualWidth / 10);
    //    //Canvas.SetTop(buttonMenu, 41); //BCanvas.ActualHeight / 10);
    //}



    //private void ButtonMenu_Click(object sender, RoutedEventArgs e)
    //{
    //    //MenuWindow.Background = Brushes.BlueViolet;
    //    //MenuWindow.Opacity = 0.5;
    //    //MenuWindow.Width = MenuWindow.Grid1.Width = 650;
    //    //MenuWindow.Height = MenuWindow.Grid1.Height = 350;
    //    //MenuWindow.AllowsTransparency = true;
    //    //MenuWindow.WindowStyle = WindowStyle.None;
    //    //MenuWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    //    //MenuWindow.Show();
    //    //ButtonsMenu();
    //}

    //public void ButtonsMenu()
    //{
    //    //var volume = new Button();
    //    //var language = new Button();
    //    //var stackPan = new StackPanel();

    //    //volume.Width = language.Width = 120;
    //    //volume.Height = language.Height = 90;
    //    //volume.Background = language.Background = Brushes.Aquamarine;
    //    //volume.Content = "Volume";
    //    //language.Content = "Language";
    //    //MenuWindow.Grid1.Children.Add(stackPan);
    //    //stackPan.Children.Add(volume);
    //    //stackPan.Children.Add(language);
    //    ////stackPan.VerticalAlignment = VerticalAlignment.Top;
    //    ////stackPan.HorizontalAlignment = HorizontalAlignment.Center;

    //}

    //private void UpdateEllipse()
    //{

    //    if (X_HeroEllipse < 0) X_HeroEllipse = 0;
    //    if (!IsLava && Y_HeroEllipse > BCanvas.ActualHeight - HeroEllipse.Height)
    //        Y_HeroEllipse = BCanvas.ActualHeight - HeroEllipse.Height;
    //    if (Y_HeroEllipse < 0) Y_HeroEllipse = 0;
    //    Speed = Canvas.GetTop(HeroEllipse) - Y_HeroEllipse;
    //    if (Math.Abs(Speed) < 0.001) IsJumped = false;

    //    Canvas.SetTop(HeroEllipse, Y_HeroEllipse);
    //    Canvas.SetLeft(HeroEllipse, X_HeroEllipse);
    //}

    //private void UpdateCoordinatesSnow()
    //{
    //    X_HeroEllipse = Canvas.GetLeft(HeroEllipse);
    //    Y_HeroEllipse = Canvas.GetTop(HeroEllipse);
    //    X_Canvas = Canvas.GetLeft(BCanvas);
    //}

    //private void Jump()
    //{
    //    if (IsJumped)
    //    {
    //        if (CounterStepJump < 25)
    //        {
    //            var shift = Shift * 4.5 - CounterStepJump;
    //            Y_HeroEllipse -= shift;
    //            CounterStepJump += 1;
    //        }
    //    }
    //    else
    //    {
    //        if (CounterStepJump > 0.1) CounterStepJump = 0;
    //    }

    //}

    //public void AddScore() //добавление очков для призовых блоков
    //{
    //    //if (IsWin)
    //    //{
    //    //    Score = 0;
    //    //    ScoreBox.Text = "    " + "Score:" + " " + Score;
    //    //}
    //    //else
    //    //{
    //    //    Score += 100;
    //    //    ScoreBox.Text = "    " + "Score:" + " " + Score;
    //    //}
    //}
    //public void CreateScore()
    //{
    //    //ScoreInGame.Create();

    //    //GridTextScore.Width = 145;
    //    //GridTextScore.Height = 70;
    //    //RotateTransform rtText = new RotateTransform(-6.5, GridTextScore.Width / 2, 0);
    //    //GridTextScore.RenderTransform = rtText;

    //    //ScoreBox.FontFamily = new FontFamily("Miama Nueva");
    //    //ScoreBox.FontSize = 14;
    //    //ScoreBox.Text = "    " + "Score:" + " " + Score;
    //    ////ScoreBox.TextAlignment = TextAlignment.Center;
    //    //ScoreBox.VerticalAlignment = VerticalAlignment.Bottom;

    //    //ScoreImage.Source = ScoreIm;
    //    //ScoreImage.Width = 145;
    //    //ScoreImage.Height = 115;

    //    //GridTextScore.Children.Add(ScoreBox);

    //    //BCanvas.Children.Add(ScoreImage);
    //    //Canvas.SetLeft(ScoreImage, 15);
    //    //Canvas.SetTop(ScoreImage, 95);

    //    //BCanvas.Children.Add(GridTextScore);
    //    //Canvas.SetLeft(GridTextScore, 15); //43//10);// BCanvas.ActualWidth / 10);
    //    //Canvas.SetTop(GridTextScore, 95); //60);// BCanvas.ActualHeight / 10 + 40);
    //}
    //public void SwingScoreImage()
    //{
    //    //if (StepSwingImage == 4) return;
    //    //switch (StepSwingImage)
    //    //{
    //    //    case 1:
    //    //        AngleScore -= SpeedScore;
    //    //        if (AngleScore < -9) StepSwingImage++;
    //    //        break;
    //    //    case 2:
    //    //        AngleScore += SpeedScore;
    //    //        if (AngleScore > 25) StepSwingImage++;
    //    //        break;
    //    //    case 3:
    //    //        AngleScore -= SpeedScore;
    //    //        if (AngleScore <= 0.5) StepSwingImage++;
    //    //        break;
    //    //}

    //    //RotateTransform rtText = new RotateTransform(AngleScore - 6.5, GridTextScore.Width / 2, 0);
    //    //GridTextScore.RenderTransform = rtText; //тут вращать лучше грид а не текст
    //    //RotateTransform rtImage = new RotateTransform(AngleScore, ScoreImage.Width / 2, 0);
    //    //ScoreImage.RenderTransform = rtImage;

    //}
    //public void RenovationScore()
    //{
    //    //BCanvas.Children.Remove(GridTextScore);
    //    //BCanvas.Children.Remove(ScoreImage);
    //    //BCanvas.Children.Add(ScoreImage);
    //    //BCanvas.Children.Add(GridTextScore);
    //}
    //public void OpenInvisibleBlock(ObjectAndParameters bonusListInvis)
    //{
    //    foreach (var shapeParamInvisibleBlock in bonusListInvis.ShapeParam.InvisibleBlocks)
    //    {
    //        var block = new Image();
    //        ObjectAndParameters frameBlock = new ObjectAndParameters(block, shapeParamInvisibleBlock);
    //        //AllBlocks.Add(frameBlock);
    //        switch (shapeParamInvisibleBlock.Name)
    //        {
    //            case NameShapeParameters.NewLevel:
    //                block.Source = NewLevel;
    //                block.Width = block.Height = 80;
    //                break;
    //            case NameShapeParameters.Brick:
    //                block.Source = SmallBlock1;
    //                block.Width = 70;
    //                block.Height = 25;
    //                break;
    //        }
    //        BCanvas.Children.Add(block);
    //        block.Stretch = Stretch.Fill;
    //        Canvas.SetLeft(block, shapeParamInvisibleBlock.X + TotalShiftBlocks);
    //        Canvas.SetTop(block, shapeParamInvisibleBlock.Y);
    //    }

    //    RenovationScore();
    //}

    //private void CollisionInvisibleBlocks(ObjectAndParameters invis, double yShape)
    //{
    //    if (Y_HeroEllipse > yShape - HeroEllipse.ActualHeight &&
    //        Y_HeroEllipse < yShape + invis.Shape.ActualHeight)// HeroEllipse.ActualHeight + invis.Shape.ActualHeight / 2)
    //    {
    //        if (invis.ShapeParam.Name == NameShapeParameters.NewLevel)
    //            NextGame();
    //        else
    //            Y_HeroEllipse = yShape - HeroEllipse.ActualHeight;// /2;
    //    }
    //}

    //private void CollisionLava(ObjectAndParameters lava)
    //{
    //    //if (IsLava) return;

    //    //double x = Canvas.GetLeft(lava.Shape);
    //    //double y = Canvas.GetTop(lava.Shape);
    //    //if (X_HeroEllipse >= x - HeroEllipse.ActualWidth / 2 &&
    //    //    X_HeroEllipse <= x + lava.Shape.Width - HeroEllipse.ActualWidth / 2 &&
    //    //    Y_HeroEllipse >= y - HeroEllipse.ActualHeight / 2)
    //    //{
    //    //    MediaPlayer.Open(new Uri("music/lava1.mp3", UriKind.RelativeOrAbsolute));
    //    //    MediaPlayer.Play();
    //    //    IsLava = true;
    //    //    GameOver();
    //    //}
    //}

    //private void CollisionShape(ObjectAndParameters shape)
    //{
    //    double xShape = Canvas.GetLeft(shape.Shape);
    //    double yShape = Canvas.GetTop(shape.Shape);
    //    if (Y_HeroEllipse > yShape - HeroEllipse.ActualHeight && Y_HeroEllipse < yShape + shape.Shape.ActualHeight + 8) //гравитация
    //    {
    //        if (X_HeroEllipse > xShape - HeroEllipse.ActualWidth &&
    //            X_HeroEllipse < xShape + CollisionValueX - HeroEllipse.ActualWidth)
    //            X_HeroEllipse = xShape - HeroEllipse.ActualWidth;

    //        if (X_HeroEllipse > xShape + shape.Shape.ActualWidth - CollisionValueX &&
    //            X_HeroEllipse < xShape + shape.Shape.ActualWidth)
    //            X_HeroEllipse = xShape + shape.Shape.ActualWidth;
    //    }
    //    if (X_HeroEllipse > xShape - HeroEllipse.ActualWidth && X_HeroEllipse < xShape + shape.Shape.ActualWidth)
    //    {
    //        if (Y_HeroEllipse > yShape + shape.Shape.ActualHeight / 3 && //сделано пересечение областей, снизу область проверки больше тк в момент прыжка скорость высокая и проскакивает тонкие блоки
    //            Y_HeroEllipse < yShape + shape.Shape.ActualHeight)
    //            Y_HeroEllipse = yShape + shape.Shape.ActualHeight;
    //        if (shape.ShapeParam.Type == TypeShapeParameters.Invisible)
    //        {
    //            CollisionInvisibleBlocks(shape, yShape);
    //        }
    //        if (Y_HeroEllipse > yShape - HeroEllipse.ActualHeight + 10 && //шарик внутри блока
    //            Y_HeroEllipse < yShape - HeroEllipse.ActualHeight + shape.Shape.ActualHeight / 2)
    //            Y_HeroEllipse = yShape - HeroEllipse.ActualHeight + 10;
    //    }
    //}


    //private bool CollisionPrizes(ObjectAndParameters prize)
    //{
    //    if (!CheckCollision(prize)) return false;
    //    MediaPlayer.Open(new Uri("music/bells.mp3", UriKind.RelativeOrAbsolute));
    //    MediaPlayer.Play();
    //    AllBlocks.Remove(prize);
    //    BCanvas.Children.Remove(prize.Shape);
    //    ShapeParameters parametersFrame = new ShapeParameters
    //    {
    //        X = prize.ShapeParam.X,
    //        Y = prize.ShapeParam.Y,
    //        Type = TypeShapeParameters.Frame,
    //    };
    //    CreateFrameBlocks(parametersFrame);
    //    AddScore();
    //    return true;
    //}

    //private void CollisionPortal(ObjectAndParameters portal)
    //{
    //    //if (!CheckCollision(portal)) return;
    //    double yShape = Canvas.GetTop(portal.Shape);
    //    double xShape = Canvas.GetLeft(portal.Shape);
    //    if (X_HeroEllipse > xShape - HeroEllipse.ActualWidth / 2 &&
    //        X_HeroEllipse < xShape + portal.Shape.ActualWidth - HeroEllipse.ActualWidth / 2 &&
    //        Y_HeroEllipse > yShape - HeroEllipse.ActualHeight + portal.Shape.ActualHeight - 20 &&
    //        Y_HeroEllipse < yShape + portal.Shape.ActualHeight)
    //    {
    //        Y_HeroEllipse = yShape + portal.Shape.ActualHeight - HeroEllipse.ActualHeight - 20;
    //        Win();
    //        OpenInvisibleBlock(portal);
    //    }
    //}


    //private void OpacityTextBlock(ObjectAndParameters block)
    //{
    //    block.Shape.Opacity -= 0.005;
    //}
    //private async void LifetimeMess(Image mess, TextBlock text, ObjectAndParameters frameBlock)
    //{
    //    await Task.Delay(7500);
    //    BCanvas.Children.Remove(mess);
    //    BCanvas.Children.Remove(text);
    //    AllBlocks.Remove(frameBlock);
    //}

    //private bool CollisionBonusBlock(ObjectAndParameters bonus)
    //{
    //    if (!CheckCollision(bonus)) return false;
    //    var rnd = Rnd.Next(1, 3);
    //    switch (rnd)
    //    {
    //        case 1:
    //            MediaPlayer.Open(new Uri("music/bonusHo.mp3", UriKind.RelativeOrAbsolute));
    //            MediaPlayer.Play();
    //            break;
    //        case 2:
    //            MediaPlayer.Open(new Uri("music/bonusWAW2.mp3", UriKind.RelativeOrAbsolute));
    //            MediaPlayer.Play();
    //            break;
    //    }
    //    BCanvas.Children.Remove(bonus.Shape);
    //    AllBlocks.Remove(bonus);
    //    OpenInvisibleBlock(bonus);
    //    ShapeParameters parametersBonus = new ShapeParameters
    //    {
    //        X = bonus.ShapeParam.X,
    //        Y = bonus.ShapeParam.Y,
    //        Type = TypeShapeParameters.BonusMessage,
    //    };
    //    CreateFrameBlocks(parametersBonus);
    //    return true;
    //}

    //private bool IsFirstPrize = true;
    //public void CreateFrameBlocks(ShapeParameters param)
    //{ 
    //    //var block = new Image();
    //    //var text = new TextBlock();
    //    //var frameBlock = new ObjectAndParameters(block, param);
    //    //var textInFrameBlock = new ObjectAndParameters(text, param);
    //    //AllBlocks.Add(textInFrameBlock);
    //    //AllBlocks.Add(frameBlock);

    //    //block.Stretch = Stretch.Fill;
    //    //block.Opacity = 0.8;
    //    //text.FontFamily = new FontFamily("Miama Nueva");
    //    //text.TextAlignment = TextAlignment.Center;
    //    //switch (param.Type)
    //    //{
    //    //    case TypeShapeParameters.Frame:
    //    //        if (IsFirstPrize)
    //    //        {
    //    //            block.Source = Mess1;
    //    //            block.Width = 150;
    //    //            block.Height = 160;
    //    //            text.Width = block.Width - 30;
    //    //            text.TextWrapping = TextWrapping.Wrap;
    //    //            text.Text = "Welcome! We are glad to see you in our New Years Game! " +
    //    //                        "This is the Prizes! Collect them to get balls in Score";
    //    //            Canvas.SetLeft(text, param.X + TotalShiftBlocks + 11);
    //    //            param.Y = CollisionFramesInCanvas(param, block);
    //    //            Canvas.SetTop(text, param.Y + 55);
    //    //            IsFirstPrize = false;
    //    //        }
    //    //        else
    //    //        {
    //    //            block.Source = Mess3;
    //    //            text.TextWrapping = TextWrapping.Wrap;
    //    //            block.Width = 90;
    //    //            block.Height = 80;
    //    //            text.Width = block.Width - 20;
    //    //            var rnd = Rnd.Next(1, 3);
    //    //            switch (rnd)
    //    //            {
    //    //                case 1:
    //    //                    text.Text = "Вravo!";
    //    //                    break;
    //    //                case 2:
    //    //                    text.Text = "Cool!";
    //    //                    break;
    //    //            }
    //    //            Canvas.SetLeft(text, param.X + TotalShiftBlocks + 9);
    //    //            param.Y = CollisionFramesInCanvas(param, block);
    //    //            Canvas.SetTop(text, param.Y + 40);
    //    //        }
    //    //        break;
    //    //    case TypeShapeParameters.BonusMessage:
    //    //        block.Source = Mess2;
    //    //        block.Width = 140;
    //    //        block.Height = 110;
    //    //        text.Width = block.Width - 10;
    //    //        text.TextWrapping = TextWrapping.Wrap;
    //    //        text.Text = "This present open invisible breaks!";
    //    //        Canvas.SetLeft(text, param.X + TotalShiftBlocks + 9);
    //    //        param.Y = CollisionFramesInCanvas(param, block);
    //    //        Canvas.SetTop(text, param.Y + 53);
    //    //        break;
    //    //}
    //    //LifetimeMess(block, text, frameBlock);
    //    //Canvas.SetLeft(block, param.X + TotalShiftBlocks);
    //    //param.Y = CollisionFramesInCanvas(param, block);
    //    //Canvas.SetTop(block, param.Y);
    //    //BCanvas.Children.Add(block);
    //    //BCanvas.Children.Add(text);
    //}

    //private double CollisionFramesInCanvas(ShapeParameters param, Image block)
    //{
    //    if (param.Y > BCanvas.ActualHeight - block.Height - 10)
    //        return BCanvas.ActualHeight - block.Height - 10;
    //    return param.Y;
    //}



}
