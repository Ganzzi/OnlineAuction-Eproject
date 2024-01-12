using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class BaseEntities
    {
        public DateTime? Created { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
