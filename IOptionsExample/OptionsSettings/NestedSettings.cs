namespace IOptionsExample.OptionsSettings;

public record NestedSettings
{
    public const string SectionName = nameof(NestedSettings);

    public string KeyOne { get; init; } = null!;

    public NestedKey KeyTwo { get; init; } = null!;
}

public record NestedKey
{
    public string Message { get; set; } = null!;
}