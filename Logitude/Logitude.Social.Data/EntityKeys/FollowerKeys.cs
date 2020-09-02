using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure; 
  
namespace Logitude.Social.Data.EntityKeys
{
   public partial class FollowerKeys : EntityKeyFields
   {
   	  public string FolloweeUserId  { get; set; }
	  
				 
	    			   
	  public string FollowerUserId  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
          return FolloweeUserId+'_'+FollowerUserId;
      }

      public override string GetEntityPMName()
      {
          return "FollowerPM";
      }
	 
   }

}
	 