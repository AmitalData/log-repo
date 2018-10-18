

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
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs; 
using Simplog.Data.CommonDataModel;

namespace Logitude.BL.CommonDataModel
{
   public class DueTypeDetails : DueType, ICloseTable<DueType, DueTypeDetails>
   {
       public List<DueTypeDetails> GetAll()
       {
		    var all = new List<DueTypeDetails>();  
            all.Add(new DueTypeDetails()
            {    
                SearchFields = "ag,agent", 
                Code = "AG", 
                Name = "Agent", 
			});
			 
            all.Add(new DueTypeDetails()
            {    
                SearchFields = "ca,carrier", 
                Code = "CA", 
                Name = "Carrier", 
			});
			 
            all.Add(new DueTypeDetails()
            {    
                SearchFields = "no,none", 
                Code = "NO", 
                Name = "none", 
			});
			 
            all.Add(new DueTypeDetails()
            {    
                SearchFields = "tx,tax", 
                Code = "TX", 
                Name = "Tax", 
			});
			 
            all.Add(new DueTypeDetails()
            {    
                SearchFields = "vl,valuation", 
                Code = "VL", 
                Name = "Valuation", 
			});
			
            return all;
       }

	    public void MapPoco(DueType newPoco)
        {   
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(DueType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

