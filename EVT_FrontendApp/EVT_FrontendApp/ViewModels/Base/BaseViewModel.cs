using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Repository;
using EVT_FrontendApp.Services;
using EVT_FrontendApp.Services.Account;
using EVT_FrontendApp.Services.Alerts;
// using Nancy.TinyIoc;
using TinyIoC;
using Xamarin.Forms;

namespace EVT_FrontendApp.ViewModels.Base
{
    public class BaseViewModel :  ExtendedBindableObject, INotifyPropertyChanged
    {
        static TinyIoCContainer _container;
        static Dictionary<Type, Type> _lookupTable;
        static BaseViewModel()
        {
            _container = new TinyIoCContainer();
            _lookupTable = new Dictionary<Type, Type>();

        }
        

        bool _isBusy = false;
        
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }


        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }
        
        
        
        string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public virtual Task InitializeAsync(object navigationData = null)
        {
            return Task.CompletedTask;
        }

        #region INotifyPropertyChanged
        
        
        public new event PropertyChangedEventHandler PropertyChanged;
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
