using Acr.UserDialogs;
using LostAndFound.Services;
using LostAndFound.ViewModels;
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
    public partial class MatchedItemDetailPage : ContentPage
    {
        MatchedItemDetailViewModel viewModel;

        public MatchedItemDetailPage(MatchedItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public MatchedItemDetailPage()
        {
            InitializeComponent();

            viewModel = new MatchedItemDetailViewModel(viewModel.Item);
            BindingContext = viewModel;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading();
            await Backend.ConfirmMatch(viewModel.Item);
            await Backend.SendEmailNotification(viewModel.Item);
            UserDialogs.Instance.HideLoading();

            await DisplayAlert("Match confirmed", "The match has been confirmed and the owner of the item has been notified.", "Ok");

            for (var counter = 1; counter < 2; counter++)
            {
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            }
            await Navigation.PopAsync();
        }
    }
}