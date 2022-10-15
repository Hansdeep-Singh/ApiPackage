namespace ApiWeb.Service.EnvironmentService
{
    public interface IEnvironmentService
    {
        string GetConfigurationValue(string Key);
        bool IsDevelopment { get; set; }
    }
}