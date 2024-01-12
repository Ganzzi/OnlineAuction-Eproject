using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Entities.Models
{
    public class Item : BaseEntities
    {
        [Key]
        public int ItemId { get; set; }
        
        public string Title { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string ImgUrl { get; set; }

        [NotMapped]
        public Stream Image { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
        public int UserId { get; set; }

        public ICollection<Bid>? Bids { get; set; }
           
        public ICollection<CategoryItem>? CategoryItems { get; set; }
     
    }
}
