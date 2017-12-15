using AppOrdineMediciCaserta.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppOrdineMediciCaserta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        private LoginModelView z;

        public Login()
        {
            InitializeComponent();
            z = new LoginModelView();
            BindingContext = z;

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Load.IsVisible = true;
            Load.IsEnabled = true;
            await Load.ProgressTo(1, 1500, Easing.SinIn);
            bool access = await z.login();
            if (access == true)
            {
                if (Platform.Equals(Device.iOS))
                {
                    App.Current.MainPage = new NavigationPage(new ListaEventiIoS());
                }
                else
                    App.Current.MainPage = new NavigationPage(new MainPage());

            }
                
        }

    }
}