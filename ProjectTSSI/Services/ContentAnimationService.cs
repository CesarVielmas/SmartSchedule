using ProjectTSSI.Global;
using ProjectTSSI.Models.Interfaces;

namespace ProjectTSSI.Services;

public class ContentAnimationService : IAnimationsService
{
    public string Identifier => "ContentAnimation";
    private Dictionary<string, Func<VisualElement, float, uint, string[], Task>> _moveAnimations;
    private Dictionary<string, Func<VisualElement, float, uint, string[], Task>> _resizeAnimations;
    private Dictionary<string, Func<VisualElement, float, uint, string[], Task>> _scaleAnimations;
    private Dictionary<string, Func<VisualElement, float, uint, string[], Task>> _visibilityAnimations;

    public ContentAnimationService()
    {
        AssignMovesAnimations();
        AssignResizeAnimations();
        AssignScalesAnimations();
        AssignVisibilitysAnimations();
    }
    #region Assign animations
    private void AssignMovesAnimations()
    {
        _moveAnimations = new Dictionary<string, Func<VisualElement, float, uint, string[], Task>>
        {
            { "MoveElementX", MoveElementX },
            { "MoveElementY", MoveElementY }
        };
    }
    private void AssignResizeAnimations()
    {
        _resizeAnimations = new Dictionary<string, Func<VisualElement, float, uint, string[], Task>>
        {
            { "ResizeHeight", ResizeHeight },
            { "ResizeWidth", ResizeWidth }
        };
    }
    private void AssignScalesAnimations()
    {
        _scaleAnimations = new Dictionary<string, Func<VisualElement, float, uint, string[], Task>>
        {
            { "ScaleColor", ScaleColor },
        };
    }
    private void AssignVisibilitysAnimations()
    {

    }
    #endregion

    #region Animations Move
    public async Task MoveElementAnimation(VisualElement element, string animationName, float percentage, uint duration, string[] additionalOptions)
    {
        if (_moveAnimations.TryGetValue(animationName, out var moveFunc))
            await moveFunc(element, percentage, duration, additionalOptions);
        else
            throw new NotImplementedException();
    }
    private Task MoveElementX(VisualElement element, float percentage, uint duration, string[] additionalOptions)
    {
        throw new NotImplementedException();
    }
    private Task MoveElementY(VisualElement element, float percentage, uint duration, string[] additionalOptions)
    {
        var tcs = new TaskCompletionSource<bool>();
        var bounds = AbsoluteLayout.GetLayoutBounds(element);
        var initialY = bounds.Y;
        var targetY = initialY + (GlobalConstants.ScreenHeight * (percentage / 100f));

        var animation = new Animation(v =>
        {
            AbsoluteLayout.SetLayoutBounds(element, new Rect(bounds.X, v, bounds.Width, bounds.Height));
        }, initialY, targetY);

        element.Animate("moveAnimation", animation, length: duration, easing: Easing.Linear,
            finished: (v, c) => tcs.SetResult(true));
        return tcs.Task;
    }
    #endregion

    #region  Animations Resize
    public async Task ReSizeAnimation(VisualElement element, string animationName, float percentage, uint duration, string[] additionalOptions)
    {
        if (_resizeAnimations.TryGetValue(animationName, out var moveFunc))
            await moveFunc(element, percentage, duration, additionalOptions);
        else
            throw new NotImplementedException();
    }
    private Task ResizeHeight(VisualElement element, float percentage, uint duration, string[] additionalOptions)
    {
        throw new NotImplementedException();
    }
    private Task ResizeWidth(VisualElement element, float percentage, uint duration, string[] additionalOptions)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Scales Animations
    public async Task ScaleElementAnimation(VisualElement element, string animationName, float percentage, uint duration, string[] additionalOptions)
    {
        if (_scaleAnimations.TryGetValue(animationName, out var moveFunc))
            await moveFunc(element, percentage, duration, additionalOptions);
        else
            throw new NotImplementedException();
    }
    private Task ScaleColor(VisualElement element, float percentage, uint duration, string[] additionalOptions)
    {
        if (additionalOptions == null)
            throw new ArgumentNullException(nameof(additionalOptions), "El parámetro 'additionalOptions' no puede ser nulo para la animación ScaleColor.");
        if (additionalOptions.Length != 2)
            throw new ArgumentException("El parámetro 'additionalOptions' debe contener 2 elementos para la animación ScaleColor.", nameof(additionalOptions));
        if (!additionalOptions[0].StartsWith("#") || !additionalOptions[1].StartsWith("#"))
            throw new ArgumentException("El parámetro 'additionalOptions' debe contener 2 elementos con colores hexadecimales", nameof(additionalOptions));
        element.AbortAnimation("GradientAnimation");
        var tcs = new TaskCompletionSource<bool>();
        var gradientBrush = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(1, 1),
            GradientStops = new GradientStopCollection{
                            new GradientStop{
                                Color = Color.FromArgb(additionalOptions[0]),
                                Offset = 0.0f
                            },
                            new GradientStop{
                                Color = Color.FromArgb(additionalOptions[1]),
                                Offset = 0.0f
                            }
                    }
        };
        element.Background = gradientBrush;
        var animation = new Animation();
        animation.WithConcurrent(
            (progress) =>
            {
                gradientBrush.GradientStops[1].Offset = (float)progress;
            },
            start: percentage,
            end: 1.0
        );
        animation.Commit(
            element,
            "GradientAnimation",
            16,
            duration,
            Easing.Linear,
            (v, c) => tcs.SetResult(true)
        );
        return tcs.Task;
    }
    #endregion

    #region Visibility Animations
    public async Task VisibilityAnimation(VisualElement element, string animationName, float percentage, uint duration, string[] additionalOptions)
    {
        throw new NotImplementedException();
    }
    #endregion
}