using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AppNotificationCenter.Database.Data;
using AppNotificationCenter.Database.Models;
using AppNotificationCenter.Models;

namespace AppNotificationCenter.ModelViews
{
    public class ProfiloModelView:INotifyPropertyChanged
    {
        private TbUtente utenteProfilo;

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
          UtenteProfilo = UtenzaData.getUser();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertychanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
