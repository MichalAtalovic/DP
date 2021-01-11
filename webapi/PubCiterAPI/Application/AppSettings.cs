namespace PubCiterAPI
{
    public static class AppSettings
    {
        /// <summary>
        /// Connection string
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// Author (The user searching for publications citing their work)
        /// </summary>
        public static string Author { get; set; }
    }
}
