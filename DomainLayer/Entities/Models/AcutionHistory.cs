using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities.Models
{
    public class AcutionHistory : BaseEntities
    {
        [Key]
        public int AcutionHistoryId { get; set; }

        [ForeignKey("ItemId")]
        public Item? Item { get; set; }
        public int ItemId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
        public int UserId { get; set; }

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public float WinningBid { get; set; }
   
    }
}
