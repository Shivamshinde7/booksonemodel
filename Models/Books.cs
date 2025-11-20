using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace book.Models
{
    public class Books
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Author { get; set; } = "";

        public string Genre { get; set; } = "";

        public string Description { get; set; } = "";

        [NotMapped] 
        public IFormFile? ImgFile { get; set; }

        public string? ImgFileName { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
