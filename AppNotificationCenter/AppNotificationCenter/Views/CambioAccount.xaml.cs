using AppNotificationCenter.Database.Data;
using AppNotificationCenter.Database.Models;
using AppNotificationCenter.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            Ricevi();
        }

        public async void Ricevi()
        {

            var lista = LoginData.getUser();
            List<Utente> user = new List<Utente>();
            for(int i=0;i<lista.Count;i++)
            {
                Utente temp = new Utente();
                temp.id = lista[i].id;
                temp.username = lista[i].username;
                temp.password = lista[i].password;
                temp.splash_logo = lista[i].splash_logo;
                temp.circle_logo = lista[i].circle_logo;
                temp.token = lista[i].token;
                temp.organizzazione = lista[i].organizzazione;
                temp.attivo = lista[i].attivo;
                temp.img = await reinderizza(lista[i].circle_logo);
                user.Add(temp);
            }
            ListaUtenze.ItemsSource = user;
        }
        public async Task<ImageSource> reinderizza(string imgsrc)
        {
            var img = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(imgsrc)));
            return img;

        }

        private void ListaUtenze_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            TbLogin temp = new TbLogin();
            var i = e.Item as Utente;
            var utenza = LoginData.getUser();
            foreach (var z in utenza)
            {
                z.attivo = false;
                LoginData.updateUser(z);

            }
            temp.id = i.id;
            temp.username = i.username;
            temp.password = i.password;
            temp.splash_logo = i.splash_logo;
            temp.circle_logo = i.circle_logo;
            temp.token = i.token;
            temp.organizzazione = i.organizzazione;
            temp.attivo = true;
            LoginData.updateUser(temp);
            App.Current.MainPage = new NavigationPage(new MainPage());
        }
    }
}