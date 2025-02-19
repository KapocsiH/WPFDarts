using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WPFDarts
{
    public partial class Shanghai : Window
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
        private int player1Score = 0;
        private int player2Score = 0;
        private string player1Name;
        private string player2Name;
        private int currentPlayer = 1;
        private int throwCount = 0;
        private int currentRound = 1;
        private bool player1HitSingle = false;
        private bool player1HitDouble = false;
        private bool player1HitTriple = false;
        private bool player2HitSingle = false;
        private bool player2HitDouble = false;
        private bool player2HitTriple = false;
        private Random random = new Random();
        private DispatcherTimer _timer;
        private Random _random = new Random();
        private int _shakeRange = 10;
        public Shanghai(string player1Name, string player2Name)
        {
            InitializeComponent();
            this.player1Name = player1Name;
            this.player2Name = player2Name;
            this.MouseMove += OnMouseMove;
            this.Cursor = Cursors.None;
            roundPlayer1.Content = $"(Round {currentRound}) {player1Name}";
            roundPlayer2.Content = $"(Round {currentRound}) {player2Name}";
            _timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(50)};
            _timer.Tick += ShakeCursor;
            _timer.Start();
            this.Deactivated += Shanghai_Deactivated;
            this.Closed += Shanghai_Closed;
        }
        private void Shanghai_Deactivated(object sender, EventArgs e)
        {
            if (_timer.IsEnabled) _timer.Stop();
        }
        private void Shanghai_Closed(object sender, EventArgs e)
        {
            if (_timer.IsEnabled) _timer.Stop();
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
            clickPosition = ApplyRandomOffset(clickPosition);
            Point bullseye = new Point(DartboardImage.ActualWidth / 2, DartboardImage.ActualHeight / 2);
            double distance = CalculateDistance(bullseye, clickPosition);
            if (distance > DartboardRadius)
            {
                RegisterThrow();
                return;
            }
            int sector = GetSector(bullseye, clickPosition);
            int score = CalculateScore(distance, sector);
            if (sector == currentRound)
            {
                if (currentPlayer == 1)
                {
                    player1Score += score;
                    player1score.Content = $"{player1Score}";
                    if (score == currentRound)
                    {
                        player1HitSingle = true;
                        player1Single.Content = "";
                    }
                    if (score == currentRound * 2)
                    {
                        player1HitDouble = true;
                        player1Double.Content = "";
                    }
                    if (score == currentRound * 3)
                    {
                        player1HitTriple = true;
                        player1Triple.Content = "";
                    }
                }
                else
                {
                    player2Score += score;
                    player2score.Content = $"{player2Score}";
                    if (score == currentRound)
                    {
                        player2HitSingle = true;
                        player2Single.Content = "";
                    }
                    if (score == currentRound * 2)
                    {
                        player2HitDouble = true;
                        player2Double.Content = "";
                    }
                    if (score == currentRound * 3)
                    {
                        player2HitTriple = true;
                        player2Triple.Content = "";
                    }
                }
            }
            RegisterThrow();
        }
        private void RegisterThrow()
        {
            throwCount++;
            if (throwCount >= 3)
            {
                if (player1HitSingle && player1HitDouble && player1HitTriple)
                {
                    MessageBox.Show($"{player1Name} nyert!");
                    return;
                }
                if (player2HitSingle && player2HitDouble && player2HitTriple)
                {
                    MessageBox.Show($"{player2Name} nyert!");
                    return;
                }
                throwCount = 0;
                currentPlayer = currentPlayer == 1 ? 2 : 1;
                if (currentPlayer == 1)
                {
                    currentRound++;
                    player1Single.Content = "Szimpla";
                    player2Single.Content = "Szimpla";
                    player1Double.Content = "Dupla";
                    player2Double.Content = "Dupla";
                    player1Triple.Content = "Tripla";
                    player2Triple.Content = "Tripla";
                    if (currentRound > 7)
                    {
                        string winner = player1Score > player2Score ? $"{player1Name}" : $"{player2Name}";
                        MessageBox.Show($"A játéknak vége! {winner} nyert!");
                        return;
                    }
                    player1HitSingle = player1HitDouble = player1HitTriple = false;
                    player2HitSingle = player2HitDouble = player2HitTriple = false;
                }
                roundPlayer1.Content = $"(Round {currentRound}) {player1Name}";
                roundPlayer2.Content = $"(Round {currentRound}) {player2Name}";
            }
        }
        private Point ApplyRandomOffset(Point originalPoint)
        {
            double offsetX = random.NextDouble() * 20 - 15;
            double offsetY = random.NextDouble() * 20 - 15;
            return new Point(originalPoint.X + offsetX, originalPoint.Y + offsetY);
        }
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this); // ez csak debughoz
            // cursorp.Content = $"Cursor Position: X = {position.X}\n Y = {position.Y}";
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
        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            player1Score = 0;
            player2Score = 0;
            currentPlayer = 1;
            throwCount = 0;
            currentRound = 1;
            player1HitSingle = player1HitDouble = player1HitTriple = false;
            player2HitSingle = player2HitDouble = player2HitTriple = false;
            player1score.Content = "0";
            player2score.Content = "0";
            roundPlayer1.Content = $"(Round 1) {player1Name}";
            roundPlayer2.Content = $"(Round 1) {player2Name}";
            _timer.Start();
        }
        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}