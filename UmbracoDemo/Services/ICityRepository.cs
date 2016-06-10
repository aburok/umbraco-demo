using System.Collections.Generic;

namespace UmbracoDemo.Services
{
    public interface ICityRepository
    {
        IEnumerable<string> GetCities();
    }
}