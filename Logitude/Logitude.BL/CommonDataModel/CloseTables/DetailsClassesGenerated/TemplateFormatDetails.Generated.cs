

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
   public class TemplateFormatDetails : TemplateFormat, ICloseTable<TemplateFormat, TemplateFormatDetails>
   {
       public List<TemplateFormatDetails> GetAll()
       {
		    var all = new List<TemplateFormatDetails>();  
            all.Add(new TemplateFormatDetails()
            {    
                SearchFields = "m,message", 
                Code = "M", 
                Name = "Message", 
			});
			 
            all.Add(new TemplateFormatDetails()
            {    
                SearchFields = "p,print", 
                Code = "P", 
                Name = "Print", 
			});
			
            return all;
       }

	    public void MapPoco(TemplateFormat newPoco)
        {   
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(TemplateFormat rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

