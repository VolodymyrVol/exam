using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exam
{
    internal class Discount
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0, 100)]
        public int Percent {  get; set; }
        [Required]
        [MaxLength(100)]
        public string SaleTopic { get; set; }
        [Required]
        public DateOnly StartTime { get; set; }
        [Required]
        public DateTime? EndTime { get; set; }
    }
}
