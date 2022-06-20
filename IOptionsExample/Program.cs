var config = new ConfigurationManager();

config.AddConfigurationJsonSettings(Directory.GetCurrentDirectory())
      .AddInMemoryCollection( new Dictionary<string, string>
      {
          {"KeyOne", "ValueOne"},
          {"KeyTwo", "ValueTwo"},
      });

Console.WriteLine("#### Read config from inmemory ####");
Console.WriteLine(config.GetValue<string>("KeyOne", "KeyOne not found"));
Console.WriteLine(config.GetValue<string>("KeyTwo", "KeyTwo not found"));
Console.WriteLine();

var endpointSettingSection = config.GetRequiredSection(EndpointSettings.SectionName);
//var listEndpointSettings = endpointSettingSection.Get<EndpointSettings[]>();
var listEndpointSettings = new List<EndpointSettings>();
endpointSettingSection.Bind(listEndpointSettings);
Console.WriteLine("#### Read section array from appsettings json in strongly typed class ####");
foreach (var endpointSetting in listEndpointSettings)
{
    Console.WriteLine(endpointSetting.ToString());
}
Console.WriteLine();

var optionsSettingSection = config.GetRequiredSection(OptionsSettings.SectionName);
var optionSetting = optionsSettingSection.Get<OptionsSettings>();
Console.WriteLine("#### Read section from appsettings json in strongly typed class ####");
Console.WriteLine(optionSetting.ToString());
Console.WriteLine();

Console.WriteLine($"!!!! RequiredSection throw {nameof(InvalidOperationException)} when section not found !!!!");
try
{
    _ = config.GetRequiredSection("fakeSection");
}
catch (InvalidOperationException ex)
{
    PrintExceptionMessage(ex);
}
Console.WriteLine();

Console.WriteLine("#### Read section and nestedsection from appsettings json in strongly typed class ####");
var nestedSettingSection = config.GetRequiredSection(NestedSettings.SectionName);
var nestedSettings = nestedSettingSection.Get<NestedSettings>();
Console.WriteLine(nestedSettings.ToString());
Console.WriteLine();

Console.WriteLine("#### Get value from appsettings json ####");
// ':' character for get key nested
Console.WriteLine(config.GetValue<string>("NestedSettings:KeyTwo:Message"));
Console.WriteLine();

var host = Host.CreateDefaultBuilder()
    //.ConfigureAppConfiguration((context, builder) => builder.AddConfiguration(config))
    .ConfigureServices(services =>
    {
        services.Configure<AuthProviders>(
            OptionsConstant.AuthProvider.Google,
            config.GetSection($"{AuthProviders.SectionName}:{OptionsConstant.AuthProvider.Google}"));

        services.Configure<AuthProviders>(
            OptionsConstant.AuthProvider.Facebook,
            config.GetSection($"{AuthProviders.SectionName}:{OptionsConstant.AuthProvider.Facebook}"));

        //configure validation on options
        services.AddOptions<ValidateSettings>()
            .Bind(config.GetRequiredSection(ValidateSettings.SectionName))
            .Validate(validateConfig => !string.IsNullOrWhiteSpace(validateConfig.Password), $"{nameof(ValidateSettings.Password)} is required");
    })
.UseConsoleLifetime()
.Build();


using var scopeService = host.Services.CreateScope();
var authProviderOptions = scopeService.ServiceProvider.GetRequiredService<IOptionsSnapshot<AuthProviders>>();
var googleAuthProvider = authProviderOptions.Get(OptionsConstant.AuthProvider.Google);
var facebookAuthProvider = authProviderOptions.Get(OptionsConstant.AuthProvider.Facebook);
Console.WriteLine("#### Read section repeated from appsettings json in same strongly typed class ####");
Console.WriteLine($"GoogleProvider settings => {googleAuthProvider}");
Console.WriteLine($"Facebook Settings => {facebookAuthProvider}");
Console.WriteLine();

Console.WriteLine("!!!! validation options !!!!");
try
{
    _ = scopeService.ServiceProvider.GetRequiredService<IOptions<ValidateSettings>>().Value;
}
catch (OptionsValidationException ex)
{
    PrintExceptionMessage(ex);
}

void PrintExceptionMessage(Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex.Message);
    Console.ResetColor();
}

//host.Run();