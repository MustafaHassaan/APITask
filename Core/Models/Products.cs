using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name Is Required"), MaxLength(100)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Description Is Required"), MaxLength(250)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Photo Is Required")]
        public string? Photopath { get; set; }

        [Required(ErrorMessage = "Price Is Required")]
        public double Price { get; set; }
    }
}
