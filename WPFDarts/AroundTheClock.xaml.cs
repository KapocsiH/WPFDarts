﻿using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Media;

namespace WPFDarts
{
    public partial class AroundTheClock : Window
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
        private int currentPlayer = 1;
        private int player1Target = 1;
        private int player2Target = 1;
        private int throwCount = 0;
        private string player1Name;
        private string player2Name;
        private Random random = new Random();
        private DispatcherTimer _timer;
        private Random _random = new Random();
        private int _shakeRange = 10;
        public AroundTheClock(string player1Name, string player2Name)
        {
            InitializeComponent();
            this.player1Name = player1Name;
            this.player2Name = player2Name;
            this.MouseMove += OnMouseMove;
            this.Cursor = Cursors.None;
            InitializeTargetListBoxes();
            UpdateCurrentPlayerDisplay();
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(50)
            };
            _timer.Tick += ShakeCursor;
            _timer.Start();
            this.Deactivated += AroundTheClock_Deactivated;
            this.Closed += AroundTheClock_Closed;
        }
        private void AroundTheClock_Deactivated(object sender, EventArgs e)
        {
            if (_timer.IsEnabled) _timer.Stop();
        }
        private void AroundTheClock_Closed(object sender, EventArgs e)
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
        private void InitializeTargetListBoxes()
        {
            for (int i = 1; i <= 20; i++)
            {
                Player1TargetsList.Items.Add(i);
                Player2TargetsList.Items.Add(i);
            }
            Player1TargetsList.Items.Add(25);
            Player2TargetsList.Items.Add(25);
            Player1TargetsList.Items.Add(50);
            Player2TargetsList.Items.Add(50);
        }
        private void DartboardImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPosition = e.GetPosition(DartboardImage);
            clickPosition = ApplyRandomOffset(clickPosition);
            Point bullseye = new Point(DartboardImage.ActualWidth / 2, DartboardImage.ActualHeight / 2);
            double distance = CalculateDistance(bullseye, clickPosition);
            if (distance > DartboardRadius)
            {
                //MessageBox.Show("Hát ez fakapu ;)");
                return;
            }
            int sector = GetSector(bullseye, clickPosition);
            int score = CalculateScore(distance, sector);
            if (currentPlayer == 1)
            {
                HandlePlayerTurn(ref player1Target, Player1TargetsList, sector, distance);
            }
            else
            {
                HandlePlayerTurn(ref player2Target, Player2TargetsList, sector, distance);
            }
            throwCount++;
            if (throwCount >= 3)
            {
                throwCount = 0;
                currentPlayer = currentPlayer == 1 ? 2 : 1;
                UpdateCurrentPlayerDisplay();
            }
        }

        private Point ApplyRandomOffset(Point originalPoint)
        {
            double offsetX = random.NextDouble() * 20 - 15;
            double offsetY = random.NextDouble() * 20 - 15;
            return new Point(originalPoint.X + offsetX, originalPoint.Y + offsetY);
        }

        private void HandlePlayerTurn(ref int playerTarget, ListBox playerTargetsList, int sector, double distance)
        {
            if (sector == playerTarget)
            {
                playerTargetsList.Items.Remove(playerTarget);
                int skipCount = 0;

                if (distance < DoubleRingOuterRadius && distance > DoubleRingInnerRadius)
                {
                    skipCount = 1;
                }
                else if (distance < TripleRingOuterRadius && distance > TripleRingInnerRadius)
                {
                    skipCount = 2;
                }
                for (int i = 0; i < skipCount; i++)
                {
                    playerTarget++;
                    playerTargetsList.Items.Remove(playerTarget);
                }
                playerTarget++;
                if (playerTarget > 20)
                {
                    if (distance < InnerBullseyeRadius)
                    {
                        MessageBox.Show($"{currentPlayer}. Játékoseltalálta a bullt és nyert!");
                        ResetGame();
                    }
                    else
                    {
                        //MessageBox.Show($"{currentPlayer}. Játékos eltalálta a {sector}. szektort. Most bullt kell dobnia!");
                    }
                }
                else
                {
                    //MessageBox.Show($"{currentPlayer}. Játékos eltalálta a {sector}. szektort. Következő szektor: {playerTarget}");
                }
            }
            else
            {
                //MessageBox.Show($"{currentPlayer}. Játékos eltalálta a {sector}. szektort. Jelenlegi szektor: {playerTarget}");
            }
        }
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);
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
        private void ResetGame()
        {
            player1Target = 1;
            player2Target = 1;
            currentPlayer = 1;
            throwCount = 0;
            Player1TargetsList.Items.Clear();
            Player2TargetsList.Items.Clear();
            InitializeTargetListBoxes();
            UpdateCurrentPlayerDisplay();
            _timer.Start();
        }
        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void UpdateCurrentPlayerDisplay()
        {
            if (player1Name != null && player2Name != null)
            {
                if (currentPlayer == 1)
                {
                    CurrentPlayerLabel.Content = $"Jelenlegi játékos: {player1Name}";
                }
                else
                {
                    CurrentPlayerLabel.Content = $"Jelenlegi játékos: {player2Name}";
                }
            }
            else
            {
                CurrentPlayerLabel.Content = $"Current Player: {currentPlayer}";
            }

        }
    }
}
