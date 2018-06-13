using AppContacts.Data;
using AppContacts.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppContacts.ViewModel
{
   public class ContactDetailPageViewModel
    {
        public Command SaveContactCommand { get; set; }
        public Contact CurrentContact { get; set; }
        public INavigation Navigation { get; set; }

        public ContactDetailPageViewModel(INavigation navigation
            , Contact contact = null)
        {
            Navigation = navigation;
            if (contact == null)
            {
                CurrentContact = new Contact();
            }
            else
            {
                CurrentContact = contact;
            }
            SaveContactCommand = new Command(async () => await SaveContact());
        }

        public async Task SaveContact()
        {
			// await App.Database.SaveItemAsync(CurrentContact);
			await ContactsManager.DefaultInstance.SaveItemAsync(CurrentContact);
            await Navigation.PopToRootAsync();
        }
    }
}

