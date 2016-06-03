using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace UmbracoTest.ViewModels
{
    public class RegisterUserViewModel
    {
        [DisplayName("User name")]
        [Required]
        public string UserName { get; set; }

        [DisplayName("Email address")]
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [Remote("ValidatePassword", "Validation", HttpMethod = "POST", AdditionalFields = "EmailAddress, UserName")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Password confirmation")]
        [Required]
        [DataType(DataType.Password)]
        public string PasswordConfirmation { get; set; }
    }
}