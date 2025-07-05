namespace ProjectTSSI.Models.Interfaces;

public interface IAnimationServiceComposer
{
    Task Animate(string serviceType, string animationType, VisualElement element, string animationName, float percentage, uint duration, string[] additionalOptions = null);
}