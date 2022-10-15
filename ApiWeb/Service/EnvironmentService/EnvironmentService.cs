namespace ApiWeb.Service.EnvironmentService
{
    public class EnvironmentService : IEnvironmentService
    {
        public string GetConfigurationValue(string Key)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Production.json")
                .Build();
            return configuration.GetSection(Key).Value;
        }
        public bool IsDevelopment { get; set; }

    }
}
