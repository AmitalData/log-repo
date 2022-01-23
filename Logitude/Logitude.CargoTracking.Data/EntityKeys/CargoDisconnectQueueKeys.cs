using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure; 
  
namespace Logitude.CargoTracking.Data.EntityKeys
{
   public partial class CargoDisconnectQueueKeys : EntityKeyFields
   {
   	  public int Id  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                  return (Id).ToString() ;
                 
      }

      public override string GetEntityPMName()
      {
          return "CargoDisconnectQueuePM";
      }
	 
   }

}
	 