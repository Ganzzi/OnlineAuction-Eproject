using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DomainLayer.Entities.Models
{
    public class User : BaseEntities
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } = "User";
        public string? Avatar { get; set; }
        [NotMapped]
        public IFormFile? AvatarFile {set; get;}
        
        public RefreshToken? RefreshToken { get; set; }
        public ICollection<Bid>? Bids { get; set; }
        public ICollection<Item>? SoldItems { get; set; }

        [InverseProperty("Rater")]
        public ICollection<Rating>? Ratings { get; set; }

        [InverseProperty("RatedUser")]
        public ICollection<Rating>? BeingRateds { get; set; }

        [NotMapped]
        public float? AverageBeingRated {get; set;}

        [InverseProperty("Winner")]
        public ICollection<AuctionHistory>? AuctionHistories { get; set; }

        [InverseProperty("User")]
        public ICollection<Notification>? Notifications { get; set; }
    }
}
