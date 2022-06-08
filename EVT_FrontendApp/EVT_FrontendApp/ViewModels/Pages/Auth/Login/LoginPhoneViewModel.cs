using System;
using System.Windows.Input;
using EVT_FrontendApp.Services.Account;
using EVT_FrontendApp.Services.Alerts;
using EVT_FrontendApp.Services.Navigation;
using EVT_FrontendApp.ViewModels.Base;
using Xamarin.Forms;

namespace EVT_FrontendApp.ViewModels.Pages.Login
{
    public class LoginPhoneViewModel : BaseViewModel
    {
        private string _phoneNumber = String.Empty;

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        private string _code;

        public string Code
        {
            get => _code;
            set => SetProperty(ref _code, value);
        }

        private bool _codeSent = false;

        public bool CodeSent
        {
            get => _codeSent;
            set => SetProperty(ref _codeSent, value);
        }

        private string _buttonText = "Send Code";

        public string ButtonText
        {
            get => _buttonText;
            set => SetProperty(ref _buttonText, value);
        }

        private ICommand _nextCommand;

        public ICommand NextCommand
        {
            get => _nextCommand;
            set => SetProperty(ref _nextCommand, value);
        }


        private IAccountService _accountService;
        private bool _codeRequested;
        private INavigationService _navigationService;
        private IAlertService _alertService;

        public LoginPhoneViewModel(IAccountService accountService, INavigationService navigationService,
            IAlertService alertService)
        {
            _accountService = accountService;
            _navigationService = navigationService;
            _alertService = alertService;

            CodeSent = false;
            NextCommand = new Command(OnNextAction);
        }

        private async void OnNextAction(object obj)
        {
            if (_codeRequested)
            {
                // verify code that the user entered

                var loginException = await _accountService.VerifyOtpCodeAsync(Code);

                if (loginException is null)
                {
                    await _navigationService.NavigateToAsync<DashboardViewModel>(null, true);
                }
                else
                {
                    await _alertService.AlertAsync("Invalid credentials!", loginException.Message);
                }
            }
            else
            {
                if (!PhoneNumber.StartsWith("+4"))
                {
                    PhoneNumber = "+4" + _phoneNumber;
                }

                var sendCodeException = await _accountService.SendOtpCodeAsync(PhoneNumber);

                if (sendCodeException is null)
                {
                    CodeSent = true;
                }
                else
                {
                    CodeSent = false;
                    
                    await _alertService.AlertAsync("Invalid phone number!", sendCodeException.Message);
                    return;
                }

                // if (!CodeSent)
                // {
                //     await _alertService.AlertAsync("Invalid phone number!", sendCodeException.Message);
                //     return;
                // }

                _codeRequested = true;
                ButtonText = "Verify Code";
            }
        }
    }
}