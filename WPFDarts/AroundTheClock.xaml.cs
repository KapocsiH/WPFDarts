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
    public partial class AroundTheClock : Window
    {
        private const double DartboardRadius = 276;
        private const double InnerBullseyeRadius = 12;
        private const double OuterBullseyeInnerRadius = 12;
        private const double OuterBullseyeOuterRadius = 25;
        private const double TripleRingInnerRadius = 160;
        private const double TripleRingOuterRadius = 173;
        private const double DoubleRingInnerRadius = 263;
        private const double DoubleRingOuterRadius = 276;
        private const double DegreePerSection = 18;
        private const double RotationOffset = 9;
        private readonly int[] sectorOrder = { 11, 14, 9, 12, 5, 20, 1, 18, 4, 13, 6, 10, 15, 2, 17, 3, 19, 7, 16, 8 };
        private int currentPlayer = 1;
        private int player1Target = 1;
        private int player2Target = 1;

        public AroundTheClock()
        {
            InitializeComponent();
            this.MouseMove += OnMouseMove;
            InitializeTargetListBoxes();
        }

        private void InitializeTargetListBoxes()
        {
            for (int i = 1; i <= 20; i++)
            {
                Player1TargetsList.Items.Add(i);
                Player2TargetsList.Items.Add(i);
            }
            Player1TargetsList.Items.Add(25);
            Player2TargetsList.Items.Add(50);
            Player1TargetsList.Items.Add(25);
            Player2TargetsList.Items.Add(50);
        }

        private void DartboardImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPosition = e.GetPosition(DartboardImage);
            Point bullseye = new Point(DartboardImage.ActualWidth / 2, DartboardImage.ActualHeight / 2);
            double distance = CalculateDistance(bullseye, clickPosition);
            if (distance > DartboardRadius)
            {
                MessageBox.Show("Hát ez fakapu ;)");
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

            currentPlayer = currentPlayer == 1 ? 2 : 1;
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
                        MessageBox.Show($"Player {currentPlayer} hit the bullseye and won the game!");
                        ResetGame();
                    }
                    else
                    {
                        MessageBox.Show($"Player {currentPlayer} hit sector {sector}. Now hit the bullseye to win!");
                    }
                }
                else
                {
                    MessageBox.Show($"Player {currentPlayer} hit sector {sector}. Next target: {playerTarget}");
                }
            }
            else
            {
                MessageBox.Show($"Player {currentPlayer} hit sector {sector}. Current target: {playerTarget}");
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);
            cursorp.Content = $"Cursor Position: X = {position.X}\n Y = {position.Y}";
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
            Player1TargetsList.Items.Clear();
            Player2TargetsList.Items.Clear();
            InitializeTargetListBoxes();
        }
    }
}
