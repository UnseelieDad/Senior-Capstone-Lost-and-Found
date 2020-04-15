using LostAndFound.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LostAndFound.ViewModels
{
    public class MatchedItemDetailViewModel : BaseViewModel
    {
        public MatchedItem Item { get; set; }
        public MatchedItemDetailViewModel(MatchedItem item = null)
        {
            Title = "Match";
            Item = item;
        }
    }
}
