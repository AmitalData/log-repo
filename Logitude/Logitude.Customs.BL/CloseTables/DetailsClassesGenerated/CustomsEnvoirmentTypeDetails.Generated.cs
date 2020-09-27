
   
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
   public class CustomsEnvoirmentTypeDetails : CustomsEnvoirmentType, ICloseTable<CustomsEnvoirmentType, CustomsEnvoirmentTypeDetails>
   {
       public List<CustomsEnvoirmentTypeDetails> GetAll()
       {
		    var all = new List<CustomsEnvoirmentTypeDetails>();  
            all.Add(new CustomsEnvoirmentTypeDetails()
            {    
                Code = "1", 
                EnglishName = "PrePilot", 
                SearchFields = "1,prepilot,פיילוט רשות", 
                Inactive = false, 
                LocalName = "פיילוט רשות", 
			});
			 
            all.Add(new CustomsEnvoirmentTypeDetails()
            {    
                Code = "2", 
                EnglishName = "Pilot", 
                SearchFields = "2,pilot,פיילוט חובה", 
                Inactive = false, 
                LocalName = "פיילוט חובה", 
			});
			 
            all.Add(new CustomsEnvoirmentTypeDetails()
            {    
                Code = "3", 
                EnglishName = "Production", 
                SearchFields = "3,production,ייצור", 
                Inactive = false, 
                LocalName = "ייצור", 
			});
			 
            all.Add(new CustomsEnvoirmentTypeDetails()
            {    
                Code = "4", 
                EnglishName = "Test", 
                SearchFields = "4,test,בדיקות", 
                Inactive = false, 
                LocalName = "בדיקות", 
			});
			 
            all.Add(new CustomsEnvoirmentTypeDetails()
            {    
                Code = "5", 
                EnglishName = "PRE Production", 
                SearchFields = "5,pre production,קדם יצור", 
                Inactive = false, 
                LocalName = "קדם יצור", 
			});
			
            return all;
       }

	    public void MapPoco(CustomsEnvoirmentType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.EnglishName = this.EnglishName;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(CustomsEnvoirmentType rec)
        {   
           return String.Concat(rec.Code,",",rec.EnglishName,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

