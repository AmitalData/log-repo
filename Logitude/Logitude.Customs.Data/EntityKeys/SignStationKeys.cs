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
   public partial class SignStationKeys : EntityKeyFields
   {
   	  public string CustomsAgentId  { get; set; }
	  
				 
	    			   
	  public string PersonId  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                 return CustomsAgentId+'_'+PersonId ;
                 
      }

      public override string GetEntityPMName()
      {
          return "SignStationPM";
      }
	 
   }

}
	 