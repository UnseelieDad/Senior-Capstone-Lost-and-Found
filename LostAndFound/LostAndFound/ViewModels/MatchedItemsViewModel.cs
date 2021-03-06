﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using LostAndFound.Models;
using LostAndFound.Views;
using LostAndFound.Services;

namespace LostAndFound.ViewModels
{
    public class MatchedItemsViewModel : BaseViewModel
    {
        public ObservableCollection<MatchedItem> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public MatchedItemsViewModel()
        {
            Items = new ObservableCollection<MatchedItem>();
            LoadItemsCommand = new Command(async (object callback) => await ExecuteLoadItemsCommand(callback as Action));

            /*MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });*/
        }

        async Task ExecuteLoadItemsCommand(Action callback)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await Backend.GetMatchedItems();
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        Items.Add(item);
                    }
                }

                callback?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}