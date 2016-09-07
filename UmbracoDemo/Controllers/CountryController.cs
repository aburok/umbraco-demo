using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace UmbracoDemo.Controllers
{
    public class CountryController : SurfaceController
    {
        [HttpGet]
        public ActionResult GetAll(string nameQuery)
        {
            var countryList = GetCountryList();
            return Json(countryList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetFirst10()
        {
            var countryList = GetCountryList()
                .Take(10)
                .ToList();
            return Json(countryList, JsonRequestBehavior.AllowGet);
        }

        public static IEnumerable<CountryInfo> GetCountryList()
        {
            ICollection<CountryInfo> cultureList = new List<CountryInfo>();

            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo culture in cultures)
            {
                RegionInfo region = new RegionInfo(culture.LCID);

                if (cultureList.All(c => c.TwoLetterIsoCode != region.TwoLetterISORegionName))
                {
                    cultureList.Add(new CountryInfo(region));
                }
            }
            return cultureList;
        }
    }

    public class CountryInfo
    {
        public CountryInfo(RegionInfo regionInfo)
            :this(regionInfo.TwoLetterISORegionName,
                 regionInfo.EnglishName,
                 regionInfo.NativeName)
        { }

        public CountryInfo(string twoLetterIsoCode, string englishName, string nativeName)
        {
            TwoLetterIsoCode = twoLetterIsoCode;
            EnglishName = englishName;
            NativeName = nativeName;
        }

        public string TwoLetterIsoCode { get; private set; }
        public string EnglishName { get; private set; }
        public string NativeName { get; private set; }
        public string TimeStamp { get { return DateTime.Now.ToString(); } }

        public override string ToString()
        {
            return this.EnglishName;
        }
    }
}
