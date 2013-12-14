using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Exceptions
{
    public interface IExceptionHandler
    {
        void HandleException(Exception e);
    }
}
