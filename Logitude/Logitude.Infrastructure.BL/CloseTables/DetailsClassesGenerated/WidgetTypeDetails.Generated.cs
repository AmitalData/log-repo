
   
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;  
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.CloseTablesClasses;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs; 
using Logitude.Infrastructure.Data;

namespace Logitude.Infrastructure.BL
{
   public class WidgetTypeDetails : WidgetType, ICloseTable<WidgetType, WidgetTypeDetails>
   {
       public List<WidgetTypeDetails> GetAll()
       {
		    var all = new List<WidgetTypeDetails>();  
            all.Add(new WidgetTypeDetails()
            {    
                Code = "Co", 
                Name = "Column", 
                SearchFields = "Column", 
			});
			 
            all.Add(new WidgetTypeDetails()
            {    
                Code = "Are", 
                Name = "Area", 
                SearchFields = "Area", 
			});
			 
            all.Add(new WidgetTypeDetails()
            {    
                Code = "Bar", 
                Name = "Bar", 
                SearchFields = "Bar", 
			});
			 
            all.Add(new WidgetTypeDetails()
            {    
                Code = "Pie", 
                Name = "Pie", 
                SearchFields = "Pie", 
			});
			 
            all.Add(new WidgetTypeDetails()
            {    
                Code = "Don", 
                Name = "Donut", 
                SearchFields = "Donut", 
			});
			
            return all;
       }

	    public void MapPoco(WidgetType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(WidgetType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

