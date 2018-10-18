
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
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CustomsCollateralsAnswerDataMapping: IMapping<CustomsCollateralsAnswerPM, CustomsCollateralsAnswer>
   {

        public void CustomPMToPOCO(CustomsCollateralsAnswerPM entityPM, CustomsCollateralsAnswer entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.CustomsCollateralId);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            AddPOCOPropertyName(POCOPropertyNames.LineNumber);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.CustomsCollateralId = entityPM.CustomsCollateralId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.LineNumber = entityPM.LineNumber;
            }
        }

        public void CustomPOCOToPM(CustomsCollateralsAnswerPM entityPM, CustomsCollateralsAnswer entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.AnswerForCollateralStatusName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AnswerEntityTypeName);


            if (entityPOCO.AnswerForCollateralStatusCode != null)
            {
                CollateralAnswerStatusQueryService collateralAnswerStatusQueryService = new CollateralAnswerStatusQueryService(entityPOCO.Tenant);
                CollateralAnswerStatusPM collateralAnswerStatus = collateralAnswerStatusQueryService.GetSingle(entityPOCO.AnswerForCollateralStatusCode, false,true);
                entityPM.AnswerForCollateralStatusName = collateralAnswerStatus.LocalName;
            }

            if (entityPOCO.AnswerEntityTypeCode != null)
            {
                CollateralAnswerTypeQueryService collateralAnswerTypeQueryService = new CollateralAnswerTypeQueryService(entityPOCO.Tenant);
                CollateralAnswerTypePM collateralAnswerType = collateralAnswerTypeQueryService.GetSingle(entityPOCO.AnswerEntityTypeCode, false, true);
                entityPM.AnswerEntityTypeName = collateralAnswerType.LocalName;
            }

            if (entityPOCO.RequestFileTypeCode != null)
            {
                CollateralAnswerTypeQueryService requestFileTypeQueryService = new CollateralAnswerTypeQueryService(entityPOCO.Tenant);
                CollateralAnswerTypePM requestFileType = requestFileTypeQueryService.GetSingle(entityPOCO.RequestFileTypeCode, false, true);
                entityPM.RequestFileTypeName = requestFileType.LocalName;
            }

            if (entityPOCO.IsClosed)
            {
                if (entityPOCO.PaymentOrderId != null)
                {
                    PaymentOrderQueryService PaymentOrderQueryService = new PaymentOrderQueryService(entityPOCO.Tenant);
                    PaymentOrderPM paymentOrder = PaymentOrderQueryService.GetSingle(entityPOCO.PaymentOrderId, false, false);
                    entityPM.PaymentOrderNumber = paymentOrder != null ? paymentOrder.PaymentNumber : null;
                    entityPM.PaymentOrderStatus = paymentOrder != null ? paymentOrder.PaymentStatusName : null;
                }
            }

        }
   }


}
   