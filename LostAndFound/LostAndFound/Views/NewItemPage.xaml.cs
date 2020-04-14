using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LostAndFound.Models;
using LostAndFound.Services;

namespace LostAndFound.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            Item = new Item { };
            Item.DateLost = DateTime.Today;
            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            Item.CreateDate = DateTime.Today;
            Item.Status = "Lost";
            if (String.IsNullOrEmpty(Item.LastName) || String.IsNullOrEmpty(Item.Color) || String.IsNullOrEmpty(Item.Description) || String.IsNullOrEmpty(Item.Email) || String.IsNullOrEmpty(Item.FirstName) || String.IsNullOrEmpty(Item.Location) || String.IsNullOrEmpty(Item.Type))
            {
                await DisplayAlert("Empty Fields", "Please fill out all fields for the lost item.", "Okay");
                return;
            }
            //Not needed because it is not being added to a list, will be needed in the admin area
            ///MessagingCenter.Send(this, "AddItem", Item);
            await Backend.SubmitLostItem(Item);
            await DisplayAlert("Item Submitted", "Thank you for submitting a lost item. When your item is found you will recieve an email with more details. \n \n Questions? Please go to Netheken 132.", "Okay");
            await Navigation.PopToRootAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}