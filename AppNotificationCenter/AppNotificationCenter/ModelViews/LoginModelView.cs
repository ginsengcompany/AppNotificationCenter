using AppNotificationCenter.Database.Data;
using AppNotificationCenter.Database.Models;
using AppNotificationCenter.Models;
using AppNotificationCenter.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AppNotificationCenter.ModelViews
{
    public class LoginModelView : INotifyPropertyChanged
    {
        private Utente user = new Utente();
        private List<Organizzazione> organizzazionePicker = new List<Organizzazione>();

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
                return user.nomeUtente;
            }
            set
            {
                user.nomeUtente = value;
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
            organizzazioniDisponibli();
        }

        public void organizzazioneScelta(Organizzazione OrganizzazioneSelezionata)
        {
            user.organizzazione = OrganizzazioneSelezionata.cod_org;
        }

        public async Task<bool> login()
        {
            token = App.Current.Properties["token"].ToString();
            REST<Utente, bool> rest = new REST<Utente, bool>();
            bool response = await rest.PostJson(URL.Login, user);
            if (response)
            {
                await App.Current.MainPage.DisplayAlert("Login", "Login Effettuata con successo", "OK");
                LoginData.InsertUser(new TbLogin(user.nomeUtente, user.password, user.token, user.organizzazione));
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
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
