using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Customs.Data.Repsitories;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsVendorQueryService : EntityQueryService<CustomsVendor, CustomsVendorKeys, CustomsVendorPM, object, CustomsVendorKeys>
    {
        public override void GetComposition(EntityKeyFields entityKeys, CustomsVendorPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            CustomsVendorKeys vendorKeys = entityKeys as CustomsVendorKeys;
            VendorCommunicationQueryService vendorCommunicationQueryService = new VendorCommunicationQueryService(context);
            entityPM.VendorCommunications = vendorCommunicationQueryService.GetMulti(vendorKeys, true);

            if (entityPM.VendorCommunications.Count > 0)
            {
                entityPM.LastLineNumber = entityPM.VendorCommunications.Max(m => m.LineNumber);
            }

            base.GetComposition(entityKeys, entityPM);
        }

        public bool DoesVendorExist(string vendorNumber,int tenant)
        {
            return repository.DoesVendorExist(vendorNumber,tenant);

        }

        public string GetIdByVendorNumber(string vendorNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(vendorNumber)) return "";
            return repository.GetIdByVendorNumber(vendorNumber, tenant);
        }

        public CustomsVendorPM GetVendorByNumber(string vendorNumber, int tenant)
        {
            string number = vendorNumber;
            if (vendorNumber != null)
            {

             number = vendorNumber.Trim();
            }
            CustomsVendor vendor = repository.GetVendorByNumber(number, tenant);

            CustomsVendorPM vendorPM = null;
            if (vendor != null)
            {
                vendorPM = new CustomsVendorPM()
                {
                    CityName = vendor.CityName,

                    CountryCode = vendor.CountryCode,
                    ConcurrencyGUID = vendor.ConcurrencyGUID,
                    InActive = vendor.InActive,
                    MainAddressLine = vendor.MainAddressLine,
                    PostalCode = vendor.PostalCode,
                    StatusCode = vendor.StatusCode,
                    SubCountryCode = vendor.SubCountryCode,
                    Tenant = vendor.Tenant,
                   VATNumber = vendor.VATNumber,
                   VendorNumber = vendor.VendorNumber,
                   VendorTypeCode = vendor.VendorTypeCode,
                   DunsNumber = vendor.DunsNumber,
                   IsPalestinian = vendor.IsPalestinian,
                   Id = vendor.Id,
                   ExternalId = vendor.ExternalId,
                   SearchFields = vendor.SearchFields,
                   TransactionTypeID = vendor.TransactionTypeID,
                   VendorName = vendor.VendorName
                };
            }

            return vendorPM;

          
        }


    }
}
