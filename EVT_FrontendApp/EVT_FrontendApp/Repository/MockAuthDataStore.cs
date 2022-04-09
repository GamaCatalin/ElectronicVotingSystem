using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EVT_FrontendApp.Models;

namespace EVT_FrontendApp.Repository
{
    public class MockAuthDataStore : IDataStore<AuthInfo>
    {
        private static MockAuthDataStore instance;
        readonly List<AuthInfo> items;
        
        public MockAuthDataStore()
        {
            items = new List<AuthInfo>()
            {
                new AuthInfo(){Id = Guid.NewGuid().ToString(), Username = "user1", Password = "pass"},
                new AuthInfo(){Id = Guid.NewGuid().ToString(), Username = "user2", Password = "pass"},
                new AuthInfo(){Id = Guid.NewGuid().ToString(), Username = "user3", Password = "pass"},
                new AuthInfo(){Id = Guid.NewGuid().ToString(), Username = "user4", Password = "pass"}
            };
        }

        public static MockAuthDataStore Instance()
        {
            return instance ?? (instance = new MockAuthDataStore());
        }
        
        public async Task<bool> AddItemAsync(AuthInfo item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(AuthInfo item)
        {
            var oldItem = items.FirstOrDefault(arg => arg.Id == item.Id);
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.FirstOrDefault(arg => arg.Id == id);
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<AuthInfo> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<AuthInfo>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}