using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AppContacts.Helper;
using AppContacts.Model;
using Microsoft.WindowsAzure.MobileServices;
namespace AppContacts.Data
{
    public class ContactsManager
    {
		static ContactsManager defaultInstance = new ContactsManager();
		private IMobileServiceClient client;
		private IMobileServiceTable<Contact> contactTable;
		public static ContactsManager DefaultInstance
		{
			get
			{
				return defaultInstance;
			}
			set
			{
				defaultInstance = value;
			}
		}
		private ContactsManager()
        {
			client = new MobileServiceClient("https://mobapplon.azurewebsites.net");
			contactTable = client.GetTable<Contact>();
        }

		public async Task<ObservableCollection<Contact>> GetItemsAsync()
        {
            try
			{
				IEnumerable<Contact> items =
					await contactTable.ToEnumerableAsync();
				return new ObservableCollection<Contact>(items);

			} 
			catch (MobileServiceInvalidOperationException mobException) 

			{
				Debug.WriteLine($"Excepción: {mobException.Message}");
            }
			catch (Exception ex)

            {
                Debug.WriteLine($"Excepción: {ex.Message}");
            }
			return null;
        }
		public async Task<ObservableCollection<Grouping<string, Contact>>>
             GetItemsGroupedAsync()
        {
			IEnumerable<Contact> contacts =
				await GetItemsAsync();
            IEnumerable<Grouping<string, Contact>> sorted =
                new Grouping<string, Contact>[0];
            if (contacts != null)
            {
                sorted =
                    from c in contacts
                    orderby c.Name
                    group c by c.Name[0].ToString()
                    into theGroup
                    select new Grouping<string, Contact>
                        (theGroup.Key, theGroup);
            }
            return new ObservableCollection<Grouping<string, Contact>>(sorted);
        }
        
		public async Task<Contact> GetItemAsync(string id)
        {
			var items = await contactTable
							  .Where(i => i.Id == id)
				              .ToListAsync();
			return items.FirstOrDefault();
        }

		public async Task SaveItemAsync(Contact item)
        {
            try
			{
				if (item.Id != null)
                {
					await contactTable.UpdateAsync(item);
                }
                else
                {
					await contactTable.InsertAsync(item);
                    
                }
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Excepción: {ex.Message}");
			}
		}


	}
}
