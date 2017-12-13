using AppOrdineMediciCaserta.Database.Data;
using AppOrdineMediciCaserta.Database.Models;
using AppOrdineMediciCaserta.Models;
using AppOrdineMediciCaserta.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Com.OneSignal;

namespace AppOrdineMediciCaserta.ModelViews
{
    public class LoginModelView : INotifyPropertyChanged
    {
        private Medico user = new Medico();
        public string matricola
        {
            get
            {
                return user.matricola;
            }
            set
            {
                user.matricola = value;
                OnPropertyChanged();
            }
        }

        public string token
        {
            get
            {
                return user.token;
            }
            set
            {
                user.token = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public LoginModelView()
        {
            matricola = "";
            token = "";
        }

        public async Task<bool> login()
        {
            OneSignal.Current.IdsAvailable(((string userID, string pushToken) =>
            {
                user.token = userID;
            }));
            REST<Medico, bool> rest = new REST<Medico, bool>();
            bool response = await rest.PostJson(URL.Login, user);
            if (response)
            {
                await App.Current.MainPage.DisplayAlert("Login", "Login Effettuata con successo", "OK");
                await LoginData.InsertUser(new TbLogin(user.matricola, user.token));
                return true;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Login", "Login Fallita", "OK");
                return false;
            }
               
        }
    }
}
