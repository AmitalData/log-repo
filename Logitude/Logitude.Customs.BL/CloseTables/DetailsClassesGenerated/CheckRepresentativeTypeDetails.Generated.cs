
   
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
   public class CheckRepresentativeTypeDetails : CheckRepresentativeType, ICloseTable<CheckRepresentativeType, CheckRepresentativeTypeDetails>
   {
       public List<CheckRepresentativeTypeDetails> GetAll()
       {
		    var all = new List<CheckRepresentativeTypeDetails>(); 
            return all;
       }

	    public void MapPoco(CheckRepresentativeType newPoco)
        {    
        }

		public string GetSearchFields(CheckRepresentativeType rec)
        {   
           return string.Empty;
        }
   }
}

