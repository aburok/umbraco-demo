using System.Web.Mvc;
using Umbraco.Web.Mvc;
using UmbracoDemo.Filters;

namespace UmbracoDemo.Controllers
{
    public class ValidationController : SurfaceController
    {
        // GET: Validation
        public ActionResult VanillaValidate()
        {
            return View();
        }

        [SetCultureInfoFilter]
        public ActionResult ValidateWithFilter()
        {
            return View();
        }
    }
}