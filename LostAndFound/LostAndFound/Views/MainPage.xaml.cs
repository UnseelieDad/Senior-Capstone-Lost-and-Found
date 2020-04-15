using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LostAndFound.Models;
using LostAndFound.Views.UIStandards;

namespace LostAndFound.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        // Test comment 2
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            detailPage.BarBackgroundColor = ColorConstants.TechBlue;

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.Browse, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Browse:
                        MenuPages.Add(id, new NavigationPage(new ItemsPage()) { BarBackgroundColor = ColorConstants.TechBlue });
                        break;
                    case (int)MenuItemType.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()) { BarBackgroundColor = ColorConstants.TechBlue });
                        break;
                    case (int)MenuItemType.Pin:
                        MenuPages.Add(id, new NavigationPage(new PinPage()) { BarBackgroundColor = ColorConstants.TechBlue });
                        break;
                    case (int)MenuItemType.Admin:
                        MenuPages.Add(id, new NavigationPage(new AdminPage()) { BarBackgroundColor = ColorConstants.TechBlue });
                        break;
                    case (int)MenuItemType.ContactUs:
                        MenuPages.Add(id, new NavigationPage(new ContactUsPage()) { BarBackgroundColor = ColorConstants.TechBlue });
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}