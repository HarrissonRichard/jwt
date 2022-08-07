namespace Tweet.Settings
{
    public class SqlServerSettings
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Trusted_Connection { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        

        public string ConnectionString
        {
            get
            {
                 return $"Server={Server};Database={Database};Trusted_Connection={Trusted_Connection};";
            }
        }
    }
}