namespace AppNotificationCenter.Services
{
    public static class URL
    {
        public const string urlRemoto = "omceoce.ak12srl.it";
        public const string urlLocale = "192.168.125.12:3000";
        public static string Eventi = "http://" + urlRemoto + "/getEventiByID";

        public static string Login = "http://" + urlRemoto + "/cercaMatricola";

        public static string ConfermaElimina = "http://" + urlRemoto + "/setEliminatoConfermato";
        public static string listaOrganizzazione = "http://" + urlRemoto + "/getListaOrganizzazione";
    }
}
