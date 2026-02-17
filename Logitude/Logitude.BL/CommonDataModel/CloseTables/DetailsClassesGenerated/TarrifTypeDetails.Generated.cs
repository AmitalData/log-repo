

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
   public class TarrifTypeDetails : TarrifType, ICloseTable<TarrifType, TarrifTypeDetails>
   {
       public List<TarrifTypeDetails> GetAll()
       {
		    var all = new List<TarrifTypeDetails>();  
            all.Add(new TarrifTypeDetails()
            {    
                Code = "S", 
                Name = "Surcharges", 
			});
			 
            all.Add(new TarrifTypeDetails()
            {    
                Code = "F", 
                Name = "Freight", 
			});
			
            return all;
       }

	    public void MapPoco(TarrifType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(TarrifType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

