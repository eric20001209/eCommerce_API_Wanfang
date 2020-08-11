using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce_API.Services
{
    public interface iMailService
    {
        string email { get; set; }
        string password { get; set; }
        void sendEmail(string email, string password);
    }
}
