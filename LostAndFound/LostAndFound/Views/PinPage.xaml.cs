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
            PinEntry.Focus();
        }

        private async void Entry_Completed(object sender, EventArgs e)
        {
            var encrypted = HashUtilities.GetHashFromString(PinEntry.Text);
            var response = await Backend.AdminLogin(encrypted);
            if (!response.Body.Equals("Incorrect PIN provided"))
            {
                await RootPage.NavigateFromMenu((int)MenuItemType.Admin);
            }
            PinEntry.Text = "";
        }
    }
}