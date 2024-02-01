using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities.helper
{
    public class successMail
    {
        public static string EmailForSeller(string checkEmailbuyer, string checkEmailseller)
        {
            return $@"<html>
        <head></head>
        <body>
        <div>
           Congratulations  {checkEmailseller} , we have the winner 
            is {checkEmailbuyer} please contact with the winner for shipping and transction plan
        </div>
        </body>
        </html>";
        }

        public static string EmailForByer(string checkEmailbuyer, string checkEmailseller)
        {
            return $@"<html>
        <head></head>
        <body>
        <div>
          Congratulations  {checkEmailbuyer}  you are the winner, 
           please contact with the seller {checkEmailseller}  for shipping and transction plan
                      
        </div>
        </body>
        </html>";
        }
    }
}
