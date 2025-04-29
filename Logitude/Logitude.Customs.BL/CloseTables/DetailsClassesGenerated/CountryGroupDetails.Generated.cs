
   
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
   public class CountryGroupDetails : CountryGroup, ICloseTable<CountryGroup, CountryGroupDetails>
   {
       public List<CountryGroupDetails> GetAll()
       {
		    var all = new List<CountryGroupDetails>();  
            all.Add(new CountryGroupDetails()
            {    
                Code = "1", 
                SearchFields = "1,כללי", 
                Inactive = false, 
                LocalName = "כללי", 
                EnglishName = "general", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "10", 
                SearchFields = "10,סלובקיה", 
                Inactive = false, 
                LocalName = "סלובקיה", 
                EnglishName = "Slovakia", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "11", 
                SearchFields = "11,הונגריה", 
                Inactive = false, 
                LocalName = "הונגריה", 
                EnglishName = "Hungary", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "12", 
                SearchFields = "12,פולין", 
                Inactive = false, 
                LocalName = "פולין", 
                EnglishName = "Poland", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "13", 
                SearchFields = "13,סלובניה", 
                Inactive = false, 
                LocalName = "סלובניה", 
                EnglishName = "Slovenia", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "14", 
                SearchFields = "14,מקסיקו", 
                Inactive = false, 
                LocalName = "מקסיקו", 
                EnglishName = "Mexico", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "15", 
                SearchFields = "15,רומניה", 
                Inactive = false, 
                LocalName = "רומניה", 
                EnglishName = "Rumania", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "16", 
                SearchFields = "16,בולגריה", 
                Inactive = false, 
                LocalName = "בולגריה", 
                EnglishName = "Bulgaria", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "17", 
                SearchFields = "17,איחוד", 
                Inactive = false, 
                LocalName = "איחוד", 
                EnglishName = "ihud", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "18", 
                SearchFields = "18,WTO", 
                Inactive = false, 
                LocalName = "WTO", 
                EnglishName = "WTO", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "19", 
                SearchFields = "19,ברזיל", 
                Inactive = false, 
                LocalName = "ברזיל", 
                EnglishName = "Brazil", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "2", 
                SearchFields = "2,איחוד מכסה", 
                Inactive = false, 
                LocalName = "איחוד מכסה", 
                EnglishName = "Union Quota", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "20", 
                SearchFields = "20,אורוגוואי", 
                Inactive = false, 
                LocalName = "אורוגוואי", 
                EnglishName = "Uruguay", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "21", 
                SearchFields = "21,פרגוואי", 
                Inactive = false, 
                LocalName = "פרגוואי", 
                EnglishName = "Paraguay", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "22", 
                SearchFields = "22,ארגנטינה", 
                Inactive = false, 
                LocalName = "ארגנטינה", 
                EnglishName = "Argentina", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "23", 
                SearchFields = "23,מדינות להסכם בנושא יצואן מאושר לתעודות מקור", 
                Inactive = false, 
                LocalName = "מדינות להסכם בנושא יצואן מאושר לתעודות מקור", 
                EnglishName = "Countries to agree on approved exporter for certificates of origin", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "24", 
                SearchFields = "24,מדינות שלא מתאימות להישבון", 
                Inactive = false, 
                LocalName = "מדינות שלא מתאימות להישבון", 
                EnglishName = "Countries not suitable for settlement", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "25", 
                SearchFields = "25,מדינות קרנה", 
                Inactive = false, 
                LocalName = "מדינות קרנה", 
                EnglishName = "Karana countries", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "26", 
                SearchFields = "26,מדינות רווחה", 
                Inactive = false, 
                LocalName = "מדינות רווחה", 
                EnglishName = "Welfare States", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "27", 
                SearchFields = "27,כל העולם", 
                Inactive = false, 
                LocalName = "כל העולם", 
                EnglishName = "The whole world", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "28", 
                SearchFields = "28,האיחוד האירופי", 
                Inactive = false, 
                LocalName = "האיחוד האירופי", 
                EnglishName = "European Union", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "29", 
                SearchFields = "29,מרקוסור", 
                Inactive = false, 
                LocalName = "מרקוסור", 
                EnglishName = "Mercosur", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "3", 
                SearchFields = "3,ארה''ב", 
                Inactive = false, 
                LocalName = "ארה''ב", 
                EnglishName = "U_S", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "4", 
                SearchFields = "4,ארצות ערב", 
                Inactive = false, 
                LocalName = "ארצות ערב", 
                EnglishName = "Arab countries", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "5", 
                SearchFields = "5,ירדן", 
                Inactive = false, 
                LocalName = "ירדן", 
                EnglishName = "Jordan", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "6", 
                SearchFields = "6,קנדה", 
                Inactive = false, 
                LocalName = "קנדה", 
                EnglishName = "Canada", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "7", 
                SearchFields = "7,אפט''א", 
                Inactive = false, 
                LocalName = "אפט''א", 
                EnglishName = "Afta", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "8", 
                SearchFields = "8,טורקיה", 
                Inactive = false, 
                LocalName = "טורקיה", 
                EnglishName = "Turkey", 
			});
			 
            all.Add(new CountryGroupDetails()
            {    
                Code = "9", 
                SearchFields = "9,צכיה", 
                Inactive = false, 
                LocalName = "צכיה", 
                EnglishName = "Czech Republic", 
			});
			
            return all;
       }

	    public void MapPoco(CountryGroup newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;  
		    newPoco.EnglishName = this.EnglishName;   
        }

		public string GetSearchFields(CountryGroup rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",",rec.EnglishName,",");
        }
   }
}

