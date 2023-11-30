using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure; 
  
namespace Logitude.Accounting.Data.EntityKeys
{
   public partial class ARPaymentsJournalKeys : EntityKeyFields
   {
   	  public int Tenant  { get; set; }
	  
				 
	    			   
	  public string PaymentId  { get; set; }
	  
				 
	    			   
	  public bool IsVoided  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                 return Tenant+'_'+PaymentId+'_'+IsVoided ;
                 
      }

      public override string GetEntityPMName()
      {
          return "ARPaymentsJournalPM";
      }
	 
   }

}
	 