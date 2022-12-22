
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data;
using Logitude.DashboardModule.Data.Repositories;

namespace Logitude.DashboardModule.BL.EntityDataMappings
{

    public partial class DashboardGlobalFilterDataMapping : IMapping<DashboardGlobalFilterPM, DashboardGlobalFilter>
    {

        public void CustomPMToPOCO(DashboardGlobalFilterPM entityPM, DashboardGlobalFilter entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert && entityPOCO != null)
            {
                entityPOCO.Id = entityPM.Id;
            }
        }

        public void CustomPOCOToPM(DashboardGlobalFilterPM entityPM, DashboardGlobalFilter entityPOCO)
        {
            GetDataSetField(entityPM, entityPOCO);
        }

        private static void GetDataSetField(DashboardGlobalFilterPM entityPM, DashboardGlobalFilter entityPOCO)
        {
            if (entityPOCO.DataSetFieldId == null) return;

            var dataSetField = new AnalyticsFactsFieldsMetaDataRepository(entityPOCO.Tenant).GetSingle(entityPOCO.DataSetFieldId, 0);
            if (dataSetField == null) return;

            entityPM.JoinedTableName = dataSetField.JoinedTableName;
            entityPM.FieldCode = dataSetField.FieldCode;
        }
    }


}
