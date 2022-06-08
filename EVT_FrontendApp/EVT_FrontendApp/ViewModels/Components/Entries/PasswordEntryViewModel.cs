using System;
using EVT_FrontendApp.ViewModels.Base;

namespace EVT_FrontendApp.ViewModels.Entries
{
    public class PasswordEntryViewModel : ExtendedBindableObject
    {
        private string _placeholder = String.Empty;
        public string Placeholder
        {
            get => _placeholder;
            set => SetProperty(ref _placeholder, value);
        }
        

        private string _text = String.Empty;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }


        private bool _isPassword;

        public bool IsPassword
        {
            get => _isPassword;
            set => SetProperty(ref _isPassword, value);
        }


        private string _passwordError;
        public string PasswordError
        {
            get => _passwordError;
            set => SetProperty(ref _passwordError, value);
        }

        public PasswordEntryViewModel(string placeholder = "", bool isPassword = true)
        {
            Placeholder = placeholder;
            IsPassword = isPassword;
            PasswordError = String.Empty;
        }
    }
}