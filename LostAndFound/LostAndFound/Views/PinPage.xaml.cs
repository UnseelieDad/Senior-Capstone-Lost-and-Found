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
        private const string Pin = "8671";

        MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        public PinPage()
        {
            InitializeComponent();
        }

        private async void Entry_Completed(object sender, EventArgs e)
        {
            if (sender is Entry entry)
            {
                if (entry.Text == Pin)
                {
                    var item = new Item()
                    {
                        Color = "blue",
                        Email = "mememcgee@wemail.com",
                        Description = "size 24 timbs",
                        CreateDate = DateTime.Now,
                        Type = "shoe",
                        Location = "basement",
                        DateLost = DateTime.Now,
                        FirstName = "Bob",
                        LastName = "Dylan",
                        Status = "Lost"
                    };
                    await Backend.SubmitLostItem(item);

                    await RootPage.NavigateFromMenu((int)MenuItemType.Admin);
                }
            }
        }
    }
}