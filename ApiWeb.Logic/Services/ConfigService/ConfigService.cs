using Microsoft.Extensions.Configuration;

namespace Logic.Services.ConfigService
{
    public class ConfigService : IConfigService
    {
        public string GetConfigurationValue(string Key)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            return configuration?.GetSection(Key)?.Value ?? string.Empty;
        }
        public bool IsDevelopment { get; set; }

    }
}
