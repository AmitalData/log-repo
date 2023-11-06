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
   public partial class ClientItemKeys : EntityKeyFields
   {
   	  public string ItemCode  { get; set; }
	  
				 
	    			   
	  public string ClientCode  { get; set; }
	  
				 
	    			   
	  public string Id  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                 return ItemCode+'_'+ClientCode+'_'+Id ;
                 
      }

      public override string GetEntityPMName()
      {
          return "ClientItemPM";
      }
	 
   }

}
	 