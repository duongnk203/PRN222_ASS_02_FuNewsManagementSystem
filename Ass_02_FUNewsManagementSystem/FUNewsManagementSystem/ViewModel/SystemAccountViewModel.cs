using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.ViewModel
{
    public class AccountLogin
    {
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
    ErrorMessage = "Email invalid")]
        public string AccountEmail { get; set; }
        [Required(ErrorMessage = "Password is not empty")]
        [RegularExpression(@"^(?=.*[\W_])(?=\S{7,}$).*$",
            ErrorMessage = "Password is invalid")]
        public string Password { get; set; }

    }

    public class AccountUpdate
    {
        public short AccountId { get; set; }
        [Required]
        public string AccountName { get; set; } = null!;
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
    ErrorMessage = "Email invalid")]
        public string AccountEmail { get; set; }
        [Required]
        public int AccountRole { get; set; }
    }
}
