using System;
using System.Windows;

namespace WPFDarts
{
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string player1Name = Player1NameTextBox.Text;
            string player2Name = Player2NameTextBox.Text;
            MainWindow MainWindow = new MainWindow(player1Name, player2Name);
            MainWindow.Show();
            this.Close();
        }
    }
}
