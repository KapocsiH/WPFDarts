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
    public partial class AroundTheClock : Window
    {
        private List<Player> players;
        private int currentPlayerIndex = 0;

        public AroundTheClock()
        {
            InitializeComponent();
            SetupGame();
        }

        private void SetupGame()
        {
            players = new List<Player>
            {
                new Player("Player 1"),
                new Player("Player 2")
            };
            UpdateUI();
        }

        private void Dartboard_Click(object sender, MouseButtonEventArgs e)
        {
            Player currentPlayer = players[currentPlayerIndex];
            GameLog.Text += $"{currentPlayer.Name} clicked on the dartboard (Target: {currentPlayer.TargetNumber})\n";

            // Simulating dartboard sections by converting click position
            int hitNumber = GetHitNumber(e.GetPosition(DartboardCanvas));
            GameLog.Text += $"Hit number: {hitNumber}\n";

            if (hitNumber == currentPlayer.TargetNumber)
            {
                GameLog.Text += $"{currentPlayer.Name} hit the target!\n";
                currentPlayer.TargetNumber++;
                if (currentPlayer.HasWon())
                {
                    GameLog.Text += $"{currentPlayer.Name} wins the game!\n";
                    return;
                }
            }
            else
            {
                GameLog.Text += $"{currentPlayer.Name} missed.\n";
            }

            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            UpdateUI();
        }

        private int GetHitNumber(Point clickPosition)
        {
            // Simplified example: Determining dartboard section based on click position
            if (clickPosition.Y < 50) return 1;
            if (clickPosition.X > 300) return 20;
            if (clickPosition.X < 100) return 5;
            return new Random().Next(1, 21); // Fallback random dartboard number
        }

        private void UpdateUI()
        {
            CurrentPlayerText.Text = $"{players[currentPlayerIndex].Name}'s Turn (Target: {players[currentPlayerIndex].TargetNumber})";
        }
    }

    public class Player
    {
        public string Name { get; private set; }
        public int TargetNumber { get; set; } = 1;

        public Player(string name)
        {
            Name = name;
        }

        public bool HasWon()
        {
            return TargetNumber > 21;
        }
    }
}
