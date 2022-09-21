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

        public string ProjectToken { get { return "55727552404267ef137b8a1579079bc1"; } }

        public bool IsValid => true;

        public MixPanelEvent BuildEvent(MixPanelActionsEvent mixPanelActionsEvent)
        {
            MixPanelEvent mixPanelEvent = new MixPanelEvent();
            mixPanelEvent.Name = mixPanelActionsEvent.ActionName;
            mixPanelEvent.AddProperty("Email", mixPanelActionsEvent.Email);
            mixPanelEvent.AddProperty("Message", mixPanelActionsEvent.Message);
            mixPanelEvent.AddProperty("DashboardId", mixPanelActionsEvent.DashboardId);
            return mixPanelEvent;
        }
    }
}