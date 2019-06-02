
   
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
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.BL.EntityPMs; 
using Logitude.TimeManagement.Data;

namespace Logitude.TimeManagement.BL.CLoseTable
{
   public class TMLocationDetails : TMLocation, ICloseTable<TMLocation, TMLocationDetails>
   {
       public List<TMLocationDetails> GetAll()
       {
		    var all = new List<TMLocationDetails>();  
            all.Add(new TMLocationDetails()
            {    
                Code = "C", 
                Name = "Client", 
                SearchFields = "C,Client,", 
			});
			 
            all.Add(new TMLocationDetails()
            {    
                Code = "H", 
                Name = "Home", 
                SearchFields = "H,Home,", 
			});
			 
            all.Add(new TMLocationDetails()
            {    
                Code = "O", 
                Name = "Office", 
                SearchFields = "O,Office,", 
			});
			 
            all.Add(new TMLocationDetails()
            {    
                Code = "D", 
                Name = "Day Off", 
                SearchFields = "D,Day Off", 
			});
			
            return all;
       }

	    public void MapPoco(TMLocation newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(TMLocation rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

