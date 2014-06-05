using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Services
{
    public interface ISerializer
    {
        /// <summary>
        /// Deserializes the string into an object of the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="stringToSerialize">The string to serialize.</param>
        /// <returns>The deserialized object.</returns>
        object DeserializeObject(Type type, string stringToSerialize);

        /// <summary>
        /// Serializes the object into a string.
        /// </summary>
        /// <param name="objectToSerialise">The object to serialise.</param>
        /// <returns>A string representing the object.</returns>
        string SerializeObject(object objectToSerialise);
    }
}
