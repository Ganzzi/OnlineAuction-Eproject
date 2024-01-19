using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities.Models
{
    public class AuctionHistory : BaseEntities
    {
        [Key]
        public int AuctionHistoryId { get; set; }

        [InverseProperty("AuctionHistory")]
        [ForeignKey("ItemId")]
        public Item? Item { get; set; }
        public int ItemId { get; set; }

        [InverseProperty("AuctionHistories")]
        [ForeignKey("WinnerId")]
        public User? Winner { get; set; }
        public int WinnerId { get; set; }
        public DateTime EndDate { get; set; }
        public float WinningBid { get; set; }
   
    }
}
