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
        public float Rate { get; set; }
        public DateTime RatingDate { get; set; }
        
        [ForeignKey("ItemId")]
        [InverseProperty("Rating")]
        public Item? Item { get; set; }
        public int ItemId { get; set; }

        [ForeignKey("RaterId")] // Specify a unique foreign key name
        [InverseProperty("Ratings")]
        public User? Rater { get; set; }
        public int RaterId { get; set; }

        [ForeignKey("RatedUserId")] // Specify a unique foreign key name
        [InverseProperty("BeingRateds")]
        public User? RatedUser { get; set; }
        public int? RatedUserId { get; set; }
    }
}
