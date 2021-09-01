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
    public class Score
    {
        public int ScoreGame=0;
        //public FrameworkElement ScoreBlock;
        public ShapeParameters ScoreParameters;

        private Grid _gridTextScore = new Grid();

        public int StepSwingImage = 4;

        private double _speedScore = 0.8;
        private TextBlock _scoreBox = new TextBlock();
        private Image _scoreImage = new Image();
        private double _angleScore = 0;
        

        public Score(int score, ShapeParameters scoreParameters)
        {
            ScoreGame = score;
            //ScoreBlock = scoreBlock;
            ScoreParameters = scoreParameters;
        }
        
        public void Create()
        {

            _gridTextScore.Width = 145;
            _gridTextScore.Height = 70;
            RotateTransform rtText = new RotateTransform(-6.5, _gridTextScore.Width / 2, 0);
            _gridTextScore.RenderTransform = rtText;

            _scoreBox.FontFamily = new FontFamily(new Uri("pack://application:,,,/Fonts/"), "./#Miama Nueva");
            _scoreBox.FontSize = 14;
            _scoreBox.Text = "    " + "Score:" + " " + ScoreGame;
            //ScoreBox.TextAlignment = TextAlignment.Center;
            _scoreBox.VerticalAlignment = VerticalAlignment.Bottom;

            _scoreImage.Source = GameResources.ScoreIm;
            _scoreImage.Width = 145;
            _scoreImage.Height = 115;

            _gridTextScore.Children.Add(_scoreBox);

            App.OpenedMainWindow.BCanvas.Children.Add(_scoreImage);
            Canvas.SetLeft(_scoreImage, 15);
            Canvas.SetTop(_scoreImage, 95);

            App.OpenedMainWindow.BCanvas.Children.Add(_gridTextScore);
            Canvas.SetLeft(_gridTextScore, 15);//43//10);// BCanvas.ActualWidth / 10);
            Canvas.SetTop(_gridTextScore, 95); //60);// BCanvas.ActualHeight / 10 + 40);

        }
        public void AddScore() //добавление очков для призовых блоков
        {
            if (App.OpenedMainWindow.IsWin)
            {
                ScoreGame = 0;
                _scoreBox.Text = "    " + "Score:" + " " + ScoreGame;
            }
            else
            {
                ScoreGame += 100;
                _scoreBox.Text = "    " + "Score:" + " " + ScoreGame;
            }
        }

        public void SwingScoreImage()
        {
            if (StepSwingImage == 4) return;
            switch (StepSwingImage)
            {
                case 1:
                    _angleScore -= _speedScore;
                    if (_angleScore < -9) StepSwingImage++;
                    break;
                case 2:
                    _angleScore += _speedScore;
                    if (_angleScore > 25) StepSwingImage++;
                    break;
                case 3:
                    _angleScore -= _speedScore;
                    if (_angleScore <= 0.5) StepSwingImage++;
                    break;
            }
            RotateTransform rtText = new RotateTransform(_angleScore - 6.5, _gridTextScore.Width / 2, 0);
            _gridTextScore.RenderTransform = rtText; //тут вращать лучше грид а не текст
            RotateTransform rtImage = new RotateTransform(_angleScore, _scoreImage.Width / 2, 0);
            _scoreImage.RenderTransform = rtImage;

        }
        public void RenovationScore()
        {
            App.OpenedMainWindow.BCanvas.Children.Remove(_gridTextScore);
            App.OpenedMainWindow.BCanvas.Children.Remove(_scoreImage);
            App.OpenedMainWindow.BCanvas.Children.Add(_scoreImage);
            App.OpenedMainWindow.BCanvas.Children.Add(_gridTextScore);
        }

    }
}
