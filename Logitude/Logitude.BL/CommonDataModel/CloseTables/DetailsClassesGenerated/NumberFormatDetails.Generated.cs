

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
   public class NumberFormatDetails : NumberFormat, ICloseTable<NumberFormat, NumberFormatDetails>
   {
       public List<NumberFormatDetails> GetAll()
       {
		    var all = new List<NumberFormatDetails>();  
            all.Add(new NumberFormatDetails()
            {    
                Code = "CD", 
                Name = "Comma Dot", 
                SearchFields = "CD,Comma Dot", 
			});
			 
            all.Add(new NumberFormatDetails()
            {    
                Code = "DC", 
                Name = "Dot Comma", 
                SearchFields = "DC,Dot Comma", 
			});
			 
            all.Add(new NumberFormatDetails()
            {    
                Code = "AD", 
                Name = "Apostrophe Dot", 
                SearchFields = "AD,Apostrophe Dot", 
			});
			
            return all;
       }

	    public void MapPoco(NumberFormat newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(NumberFormat rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

