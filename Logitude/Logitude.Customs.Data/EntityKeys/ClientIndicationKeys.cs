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
   public partial class ClientIndicationKeys : EntityKeyFields
   {
   	  public string IndicationId  { get; set; }
	  
				 
	    			   
	  public string ClientId  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                 return IndicationId+'_'+ClientId ;
                 
      }

      public override string GetEntityPMName()
      {
          return "ClientIndicationPM";
      }
	 
   }

}
	 