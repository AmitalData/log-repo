
   
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
   public class CustomsAddressTypeDetails : CustomsAddressType, ICloseTable<CustomsAddressType, CustomsAddressTypeDetails>
   {
       public List<CustomsAddressTypeDetails> GetAll()
       {
		    var all = new List<CustomsAddressTypeDetails>();  
            all.Add(new CustomsAddressTypeDetails()
            {    
                Code = "1", 
                SearchFields = "1,עסק", 
                Inactive = false, 
                LocalName = "עסק", 
			});
			 
            all.Add(new CustomsAddressTypeDetails()
            {    
                Code = "10", 
                SearchFields = "10,איש קשר למטען", 
                Inactive = false, 
                LocalName = "איש קשר למטען", 
			});
			 
            all.Add(new CustomsAddressTypeDetails()
            {    
                Code = "11", 
                SearchFields = "11,שע''ם/מרשם", 
                Inactive = false, 
                LocalName = "שע''ם/מרשם", 
			});
			 
            all.Add(new CustomsAddressTypeDetails()
            {    
                Code = "12", 
                SearchFields = "12,שע''ם/מרשם - דואר", 
                Inactive = false, 
                LocalName = "שע''ם/מרשם - דואר", 
			});
			 
            all.Add(new CustomsAddressTypeDetails()
            {    
                Code = "13", 
                SearchFields = "13,משלוח", 
                Inactive = false, 
                LocalName = "משלוח", 
			});
			 
            all.Add(new CustomsAddressTypeDetails()
            {    
                Code = "2", 
                SearchFields = "2,פרטית", 
                Inactive = false, 
                LocalName = "פרטית", 
			});
			 
            all.Add(new CustomsAddressTypeDetails()
            {    
                Code = "3", 
                SearchFields = "3,התכתבות", 
                Inactive = false, 
                LocalName = "התכתבות", 
			});
			 
            all.Add(new CustomsAddressTypeDetails()
            {    
                Code = "4", 
                SearchFields = "4,סניף / מפעל / מחסן", 
                Inactive = false, 
                LocalName = "סניף / מפעל / מחסן", 
			});
			 
            all.Add(new CustomsAddressTypeDetails()
            {    
                Code = "5", 
                SearchFields = "5,איש קשר", 
                Inactive = false, 
                LocalName = "איש קשר", 
			});
			 
            all.Add(new CustomsAddressTypeDetails()
            {    
                Code = "7", 
                SearchFields = "7,שע''ם/מע''ם - עסק", 
                Inactive = false, 
                LocalName = "שע''ם/מע''ם - עסק", 
			});
			 
            all.Add(new CustomsAddressTypeDetails()
            {    
                Code = "8", 
                SearchFields = "8,שע''ם/מע''ם - פרטית", 
                Inactive = false, 
                LocalName = "שע''ם/מע''ם - פרטית", 
			});
			 
            all.Add(new CustomsAddressTypeDetails()
            {    
                Code = "9", 
                SearchFields = "9,שע''ם/מע''ם - התכתבות", 
                Inactive = false, 
                LocalName = "שע''ם/מע''ם - התכתבות", 
			});
			 
            all.Add(new CustomsAddressTypeDetails()
            {    
                Code = "99", 
                SearchFields = "99,שע''ם/MTEL", 
                Inactive = false, 
                LocalName = "שע''ם/MTEL", 
			});
			
            return all;
       }

	    public void MapPoco(CustomsAddressType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(CustomsAddressType rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

