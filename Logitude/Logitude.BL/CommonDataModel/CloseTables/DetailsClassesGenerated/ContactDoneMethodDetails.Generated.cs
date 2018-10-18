

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
   public class ContactDoneMethodDetails : ContactDoneMethod, ICloseTable<ContactDoneMethod, ContactDoneMethodDetails>
   {
       public List<ContactDoneMethodDetails> GetAll()
       {
		    var all = new List<ContactDoneMethodDetails>();  
            all.Add(new ContactDoneMethodDetails()
            {    
                Code = "EM", 
                SearchFields = "EM,Email", 
                Name = "Email", 
			});
			 
            all.Add(new ContactDoneMethodDetails()
            {    
                Code = "GT", 
                SearchFields = "GT,Gift", 
                Name = "Gift", 
			});
			 
            all.Add(new ContactDoneMethodDetails()
            {    
                Code = "NO", 
                SearchFields = "NO,None", 
                Name = "None", 
			});
			 
            all.Add(new ContactDoneMethodDetails()
            {    
                Code = "PC", 
                SearchFields = "PC,Phone Call", 
                Name = "Phone Call", 
			});
			 
            all.Add(new ContactDoneMethodDetails()
            {    
                Code = "SM", 
                SearchFields = "SM,Sms", 
                Name = "Sms", 
			});
			
            return all;
       }

	    public void MapPoco(ContactDoneMethod newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(ContactDoneMethod rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

