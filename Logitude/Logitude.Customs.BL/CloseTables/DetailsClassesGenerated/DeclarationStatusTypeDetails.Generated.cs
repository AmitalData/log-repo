
   
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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL
{
   public class DeclarationStatusTypeDetails : DeclarationStatusType, ICloseTable<DeclarationStatusType, DeclarationStatusTypeDetails>
   {
       public List<DeclarationStatusTypeDetails> GetAll()
       {
		    var all = new List<DeclarationStatusTypeDetails>(); 
            return all;
       }

	    public void MapPoco(DeclarationStatusType newPoco)
        {    
        }

		public string GetSearchFields(DeclarationStatusType rec)
        {   
           return string.Empty;
        }
   }
}

