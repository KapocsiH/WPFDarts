using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Xps;

namespace WPFDarts
{
    public partial class _501 : Window
    {
        private const double DartboardRadius = 346;
        private const double InnerBullseyeRadius = 14;
        private const double OuterBullseyeOuterRadius = 33;
        private const double TripleRingInnerRadius = 198;
        private const double TripleRingOuterRadius = 218;
        private const double DoubleRingInnerRadius = 328;
        private const double DoubleRingOuterRadius = 346;
        private const double DegreePerSection = 18;
        private const double RotationOffset = 9;
        private readonly int[] sectorOrder = { 11, 14, 9, 12, 5, 20, 1, 18, 4, 13, 6, 10, 15, 2, 17, 3, 19, 7, 16, 8 };
        private int player1Score = 501;
        private int player2Score = 501;
        private int currentPlayer = 1;
        private int throwCount = 0;
        private DispatcherTimer timer;
        private Random rand;
        private Random random = new Random();
        private DispatcherTimer _timer;
        private Random _random = new Random();
        private int _shakeRange = 10;
        public _501()
        {
            InitializeComponent();
            this.MouseMove += OnMouseMove;
            UpdateCurrentPlayerDisplay();
            this.Cursor = Cursors.None;
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(50) // Adjust shake speed
            };
            _timer.Tick += ShakeCursor;
            _timer.Start();
        }
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }
        private void ShakeCursor(object sender, EventArgs e)
        {
            if (IsMouseOverDartboard())
            {
                GetCursorPos(out POINT cursorPos);
                int newX = cursorPos.X + _random.Next(-_shakeRange, _shakeRange);
                int newY = cursorPos.Y + _random.Next(-_shakeRange, _shakeRange);
                SetCursorPos(newX, newY);
            }
        }
        private bool IsMouseOverDartboard()
        {
            Point mousePosition = Mouse.GetPosition(DartboardImage);
            return mousePosition.X >= 0 && mousePosition.X <= DartboardImage.ActualWidth &&
                   mousePosition.Y >= 0 && mousePosition.Y <= DartboardImage.ActualHeight;
        }
        private void DartboardImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPosition = e.GetPosition(DartboardImage);
            Random rand = new Random();
            double offsetX = rand.NextDouble() * 10 - 15;
            double offsetY = rand.NextDouble() * 10 - 15;
            clickPosition.Offset(offsetX, offsetY);
            Point bullseye = new Point(DartboardImage.ActualWidth / 2, DartboardImage.ActualHeight / 2);
            double distance = CalculateDistance(bullseye, clickPosition);
            if (distance > DartboardRadius)
            {
                MessageBox.Show("Outside Dartboard");
                return;
            }
            int sector = GetSector(bullseye, clickPosition);
            int score = CalculateScore(distance, sector);
            MessageBox.Show($"You clicked on sector: {sector}, Score: {score}");
            if (currentPlayer == 1)
            {
                if (player1Score - score == 0)
                {
                    player1Score -= score;
                    Player1ScoreLabel.Content = $"Player 1 Score: {player1Score}";
                    MessageBox.Show("Player 1 wins!");
                    ResetGame();
                    return;
                }
                else if (player1Score - score < 0)
                {
                    MessageBox.Show("Player 1 missed the exact score!");
                    currentPlayer = 2;
                    UpdateCurrentPlayerDisplay();
                    return;
                }
                else
                {
                    player1Score -= score;
                    Player1ScoreLabel.Content = $"Player 1 Score: {player1Score}";
                }
            }
            else
            {
                if (player2Score - score == 0)
                {
                    player2Score -= score;
                    Player2ScoreLabel.Content = $"Player 2 Score: {player2Score}";
                    MessageBox.Show("Player 2 wins!");
                    ResetGame();
                    return;
                }
                else if (player2Score - score < 0)
                {
                    MessageBox.Show("Player 2 missed the exact score!");
                    currentPlayer = 1;
                    UpdateCurrentPlayerDisplay();
                    return;
                }
                else
                {
                    player2Score -= score;
                    Player2ScoreLabel.Content = $"Player 2 Score: {player2Score}";
                }
            }
            throwCount++;
            if (throwCount >= 1)
            {
                throwCount = 0;
                currentPlayer = currentPlayer == 1 ? 2 : 1;
                UpdateCurrentPlayerDisplay();
            }
        }
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);
            cursorp.Content = $"Cursor Position: X = {position.X}\n Y = {position.Y}";
            Canvas.SetLeft(CursorRing, position.X - CursorRing.Width / 2);
            Canvas.SetTop(CursorRing, position.Y - CursorRing.Height / 2);
        }
        private double CalculateDistance(Point center, Point mousePosition)
        {
            double distanceX = mousePosition.X - center.X;
            double distanceY = mousePosition.Y - center.Y;
            double distanceFromCenter = Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2));
            return distanceFromCenter;
        }
        private int GetSector(Point center, Point mousePosition)
        {
            double angle = Math.Atan2(mousePosition.Y - center.Y, mousePosition.X - center.X) * (180 / Math.PI) + 180;
            angle = (angle + RotationOffset) % 360;
            int sectorIndex = (int)(angle / DegreePerSection);
            return sectorOrder[sectorIndex];
        }
        private int CalculateScore(double distance, int sector)
        {
            int score = 0;
            if (distance < InnerBullseyeRadius) score = 50;
            else if (distance < OuterBullseyeOuterRadius) score = 25;
            else if (distance < TripleRingOuterRadius && distance > TripleRingInnerRadius) score = sector * 3;
            else if (distance < DoubleRingOuterRadius && distance > DoubleRingInnerRadius) score = sector * 2;
            else score = sector;
            return score;
        }
        private void ResetGame()
        {
            player1Score = 501;
            player2Score = 501;
            currentPlayer = 1;
            throwCount = 0;
            Player1ScoreLabel.Content = $"Player 1 Score: {player1Score}";
            Player2ScoreLabel.Content = $"Player 2 Score: {player2Score}";
            UpdateCurrentPlayerDisplay();
        }
        private void UpdateCurrentPlayerDisplay()
        {
            CurrentPlayerLabel.Content = $"Current Player: {currentPlayer}";
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
