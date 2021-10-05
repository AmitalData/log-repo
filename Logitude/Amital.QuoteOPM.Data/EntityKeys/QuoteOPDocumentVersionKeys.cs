using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure; 
  
namespace Amital.QuoteOPM.Data.EntityKeys
{
   public partial class QuoteOPDocumentVersionKeys : EntityKeyFields
   {
   	  public string QuoteOPId  { get; set; }
	  
				 
	    			   
	  public int VersionNumber  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                 return QuoteOPId+'_'+VersionNumber ;
                 
      }

      public override string GetEntityPMName()
      {
          return "QuoteOPDocumentVersionPM";
      }
	 
   }

}
	 