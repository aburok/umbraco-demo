using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace UmbracoDemo.Services
{
    public interface IUmbracoService
    {
        string GetCultureCode();
    }

    public class UmbracoService : IUmbracoService
    {
        private readonly IDomainService _domainService;

        public UmbracoService(IDomainService domainService)
        {
            _domainService = domainService;
        }

        // TODO : poor man ioc
        public UmbracoService()
            : this(UmbracoContext.Current.Application.Services.DomainService)
        { }

        protected HttpRequest Request => HttpContext.Current.Request;

        public string GetCultureCode()
        {
            //Get language id from the Root Node ID
            string requestDomain = Request.ServerVariables["SERVER_NAME"].ToLower();

            var domain = GetMatchedDomain(requestDomain);

            if (domain != null)
            {
                return domain.LanguageIsoCode;
            }

            return "en-US";
        }

        protected string NormalizeUrl(string originalUrl)
        {
            return originalUrl
                .Replace("https://", string.Empty)
                .Replace("http://", string.Empty);
        }

        /// <summary>
        /// Gets domain object from request. Errors if no domain is found.
        /// </summary>
        /// <param name="requestDomain"></param>
        /// <returns></returns>
        public IDomain GetMatchedDomain(string requestDomain)
        {
            var domainList = _domainService
                .GetAll(true)
                .ToList();

            string fullRequest = Request.Url.AbsolutePath.ToLower().Contains("/umbraco/surface")
                 ? NormalizeUrl(Request.UrlReferrer.AbsoluteUri)
                 : requestDomain + Request.Url.AbsolutePath;

            // walk backwards on full Request until domain found
            string currentTest = fullRequest;
            IDomain matchedDomain = null;
            while (currentTest.Contains("/"))
            {
                matchedDomain = domainList
                    .SingleOrDefault(x => x.DomainName == currentTest.TrimEnd('/'));
                if (matchedDomain != null)
                {
                    // this is the actual domain
                    break;
                }
                if (currentTest == requestDomain)
                {
                    // no more to test.
                    break;
                }

                currentTest = currentTest.Substring(0, currentTest.LastIndexOf("/"));
                matchedDomain = domainList
                    .SingleOrDefault(x => x.DomainName == currentTest);
                if (matchedDomain != null)
                {
                    // this is the actual domain
                    break;
                }
            }

            return matchedDomain;
        }
    }
}
