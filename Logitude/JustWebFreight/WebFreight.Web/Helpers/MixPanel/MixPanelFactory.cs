using System;
using WebFreight.Web.Helpers.MixPanel.events;

namespace WebFreight.Web.Helpers.MixPanel
{
    public static class MixPanelFactory
    {
        public static IMixPanelActionsService Create(string projectName, int tenant)
        {
            if (projectName == "LogBox")
            {
                return new LogBoxActionsEvent(tenant);
            }
            throw new Exception("Please Provide Mix Panel Event");
        }
    }
}