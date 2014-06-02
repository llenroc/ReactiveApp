using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Models
{
    public class Root
    {
        public IEnumerable<SampleDataGroup> Groups { get; set; }
    }
}
