using System;

using LostAndFound.Models;

namespace LostAndFound.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Type;
            Item = item;
        }
    }
}
