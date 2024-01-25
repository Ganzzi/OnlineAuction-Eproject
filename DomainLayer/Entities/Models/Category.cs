using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities.Models
{
    public class Category : BaseEntities
    {
        [Key]
        public int? CategoryId { get; set; }
        [Required(ErrorMessage = "CategoryName require")]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Description require")]
        public string? Description { get; set; }
        public ICollection<CategoryItem>? CategoryItems { get; set;}
    }
}
