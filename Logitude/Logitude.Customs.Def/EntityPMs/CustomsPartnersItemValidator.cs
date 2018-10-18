using Logitude.Customs.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityPMs
{
  public  class CustomsPartnersItemValidator
    {

      public static ValidationResult IsClassificationCodeValid(
       CustomsPartnersItemPM entity,
       System.ComponentModel.DataAnnotations.ValidationContext context)
      {


          bool valid = true;

          if (entity != null)
          {
             
                  if (!entity.IsClassificationCodeValid)
                  {
                      valid = false;
                      return new ValidationResult("invalid value");
                  }
                  return null;
              

          }


          return null;
      }
    }
}
