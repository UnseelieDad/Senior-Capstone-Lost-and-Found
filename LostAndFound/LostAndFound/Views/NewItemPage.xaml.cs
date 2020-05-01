using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LostAndFound.Models;
using LostAndFound.Services;
using Acr.UserDialogs;

namespace LostAndFound.Views
{
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            Item = new Item { };
            Item.DateLost = DateTime.Today;
            LostDatePicker.MaximumDate = DateTime.Today;
            BindingContext = this;
            ColorPickerSetup();
            TypePickerSetup();
            LocationPickerSetup();
        }

        private void LocationPickerSetup()
        {
            List<string> LocationList = new List<string>
            {
                "Student Center",
                "Tolliver",
                "Book Store",
                "Wyly",
                "Library",
                "Quad",
                "Howard",
                "Keeny",
                "University",
                "Bogard",
                "COB",
                "IESB",
                "Nethken",
                "Carson Taylor",
                "Robinson",
                "Hale",
                "Woodard",
                "GTM",
                "Davison",
                "Legacy Park Apartments",
                "Aswell Suites",
                "Adams Suites",
                "Adams Hall",
                "Mitchell Hall",
                "Dudley Hall",
                "Lambright Apartment",
                "Baseball Field Apartments",
                "Downtown Apartments"
            };


            foreach (string itemLocation in LocationList)
            {
                LocationPicker.Items.Add(itemLocation);
            }

            LocationPicker.SelectedIndexChanged += (sender, args) =>
            {
                string locationName = LocationPicker.Items[LocationPicker.SelectedIndex];
                Item.Location = locationName;
            };
        }

        private void TypePickerSetup()
        {
            List<string> ItemTypeList = new List<string>
            {
                { "Shoes"},

                { "Technology"},
                { "Books"},
                { "Backpack"},
                { "Clothing" },
                { "Personal Items"},
            };

            foreach (string itemType in ItemTypeList)
            {
                TypePicker.Items.Add(itemType);
            }

            TypePicker.SelectedIndexChanged += (sender, args) =>
            {

                string typeName = TypePicker.Items[TypePicker.SelectedIndex];
                Item.Type = typeName;
            };
        }

        private void ColorPickerSetup()
        {
            List<string> nameToColor = new List<string>
            {
                { "Black"},
                { "Blue"},
                { "Gray"},
                { "Green"},
                { "Purple" },
                { "Orange"},
                { "Pink"},
                { "Red"},
                { "Yellow"},
                { "White"}
            };

            foreach (string colorName in nameToColor)
            {
                ColorPicker.Items.Add(colorName);
            }

            ColorPicker.SelectedIndexChanged += (sender, args) =>
            {

                string colorName = ColorPicker.Items[ColorPicker.SelectedIndex];
                Item.Color = colorName;
            };
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            SubmitButton.IsEnabled = false;
            Item.CreateDate = DateTime.Today;
            Item.Status = "Lost";
            if (String.IsNullOrEmpty(Item.LastName) || String.IsNullOrEmpty(Item.Color) || String.IsNullOrEmpty(Item.Description) || String.IsNullOrEmpty(Item.Email) || String.IsNullOrEmpty(Item.FirstName) || String.IsNullOrEmpty(Item.Location) || String.IsNullOrEmpty(Item.Type))
            {
                await DisplayAlert("Empty Fields", "Please fill out all fields for the lost item.", "Okay");
                return;
            }
            //Not needed because it is not being added to a list, will be needed in the admin area
            ///MessagingCenter.Send(this, "AddItem", Item);
            UserDialogs.Instance.ShowLoading();
            await Backend.SubmitLostItem(Item);
            UserDialogs.Instance.HideLoading();
            await DisplayAlert("Item Submitted", "Thank you for submitting a lost item. When your item is found you will recieve an email with more details. \n \n Questions? Please go to Netheken 132.", "Dismiss");
            await Navigation.PopToRootAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}