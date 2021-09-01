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
    class Portals: VisibleBlocks, IGameObject
    {
        //public FrameworkElement ShapeBlock { get; set; }
        //public ShapeParameters ShapeParam { get; set; }
        //public bool IsRemoved { get; set; }

        public Portals(FrameworkElement portal, ShapeParameters portalParameters) : base(portal, portalParameters)
        {
            ShapeBlock = portal;
            ShapeParam = portalParameters;
        }


        public new void Create()
        {
            if (!(ShapeBlock is Image localPor)) return;
            localPor.Source = GameResources.Portal;
            localPor.Height = 145;
            localPor.Width = 120;
        }
        public new void Collision()
        {
            if (!App.OpenedMainWindow.CheckCollision(ShapeBlock)) return;
            double yShape = Canvas.GetTop(ShapeBlock);
            double xShape = Canvas.GetLeft(ShapeBlock);
            if (Hero.X > xShape - Hero.Width / 2 &&
                Hero.X < xShape + ShapeBlock.ActualWidth - Hero.Width / 2 &&
                Hero.Y > yShape - Hero.Height + ShapeBlock.ActualHeight - 20 &&
                Hero.Y < yShape + ShapeBlock.ActualHeight)
            {
                Hero.Y = yShape + ShapeBlock.ActualHeight - Hero.Height - 20;
                App.OpenedMainWindow.Win();
                var portal = new VisibleBlocks(ShapeBlock, ShapeParam);
                portal.Create(ShapeParam.InvisibleBlocks);
                
            }
        }


    }
}
