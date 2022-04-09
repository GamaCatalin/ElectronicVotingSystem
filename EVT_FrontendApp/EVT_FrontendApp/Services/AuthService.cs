using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Repository;

namespace EVT_FrontendApp.Services
{
    public class AuthService
    {
        private readonly MockAuthDataStore _authData;
        private AuthInfo _currentAuthUser;

        public AuthService()
        {
            _authData = MockAuthDataStore.Instance();
            _currentAuthUser = null;
        }

        public async Task<bool> LoginUser(string username, string password)
        {
            var items = await _authData.GetItemsAsync(true);

            var foundUser = items.FirstOrDefault(item => item.Username == username && item.Password == password);

            if (foundUser != default(AuthInfo) )
            {
                _currentAuthUser = foundUser;
                return true;
            }

            return false;
        }

        public async Task<bool> RegisterUser(string username, string password)
        {
            if (_currentAuthUser != null)
            {
                return false;
            }
            
            var items = await _authData.GetItemsAsync(true);

            var foundUser = items.FirstOrDefault(item => item.Username == username);

            if (foundUser != default(AuthInfo))
            {
                return false;
            }

            var status = await _authData.AddItemAsync(new AuthInfo(username, password));
            return status;
        }

        public async Task<bool> LogoutUser()
        {
            await _authData.GetItemAsync(_currentAuthUser.Id);
            _currentAuthUser = null;
            return true;
        }
    }
}