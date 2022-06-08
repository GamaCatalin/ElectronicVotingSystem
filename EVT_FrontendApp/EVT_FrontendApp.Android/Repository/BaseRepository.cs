using System.Collections.Generic;
using System.Threading.Tasks;
using EVT_FrontendApp.Droid.Extensions;
using EVT_FrontendApp.Droid.Services.ServiceListeners;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Repository;
using Firebase.Firestore;

namespace EVT_FrontendApp.Droid.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : IIdentifiable
    {
        protected string DocumentPath { get; set; }

        public BaseRepository(string documentPath = "")
        {
            SetDocumentPath(documentPath);
        }

        public virtual Task<T> Get(string id)
        {
            var tcs = new TaskCompletionSource<T>();

            FirebaseFirestore.Instance
                .Collection(DocumentPath)
                .Document(id)
                .Get()
                .AddOnCompleteListener(new OnDocumentCompleteListener<T>(tcs));

            return tcs.Task;
        }

        public virtual Task<IList<T>> GetAll()
        {
            var tcs = new TaskCompletionSource<IList<T>>();
            
            FirebaseFirestore.Instance
                .Collection(DocumentPath)
                .Get()
                .AddOnCompleteListener(new OnCollectionCompleteListener<T>(tcs));

            return tcs.Task;
        }

        public virtual Task<string> Save(T item)
        {
            var tcs = new TaskCompletionSource<string>();

            FirebaseFirestore.Instance
                .Collection(DocumentPath)
                .Add(item.Convert())
                .AddOnCompleteListener(new OnSaveCompleteListener(tcs));

            return tcs.Task;
        }
        
        public virtual Task<string> Add(T item)
        {
            var tcs = new TaskCompletionSource<string>();

            FirebaseFirestore.Instance
                .Collection(DocumentPath)
                .Add(item.Convert())
                .AddOnCompleteListener(new OnSaveCompleteListener(tcs));

            return tcs.Task;
        }

        public virtual Task<bool> Delete(string id)
        {
            var tcs = new TaskCompletionSource<bool>();

            FirebaseFirestore.Instance
                .Collection(DocumentPath)
                .Document(id)
                .Delete()
                .AddOnCompleteListener(new OnDeleteCompleteListener(tcs));
                
            return tcs.Task;
        }

        public virtual IRepository<T> SetDocumentPath(string documentPath)
        {
            DocumentPath = documentPath;
            return this;
        }

        public object Clone()
        {
            return new BaseRepository<T>(DocumentPath);
        }
    }
}