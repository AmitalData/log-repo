using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.MixPanelTracker
{
    public class MixPanelEvent
    {
        public MixPanelEvent(string name)
        {
            Name = name;
        }
        public MixPanelEvent()
        {
        }
        private StringBuilder eventJSON = new StringBuilder();

        public string Name { get; set; }
        public List<TrackingEventProperty> Properties = new List<TrackingEventProperty>();

        public override string ToString() => GetEventJSON();
        
        public void AddProperty(string name, string value)
        {
            Properties.Add(new TrackingEventProperty(name, value));
        }
        public string GetEventJSON()
        {
            eventJSON = new StringBuilder();
            eventJSON.Append("{ ");

            eventJSON.Append($" \"event\": \"{Name}\",");

            eventJSON.Append(" \"properties\": {");
            AppendPropertiesToJSON();
            eventJSON.Append(" }");

            eventJSON.Append("}");

            return eventJSON.ToString();
        }

        private void AppendPropertiesToJSON()
        {
            for (int i = 0; i < Properties.Count; i++)
            {
                AppendProperty(Properties[i]);
                if (i < Properties.Count-1)
                    eventJSON.Append(" ,");
            }
        }

        private void AppendProperty(TrackingEventProperty property)
        {
            eventJSON.Append($" \"{property.Name}\": \"{property.Value}\"");
        }
        
    }
}
