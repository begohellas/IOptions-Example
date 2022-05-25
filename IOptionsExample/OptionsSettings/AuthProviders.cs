namespace IOptionsExample.OptionsSettings;

public record AuthProviders
{
    public const string SectionName = nameof(AuthProviders);

    public string ClientId { get; init; } = null!;
    public string ClientSecret { get; init; } = null!;
}