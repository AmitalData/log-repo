using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomBankQueryService
    {

        public override void GetComposition(EntityKeyFields entityKeys, CustomBankPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            CustomBankKeys bankKeys = entityKeys as CustomBankKeys;
            CustomBanksCardQueryService customBanksCardQueryService = new CustomBanksCardQueryService(context);

            entityPM.CustomBanksCards = customBanksCardQueryService.GetMulti(bankKeys, true);

        }

        public List<CustomBankList> GetCustomBanksByCard(string cardId, int tenant)
        {
            List<CustomBank> banks = repository.GetCustomBanksForCard(cardId, tenant);
            List<CustomBankList> CustomBanks = new List<CustomBankList>();
            foreach (CustomBank item in banks)
            {
                CustomBankList customBank = new CustomBankList()
                {
                    Id = item.Id,
                    AccountNumber = item.AccountNumber,
                    BankAddress = item.BankAddress,
                    InActive = item.InActive,
                    BankCode = item.BankCode,
                    BranchCode = item.BranchCode,
                    EnglishName = item.EnglishName,
                    InternalCode = item.InternalCode,
                    LocalName = item.LocalName,
                    PayerTypeCode = item.PayerTypeCode,
                    Tenant = item.Tenant,
                    Name = item.LocalName != null ? item.LocalName : item.EnglishName,
                };

                CustomBanks.Add(customBank);
            

            }

            return CustomBanks;

        }

        public CustomBankPM GetCustomBankByCode(string code, int tenant)
        {
            CustomBank bank = repository.GetCustomBanksByCode(code, tenant);
            CustomBankPM bankPM = null;

            if (bank != null)
            {
                bankPM = new CustomBankPM()
                {
                    Id = bank.Id,
                    AccountNumber = bank.AccountNumber,
                    BankAddress = bank.BankAddress,
                    InActive = bank.InActive,
                    BankCode = bank.BankCode,
                    BranchCode = bank.BranchCode,
                    EnglishName = bank.EnglishName,
                    InternalCode = bank.InternalCode,
                    LocalName = bank.LocalName,
                    PayerTypeCode = bank.PayerTypeCode,
                    Tenant = bank.Tenant,
                };

                CustomBanksCardQueryService cardQueryService = new CustomBanksCardQueryService(tenant);
                bankPM.CustomBanksCards = cardQueryService.GetMulti(new CustomBankKeys() { Id = bankPM.Id }, false);

            }

          
            return bankPM;
        }


        public CustomBankPM GetCustomBankByInternalCode(string code, int tenant)
        {
            CustomBank bank = repository.GetCustomBanksByInternalCode(code, tenant);
            CustomBankPM bankPM = null;

            if (bank != null)
            {
                bankPM = new CustomBankPM()
                {
                    Id = bank.Id,
                    AccountNumber = bank.AccountNumber,
                    BankAddress = bank.BankAddress,
                    InActive = bank.InActive,
                    BankCode = bank.BankCode,
                    BranchCode = bank.BranchCode,
                    EnglishName = bank.EnglishName,
                    InternalCode = bank.InternalCode,
                    LocalName = bank.LocalName,
                    PayerTypeCode = bank.PayerTypeCode,
                    Tenant = bank.Tenant,
                };

                CustomBanksCardQueryService cardQueryService = new CustomBanksCardQueryService(tenant);
                bankPM.CustomBanksCards = cardQueryService.GetMulti(new CustomBankKeys() { Id = bankPM.Id }, false);


            }



            return bankPM;
        }
    }
}
