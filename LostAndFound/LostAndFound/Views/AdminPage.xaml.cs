﻿using System;
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