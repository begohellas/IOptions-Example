namespace IOptionsExample.Extensions;

public static class ConfigurationManagerExtensions
{
    public static IConfigurationBuilder AddConfigurationJsonSettings(
        this ConfigurationManager configurationManager,
        string path,
        string filename = "appsettings.json"
        )
    {
        ArgumentNullException.ThrowIfNull(path);

        if (!filename.EndsWith("json", StringComparison.CurrentCultureIgnoreCase))
        {
            throw new ArgumentException("filename type must be json", nameof(filename));
        }

        return configurationManager.SetBasePath(path)
                                    .AddJsonFile(filename, false, true);
    }
}