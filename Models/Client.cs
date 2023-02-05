using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ASP.Net_MVC_Assignment.Models
{
    public class Client
    {
        public Client()
        {
            this.ClientAccounts = new HashSet<ClientAccount>();
        }

        [Key]
        [Display(Name = "Client ID")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientID { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }

        public virtual ICollection<ClientAccount> ClientAccounts { get; set; }
    }

}
