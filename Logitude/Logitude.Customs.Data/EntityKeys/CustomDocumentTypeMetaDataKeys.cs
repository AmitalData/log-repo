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
   public partial class CustomDocumentTypeMetaDataKeys : EntityKeyFields
   {
   	  public string MetaDataTypeCode  { get; set; }
	  
				 
	    			   
	  public string DocumentTypeCode  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                 return MetaDataTypeCode+'_'+DocumentTypeCode ;
                 
      }

      public override string GetEntityPMName()
      {
          return "CustomDocumentTypeMetaDataPM";
      }
	 
   }

}
	 