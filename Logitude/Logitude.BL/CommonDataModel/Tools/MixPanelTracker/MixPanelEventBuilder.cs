using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Logitude.BL.CommonDataModel.Tools.MixPanelTracker
{
    public class MixPanelEventBuilder
    {
        public string EventName;
        private StringBuilder eventJSON = new StringBuilder();
        private MixPanelEvent @event = new MixPanelEvent();

        List<TrackingEventProperty> EventProperties = new List<TrackingEventProperty>();

        public void BuildEvent(string eventName)
        {
            @event.Name = eventName;            
        }

        public void AddProperty(string name, string value)
        {
            @event.Properties.Add(new TrackingEventProperty(name,value));
        }


    }

    public class TrackingEventProperty 
    {
        public TrackingEventProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }
        public string Name { get; set; }
        public string Value { get; set; }
    }

}
