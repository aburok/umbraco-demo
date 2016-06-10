using System.Collections.Generic;

namespace UmbracoDemo.Services
{
    public class CityRepository : ICityRepository
    {
        public IEnumerable<string> GetCities()
        {
            return new List<string>()
            {
                "London", "Cracow", "Berlin", "Paris", "Madrid", "Moscow"
            };
        }
    }
}