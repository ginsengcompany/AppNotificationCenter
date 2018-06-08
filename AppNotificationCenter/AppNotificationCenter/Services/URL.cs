namespace AppNotificationCenter.Services
{
    public static class URL
    {
        public const string urlRemoto = "omceoce.ak12srl.it";
        public const string urlLocale = "192.168.125.25:3000";
        public static string Eventi = "http://" + urlRemoto + "/getEventiByID";

        public static string Login = "http://" + urlRemoto + "/cercaUsername";

        public static string ConfermaElimina = "http://" + urlRemoto + "/setEliminatoConfermato";
        public static string listaOrganizzazione = "http://" + urlRemoto + "/getListaOrganizzazione";
    }
}
