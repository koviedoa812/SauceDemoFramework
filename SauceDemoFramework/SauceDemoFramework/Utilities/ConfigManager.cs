using Microsoft.Extensions.Configuration;

namespace SauceDemoFramework.Utilities
{
    public static class ConfigManager
    {
        private static readonly IConfiguration _config;

        static ConfigManager()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public static string BaseUrl => _config["Settings:BaseUrl"]!;
        public static string Browser => _config["Settings:Browser"]!;
        public static int ImplicitWait => int.Parse(_config["Settings:ImplicitWait"]!);
        public static int ExplicitWait => int.Parse(_config["Settings:ExplicitWait"]!);
        public static string ValidUsername => _config["Credentials:ValidUsername"]!;
        public static string ValidPassword => _config["Credentials:ValidPassword"]!;
        public static string InvalidUsername => _config["Credentials:InvalidUsername"]!;
        public static string InvalidPassword => _config["Credentials:InvalidPassword"]!;
    }
}