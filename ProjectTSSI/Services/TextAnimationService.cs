using ProjectTSSI.Models.Interfaces;

namespace ProjectTSSI.Services;

public class TextAnimationService : IAnimationsService
{
    public string Identifier => "TextAnimation";
    private Dictionary<string, Func<VisualElement, float, uint, string[], Task>> _moveAnimations;
    private Dictionary<string, Func<VisualElement, float, uint, string[], Task>> _resizeAnimations;
    private Dictionary<string, Func<VisualElement, float, uint, string[], Task>> _scaleAnimations;
    private Dictionary<string, Func<VisualElement, float, uint, string[], Task>> _visibilityAnimations;

    public TextAnimationService()
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
            { "ScaleMore", ScaleMore },
            { "ScaleDismiss", ScaleDismiss },
            { "ScaleFocusPercentage", ScaleFocusPercentage }
        };
    }
    private void AssignVisibilitysAnimations()
    {
        _visibilityAnimations = new Dictionary<string, Func<VisualElement, float, uint, string[], Task>>
        {
            { "OpacityFadeIn", OpacityFadeIn },
            { "OpacityFadeOut", OpacityFadeOut }
        };
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
        throw new NotImplementedException();
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
    private Task ScaleMore(VisualElement element, float percentage, uint duration, string[] additionalOptions)
    {
        throw new NotImplementedException();
    }
    private Task ScaleDismiss(VisualElement element, float percentage, uint duration, string[] additionalOptions)
    {
        throw new NotImplementedException();
    }
    private async Task ScaleFocusPercentage(VisualElement element, float percentage, uint duration, string[] additionalOptions)
    {
        uint durationUp = (uint)(duration * 0.55);
        uint durationDown = duration - durationUp;
        await element.ScaleTo(1 + (percentage / 100), durationUp, Easing.CubicIn);
        await element.ScaleTo(1.0, durationDown, Easing.CubicOut);
    }
    #endregion

    #region Visibility Animations
    public async Task VisibilityAnimation(VisualElement element, string animationName, float percentage, uint duration, string[] additionalOptions)
    {
        if (_visibilityAnimations.TryGetValue(animationName, out var moveFunc))
            await moveFunc(element, percentage, duration, additionalOptions);
        else
            throw new NotImplementedException();
    }
    private Task OpacityFadeIn(VisualElement element, float percentage, uint duration, string[] additionalOptions)
    {
        element.AbortAnimation("FadeIn");
        var tcs = new TaskCompletionSource<bool>();
        var fadeInAnimation = new Animation(v => element.Opacity = v, percentage, 1);
        fadeInAnimation.Commit(element, "FadeIn", 16, duration, Easing.Linear, (v, c) => tcs.SetResult(true));
        return tcs.Task;
    }
    private Task OpacityFadeOut(VisualElement element, float percentage, uint duration, string[] additionalOptions)
    {
        element.AbortAnimation("FadeOut");
        var tcs = new TaskCompletionSource<bool>();
        var fadeOutAnimation = new Animation(v => element.Opacity = v, percentage, 0);
        fadeOutAnimation.Commit(element, "FadeOut", 16, duration, Easing.Linear, (v, c) => tcs.SetResult(true));
        return tcs.Task;
    }
    #endregion
}