using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Xps;

namespace WPFDarts
{
    /// <summary>
    /// Interaction logic for _501.xaml
    /// </summary>
    public partial class _501 : Window
    {
        private const double DartboardRadius = 200;
        private const double InnerBullseyeRadius = 20;
        private const double OuterBullseyeInnerRadius = 50;
        private const double OuterBullseyeOuterRadius = 55;
        private const double TripleRingInnerRadius = 150;
        private const double TripleRingOuterRadius = 155;
        private const double DoubleRingInnerRadius = 170;
        private const double DoubleRingOuterRadius = 175;

        private const double DegreePerSection = 18;

        private void DartBoard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition(DartBoard);

            Point bullseye = new Point(DartBoard.Width / 2, DartBoard.Height / 2);

            double distance = CalculateDistance(bullseye, mousePosition);

            if (distance > DartboardRadius)
            {
                ScoreText.Text = "Outside Dartboard";
                return;
            }
        }

        private double CalculateDistance(Point center, Point mousePosition)
        {
            double distanceX = mousePosition.X - center.X;
            double distanceY = mousePosition.Y - center.Y;

            double distanceFromCenter = Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceX, 2));
            return distanceFromCenter;
        }

        private double PointSector(double distance)
        {
            int score = 0;

            if (distance < InnerBullseyeRadius) score += 50;
            else if (distance < OuterBullseyeOuterRadius) score += 25;
            else if (distance < TripleRingOuterRadius) ; //score += Zone * 3
            else if (distance < DoubleRingOuterRadius) ; //score += Zone * 2
            //else score += Zone

            return score;
        }
    }
}
