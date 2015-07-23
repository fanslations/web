using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Thi.Core
{
    public static class JsonHelper
    {
        /// <summary>
        /// Serializes the specified model to json string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">The model to serialize.</param>
        /// <returns></returns>
        public static string Serialize<T>(T model)
        {
            return JsonConvert.SerializeObject(model);
        }

        /// <summary>
        /// Deserializes the specified json to a model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
