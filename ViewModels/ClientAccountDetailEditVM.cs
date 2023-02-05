using ASP.Net_MVC_Assignment.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ASP.Net_MVC_Assignment.ViewModels
{
    public class ClientAccountDetailEditVM
    {
        public string Message { get; set; }

        [Display(Name = "Client ID")]
        public int ClientID { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Alphabetical only please.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Alphabetical only please.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }

        [Display(Name = "Account Number")]
        public int AccountNum { get; set; }

        [Display(Name = "Account Type")]
        public string AccountType { get; set; }

        [Required(ErrorMessage = "Balance is required.")]
        [RegularExpression("^\\s*(?=.*[1-9])\\d*(?:\\.\\d{1,2})?\\s*$", ErrorMessage = "Positive number with two decimal digits only please")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double Balance { get; set; }
     
    }
    
}
