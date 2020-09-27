
   
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
   public class InternalBorderSiteTypeDetails : InternalBorderSiteType, ICloseTable<InternalBorderSiteType, InternalBorderSiteTypeDetails>
   {
       public List<InternalBorderSiteTypeDetails> GetAll()
       {
		    var all = new List<InternalBorderSiteTypeDetails>();  
            all.Add(new InternalBorderSiteTypeDetails()
            {    
                Code = "-1", 
                SearchFields = "-1,סופה", 
                Inactive = true, 
                LocalName = "סופה", 
			});
			 
            all.Add(new InternalBorderSiteTypeDetails()
            {    
                Code = "-16", 
                SearchFields = "-16,חסם צהוב", 
                Inactive = true, 
                LocalName = "חסם צהוב", 
			});
			 
            all.Add(new InternalBorderSiteTypeDetails()
            {    
                Code = "-2", 
                SearchFields = "-2,מזמוריה-הר חומה", 
                Inactive = true, 
                LocalName = "מזמוריה-הר חומה", 
			});
			 
            all.Add(new InternalBorderSiteTypeDetails()
            {    
                Code = "12725", 
                SearchFields = "12725,תרקומיה", 
                Inactive = false, 
                LocalName = "תרקומיה", 
			});
			 
            all.Add(new InternalBorderSiteTypeDetails()
            {    
                Code = "12734", 
                SearchFields = "12734,חסם צהוב", 
                Inactive = false, 
                LocalName = "חסם צהוב", 
			});
			 
            all.Add(new InternalBorderSiteTypeDetails()
            {    
                Code = "139514", 
                SearchFields = "139514,ארז", 
                Inactive = false, 
                LocalName = "ארז", 
			});
			 
            all.Add(new InternalBorderSiteTypeDetails()
            {    
                Code = "139530", 
                SearchFields = "139530,כרם שלום מעבר יבשתי", 
                Inactive = false, 
                LocalName = "כרם שלום מעבר יבשתי", 
			});
			 
            all.Add(new InternalBorderSiteTypeDetails()
            {    
                Code = "139555", 
                SearchFields = "139555,ג'אלמה-גלבוע", 
                Inactive = false, 
                LocalName = "ג'אלמה-גלבוע", 
			});
			 
            all.Add(new InternalBorderSiteTypeDetails()
            {    
                Code = "139563", 
                SearchFields = "139563,שער אפרים", 
                Inactive = false, 
                LocalName = "שער אפרים", 
			});
			 
            all.Add(new InternalBorderSiteTypeDetails()
            {    
                Code = "139571", 
                SearchFields = "139571,ביטוניה-עופר", 
                Inactive = false, 
                LocalName = "ביטוניה-עופר", 
			});
			 
            all.Add(new InternalBorderSiteTypeDetails()
            {    
                Code = "20", 
                SearchFields = "20,קרני", 
                Inactive = true, 
                LocalName = "קרני", 
			});
			 
            all.Add(new InternalBorderSiteTypeDetails()
            {    
                Code = "21", 
                SearchFields = "21,ארז", 
                Inactive = true, 
                LocalName = "ארז", 
			});
			 
            all.Add(new InternalBorderSiteTypeDetails()
            {    
                Code = "24", 
                SearchFields = "24,שער אפרים", 
                Inactive = true, 
                LocalName = "שער אפרים", 
			});
			 
            all.Add(new InternalBorderSiteTypeDetails()
            {    
                Code = "25", 
                SearchFields = "25,ביטוניה-עופר", 
                Inactive = true, 
                LocalName = "ביטוניה-עופר", 
			});
			 
            all.Add(new InternalBorderSiteTypeDetails()
            {    
                Code = "27", 
                SearchFields = "27,תרקומיה", 
                Inactive = true, 
                LocalName = "תרקומיה", 
			});
			 
            all.Add(new InternalBorderSiteTypeDetails()
            {    
                Code = "29", 
                SearchFields = "29,ג'אלמה-גלבוע", 
                Inactive = true, 
                LocalName = "ג'אלמה-גלבוע", 
			});
			 
            all.Add(new InternalBorderSiteTypeDetails()
            {    
                Code = "30", 
                SearchFields = "30,שמעה", 
                Inactive = true, 
                LocalName = "שמעה", 
			});
			 
            all.Add(new InternalBorderSiteTypeDetails()
            {    
                Code = "79", 
                SearchFields = "79,כרם שלום", 
                Inactive = true, 
                LocalName = "כרם שלום", 
			});
			
            return all;
       }

	    public void MapPoco(InternalBorderSiteType newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(InternalBorderSiteType rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

