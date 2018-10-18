

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
   public class VatUniqueTypeDetails : VatUniqueType, ICloseTable<VatUniqueType, VatUniqueTypeDetails>
   {
       public List<VatUniqueTypeDetails> GetAll()
       {
		    var all = new List<VatUniqueTypeDetails>();  
            all.Add(new VatUniqueTypeDetails()
            {    
                Code = "UNT", 
                Name = "Not Unique", 
                SearchFields = "UNT,Not Unique", 
			});
			 
            all.Add(new VatUniqueTypeDetails()
            {    
                Code = "USC", 
                Name = "Unique for a specific country", 
                SearchFields = "USC,Unique for a specific country", 
			});
			 
            all.Add(new VatUniqueTypeDetails()
            {    
                Code = "UFA", 
                Name = "Unique for all countries", 
                SearchFields = "UFA,Unique for all countries", 
			});
			
            return all;
       }

	    public void MapPoco(VatUniqueType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(VatUniqueType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

