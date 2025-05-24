#if WINDOWS
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.UI.Core;

namespace ProjectTSSI
{
    public static class CursorExtensions
    {
        public static void SetCustomCursor(this VisualElement element, CursorIcon cursorIcon, IMauiContext mauiContext)
        {
            if (mauiContext == null || element.Handler?.PlatformView is not Control nativeControl)
                return;

            var window = Microsoft.UI.Xaml.Window.Current;
            if (window == null)
                return;

            CoreCursor cursor = cursorIcon switch
            {
                CursorIcon.Wait => new CoreCursor(CoreCursorType.Wait, 0),
                CursorIcon.Hand => new CoreCursor(CoreCursorType.Hand, 0),
                CursorIcon.IBeam => new CoreCursor(CoreCursorType.IBeam, 0),
                CursorIcon.Cross => new CoreCursor(CoreCursorType.Cross, 0),
                CursorIcon.SizeAll => new CoreCursor(CoreCursorType.SizeAll, 0),
                _ => new CoreCursor(CoreCursorType.Arrow, 0),
            };

            nativeControl.PointerEntered += (s, e) =>
            {
                window.CoreWindow.PointerCursor = cursor;
            };
            nativeControl.PointerExited += (s, e) =>
            {
                window.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
            };
        }
    }
}
#endif
