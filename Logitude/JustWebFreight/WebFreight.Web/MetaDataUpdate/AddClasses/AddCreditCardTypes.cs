using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using WebFreight.Web.Helpers;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Logitude.Server.Tools.Counters;
namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddCreditCardTypes
    {
        public static void AddCreditCardType(CreditCardTypeDetails creditCardTypeDetails, CreditCardTypeRepository creditCardTypeRepository, Dictionary<string, CreditCardType> tenantCreditCardTypes)
        {
            if (tenantCreditCardTypes.Keys.Contains(creditCardTypeDetails.Code))
            {
                CreditCardType creditCardType = tenantCreditCardTypes[creditCardTypeDetails.Code];

                creditCardType.Name = creditCardTypeDetails.Name;
                creditCardType.Code = creditCardTypeDetails.Code;
                creditCardType.Tenant = creditCardTypeDetails.Tenant;
                creditCardType.SearchFields = creditCardTypeDetails.Code + "," + creditCardTypeDetails.Name;
                creditCardTypeRepository.Update(creditCardType);
            }
            else
            {
                CreditCardType newCreditCardType = new CreditCardType()
                {
                    Tenant = creditCardTypeDetails.Tenant,
                    Name = creditCardTypeDetails.Name,
                    Code = creditCardTypeDetails.Code,
                    SearchFields = creditCardTypeDetails.Code + "," + creditCardTypeDetails.Name,
                    Id = IdCounter.GetNumber("CreditCardType", creditCardTypeDetails.Tenant).ToString(),

                };
                creditCardTypeRepository.Add(newCreditCardType);
            }
        }
    }
}