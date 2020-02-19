using System;
using System.Collections.Generic;
using System.Text;

namespace LostAndFound.Models
{
    public enum MenuItemType
    {
        Browse,
        About,
        Pin,
        Admin
    }

    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
