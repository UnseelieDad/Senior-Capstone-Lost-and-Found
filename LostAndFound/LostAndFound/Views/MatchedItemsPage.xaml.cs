using LostAndFound.Models;
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
    public partial class MatchedItemsPage : ContentPage
    {
        MatchedItemsViewModel viewModel;

        public MatchedItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new MatchedItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as MatchedItem;
            if (item == null)
                return;

            await Navigation.PushAsync(new MatchedItemDetailPage(new MatchedItemDetailViewModel(item)));

            // Manually deselect item.
            MatchedItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute((Action)(() =>
                {
                    if (viewModel.Items.Count == 0)
                    {
                        MatchedItemsListView.IsVisible = false;
                        NoLostItemsLabel.IsVisible = true;
                    }
                }));

            //Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            //{
            //    if (viewModel.Items.Count == 0)
            //    {
            //        MatchedItemsListView.IsVisible = false;
            //        NoLostItemsLabel.IsVisible = true;
            //    }

            //    return false;
            //});
        }
    }
}