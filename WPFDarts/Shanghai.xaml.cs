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
    /// Interaction logic for Shanghai.xaml
    /// </summary>
    public partial class Shanghai : Window
    {
        public Shanghai()
        {
            InitializeComponent();
            this.MouseMove += OnMouseMove;
        }
        private void DartboardImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPosition = e.GetPosition(DartboardImage);
            int sector = GetSector(clickPosition.X, clickPosition.Y);
            MessageBox.Show($"You clicked on sector: {sector}");
        }
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this); // Get cursor position relative to window
            Title = $"Cursor Position: X = {position.X}, Y = {position.Y}"; // Display in window title
        }
        private int GetSector(double x, double y)
        {
            double centerX = DartboardImage.ActualWidth / 2;
            double centerY = DartboardImage.ActualHeight / 2;
            double angle = Math.Atan2(y - centerY, x - centerX) * (180 / Math.PI) + 180;

            // Assuming 20 sectors, each 18 degrees
            int sector = (int)(angle / 18) + 10;
            return sector;
        }
    }
}
