using System.Web.Mvc;
using Castle.Core.Smtp;
using Umbraco.Web.Mvc;
using UmbracoDemo.Services;

namespace UmbracoDemo.Controllers
{
    public class ExampleController : SurfaceController
    {
        private readonly IEmailService _emailService;

        public ExampleController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // GET: Example
        public ActionResult Index()
        {
            _emailService.SendEmail("test@test.test", "test.test@test.test", "Test email", "Testing test email.");
            return View();
        }
    }
}