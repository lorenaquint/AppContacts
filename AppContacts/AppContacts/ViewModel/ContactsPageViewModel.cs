using AppContacts.Data;
using AppContacts.Helper;
using AppContacts.Model;
using AppContacts.View;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppContacts.ViewModel
{
    public  class ContactsPageViewModel
    {
        public ObservableCollection<Grouping<string, Contact>>
            ContactsList
        { get; set; }

        public Contact CurrentContact { get; set; }
        public Command AddContactCommand { get; set; }
        public Command ItemTappedCommand { get; }
        public INavigation Navigation { get; set; }

        public ContactsPageViewModel(INavigation navigation)
        {
			Navigation = navigation;
            //Indica si esta conectado
			var isConnected = CrossConnectivity.Current.IsConnected;
            Task.Run(async () =>
			         ContactsList = await ContactsManager.DefaultInstance.GetItemsGroupedAsync(isConnected)).Wait();
            AddContactCommand = new Command(async () => await
            GoToContactDetailPage());
            ItemTappedCommand = new Command(async () => GoToContactDetailPage(CurrentContact));
        }

        public async Task GoToContactDetailPage(Contact contact = null)
        {
            if (contact == null)
            {
                await Navigation.PushAsync(new ContactDetailPage());
            }
            else
            {
                await Navigation.PushAsync(new ContactDetailPage(CurrentContact));
            }
        }
    }
}

