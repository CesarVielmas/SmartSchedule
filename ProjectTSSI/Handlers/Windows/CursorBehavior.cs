namespace ProjectTSSI.Handlers.Windows
{
    public class CursorBehavior
    {
        public static readonly BindableProperty CursorProperty = BindableProperty.CreateAttached(
            "Cursor", typeof(CursorIcon), typeof(CursorBehavior), CursorIcon.Arrow, propertyChanged: OnCursorChanged);

        public static CursorIcon GetCursor(BindableObject view)
        {
            return (CursorIcon)view.GetValue(CursorProperty);
        }

        public static void SetCursor(BindableObject view, CursorIcon value)
        {
            view.SetValue(CursorProperty, value);
        }

        private static void OnCursorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is VisualElement visualElement)
            {
                visualElement.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "Handler")
                    {
                        var handler = visualElement.Handler;
                        if (handler != null)
                        {
                            visualElement.SetCustomCursor((CursorIcon)newValue, handler.MauiContext);
                        }
                    }
                };

                if (visualElement.Handler != null)
                {
                    visualElement.SetCustomCursor((CursorIcon)newValue, visualElement.Handler.MauiContext);
                }
            }
        }
    }
}
