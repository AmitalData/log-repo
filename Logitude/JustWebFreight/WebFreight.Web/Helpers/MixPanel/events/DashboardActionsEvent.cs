using Logitude.BL.CommonDataModel.Tools.MixPanelTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.MixPanel.events
{
    public class DashboardActionsEvent : IMixPanelActionsService
    {
        private readonly int tenant;

        public DashboardActionsEvent(int tenant)
        {
            this.tenant = tenant;
        }

        public string ProjectToken { get { return ""; } }

        public bool IsValid => true;

        public MixPanelEvent BuildEvent(MixPanelActionsEvent mixPanelActionsEvent)
        {
            MixPanelEvent mixPanelEvent = new MixPanelEvent();
            mixPanelEvent.Name = mixPanelActionsEvent.ActionName;
            mixPanelEvent.AddProperty("email", mixPanelActionsEvent.Email);
            mixPanelEvent.AddProperty("is_public", "False");
            mixPanelEvent.AddProperty("tenant", tenant.ToString());
            return mixPanelEvent;
        }
    }
}