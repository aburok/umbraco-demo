using System.Web.Mvc;
using System.Web.UI;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using UmbracoTest.Validation;

namespace UmbracoTest.Controllers
{
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    public class ValidationController : SurfaceController
    {
        [System.Web.Http.HttpPost]
        public JsonResult ValidatePassword(ValidatePasswordRequest request)
        {
            // both are required
            if (string.IsNullOrWhiteSpace(request.EmailAddress)
                || string.IsNullOrWhiteSpace(request.UserName)
                || string.IsNullOrWhiteSpace(request.Password))
            {
                return Json(true);
            }

            var validationResults = PasswordValidator.Validate(
                request.Password,
                request.UserName,
                request.EmailAddress);

            // if no errors were returned then return true
            if (validationResults == PasswordMessage.Valid)
                return Json(true);

            return Json(validationResults.ToString());
        }
    }

    public class ValidatePasswordRequest
    {
        public string Password { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
    }
}