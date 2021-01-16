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

        /// <summary>
        /// Path to semantics sync script
        /// </summary>
        public static string SemanticsScriptPath { get; set; }

        /// <summary>
        /// Path to Google Scholar sync script
        /// </summary>
        public static string ScholarScriptPath { get; set; }

        /// <summary>
        /// Folder containing API outputs
        /// </summary>
        public static string OutputFolder { get; set; }
    }
}
