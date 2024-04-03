using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
   public partial class InterestTransactionUpdateService
    {

        protected override void OnCreating(InterestTransactionPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("InterestTransaction", entityPM.Tenant);
            entityPM.CreateDateTime = DateTime.UtcNow;
        }


        protected override void OnUpdating(InterestTransactionPM entityPM)
        {
            entityPM.UpdateDateTime = DateTime.UtcNow;
        }

        protected override void AfterUpdating(InterestTransactionPM entityPM, EntityPM entityParentPM)
        {
            string arInvoiceInterestEntityType = "1";
            string journalInterestEntityTypeCode = "3";
            if (entityPM.InterestEntityType != arInvoiceInterestEntityType && 
                (entityPM.InterestEntityType != journalInterestEntityTypeCode || 
                    (String.IsNullOrEmpty(entityPM.SearchFields) && String.IsNullOrEmpty(EntityPOCO.SearchFields))))
            {
                BuildSearchField(entityPM);
            }
            
            Repository.Update(EntityPOCO);
            SubmitChanges();

        }

        private void BuildSearchField(InterestTransactionPM entityPM)
        {
            InterestTransactionList entityList = GetEntityList(entityPM);

            string searchFields = "";

            MethodHelper.AddToSearchFields(ref searchFields, entityList.JournalNumber);
            MethodHelper.AddToSearchFields(ref searchFields, entityList.Source);
            MethodHelper.AddToSearchFields(ref searchFields, entityList.LocalAmount.ToString());
            MethodHelper.AddToSearchFields(ref searchFields, entityList.ForeignAmount.ToString());

            IAccountingContext context = AccountingContext.GetContext(entityPM.Tenant);
            GLAccountQueryService _myGLAccountQueryService = new GLAccountQueryService(context);
            var GLAccountDisplayNumber = _myGLAccountQueryService.GetDisplayNumberByGLAccountId(entityPM.GLAccountId, entityPM.Tenant);

            if (GLAccountDisplayNumber != null)
                MethodHelper.AddToSearchFields(ref searchFields, GLAccountDisplayNumber);

            entityPM.SearchFields = searchFields;
            EntityPOCO.SearchFields = searchFields;
        }

        private InterestTransactionList GetEntityList(InterestTransactionPM entityPM)
        {
            InterestTransactionListQueryService listQuery = new InterestTransactionListQueryService(currentContext);
            var entityList = listQuery.GetSingle(entityPM.Id, entityPM.Tenant);
            return entityList;
        }


    }
}
