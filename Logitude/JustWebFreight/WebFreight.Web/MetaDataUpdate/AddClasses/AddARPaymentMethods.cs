using Logitude.Server.Tools.Counters;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{

    public class AddARPaymentMethods
    {
        public static void AddARPaymentMethod(ARPaymentMethodDetails arpaymentMethodDetails, ARPaymentMethodRepository arPaymentMethodRepository, Dictionary<string, ARPaymentMethod> tenantARPaymentMethods)
        {
            if (tenantARPaymentMethods.Keys.Contains(arpaymentMethodDetails.Code))
            {
                ARPaymentMethod ARPamentMethod = tenantARPaymentMethods[arpaymentMethodDetails.Code];

                ARPamentMethod.Tenant = arpaymentMethodDetails.Tenant;
                ARPamentMethod.Name = arpaymentMethodDetails.Name;
                ARPamentMethod.AddedManually = false;
                ARPamentMethod.InActive = false;
                ARPamentMethod.Code = arpaymentMethodDetails.Code;
                ARPamentMethod.SearchFields = arpaymentMethodDetails.Code + "," + arpaymentMethodDetails.Name;
                arPaymentMethodRepository.Update(ARPamentMethod);
            }
            else
            {
                ARPaymentMethod newARPaymentMethod = new ARPaymentMethod()
                {
                    Tenant = arpaymentMethodDetails.Tenant,
                    Name = arpaymentMethodDetails.Name,
                    AddedManually = false,
                    InActive = false,
                    Code = arpaymentMethodDetails.Code,
                    SearchFields = arpaymentMethodDetails.Code + "," + arpaymentMethodDetails.Name,
                    Id = IdCounter.GetNumber("ARPaymentMethod", arpaymentMethodDetails.Tenant).ToString(),

                };
                arPaymentMethodRepository.Add(newARPaymentMethod);
            }
        }
    }

}