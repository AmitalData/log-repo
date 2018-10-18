

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
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityPMs; 
using Simplog.Data.ShipmentsModel;

namespace Logitude.BL.ShipmentsModel
{
   public class OBLTypeDetails : OBLType, ICloseTable<OBLType, OBLTypeDetails>
   {
       public List<OBLTypeDetails> GetAll()
       {
		    var all = new List<OBLTypeDetails>();  
            all.Add(new OBLTypeDetails()
            {    
                Code = "EXPR", 
                SearchFields = "EXPR,Express Release", 
                Name = "Express Release", 
			});
			 
            all.Add(new OBLTypeDetails()
            {    
                Code = "OBLR", 
                SearchFields = "OBLR,OBL Required", 
                Name = "OBL Required", 
			});
			
            return all;
       }

	    public void MapPoco(OBLType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(OBLType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

