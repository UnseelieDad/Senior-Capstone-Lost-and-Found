using Acr.UserDialogs;
using LostAndFound.Models;
using LostAndFound.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LostAndFound.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinPage : ContentPage
    {

        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        private static readonly TimeSpan lockoutLength = TimeSpan.FromMinutes(5);
        private static readonly int maxAttempts = 3;
        private bool wasLockedOut;
        private int attempts;

        public PinPage()
        {
            InitializeComponent();
            attempts = 0;
            wasLockedOut = false;
            PinEntry.Focus();
        }

        private async void Entry_Completed(object sender, EventArgs e)
        {
            if (IsLockedOut())
            {
                await DisplayAlert("Alert", "You are temporarily locked out.", "OK");
                return;
            }

            var encrypted = HashUtilities.GetHashFromString(PinEntry.Text);
            UserDialogs.Instance.ShowLoading();
            var response = await Backend.AdminLogin(encrypted);
            UserDialogs.Instance.HideLoading();
            if (response != null)
            {
                if (!response.Body.Equals("Incorrect PIN provided"))
                {
                    await RootPage.NavigateFromMenu((int)MenuItemType.Admin);
                }
                else
                {
                    await DisplayAlert("Error", response.Body, "OK");
                    attempts++;
                    if (attempts >= maxAttempts)
                    {
                        LockOut();
                    }
                }
            }
            else
            {
                await DisplayAlert("Error", "Unable to connect.", "OK");
            }
        }

        private async void LockOut()
        {
            await DisplayAlert("Alert", "You have entered an incorrect PIN too many times. You will be locked out for 5 minutes.", "OK");
            Preferences.Set("lockout", DateTime.Now);
            wasLockedOut = true;
        }

        private bool IsLockedOut()
        {
            var lastLockout = Preferences.Get("lockout", new DateTime());
            if (DateTime.Now - lastLockout > lockoutLength)
            {
                if (wasLockedOut)
                {
                    attempts = 0;
                    wasLockedOut = false;
                }
                return false;
            }

            wasLockedOut = true;
            return true;
        }
    }
}