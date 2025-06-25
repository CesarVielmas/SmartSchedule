using System.Diagnostics;
using System.Globalization;
using Microsoft.Maui.Devices;
using System.ComponentModel;
using System.Text.Json;
using ProjectTSSI.Handlers.Windows;
using ProjectTSSI.Models.CustomHandlers;
using ProjectTSSI.Models.Interfaces;
using ProjectTSSI.Models;

namespace ProjectTSSI.Global;

#region Global Constants
public static class GlobalConstants
{
    public static double ScreenWidth { get; set; }
    public static double ScreenHeight { get; set; }
    public static double ScreenDensity { get; set; }
    public static bool IsDebugMode { get; set; }
    public static readonly string ConfigPath = "Resources/JsonsConfigs/CustomComponents.json";
    public static DebugInspector DebugInspector { get; set; }
    public static List<object> CustomComponent { get; set; } = new List<object>();
    public static List<object> CustomComponentWithOutChanges { get; set; } = new List<object>();
    public static void UpdateScreenMetrics(double width, double height, double density)
    {
        ScreenWidth = width;
        ScreenHeight = height;
        ScreenDensity = density;
    }
}
#endregion
#region Global Methods
public static class GlobalMethods
{
    public static Type GetTypeFromKey(string key)
    {
        if (key.EndsWith("_ResponsiveConfig", StringComparison.OrdinalIgnoreCase))
            return typeof(ResponsiveConfig);
        if (key.EndsWith("_GridResponsiveConfig", StringComparison.OrdinalIgnoreCase))
            return typeof(ResponsiveGridConfig);
        if (key.EndsWith("_StackLayoutResponsiveConfig", StringComparison.OrdinalIgnoreCase))
            return typeof(ResponsiveStackLayoutConfig);
        if (key.EndsWith("_LabelResponsiveConfig", StringComparison.OrdinalIgnoreCase))
            return typeof(ResponsiveLabelConfig);
        if (key.EndsWith("_FrameResponsiveConfig", StringComparison.OrdinalIgnoreCase))
            return typeof(ResponsiveFrameConfig);
        if (key.EndsWith("_BorderResponsiveConfig", StringComparison.OrdinalIgnoreCase))
            return typeof(ResponsiveBorderConfig);
        if (key.EndsWith("_EntryResponsiveConfig", StringComparison.OrdinalIgnoreCase))
            return typeof(ResponsiveEntryConfig);
        if (key.EndsWith("_ImageResponsiveConfig", StringComparison.OrdinalIgnoreCase))
            return typeof(ResponsiveImageConfig);
        if (key.EndsWith("_ButtonResponsiveConfig", StringComparison.OrdinalIgnoreCase))
            return typeof(ResponsiveButtonConfig);
        if (key.EndsWith("_BoxViewResponsiveConfig", StringComparison.OrdinalIgnoreCase))
            return typeof(ResponsiveBoxViewConfig);
        //add more for more objects custom bindable
        return null;
    }
    public static void LoadJsonConfigurations(Dictionary<string, INotifyPropertyChanged> Configurations)
    {
        if (!File.Exists(GlobalConstants.ConfigPath))
        {
            Logger.Log("No se encontro el archivo");
            File.WriteAllText(GlobalConstants.ConfigPath, "{}");
        }
        var json = File.ReadAllText(GlobalConstants.ConfigPath);
        var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
        foreach (var kvp in dict)
        {
            var type = GlobalMethods.GetTypeFromKey(kvp.Key);
            if (type == null) continue;
            var configObject = (INotifyPropertyChanged)JsonSerializer.Deserialize(kvp.Value.GetRawText(), type);
            Configurations[kvp.Key] = configObject;
            configObject.PropertyChanged += (_, _) => GlobalMethods.SaveConfig(Configurations);
        }
    }
    public static void SaveConfig(Dictionary<string, INotifyPropertyChanged> Configurations)
    {
        var serialized = new Dictionary<string, object>();
        foreach (var (key, value) in Configurations)
        {
            serialized[key] = value;
        }
        var json = JsonSerializer.Serialize(serialized, new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new ObjectToInferredTypeConverter() }
        });
        File.WriteAllText(GlobalConstants.ConfigPath, json);
    }
    public static Thickness ConvertThicknessFromString(string thicknessString)
    {
        if (string.IsNullOrEmpty(thicknessString))
            return new Thickness(0);
        string[] parts = thicknessString.Split(',');
        double[] values = new double[4];
        for (int i = 0; i < values.Length; i++)
        {
            if (i < parts.Length && !string.IsNullOrWhiteSpace(parts[i]))
            {
                double.TryParse(parts[i], NumberStyles.Any, CultureInfo.InvariantCulture, out values[i]);
            }
        }
        double left = (GlobalConstants.ScreenWidth * values[0]) / GlobalConstants.ScreenDensity;
        double top = (GlobalConstants.ScreenHeight * values[1]) / GlobalConstants.ScreenDensity;
        double right = (GlobalConstants.ScreenWidth * values[2]) / GlobalConstants.ScreenDensity;
        double bottom = (GlobalConstants.ScreenHeight * values[3]) / GlobalConstants.ScreenDensity;
        switch (parts.Length)
        {
            case 1:
                return new Thickness(left);

            case 2:
                return new Thickness(left, top);

            default:
                return new Thickness(left, top, right, bottom);
        }
    }
    public static CornerRadius ConvertCornerRadiusFromString(string cornerRadiusString)
    {
        if (string.IsNullOrEmpty(cornerRadiusString))
            return new CornerRadius(0);
        string[] parts = cornerRadiusString.Split(',');
        double[] values = new double[4];
        for (int i = 0; i < values.Length; i++)
        {
            if (i < parts.Length && !string.IsNullOrWhiteSpace(parts[i]))
            {
                double.TryParse(parts[i], NumberStyles.Any, CultureInfo.InvariantCulture, out values[i]);
            }
        }
        double left = (GlobalConstants.ScreenWidth * values[0]) / GlobalConstants.ScreenDensity;
        double top = (GlobalConstants.ScreenHeight * values[1]) / GlobalConstants.ScreenDensity;
        double right = (GlobalConstants.ScreenWidth * values[2]) / GlobalConstants.ScreenDensity;
        double bottom = (GlobalConstants.ScreenHeight * values[3]) / GlobalConstants.ScreenDensity;
        return new CornerRadius(left, top, right, bottom);
    }
    public static Shadow ConvertShadowFromString(string shadowString)
    {
        float defaultRadius = 0f;
        float defaultOpacity = 0f;
        Color defaultColor = Colors.Black;
        Point defaultOffset = new Point(0, 0);
        var shadow = new Shadow
        {
            Radius = defaultRadius,
            Opacity = defaultOpacity,
            Brush = new SolidColorBrush(defaultColor),
            Offset = defaultOffset
        };
        if (string.IsNullOrWhiteSpace(shadowString))
            return shadow;
        string[] parts = shadowString.Split(',', StringSplitOptions.None);
        if (parts.Length > 0 && !string.IsNullOrWhiteSpace(parts[0]))
        {
            if (float.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out float radius))
            {
                shadow.Radius = radius;
            }
        }
        if (parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1]))
        {
            if (float.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float opacity))
            {
                shadow.Opacity = Math.Clamp(opacity, 0f, 1f);
            }
        }
        if (parts.Length > 2 && !string.IsNullOrWhiteSpace(parts[2]))
        {
            if (parts[2].StartsWith("#") && !int.TryParse(parts[2], out _))
            {
                var color = Color.FromArgb(parts[2].Trim());
                shadow.Brush = new SolidColorBrush(color);
            }
        }
        if (parts.Length > 3 && !string.IsNullOrWhiteSpace(parts[3]))
        {
            string offsetStr = parts[3].Trim();
            if (offsetStr.StartsWith("(") && offsetStr.EndsWith(")"))
            {
                string coords = offsetStr.Substring(1, offsetStr.Length - 2);
                string[] xy = coords.Split(',');
                if (xy.Length >= 2)
                {
                    if (double.TryParse(xy[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double x) &&
                        double.TryParse(xy[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double y))
                    {
                        shadow.Offset = new Point((int)((GlobalConstants.ScreenWidth * x) / GlobalConstants.ScreenDensity), (int)((GlobalConstants.ScreenHeight * y) / GlobalConstants.ScreenDensity));
                    }
                }
                else if (xy.Length == 1)
                {
                    if (double.TryParse(xy[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
                    {
                        shadow.Offset = new Point((int)((GlobalConstants.ScreenWidth * val) / GlobalConstants.ScreenDensity), (int)((GlobalConstants.ScreenHeight * val) / GlobalConstants.ScreenDensity));
                    }
                }
            }
        }
        return shadow;
    }
    public static async Task AnimateTextByWord(Label label, string fullText, int delayMilliseconds = 300)
    {
        if (string.IsNullOrWhiteSpace(fullText) || label == null)
            return;
        label.Text = "";
        for (int i = 0; i < fullText.Length; i++)
        {
            label.Text = label.Text.Split('|')[0];
            label.Text += (i == 0 ? "" : " ") + fullText[i] + "|";
            await Task.Delay(delayMilliseconds);
        }
        label.Text = label.Text.Substring(0, label.Text.Length - 1);
    }
    public static void AnimateMoveAbsoluteElementY(VisualElement element, string position = "", double positionY = 0, uint duration = 0)
    {
        element.AbortAnimation("MoveAnimationAbsoluteY");
        Rect currentBounds = AbsoluteLayout.GetLayoutBounds(element);
        double startY = currentBounds.Y;
        double endY = positionY;
        if (position != "")
        {
            switch (position)
            {
                case "top":
                    endY = 0.1;
                    break;
                case "bottom":
                    endY = 1;
                    break;
                case "center":
                    endY = 0.5;
                    break;
            }
        }
        var animation = new Animation(v =>
        {
            AbsoluteLayout.SetLayoutBounds(element, new Rect(currentBounds.X, v, currentBounds.Width, currentBounds.Height));
        }, startY, endY, Easing.CubicOut);
        animation.Commit(element, "MoveAnimationAbsoluteY", 16, duration, Easing.Linear);
    }
    public static void OnGlobalTapDebug(object sender, EventArgs e)
    {
        if (sender is ITapCustomComponent customElement && GlobalConstants.IsDebugMode)
        {
            customElement.IsTapEnabled = !customElement.IsTapEnabled;
            if (GlobalConstants.CustomComponent.Count > 0 && GlobalConstants.CustomComponentWithOutChanges.Count > 0)
            {
                string automationIdComponentActual = customElement.GetCustomerComponentData().FirstOrDefault().GetType().GetProperty("AutomationId").GetValue(customElement.GetCustomerComponentData().FirstOrDefault()) as string;
                string automationIdComponentLast = GlobalConstants.CustomComponent.FirstOrDefault().GetType().GetProperty("AutomationId").GetValue(GlobalConstants.CustomComponent.FirstOrDefault()) as string;
                if (automationIdComponentActual != automationIdComponentLast)
                {
                    (GlobalConstants.CustomComponent.FirstOrDefault(x => x is ITapCustomComponent) as ITapCustomComponent)?.OnUnTapStyle();
                    GlobalConstants.CustomComponent = new List<object>();
                    GlobalConstants.CustomComponentWithOutChanges = new List<object>();
                    GlobalConstants.CustomComponent.AddRange(customElement.GetCustomerComponentData());
                    GlobalConstants.CustomComponentWithOutChanges.AddRange(customElement.GetCustomerComponentData());
                    customElement.OnTapStyle();
                    GlobalConstants.DebugInspector.OnSelectedElement(GlobalConstants.CustomComponent);

                }
            }
            else
            {
                GlobalConstants.CustomComponent.AddRange(customElement.GetCustomerComponentData());
                GlobalConstants.CustomComponentWithOutChanges.AddRange(customElement.GetCustomerComponentData());
                GlobalConstants.DebugInspector.OnSelectedElement(GlobalConstants.CustomComponent);
            }


        }
    }
    public static void OnGlobalUnTapDebug()
    {
        if (GlobalConstants.CustomComponent.Count > 0 && GlobalConstants.CustomComponentWithOutChanges.Count > 0)
        {
            (GlobalConstants.CustomComponent.FirstOrDefault(x => x is ITapCustomComponent) as ITapCustomComponent)?.OnUnTapStyle();
            GlobalConstants.CustomComponent = new List<object>();
            GlobalConstants.CustomComponentWithOutChanges = new List<object>();
        }
    }
}
public static class Logger
{
    private static string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "maui_log.txt");
    private static StreamWriter? logWriter;

    public static void Init()
    {
        logWriter = new StreamWriter(logPath, append: true) { AutoFlush = true };
        Process.Start("notepad.exe", logPath);
    }

    public static void Log(string mensaje)
    {
        logWriter?.WriteLine($"{DateTime.Now:HH:mm:ss} - {mensaje}");
    }

    public static void Close()
    {
        logWriter?.Close();
    }
}

#endregion