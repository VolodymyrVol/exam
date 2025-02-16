using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exam
{
    internal class Book
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string PubName { get; set; }
        [Required]
        public int Pages { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public DateOnly Year { get; set; }
        [Required]
        [Column("Buyprice")]
        public int StartPrice { get; set; }
        [Required]
        public int SellPrice { get; set; }
        public string? ChapterInfo { get; set; }
    }
}
