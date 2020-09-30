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
   public partial class ClaimsRelatedEntitiesSeizureKeys : EntityKeyFields
   {
   	  public string ClaimId  { get; set; }
	  
				 
	    			   
	  public int CounterKey  { get; set; }
	  
				 
	    			   
	  public int SeizureLinoNo  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                 return ClaimId+'_'+CounterKey+'_'+SeizureLinoNo ;
                 
      }

      public override string GetEntityPMName()
      {
          return "ClaimsRelatedEntitiesSeizurePM";
      }
	 
   }

}
	 