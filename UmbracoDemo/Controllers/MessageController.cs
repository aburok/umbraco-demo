using System.Web.Mvc;
using Umbraco.Web.Mvc;
using UmbracoDemo.Filters;

namespace UmbracoDemo.Controllers
{
    public class MessageController : SurfaceController
    {
        [HttpPost]
        public ActionResult WithoutCultureFilter()
        {
            return Json(GetMessage());
        }

        [SetCultureInfoFilter]
        [HttpPost]
        public ActionResult WithCultureFilter()
        {
            return Json(GetMessage());
        }

        private string GetMessage()
        {
            var message = Umbraco.GetDictionaryValue("Message");
            return message;
        }
    }
}