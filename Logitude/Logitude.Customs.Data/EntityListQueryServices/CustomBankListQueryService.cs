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
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.Data.Repsitories;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class CustomBankListQueryService
    {
       
	    private IQueryable<CustomBankList> GetIqueryableList(IQueryable<CustomBank> iQueryable)
        {

            var customBanks = (from customBanksCard in context.CustomBanksCards 
                                                 group customBanksCard by customBanksCard.CustomBankId  into g 
                                                 orderby g.Key select g);

         

            List<string> customBankIds = new List<string>();
            foreach (var item in customBanks)
            {
                customBankIds.Add(item.Key);
            }



            IQueryable<CustomBankList> result = from entity in iQueryable.Include("CustomerActivityType").Include("Bank").Include("Branch")
                                                select new CustomBankList()
                                                 {
                                                     Id = entity.Id,
                                                     AccountNumber = entity.AccountNumber,
                                                    ClientBank = customBankIds.Contains(entity.Id) ? true : false,
                                                     BankAddress = entity.BankAddress,
                                                     Tenant = entity.Tenant,
                                                     InActive = entity.InActive,
                                                     BankCode = entity.BankCode,
                                                     BranchCode = entity.BranchCode,
                                                     BankName = entity.Bank != null? entity.Bank.LocalName : null,
                                                     BranchName = entity.CustomsBranch != null ? entity.CustomsBranch.LocalName : null,
                                                     InternalCode = entity.InternalCode,
                                                     PayerTypeCode = entity.PayerTypeCode,
                                                     SearchFields = entity.SearchFields,
                                                     EnglishName = entity.EnglishName,
                                                     LocalName = entity.LocalName,
                                                     PayerTypeName = entity.CustomerActivityType != null ? entity.CustomerActivityType.LocalName : null,
                                                    Name = entity.LocalName != null ? entity.LocalName : entity.EnglishName,
                                                };
            return result;

        
		}

        private IQueryable<CustomBank> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CustomBank> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	