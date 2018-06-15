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
		public Command DeleteContactCommand { get; set; }
        public Contact CurrentContact { get; set; }
		public bool IsEnabled { get; set; }
        public INavigation Navigation { get; set; }

        public ContactDetailPageViewModel(INavigation navigation
            , Contact contact = null)
        {
            Navigation = navigation;
            if (contact == null)
            {
                CurrentContact = new Contact();
				IsEnabled = false;
            }
            else
            {
				IsEnabled = true;
                CurrentContact = contact;
            }
            SaveContactCommand = new Command(async () => await SaveContact());
			DeleteContactCommand = new Command(async () => await DeleteContact());
        }

        public async Task SaveContact()
        {
			// await App.Database.SaveItemAsync(CurrentContact);
			await ContactsManager.DefaultInstance.SaveItemAsync(CurrentContact);
            await Navigation.PopToRootAsync();
        }
		private async Task DeleteContact()
        {
			await ContactsManager.DefaultInstance.DeletetemAsync(CurrentContact);
            await Navigation.PopToRootAsync();
        }
    }
}

