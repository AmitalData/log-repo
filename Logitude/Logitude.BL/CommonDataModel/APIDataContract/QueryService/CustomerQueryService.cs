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

        public CustomerPM MapAndValidate(Customer MyEntity, int Tenant, string ComputingPartnerName = "", bool isUpdate = false)
        {
			try
			{
				CustomerQuery myQuery = new CustomerQuery(Tenant);
				CustomerPM myCustomer = new CustomerPM();
				if (!string.IsNullOrEmpty(MyEntity.Id))
				{
					myCustomer = myQuery.GetSinglePM(MyEntity.Id, Tenant);
				}

				if (myCustomer == null)
				{
					throw new ApplicationException("Customer with Id " + MyEntity.Id + " doesn't exist");
				}

				if (!string.IsNullOrEmpty(MyEntity.Code))
				{
					myCustomer = myQuery.GetSingleCustomerPMByCode(MyEntity.Code, Tenant);
				}

				if (myCustomer == null)
				{
					throw new ApplicationException("Customer with Code " + MyEntity.Code + " doesn't exist");
				}

				myCustomer.Id = MyEntity.Id;
				myCustomer.Code = MyEntity.Code;

				//if (string.IsNullOrEmpty(myCustomer.Code))
				//	myCustomer.Code = MyEntity.PartnerCode;

				if (!isUpdate)
				{
					myCustomer.EnglishName = MyEntity.EnglishName;
					myCustomer.VatNumber = MyEntity.VatNumber;
					myCustomer.LeadDescription = MyEntity.LeadDescription;
					myCustomer.StartWorkingDate = MyEntity.StartWorkingDate;

					if (!string.IsNullOrEmpty(MyEntity.LocalName))
						myCustomer.LocalName = FormatHelper.ConvertFromBase64(MyEntity.LocalName);

					myCustomer.PaymentTermId = this.GetPaymentTermId(MyEntity.PaymentTerm, Tenant, ComputingPartnerName);
					myCustomer.MainAddressId = this.GetAddressId(MyEntity.MainAddress, Tenant, ComputingPartnerName);
					myCustomer.BillingAddressId = this.GetAddressId(MyEntity.BillingAddress, Tenant, ComputingPartnerName);
					myCustomer.PickupDeliveryAddressId = this.GetAddressId(MyEntity.PickupDeliveryAddress, Tenant, ComputingPartnerName);
					myCustomer.AccountManagerUserId = this.GetUserId(MyEntity.AccountManagerUser, Tenant, ComputingPartnerName);
					myCustomer.SalesmanUserId = this.GetUserId(MyEntity.SalesmanUser, Tenant, ComputingPartnerName);
					myCustomer.CollectorId = this.GetUserId(MyEntity.Collector, Tenant, ComputingPartnerName);
					myCustomer.TeamId = this.GetTeamId(MyEntity.Team, Tenant, ComputingPartnerName);
					myCustomer.IndustryId = this.GetIndustryId(MyEntity.Industry, Tenant, ComputingPartnerName);
					myCustomer.InvoiceCurrencyId = this.GetCurrencyId(MyEntity.InvoiceCurrency, Tenant, ComputingPartnerName);
					myCustomer.VatTypeId = this.GetVatTypeId(MyEntity.VatType, Tenant, ComputingPartnerName);
					myCustomer.LeadSourceId = this.GetLeadSourceId(MyEntity.LeadSource, Tenant, ComputingPartnerName);
					myCustomer.GLAccountId = this.GetGLAccountId(MyEntity.GLAccount, Tenant, ComputingPartnerName);

					AddressPM mainAddress = this.GetAddress(MyEntity.MainAddress, Tenant, ComputingPartnerName, "M");
					AddressPM billingAddress = this.GetAddress(MyEntity.BillingAddress, Tenant, ComputingPartnerName, "B");
					AddressPM pickupDeliveryAddress = this.GetAddress(MyEntity.PickupDeliveryAddress, Tenant, ComputingPartnerName, "P");

					if (mainAddress != null) myCustomer.Addresses.Add(mainAddress);
					if (billingAddress != null) myCustomer.Addresses.Add(billingAddress);
					if (pickupDeliveryAddress != null) myCustomer.Addresses.Add(pickupDeliveryAddress);

					if (MyEntity.Contacts != null && MyEntity.Contacts.Count > 0)
					{
						ContactQueryService contactService1 = new ContactQueryService(Tenant);
						myCustomer.Contacts = contactService1.ContactCustomDataMappingAndValidatin(MyEntity, MyEntity.Contacts, Tenant, ComputingPartnerName);
					}
				}

				return myCustomer;
			}

			catch (Exception ex)
			{
				throw ex;
			}
        }
        private string GetPaymentTermId(PaymentTerm paymentTerm, int tenant, string computingPartnerName)
        {
			PaymentTermQueryService paymentTermService = new PaymentTermQueryService(tenant);
			if (paymentTerm != null)
			{
				var myPaymentTermPM = paymentTermService.PaymentTermDataMappingAndValidatin(paymentTerm, tenant, computingPartnerName);
				if (myPaymentTermPM != null)
					return myPaymentTermPM.Id;
			}

			return null;
		}
		private string GetAddressId(Address address, int tenant, string computingPartnerName)
		{
			AddressQueryService addressService = new AddressQueryService(tenant);
			if (address != null)
			{
				var myAddressPM = addressService.AddressCustomDataMappingAndValidatin(address, tenant, computingPartnerName);
				if (myAddressPM != null)
					return myAddressPM.Id;
			}

			return null;
		}
		private string GetUserId(User user, int tenant, string computingPartnerName)
		{
			UserQueryService userService = new UserQueryService(tenant);
			if (user != null)
			{
				var myUserPM = userService.UserCustomDataMappingAndValidatin(user, tenant, computingPartnerName);
				if (myUserPM != null)
					return myUserPM.Id;
			}

			return null;
		}
		private string GetTeamId(Team team, int tenant, string computingPartnerName)
		{
			TeamQueryService teamService = new TeamQueryService(tenant);
			if (team != null)
			{
				var myTeamPM = teamService.TeamDataMappingAndValidatin(team, tenant, computingPartnerName);
				if (myTeamPM != null)
					return myTeamPM.Id;
			}

			return null;
		}
		private string GetIndustryId(Industry industry, int tenant, string computingPartnerName)
		{
			IndustryQueryService industryService = new IndustryQueryService(tenant);
			if (industry != null)
			{
				var myIndustryPM = industryService.IndustryDataMappingAndValidatin(industry, tenant, computingPartnerName);
				if (myIndustryPM != null)
					return myIndustryPM.Id;
			}

			return null;
		}
		private string GetCurrencyId(Currency currency, int tenant, string computingPartnerName)
		{
			CurrencyQueryService currencyService = new CurrencyQueryService(tenant);
			if (currency != null)
			{
				var myCurrencyPM = currencyService.CurrencyDataMappingAndValidatin(currency, tenant, computingPartnerName);
				if (myCurrencyPM != null)
					return myCurrencyPM.Id;
			}

			return null;
		}
		private string GetVatTypeId(VatType vatType, int tenant, string computingPartnerName)
		{
			VatTypeQueryService vatTypeService = new VatTypeQueryService(tenant);
			if (vatType != null)
			{
				var myVatTypePM = vatTypeService.VatTypeDataMappingAndValidatin(vatType, tenant, computingPartnerName);
				if (myVatTypePM != null)
					return myVatTypePM.Id;
			}

			return null;
		}
		private string GetLeadSourceId(LeadSource leadSource, int tenant, string computingPartnerName)
		{
			LeadSourceQueryService leadSourceService = new LeadSourceQueryService(tenant);
			if (leadSource != null)
			{
				var myLeadSourcePM = leadSourceService.LeadSourceDataMappingAndValidatin(leadSource, tenant, computingPartnerName);
				if (myLeadSourcePM != null)
					return myLeadSourcePM.Id;
			}

			return null;
		}
		private string GetGLAccountId(GLAccount gLAccount, int tenant, string computingPartnerName)
		{
			GLAccountQueryService gLAccountService = new GLAccountQueryService(tenant);
			if (gLAccount != null)
			{
				var myGLAccountPM = gLAccountService.GLAccountCustomDataMappingAndValidatin(gLAccount, tenant);
				if (myGLAccountPM != null)
					return myGLAccountPM.Id;
			}

			return null;
		}
		private AddressPM GetAddress(Address address, int tenant, string computingPartnerName, string addressType)
		{
			AddressQueryService addressService = new AddressQueryService(tenant);
			if (address != null)
			{
				AddressPM addressPM = addressService.AddressDataMappingAndValidatin(address, tenant);
				if (!string.IsNullOrEmpty(address.City))
					addressPM = addressService.AddressCustomDataMappingAndValidatin_CityCountry(address, tenant, computingPartnerName);

				addressPM.AddressTypeId = addressType;
				addressPM.Description = this.GetAddressDescription(addressType);
				addressPM.Tenant = tenant;

				if (!string.IsNullOrEmpty(address.Name))
					addressPM.Name = FormatHelper.ConvertFromBase64(address.Name);

				if (!string.IsNullOrEmpty(address.Address1))
					addressPM.Address1 = FormatHelper.ConvertFromBase64(address.Address1);

				if (!string.IsNullOrEmpty(address.Address2))
					addressPM.Address2 = FormatHelper.ConvertFromBase64(address.Address2);

				return addressPM;
			}

			return null;
		}
		private string GetAddressDescription(string addressType)
		{
			if (addressType == "M")
				return "Main Address";
			if (addressType == "B")
				return "Billing Address";
			if (addressType == "P")
				return "Pickup Delivery Address";
			else
				return "Others";
		}
		public Customer CustomerCustomDataMapping(string code, int Tenant)
        {
            try
            {

                CustomerQueryService customerQueryService = new CustomerQueryService(Tenant);
                var ChargeType = customerQueryService.GetCustomerById(code, Tenant);
                return ChargeType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
