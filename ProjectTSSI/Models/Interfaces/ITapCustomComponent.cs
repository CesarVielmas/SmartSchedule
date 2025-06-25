
namespace ProjectTSSI.Models.Interfaces;

public interface ITapCustomComponent
{
    bool IsTapEnabled { get; set; }
    void OnTapStyle();
    void OnUnTapStyle();
    List<object> GetCustomerComponentData();
}