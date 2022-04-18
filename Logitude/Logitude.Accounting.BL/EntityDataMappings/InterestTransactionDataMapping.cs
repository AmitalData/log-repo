
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class InterestTransactionDataMapping: IMapping<InterestTransactionPM, InterestTransaction>
   {

        public void CustomPMToPOCO(InterestTransactionPM entityPM, InterestTransaction entityPOCO)
        {
           
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.InterestValueDate);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
               
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.Id = entityPM.Id;
                entityPOCO.InterestValueDate = entityPM.InterestValueDate.Date;
            }

            //BuildSearchField(entityPM, entityPOCO);
        }

        public void CustomPOCOToPM(InterestTransactionPM entityPM, InterestTransaction entityPOCO)
        {
        }

        private void BuildSearchField(InterestTransactionPM entityPM, InterestTransaction entityPOCO)
        {
            InterestTransactionList entityList = GetEntityList(entityPM);

            string searchFields = "";

            MethodHelper.AddToSearchFields(ref searchFields, entityList.JournalNumber);
            MethodHelper.AddToSearchFields(ref searchFields, entityList.Source);

            entityPM.SearchFields = searchFields;
            entityPOCO.SearchFields = searchFields;
        }

        private InterestTransactionList GetEntityList(InterestTransactionPM entityPM)
        {
            var context = AccountingContext.GetContext(entityPM.Tenant);
            InterestTransactionListQueryService listQuery = new InterestTransactionListQueryService(context);
            var entityList = listQuery.GetSingle(entityPM.Id, entityPM.Tenant);
            return entityList;
        }
    }


}
   