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
   public partial class DeclarationExportRecipientKeys : EntityKeyFields
   {
   	  public string DeclarationId  { get; set; }
	  
				 
	    			   
	  public int? LineNumber  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                 return DeclarationId+'_'+LineNumber ;
                 
      }

      public override string GetEntityPMName()
      {
          return "DeclarationExportRecipientPM";
      }
	 
   }

}
	 