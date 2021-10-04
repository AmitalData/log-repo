using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
    public partial class VendorQueryService
    {
        public VendorPM VendorCustomDataMappingAndValidating(Vendor MyEntity, int Tenant, string ComputingPartnerCode = "")
        {
            try
            {
                UserQuery userQuery = new UserQuery(Tenant);
                UserPM MyUserPM = userQuery.GetSingleUserPMByEmail("system@tenant" + Tenant + ".com", Tenant, false);

                VendorPM temp = this.MapAndValidate(MyEntity, Tenant, ComputingPartnerCode);
                temp.Tenant = Tenant;
                temp.PartnerTypeId = "VD";
                temp.CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                temp.UpdateDate = TenantServerConfigration.GetCurrentDateTime(Tenant);

                if (string.IsNullOrEmpty(temp.CreatedByUserId))
                {
                    temp.CreatedByUserId = MyUserPM.Id;
                    temp.UpdatedByUserId = MyUserPM.Id;
                }

                return temp;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public VendorPM MapAndValidate(Vendor MyEntity, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                var myQuery = new VendorQuery(Tenant);
                var temp = new VendorPM();
                if (!string.IsNullOrEmpty(MyEntity.Id))
                {
                    temp = myQuery.GetSinglePM(MyEntity.Id, Tenant);
                }
                
                if (temp == null)
                {
                    throw new ApplicationException("Card with Id " + MyEntity.Id + " doesn't exist");
                }

                if (string.IsNullOrEmpty(temp.Id))
                {
                    temp.Id = MyEntity.Id;
                }

                temp.EnglishName = MyEntity.EnglishName;                
                temp.VatNumber = MyEntity.VatNumber;
                temp.Code = MyEntity.Code;
                temp.GLAccountNumber = MyEntity.GLAccount.InternalNumber;
                

                if (!string.IsNullOrEmpty(MyEntity.LocalName))
                {
                    temp.LocalName = FormatHelper.ConvertFromBase64(MyEntity.LocalName);
                }

                PaymentTermQueryService PaymentTermPaymentTermService = new PaymentTermQueryService(Tenant);
                if (MyEntity.PaymentTerm != null)
                {
                    var myPaymentTermPM = PaymentTermPaymentTermService.PaymentTermDataMappingAndValidatin(MyEntity.PaymentTerm, Tenant, ComputingPartnerName);
                    if (myPaymentTermPM != null)
                    {
                        temp.PaymentTermId = myPaymentTermPM.Id;
                    }
                }
                                
                if (MyEntity.MainAddress != null)
                {
                    AddressQueryService AddressQueryService = new AddressQueryService(Tenant);
                    AddressPM address = AddressQueryService.AddressDataMappingAndValidatin(MyEntity.MainAddress, Tenant);
                    if (!string.IsNullOrEmpty(MyEntity.MainAddress.City))
                    {
                        address = AddressQueryService.AddressCustomDataMappingAndValidatin_CityCountry(MyEntity.MainAddress, Tenant, ComputingPartnerName);
                    }

                    else if (!string.IsNullOrEmpty(MyEntity.MainAddress.City))
                    {
                        address.City = MyEntity.MainAddress.City;
                    }

                    address.AddressTypeId = "M";
                    address.Description = "Main Address";
                    address.Tenant = Tenant;

                    if (!string.IsNullOrEmpty(MyEntity.MainAddress.Name))
                    {
                        address.Name = FormatHelper.ConvertFromBase64(MyEntity.MainAddress.Name);
                    }

                    if (!string.IsNullOrEmpty(MyEntity.MainAddress.Address1))
                    {
                        address.Address1 = FormatHelper.ConvertFromBase64(MyEntity.MainAddress.Address1);
                    }
                    temp.Addresses.Add(address);
                }

                if (MyEntity.BillingAddress != null)
                {
                    SetBillingAddress(MyEntity, ComputingPartnerName, temp);
                }

                return temp;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void SetBillingAddress(Vendor MyEntity, string ComputingPartnerName, VendorPM temp)
        {
            AddressQueryService AddressQueryService = new AddressQueryService(temp.Tenant);
            AddressPM address = AddressQueryService.AddressDataMappingAndValidatin(MyEntity.BillingAddress, temp.Tenant);
            if (!string.IsNullOrEmpty(MyEntity.BillingAddress.City))
            {
                address = AddressQueryService.AddressCustomDataMappingAndValidatin_CityCountry(MyEntity.BillingAddress, temp.Tenant, ComputingPartnerName);
            }

            else if (!string.IsNullOrEmpty(MyEntity.BillingAddress.City))
            {
                address.City = MyEntity.MainAddress.City;
            }

            address.AddressTypeId = "B";
            address.Description = "Billing Address";
            address.Tenant = temp.Tenant;

            if (!string.IsNullOrEmpty(MyEntity.BillingAddress.Name))
            {
                address.Name = FormatHelper.ConvertFromBase64(MyEntity.BillingAddress.Name);
            }

            if (!string.IsNullOrEmpty(MyEntity.BillingAddress.Address1))
            {
                address.Address1 = FormatHelper.ConvertFromBase64(MyEntity.BillingAddress.Address1);
            }
            temp.Addresses.Add(address);
        }

    }
}
