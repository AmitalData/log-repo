

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
   public class CustomsInterfaceDetails : CustomsInterface, ICloseTable<CustomsInterface, CustomsInterfaceDetails>
   {
       public List<CustomsInterfaceDetails> GetAll()
       {
		    var all = new List<CustomsInterfaceDetails>();  
            all.Add(new CustomsInterfaceDetails()
            {    
                Code = "ABM", 
                SearchFields = "ABM,ABM CustomsWare", 
                InterfaceType = "LO", 
                Name = "ABM CustomsWare", 
			});
			 
            all.Add(new CustomsInterfaceDetails()
            {    
                Code = "ART", 
                SearchFields = "ART,Artemus", 
                InterfaceType = "IM", 
                Name = "Artemus", 
			});
			 
            all.Add(new CustomsInterfaceDetails()
            {    
                Code = "CBP", 
                SearchFields = "CBP,CBP direct", 
                InterfaceType = "EX", 
                Name = "CBP direct", 
			});
			 
            all.Add(new CustomsInterfaceDetails()
            {    
                Code = "CMN", 
                SearchFields = "CMN,Maman Courier", 
                InterfaceType = "IM", 
                Name = "Maman Courier", 
			});
			 
            all.Add(new CustomsInterfaceDetails()
            {    
                Code = "NO", 
                SearchFields = "NO,None", 
                InterfaceType = "NO", 
                Name = "None", 
			});
			
            return all;
       }

	    public void MapPoco(CustomsInterface newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.InterfaceType = this.InterfaceType;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(CustomsInterface rec)
        {   
           return String.Concat(rec.Code,",",rec.InterfaceType,",",rec.Name,",");
        }
   }
}

