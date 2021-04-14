
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ExportStorgeDataMapping: IMapping<ExportStorgePM, ExportStorge>
   {

        public void CustomPMToPOCO(ExportStorgePM entityPM, ExportStorge entityPOCO)
        {

            entityPOCO.DeclarationId = entityPM.DeclarationId;
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(ExportStorgePM entityPM, ExportStorge entityPOCO)
        {
            //throw new NotImplementedException();
        }
        private static void BuildSearchFields(ExportStorgePM entityPM, ExportStorge poco, bool isNewEntity)
        {
            string result = "";
            result = result + "," + entityPM.StorageNo;
            if (!string.IsNullOrEmpty(entityPM.ExportFileNo))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.ExportFileNo : result + "," + entityPM.ExportFileNo;
            }
            if (!string.IsNullOrEmpty(entityPM.CustomFileNo))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.CustomFileNo : result + "," + entityPM.CustomFileNo;
            }
            if (!string.IsNullOrEmpty(entityPM.ContainerNumber))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.ContainerNumber : result + "," + entityPM.ContainerNumber;
            }
            if (!string.IsNullOrEmpty(entityPM.ExporterRef))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.ExporterRef : result + "," + entityPM.ExporterRef;
            }

            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }
    }


}
   