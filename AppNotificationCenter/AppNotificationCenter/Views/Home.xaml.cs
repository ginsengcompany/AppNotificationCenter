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
    public partial class Home : ContentPage
    {
        public Home()
        {
            InitializeComponent();
            Logo();
         
        }
        
        public async void Logo()
        {
            caricamento.IsRunning = true;
            Device.StartTimer(TimeSpan.FromSeconds(3), () =>
            {
                App.Current.MainPage = new NavigationPage(new MainPage());
                return false;
            });
           
        }
    }
}