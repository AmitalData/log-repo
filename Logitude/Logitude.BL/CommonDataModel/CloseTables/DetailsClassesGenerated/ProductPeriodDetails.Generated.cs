

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
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs; 
using Simplog.Data.CommonDataModel;

namespace Logitude.BL.CommonDataModel
{
   public class ProductPeriodDetails : ProductPeriod, ICloseTable<ProductPeriod, ProductPeriodDetails>
   {
       public List<ProductPeriodDetails> GetAll()
       {
		    var all = new List<ProductPeriodDetails>();  
            all.Add(new ProductPeriodDetails()
            {    
                Code = "MO", 
                SearchFields = "MO,Monthly", 
                Name = "Monthly", 
			});
			 
            all.Add(new ProductPeriodDetails()
            {    
                Code = "QU", 
                SearchFields = "QU,Quarterly", 
                Name = "Quarterly", 
			});
			 
            all.Add(new ProductPeriodDetails()
            {    
                Code = "YE", 
                SearchFields = "YE,Yearly", 
                Name = "Yearly", 
			});
			
            return all;
       }

	    public void MapPoco(ProductPeriod newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(ProductPeriod rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

