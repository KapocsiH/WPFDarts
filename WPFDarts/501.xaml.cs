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
        private int score = 0;

        // Inner Bullseye Click (50 points)
        private void BullseyeInner_Click(object sender, MouseButtonEventArgs e)
        {
            score += 50;
            UpdateScore();
        }

        // Outer Bullseye Click (25 points)
        private void BullseyeOuter_Click(object sender, MouseButtonEventArgs e)
        {
            score += 25;
            UpdateScore();
        }

        // Double Ring Click (Doubles the score)
        private void DoubleRing_Click(object sender, MouseButtonEventArgs e)
        {
            score *= 2;
            UpdateScore();
        }

        // Triple Ring Click (Triples the score)
        private void TripleRing_Click(object sender, MouseButtonEventArgs e)
        {
            score *= 3;
            UpdateScore();
        }

        // Updates the score display
        private void UpdateScore()
        {
            ScoreText.Text = $"Score: {score}";
        }
    }
}
