using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DomainLayer.Entities.helper
{
    public class EmailBody
    {
        public static string EmailStringBody(string email, string emailToken, IConfiguration configuration)
        {
            string ClientUrl = configuration["ClientUrl"] ?? "https://localhost:3000";

            return $@"
                <head></head>
                <html>
                    <body>
                        <div>
                            <h3>reset</h3>
                            <a href=""{ClientUrl}/auth/reset_password?email={email}&code={emailToken}"" target=""_blank"">ResetPassword</a>    
                        </div>
                    </body>
                </html>";
        }

    }
}
