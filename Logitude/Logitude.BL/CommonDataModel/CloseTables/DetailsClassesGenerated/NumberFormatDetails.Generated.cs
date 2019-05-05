

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
                Name = "Comma Dot 1,234.50 ", 
                SearchFields = "CD,Comma Dot 1,234.50", 
			});
			 
            all.Add(new NumberFormatDetails()
            {    
                Code = "DC", 
                Name = "Dot Comma 1.234,50", 
                SearchFields = "DC,Dot Comma 1.234,50", 
			});
			 
            all.Add(new NumberFormatDetails()
            {    
                Code = "AD", 
                Name = "Apostrophe Dot 1'234.50 ", 
                SearchFields = "AD,Apostrophe Dot 1'234.50", 
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

