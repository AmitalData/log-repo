
   
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
   public class PhysicalCheckOperationDetails : PhysicalCheckOperation, ICloseTable<PhysicalCheckOperation, PhysicalCheckOperationDetails>
   {
       public List<PhysicalCheckOperationDetails> GetAll()
       {
		    var all = new List<PhysicalCheckOperationDetails>();  
            all.Add(new PhysicalCheckOperationDetails()
            {    
                Code = "1", 
                SearchFields = "1,invite,זימון", 
                Inactive = false, 
                LocalName = "זימון", 
                EnglishName = "Invite", 
			});
			 
            all.Add(new PhysicalCheckOperationDetails()
            {    
                Code = "2", 
                SearchFields = "2,update,עדכון", 
                Inactive = false, 
                LocalName = "עדכון", 
                EnglishName = "Update", 
			});
			 
            all.Add(new PhysicalCheckOperationDetails()
            {    
                Code = "3", 
                SearchFields = "3,cancel,ביטול", 
                Inactive = false, 
                LocalName = "ביטול", 
                EnglishName = "Cancel", 
			});
			 
            all.Add(new PhysicalCheckOperationDetails()
            {    
                Code = "4", 
                SearchFields = "4,cancel invitation,ביטול יזום", 
                Inactive = false, 
                LocalName = "ביטול יזום", 
                EnglishName = "Cancel Invitation", 
			});
			 
            all.Add(new PhysicalCheckOperationDetails()
            {    
                Code = "5", 
                SearchFields = "5,end - moved to another site,סיום -העברה לאתר אחר", 
                Inactive = false, 
                LocalName = "סיום -העברה לאתר אחר", 
                EnglishName = "End - moved to another site", 
			});
			 
            all.Add(new PhysicalCheckOperationDetails()
            {    
                Code = "6", 
                SearchFields = "6,end - moved to customer,סיום -העברה לאתר אחר", 
                Inactive = false, 
                LocalName = "סיום -העברה לאתר אחר", 
                EnglishName = "End - Moved to Customer", 
			});
			
            return all;
       }

	    public void MapPoco(PhysicalCheckOperation newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;  
		    newPoco.EnglishName = this.EnglishName;   
        }

		public string GetSearchFields(PhysicalCheckOperation rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",",rec.EnglishName,",");
        }
   }
}

