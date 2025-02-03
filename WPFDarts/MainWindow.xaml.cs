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
        private int player1Target = 1;
        private int player2Target = 1;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void PlayerHitsTarget(int playerNumber)
        {
            if (playerNumber == 1)
            {
                if (player1Target < 20)
                    player1Target++;
                else
                    MessageBox.Show("Player 1 Wins!");

                Player1Target.Text = player1Target.ToString();
            }
            else if (playerNumber == 2)
            {
                if (player2Target < 20)
                    player2Target++;
                else
                    MessageBox.Show("Player 2 Wins!");

                Player2Target.Text = player2Target.ToString();
            }
        }

        private void Player1Hit_Click(object sender, RoutedEventArgs e)
        {
            PlayerHitsTarget(1);
        }

        private void Player2Hit_Click(object sender, RoutedEventArgs e)
        {
            PlayerHitsTarget(2);
        }
    }
}