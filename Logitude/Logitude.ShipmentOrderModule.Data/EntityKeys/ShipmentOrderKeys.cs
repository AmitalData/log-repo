using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure; 
  
namespace Logitude.ShipmentOrderModule.Data.EntityKeys
{
   public partial class ShipmentOrderKeys : EntityKeyFields
   {
   	  public string Id  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                 return Id ;
                 
      }

      public override string GetEntityPMName()
      {
          return "ShipmentOrderPM";
      }
	 
   }

}
	 