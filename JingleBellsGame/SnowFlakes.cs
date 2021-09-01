using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JingleBellsGame
{
    class SnowFlakes
    {
        public FrameworkElement SnowFlake;
        public List<FrameworkElement> ListSnowFlakes;
        public SnowFlakes(FrameworkElement snowFlake, List<FrameworkElement> listSnowFlakes)
        {
            SnowFlake = snowFlake;
            ListSnowFlakes = listSnowFlakes;
        }
        //public void InitSnowFlakes()
        //{
        //    while (ListSnowFlakes.Count < 150)
        //    {
        //        Image snowFlake = new Image();
        //        var rnd = Rnd.Next(1, 4);

        //        switch (rnd)
        //        {
        //            case 1:
        //                snowFlake.Source = SnowFlake1Picture;
        //                break;
        //            case 2:
        //                snowFlake.Source = SnowFlake2Picture;
        //                break;
        //            case 3:
        //                snowFlake.Source = SnowFlake3Picture;
        //                break;
        //        }
        //        snowFlake.Height = snowFlake.Width = Rnd.Next(15, 60);
        //        SnowFlakes.Add(snowFlake);
        //        if (snowFlake.Height < 30) BCanvas.Children.Insert(0, snowFlake);
        //        else BCanvas.Children.Add(snowFlake);
        //        Canvas.SetLeft(snowFlake, Rnd.Next((int)-(snowFlake.Height + BCanvas.ActualWidth), (int)BCanvas.ActualWidth * 2));
        //        if (IsFirstInitSnowFlakes)
        //            Canvas.SetTop(snowFlake, Rnd.Next((int)-snowFlake.Width, (int)BCanvas.ActualHeight));
        //        else
        //            Canvas.SetTop(snowFlake, -snowFlake.Width);
        //    }
        //    IsFirstInitSnowFlakes = false;
        //}
        ////private void InitSnowFlakes()
        ////{
        ////    while (SnowFlakes.Count < 150)
        ////    {
        ////        Image snowFlake = new Image();
        ////        var rnd = Rnd.Next(1, 4);

        ////        switch (rnd)
        ////        {
        ////            case 1:
        ////                snowFlake.Source = SnowFlake1Picture;
        ////                break;
        ////            case 2:
        ////                snowFlake.Source = SnowFlake2Picture;
        ////                break;
        ////            case 3:
        ////                snowFlake.Source = SnowFlake3Picture;
        ////                break;
        ////        }
        ////        snowFlake.Height = snowFlake.Width = Rnd.Next(15, 60);
        ////        SnowFlakes.Add(snowFlake);
        ////        if (snowFlake.Height < 30) BCanvas.Children.Insert(0, snowFlake);
        ////        else BCanvas.Children.Add(snowFlake);
        ////        Canvas.SetLeft(snowFlake, Rnd.Next((int)-(snowFlake.Height + BCanvas.ActualWidth), (int)BCanvas.ActualWidth * 2));
        ////        if (IsFirstInitSnowFlakes)
        ////            Canvas.SetTop(snowFlake, Rnd.Next((int)-snowFlake.Width, (int)BCanvas.ActualHeight));
        ////        else
        ////            Canvas.SetTop(snowFlake, -snowFlake.Width);
        ////    }
        ////    IsFirstInitSnowFlakes = false;
        ////}
        //private void MoveSnowFlakes()
        //{
        //    for (var i = 0; i < SnowFlakes.Count; i++)
        //    {
        //        var snowFlake = SnowFlakes[i];
        //        double xSnowFlake = Canvas.GetLeft(snowFlake);
        //        double ySnowFlake = Canvas.GetTop(snowFlake);
        //        if (ySnowFlake > BCanvas.ActualHeight ||
        //            xSnowFlake < -snowFlake.ActualWidth - BCanvas.ActualWidth ||
        //            xSnowFlake > BCanvas.ActualWidth * 2)
        //        {
        //            BCanvas.Children.Remove(snowFlake);
        //            SnowFlakes.Remove(snowFlake);
        //        }

        //        ySnowFlake += snowFlake.Width / 20;
        //        Canvas.SetTop(snowFlake, ySnowFlake);
        //    }
        //    //if (Y_SnowFlakes1 > BCanvas.ActualHeight || 
        //    //    X_SnowFlakes1 < -SnowFlake1.Width || 
        //    //    X_SnowFlakes1 > BCanvas.ActualWidth)
        //    //{

        //    //    SnowFlake1.Width = Rnd.Next(30,120);
        //    //    SnowFlake1.Height = SnowFlake1.Width;
        //    //    Y_SnowFlakes1 = -SnowFlake1.ActualHeight;
        //    //    X_SnowFlakes1 = Rnd.Next((int)-SnowFlake1.Width, (int)BCanvas.ActualWidth);
        //    //}
        //    //Y_SnowFlakes1 += Shift - 3;
        //}
        //private void MoveShapes() //у всех
        //{
        //    foreach (var snowFlake in SnowFlakes)
        //    {
        //        double xShape = Canvas.GetLeft(snowFlake);
        //        if (MoveRight)
        //        {
        //            xShape -= Shift;
        //        }
        //        if (MoveLeft)
        //        {
        //            xShape += Shift;
        //        }
        //        UpdateShapes(snowFlake, xShape);
        //    }

        //    if (MoveRight) TotalShiftBlocks -= Shift;
        //    if (MoveLeft) TotalShiftBlocks += Shift;
        //    foreach (var objAndParam in AllBlocks)
        //    {
        //        double xShape = Canvas.GetLeft(objAndParam.Shape);
        //        if (MoveRight)
        //        {
        //            xShape -= Shift;
        //        }
        //        if (MoveLeft)
        //        {
        //            xShape += Shift;
        //        }
        //        UpdateShapes(objAndParam.Shape, xShape);
        //    }

        //    MoveRight = false;
        //    MoveLeft = false;
        //}
    }
}
