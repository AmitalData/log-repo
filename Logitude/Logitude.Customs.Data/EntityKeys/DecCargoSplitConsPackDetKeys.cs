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
   public partial class DecCargoSplitConsPackDetKeys : EntityKeyFields
   {
   	  public string DeclarationCargoSplitId  { get; set; }
	  
				 
	    			   
	  public int? DecCargoSplitConsLineNo  { get; set; }
	  
				 
	    			   
	  public int DecCargoSplitConsItemLine  { get; set; }
	  
				 
	    			   
	  public int PackageLine  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                 return DeclarationCargoSplitId+'_'+DecCargoSplitConsLineNo+'_'+DecCargoSplitConsItemLine+'_'+PackageLine ;
                 
      }

      public override string GetEntityPMName()
      {
          return "DecCargoSplitConsPackDetPM";
      }
	 
   }

}
	 