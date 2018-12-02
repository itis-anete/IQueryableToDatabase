using System;

namespace Tests
{
    public static class AppSettings
    {
        private static string azureLogin;
        private static string azurePassword;

        public static string LocalConnectionString =
            "Server=(localdb)\\MSSQLLocalDB;" +
            "Database=QueryProviderDb;" +
            "Trusted_Connection=True;";

        public static string AzureConnectionString
        {
            get
            {
                if (azureLogin == null) RequestLoginPassword();
                return 
                    "Server=tcp:zadykian.database.windows.net,1433;" +
                    "Initial Catalog=QueryProviderDb;" +
                    "Persist Security Info=False;" +
                    $"User ID={azureLogin};" +
                    $"Password={azurePassword};" +
                    "MultipleActiveResultSets=False;" +
                    "Encrypt=True;" +
                    "TrustServerCertificate=False;" +
                    "Connection Timeout=30;";
            }
        }

        private static void RequestLoginPassword()
        {
            Console.Write("Enter Azure Login: ");
            azureLogin = Console.ReadLine();
            Console.Write("Enter Password: ");
            var fontColor = Console.ForegroundColor;
            Console.ForegroundColor = Console.BackgroundColor;
            azurePassword = Console.ReadLine();
            Console.ForegroundColor = fontColor;
        }
    }
}