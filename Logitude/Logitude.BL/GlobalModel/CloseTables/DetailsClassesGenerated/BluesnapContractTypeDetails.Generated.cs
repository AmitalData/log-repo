

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
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.GlobalModel.EntityPMs; 
using Simplog.Global.Data.GlobalModel;

namespace Logitude.BL.GlobalModel
{
   public class BluesnapContractTypeDetails : BluesnapContractType, ICloseTable<BluesnapContractType, BluesnapContractTypeDetails>
   {
       public List<BluesnapContractTypeDetails> GetAll()
       {
		    var all = new List<BluesnapContractTypeDetails>();  
            all.Add(new BluesnapContractTypeDetails()
            {    
                Code = "BA", 
                Name = "Basic", 
                SearchFields = "BA,Basic", 
			});
			 
            all.Add(new BluesnapContractTypeDetails()
            {    
                Code = "CRM", 
                Name = "CRM", 
                SearchFields = "CRM,CRM", 
			});
			 
            all.Add(new BluesnapContractTypeDetails()
            {    
                Code = "EAWB", 
                Name = "e-AWB", 
                SearchFields = "EAWB,e-AWB", 
			});
			 
            all.Add(new BluesnapContractTypeDetails()
            {    
                Code = "OT", 
                Name = "One Time", 
                SearchFields = "OT,One Time", 
			});
			 
            all.Add(new BluesnapContractTypeDetails()
            {    
                SearchFields = "EABS,e-AWB stock", 
                Code = "EABS", 
                Name = "e-AWB stock", 
			});
			 
            all.Add(new BluesnapContractTypeDetails()
            {    
                Name = "INTTRA stock", 
                Code = "INTS", 
                SearchFields = "INTS,INTTRA stock", 
			});
			
            return all;
       }

	    public void MapPoco(BluesnapContractType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(BluesnapContractType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

