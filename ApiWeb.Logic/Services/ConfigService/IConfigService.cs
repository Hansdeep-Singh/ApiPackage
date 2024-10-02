namespace Logic.Services.ConfigService
{
    public interface IConfigService
    {
        string GetConfigurationValue(string Key);
        bool IsDevelopment { get; set; }
    }
}