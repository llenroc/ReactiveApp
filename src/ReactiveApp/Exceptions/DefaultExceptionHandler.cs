using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Interfaces;
using Splat;

namespace ReactiveApp.Exceptions
{
    public class DefaultExceptionHandler : IExceptionHandler, IEnableLogger
    {
        private readonly IFullLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultExceptionHandler"/> class.
        /// </summary>
        public DefaultExceptionHandler()
        {
            this.logger = this.Log();
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void HandleException(Exception e)
        {
            this.logger.ErrorException(e.Message, e);
        }
    }
}
