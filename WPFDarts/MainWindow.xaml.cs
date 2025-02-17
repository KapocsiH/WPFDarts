using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFDarts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Shanghai_Click(object sender, RoutedEventArgs e)
        {
            Shanghai shanghai = new Shanghai();
            shanghai.Show();
        }

        private void _501_Click(object sender, RoutedEventArgs e)
        {
            _501 _501 = new _501();
            _501.Show();
        }

        private void AroundTheClock_Click(object sender, RoutedEventArgs e)
        {
            AroundTheClock AroundTheClock = new AroundTheClock();
            AroundTheClock.Show();
        }
    }
}