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
   public partial class PostLikeKeys : EntityKeyFields
   {
   	  public string PostId  { get; set; }
	  
				 
	    			   
	  public string UserId  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
          return PostId+'_'+UserId;
      }

      public override string GetEntityPMName()
      {
          return "PostLikePM";
      }
	 
   }

}
	 