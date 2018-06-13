using AppContacts.Data;
using AppContacts.Services;
using AppContacts.View;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace AppContacts
{
	public partial class App : Application
	{
		
        //private static ContactsDatabase database;

        //public static ContactsDatabase Database
        //{
        //    get
        //    {
        //        if (database == null)
        //        {
        //            try
        //            {
        //                database =
        //                    new ContactsDatabase(DependencyService
        //                        .Get<IFileHelper>()
        //                        .GetLocalFilePath("contactsdb.db3"));
        //            }
        //            catch (Exception ex)
        //            {
        //                Debug.WriteLine(ex.Message);
        //            }
        //        }
        //        return database;
        //    }
        //}

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new ContactsPage());
        }
        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
