using System;
using System.IO;
using System.Threading.Tasks;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Services.Account;
using EVT_FrontendApp.Services.Image;
using EVT_FrontendApp.Services.Navigation;
using EVT_FrontendApp.ViewModels.Base;
using EVT_FrontendApp.ViewModels.Buttons;
using EVT_FrontendApp.ViewModels.Entries;
using EVT_FrontendApp.ViewModels.Pages.Login;
using Firebase.Storage;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EVT_FrontendApp.ViewModels.Pages.Profile
{
    public class ProfileViewModel : BaseViewModel
    {
        private static AuthenticatedUser _user;

        public AuthenticatedUser User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private ImageSource _profilePicture;
        public ImageSource ProfilePicture
        {
            get => _profilePicture;
            set => SetProperty(ref _profilePicture, value);
        }

        private bool _isSavedSuccessfully;
        public bool IsSavedSuccessfully
        {
            get => _isSavedSuccessfully;
            set => SetProperty(ref _isSavedSuccessfully, value);
        }

        
        
        private string _successfulSaveText;
        public string SuccessfulSaveText
        {
            get => _successfulSaveText;
            set => SetProperty(ref _successfulSaveText, value);
        }

        private double _uploadPhotoProgress;

        public double UploadPhotoProgress
        {
            get => _uploadPhotoProgress;
            set => SetProperty(ref _uploadPhotoProgress, value);
        }
        
        private bool _isProgressBarVisible;
        public bool IsProgressBarVisible
        {
            get => _isSavedSuccessfully;
            set => SetProperty(ref _isSavedSuccessfully, value);
        }
        
        public TextEntryViewModel FirstNameEntryViewModel { get; set; }
        public TextEntryViewModel LastNameEntryViewModel { get; set; }
        
        public ButtonModel LogOutModel { get; set; }
        public ButtonModel SaveChangesModel { get; set; }
        public ButtonModel ChangePictureButtonModel { get; set; }
        
        
        private INavigationService _navigationService;
        private IAccountService _accountService;
        private IImageService _imageService;

        public ProfileViewModel(INavigationService navigationService, IAccountService accountService, IImageService imageService)
        {
            _navigationService = navigationService;
            _accountService = accountService;
            _imageService = imageService;
            
            FirstNameEntryViewModel = new TextEntryViewModel("First Name");
            LastNameEntryViewModel = new TextEntryViewModel("Last Name");

            

            LogOutModel = new ButtonModel("LOG OUT", OnLogoutAction);
            SaveChangesModel = new ButtonModel("SAVE CHANGES", OnSaveChangesAction);
            ChangePictureButtonModel = new ButtonModel("Change profile picture", OnChangeProfilePictureAction);

            IsProgressBarVisible = false;
        }

        private async void OnChangeProfilePictureAction()
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo == null)
            {
                return;
            }

            var fileExtension = "." + photo.ContentType.Split('/')[1];

            // var photoStream = new MemoryStream(_imageService.ResizeImageToStream(await photo.OpenReadAsync(), 150));

            
            var uploadPhotoTask = new FirebaseStorage("electronicvotingsystem-f56d2.appspot.com",
                    new FirebaseStorageOptions
                    {
                        ThrowOnCancel = true
                    })
                .Child("ProfilePictures")
                .Child(_user.Id + fileExtension)
                .PutAsync(await photo.OpenReadAsync());
                // .PutAsync(photoStream);

            IsProgressBarVisible = true;
            uploadPhotoTask.Progress.ProgressChanged += UploadPhotoProgressChanged;



            var photoLink = await uploadPhotoTask;
            AuthenticatedUser user = User;

            user.ProfilePictureURL = photoLink;
            
            IsBusy = true;
            
            var result = await _accountService.UpdateUserAsync(user);

            // if (string.IsNullOrEmpty(result))
            // {
            //     return;
            // }
            // else
            // {
            //     
            // }
            IsBusy = false;
            IsSavedSuccessfully = true;
            IsProgressBarVisible = false;
            LoadProfilePicture();
        }

        private void UploadPhotoProgressChanged(object sender, FirebaseStorageProgress e)
        {
            if (e.Percentage >= 100)
            {
                IsProgressBarVisible = false;
                LoadProfilePicture();
            }
            UploadPhotoProgress = e.Percentage / 100;
        }

        private async void OnLogoutAction()
        {
            _accountService.LogoutAsync();
            await _navigationService.NavigateToAsync<LoginViewModel>(null, true);
        }

        private async void OnSaveChangesAction()
        {
            IsBusy = true;
            AuthenticatedUser updatedUser = User;
            updatedUser.FirstName = FirstNameEntryViewModel.Text;
            updatedUser.LastName = LastNameEntryViewModel.Text;

            var result = await _accountService.UpdateUserAsync(updatedUser);

            // if (string.IsNullOrEmpty(result))
            // {
            //     return;
            // }
            // else
            // {
            //     
            // }
            IsBusy = false;
            IsSavedSuccessfully = true;
            
            FlashSuccessTextOnScreen();
        }

        private async void FlashSuccessTextOnScreen()
        {
            SuccessfulSaveText = "Changes saved!";
            await Task.Delay(3000);
            SuccessfulSaveText = "";            
        }

        

        private async void LoadProfilePicture()
        {
            User = await _accountService.GetUserAsync();
            if (string.IsNullOrEmpty(User.ProfilePictureURL))
            {
                OSAppTheme appTheme = App.Current.RequestedTheme;
                ImageSource profilePicture = appTheme == OSAppTheme.Dark
                    ? ImageSource.FromFile("UserDefault_Black_128")
                    : ImageSource.FromFile("UserDefault_White_128");

                ProfilePicture = profilePicture;
            }
            else
            {
                ProfilePicture = ImageSource.FromUri(new Uri(User.ProfilePictureURL));
            }
        }
        
        public override Task InitializeAsync(object navigationData = null)
        {
            if (navigationData is AuthenticatedUser user)
            {
                User = user;
            }

            FirstNameEntryViewModel.Text = User.FirstName;
            LastNameEntryViewModel.Text = User.LastName;

            LoadProfilePicture();
            

            return base.InitializeAsync(navigationData);
        }
    }
}