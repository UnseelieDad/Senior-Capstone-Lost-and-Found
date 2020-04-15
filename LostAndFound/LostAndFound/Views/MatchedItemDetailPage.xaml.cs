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

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}