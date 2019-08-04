

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
   public class UsoCFDIDetails : UsoCFDI, ICloseTable<UsoCFDI, UsoCFDIDetails>
   {
       public List<UsoCFDIDetails> GetAll()
       {
		    var all = new List<UsoCFDIDetails>();  
            all.Add(new UsoCFDIDetails()
            {    
                Code = "G01", 
                SearchFields = "G01,Adquisición de mercancias", 
                Name = "Adquisición de mercancias", 
			});
			 
            all.Add(new UsoCFDIDetails()
            {    
                Code = "G02", 
                SearchFields = "G02,Devoluciones, descuentos o bonificaciones", 
                Name = "Devoluciones, descuentos o bonificaciones", 
			});
			 
            all.Add(new UsoCFDIDetails()
            {    
                Code = "G03", 
                SearchFields = "G03,Gastos en general", 
                Name = "Gastos en general", 
			});
			 
            all.Add(new UsoCFDIDetails()
            {    
                Code = "P01", 
                SearchFields = "P01,Por definir", 
                Name = "Por definir", 
			});
			 
            all.Add(new UsoCFDIDetails()
            {    
                Code = "I02", 
                Name = "Mobilario y equipo de oficina por inversiones", 
                SearchFields = "I02,Mobilario y equipo de oficina por inversiones", 
			});
			 
            all.Add(new UsoCFDIDetails()
            {    
                Code = "I03", 
                Name = "Equipo de transporte", 
                SearchFields = "I03,Equipo de transporte", 
			});
			 
            all.Add(new UsoCFDIDetails()
            {    
                Code = "I04", 
                Name = "Equipo de computo y accesorios", 
                SearchFields = "I04,Equipo de computo y accesorios", 
			});
			 
            all.Add(new UsoCFDIDetails()
            {    
                Code = "I06", 
                Name = "Comunicaciones telefónicas", 
                SearchFields = "I06,Comunicaciones telefónicas", 
			});
			
            return all;
       }

	    public void MapPoco(UsoCFDI newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Name = this.Name;   
        }

		public string GetSearchFields(UsoCFDI rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",");
        }
   }
}

