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
   public partial class QuoteOPVATsTotalKeys : EntityKeyFields
   {
   	
	 
	  public override string GetFullKey()
      {
                 return  ;
                 
      }

      public override string GetEntityPMName()
      {
          return "QuoteOPVATsTotalPM";
      }
	 
   }

}
	 