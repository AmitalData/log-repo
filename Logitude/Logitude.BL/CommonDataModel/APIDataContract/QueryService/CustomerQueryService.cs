using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
	public partial class CustomerQueryService
	{
		private int tenant;
		private string computingPartnerCode;
		public CustomerPM CustomerCustomDataMappingAndValidating(Customer MyEntity, int tenant, string computingPartnerCode = "")
		{
			try
			{
				this.tenant = tenant;
				this.computingPartnerCode = computingPartnerCode;
				UserQuery userQuery = new UserQuery(tenant);
				UserPM MyUserPM = userQuery.GetSingleUserPMByEmail("system@tenant" + tenant + ".com", tenant, false);

				CustomerPM temp = this.MapAndValidate(MyEntity);
				temp.Tenant = tenant;
				temp.PartnerTypeId = MyEntity.IsPotential ? "PO" : "CS";
				temp.CustomerStatusCode = MyEntity.IsPotential ? "POT" : "ACT";
				temp.IsCustomer = true;
				temp.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
				temp.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);

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

		private CustomerPM MapAndValidate(Customer MyEntity, bool isUpdate = false)
		{
			try
			{
				CustomerQuery myQuery = new CustomerQuery(tenant);
				CustomerPM myCustomer = new CustomerPM();
				if (!string.IsNullOrEmpty(MyEntity.Id))
				{
					myCustomer = myQuery.GetSinglePM(MyEntity.Id, tenant);
				}

				if (myCustomer == null)
				{
					throw new ApplicationException("Customer with Id " + MyEntity.Id + " doesn't exist");
				}

				if (!string.IsNullOrEmpty(MyEntity.Code))
				{
					myCustomer = myQuery.GetSingleCustomerPMByCode(MyEntity.Code, tenant);
				}

				if (myCustomer == null)
				{
					throw new ApplicationException("Customer with Code " + MyEntity.Code + " doesn't exist");
				}

				myCustomer.Id = MyEntity.Id;
				myCustomer.Code = MyEntity.Code;
				myCustomer.IsPotential = MyEntity.IsPotential;

				if (!isUpdate)
				{
					myCustomer.EnglishName = MyEntity.EnglishName;
					myCustomer.VatNumber = MyEntity.VatNumber;
					myCustomer.LeadDescription = MyEntity.LeadDescription;
					myCustomer.ReceivablesAccountingCard = MyEntity.ReceivableExternalId;

					if (!string.IsNullOrEmpty(MyEntity.LocalName))
						myCustomer.LocalName = FormatHelper.ConvertFromBase64(MyEntity.LocalName);

					myCustomer.PaymentTermId = this.GetPaymentTermId(MyEntity.PaymentTerm);
					myCustomer.AccountManagerUserId = this.GetUserId(MyEntity.AccountManagerUser);
					myCustomer.SalesmanUserId = this.GetUserId(MyEntity.SalesmanUser);
					myCustomer.CollectorId = this.GetUserId(MyEntity.Collector);
					myCustomer.TeamId = this.GetTeamId(MyEntity.Team);
					myCustomer.IndustryId = this.GetIndustryId(MyEntity.Industry);
					myCustomer.InvoiceCurrencyId = this.GetCurrencyId(MyEntity.InvoiceCurrency);
					myCustomer.VatTypeId = this.GetVatTypeId(MyEntity.VatType);
					myCustomer.LeadSourceId = this.GetLeadSourceId(MyEntity.LeadSource);
					myCustomer.GLAccountId = this.GetGLAccountId(MyEntity.GLAccount);
					myCustomer.CustomerSizeId = this.GetCustomerSizeId(MyEntity.CustomerSize);

					AddressPM mainAddress = this.GetAddress(MyEntity.MainAddress, "M", myCustomer.EnglishName);
					AddressPM billingAddress = this.GetAddress(MyEntity.BillingAddress, "B", myCustomer.EnglishName);
					AddressPM pickupDeliveryAddress = this.GetAddress(MyEntity.PickupDeliveryAddress, "P", myCustomer.EnglishName);

					if (mainAddress != null)
					{
						if (MyEntity.IsPotential) 
							this.SetPotentialAddressFieldsOnCustomer(myCustomer, mainAddress);
						else 
							myCustomer.Addresses.Add(mainAddress);
					}

					if (billingAddress != null) myCustomer.Addresses.Add(billingAddress);
					if (pickupDeliveryAddress != null) myCustomer.Addresses.Add(pickupDeliveryAddress);

					if (MyEntity.Contacts != null && MyEntity.Contacts.Count > 0)
					{
						ContactQueryService contactService1 = new ContactQueryService(tenant);
						myCustomer.Contacts = contactService1.ContactCustomDataMappingAndValidatin(MyEntity, MyEntity.Contacts, tenant, computingPartnerCode);
					}

					CustomFieldQueryService customFieldService = new CustomFieldQueryService(tenant, "Customer");
					if (MyEntity.CustomFields != null)
					{
						customFieldService.CustomFieldCustomDataMappingAndValidatin(MyEntity.CustomFields, myCustomer, tenant);
					}
				}

				return myCustomer;
			}

			catch (Exception ex)
			{
				throw ex;
			}
		}
		private string GetPaymentTermId(PaymentTerm paymentTerm)
		{
			PaymentTermQueryService paymentTermService = new PaymentTermQueryService(tenant);
			if (paymentTerm != null)
			{
				var myPaymentTermPM = paymentTermService.PaymentTermDataMappingAndValidatin(paymentTerm, tenant, computingPartnerCode);
				if (myPaymentTermPM != null)
					return myPaymentTermPM.Id;
			}

			return null;
		}
		private string GetUserId(User user)
		{
			UserQueryService userService = new UserQueryService(tenant);
			if (user != null)
			{
				var myUserPM = userService.UserDataMappingAndValidatin(user, tenant, computingPartnerCode);
				if (myUserPM != null)
					return myUserPM.Id;
			}

			return null;
		}
		private string GetTeamId(Team team)
		{
			TeamQueryService teamService = new TeamQueryService(tenant);
			if (team != null)
			{
				var myTeamPM = teamService.TeamDataMappingAndValidatin(team, tenant, computingPartnerCode);
				if (myTeamPM != null)
					return myTeamPM.Id;
			}

			return null;
		}
		private string GetIndustryId(Industry industry)
		{
			IndustryQueryService industryService = new IndustryQueryService(tenant);
			if (industry != null)
			{
				var myIndustryPM = industryService.IndustryDataMappingAndValidatin(industry, tenant, computingPartnerCode);
				if (myIndustryPM != null)
					return myIndustryPM.Id;
			}

			return null;
		}
		private string GetCurrencyId(Currency currency)
		{
			CurrencyQueryService currencyService = new CurrencyQueryService(tenant);
			if (currency != null)
			{
				var myCurrencyPM = currencyService.CurrencyDataMappingAndValidatin(currency, tenant, computingPartnerCode);
				if (myCurrencyPM != null)
					return myCurrencyPM.Id;
			}

			return null;
		}
		private string GetVatTypeId(VatType vatType)
		{
			VatTypeQueryService vatTypeService = new VatTypeQueryService(tenant);
			if (vatType != null)
			{
				var myVatTypePM = vatTypeService.VatTypeDataMappingAndValidatin(vatType, tenant, computingPartnerCode);
				if (myVatTypePM != null)
					return myVatTypePM.Id;
			}

			return null;
		}
		private string GetLeadSourceId(LeadSource leadSource)
		{
			LeadSourceQueryService leadSourceService = new LeadSourceQueryService(tenant);
			if (leadSource != null)
			{
				var myLeadSourcePM = leadSourceService.LeadSourceDataMappingAndValidatin(leadSource, tenant, computingPartnerCode);
				if (myLeadSourcePM != null)
					return myLeadSourcePM.Id;
			}

			return null;
		}
		private string GetGLAccountId(GLAccount gLAccount)
		{
			GLAccountQueryService gLAccountService = new GLAccountQueryService(tenant);
			if (gLAccount != null)
			{
				var myGLAccountPM = gLAccountService.GLAccountCustomDataMappingAndValidatin(gLAccount, tenant, computingPartnerCode);
				if (myGLAccountPM != null)
					return myGLAccountPM.Id;
			}

			return null;
		}
		private string GetCustomerSizeId(CustomerSize customerSize)
		{
			CustomerSizeQueryService customerSizeQuery = new CustomerSizeQueryService(tenant);
			if (customerSize != null)
			{
				var myCustomerSizePM = customerSizeQuery.CustomerSizeDataMappingAndValidatin(customerSize, tenant, computingPartnerCode);
				if (myCustomerSizePM != null)
					return myCustomerSizePM.Id;
			}

			return null;
		}
		private AddressPM GetAddress(Address address, string addressType, string customerName)
		{
			AddressQueryService addressService = new AddressQueryService(tenant);
			if (address != null)
			{
				AddressPM addressPM = addressService.AddressDataMappingAndValidatin(address, tenant, computingPartnerCode);
				if (!string.IsNullOrEmpty(address.City))
					addressPM = addressService.AddressCustomDataMappingAndValidatin_CityCountry(address, tenant, computingPartnerCode);

				addressPM.AddressTypeId = addressType;
				addressPM.Description = this.GetAddressDescription(addressType);
				addressPM.Tenant = tenant;

				if (!string.IsNullOrEmpty(address.Name))
					addressPM.Name = FormatHelper.ConvertFromBase64(address.Name);
				else
					addressPM.Name = customerName;

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
		private void SetPotentialAddressFieldsOnCustomer(CustomerPM myCustomer, AddressPM mainAddress)
		{
			myCustomer.City_Potential = mainAddress.City;
			myCustomer.CountryId_Potential = mainAddress.CountryId;
			myCustomer.Address1_Potential = mainAddress.Address1;
			myCustomer.StateId_Potential = mainAddress.StateId;
			myCustomer.Address2_Potential = mainAddress.Address2;
			myCustomer.ZipCode_Potential = mainAddress.ZipCode;
			myCustomer.FaxNumber_Potential = mainAddress.FaxNumber;
			myCustomer.PhoneNumber_Potential = mainAddress.PhoneNumber;
		}

		public List<Contact> GetCustomerContacts(Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Customer customer, int tenant)
        {
			string customerId = customer?.Id;
			string primaryContactId = customer?.PrimaryContact?.Id;
			if (string.IsNullOrEmpty(customerId)) return null;

			List<Contact> contacts = new List<Contact>();
			ContactQuery entityQuery = new ContactQuery(tenant);
			List<ContactPM> result = entityQuery.GetContactsbyCustomerId(customerId, tenant).ToList();
			result.ForEach(item =>
			{
				contacts.Add(new Contact()
				{
					Email = item.Email,
					EnglishName = item.EnglishName,
					BusinessPhone = item.BusinessPhone,
					Mobile = item.Mobile,
					IsPrimaryContact = (primaryContactId == item.Id ? true: false),
					Notes = item.Notes,
					InActive = item.InActive,
				});
			});

			return contacts;
		}
	}
}
