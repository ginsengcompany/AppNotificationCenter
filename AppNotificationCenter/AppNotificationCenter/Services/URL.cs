namespace AppNotificationCenter.Services
{
    public static class URL
    {
        public const string urlRemoto = "omceoce.ak12srl.it";
        public const string urlLocale = "192.168.125.33:3000";
        public static string Eventi = "http://" + urlLocale + "/getEventiByID";

        public static string Login = "http://" + urlLocale + "/cercaUsername";

        public static string ConfermaElimina = "http://" + urlLocale + "/setEliminatoConfermato";
        public static string listaOrganizzazione = "http://" + urlLocale + "/getListaOrganizzazione";
        public static string modificaContatto = "http://" + urlLocale + "/modificaContatto";
    }
}
