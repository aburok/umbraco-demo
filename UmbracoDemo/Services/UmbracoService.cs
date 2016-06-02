using System;
using OneCms.BusinessLogic.Services;
using umbraco.cms.businesslogic.web;
using Umbraco.Core.Services;

namespace Application.BusinessLogic.Services
{
    using System.Linq;
    using System.Web;
    using Constants;
    using OneCms.BusinessLogic;
    using Umbraco.Core.Models;
    using Umbraco.Web;

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
            :this(UmbracoContext.Current.Application.Services.DomainService)
        { }

        public string GetCultureCode()
        {
            //Get language id from the Root Node ID
            string requestDomain = HttpContext.Current.Request
                .ServerVariables["SERVER_NAME"].ToLower();

            var domain = GetMatchedDomain(requestDomain);

            if (domain != null)
            {
                return domain.LanguageIsoCode;
            }

            return "en-US";
        }

        /// <summary>
        /// Gets domain object from request. Errors if no domain is found.
        /// </summary>
        /// <param name="requestDomain"></param>
        /// <returns></returns>
        public IDomain GetMatchedDomain(string requestDomain)
        {
            //var domains = HttpRuntime.Cache.Get("UmbracoDomainList") as List<Domain>;

            //var domains = Domain.GetDomains();
            var domainList = _domainService.GetAll(true);

            string fullRequest = HttpContext.Current.Request.Url.AbsolutePath.ToLower().Contains("/umbraco/surface")
                                     ? HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Replace("https://", string.Empty).Replace("http://", string.Empty)
                                     : requestDomain + HttpContext.Current.Request.Url.AbsolutePath;

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

            //Umbraco allows domains in this format ch.domain.com/fr
            //These are used to handle region sites that have sub languages
            //This final check looks for ch.domain.com just in case the request was something like
            //ch.domain.com/blah - this will not get found by the explicit match above, so we just find the closest
            //matching domain and use that instead.

            return matchedDomain;
        }
    }
}
