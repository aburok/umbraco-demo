using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using Application.BusinessLogic.Services;

namespace Application.Web.Controllers
{
    public class SetCultureInfoFilter : ActionFilterAttribute
    {
        private readonly IUmbracoService _umbracoService;

        public SetCultureInfoFilter(IUmbracoService umbracoService)
        {
            _umbracoService = umbracoService;
        }

        //TODO : poor man ioc
        public SetCultureInfoFilter() : this(new UmbracoService()) { }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var isAjaxRequest = filterContext.RequestContext.HttpContext.Request.IsAjaxRequest();
            if (isAjaxRequest)
            {
                SetCultureForAjax();
            }
            base.OnActionExecuting(filterContext);
        }

        protected void SetCultureForAjax()
        {
            //we lose culture and dictionary wont get labels therefore set it on the thread
            string code = _umbracoService.GetCultureCode();
            //Get the culture info of the language code
            CultureInfo culture = CultureInfo.CreateSpecificCulture(code);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}
