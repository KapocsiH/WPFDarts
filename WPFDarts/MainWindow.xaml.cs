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
        private string playername1;
        private string playername2;
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(string playername1, string playername2)
        {
            InitializeComponent();
            this.playername1 = playername1;
            this.playername2 = playername2;
        }
        private void _501_Click(object sender, RoutedEventArgs e)
        {
            _501 _501 = new _501();
            _501.Show();
            this.Close();
        }
        private void AroundTheClock_Click(object sender, RoutedEventArgs e)
        {
            AroundTheClock aroundTheClock = new AroundTheClock(playername1, playername2);
            aroundTheClock.Show();
            this.Close();
        }
        private void Shanghai_Click(object sender, RoutedEventArgs e)
        {
            Shanghai shanghai = new Shanghai();
            shanghai.Show();
            this.Close();
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }
    }
}