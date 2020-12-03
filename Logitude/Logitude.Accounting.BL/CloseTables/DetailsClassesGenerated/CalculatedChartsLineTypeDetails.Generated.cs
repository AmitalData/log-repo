
   
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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL
{
   public class CalculatedChartsLineTypeDetails : CalculatedChartsLineType, ICloseTable<CalculatedChartsLineType, CalculatedChartsLineTypeDetails>
   {
       public List<CalculatedChartsLineTypeDetails> GetAll()
       {
		    var all = new List<CalculatedChartsLineTypeDetails>();  
            all.Add(new CalculatedChartsLineTypeDetails()
            {    
                Code = "1", 
                LocalName = "כרטיס", 
                EnglishName = "GLAccount", 
                SearchFields = "1,GLAccount,כרטיס", 
			});
			 
            all.Add(new CalculatedChartsLineTypeDetails()
            {    
                Code = "2", 
                LocalName = "קבוצת מאזן", 
                EnglishName = "Chart of Account", 
                SearchFields = "2,Chart of Account,קבוצת מאזן", 
			});
			
            return all;
       }

	    public void MapPoco(CalculatedChartsLineType newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.LocalName = this.LocalName;  
		    newPoco.EnglishName = this.EnglishName;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(CalculatedChartsLineType rec)
        {   
           return String.Concat(rec.Code,",",rec.LocalName,",",rec.EnglishName,",");
        }
   }
}

