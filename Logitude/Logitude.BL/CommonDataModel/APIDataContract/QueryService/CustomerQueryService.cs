using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
    public partial class CustomerQueryService
    {
        public CustomerPM CustomerCustomDataMappingAndValidating(Customer MyEntity, int Tenant, string ComputingPartnerCode = "")
        {
            try
            {
                UserQuery userQuery = new UserQuery(Tenant);
                UserPM MyUserPM = userQuery.GetSingleUserPMByEmail("system@tenant" + Tenant + ".com", Tenant, false);

                CustomerPM temp = this.MapAndValidate(MyEntity, Tenant, ComputingPartnerCode);
                temp.Tenant = Tenant;
                temp.PartnerTypeId = "CS";
                temp.CustomerStatusCode = "ACT";
                temp.IsCustomer = true;
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

        public CustomerPM MapAndValidate(Customer MyEntity, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                var myQuery = new CustomerQuery(Tenant);
                var temp = new CustomerPM();
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

                if(!string.IsNullOrEmpty(MyEntity.LocalName))
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

                AddressQueryService AddressAddressService = new AddressQueryService(Tenant);
                if (MyEntity.MainAddress != null)
                {
                    var myMainAddressPM = AddressAddressService.AddressCustomDataMappingAndValidatin(MyEntity.MainAddress, Tenant, ComputingPartnerName);
                    if (myMainAddressPM != null)
                    {
                        temp.MainAddressId = myMainAddressPM.Id;
                    }
                }
                
                if (MyEntity.BillingAddress != null)
                {
                    var myBillingAddressPM = AddressAddressService.AddressCustomDataMappingAndValidatin(MyEntity.BillingAddress, Tenant, ComputingPartnerName);
                    if (myBillingAddressPM != null)
                    {
                        temp.BillingAddressId = myBillingAddressPM.Id;
                    }
                }
                
                if (MyEntity.Contacts != null && MyEntity.Contacts.Count > 0)
                {
                    ContactQueryService ContactService2 = new ContactQueryService(Tenant);
                    temp.Contacts = ContactService2.ContactCustomDataMappingAndValidatin(MyEntity, MyEntity.Contacts, Tenant, ComputingPartnerName);
                }

                AddressQueryService AddressQueryService = new AddressQueryService(Tenant);
                if (MyEntity.MainAddress != null)
                {
                    AddressPM address = AddressQueryService.AddressDataMappingAndValidatin(MyEntity.MainAddress, Tenant);
                    if (!string.IsNullOrEmpty(MyEntity.MainAddress.City.Code))
                    {
                        address = AddressQueryService.AddressCustomDataMappingAndValidatin_CityCountry(MyEntity.MainAddress, Tenant, ComputingPartnerName);
                    }

                    else if(!string.IsNullOrEmpty(MyEntity.MainAddress.City.Name))
                    {                        
                        address.City = MyEntity.MainAddress.City.Name;
                    }
                    
                    address.AddressTypeId = "M";
                    address.Description = "Main Address";
                    address.Tenant = Tenant;                    

                    if(!string.IsNullOrEmpty(MyEntity.MainAddress.Name))
                    {
                        address.Name = FormatHelper.ConvertFromBase64(MyEntity.MainAddress.Name);                        
                    }

                    if (!string.IsNullOrEmpty(MyEntity.MainAddress.Address1))
                    {
                        address.Name = FormatHelper.ConvertFromBase64(MyEntity.MainAddress.Address1);
                    }

                    if (!string.IsNullOrEmpty(MyEntity.MainAddress.Address2))
                    {
                        address.Name = FormatHelper.ConvertFromBase64(MyEntity.MainAddress.Address2);
                    }

                    temp.Addresses.Add(address);
                }

                if (MyEntity.BillingAddress != null)
                {
                    AddressPM address = AddressQueryService.AddressDataMappingAndValidatin(MyEntity.BillingAddress, Tenant);
                    if (!string.IsNullOrEmpty(MyEntity.BillingAddress.City.Code))
                    {
                        address = AddressQueryService.AddressCustomDataMappingAndValidatin_CityCountry(MyEntity.BillingAddress, Tenant, ComputingPartnerName);
                    }

                    else if (!string.IsNullOrEmpty(MyEntity.BillingAddress.City.Name))
                    {
                        address.City = MyEntity.BillingAddress.City.Name;
                    }

                    address.AddressTypeId = "B";
                    address.Description = "Billing Address";
                    address.Tenant = Tenant;

                    if (!string.IsNullOrEmpty(MyEntity.BillingAddress.Name))
                    {
                        address.Name = FormatHelper.ConvertFromBase64(MyEntity.BillingAddress.Name);
                    }

                    if (!string.IsNullOrEmpty(MyEntity.MainAddress.Address1))
                    {
                        address.Name = FormatHelper.ConvertFromBase64(MyEntity.BillingAddress.Address1);
                    }

                    if (!string.IsNullOrEmpty(MyEntity.MainAddress.Address2))
                    {
                        address.Name = FormatHelper.ConvertFromBase64(MyEntity.BillingAddress.Address2);
                    }

                    temp.Addresses.Add(address);
                }
                
                return temp;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
