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
   public partial class ClaimsRelatedEntityKeys : EntityKeyFields
   {
   	  public string ClaimId  { get; set; }
	  
				 
	    			   
	  public int EntityCounterKey  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
          return ClaimId+'_'+EntityCounterKey;
      }

      public override string GetEntityPMName()
      {
          return "ClaimsRelatedEntityPM";
      }
	 
   }

}
	 