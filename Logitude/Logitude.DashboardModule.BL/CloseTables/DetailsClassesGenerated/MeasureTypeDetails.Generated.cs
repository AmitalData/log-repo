
   
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
   public class MeasureTypeDetails : MeasureType, ICloseTable<MeasureType, MeasureTypeDetails>
   {
       public List<MeasureTypeDetails> GetAll()
       {
		    var all = new List<MeasureTypeDetails>();  
            all.Add(new MeasureTypeDetails()
            {    
                Code = "Sum", 
                Name = "Sum", 
                SearchFields = "Sum", 
			});
			 
            all.Add(new MeasureTypeDetails()
            {    
                Code = "Min", 
                Name = "Min", 
                SearchFields = "Min", 
			});
			 
            all.Add(new MeasureTypeDetails()
            {    
                Code = "Max", 
                Name = "Max", 
                SearchFields = "Max", 
			});
			 
            all.Add(new MeasureTypeDetails()
            {    
                Code = "Count", 
                Name = "Count", 
                SearchFields = "Count", 
			});
			 
            all.Add(new MeasureTypeDetails()
            {    
                Code = "Avg", 
                Name = "Average", 
                SearchFields = "Average", 
			});
			
            return all;
       }

	    public void MapPoco(MeasureType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(MeasureType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

