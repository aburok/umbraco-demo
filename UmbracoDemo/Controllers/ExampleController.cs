using System.Web.Mvc;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;
using UmbracoDemo.Services;

namespace UmbracoDemo.Controllers
{
    public class ExampleController : SurfaceController
    {
        private readonly IContentService _contentService;
        private readonly ICityRepository _cityRepository;

        public ExampleController(IContentService contentService,
            ICityRepository cityRepository)
        {
            _contentService = contentService;
            _cityRepository = cityRepository;
        }

        // GET: Example
        public ActionResult Index()
        {
            var rootContent = _contentService.GetRootContent();
            ViewBag.Cities = _cityRepository.GetCities();

            return View(rootContent);
        }
    }
}