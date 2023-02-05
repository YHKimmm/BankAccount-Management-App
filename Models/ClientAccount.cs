using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ASP.Net_MVC_Assignment.Models
{
    public class ClientAccount
    {
        public int ClientID { get; set; }
        
        public int AccountNum { get; set; }

        public virtual Client Client { get; set; } = null!;
        public virtual BankAccount BankAccount { get; set; } = null!;
    }
}
 