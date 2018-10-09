using AppNotificationCenter.Database.Data;
using AppNotificationCenter.Database.Models;
using AppNotificationCenter.Models;
using AppNotificationCenter.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppNotificationCenter.ModelViews
{
    public class LoginModelView : INotifyPropertyChanged
    {
        private Utente user = new Utente();
        private List<Organizzazione> organizzazionePicker = new List<Organizzazione>();
        private ImageSource _logoOrganizzazione;

        public ImageSource LogoOrganizzazione
        {
            get { return _logoOrganizzazione; }
            set
            {
                _logoOrganizzazione = value;
                OnPropertyChanged();
            }
        }
        public List<Organizzazione> _organizzazionePicker
        {
            get
            {
                return organizzazionePicker;
            }
            set
            {
                organizzazionePicker = value;
                OnPropertyChanged();
            }
        }

        public string _nomeUtente
        {
            get
            {
                return user.username;
            }
            set
            {
                user.username = value;
                OnPropertyChanged();
            }
        }

        public string _password
        {
            get
            {
                return user.password;
            }
            set
            {
                user.password = value;
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

        public string organizzazione
        {
            get
            {
                return user.organizzazione;
            }
            set
            {
                user.organizzazione = value;
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
            _nomeUtente = "";
            _password = "";
            token = "";
            organizzazione = "";
            LogoOrganizzazione = "logoOrdineMedici.png";
            organizzazioniDisponibli();
        }

        public void organizzazioneScelta(Organizzazione OrganizzazioneSelezionata)
        {
            user.organizzazione = OrganizzazioneSelezionata.cod_org;
            LogoOrganizzazione= Xamarin.Forms.ImageSource.FromStream( () => new MemoryStream(Convert.FromBase64String(OrganizzazioneSelezionata.logo)));
        }

        public async Task<bool> login()
        {
            token = App.Current.Properties["token"].ToString();
            REST<Utente, Final> rest = new REST<Utente, Final>();
            var response = await rest.PostJson(URL.Login, user);
            if (response.status)
            {
                await App.Current.MainPage.DisplayAlert("Login", "Login Effettuata con successo", "OK");
                LoginData.InsertUser(new TbLogin(user.username, user.password, user.token, user.organizzazione));
                UtenzaData.InsertUser(new TbUtente(response.final[0]));
                return true;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Login", "Login Fallita", "OK");
                return false;
            }
        }

        public async Task<bool> organizzazioniDisponibli()
        {
            REST<string, Organizzazione> rest = new REST<string, Organizzazione>();
            List<Organizzazione> response = await rest.getJsonList(URL.listaOrganizzazione);
            if (response.Count > 0)
            {
                _organizzazionePicker = response;
                _nomeUtente = _organizzazionePicker[0].cod_org;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
