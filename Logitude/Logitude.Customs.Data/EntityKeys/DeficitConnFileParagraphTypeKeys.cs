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
   public partial class DeficitConnFileParagraphTypeKeys : EntityKeyFields
   {
   	  public string DeficitId  { get; set; }
	  
				 
	    			   
	  public string DeclarationId  { get; set; }
	  
				 
	    			   
	  public string ParagraphTypeCode  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
          return DeficitId+'_'+DeclarationId+'_'+ParagraphTypeCode;
      }

      public override string GetEntityPMName()
      {
          return "DeficitConnFileParagraphTypePM";
      }
	 
   }

}
	 