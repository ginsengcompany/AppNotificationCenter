using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppNotificationCenter.Database.Data;
using AppNotificationCenter.Database.Models;
using AppNotificationCenter.Models;
using AppNotificationCenter.Services;
using Xamarin.Forms;

namespace AppNotificationCenter.ModelViews
{
    public class ProfiloModelView:INotifyPropertyChanged
    {
        private TbUtente utenteProfilo;
        private bool isEnabled=false;
        private bool isEnabledModifica = true;
        private string helper="";
        private Color coloreModifica = Color.Default;


        public string Helper
        {
            get { return helper; }
            set
            {
                OnPropertychanged();
                helper = value;
            }
        }
        public Color ColoreModifica
        {
            get { return coloreModifica; }
            set
            {
                OnPropertychanged();
                coloreModifica = value;
            }
        }

        public bool IsEnabledModifica
        {
            get { return isEnabledModifica; }
            set
            {
                OnPropertychanged();
                isEnabledModifica = value;
            }
        }
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                OnPropertychanged();
                isEnabled = value;
            }
        }

        public TbUtente UtenteProfilo
        {
            get { return utenteProfilo; }
            set
            {
                OnPropertychanged();
                utenteProfilo = value;
            }
        }
    
        public ProfiloModelView()
        {
            IsEnabled = false;
            isEnabledModifica = true;
            UtenteProfilo = UtenzaData.getUser();
        }
        public ICommand ModificaInfo
        {
            get
            {
                return new Command(() =>
                {
                    IsEnabled = true;
                    Helper = "Questo campo è modificabile";
                    isEnabledModifica = false;
                });
            }
        }
        public ICommand btnAnnulla
        {
            get
            {
                return new Command(() =>
                {
                    IsEnabled = false;
                    isEnabledModifica = true;
                    UtenteProfilo = UtenzaData.getUser();
                });
            }
        }
        public ICommand btnConferma
        {
            get
            {
                return new Command(async () =>
                {
                    IsEnabled = false;
                    isEnabledModifica = true;
                    REST<TbUtente,Response> connessioneModifica = new REST<TbUtente, Response>();
                    var response = await connessioneModifica.PostJson(URL.modificaContatto, UtenteProfilo);
                    if (response.status)
                    {
                        UtenzaData.UpdateUser(UtenteProfilo);
                    }
                    else
                    {
                       await App.Current.MainPage.DisplayAlert("Attenzione", "aggiornamento dati non riuscito", "ok");
                        UtenteProfilo = UtenzaData.getUser();
                    }
                });
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertychanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class Response
    {
        public bool status { get; set; }
    }

}
