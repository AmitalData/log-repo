

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
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs; 
using Simplog.Data.InfrastructureModel;

namespace Logitude.BL.InfrastructureModel
{
   public class DWCategoriesDetails : DWCategories, ICloseTable<DWCategories, DWCategoriesDetails>
   {
       public List<DWCategoriesDetails> GetAll()
       {
		    var all = new List<DWCategoriesDetails>(); 
            return all;
       }

	    public void MapPoco(DWCategories newPoco)
        {    
        }

		public string GetSearchFields(DWCategories rec)
        {   
           return string.Empty;
        }
   }
}

