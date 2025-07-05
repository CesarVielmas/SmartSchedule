using ProjectTSSI.Models.Interfaces;

namespace ProjectTSSI.Services;

public class ComposerAnimationsService : IAnimationServiceComposer
{
    private readonly Dictionary<string, IAnimationsService> _services;

    public ComposerAnimationsService(IEnumerable<IAnimationsService> services)
    {
        _services = services.ToDictionary(s => s.Identifier, StringComparer.OrdinalIgnoreCase);
    }
    public Task Animate(string serviceType, string animationType, VisualElement element, string animationName, float percentage, uint duration, string[] additionalOptions = null)
    {
        if (_services.TryGetValue(serviceType, out var service))
            if (animationType == "Scale")
                return service.ScaleElementAnimation(element, animationName, percentage, duration, additionalOptions);
            else if (animationType == "Move")
                return service.MoveElementAnimation(element, animationName, percentage, duration, additionalOptions);
            else if (animationType == "Resize")
                return service.ReSizeAnimation(element, animationName, percentage, duration, additionalOptions);
            else if (animationType == "Visibility")
                return service.VisibilityAnimation(element, animationName, percentage, duration, additionalOptions);
            else
                throw new InvalidOperationException($"Animation service '{animationType}' not found.");
        else
            throw new InvalidOperationException($"Animation service '{serviceType}' not found.");
    }
}