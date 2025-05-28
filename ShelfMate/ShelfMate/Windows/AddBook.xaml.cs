using System;
using System.Collections.Generic;
using System.IO;
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
using ShelfMate.Data;
using ShelfMate.Helpers;
using ShelfMate.Models;


namespace ShelfMate.Windows
{
    /// <summary>
    /// Interaction logic for AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        AppDbContext _db = new AppDbContext();
        User u = null;
        Book editingBook;
        string relativePath;

        public AddBook()
        {
            InitializeComponent();
        }

        public AddBook(User user)
        {
            InitializeComponent();

            this.u = user;

        }

        public AddBook(User user, Book b)
        {
            InitializeComponent();
            this.u = user;

            if (b != null) {
                editingBook = b;
                titleTxtBox.Text = editingBook.Title;
                authorTxtBox.Text = editingBook.Author;
                genreComboBox.Text = editingBook.Genre;
                yearTxtBox.Text = editingBook.PublishYear.ToString();
                string fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, editingBook.CoverImagePath);
                coverImage.Source = new BitmapImage(new Uri(fullPath, UriKind.Absolute));
            }
        }

        private void Image_DragOver(object sender, DragEventArgs e)
        {
            
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1 && IsImageFile(files[0]))
                {
                    e.Effects = DragDropEffects.Copy; 
                }
                else
                {
                    e.Effects = DragDropEffects.None;  
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }

            e.Handled = true;
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1 && IsImageFile(files[0]))
                {
                    string originalPath = files[0];
                    string extension = System.IO.Path.GetExtension(originalPath);
                    string fileNameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(originalPath);

                    
                    string uniqueName = $"{fileNameWithoutExt}_{Guid.NewGuid()}{extension}";

                    
                    string imagesFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                    Directory.CreateDirectory(imagesFolder);

                    
                    string destPath = System.IO.Path.Combine(imagesFolder, uniqueName);

                    
                    File.Copy(originalPath, destPath, true);

                    
                    coverImage.Source = new BitmapImage(new Uri(destPath));

                    
                    relativePath = System.IO.Path.Combine("Images", uniqueName);
                }
            }
        }

        private bool IsImageFile(string path)
        {
            string extension = System.IO.Path.GetExtension(path).ToLower();
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".bmp"||extension == ".webp";
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidationHelper.VerifyEmpty (titleTxtBox.Text)) {
                MessageBox.Show("Completeaza titlul!", "Atentie", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ValidationHelper.VerifyEmpty(authorTxtBox.Text))
            {
                MessageBox.Show("Completeaza autorul!", "Atentie", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ValidationHelper.VerifyEmpty(genreComboBox.Text))
            {
                MessageBox.Show("Completeaza genul!", "Atentie", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ValidationHelper.VerifyEmpty(yearTxtBox.Text))
            {
                MessageBox.Show("Completeaza anul publicarii!", "Atentie", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (coverImage.Source == null)
            {
                MessageBox.Show("Adauga coperta!", "Atentie", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (editingBook != null)
            {

                editingBook.Title = titleTxtBox.Text;
                editingBook.Author = authorTxtBox.Text;
                editingBook.Genre = genreComboBox.Text;
                editingBook.PublishYear = int.Parse(yearTxtBox.Text);
                editingBook.CoverImagePath = relativePath ?? editingBook.CoverImagePath;

                _db.Books.Update(editingBook);
            }
            else
            {
                Book newBook = new Book
                {
                    Title = titleTxtBox.Text,
                    Author = authorTxtBox.Text,
                    Genre = genreComboBox.Text,
                    PublishYear = int.Parse(yearTxtBox.Text),
                    CoverImagePath = relativePath,
                    UserId = u.Id
                };

                _db.Books.Add(newBook);
            }

            _db.SaveChanges();
            MessageBox.Show("Cartea a fost salvată!");
            MainWindow main = new MainWindow(u);
            main.Show();
            this.Close();
            
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(u);
            mainWindow.Show();
            this.Close();
        }
    }
}
