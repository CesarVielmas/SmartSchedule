namespace ProjectTSSI.Models.Interfaces;

public interface IAnimationsService
{
    string Identifier { get; }
    Task ScaleElementAnimation(VisualElement element, string animationName, float percentage, uint duration, string[] additionalOptions);
    Task MoveElementAnimation(VisualElement element, string animationName, float percentage, uint duration, string[] additionalOptions);
    Task ReSizeAnimation(VisualElement element, string animationName, float percentage, uint duration, string[] additionalOptions);
    Task VisibilityAnimation(VisualElement element, string animationName, float percentage, uint duration, string[] additionalOptions);
}