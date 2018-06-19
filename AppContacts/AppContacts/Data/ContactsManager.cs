using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AppContacts.Helper;
using AppContacts.Model;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
namespace AppContacts.Data
{
    public class ContactsManager
    {
		static ContactsManager defaultInstance = new ContactsManager();
		private IMobileServiceClient client;
		private IMobileServiceSyncTable<Contact> contactTable;
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
			var store = new MobileServiceSQLiteStore("contact2.db");
			store.DefineTable<Contact>();
			this.client.SyncContext.InitializeAsync(store);
			contactTable = client.GetSyncTable<Contact>();
            
        }

		public async Task<ObservableCollection<Contact>> GetItemsAsync(bool syncItems = false)
        {
            try
			{
				if (syncItems)
				{
					await SyncAsync();
				}
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
			 GetItemsGroupedAsync(bool syncItems = false)
		{
			try
			{
				if (syncItems)
				{
					//Si estamos conectados a internet se ejecuta este método
					await SyncAsync();
				}
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
                   
		public async  Task SyncAsync()
		{
			//Lista que permite alamcenar errores
			ReadOnlyCollection<MobileServiceTableOperationError> syncErrores = null;

			try
			{
				//Se manda tod al información disponible en la bd local
				//Que no este sincronizada con el backend
				await this.client.SyncContext.PushAsync();
                //idQuery es una cadena que identifica exclusivamente 
                //la consulta y se utiliza para realizar un seguimiento
                //del estado de su sincornizacion
				//this.contactTable.CreateQuery() nos va a devolver toda
                //La infromación que se encuentre en el backend para tener
                //Toda la info sincronizada en nuestro dispositivo
				await this.contactTable.PullAsync("idQuery",
				                                  this.contactTable.CreateQuery());
			}
			catch (MobileServicePushFailedException ex)
			{
				if (ex.PushResult != null)
				{
					syncErrores = ex.PushResult.Errors;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			if (syncErrores!= null)
			{
				foreach (var item in syncErrores)
				{
                    //Preguntamos si el tipo del error es un update
					if (item.OperationKind == MobileServiceTableOperationKind.Update)
					{
						//Lleva a cabo una operación de la cancelación de la tabla
						//Y actualizar la instancia local del item con el item suministrado
						await item.CancelAndUpdateItemAsync(item.Result);
                      
					}
					else
					{
						//Cancela y descarta la sincornización de los elementos
						await item.CancelAndDiscardItemAsync();
					}
				}
			}
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
        
		public async Task DeletetemAsync(Contact item)
        {
            try
            {
				await contactTable.DeleteAsync(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción: {ex.Message}");
            }
        }


	}
}
