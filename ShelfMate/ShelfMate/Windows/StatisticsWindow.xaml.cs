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
using LiveCharts;
using LiveCharts.Wpf;
using System.Linq;
using System.Collections.Generic;
using ShelfMate.Data;
using ShelfMate.Models;

namespace ShelfMate.Windows
{
    public partial class StatisticsWindow : Window
    {
        AppDbContext _db = new AppDbContext();
        User u = null;
        public StatisticsWindow()
        {
            InitializeComponent();
            LoadGenreStatistics();
        }

        public StatisticsWindow(User user)
        {
            InitializeComponent();
            LoadGenreStatistics();
            u = user;
        }

        private void LoadGenreStatistics()
        {
            
            var genreStats = _db.Books
                                .GroupBy(b => b.Genre)
                                .Select(g => new
                                {
                                    Genre = g.Key,
                                    Count = g.Count()
                                }).ToList();

            
            foreach (var item in genreStats)
            {
                genrePieChart.Series.Add(new PieSeries
                {
                    Title = item.Genre,
                    Values = new ChartValues<int> { item.Count },
                    DataLabels = true
                });
            }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(u);
            mainWindow.Show();
            this.Close();
        }
    }
}
