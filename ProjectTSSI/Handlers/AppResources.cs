using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace ProjectTSSI.Handlers;

public static class AppResources
{
    private static readonly ResourceManager _resourceManager =
        new ResourceManager("ProjectTSSI.Resources.Strings.AppResources", typeof(AppResources).Assembly);

    public static string TittleLogin => _resourceManager.GetString("TittleLogin", CultureInfo.CurrentUICulture);
    public static string SecondTittleLogin => _resourceManager.GetString("SecondTittleLogin", CultureInfo.CurrentUICulture);
    public static string MailLoginTittle => _resourceManager.GetString("MailLoginTittle", CultureInfo.CurrentUICulture);
    public static string PasswordLoginTittle => _resourceManager.GetString("PasswordLoginTittle", CultureInfo.CurrentUICulture);
    public static string EnterLoginTittle => _resourceManager.GetString("EnterLoginTittle", CultureInfo.CurrentUICulture);
    public static string OptionsLoginTittle => _resourceManager.GetString("OptionsLoginTittle", CultureInfo.CurrentUICulture);
    public static string RegisterLoginTittle => _resourceManager.GetString("RegisterLoginTittle", CultureInfo.CurrentUICulture);
    public static string WithOutAccountLoginTittle => _resourceManager.GetString("WithOutAccountLoginTittle", CultureInfo.CurrentUICulture);
    public static string DebugInspectorInit => _resourceManager.GetString("DebugInspectorInit", CultureInfo.CurrentUICulture);

}
public static class LocalizationManager
{
    public static void SetCulture(string languageCode)
    {
        var culture = new CultureInfo(languageCode);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }
}

[ContentProperty(nameof(Key))]
public class TranslateExtension : IMarkupExtension
{
    public string Key { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        if (Key == null)
            return "";

        var resourceManager = new ResourceManager("ProjectTSSI.Resources.Strings.AppResources", typeof(AppResources).GetTypeInfo().Assembly);
        return resourceManager.GetString(Key, CultureInfo.CurrentUICulture) ?? Key;
    }
}


