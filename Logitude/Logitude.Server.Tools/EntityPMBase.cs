using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools
{
   public class EntityPMBase
    {

        public static bool SuppressCreateNotifyPropertyChangeValues = false;
        List<NotifyPropertyChangeValues> changedProperties;
        public List<NotifyPropertyChangeValues> ChangedProperties
        {
            get
            {
                if (changedProperties == null)
                {
                    if (SuppressCreateNotifyPropertyChangeValues) return null;
                    changedProperties = new List<NotifyPropertyChangeValues>();
                }
                return changedProperties;
            }
        }
        public void NotifyPropertyChanged(NotifyPropertyChangeValues values)
        {
            if (SuppressCreateNotifyPropertyChangeValues) return;
            values.Id = Guid.NewGuid().ToString();
            ChangedProperties.Add(values);
        }

    }
}
