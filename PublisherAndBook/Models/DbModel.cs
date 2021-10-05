using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PublisherAndBook.Models
{
    public class Publisher
    {
        public Publisher() { this.Books = new List<Book>(); }
        public int PublisherId { get; set; }
        [Required, StringLength(50), Display(Name = "Publisher Name")]
        public string PublisherName { get; set; }
        [Required, Column(TypeName = "date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Establish { get; set; }
        //Navigation
        public virtual ICollection<Book> Books { get; set; }
    }
    public class Book
    {
        public int BookId { get; set; }
        [Required, StringLength(50), Display(Name = "Book Name")]
        public string BookName { get; set; }
        [Required, Column(TypeName = "money")]
        public decimal Price { get; set; }
        public bool Status { get; set; }
        [Required, StringLength(150)]
        public string Picture { get; set; }
        //FK
        [Required, ForeignKey("Publisher"), Display(Name = "Publisher Name")]
        public int PublisherId { get; set; }
        //Navigation
        public virtual Publisher Publisher { get; set; }
    }
    public class ManageDbContext : DbContext
    {
        public ManageDbContext(DbContextOptions<ManageDbContext> options) : base(options) { }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
