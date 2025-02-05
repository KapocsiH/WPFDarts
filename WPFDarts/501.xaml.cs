using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WPFDarts
{
    /// <summary>
    /// Interaction logic for _501.xaml
    /// </summary>
    public partial class _501 : Window
    {
        // Define the radii for the dartboard (in pixels)
        private const double DartboardRadius = 200; // Radius of the entire dartboard (adjust this as needed)
        private const double InnerBullseyeRadius = 20; // Inner bullseye radius (50 points)
        private const double OuterBullseyeInnerRadius = 50; // Inner radius of the outer bullseye (25 points)
        private const double OuterBullseyeOuterRadius = 55; // Outer radius of the outer bullseye
        private const double TripleRingInnerRadius = 150; // Inner radius of the triple ring
        private const double TripleRingOuterRadius = 155; // Outer radius of the triple ring
        private const double DoubleRingInnerRadius = 170; // Inner radius of the double ring
        private const double DoubleRingOuterRadius = 175; // Outer radius of the double ring

        // Define the angular slices for each point zone (simplified, normally these would be divided evenly)
        private const double DegreePerSection = 18; // Each section is 18 degrees for a 360-degree dartboard (360/20)

        private void DartBoard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Get the position of the mouse click relative to the dartboard image
            Point mousePosition = e.GetPosition(DartBoard);

            // The center of the dartboard (bullseye)
            Point bullseye = new Point(DartBoard.Width / 2, DartBoard.Height / 2);

            // Calculate the distance from the bullseye
            double distance = CalculateDistance(bullseye, mousePosition);

            // If the distance is greater than the dartboard's radius, the click is outside the dartboard
            if (distance > DartboardRadius)
            {
                ScoreText.Text = "Outside Dartboard";
                return;
            }

            // Calculate the angle of the click to determine the point zone (0 to 360 degrees)
            double angle = CalculateAngle(bullseye, mousePosition);

            // Determine the score zone based on the angle
            string scoreZone = GetScoreZone(angle);

            // Determine if the click is in a special area like the bullseye or a ring (triple, double)
            string specialArea = GetSpecialArea(distance);

            // Display the result
            ScoreText.Text = $"Distance: {distance:F2} | Zone: {scoreZone} | {specialArea}";
        }

        // Method to calculate distance between two points
        private double CalculateDistance(Point point1, Point point2)
        {
            double deltaX = point2.X - point1.X;
            double deltaY = point2.Y - point1.Y;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        // Method to calculate angle between the center and the clicked point
        private double CalculateAngle(Point bullseye, Point clickPosition)
        {
            double deltaX = clickPosition.X - bullseye.X;
            double deltaY = clickPosition.Y - bullseye.Y;
            double angle = Math.Atan2(deltaY, deltaX) * (180 / Math.PI); // Convert radians to degrees
            if (angle < 0) angle += 360; // Make sure the angle is always positive
            return angle;
        }

        // Method to get the point zone based on the angle (0 to 360 degrees)
        private string GetScoreZone(double angle)
        {
            int section = (int)(angle / DegreePerSection) + 1; // Determine which section (1-20)
            if (section > 20) section = 1; // Ensure we wrap around after 20

            // Return the corresponding point zone (assuming a simple 1-20 dartboard layout)
            return $"{section} points";
        }

        // Method to determine if the click is in a special area (bullseye, triple ring, double ring)
        private string GetSpecialArea(double distance)
        {
            // Check for bullseye
            if (distance <= InnerBullseyeRadius)
            {
                return "Bullseye (50 points)";
            }

            // Check for outer bullseye as a band (between 50 and 55 pixels)
            if (distance >= OuterBullseyeInnerRadius && distance <= OuterBullseyeOuterRadius)
            {
                return "Outer Bullseye (25 points)";
            }

            // Check for triple ring (narrow band between 150 and 155)
            if (distance >= TripleRingInnerRadius && distance <= TripleRingOuterRadius)
            {
                return "Triple Ring Area";
            }

            // Check for double ring (narrow band between 170 and 175)
            if (distance >= DoubleRingInnerRadius && distance <= DoubleRingOuterRadius)
            {
                return "Double Ring Area";
            }

            // If it's within the normal scoring area but not in any special ring
            return "Normal Zone";
        }
    }
}
