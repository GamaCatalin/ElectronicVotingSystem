using System;
using EVT_FrontendApp.ViewModels.Base;

namespace EVT_FrontendApp.ViewModels.Entries
{
    public class TextEntryViewModel : ExtendedBindableObject
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

        private string _fieldError;

        public string FieldError
        {
            get => _fieldError;
            set => SetProperty(ref _fieldError, value);
        }

        public TextEntryViewModel(string placeholder = "", bool isPassword = false)
        {
            Placeholder = placeholder;
            IsPassword = isPassword;
            FieldError = string.Empty;
        }
    }
}