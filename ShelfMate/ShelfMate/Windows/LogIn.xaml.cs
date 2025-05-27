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
using Microsoft.EntityFrameworkCore;
using ShelfMate.Data;
using ShelfMate.Models;
using ShelfMate.Helpers;

namespace ShelfMate.Windows
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        AppDbContext _db = new AppDbContext();
        
        public LogIn()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidationHelper.VerifyEmpty(usernameTxtBox.Text)) {
                MessageBox.Show("Completeaza numele de utilizator!", "Atentie", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ValidationHelper.VerifyEmpty(passwordTxtBox.Password)) {
                MessageBox.Show("Completeaza parola!", "Atentie", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            var exists = _db.Users.Include( x => x.Books).FirstOrDefault( x => x.Username == usernameTxtBox.Text );

            if(exists == null)
            {
                MessageBox.Show("Nu exista un cont cu acest username.", "Atentie", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!(exists.Password == passwordTxtBox.Password))
            {
                MessageBox.Show("Parola incorecta.", "Atentie", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MainWindow window = new MainWindow(exists);
            window.Show();
            this.Close();
        }
    }
}
