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
   public partial class DefaultTypeKeys : EntityKeyFields
   {
   	  public string Id  { get; set; }
	  
				 
	    			   
	  public string Code  { get; set; }
	  
				 
	    			   
	  public string Distr  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                 return Id+'_'+Code+'_'+Distr ;
                 
      }

      public override string GetEntityPMName()
      {
          return "DefaultTypePM";
      }
	 
   }

}
	 