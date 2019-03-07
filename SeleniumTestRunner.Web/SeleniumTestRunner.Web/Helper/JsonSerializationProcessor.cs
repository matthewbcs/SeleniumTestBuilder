using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SeleniumTestRunner.Web.Helper
{
    public class JsonSerializationProcessor
    {
         public T Deserialize<T>(string rawJson)
        {
            return (T) Deserialize(rawJson, typeof(T));
        }

        public T Deserialize<T>(string rawJson, Dictionary<string, string> propertyMappings)
        {
            return (T) Deserialize(rawJson, typeof(T), propertyMappings);
        }

        public object Deserialize(string rawJson, Type t)
        {
            return JsonConvert.DeserializeObject(rawJson, t);
        }

        public object Deserialize(string rawJson, Type t, Dictionary<string, string> propertyMappings)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ContractResolver = new JsonPropertyNameChanger(propertyMappings)
            };

            return JsonConvert.DeserializeObject(rawJson, t, settings);
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public string Serialize<T>(T obj, Dictionary<string, string> propertyMappings)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ContractResolver = new JsonPropertyNameChanger(propertyMappings)
            };

            return JsonConvert.SerializeObject(obj, settings);
        }




        //******************************************************************************
        //This is used to provide custom mapping for property name
        internal class JsonPropertyNameChanger : DefaultContractResolver
        {
            private readonly Dictionary<string, string> _propertyMappings;

            private readonly NamingStrategy _camelCaseNamingStrategy = new CamelCaseNamingStrategy
            {
                ProcessDictionaryKeys = true,
                OverrideSpecifiedNames = true
            };


            public JsonPropertyNameChanger(Dictionary<string, string> propertyMappings)
            {
                //DO NOT SET THE NAMING STRATEGY HERE OTHERWISE THE override ResolvePropertyName will not be used
                _propertyMappings = propertyMappings;
            }

            protected override string ResolvePropertyName(string propertyName)
            {
                string resolvedName = null;

                //This will try to relove based on above mentioned mapping
                bool doesCustomPropertyNameExists = _propertyMappings.TryGetValue(propertyName, out resolvedName);

                //if custom mapping exists in the list above, return that
                if (doesCustomPropertyNameExists)
                    return resolvedName;

                //else use the Camel Casing
                return _camelCaseNamingStrategy.GetPropertyName(propertyName, false);
            }
        }
    }
    }
