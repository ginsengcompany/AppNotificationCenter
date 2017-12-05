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
            bool access = await z.login();
            if (access == true)
                App.Current.MainPage = new NavigationPage(new MainPage());
        }

    }
}