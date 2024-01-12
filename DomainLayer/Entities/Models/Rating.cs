using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities.Models
{
    public class Rating : BaseEntities
    {
        [Key]
        public int RatingId { get; set; }
        
        [ForeignKey("ItemId")]
        public Item? Item { get; set; }
        public int ItemId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
        public int UserId { get; set; }
        
        public float Rate { get; set; }
        public DateTime RatingDate { get; set; }
    }
}
