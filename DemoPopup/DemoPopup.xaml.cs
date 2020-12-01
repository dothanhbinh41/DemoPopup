using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace DemoPopup
{
    public partial class DemoPopup :  PopupPage
    {
        public event EventHandler<Item> Selected;
        public DemoPopup()
        {
            InitializeComponent(); 
        }

        void LstItem_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem is Item item)
            { 
                Selected?.Invoke(this, item);
            }
        }

        public void SetData(List<Item> lst, Item df)
        {
            lstItem.ItemsSource = lst;
            lstItem.SelectedItem = df;
        }
    }
}