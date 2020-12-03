
   
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
   public class CurrencyTypeDetails : CurrencyType, ICloseTable<CurrencyType, CurrencyTypeDetails>
   {
       public List<CurrencyTypeDetails> GetAll()
       {
		    var all = new List<CurrencyTypeDetails>();  
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "AUD", 
                SearchFields = "AUD,דולר אוסטרלי", 
                Inactive = false, 
                LocalName = "דולר אוסטרלי", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "BGN", 
                SearchFields = "BGN,לב בולגרי", 
                Inactive = false, 
                LocalName = "לב בולגרי", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "BRL", 
                SearchFields = "BRL,ריאל ברזיל", 
                Inactive = false, 
                LocalName = "ריאל ברזיל", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "BYR", 
                SearchFields = "BYR,רובל בילרוסי", 
                Inactive = false, 
                LocalName = "רובל בילרוסי", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "CAD", 
                SearchFields = "CAD,דולר קנדי", 
                Inactive = false, 
                LocalName = "דולר קנדי", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "CHF", 
                SearchFields = "CHF,פרנק שויצרי", 
                Inactive = false, 
                LocalName = "פרנק שויצרי", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "CLP", 
                SearchFields = "CLP,פזו צ'יליאני", 
                Inactive = false, 
                LocalName = "פזו צ'יליאני", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "CNY", 
                SearchFields = "CNY,יואן סיני", 
                Inactive = false, 
                LocalName = "יואן סיני", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "CZK", 
                SearchFields = "CZK,כתר צ'כי", 
                Inactive = false, 
                LocalName = "כתר צ'כי", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "DKK", 
                SearchFields = "DKK,כתר דני", 
                Inactive = false, 
                LocalName = "כתר דני", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "EGP", 
                SearchFields = "EGP,לירה מצרית", 
                Inactive = false, 
                LocalName = "לירה מצרית", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "EUR", 
                SearchFields = "EUR,אירו", 
                Inactive = false, 
                LocalName = "אירו", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "GBP", 
                SearchFields = "GBP,לירה שטרלינג", 
                Inactive = false, 
                LocalName = "לירה שטרלינג", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "HKD", 
                SearchFields = "HKD,דולר הונג-קונג", 
                Inactive = false, 
                LocalName = "דולר הונג-קונג", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "HUF", 
                SearchFields = "HUF,פורינט הונגרי", 
                Inactive = false, 
                LocalName = "פורינט הונגרי", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "ILS", 
                SearchFields = "ILS,שקל ישראלי חדש", 
                Inactive = false, 
                LocalName = "שקל ישראלי חדש", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "INR", 
                SearchFields = "INR,רופיה הודית", 
                Inactive = false, 
                LocalName = "רופיה הודית", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "ISK", 
                SearchFields = "ISK,כתר איסלנדי", 
                Inactive = false, 
                LocalName = "כתר איסלנדי", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "JOD", 
                SearchFields = "JOD,דינר ירדני", 
                Inactive = false, 
                LocalName = "דינר ירדני", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "JPY", 
                SearchFields = "JPY,יין יפני", 
                Inactive = false, 
                LocalName = "יין יפני", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "KRW", 
                SearchFields = "KRW,ואן דרום קוריאה", 
                Inactive = false, 
                LocalName = "ואן דרום קוריאה", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "LBP", 
                SearchFields = "LBP,לירה לבנונית", 
                Inactive = false, 
                LocalName = "לירה לבנונית", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "LTL", 
                SearchFields = "LTL,ליטאס ליטאי", 
                Inactive = false, 
                LocalName = "ליטאס ליטאי", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "LVL", 
                SearchFields = "LVL,לטס לטבי", 
                Inactive = false, 
                LocalName = "לטס לטבי", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "MXN", 
                SearchFields = "MXN,פזו מקסיקני", 
                Inactive = false, 
                LocalName = "פזו מקסיקני", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "NOK", 
                SearchFields = "NOK,כתר נורבגי", 
                Inactive = false, 
                LocalName = "כתר נורבגי", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "NPR", 
                SearchFields = "NPR,רופי נפאלי", 
                Inactive = false, 
                LocalName = "רופי נפאלי", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "NZD", 
                SearchFields = "NZD,דולר ניו זילנדי", 
                Inactive = false, 
                LocalName = "דולר ניו זילנדי", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "PHP", 
                SearchFields = "PHP,פזו פיליפיני", 
                Inactive = false, 
                LocalName = "פזו פיליפיני", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "PLN", 
                SearchFields = "PLN,זלוטי פולין", 
                Inactive = false, 
                LocalName = "זלוטי פולין", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "RON", 
                SearchFields = "RON,ניו לאוי רומניה", 
                Inactive = false, 
                LocalName = "ניו לאוי רומניה", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "RUB", 
                SearchFields = "RUB,רובל רוסי", 
                Inactive = false, 
                LocalName = "רובל רוסי", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "SEK", 
                SearchFields = "SEK,קורונה שוודי", 
                Inactive = false, 
                LocalName = "קורונה שוודי", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "SGD", 
                SearchFields = "SGD,דולר סינגפור", 
                Inactive = false, 
                LocalName = "דולר סינגפור", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "THB", 
                SearchFields = "THB,באט תאילנד", 
                Inactive = false, 
                LocalName = "באט תאילנד", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "TRY", 
                SearchFields = "TRY,לירה טורקית חדשה", 
                Inactive = false, 
                LocalName = "לירה טורקית חדשה", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "TWD", 
                SearchFields = "TWD,דולר טיוואני", 
                Inactive = false, 
                LocalName = "דולר טיוואני", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "UAH", 
                SearchFields = "UAH,הריבניה אוקריאנה", 
                Inactive = false, 
                LocalName = "הריבניה אוקריאנה", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "USD", 
                SearchFields = "USD,דולר ארה''ב", 
                Inactive = false, 
                LocalName = "דולר ארה''ב", 
			});
			 
            all.Add(new CurrencyTypeDetails()
            {    
                Code = "ZAR", 
                SearchFields = "ZAR,רנד ד. אפריקה", 
                Inactive = false, 
                LocalName = "רנד ד. אפריקה", 
			});
			
            return all;
       }

	    public void MapPoco(CurrencyType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(CurrencyType rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

