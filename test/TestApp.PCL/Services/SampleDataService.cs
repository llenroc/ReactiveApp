using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TestApp.Models;

namespace TestApp.Services
{
    public class SampleDataService : ISampleDataService
    {
        public SampleDataService()
        {
        }

        public async Task<Root> GetSampleDataAsync()
        {
            using (var stream = typeof(SampleDataService).GetTypeInfo().Assembly.GetManifestResourceStream("TestApp.SampleData.json"))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    return JsonConvert.DeserializeObject<Root>(await streamReader.ReadToEndAsync());
                }
            }           
        }
    }
}
