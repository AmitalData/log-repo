using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class TruckerValidating
    {
        public static void Validate(EntityPMs.TruckerPM entityPM, Card Card, ICommonDataContext myContext, bool isNewEntity)
        {
            TenantRepository tenantRepository = new TenantRepository(myContext);
            Tenant myTenant = tenantRepository.GetSingleTenantOnly(entityPM.Tenant);
            CardValidating.ValidateCode_Unique(Card, myContext);

            

            if (myTenant.ApplyVATForAllPartners)
            {
                if (Card != null)
                {
                    VATValidating.ValidateVAT_Required(Card, myTenant);
                    VATValidating.ValidateVAT_Unique(Card, myContext, myTenant, isNewEntity);
                    VATValidating.ValidateVAT_Format(Card, myContext, myTenant);
                }
            }

            foreach (AddressPM itemPM in entityPM.Addresses)
            {
                AddressValidating.Validate(itemPM);
            }
        }
    }
}