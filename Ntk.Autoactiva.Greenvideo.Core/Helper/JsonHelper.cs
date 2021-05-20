using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Ntk.Autoactiva.Greenvideo.Core.Helper
{
    public static class JsonHelper
    {
        static JsonHelper()
        {
            Deserializesettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Error = HandleDeserializationError
            };
        }

        private static void HandleDeserializationError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs errorArgs)
        {
            var currentError = errorArgs.ErrorContext.Error.Message;
            errorArgs.ErrorContext.Handled = true;
        }
        private static JsonSerializerSettings Deserializesettings;

        public static string JsonHelperSerializeObject<T>(this T obj) where T : class
        {
            if (obj == null)
                return "";

            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            });

        }

        public static string JsonHelperSerializeObjectIncludeNullValue<T>(this T obj) where T : class
        {
            if (obj == null)
                return "";

            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

        }
        public static T JsonHelperDeserializeObject<T>(this T defaultModel, string json) where T : class
        {
            if (string.IsNullOrEmpty(json))
            {
                if (defaultModel != null)
                    return defaultModel;
                return Activator.CreateInstance<T>();
            }
            return JsonConvert.DeserializeObject<T>(json, Deserializesettings);
        }

        public static T JsonHelperDeserializeObject<T>(this string json) where T : class
        {
            if (string.IsNullOrEmpty(json))
            {
                return Activator.CreateInstance<T>();
            }
            try
            {
                return JsonConvert.DeserializeObject<T>(json, Deserializesettings);
            }
            catch
            {


            }
            return Activator.CreateInstance<T>();
        }
        public static T JsonHelperDeserializeObject<T>(this string json, Type type) where T : class
        {
            if (string.IsNullOrEmpty(json))
                return Activator.CreateInstance<T>();
            try
            {
                return (T)JsonConvert.DeserializeObject(json, type);
            }
            catch
            {


            }
            return Activator.CreateInstance<T>();

        }
        public static object JsonHelperDeserializeObject(this string json, Type type)
        {
            if (string.IsNullOrEmpty(json))
                return Activator.CreateInstance(type);
            try
            {
                return JsonConvert.DeserializeObject(json, type);
            }
            catch
            {


            }
            return Activator.CreateInstance(type);
        }

        public static byte[] ToByteArray<T>(this T t)
        {
            using (var memoryStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, t);
                return memoryStream.ToArray();
            }
        }

        public static object FromByteArray(this byte[] data)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream);
            }
        }
    }
}
