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
    public interface IGameObject
    {
        FrameworkElement ShapeBlock { get; set; }
        ShapeParameters ShapeParam { get; set; }
        bool IsRemoved { get; set; }
        void Collision();

        void Create();
    }
}