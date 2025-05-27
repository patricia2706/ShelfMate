using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfMate.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int PublishYear { get; set; }
        public string? CoverImagePath { get; set; }
        public int UserId { get; set; } 
        public User? User { get; set; }

        public string FullCoverImagePath
        {
            get
            {
                if (string.IsNullOrEmpty(CoverImagePath)) return null;

                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CoverImagePath);
            }
        }


    }
}

