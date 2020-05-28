
   
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
   public class AssigneeNotificationTypeDetails : AssigneeNotificationType, ICloseTable<AssigneeNotificationType, AssigneeNotificationTypeDetails>
   {
       public List<AssigneeNotificationTypeDetails> GetAll()
       {
		    var all = new List<AssigneeNotificationTypeDetails>();  
            all.Add(new AssigneeNotificationTypeDetails()
            {    
                Code = "A", 
                EnglishName = "Action", 
                SearchFields = "a,action,לפעולה", 
                Inactive = false, 
                LocalName = "לפעולה", 
			});
			 
            all.Add(new AssigneeNotificationTypeDetails()
            {    
                Code = "I", 
                EnglishName = "Info", 
                SearchFields = "i,info,לידיעה", 
                Inactive = false, 
                LocalName = "לידיעה", 
			});
			
            return all;
       }

	    public void MapPoco(AssigneeNotificationType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.EnglishName = this.EnglishName;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(AssigneeNotificationType rec)
        {   
           return String.Concat(rec.Code,",",rec.EnglishName,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

