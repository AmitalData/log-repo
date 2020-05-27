
   
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
   public class AddressPurposeDetails : AddressPurpose, ICloseTable<AddressPurpose, AddressPurposeDetails>
   {
       public List<AddressPurposeDetails> GetAll()
       {
		    var all = new List<AddressPurposeDetails>();  
            all.Add(new AddressPurposeDetails()
            {    
                Code = "1", 
                SearchFields = "1,חזקה משפטית", 
                Inactive = false, 
                LocalName = "חזקה משפטית", 
			});
			 
            all.Add(new AddressPurposeDetails()
            {    
                Code = "10", 
                SearchFields = "10,נשגר", 
                Inactive = false, 
                LocalName = "נשגר", 
			});
			 
            all.Add(new AddressPurposeDetails()
            {    
                Code = "11", 
                SearchFields = "11,מיודע", 
                Inactive = false, 
                LocalName = "מיודע", 
			});
			 
            all.Add(new AddressPurposeDetails()
            {    
                Code = "12", 
                SearchFields = "12,לחומרים מסוכנים", 
                Inactive = false, 
                LocalName = "לחומרים מסוכנים", 
			});
			 
            all.Add(new AddressPurposeDetails()
            {    
                Code = "13", 
                SearchFields = "13,לבלדרות", 
                Inactive = false, 
                LocalName = "לבלדרות", 
			});
			 
            all.Add(new AddressPurposeDetails()
            {    
                Code = "14", 
                SearchFields = "14,יבוא אישי", 
                Inactive = false, 
                LocalName = "יבוא אישי", 
			});
			 
            all.Add(new AddressPurposeDetails()
            {    
                Code = "15", 
                SearchFields = "15,גרעונות", 
                Inactive = false, 
                LocalName = "גרעונות", 
			});
			 
            all.Add(new AddressPurposeDetails()
            {    
                Code = "16", 
                SearchFields = "16,מערכות מידע", 
                Inactive = false, 
                LocalName = "מערכות מידע", 
			});
			 
            all.Add(new AddressPurposeDetails()
            {    
                Code = "2", 
                SearchFields = "2,כללי", 
                Inactive = false, 
                LocalName = "כללי", 
			});
			 
            all.Add(new AddressPurposeDetails()
            {    
                Code = "3", 
                SearchFields = "3,פט''מ", 
                Inactive = false, 
                LocalName = "פט''מ", 
			});
			 
            all.Add(new AddressPurposeDetails()
            {    
                Code = "4", 
                SearchFields = "4,הישבון", 
                Inactive = false, 
                LocalName = "הישבון", 
			});
			 
            all.Add(new AddressPurposeDetails()
            {    
                Code = "5", 
                SearchFields = "5,תביעות", 
                Inactive = false, 
                LocalName = "תביעות", 
			});
			 
            all.Add(new AddressPurposeDetails()
            {    
                Code = "6", 
                SearchFields = "6,פקדונות", 
                Inactive = false, 
                LocalName = "פקדונות", 
			});
			 
            all.Add(new AddressPurposeDetails()
            {    
                Code = "7", 
                SearchFields = "7,ערבויות", 
                Inactive = false, 
                LocalName = "ערבויות", 
			});
			 
            all.Add(new AddressPurposeDetails()
            {    
                Code = "8", 
                SearchFields = "8,פרה רולינג", 
                Inactive = false, 
                LocalName = "פרה רולינג", 
			});
			 
            all.Add(new AddressPurposeDetails()
            {    
                Code = "9", 
                SearchFields = "9,שוגר", 
                Inactive = false, 
                LocalName = "שוגר", 
			});
			
            return all;
       }

	    public void MapPoco(AddressPurpose newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(AddressPurpose rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

