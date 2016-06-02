using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace UmbracoTest.ViewModels
{
    public class MemberRegisterModel : RenderModel, IValidatableObject
    {
        public MemberRegisterModel()
            : base(UmbracoContext.Current.PublishedContentRequest.PublishedContent)
        { }

        [DisplayName("First Name")]
        public virtual string GivenNames { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public virtual string FamilyNames { get; set; }

        [DisplayName("Email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [DisplayName("Email Confirmation")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddressConfirmation { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new System.NotImplementedException();
        }
    }
}
