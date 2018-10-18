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
   public partial class DeclarationConstraintKeys : EntityKeyFields
   {
   	  public string DeclarationID  { get; set; }
	  
				 
	    			   
	  public string ConstraintNumber  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
          return DeclarationID+'_'+ConstraintNumber;
      }

      public override string GetEntityPMName()
      {
          return "DeclarationConstraintPM";
      }
	 
   }

}
	 