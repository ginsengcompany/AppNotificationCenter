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
using Xamarin.Forms;

namespace AppNotificationCenter.ModelViews
{
    public class ProfiloModelView:INotifyPropertyChanged
    {
        private TbUtente utenteProfilo;
        private bool isEnabled=false;
        private bool isEnabledModifica = true;
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
                return new Command(() =>
                {
                    IsEnabled = false;
                    isEnabledModifica = true;
                });
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertychanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
