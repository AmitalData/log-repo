
   
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
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.BL.EntityPMs; 
using Logitude.DashboardModule.Data;

namespace Logitude.DashboardModule.BL
{
   public class WidgetTypeDetails : WidgetType, ICloseTable<WidgetType, WidgetTypeDetails>
   {
       public List<WidgetTypeDetails> GetAll()
       {
		    var all = new List<WidgetTypeDetails>();  
            all.Add(new WidgetTypeDetails()
            {    
                Code = "line", 
                Name = "Line", 
                SearchFields = "Line", 
			});
			 
            all.Add(new WidgetTypeDetails()
            {    
                Code = "bar", 
                Name = "Bar", 
                SearchFields = "Bar", 
			});
			 
            all.Add(new WidgetTypeDetails()
            {    
                Code = "pie", 
                Name = "Pie", 
                SearchFields = "Pie", 
			});
			 
            all.Add(new WidgetTypeDetails()
            {    
                Code = "donut", 
                Name = "Donut", 
                SearchFields = "Donut", 
			});
			 
            all.Add(new WidgetTypeDetails()
            {    
                Code = "kpi", 
                Name = "KPI", 
                SearchFields = "Kpi", 
			});
			 
            all.Add(new WidgetTypeDetails()
            {    
                Code = "column", 
                Name = "Column", 
                SearchFields = "Column", 
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

