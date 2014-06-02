using System.Threading.Tasks;
using TestApp.Models;

namespace TestApp.Services
{
    public interface ISampleDataService
    {
        Task<Root> GetSampleDataAsync();
    }
}