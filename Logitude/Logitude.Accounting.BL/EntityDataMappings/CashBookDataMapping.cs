using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Web;
using System;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class CashBookDataMapping: IMapping<CashBookPM, CashBook>
   {

        public void CustomPMToPOCO(CashBookPM entityPM, CashBook entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(CashBookPM entityPM, CashBook entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CashBookTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CurrencyName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CurrencyCode);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CurrencySign);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CreatedByUserName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.UpdatedByUserName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AccountNumber);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AccountName);


            // GET logged contact, RTL
            ContactPM contact = GetLoggedContact(entityPOCO.Tenant) ?? new ContactPM();
            bool showLocals = !contact.DontShowLocal;

            if (entityPOCO.CashBookTypeCode != null)
            {
                CashBookTypeQueryService cashBookTypeQueryService = new CashBookTypeQueryService(entityPOCO.Tenant);
                CashBookTypePM cashBookType = cashBookTypeQueryService.GetSingle(entityPOCO.CashBookTypeCode, false, true);
                if (cashBookType != null) entityPM.CashBookTypeName = (showLocals ? cashBookType.LocalName : cashBookType.EnglishName);
            }

            if (entityPOCO.CurrencyId != null)
            {
                CurrencyQuery currencyQueryService = new CurrencyQuery(entityPOCO.Tenant);
                CurrencyPM currency = currencyQueryService.GetSinglePM(entityPOCO.CurrencyId, entityPOCO.Tenant);
                if (currency != null)
                {
                    entityPM.CurrencyName = currency.EnglishName;
                    entityPM.CurrencyCode = currency.Code;
                    entityPM.CurrencySign = currency.Sign;
                }
            }


            if (entityPOCO.AccountId != null)
            {
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(entityPOCO.Tenant);
                GLAccountPM account = gLAccountQueryService.GetSingle(entityPOCO.AccountId, false, true);
                if (account != null)
                {
                    entityPM.AccountName = (showLocals ? account.LocalName : account.EnglishName); 
                    entityPM.AccountNumber = account.DisplayNumber;
                }
            }

            if (entityPOCO.CreatedByUserId != null)
            {
                Contact userContact = ContactRepository.GetSingleContact(entityPOCO.CreatedByUserId, entityPOCO.Tenant, true);
                if (userContact != null)
                {
                    entityPM.CreatedByUserName = (showLocals ? userContact.LocalName : userContact.EnglishName); 
                }
            }

            if (entityPOCO.UpdatedByUserId != null)
            {
                Contact userContact = ContactRepository.GetSingleContact(entityPOCO.UpdatedByUserId, entityPOCO.Tenant, true);
                if (userContact != null)
                {
                    entityPM.UpdatedByUserName = (showLocals ? userContact.LocalName : userContact.EnglishName); 
                }
            }

            if (entityPOCO.BranchId != null)
            {
                BranchQuery branchQueryService = new BranchQuery(entityPOCO.Tenant);
                BranchPM branch = branchQueryService.GetSinglePM(entityPOCO.BranchId, entityPOCO.Tenant);
                if (branch != null)
                {
                    entityPM.BranchName = (showLocals ? branch.LocalName : branch.EnglishName);
                }
            }

        }


        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }



        private static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }

        
    }


}
   