using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Customs.Data.Utils;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class ClaimListQueryService
    {
	    private IQueryable<ClaimList> GetIqueryableList(IQueryable<Claim> iQueryable)
        {
            IQueryable<ClaimList> query = (from a in iQueryable.Include("ImporterTypeForClaim").Include("PassportType").Include("PassportCountryType").Include("ClaimSubmiterType").Include("BeneficiaryActivityType").Include("AccountCountry").Include("AccountCurrencyType").Include("AccountBranch")
                                           join d in context.Tapags.Include("CustomerCard").Include("ReferantUser")      
                                           on a.Id equals d.Id 

                                            select new ClaimList()
											{
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                ImporterClaimTypeCode = a.ImporterClaimTypeCode,
                                                ImporterClaimTypeName = a.ImporterClaimType.LocalName != null ? a.ImporterClaimType.LocalName : a.ImporterClaimType.EnglishName,
                                                SoldierPersonalNumber = a.SoldierPersonalNumber,
                                                SubmitDate = a.SubmitDate,
					                            //ClientId = a.ClientId,					
					                            PassportTypeCode = a.PassportTypeCode,
                                                PassportTypeName = a.PassportType.LocalName != null ? a.PassportType.LocalName : a.PassportType.EnglishName,
                                                PassportNumber = a.PassportNumber,
                                                PassportCountryTypeCode = a.PassportCountryTypeCode,
                                                PassportCountryTypeName = a.PassportCountryType.LocalName != null ? a.PassportCountryType.LocalName : a.PassportCountryType.EnglishName,
                                                CustomsAddressCode = a.CustomsAddressCode,
                                                ContactPhoneAddressCode = a.ContactPhoneAddressCode,
                                                ClaimSubmiterNumber = a.ClaimSubmiterNumber,
                                                ClaimSubmiterTypeCode = a.ClaimSubmiterTypeCode,
                                                ClaimSubmiterTypeName = a.ClaimSubmiterType.LocalName != null ? a.ClaimSubmiterType.LocalName : a.ClaimSubmiterType.EnglishName,
                                                HebrewCorporationName = a.HebrewCorporationName,
                                                AddressCode = a.AddressCode,
                                                BeneficiaryActivityTypeCode = a.BeneficiaryActivityTypeCode,
                                                BeneficiaryActivityTypeName = a.BeneficiaryActivityType.LocalName != null ? a.BeneficiaryActivityType.LocalName : a.BeneficiaryActivityType.EnglishName,
                                                AccountCountryCode = a.AccountCountryCode,
                                                AccountCountryName = a.AccountCountry.LocalName != null ? a.AccountCountry.LocalName : a.AccountCountry.EnglishName,
                                                BankTypeCode = a.BankTypeCode,
                                                AccountBranchCode = a.AccountBranchCode,
                                                AccountBranchName = a.AccountBranch.LocalName != null ? a.AccountBranch.LocalName : a.AccountBranch.EnglishName,
                                                AccountNumber = a.AccountNumber,
                                                AccountCurrencyTypeCode = a.AccountCurrencyTypeCode,
                                                AccountCurrencyTypeName = a.AccountCurrencyType.LocalName != null ? a.AccountCurrencyType.LocalName : a.AccountCurrencyType.EnglishName,
                                                ForeignBank = a.ForeignBank,
                                                ForeignBranch = a.ForeignBranch,
                                                ForeignAccountNumber = a.ForeignAccountNumber,
                                                ImporterAffidavit = a.ImporterAffidavit,
                                                RawMaterialsDescription = a.RawMaterialsDescription,
                                                CustomsFiles = a.CustomsFiles,
					                            SearchFields = a.SearchFields,
                                                TapagNumber = d.TapagNumber,
                                                LeadingFileNumber = d.LeadingFileNumber,
                                                TapagTypeCode = d.TapagTypeCode,
                                                CustomerId = d.CustomerId,
                                                CustomerName = d.CustomerCard.LocalName != null ? d.CustomerCard.LocalName : d.CustomerCard.EnglishName,
                                                //ImporterId = d.ImporterId,
                                                CreateDate = d.CreateDate,
                                                FollowDate = d.FollowDate,
                                                ValidityDate = d.ValidityDate,
                                                IsClosed = d.IsClosed,
                                                TapagId = d.Id,
                                                ReferantName = d.ReferantUser.Contact.LocalName != null ? d.ReferantUser.Contact.LocalName : d.ReferantUser.Contact.EnglishName,
                                                CustomsBranchCode = d.CustomsBranchCode,
                                                CustomsBranchName = d.CustomsBranch != null ? d.CustomsBranch.LocalName : null,
                                            });

            //Freelancer filtering
            query = GetFreelancerQuery(query);

            return query;
		}

		private IQueryable<Claim> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Claim> iQueryable, int tenant)
        {
            return iQueryable;
		}

        public IQueryable<ClaimList> GetFreelancerQuery(IQueryable<ClaimList> queryableData)
        {
            int tenant = FreelancerCustomersUtil.GetLoggedTenant(); // get current logged tenant 

            FreelancerCustomersUtil frlUtil = new FreelancerCustomersUtil(tenant);
            if (frlUtil.user.IsFreelancer)
            {
                List<string> customersIds = frlUtil.GetConnectedCustomersIds(tenant);
                if (customersIds.Count > 0)
                {
                    queryableData = queryableData.Where(d => customersIds.Contains(d.CustomerId));
                }
            }

            return queryableData;
        }
    }


}
	