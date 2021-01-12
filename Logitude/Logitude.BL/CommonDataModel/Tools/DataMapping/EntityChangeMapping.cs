using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
   public class EntityChangeMapping
    {

       public static void MapEntity(EntityChangePM entityPM, EntityChange entityPOCO, bool isNewState)
       {
           if (isNewState)
           {
               entityPOCO.Id = entityPM.Id;
               entityPOCO.Tenant = entityPM.Tenant;
           }


           entityPOCO.CheckStartDate = entityPM.CheckStartDate;
           entityPOCO.CreateByUserId = entityPM.CreateByUserId;
           entityPOCO.CreateDate = entityPM.CreateDate;
           entityPOCO.DoneDate = entityPM.DoneDate;
           entityPOCO.ObjectTableId = entityPM.ObjectTableId;
           entityPOCO.EntityId = entityPM.EntityId;
           entityPOCO.HasExecutedRecord = entityPM.HasExecutedRecord;
           entityPOCO.AutomationConditionFieldsXml = entityPM.AutomationConditionFieldsXml;

           entityPOCO.EmailAutomationSsucceedXml = entityPM.EmailAutomationSsucceedXml;
           entityPOCO.SetAutomationSsucceedXml = entityPM.SetAutomationSsucceedXml;
           entityPOCO.EmailAutomationFailedXml = entityPM.EmailAutomationFailedXml;
           entityPOCO.SetAutomationFailedXml = entityPM.SetAutomationFailedXml;

           entityPOCO.ChangesAutomationFieldsXml = entityPM.ChangesAutomationFieldsXml;
           entityPOCO.ChangesFieldsXml = entityPM.ChangesFieldsXml;
           entityPOCO.ExecutionTime = entityPM.ExecutionTime;


            entityPOCO.SetSLAAutomationFailedXml = entityPM.SetSLAAutomationFailedXml;
            entityPOCO.SetSLAAutomationSsucceedXml = entityPM.SetSLAAutomationSsucceedXml;
            entityPOCO.FollowUpAutomationFailedXml = entityPM.FollowUpAutomationFailedXml;
            entityPOCO.FollowUpAutomationSsucceedXml = entityPM.FollowUpAutomationSsucceedXml;

            entityPOCO.QueuedTaskAutomationFailedXml = entityPM.QueuedTaskAutomationFailedXml;
            entityPOCO.QueuedTaskAutomationSsucceedXml = entityPM.QueuedTaskAutomationSsucceedXml;
            entityPOCO.SendInterfaceAutomationFailedXml = entityPM.SendInterfaceAutomationFailedXml;
            entityPOCO.SendInterfaceAutomationSsucceedXml = entityPM.SendInterfaceAutomationSsucceedXml;
            entityPOCO.SendDocumentAutomationFailedXml = entityPM.SendDocumentAutomationFailedXml;
            entityPOCO.SendDocumentAutomationSsucceedXml = entityPM.SendDocumentAutomationSsucceedXml;
        }

    }




}
