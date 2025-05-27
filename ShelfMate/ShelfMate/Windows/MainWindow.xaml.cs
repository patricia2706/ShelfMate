using ShelfMate.Data;
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
using ShelfMate.Models;
using ShelfMate.Data;
using System.Collections.ObjectModel;
using ShelfMate.Windows;

namespace ShelfMate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AppDbContext _db = new AppDbContext();
        User user = null;
        public ObservableCollection<Book> Books { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Books = new ObservableCollection<Book>(_db.Books.ToList());
            this.DataContext = this;
        }


        public MainWindow(User u)
        {
            InitializeComponent();
            user = u;
            Books = new ObservableCollection<Book>(_db.Books.Where(b => b.UserId == user.Id).ToList());
            this.DataContext = this;
        }

        private void addBookBtn_Click(object sender, RoutedEventArgs e)
        {
            AddBook addBook = new AddBook(user);
            addBook.Show();
            this.Close();
            
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedBook = (Book)dataGridBooks.SelectedItem;

            if (selectedBook != null)
            {
                editBookBtn.Visibility = Visibility.Visible;
                deleteBookBtn.Visibility = Visibility.Visible;
            }
            else
            {
                editBookBtn.Visibility = Visibility.Collapsed;
                deleteBookBtn.Visibility = Visibility.Collapsed;
            }

        }

        private void editBookBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = (Book)dataGridBooks.SelectedItem;
            AddBook edit = new AddBook(user, selectedBook);
            edit.Show();
            this.Close();

        }

        private void deleteBookBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = (Book)dataGridBooks.SelectedItem;
            if (selectedBook != null) {
                var result = MessageBox.Show("Doriti sa stergeti cartea selectata?", "Atentie", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes) {
                    _db.Books.Remove(selectedBook);
                    _db.SaveChanges();
                    MessageBox.Show("Cartea a fost stearsa.","Stergere completa",MessageBoxButton.OK);
                    MainWindow mainWindow = new MainWindow(user);
                    mainWindow.Show();
                    this.Close();
                    
                }
                else
                {
                    MessageBox.Show("Cartea nu a fost stearsa.", "Stergere incompleta", MessageBoxButton.OK);
                }
            }
        }

        private void filtrareBookBtn_Click(object sender, RoutedEventArgs e)
        {
            bool suntVizibile = allRadioBtn.Visibility == Visibility.Visible;

            
            var nouaVizibilitate = suntVizibile ? Visibility.Collapsed : Visibility.Visible;

            allRadioBtn.Visibility = nouaVizibilitate;
            fictiuneRadioBtn.Visibility = nouaVizibilitate;
            romanceRadioBtn.Visibility = nouaVizibilitate;
            SFRadioBtn.Visibility = nouaVizibilitate;
            thrillerRadioBtn.Visibility = nouaVizibilitate;
            biografieRadioBtn.Visibility = nouaVizibilitate;
            istorieRadioBtn.Visibility = nouaVizibilitate;
        }

        private void fictiuneRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            Books = new ObservableCollection<Book>(_db.Books.Where(b => b.UserId == user.Id && b.Genre.Equals("Ficțiune")).ToList());
            dataGridBooks.ItemsSource = Books;
        }

        private void romanceRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            Books = new ObservableCollection<Book>(_db.Books.Where(b => b.UserId == user.Id && b.Genre.Equals("Romantic")).ToList());
            dataGridBooks.ItemsSource = Books;
        }

        private void SFRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            Books = new ObservableCollection<Book>(_db.Books.Where(b => b.UserId == user.Id && b.Genre.Equals("SF")).ToList());
            dataGridBooks.ItemsSource = Books;
        }

        private void thrillerRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            Books = new ObservableCollection<Book>(_db.Books.Where(b => b.UserId == user.Id && b.Genre.Equals("Thriller")).ToList());

            dataGridBooks.ItemsSource = Books;
        }

        private void biografieRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            Books = new ObservableCollection<Book>(_db.Books.Where(b => b.UserId == user.Id && b.Genre.Equals("Biografie")).ToList());

            dataGridBooks.ItemsSource = Books;
        }

        private void istorieRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            Books = new ObservableCollection<Book>(_db.Books.Where(b => b.UserId == user.Id && b.Genre.Equals("Istorie")).ToList());

            dataGridBooks.ItemsSource = Books;

        }

        private void allRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            Books = new ObservableCollection<Book>(_db.Books.Where(b => b.UserId == user.Id).ToList());

            dataGridBooks.ItemsSource = Books;
        }

        private void statisticsBtn_Click(object sender, RoutedEventArgs e)
        {
            StatisticsWindow statsWindow = new StatisticsWindow(user);
            statsWindow.Show();
            this.Close();
        }
    }
}