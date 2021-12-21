using System.Text;

namespace VialApp.Tools
{
    public static class Configuration
    {
        private static IConfigurationRoot? _main = null;
        public static void Load()
        {
            if (_main == null)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                _main = builder.Build();
            }
        }

        public static string GetConnectionString(string ConnectionId)
        {
            Load();
            return _main.GetConnectionString(ConnectionId);
        }

        public static byte[] GetSalt()
        {
            Load();
            return Encoding.ASCII.GetBytes(_main.GetSection("Salt").Value);
        }
    }
}
