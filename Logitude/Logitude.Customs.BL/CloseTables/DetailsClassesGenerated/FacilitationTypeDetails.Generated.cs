
   
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
   public class FacilitationTypeDetails : FacilitationType, ICloseTable<FacilitationType, FacilitationTypeDetails>
   {
       public List<FacilitationTypeDetails> GetAll()
       {
		    var all = new List<FacilitationTypeDetails>();  
            all.Add(new FacilitationTypeDetails()
            {    
                Code = "1", 
                LocalName = "AEO", 
                EnglishName = "A- EO", 
                Inactive = false, 
                SearchFields = "A- EO,1,AEO", 
			});
			 
            all.Add(new FacilitationTypeDetails()
            {    
                Inactive = false, 
                Code = "2", 
                LocalName = "יבואן מאושר", 
                EnglishName = "Approved Importer", 
                SearchFields = "Approved Importer,יבואן מאושר,2", 
			});
			
            return all;
       }

	    public void MapPoco(FacilitationType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.LocalName = this.LocalName;  
		    newPoco.EnglishName = this.EnglishName;  
		    newPoco.Inactive = this.Inactive;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(FacilitationType rec)
        {   
           return String.Concat(rec.Code,",",rec.LocalName,",",rec.EnglishName,",",rec.Inactive,",");
        }
   }
}

