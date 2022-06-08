using System;
using System.Text.RegularExpressions;
using System.Windows.Input;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Services.Account;
using EVT_FrontendApp.Services.Alerts;
using EVT_FrontendApp.Services.Navigation;
using EVT_FrontendApp.ViewModels.Base;
using EVT_FrontendApp.ViewModels.Buttons;
using EVT_FrontendApp.ViewModels.Entries;
using EVT_FrontendApp.ViewModels.Pages.Login;
using Xamarin.Forms;

namespace EVT_FrontendApp.ViewModels.Pages.Signup
{
    public class SignupViewModel : BaseViewModel
    {
        private ICommand _registerCommand;

        public ICommand RegisterCommand
        {
            get => _registerCommand;
            set => SetProperty(ref _registerCommand, value);
        }
        
                
        public TextEntryViewModel EmailEntryViewModel { get; set; }
        public TextEntryViewModel FirstNameEntryViewModel { get; set; }
        public TextEntryViewModel LastNameEntryViewModel { get; set; }
        public PasswordEntryViewModel PasswordEntryViewModel { get; set; }
        
        public PasswordEntryViewModel PasswordConfirmationEntryViewModel { get; set; }
        public TextEntryViewModel PhoneNumberEntryViewModel { get; set; }

        public ButtonModel SignUpModel { get; set; }


        private INavigationService _navigationService;
        private IAccountService _accountService;
        private IAlertService _alertService;
        
        

        public SignupViewModel(IAlertService alertService, IAccountService accountService,
            INavigationService navigationService)
        {
            _navigationService = navigationService;
            _accountService = accountService;
            _alertService = alertService;

            
            EmailEntryViewModel = new TextEntryViewModel("Email");
            FirstNameEntryViewModel = new TextEntryViewModel("First Name");
            LastNameEntryViewModel = new TextEntryViewModel("Last Name");
            PasswordEntryViewModel = new PasswordEntryViewModel("Password");
            PasswordConfirmationEntryViewModel = new PasswordEntryViewModel("Confirm Password");
            PhoneNumberEntryViewModel = new TextEntryViewModel("Phone number (optional)");
            
            SignUpModel = new ButtonModel("SIGN UP", OnSignUpAction);
        }

        private async void OnSignUpAction()
        {
            if (!ValidateEmail())
            {
                return;
            }
            
            if (!ValidatePassword())
            {
                return;
            }

            if (!ValidatePhoneNumber())
            {
                return;
            }

            IsBusy = true;
        
            var email = EmailEntryViewModel.Text;
            var password = PasswordEntryViewModel.Text;

            var user = CreateUser();
            
            var signUpException = await _accountService.SignupAsync(email, password, user);

            if (signUpException is null)
            {
                AddPhoneNumberAsLoginOption();
                            
                // await _navigationService.NavigateToAsync<LoginViewModel>();
                IsBusy = false;
                await _navigationService.GoBackAsync();
            }
            else
            {
                HandleSignUpException(signUpException);
                
            }

            IsBusy = false;
        }

        private AuthenticatedUser CreateUser()
        {
            var firstName = FirstNameEntryViewModel.Text;
            var lastName = LastNameEntryViewModel.Text;
            
            return new AuthenticatedUser
            {
                FirstName = firstName,
                LastName = lastName,
                Email = EmailEntryViewModel.Text
            };
        }
        
        private void AddPhoneNumberAsLoginOption()
        {
            // var phoneNumber = PhoneNumberEntryViewModel.Text;
        }

        private async void HandleSignUpException(AggregateException exception)
        {
            var innerExceptionMessage = exception.InnerExceptions[0].Message;

            if (innerExceptionMessage == "The email address is already in use by another account.")
            {
                EmailEntryViewModel.FieldError = "Email already in use";
            }
            else
            {
                await _alertService.AlertAsync("Signup exception", innerExceptionMessage);
            }
        }
        
        private bool ValidatePassword()
        {
            var password = PasswordEntryViewModel.Text;
            var passwordConfirmation = PasswordConfirmationEntryViewModel.Text;

            PasswordEntryViewModel.PasswordError = "";
            PasswordConfirmationEntryViewModel.PasswordError = "";


            if (password.Length < 6 || string.IsNullOrEmpty(password))
            {
                PasswordEntryViewModel.PasswordError = "Password must be at least 6 characters long!";

                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                PasswordEntryViewModel.PasswordError = "Password must not be empty!";
                    
                return false;
            }


            if (password != passwordConfirmation)
            {
                PasswordEntryViewModel.PasswordError = "Passwords do not match!";
                PasswordConfirmationEntryViewModel.PasswordError = "Passwords do not match!";
                return false;
            }

            return true;
        }

        private bool ValidateEmail()
        {
            var email = EmailEntryViewModel.Text;
            EmailEntryViewModel.FieldError = "";

            if (string.IsNullOrWhiteSpace(email))
            {
                EmailEntryViewModel.FieldError = "Email must not be empty!";
                    
                return false;
            }

            var emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (!emailRegex.IsMatch(email))
            {
                EmailEntryViewModel.FieldError = "Email is not valid!";

                return false;
            }
                        
            return true;
        }
        
        private bool ValidatePhoneNumber()
        {
            var phoneNumber = PhoneNumberEntryViewModel.Text;

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return true;
            }
            
            if (!phoneNumber.StartsWith("+4"))
            {
                phoneNumber = "+4" + phoneNumber;
                PhoneNumberEntryViewModel.Text = phoneNumber;
            }
            
            var phoneNumberRegex = new Regex(@"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$");

            if (!phoneNumberRegex.IsMatch(phoneNumber))
            {
                PhoneNumberEntryViewModel.FieldError = "Phone number is not valid!";

                return false;
            }
                        
            return true;
        }
    }
    
    
}