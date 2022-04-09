using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EVT_FrontendApp.Models;

namespace EVT_FrontendApp.Repository
{
    public class MockElectionDataStore : IDataStore<Ballot>
    {
        readonly List<Ballot> items;

        public MockElectionDataStore()
        {
            items = new List<Ballot>()
            {
                new Ballot()
                {
                    Id = Guid.NewGuid().ToString(), ElectionName = "Election 1", Options = new List<CandidateOption>()
                    {
                        new CandidateOption(){Id = Guid.NewGuid().ToString(), Title = "Candidate 1-1"},
                        new CandidateOption(){Id = Guid.NewGuid().ToString(), Title = "Candidate 1-2"},
                        new CandidateOption(){Id = Guid.NewGuid().ToString(), Title = "Candidate 1-3"},
                        new CandidateOption(){Id = Guid.NewGuid().ToString(), Title = "Candidate 1-4"},
                        new CandidateOption(){Id = Guid.NewGuid().ToString(), Title = "Hold Vote"}
                    }
                },
                
                new Ballot()
                {
                    Id = Guid.NewGuid().ToString(), ElectionName = "Election 2", Options = new List<CandidateOption>()
                    {
                        new CandidateOption(){Id = Guid.NewGuid().ToString(), Title = "Candidate 2-1"},
                        new CandidateOption(){Id = Guid.NewGuid().ToString(), Title = "Candidate 2-2"},
                        new CandidateOption(){Id = Guid.NewGuid().ToString(), Title = "Candidate 2-3"},
                        new CandidateOption(){Id = Guid.NewGuid().ToString(), Title = "Candidate 2-4"},
                        new CandidateOption(){Id = Guid.NewGuid().ToString(), Title = "Hold Vote"}
                    }
                },
                
                new Ballot()
                {
                    Id = Guid.NewGuid().ToString(), ElectionName = "Election 3", Options = new List<CandidateOption>()
                    {
                        new CandidateOption(){Id = Guid.NewGuid().ToString(), Title = "Candidate 3-1"},
                        new CandidateOption(){Id = Guid.NewGuid().ToString(), Title = "Candidate 3-2"},
                        new CandidateOption(){Id = Guid.NewGuid().ToString(), Title = "Candidate 3-3"},
                        new CandidateOption(){Id = Guid.NewGuid().ToString(), Title = "Candidate 3-4"},
                        new CandidateOption(){Id = Guid.NewGuid().ToString(), Title = "Hold Vote"}
                    }
                }
            };
        }

        public async Task<bool> AddItemAsync(Ballot item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Ballot item)
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

        public async Task<Ballot> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Ballot>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}