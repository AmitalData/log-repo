
   
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
   public class BIReportsTypeDetails : BIReportsType, ICloseTable<BIReportsType, BIReportsTypeDetails>
   {
       public List<BIReportsTypeDetails> GetAll()
       {
		    var all = new List<BIReportsTypeDetails>();  
            all.Add(new BIReportsTypeDetails()
            {    
                Name = "Excel", 
                Code = "EXL", 
                SearchFields = "EXL,Excel", 
			});
			 
            all.Add(new BIReportsTypeDetails()
            {    
                Name = "PDF", 
                Code = "PDF", 
                SearchFields = "PDF", 
			});
			
            return all;
       }

	    public void MapPoco(BIReportsType newPoco)
        {   
		    newPoco.Name = this.Name;  
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(BIReportsType rec)
        {   
           return String.Concat(rec.Name,",",rec.Code,",");
        }
   }
}

