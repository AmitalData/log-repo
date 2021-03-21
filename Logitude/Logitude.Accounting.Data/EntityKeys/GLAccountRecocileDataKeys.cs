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
   public partial class GLAccountRecocileDataKeys : EntityKeyFields
   {
   	  public string AccountId  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                 return AccountId ;
                 
      }

      public override string GetEntityPMName()
      {
          return "GLAccountRecocileDataPM";
      }
	 
   }

}
	 