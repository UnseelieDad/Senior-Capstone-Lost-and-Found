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
    public partial class AdminPage : ContentPage
    {
        public AdminPage()
        {
            InitializeComponent();
            AdminAreaLabel.Text = "Welcome to the Lost and Found Admin Area. \n \n Here You Can View: Currently Lost Items and Potentially Matched Items. \n \n The system automatically emails the owner with pickup information when item matches are confirmed.";
        }

        private async void LostButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ItemsPage());
        }

        private async void MatchedButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MatchedItemsPage());
        }
    }
}