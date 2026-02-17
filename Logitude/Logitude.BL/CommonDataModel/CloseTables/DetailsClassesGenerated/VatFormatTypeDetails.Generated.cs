

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
   public class VatFormatTypeDetails : VatFormatType, ICloseTable<VatFormatType, VatFormatTypeDetails>
   {
       public List<VatFormatTypeDetails> GetAll()
       {
		    var all = new List<VatFormatTypeDetails>();  
            all.Add(new VatFormatTypeDetails()
            {    
                Code = "FSC", 
                Name = "Apply for a Specific Country", 
                SearchFields = "FSC,Apply for a Specific Country", 
			});
			 
            all.Add(new VatFormatTypeDetails()
            {    
                Code = "FAC", 
                Name = "Apply for All Countries", 
                SearchFields = "FAC,Apply for All Countries", 
			});
			 
            all.Add(new VatFormatTypeDetails()
            {    
                Code = "NOF", 
                Name = "No Format", 
                SearchFields = "NOF,No Format", 
			});
			
            return all;
       }

	    public void MapPoco(VatFormatType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(VatFormatType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

