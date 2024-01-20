using DomainLayer.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities.Models
{
    public class CategoryItem:BaseEntities
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ItemId")]
        public Item? Item { get; set; }
        public int ItemId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        public int? CategoryId { get; set; }
    }
}
