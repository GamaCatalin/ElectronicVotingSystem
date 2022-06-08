using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using EVT_FrontendApp.Services;
using System.Threading.Tasks;
using Android.Gms.Extensions;
using EVT_FrontendApp.Droid.Extensions;
using EVT_FrontendApp.Droid.Services;
using EVT_FrontendApp.Droid.Services.ServiceListeners;
using EVT_FrontendApp.Models;
using EVT_FrontendApp.Repository;
using Firebase.Auth;
using Xamarin.Forms;
using EVT_FrontendApp.Services.Account;
using EVT_FrontendApp.ViewModels.Base;
using Firebase;
using Firebase.Firestore;
using Firebase.Firestore.Auth;
using Google.Rpc.Context;
using Java.Util.Concurrent;
using Xamarin.Essentials;

[assembly: Dependency(typeof(AccountService))]

namespace EVT_FrontendApp.Droid.Services
{
    class AccountService : PhoneAuthProvider.OnVerificationStateChangedCallbacks, IAccountService
    {
        private const long OTP_TIMEOUT = 30;
        private TaskCompletionSource<AggregateException> _phoneAuthTcs;
        private string _verificationId;


        public AccountService()
        {
        }

        public Task<AggregateException> LoginAsync(string username, string password)
        {
            var tcs = new TaskCompletionSource<AggregateException>();

            var sha = new SHA512Managed();
            var encryptedHash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            sha.Clear();
            var encryptedPassword = Convert.ToBase64String(encryptedHash);

            FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(username, encryptedPassword)
                .ContinueWith((task) => OnAuthCompleted(task, tcs));

            return tcs.Task;
        }

        public Task<AggregateException> SignupAsync(string email, string password, AuthenticatedUser user)
        {
            var tcs = new TaskCompletionSource<AggregateException>();

            var sha = new SHA512Managed();
            var encryptedHash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            sha.Clear();
            var encryptedPassword = Convert.ToBase64String(encryptedHash);


            FirebaseAuth.Instance
                .CreateUserWithEmailAndPasswordAsync(email, encryptedPassword)
                .ContinueWith((task) => OnSignupCompleted(task, tcs, user));

            // FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, encryptedPassword).Con
            

            return tcs.Task;
        }

        public Task<AggregateException> SendOtpCodeAsync(string phoneNumber)
        {
            _phoneAuthTcs = new TaskCompletionSource<AggregateException>();
            PhoneAuthProvider.Instance.VerifyPhoneNumber(phoneNumber,
                OTP_TIMEOUT,
                TimeUnit.Seconds,
                Platform.CurrentActivity,
                this
            );

            return _phoneAuthTcs.Task;
        }

        public Task<AggregateException> VerifyOtpCodeAsync(string code)
        {
            if (!string.IsNullOrWhiteSpace(_verificationId))
            {
                var credential = PhoneAuthProvider.GetCredential(_verificationId, code);
                var tcs = new TaskCompletionSource<AggregateException>();
                FirebaseAuth.Instance.SignInWithCredentialAsync(credential)
                    .ContinueWith((task) => OnAuthCompleted(task, tcs));
                
                return tcs.Task;
            }

            return Task.FromResult(new AggregateException());
        }

        public Task<AuthenticatedUser> GetUserAsync()
        {
            var tcs = new TaskCompletionSource<AuthenticatedUser>();

            FirebaseFirestore.Instance
                .Collection("users")
                .Document(FirebaseAuth.Instance.CurrentUser.Uid)
                .Get()
                .AddOnCompleteListener(new OnGetCompleteListener(tcs));

            return tcs.Task;
        }

        public async Task<string> UpdateUserAsync(AuthenticatedUser newUser)
        {
            var tcs = new TaskCompletionSource<string>();
            var userRepository = ViewModelLocator.Resolve<IRepository<AuthenticatedUser>>();

            var result = await userRepository.Save(newUser);
            tcs.TrySetResult(result);

            return await tcs.Task;
        }


        private void OnAuthCompleted(Task task, TaskCompletionSource<AggregateException> tcs)
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                // something went wrong
                tcs.SetResult(task.Exception);
                return;
            }

            var isEmailVerified = FirebaseAuth.Instance.CurrentUser.IsEmailVerified; 
            
            if (isEmailVerified)
            {
                tcs.SetResult(new AggregateException("Email is not verified!"));
            }
            else
            {
                tcs.SetResult(task.Exception);
            }
            
            _verificationId = null;
        }

        private void OnSignupCompleted(Task<IAuthResult> task, TaskCompletionSource<AggregateException> tcs,
            AuthenticatedUser user)
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                // signup broke
                tcs.SetResult(task.Exception);
                return;
            }

            var newUserId = task.Result.User.Uid;

            var verificationTcs = new TaskCompletionSource<AggregateException>();

            task.Result.User.SendEmailVerification();
            
            FirebaseFirestore.Instance.Collection("users").Document(newUserId).Set(user.Convert());

            tcs.SetResult(task.Exception);
        }

        public override void OnVerificationCompleted(PhoneAuthCredential p0)
        {
            System.Diagnostics.Debug.WriteLine("PhoneAuthCredential created Automatically");
        }

        public override void OnVerificationFailed(FirebaseException p0)
        {
            System.Diagnostics.Debug.WriteLine("Verification Failed: " + p0.Message);
            _phoneAuthTcs?.TrySetResult(new AggregateException(p0.Message));
        }

        public override void OnCodeSent(string verificationId,
            PhoneAuthProvider.ForceResendingToken forceResendingToken)
        {
            base.OnCodeSent(verificationId, forceResendingToken);
            _verificationId = verificationId;
            _phoneAuthTcs?.TrySetResult(null);
        }

        public async Task<string> GetUserOrganizationAsync()
        {
            var user = await GetUserAsync();
            return user.OrganizationId;
        }

        public Task<string> GetUserIdAsync()
        {
            return Task.FromResult(FirebaseAuth.Instance.CurrentUser.Uid);
        }

        public Task<AuthenticatedUser> GetUserByEmailAsync(string email)
        {
            var tcs = new TaskCompletionSource<AuthenticatedUser>();


            FirebaseFirestore.Instance
                .Collection("users")
                .WhereEqualTo("Email", email)
                .AddSnapshotListener(new OnUserFilterSnapshotListener(tcs));

            return tcs.Task;
        }

        public async Task<string> SetUserOrganizationAsync(AuthenticatedUser user, string organizationId)
        {
            user.OrganizationId = organizationId;

            return await UpdateUserAsync(user);
        }

        public async Task<bool> RemoveOrganizationFromUserAsync(string userId)
        {
            var result = await FirebaseFirestore.Instance.Collection("users").Document(userId)
                .Update("OrganizationId", "");

            return true;
        }

        public void LogoutAsync()
        {
            FirebaseAuth.Instance.SignOut();
        }

        public Task<bool> SendPasswordResetEmailAsync(string email)
        {
            var tcs = new TaskCompletionSource<bool>();

            FirebaseAuth.Instance
                .SendPasswordResetEmail(email)
                .AddOnCompleteListener(new OnResetPasswordCompleteListener(tcs));


            return tcs.Task;
        }
    }
}