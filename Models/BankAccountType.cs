using System.ComponentModel.DataAnnotations;

namespace ASP.Net_MVC_Assignment.Models
{
    public class BankAccountType
    {
        public BankAccountType()
        {
            this.BankAccounts = new HashSet<BankAccount>();
        }

        [Key]
        [Required]
        public string AccountType { get; set; }

        public ICollection<BankAccount> BankAccounts { get; set; }

    }
}
