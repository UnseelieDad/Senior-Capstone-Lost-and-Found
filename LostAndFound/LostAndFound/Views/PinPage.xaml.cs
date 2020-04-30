using LostAndFound.Models;
using LostAndFound.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LostAndFound.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        public PinPage()
        {
            InitializeComponent();
        }

        private async void Entry_Completed(object sender, EventArgs e)
        {
            if (sender is Entry entry)
            {
                var encrypted = HashUtilities.GetHashFromString(entry.Text);
                var response = await Backend.AdminLogin(encrypted);
                if (response != null)
                {
                    if (!response.Body.Equals("Incorrect PIN provided"))
                    {
                        await RootPage.NavigateFromMenu((int)MenuItemType.Admin);
                    }
                    else
                    {
                        await DisplayAlert("Error", response.Body, "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Unable to connect.", "OK");
                }
            }
        }
    }
}