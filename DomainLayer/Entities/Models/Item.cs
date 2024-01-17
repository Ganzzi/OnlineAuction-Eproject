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
        public float StartingPrice { get; set; }
        public float IncreasingAmount { get; set; }
        public float? MinSellingPrice { get; set; }
        public float? ReservePrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Image { get; set; }

        [NotMapped]
        public Stream ImageFile { get; set; }

        [ForeignKey("SellerId")]
        public User? Seller { get; set; }
        public int SellerId { get; set; }

        [InverseProperty("Item")]
        public Rating? Rating {get; set;}
        public ICollection<Bid>? Bids { get; set; }
        public ICollection<CategoryItem>? CategoryItems { get; set; }
    }
}
