using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure; 
  
namespace Logitude.TariffModule.Data.EntityKeys
{
   public partial class TariffVersionKeys : EntityKeyFields
   {
   	  public string TariffId  { get; set; }
	  
				 
	    			   
	  public int Version  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
          return TariffId+'_'+Version;
      }

      public override string GetEntityPMName()
      {
          return "TariffVersionPM";
      }
	 
   }

}
	 