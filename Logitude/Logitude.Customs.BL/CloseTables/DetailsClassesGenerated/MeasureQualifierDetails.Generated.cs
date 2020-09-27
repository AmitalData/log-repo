
   
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
   public class MeasureQualifierDetails : MeasureQualifier, ICloseTable<MeasureQualifier, MeasureQualifierDetails>
   {
       public List<MeasureQualifierDetails> GetAll()
       {
		    var all = new List<MeasureQualifierDetails>();  
            all.Add(new MeasureQualifierDetails()
            {    
                Code = "1", 
                SearchFields = "1, כמות יחידות בחשבון,,", 
                Inactive = false, 
                LocalName = " כמות יחידות בחשבון", 
			});
			 
            all.Add(new MeasureQualifierDetails()
            {    
                Code = "2", 
                SearchFields = "2,כמות סטטיסטית,,", 
                Inactive = false, 
                LocalName = "כמות סטטיסטית", 
			});
			 
            all.Add(new MeasureQualifierDetails()
            {    
                Code = "3", 
                SearchFields = "3,כמות נוספת,,", 
                Inactive = false, 
                LocalName = "כמות נוספת", 
			});
			
            return all;
       }

	    public void MapPoco(MeasureQualifier newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(MeasureQualifier rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

