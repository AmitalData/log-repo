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
   public partial class ClientsAddressCommTypeKeys : EntityKeyFields
   {
   	  public string ClientId  { get; set; }
	  
				 
	    			   
	  public string AddressId  { get; set; }
	  
				 
	    			   
	  public int Line  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
          return ClientId+'_'+AddressId+'_'+Line;
      }

      public override string GetEntityPMName()
      {
          return "ClientsAddressCommTypePM";
      }
	 
   }

}
	 