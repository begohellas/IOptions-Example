namespace IOptionsExample.OptionsSettings;

public record ValidateSettings
{
    public const string SectionName = nameof(ValidateSettings);

    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}