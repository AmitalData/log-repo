using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class APILogsMapping
    {
        public static void MapEntity(APILogsPM entityPM, APILogs entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.CreateDate = entityPM.CreateDate;
                entityPOCO.CreateDateUTC = entityPM.CreateDateUTC;
            }

            entityPOCO.Direction = entityPM.Direction;
            entityPOCO.CorrelationId = entityPM.CorrelationId;

            entityPOCO.EntityId = entityPM.EntityId;

            entityPOCO.ExpirationDate = entityPM.ExpirationDate;

            entityPOCO.LastExceptionMessage = entityPM.LastExceptionMessage;

            entityPOCO.LastUpdateDate = entityPM.LastUpdateDate;
            entityPOCO.LastUpdateDateUTC = entityPM.LastUpdateDateUTC;

            entityPOCO.NumberOfRetries = entityPM.NumberOfRetries;
            entityPOCO.ObjectTableId = entityPM.ObjectTableId;
            entityPOCO.PartnerName = entityPM.PartnerName;
            entityPOCO.Refrence = entityPM.Refrence;
            //entityPOCO.SearchFields = entityPM.SearchFields;
            entityPOCO.Status = entityPM.Status;
            entityPOCO.Subject = entityPM.Subject;
            entityPOCO.CustomerId = entityPM.CustomerId;
            entityPOCO.BatchNumber = entityPM.BatchNumber;
            entityPOCO.QueueMessageMoreDetailsId = entityPM.QueueMessageMoreDetailsId;
            entityPOCO.QueueType = entityPM.QueueType;
            entityPOCO.QueueMessage = entityPM.QueueMessage;
            BuildSearchField(entityPM, entityPOCO);
        }

        public static void BuildSearchField(APILogsPM entityPM, APILogs entityPoco)
        {
            string mySearchFields = "";
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.BatchNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomerId);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Subject);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Status);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Refrence);

            entityPM.SearchFields = mySearchFields;
            entityPoco.SearchFields = mySearchFields;
        }
    }
}
