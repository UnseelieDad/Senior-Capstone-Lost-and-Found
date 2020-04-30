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
    public partial class ContactUsPage : ContentPage
    {
        public ContactUsPage()
        {
            InitializeComponent();
            TeamMemberTitleLabel.Text = "Team Members: \n Chris Damare \n Nick Jones \n Seth Martin \n Kaleb Rhody \n Daniel Valcho";
        }
    }
}