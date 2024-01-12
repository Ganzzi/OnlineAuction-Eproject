using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities.Models
{
    public class Bid : BaseEntities
    {
        [Key]
        public int BidId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }     
        public int UserId { get; set; }

        [ForeignKey("ItemId")]
        public Item? Item { get; set; }
        public int ItemId { get; set; }

        public float BidAmout { get; set; }
        public DateTime BidDate { get; set; }
    }
}
