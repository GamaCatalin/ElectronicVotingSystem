using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EVT_FrontendApp.Models;

namespace EVT_FrontendApp.Repository
{
    public interface IRepository<T> : ICloneable where T : IIdentifiable
    {
        Task<T> Get(string id);
        Task<IList<T>> GetAll();
        Task<string> Save(T item);
        Task<string> Add(T item);

        Task<bool> Delete(string id);
        IRepository<T> SetDocumentPath(string documentPath);
    }
}