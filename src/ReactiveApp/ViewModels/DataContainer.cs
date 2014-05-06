using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.ViewModels
{
    public class DataContainer : IDataContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContainer"/> class.
        /// </summary>
        public DataContainer()
        {
            this.Data = new Dictionary<string, string>();
        }

        public IDictionary<string, string> Data { get; private set; }
    }
}
