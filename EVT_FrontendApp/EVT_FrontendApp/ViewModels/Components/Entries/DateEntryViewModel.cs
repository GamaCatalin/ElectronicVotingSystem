using System;
using EVT_FrontendApp.ViewModels.Base;

namespace EVT_FrontendApp.ViewModels.Entries
{
    public class DateEntryViewModel : ExtendedBindableObject
    {

        public DateTime MinimumDate
        {
            get;
            set;
        }

        public DateTime MaximumDate
        {
            get;
            set;
        }
        
        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }


        private string _fieldError;
        public string FieldError
        {
            get => _fieldError;
            set => SetProperty(ref _fieldError, value);
        }

        public DateEntryViewModel(string placeholder = "", bool isPassword = false)
        {
            Date = DateTime.Now;
            FieldError = string.Empty;
            MinimumDate = Date;
            MaximumDate = Date + TimeSpan.FromDays(50);
        }
    }
}