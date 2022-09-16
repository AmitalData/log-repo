using System;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityQueries;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Simplog.Data.Helpers;
using System.Linq;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class MAWBStackService
    {
        bool isNewEntity;
        private int tenant;
        public MAWBStack Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private MAWBStackPM entityPm;
        private ICommonDataContext objectContext;
        private MAWBStackRepository entityRepository;
        public MAWBStackService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new MAWBStackRepository(objectContext);
        }

        public void Create(MAWBStackPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;

            bool exist = entityRepository.GetSingleMAWBStackByNumberAirline(entityPM.Number, entityPM.Tenant, entityPM.AirlineId) != null ? true : false;

            if (!exist)
            {
                this.Poco = new MAWBStack();

                this.Poco.Id = IdCounter.GetNumber("MAWBStack", entityPM.Tenant).ToString();
                this.Poco.Id = this.entityPm.Id;
                MAWBStackMapping.MapEntity(entityPM, Poco, isNewEntity);
                MAWBStackTracing.Trace(entityPM, Poco, isNewEntity);
                MAWBStackValidating.Validate(entityPM);
                
                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", entityPM.Tenant);
                msg = msg.Replace("%Entity", "Stack Number");
                throw new Exception(msg);
            }

        }

        public void Update(MAWBStackPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleMAWBStack(entityPM.Id, entityPm.Tenant);

            bool exist = entityRepository.GetSingleMAWBStackByNumberAirline(entityPM.Number, entityPM.Tenant, entityPM.AirlineId) != null ? true : false;

            if (!exist)
            {
                MAWBStackValidating.Validate(entityPM);
                MAWBStackTracing.Trace(entityPM, Poco, isNewEntity);
                MAWBStackMapping.MapEntity(entityPM, Poco, isNewEntity);
                entityRepository.Update(Poco);
                entityRepository.SubmitChanges();
            }
            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", entityPM.Tenant);
                msg = msg.Replace("%Entity", "Stack Number");
                throw new Exception(msg);
            }
        }

        public void DeleteMAWBStacksOperation(string stackId, string airlineId, bool isSeriesDelete, string loggedUserId)
        {
            MAWBStack entity = entityRepository.GetSingleMAWBStack(stackId, tenant);
            string notes = "";
            string eventCode = "";

            if (!isSeriesDelete)
            {
                this.DeleteSingleAirlineStock(entity, isSeriesDelete);
                notes = "AWB  number [" + entity.Number + "] removed";
                eventCode = "AWBR";
            }

            else
            {
                List<MAWBStack> stacksList = entityRepository.GetMAWBStacksByInsertionDate(tenant, airlineId, entity.InsertionDate);
                foreach (MAWBStack stack in stacksList)
                {
                    this.DeleteSingleAirlineStock(stack, isSeriesDelete);
                }

                notes = "AWB stack inserted on [" + entity.InsertionDate.ToShortDateString() + "] at [" + entity.InsertionDate.ToShortTimeString() + "] removed";
                eventCode = "AWBD";
            }
            
            this.TraceDeletingAirlineStock(eventCode, loggedUserId, airlineId, notes);

            TableLastUpdateClass.UpdateTableHistory(entity.Tenant, "MAWBStack");
            entityRepository.SubmitChanges();
        }
        private void DeleteSingleAirlineStock(MAWBStack entity, bool isSeriesDelete)
        {
            if (entity.IsUsed)
            {
                this.ThrowDeleteAirlineStockException(entity, isSeriesDelete);
            }

            else
            {
                entityRepository.Remove(entity);                             
            }
        }
        private void TraceDeletingAirlineStock(string eventCode, string loggedUserId, string airlineId, string notes)
        {
            if (loggedUserId != null)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = eventCode,
                    UserId = loggedUserId,
                    EntityId = airlineId,
                    ObjectTableName = "Airline",
                    Notes = notes,                    
                });
            }
        }
        private void ThrowDeleteAirlineStockException(MAWBStack entity, bool isSeriesDelete)
        {
            string message = "Can't remove stock since it being used in shipment or booking";
            if(isSeriesDelete)
            {
                message = "Can't remove stock: " + entity .Number + " since it being used in shipment or booking";
            }

            throw new ApplicationException(message);
        }


        public void CreateMAWBStacks(string airlineId, int startNumber, int endNumber, string assignedToId, string loggedUserId)
        {

            DateTime insertionDate = TenantServerConfigration.GetCurrentDateTime(tenant);

            int start = startNumber;
            while (start <= endNumber)
            {
                int chk = start % 7;
                string newNumberStr = start.ToString() + chk;
                int newNumber = int.Parse(newNumberStr);

                MAWBStackPM stackPm = new MAWBStackPM()
                {
                    AirlineId = airlineId,
                    Tenant = tenant,
                    Number = newNumber,
                    InsertionDate = insertionDate,
                    AssignedToId = assignedToId,
                };

                if (!new MAWBStackQuery(entityRepository).CheckMAWBStackExistByAirlineIdAndNumber(airlineId, tenant, newNumber))
                {
                    MAWBStack newEntity = new MAWBStack()
                    {
                        Id = IdCounter.GetNumber("MAWBStack", tenant).ToString(),
                        Tenant = tenant,
                    };

                    stackPm.Id = newEntity.Id;
                    stackPm.Tenant = newEntity.Tenant;
                    MAWBStackMapping.MapEntity(stackPm, newEntity, true);

                    entityRepository.Add(newEntity);
                }

                else
                {
                    string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", tenant);
                    msg = msg.Replace("%Entity", "Stack Number");
                    msg += "[" + newNumber + "]";
                    throw new Exception(msg);
                }

                start++;
            }

            if (loggedUserId != null)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "AWBA",
                    UserId = loggedUserId,
                    EntityId = airlineId,
                    ObjectTableName = "Airline",
                    Notes = "AWB stack from [" + startNumber + "] to [" + endNumber + "] added",
                });
            }

            entityRepository.SubmitChanges();
        }
    }
}
