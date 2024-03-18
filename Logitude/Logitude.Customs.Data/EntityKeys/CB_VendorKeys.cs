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
   public partial class CB_VendorKeys : EntityKeyFields
   {
   	  public string ID  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                 return ID ;
                 
      }

      public override string GetEntityPMName()
      {
          return "CB_VendorPM";
      }
	 
   }

}
	 