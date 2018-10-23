
   
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
   public class MeasureQualifierDetails : MeasureQualifier, ICloseTable<MeasureQualifier, MeasureQualifierDetails>
   {
       public List<MeasureQualifierDetails> GetAll()
       {
		    var all = new List<MeasureQualifierDetails>(); 
            return all;
       }

	    public void MapPoco(MeasureQualifier newPoco)
        {    
        }

		public string GetSearchFields(MeasureQualifier rec)
        {   
           return string.Empty;
        }
   }
}

