

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
   public class CheckDigitControlAlgorithmDetails : CheckDigitControlAlgorithm, ICloseTable<CheckDigitControlAlgorithm, CheckDigitControlAlgorithmDetails>
   {
       public List<CheckDigitControlAlgorithmDetails> GetAll()
       {
		    var all = new List<CheckDigitControlAlgorithmDetails>();  
            all.Add(new CheckDigitControlAlgorithmDetails()
            {    
                Code = "NONE", 
                Name = "None", 
                SearchFields = "NONE, None", 
			});
			 
            all.Add(new CheckDigitControlAlgorithmDetails()
            {    
                Code = "LUHN", 
                Name = "Luhn Algorithm", 
                SearchFields = "LUHN, Luhn Algorithm", 
			});
			
            return all;
       }

	    public void MapPoco(CheckDigitControlAlgorithm newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(CheckDigitControlAlgorithm rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

