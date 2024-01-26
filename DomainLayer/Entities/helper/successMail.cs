using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities.helper
{
    public class successMail
    {
        public static string EmailForSeller(string email)
        {
            return $@"<html>
        <head></head>
        <body>
        <div>
           {email}       
        </div>
        </body>
        </html>";
        }

        public static string EmailForByer(string email)
        {
            return $@"<html>
        <head></head>
        <body>
        <div>
           {email}       
        </div>
        </body>
        </html>";
        }
    }
}
