﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotificationCenter.Models
{
    public class Utente
    {
        public string matricola { get; set; }
        public string organizzazione { get; set; }
        public string token { get; set; }
        public string eliminato { get; set; }
    }
}