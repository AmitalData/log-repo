
   
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
   public class CommunicationTypeDetails : CommunicationType, ICloseTable<CommunicationType, CommunicationTypeDetails>
   {
       public List<CommunicationTypeDetails> GetAll()
       {
		    var all = new List<CommunicationTypeDetails>();  
            all.Add(new CommunicationTypeDetails()
            {    
                Code = "1", 
                SearchFields = "1", 
                Inactive = false, 
                LocalName = "111", 
                EnglishName = "One", 
			});
			 
            all.Add(new CommunicationTypeDetails()
            {    
                Code = "AH", 
                SearchFields = "AH,אתר אינטרנט", 
                Inactive = false, 
                LocalName = "אתר אינטרנט", 
                EnglishName = "website", 
			});
			 
            all.Add(new CommunicationTypeDetails()
            {    
                Code = "AL", 
                SearchFields = "AL,נייד", 
                Inactive = false, 
                LocalName = "נייד", 
                EnglishName = "Mobile", 
			});
			 
            all.Add(new CommunicationTypeDetails()
            {    
                Code = "EM", 
                SearchFields = "EM,דואר אלקטרוני", 
                Inactive = false, 
                LocalName = "דואר אלקטרוני", 
                EnglishName = "Email", 
			});
			 
            all.Add(new CommunicationTypeDetails()
            {    
                Code = "FX", 
                SearchFields = "FX,פקס", 
                Inactive = false, 
                LocalName = "פקס", 
                EnglishName = "fax", 
			});
			 
            all.Add(new CommunicationTypeDetails()
            {    
                Code = "TE", 
                SearchFields = "TE,נייח", 
                Inactive = false, 
                LocalName = "נייח", 
                EnglishName = "landline", 
			});
			
            return all;
       }

	    public void MapPoco(CommunicationType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;  
		    newPoco.EnglishName = this.EnglishName;   
        }

		public string GetSearchFields(CommunicationType rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",",rec.EnglishName,",");
        }
   }
}

