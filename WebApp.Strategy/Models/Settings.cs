namespace WebApp.Strategy.Models
{
    public class Settings
    {
        public static string ClaimDatabaseType = "databasetype";
        public DatabaseTypes DatabaseType { get; set; }

        public DatabaseTypes GetDefaultDatabaseType => DatabaseTypes.SqlServer;
    }
}
