using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class PartnersDomainService
    {
        public IQueryable<MAWBStack> GetMAWBStacks(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            mAWBStackRepository = new MAWBStackRepository(tenant);
            return this.mAWBStackRepository.GetMAWBStacks(0);
        }

        public IQueryable<MAWBStack> GetMAWBStacksByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            mAWBStackRepository = new MAWBStackRepository(tenant);
            return this.mAWBStackRepository.GetMAWBStacks(tenant);
        }

        public IQueryable<MAWBStackPM> GetMAWBStackPMsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            mawbStackQuery = new MAWBStackQuery(tenant);
            return this.mawbStackQuery.GetMAWBStackPMsByTenant(tenant);
        }

        public MAWBStackPM GetSingleMAWBStackPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            mawbStackQuery = new MAWBStackQuery(tenant);
            return mawbStackQuery.GetSinglePM(id, tenant);
        }

        public MAWBStackPM GetMAWBStackPMByNumber(int number, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            mawbStackQuery = new MAWBStackQuery(tenant);
            MAWBStackPM stack = mawbStackQuery.GetSingleMAWBStackPMByNumber(number, tenant);
            return stack;
        }

        [Invoke]
        public bool CardHasAssignedMawbStacks(string airlineId, string customerId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            mawbStackQuery = new MAWBStackQuery(tenant);

            return mawbStackQuery.HasMAWBStackPMsForAirlineIdAndCustomerId(airlineId, customerId, tenant);
        }

        public IQueryable<MAWBStackPM> GetMAWBStackPMsByAirlineId(string airlineId, int tenant, int pageSize, int pageIndex)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            mawbStackQuery = new MAWBStackQuery(tenant);
            int skippedStacks = pageSize * (pageIndex - 1);
            IQueryable<MAWBStackPM> query = mawbStackQuery.GetMAWBStackPMsByAirlineId(airlineId, tenant).AsQueryable().OrderBy(a => a.InsertionDate);
            query = query.Skip(skippedStacks);
            query = query.Take(pageSize);
            return query;
        }

        public int GetMAWBStackPMsCountByAirlineId(string airlineId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            mawbStackQuery = new MAWBStackQuery(tenant);
            List<MAWBStackPM> query = mawbStackQuery.GetMAWBStackPMsByAirlineId(airlineId, tenant);
            return query.Count();
        }

        public IQueryable<MAWBStackPM> GetMAWBStackPMsByAirlineIdAndShipperId(string airlineId, string customerId, int tenant, int pageSize, int pageIndex)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            mawbStackQuery = new MAWBStackQuery(tenant);
            int skippedStacks = pageSize * (pageIndex - 1);
            IQueryable<MAWBStackPM> query = mawbStackQuery.GetMAWBStackPMsByAirlineIdAndCustomerId(airlineId, customerId, tenant).AsQueryable().OrderBy(a => a.InsertionDate);
            query = query.Skip(skippedStacks);
            query = query.Take(pageSize);
            return query;
        }

        public int GetMAWBStackPMsCountByAirlineIdAndShipperId(string airlineId, string customerId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            mawbStackQuery = new MAWBStackQuery(tenant);
            List<MAWBStackPM> query = mawbStackQuery.GetMAWBStackPMsByAirlineIdAndCustomerId(airlineId, customerId, tenant);
            return query.Count();
        }



        public void InsertMAWBStack(MAWBStackPM entityPm)
        {
            SecurityUtility.CheckContactFeature("Airline", "NEW", entityPm.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPm.Tenant);
            }
            MAWBStackService service = new MAWBStackService(objectContext, entityPm.Tenant);
            service.Create(entityPm);
            TableLastUpdateClass.UpdateTableHistory(entityPm.Tenant, "MAWBStack");

            //mAWBStackRepository = new MAWBStackRepository(objectContext);
            //bool exist = mAWBStackRepository.GetSingleMAWBStackByNumberAirline(entityPm.Number, entityPm.Tenant, entityPm.AirlineId) != null ? true : false;

            //if (!exist)
            //{
            //    MAWBStack newEntity = new MAWBStack();
            //    newEntity.Id = IdCounter.GetNumber("MAWBStack", entityPm.Tenant).ToString();
            //    entityPm.Id = newEntity.Id;
            //    MapMAWBStackMAWBStack(entityPm, newEntity);
            //    mAWBStackRepository.Add(newEntity);

            //    TableLastUpdateClass.UpdateTableHistory(entityPm.Tenant, "MAWBStack");
            //}

            //else
            //{
            //    string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", entityPm.Tenant);
            //    msg = msg.Replace("%Entity", "Stack Number");
            //    throw new Exception(msg);
            //}
        }

        public void UpdateMAWBStack(MAWBStackPM currententityPm)
        {
            SecurityUtility.CheckContactFeature("Airline", "UPDATE", currententityPm.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currententityPm.Tenant);
            }

            MAWBStackService service = new MAWBStackService(objectContext, currententityPm.Tenant);
            service.Update(currententityPm);
        }

        public void DeleteMAWBStack(MAWBStackPM entityPm)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPm.Tenant);
            }

            mAWBStackRepository = new MAWBStackRepository(objectContext);
            MAWBStack removedEntity = mAWBStackRepository.GetSingleMAWBStack(entityPm.Id, entityPm.Tenant);
            mAWBStackRepository.Remove(removedEntity);
        }

        public void MapMAWBStackMAWBStack(MAWBStackPM entityPm, MAWBStack entity)
        {
            entity.Number = entityPm.Number;
            entity.AirlineId = entityPm.AirlineId;
            entity.Tenant = entityPm.Tenant;
            entity.InsertionDate = entityPm.InsertionDate;
            entity.Notes = entityPm.Notes;
            entity.AssignedToId = entityPm.AssignedToId;
            entityPm.IsUsed = entityPm.IsUsed;
        }

        [Invoke]
        public void CreateMAWBStacks(string airlineId, int tenant, int startNumber, int endNumber,string assignedToId)
        {
            SecurityUtility.CheckContactFeature("Airline", "NEW", tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            string loggedUserId = null;
            ContactRepository contactsRepository = new ContactRepository(objectContext);
            ContactQuery contactQuery = new ContactQuery(contactsRepository);
            ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
            if (contact != null)
            {
                loggedUserId = contact.Id;
            }

            MAWBStackService service = new MAWBStackService(objectContext, tenant);
            service.CreateMAWBStacks(airlineId, startNumber, endNumber, assignedToId, loggedUserId);

            //mAWBStackRepository = new MAWBStackRepository(tenant);
            //mawbStackQuery = new MAWBStackQuery(mAWBStackRepository);

            //ContactRepository contactsRepository = new ContactRepository(tenant);
            //ContactQuery contactQuery  = new ContactQuery(contactsRepository);
            //ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
            //DateTime insertionDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            //List<MAWBStackPM> stacksList = mawbStackQuery.GetAllMAWBStackPMsByAirlineId(airlineId, tenant).ToList();
            //int start = startNumber;

            //while (start <= endNumber)
            //{
            //    int chk = start % 7;
            //    string newNumberStr = start.ToString() + chk;
            //    int newNumber = int.Parse(newNumberStr);

            //    MAWBStackPM stackPm = new MAWBStackPM()
            //    {
            //        AirlineId = airlineId,
            //        Tenant = tenant,
            //        Number = newNumber,
            //        InsertionDate = insertionDate,
            //        AssignedToId = assignedToId,
            //    };

            //    if (stacksList.Where(n => n.Number == newNumber).FirstOrDefault() == null)
            //    {
            //        MAWBStack newEntity = new MAWBStack();
            //        newEntity.Id = IdCounter.GetNumber("MAWBStack", tenant).ToString();
            //        stackPm.Id = newEntity.Id;
            //        MapMAWBStackMAWBStack(stackPm, newEntity);
            //        mAWBStackRepository.Add(newEntity);
            //    }
            //    else
            //    {
            //        string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", tenant);
            //        msg = msg.Replace("%Entity", "Stack Number");
            //        msg += "[" + newNumber+"]";
            //        throw new Exception(msg);
            //    }
            //    start++;
            //}

            //if (contact != null)
            //{
            //    EventTracer.CreateTraceEvent(new EventTracerArgs()
            //    {
            //        Tenant = tenant,
            //        EventTypeCode = "AWBA",
            //        UserId = contact.Id,
            //        EntityId = airlineId,
            //        ObjectTableName = "Airline",
            //        Notes = "AWB stack from [" + startNumber + "] to [" + endNumber + "] added",
            //    });
            //}

            //mAWBStackRepository.SubmitChanges();
        }

        [Invoke]
        public void DeleteMAWBStacksOperation(string airlineId, int tenant, bool isSeriesDelete, string stackId)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            string loggedUserId = null;
            ContactRepository contactsRepository = new ContactRepository(objectContext);
            ContactQuery contactQuery = new ContactQuery(contactsRepository);
            ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);
            if (contact != null)
            {
                loggedUserId = contact.Id;
            }

            MAWBStackService service = new MAWBStackService(objectContext, tenant);
            service.DeleteMAWBStacksOperation(stackId, airlineId, isSeriesDelete, loggedUserId);

            //mAWBStackRepository = new MAWBStackRepository(objectContext);
            //ContactRepository contactsRepository = new ContactRepository(objectContext);
            //ContactQuery contactQuery = new ContactQuery(contactsRepository);
            //ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, true);

            //MAWBStack entity = mAWBStackRepository.GetSingleMAWBStack(stackId, tenant);
            //if (!isSeriesDelete)
            //{
            //    mAWBStackRepository.Remove(entity);
            //    if (contact != null)
            //    {
            //        EventTracer.CreateTraceEvent(new EventTracerArgs()
            //        {
            //            Tenant = tenant,
            //            EventTypeCode = "AWBR",
            //            UserId = contact.Id,
            //            EntityId = airlineId,
            //            ObjectTableName = "Airline",
            //            Notes = "AWB  number [" + entity.Number + "] removed",
            //        });
            //    }
            //}

            //else
            //{
            //    List<MAWBStack> stacksList = mAWBStackRepository.GetMAWBStacksByInsertionDate(tenant, airlineId, entity.InsertionDate);

            //    foreach (MAWBStack stack in stacksList)
            //    {
            //        mAWBStackRepository.Remove(stack);
            //    }

            //    if (contact != null)
            //    {
            //        EventTracer.CreateTraceEvent(new EventTracerArgs()
            //        {
            //            Tenant = tenant,
            //            EventTypeCode = "AWBD",
            //            UserId = contact.Id,
            //            EntityId = airlineId,
            //            ObjectTableName = "Airline",
            //            Notes = "AWB stack inserted on [" + entity.InsertionDate.ToShortDateString() + "] at [" + entity.InsertionDate.ToShortTimeString() + "] removed",
            //        });
            //    }
            //}

            //TableLastUpdateClass.UpdateTableHistory(entity.Tenant, "MAWBStack");
            //mAWBStackRepository.SubmitChanges();
        }
    }
}