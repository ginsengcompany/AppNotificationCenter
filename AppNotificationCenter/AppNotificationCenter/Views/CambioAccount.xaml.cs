using AppNotificationCenter.Database.Data;
using AppNotificationCenter.Database.Models;
using AppNotificationCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppNotificationCenter.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CambioAccount : ContentPage
	{
		public CambioAccount ()
		{
			InitializeComponent ();
            ListaUtenze.ItemsSource= LoginData.getUser();
        }

        private void ListaUtenze_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            TbLogin i = new TbLogin();
            i = e.Item as TbLogin;
            var utenza = LoginData.getUser();
            foreach (var z in utenza)
            {
                z.attivo = false;
                LoginData.updateUser(z);
            }
            i.attivo = true;
            LoginData.updateUser(i);
            App.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}