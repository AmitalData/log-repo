

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
   public class MetodoPagoDetails : MetodoPago, ICloseTable<MetodoPago, MetodoPagoDetails>
   {
       public List<MetodoPagoDetails> GetAll()
       {
		    var all = new List<MetodoPagoDetails>();  
            all.Add(new MetodoPagoDetails()
            {    
                Code = "PPD", 
                SearchFields = "PPD,Pago en parcialidades o diferido", 
                Name = "Pago en parcialidades o diferido", 
			});
			 
            all.Add(new MetodoPagoDetails()
            {    
                Code = "PUE", 
                SearchFields = "PUE,Pago en una sola exhibición", 
                Name = "Pago en una sola exhibición", 
			});
			
            return all;
       }

	    public void MapPoco(MetodoPago newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(MetodoPago rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

