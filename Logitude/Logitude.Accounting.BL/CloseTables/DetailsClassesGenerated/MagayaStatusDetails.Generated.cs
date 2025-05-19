
   
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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL
{
   public class MagayaStatusDetails : MagayaStatus, ICloseTable<MagayaStatus, MagayaStatusDetails>
   {
       public List<MagayaStatusDetails> GetAll()
       {
		    var all = new List<MagayaStatusDetails>();  
            all.Add(new MagayaStatusDetails()
            {    
                StatusCode = "1", 
                StatusName = "Created", 
			});
			 
            all.Add(new MagayaStatusDetails()
            {    
                StatusCode = "2", 
                StatusName = "Pending", 
			});
			 
            all.Add(new MagayaStatusDetails()
            {    
                StatusCode = "3", 
                StatusName = "In Progress", 
			});
			 
            all.Add(new MagayaStatusDetails()
            {    
                StatusCode = "4", 
                StatusName = "Done", 
			});
			
            return all;
       }

	    public void MapPoco(MagayaStatus newPoco)
        {   
		    newPoco.StatusCode = this.StatusCode;  
		    newPoco.StatusName = this.StatusName;   
        }

		public string GetSearchFields(MagayaStatus rec)
        {   
           return String.Concat(rec.StatusCode,",",rec.StatusName,",");
        }
		public string Code
        {
            get
            {
                return StatusCode;//throw new NotImplementedException();
            }
            set
            {
                StatusCode = value;//throw new NotImplementedException();
            }
        }
   }
}

