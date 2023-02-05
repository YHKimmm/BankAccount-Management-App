using System.ComponentModel.DataAnnotations;

namespace ASP.Net_MVC_Assignment.ViewModels
{
    public class ClientAccountVM
    {
        public int ClientID { get; set; }
        [Display(Name = "Account Number")]
        public int AccountNum { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Account Type")]
        public string AccountType { get; set; }
    }
}
