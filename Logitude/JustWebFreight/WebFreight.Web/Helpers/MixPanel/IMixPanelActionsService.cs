using Logitude.BL.CommonDataModel.Tools.MixPanelTracker;

namespace WebFreight.Web.Helpers.MixPanel
{
    public interface IMixPanelActionsService
    {
        string ProjectToken { get; }
        MixPanelEvent BuildMixPanelEvent(MixPanelActionsEvent mixPanelActionsEvent);
    }
}
