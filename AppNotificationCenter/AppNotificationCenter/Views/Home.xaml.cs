using AppNotificationCenter.Database.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppNotificationCenter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        public Home()
        {
            InitializeComponent();
            Logo();


        }

        public async void Logo()
        {
            var utenza = LoginData.getUserAttivo();
            Immagine.Source = await reinderizza(utenza[0].splash_logo);
            caricamento.IsRunning = true;
            Device.StartTimer(TimeSpan.FromSeconds(3), () =>
            {
                App.Current.MainPage = new NavigationPage(new MainPage());
                return false;
            });
        }

        public async Task<ImageSource>reinderizza(string imgsrc)
        {
            var img = ImageSource.FromStream(
               () => new MemoryStream(Convert.FromBase64String(imgsrc)));
            return img;
        }
    }
}