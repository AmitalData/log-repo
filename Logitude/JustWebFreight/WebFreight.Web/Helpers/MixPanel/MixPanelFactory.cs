using System;
using WebFreight.Web.Helpers.MixPanel.events;

namespace WebFreight.Web.Helpers.MixPanel
{
    public static class MixPanelFactory
    {
        public static IMixPanelActionsService Create(string projectName, int tenant)
        {
            switch (projectName)
            {
                case "LogBox":  return new LogBoxActionsEvent(tenant);
                case "Dashboard": return new DashboardActionsEvent(tenant);
            }
            throw new Exception("Please Provide Mix Panel Event");
        }
    }
}