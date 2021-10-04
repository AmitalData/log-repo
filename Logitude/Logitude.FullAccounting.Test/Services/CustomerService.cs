using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Codes;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.PartnersPreparation;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Assist;

namespace Logitude.FullAccounting.Test.Services.Preparation
{
    public class CustomerService
    {
        public Partner Create(TechTalk.SpecFlow.Table table)
        {
            dynamic customerTable = table.CreateDynamicInstance();

            var partnerParameters =  new PartnerParameters()
            {
                Code = "T"+DateTime.Now.Ticks.ToString().Substring(4),
                IsCustomer = true,
                Name = customerTable.Name,
                TypeCode = PartnerTypeCodes.Customer
            };
            return DataPreparation.BuildPartner(partnerParameters);
        }
        
    }
}
