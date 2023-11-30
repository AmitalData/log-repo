using Logitude.BL.CommonDataModel.Tools.MixPanelTracker;

namespace WebFreight.Web.Helpers.MixPanel
{
    public interface IMixPanelActionsService
    {
        string ProjectToken { get; }
        bool IsValid { get; }
        MixPanelEvent BuildEvent(MixPanelActionsEvent mixPanelActionsEvent);
    }
}
