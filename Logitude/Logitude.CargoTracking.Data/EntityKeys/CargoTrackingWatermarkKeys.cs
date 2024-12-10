using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
   public partial class CargoTrackingWatermarkKeys : EntityKeyFields
   {
   	  public string TableName  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                  return (TableName).ToString() ;
                 
      }

      public override string GetEntityPMName()
      {
          return "CargoTrackingWatermarkPM";
      }
	 
   }

}
	 