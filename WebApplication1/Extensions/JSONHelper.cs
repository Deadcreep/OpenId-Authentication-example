using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Extensions
{
    public class JSONHelper
    {
        public static T Deserialise<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                DataContractJsonSerializer serialiser = new DataContractJsonSerializer(obj.GetType());
                obj = (T)serialiser.ReadObject(ms);
            }
            return obj;
        }
    }
}
