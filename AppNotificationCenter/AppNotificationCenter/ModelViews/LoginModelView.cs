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
            var utenza = LoginData.getUser();
            if(utenza.Count > 0)
            {
                if (string.IsNullOrEmpty(utenza[0].splash_logo))
                {
                    LoginData.dropUser(utenza[0]);
                    //App.Current.MainPage.DisplayAlert("ATTENZIONE", "Rieffettuare la login per adeguarsi al nuovo aggiornamento", "OK");
                }
            }
           organizzazioniDisponibli();
        }

  
        public void organizzazioneScelta()
        {
            char[] delimiterChars = {'-', '\t' };
            var temp = user.username.Split(delimiterChars);
            user.organizzazione = temp[0].ToString().ToUpper();
            foreach(var i in _organizzazionePicker)
            {
                if(user.organizzazione==i.cod_org)
                {
                    user.circle_logo = i.circle_logo;
                    user.splash_logo = i.splash_logo;
                }
            }
            
        }
        public async Task<bool> login()
        {
            token = App.Current.Properties["token"].ToString();
            REST<Utente, Final> rest = new REST<Utente, Final>();
            string usernameUp = user.username.ToUpper();
            user.username = usernameUp.Trim();
            var response = await rest.PostJson(URL.Login, user);
            if (response != null)
            {
                if (response.status)
                {
                    if (response.final[0].attivo == false)
                    {

                        await App.Current.MainPage.DisplayAlert("Login", "Utenza non attiva", "OK");
                        return false;
                    }
                    else
                    {
                        //await App.Current.MainPage.DisplayAlert("Login", "Login Effettuata con successo", "OK");
                        response.final[0].organizzazione = user.organizzazione;
                        var medico = LoginData.getUser();
                        bool flag = true;
                        for (int i = 0; i < medico.Count; i++)
                        {
                            if (user.username == medico[i].username)
                            {
                                await App.Current.MainPage.DisplayAlert("Attenzione", "Utenza per la quale prova ad accedere è già collegata", "ok");
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            foreach (var i in medico)
                            {
                                i.attivo = false;
                                LoginData.updateUser(i);
                            }
                            LoginData.InsertUser(new TbLogin(user.username.ToUpper(), user.password, user.token, user.organizzazione,user.circle_logo,user.splash_logo,true));
                            UtenzaData.InsertUser(new TbUtente(response.final[0]));

                        }
                        return flag;
                        
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Login", "Login Fallita", "OK");
                    return false;
                }

            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Login", "Connessione assente", "OK");
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
