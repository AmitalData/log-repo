
   
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
   public class InterfaceSendOptionDetails : InterfaceSendOption, ICloseTable<InterfaceSendOption, InterfaceSendOptionDetails>
   {
       public List<InterfaceSendOptionDetails> GetAll()
       {
		    var all = new List<InterfaceSendOptionDetails>();  
            all.Add(new InterfaceSendOptionDetails()
            {    
                Code = "D", 
                EnglishName = "DCA Out", 
                SearchFields = "d,dca out,כספת", 
                Inactive = false, 
                LocalName = "כספת", 
			});
			 
            all.Add(new InterfaceSendOptionDetails()
            {    
                Code = "WB", 
                EnglishName = "Asynchronous Web Service", 
                SearchFields = "wb,asynchronous web service,שליחה ברקע", 
                Inactive = false, 
                LocalName = "שליחה ברקע", 
			});
			 
            all.Add(new InterfaceSendOptionDetails()
            {    
                Code = "WI", 
                EnglishName = "Interactive Web Service", 
                SearchFields = "wi,interactive web service,אינטרקטיבי", 
                Inactive = false, 
                LocalName = "אינטרקטיבי", 
			});
			
            return all;
       }

	    public void MapPoco(InterfaceSendOption newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.EnglishName = this.EnglishName;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(InterfaceSendOption rec)
        {   
           return String.Concat(rec.Code,",",rec.EnglishName,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

