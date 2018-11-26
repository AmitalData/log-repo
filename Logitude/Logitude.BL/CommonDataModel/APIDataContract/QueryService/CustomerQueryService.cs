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

                if (!string.IsNullOrEmpty(MyEntity.PartnerCode))
                {
                    ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(Tenant);
                    var MyCode = helper.GetLogitudeCodeTranslation(MyEntity.PartnerCode, ComputingPartnerName, "Card");

                    if (string.IsNullOrEmpty(MyCode))
                    {
                        throw new ApplicationException("Card with Partner Code " + MyEntity.PartnerCode + " doesn't match any record");
                    }

                    temp = myQuery.GetSingleCustomerPMByCode(MyCode, Tenant);
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
                temp.LocalName = MyEntity.LocalName;
                temp.VatNumber = MyEntity.VatNumber;

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

                if (string.IsNullOrEmpty(temp.Code))
                {
                    temp.Code = MyEntity.PartnerCode;
                }

                if (MyEntity.Contacts != null && MyEntity.Contacts.Count > 0)
                {
                    ContactQueryService ContactService2 = new ContactQueryService(Tenant);
                    temp.Contacts = ContactService2.ContactCustomDataMappingAndValidatin(MyEntity, MyEntity.Contacts, Tenant, ComputingPartnerName);
                }

                if (MyEntity.MainAddress != null)
                {
                    AddressQueryService AddressQueryService = new AddressQueryService(Tenant);
                    AddressPM address = AddressQueryService.AddressCustomDataMappingAndValidatin_CityCountry(MyEntity.MainAddress, Tenant, ComputingPartnerName);
                    address.AddressTypeId = "M";
                    address.Description = "Main Address";
                    address.Tenant = Tenant;
                    temp.Addresses.Add(address);
                }

                if (MyEntity.BillingAddress != null)
                {
                    AddressQueryService AddressQueryService = new AddressQueryService(Tenant);
                    AddressPM address = AddressQueryService.AddressCustomDataMappingAndValidatin_CityCountry(MyEntity.BillingAddress, Tenant, ComputingPartnerName);
                    address.AddressTypeId = "B";
                    address.Description = "Billing Address";
                    address.Tenant = Tenant;
                    temp.Addresses.Add(address);
                }

                //IGLAccountQueryServiceExt GLAccountGLAccountService = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
                //if (MyEntity.GLAccount != null)
                //{
                //    var myGLAccountPM = GLAccountGLAccountService.GLAccountDataMappingAndValidatin(MyEntity.GLAccount, Tenant, ComputingPartnerName);
                //    if (myGLAccountPM != null)
                //    {
                //        temp.GLAccountId = myGLAccountPM.Id;                        
                //    }
                //}

                return temp;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
