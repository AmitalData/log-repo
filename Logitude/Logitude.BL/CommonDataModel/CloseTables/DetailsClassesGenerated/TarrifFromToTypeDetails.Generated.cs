

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
   public class TarrifFromToTypeDetails : TarrifFromToType, ICloseTable<TarrifFromToType, TarrifFromToTypeDetails>
   {
       public List<TarrifFromToTypeDetails> GetAll()
       {
		    var all = new List<TarrifFromToTypeDetails>();  
            all.Add(new TarrifFromToTypeDetails()
            {    
                Code = "T", 
                Name = "To", 
			});
			 
            all.Add(new TarrifFromToTypeDetails()
            {    
                Code = "F", 
                Name = "From", 
			});
			
            return all;
       }

	    public void MapPoco(TarrifFromToType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(TarrifFromToType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

