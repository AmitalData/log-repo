using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure; 
  
namespace Logitude.CRM.Data.EntityKeys
{
   public partial class OpportunityCompetitorKeys : EntityKeyFields
   {
   	  public string OpportunityId  { get; set; }
	  
				 
	    			   
	  public string CompetitorId  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
          return OpportunityId+'_'+CompetitorId;
      }

      public override string GetEntityPMName()
      {
          return "OpportunityCompetitorPM";
      }
	 
   }

}
	 