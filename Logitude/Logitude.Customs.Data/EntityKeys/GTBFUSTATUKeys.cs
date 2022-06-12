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
   public partial class GTBFUSTATUKeys : EntityKeyFields
   {

       public string Code;
	  public override string GetFullKey()
      {
                 return this.Code;
                 
      }

      public override string GetEntityPMName()
      {
          return "GTBFUSTATUPM";
      }
	 
   }

}
	 