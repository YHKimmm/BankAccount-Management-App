using ASP.Net_MVC_Assignment.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ASP.Net_MVC_Assignment.ViewModels
{
    public class BankAccountVM
    {

        [Display(Name = "Account Number")]
        public int AccountNum { get; set; }

        [Display(Name = "Account Type")]
        public string AccountType { get; set; }

        [Required(ErrorMessage = "Balance is required.")]
        [RegularExpression("^\\s*(?=.*[1-9])\\d*(?:\\.\\d{1,2})?\\s*$", ErrorMessage = "Positive number with two decimal digits only please")]
        public double Balance { get; set; }
    }
}
