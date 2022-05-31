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
   public partial class AvailableStatusFieldKeys : EntityKeyFields
   {
   	  public string FieldCode  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                 return FieldCode ;
                 
      }

      public override string GetEntityPMName()
      {
          return "AvailableStatusFieldPM";
      }
	 
   }

}
	 