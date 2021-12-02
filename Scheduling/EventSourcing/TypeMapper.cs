using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Scheduling.EventSourcing;

public static class TypeMapper
{
        static readonly Dictionary<string, Func<JObject, object>> DataToType
            = new Dictionary<string, Func<JObject, object>>();

        static readonly Dictionary<Type, Func<object, (string, JObject)>> TypeToData
            = new Dictionary<Type, Func<object, (string, JObject)>>();

        public static void Map<T>(string name, Func<JObject, object> dataToType, Func<T, JObject> typeToData)
        {
            if (DataToType.ContainsKey(name))
                return;

            DataToType[name] = dataToType;
            TypeToData[typeof(T)] = o =>
            {
                var data = typeToData((T) o);
                return (name, data);
            };
        }

        public static bool TryGetDataToType(string name) => DataToType.TryGetValue(name, out _);

        public static bool TryGetTypeToData(Type type) => TypeToData.TryGetValue(type, out _);

        public static Func<JObject, object> GetDataToType(string name)
        {
            if (!TryGetDataToType(name))
                throw new Exception($"Failed to find type mapped with '{name}'");

            return DataToType[name];
        }

        public static Func<object, (string name, JObject data)> GetTypeToData(Type type)
        {
            if (!TryGetTypeToData(type))
                throw new Exception($"Failed to find name mapped with '{type}'");

            return TypeToData[type];
        }
}