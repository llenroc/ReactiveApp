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
        /// <param name="data">The data.</param>
        public DataContainer(IDictionary<string, string> data = null)
        {
            this.Data = data ?? new Dictionary<string, string>();
        }

        public IDictionary<string, string> Data { get; private set; }
    }
}
