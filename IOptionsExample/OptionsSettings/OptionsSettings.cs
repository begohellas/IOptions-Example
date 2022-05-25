namespace IOptionsExample.OptionsSettings;

public record OptionsSettings
{
    public const string SectionName = nameof(OptionsSettings);

    public bool InMemory { get; init; }
    public string HeaderText { get; init; } = null!;
    public int Timeout { get; init; }
}