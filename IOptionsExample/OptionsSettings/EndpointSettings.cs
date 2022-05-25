namespace IOptionsExample.OptionsSettings;

public record EndpointSettings
{
    public const string SectionName = nameof(EndpointSettings);

    public string Name { get; init; } = null!;
    public Uri Url { get; init; } = null!;
}