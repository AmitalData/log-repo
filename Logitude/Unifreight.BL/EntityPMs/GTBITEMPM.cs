using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class GTBITEMPM 
    {
        //private string searchFields;
        //public string SearchFields
        //{
        //    get
        //    {
        //        return searchFields;
        //    }
        //    set
        //    {
        //        if (searchFields != value)
        //        {
        //            NotifyPropertyChangeValues values = new NotifyPropertyChangeValues() { PropertyName = "SearchFields", OldValue = searchFields, NewValue = value, PropertyType = "string" };
        //            NotifyPropertyChanged(values);
        //            searchFields = value;
        //        }

        //    }
        //}

        public string SearchFields { get; set; }
    }
}
