

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
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityPMs; 
using Simplog.Data.InvoiceModel;

namespace Logitude.BL.InvoiceModel
{
   public class SATInterfaceDetails : SATInterface, ICloseTable<SATInterface, SATInterfaceDetails>
   {
       public List<SATInterfaceDetails> GetAll()
       {
		    var all = new List<SATInterfaceDetails>();  
            all.Add(new SATInterfaceDetails()
            {    
                Code = "CONT", 
                SearchFields = "cont,contpaq", 
                Name = "Contpaq", 
			});
			 
            all.Add(new SATInterfaceDetails()
            {    
                Code = "NONE", 
                SearchFields = "none,none", 
                Name = "None", 
			});
			 
            all.Add(new SATInterfaceDetails()
            {    
                Code = "PROF", 
                SearchFields = "prof,profact 3.2", 
                Name = "Profact 3.2", 
			});
			 
            all.Add(new SATInterfaceDetails()
            {    
                Code = "PROF33", 
                SearchFields = "prof33,profact 3.3", 
                Name = "Profact 3.3", 
			});
			
            return all;
       }

	    public void MapPoco(SATInterface newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(SATInterface rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

