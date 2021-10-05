using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PublisherAndBook.ViewModels
{
    public class BookCreateModel
    {
        public int BookId { get; set; }
        [Required, StringLength(50), Display(Name = "Book Name")]
        public string BookName { get; set; }
        [Required, Column(TypeName = "money")]
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public IFormFile Picture { get; set; }
        //FK
        [Required, ForeignKey("Publisher"), Display(Name = "Publisher Name")]
        public int PublisherId { get; set; }
    }
}
