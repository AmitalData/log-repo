

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
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityPMs; 
using Simplog.Data.InvoiceModel;

namespace Logitude.BL.InvoiceModel
{
   public class QBOGlobalTaxCalculationDetails : QBOGlobalTaxCalculation, ICloseTable<QBOGlobalTaxCalculation, QBOGlobalTaxCalculationDetails>
   {
       public List<QBOGlobalTaxCalculationDetails> GetAll()
       {
		    var all = new List<QBOGlobalTaxCalculationDetails>();  
            all.Add(new QBOGlobalTaxCalculationDetails()
            {    
                Code = "TI", 
                Name = "Tax Inclusive", 
                SearchFields = "TI,Tax Inclusive", 
			});
			 
            all.Add(new QBOGlobalTaxCalculationDetails()
            {    
                Code = "TE", 
                Name = "Tax Exclusive", 
                SearchFields = "TE,Tax Exclusive", 
			});
			 
            all.Add(new QBOGlobalTaxCalculationDetails()
            {    
                Code = "OS", 
                Name = "Out Of Scope", 
                SearchFields = "OS,Out Of Scope", 
			});
			
            return all;
       }

	    public void MapPoco(QBOGlobalTaxCalculation newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);    
        }

		public string GetSearchFields(QBOGlobalTaxCalculation rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

