

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
			 
            all.Add(new OBLTypeDetails()
            {    
                Code = "SEWY", 
                Name = "Seaway", 
                SearchFields = "SEWY,Seaway", 
			});
			 
            all.Add(new OBLTypeDetails()
            {    
                Code = "BLSR", 
                Name = "BL Surrender", 
                SearchFields = "BLSR,BL Surrender", 
			});
			 
            all.Add(new OBLTypeDetails()
            {    
                Code = "AGOR", 
                Name = "Against Original", 
                SearchFields = "AGOR,Against Original", 
			});
			 
            all.Add(new OBLTypeDetails()
            {    
                Code = "SEWA", 
                Name = "Sea Waybill", 
                SearchFields = "SEWA,Sea Waybill", 
			});
			 
            all.Add(new OBLTypeDetails()
            {    
                Code = "BLEB", 
                Name = "BL Endorsed By Bank", 
                SearchFields = "BLEB,BL Endorsed By Bank", 
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

