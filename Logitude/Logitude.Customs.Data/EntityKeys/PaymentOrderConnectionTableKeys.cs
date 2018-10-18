using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure; 
  
namespace Logitude.Customs.Data.EntityKeys
{
   public partial class PaymentOrderConnectionTableKeys : EntityKeyFields
   {
   	  public string PaymentOrderId  { get; set; }
	  
				 
	    			   
	  public string ConnectedEntityId  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
          return PaymentOrderId+'_'+ConnectedEntityId;
      }

      public override string GetEntityPMName()
      {
          return "PaymentOrderConnectionTablePM";
      }
	 
   }

}
	 