using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReactiveApp.Services;

namespace TestApp
{
    public class TestAppJsonSerializer : ISerializer
    {
        private readonly Newtonsoft.Json.JsonSerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="WPNLJsonSerializer"/> class.
        /// This class is used to serialize complex object used in navigation.
        /// </summary>
        public TestAppJsonSerializer(JsonSerializerSettings settings = null)
        {
            //creates a JsonSerizlizer from the specified settings.
            settings = settings ?? new JsonSerializerSettings()
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All,
            };

            serializer = Newtonsoft.Json.JsonSerializer.Create(settings);
        }

        /// <summary>
        /// Deserializes the provided string to an object of the provided type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="stringToSerialize">The string to serialize.</param>
        /// <returns>An object of the specified type.</returns>
        public object DeserializeObject(Type type, string stringToSerialize)
        {
            using (var reader = new StringReader(stringToSerialize))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize(jsonReader, type);
                }
            }
        }

        /// <summary>
        /// Serializes the object into a string.
        /// </summary>
        /// <param name="objectToSerialise">The object to serialise.</param>
        /// <returns>A string representing the object.</returns>
        public string SerializeObject(object objectToSerialise)
        {
            using (var writer = new StringWriter())
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    serializer.Serialize(jsonWriter, objectToSerialise);
                }
                return writer.ToString();
            }
        }
    }
}
