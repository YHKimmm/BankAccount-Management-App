using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASP.Net_MVC_Assignment.Models
{
    public class BankAccount
    {
        public BankAccount()
        {
            this.ClientAccounts = new HashSet<ClientAccount>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountNum { get; set; }

        [ForeignKey("AccountType")]
        public string AccountType { get; set; }

        public double Balance { get; set; }

        public BankAccountType BankAccountType { get; set; }

        public virtual ICollection<ClientAccount> ClientAccounts { get; set; }

    }
}
