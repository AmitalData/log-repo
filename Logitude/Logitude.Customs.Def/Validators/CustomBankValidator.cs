using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.Validators
{
   public class CustomBankValidator
    {

       public static ValidationResult IsInternalCodeExist(
          CustomBankPM bank,    
          System.ComponentModel.DataAnnotations.ValidationContext context)
       {
           bool valid = true;


           ICustomContext customContext = CustomContext.GetContext(bank.Tenant);
           CustomBankListQueryService customBankQuery = new CustomBankListQueryService(customContext);
           CustomBankRepository repository = new CustomBankRepository(bank.Tenant);

           CustomBank CustomBank = repository.GetSingle(new CustomBankKeys() { Id = bank.Id });
           if (CustomBank != null)
           {
               if (CustomBank.InternalCode != bank.InternalCode)
               {

                   bool exist = (customBankQuery.GetList(bank.Tenant).Where(d => d.InternalCode == bank.InternalCode && d.Tenant == bank.Tenant)).Any();
                 
                       if (exist)
                       {
                           valid = false;
                           return new ValidationResult(TranslateTextsClass.Translate("Customs.CustomBank.O.InternalCodekAlreadyExist", bank.Tenant));
                       }
                       return null;
                   }
               
           }
           

           return null;
       }

    }
}
