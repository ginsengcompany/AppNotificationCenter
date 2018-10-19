namespace AppNotificationCenter.Services
{
    public static class URL
    {
        public const string urlRemoto = "https://notification-center.ak12srl.it/";
        public const string urlLocale = "http://192.168.125.14:3004";
        public static string Eventi = urlRemoto + "/getEventiByID";

        public static string Login = urlRemoto + "/cercaUsername";

        public static string ConfermaElimina =urlRemoto + "/setEliminatoConfermato";
        public static string listaOrganizzazione = urlRemoto + "/getListaOrganizzazione";
        public static string modificaContatto = urlRemoto + "/modificaContatto";
    }
}
