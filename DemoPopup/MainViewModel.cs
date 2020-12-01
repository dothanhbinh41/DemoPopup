using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace DemoPopup
{
    public class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Item : ModelBase
    {
        public string Text { get; set; }
        public bool IsSelected { get; set; }
    }

    public interface IPopupService
    {
        Task<Item> ChooseItemAsync(List<Item> items, Item df = null);
    }

    public class PopupService : IPopupService
    {
        TaskCompletionSource<Item> source;
        public Task<Item> ChooseItemAsync(List<Item> items, Item df = null)
        { 
                source = new TaskCompletionSource<Item>();
            
            DemoPopup page = new DemoPopup();
            page.SetData(items, df);
            page.Selected += Page_Selected;
            PopupNavigation.Instance.PushAsync(page);
            return source.Task;
        }

        private void Page_Selected(object sender, Item e)
        {
            PopupNavigation.Instance.PopAsync(true);
            source?.TrySetResult(e);
        }
    }


    public class MainViewModel : ModelBase
    {
        IPopupService popupService;
        public MainViewModel()
        {
            MockData();
            popupService = new PopupService();
        }

        void MockData()
        {
            items = new List<Item>();
            for (int i = 0; i < 1000; i++)
            {
                items.Add(new Item { Text = $"Item num {i + 1}" });
            }

            SelectedItem = items.FirstOrDefault();
        }

        List<Item> items;
        public Item SelectedItem { get; set; }


        ICommand _ClickCommand;
        public ICommand ClickCommand => _ClickCommand = _ClickCommand ?? new Command(ExcuteClickCommand);
        async void ExcuteClickCommand()
        {
            // call popup service if use prism
            SelectedItem = await popupService.ChooseItemAsync(items, SelectedItem);
        } 
    }
}
