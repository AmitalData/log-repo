
   
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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL
{
   public class FclLclCodeDetails : FclLclCode, ICloseTable<FclLclCode, FclLclCodeDetails>
   {
       public List<FclLclCodeDetails> GetAll()
       {
		    var all = new List<FclLclCodeDetails>();  
            all.Add(new FclLclCodeDetails()
            {    
                Code = "F", 
                Name = "FCL", 
                SearchFields = "f,fcl", 
			});
			 
            all.Add(new FclLclCodeDetails()
            {    
                Code = "L", 
                Name = "LCL", 
                SearchFields = "l,lcl", 
			});
			
            return all;
       }

	    public void MapPoco(FclLclCode newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(FclLclCode rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

