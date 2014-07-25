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
    public class JsonSerializer : ISerializer
    {
        private readonly Newtonsoft.Json.JsonSerializer serializer;
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializer"/> class.
        /// </summary>
        public JsonSerializer(JsonSerializerSettings settings = null)
        {
            settings = settings ?? new JsonSerializerSettings()
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All,
            };

            serializer = Newtonsoft.Json.JsonSerializer.Create(settings);
        }

        public object DeserializeObject(Type type, string stringToSerialize)
        {
            using(var reader = new StringReader(stringToSerialize))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize(jsonReader);
                }
            }
        }

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
