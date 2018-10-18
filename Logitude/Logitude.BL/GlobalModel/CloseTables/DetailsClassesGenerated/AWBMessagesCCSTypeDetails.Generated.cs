

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
   public class AWBMessagesCCSTypeDetails : AWBMessagesCCSType, ICloseTable<AWBMessagesCCSType, AWBMessagesCCSTypeDetails>
   {
       public List<AWBMessagesCCSTypeDetails> GetAll()
       {
		    var all = new List<AWBMessagesCCSTypeDetails>();  
            all.Add(new AWBMessagesCCSTypeDetails()
            {    
                Code = "CHAMP", 
                SearchFields = "CHAMP,Champ", 
                Name = "Champ", 
			});
			 
            all.Add(new AWBMessagesCCSTypeDetails()
            {    
                Code = "GLSHK", 
                SearchFields = "GLSHK,GLSHK", 
                Name = "GLSHK", 
			});
			
            return all;
       }

	    public void MapPoco(AWBMessagesCCSType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(AWBMessagesCCSType rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

