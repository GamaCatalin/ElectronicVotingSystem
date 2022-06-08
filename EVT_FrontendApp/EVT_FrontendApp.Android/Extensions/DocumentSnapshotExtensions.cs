using System.Collections.Generic;
using System.Linq;
using EVT_FrontendApp.Models;
using Firebase.Firestore;
using Java.Lang;
using Newtonsoft.Json;


namespace EVT_FrontendApp.Droid.Extensions
{
    public static class DocumentSnapshotExtensions
    {
        public static T Convert<T>(this DocumentSnapshot doc) where T : IIdentifiable
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(doc.Data.ToDictionary());
                var item = JsonConvert.DeserializeObject<T>(jsonString);
                item.Id = doc.Id;
                return item;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return default;
        }
        
        public static IList<T> Convert<T>(this QuerySnapshot docs) where T : IIdentifiable
        {
            var list = new List<T>();

            foreach (var doc in docs.Documents)
            {
                list.Add(doc.Convert<T>());
            }
            
            return list;
        }
    }
}