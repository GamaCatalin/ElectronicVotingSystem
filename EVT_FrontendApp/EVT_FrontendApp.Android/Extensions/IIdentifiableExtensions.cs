using System;
using System.Collections.Generic;
using EVT_FrontendApp.Models;
using Java.Util;
using Newtonsoft.Json;

namespace EVT_FrontendApp.Droid.Extensions
{
    public static class IIdentifiableExtensions
    {
        public static HashMap Convert(this IIdentifiable item)
        {
            var dict = new Java.Util.HashMap();

            var jsonStr = JsonConvert.SerializeObject(item);
            var propertyDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonStr);

            foreach (var key in propertyDict.Keys)
            {
                if (key.Equals("Id"))
                    continue;

                var val = propertyDict[key];
                Java.Lang.Object javaVal = null;

                if (val is string str)
                {
                    javaVal = new Java.Lang.String(str);
                }
                else if (val is double dbl)
                {
                    javaVal = new Java.Lang.Double(dbl);
                }
                else if (val is int intVal)
                {
                    javaVal = new Java.Lang.Integer(intVal);
                }
                else if (val is long longVal)
                {
                    javaVal = new Java.Lang.Long(longVal);
                }
                else if (val is DateTime dt)
                {
                    javaVal = val.ToString();
                }
                else if (val is bool boolVal)
                {
                    javaVal = new Java.Lang.Boolean(boolVal);
                }

                if (javaVal != null)
                {
                    dict.PutIfAbsent(key,javaVal);
                }
                    
            }
            
            return dict;
        }
    }
}