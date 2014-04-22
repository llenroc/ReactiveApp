using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Services
{
    public interface INavigationSerializer
    {
        string SerializeObject(object objectToSerialise);

        object DeserializeObject(Type type, string stringToSerialize);
    }
}
