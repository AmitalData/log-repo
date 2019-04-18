using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class EntityChangeQuery
    {
        EntityChangeRepository repository;
        public EntityChangeQuery(int tenant)
        {
            repository = new EntityChangeRepository(tenant);
        }
        public EntityChangeQuery(EntityChangeRepository EntityChangeRepository)
        {
            repository = EntityChangeRepository;
        }

        public List<EntityChangePM> GetEntityChangePMsByEntityIdAndObjectTable(string entityId, string objectTable, int tenant)
        {
            List<EntityChangePM> entityChangees = (from a in repository.context.EntityChanges
                                                   where a.Tenant == tenant && a.EntityId == entityId && a.ObjectTableId == objectTable
                                                   select new EntityChangePM()
                                                   {
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       CheckStartDate = a.CheckStartDate,
                                                       CreateByUserId = a.CreateByUserId,
                                                       CreateDate = a.CreateDate,
                                                       DoneDate = a.DoneDate,
                                                       EntityId = a.EntityId,
                                                       HasExecutedRecord = a.HasExecutedRecord,
                                                       ObjectTableId = a.ObjectTableId,
                                                       ExecutionTime = a.ExecutionTime,
                                                   }).OrderByDescending(d => d.CreateDate).ToList();
            #region CreateByUserName

            if (entityChangees.Count > 0)
            {
                List<string> contactIds = new List<string>();
                foreach (EntityChangePM item in entityChangees)
                {
                    if (!string.IsNullOrEmpty(item.CreateByUserId))
                    {
                        if (!contactIds.Contains(item.CreateByUserId)) contactIds.Add(item.CreateByUserId);

                    }
                }

                List<ContactList> contactLists = null;
                if (contactIds.Count > 0)
                {
                    ContactQuery contactQuery = new ContactQuery(tenant);
                    contactLists = contactQuery.GetContactListsByListIds(contactIds, tenant).ToList();


                    foreach (EntityChangePM item in entityChangees)
                    {
                        if (!string.IsNullOrEmpty(item.CreateByUserId) && contactLists != null)
                        {
                            ContactList contactList = contactLists.Where(d => d.Id == item.CreateByUserId).FirstOrDefault();
                            if (contactList != null) item.CreateByUserName = contactList.EnglishName;

                        }

                    }

                }
                #endregion


            }

            return entityChangees;
        }

        public EntityChangePM GetSinglePM(string id,  int tenant)
        {

            var query = (from a in repository.context.EntityChanges.Include("User.Contact")
                         where a.Tenant == tenant && a.Id == id 
                         select new EntityChangePM()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             AutomationConditionFieldsXml = a.AutomationConditionFieldsXml,
                             CheckStartDate = a.CheckStartDate,
                             CreateByUserId = a.CreateByUserId,
                             CreateDate = a.CreateDate,
                             DoneDate = a.DoneDate,
                             EntityId = a.EntityId,
                             EmailAutomationFailedXml = a.EmailAutomationFailedXml,
                             SetAutomationFailedXml = a.SetAutomationFailedXml,
                             EmailAutomationSsucceedXml = a.EmailAutomationSsucceedXml,
                             SetAutomationSsucceedXml = a.SetAutomationSsucceedXml,
                             ChangesAutomationFieldsXml = a.ChangesAutomationFieldsXml,
                             ChangesFieldsXml = a.ChangesFieldsXml,
                             HasExecutedRecord = a.HasExecutedRecord,
                             ObjectTableId = a.ObjectTableId,
                             ExecutionTime = a.ExecutionTime,
                             CreateByUserName = a.CreateByUser != null ? a.CreateByUser.Contact.EnglishName : "",
                             FollowUpAutomationFailedXml = a.FollowUpAutomationFailedXml,
                             FollowUpAutomationSsucceedXml = a.FollowUpAutomationSsucceedXml,
                             SetSLAAutomationFailedXml = a.SetSLAAutomationFailedXml,
                             SetSLAAutomationSsucceedXml = a.SetSLAAutomationSsucceedXml,
                      
                         }).FirstOrDefault();
            return query;
        }

        public IQueryable<EntityChangeList> GetIQueryableEntityList(IQueryable<EntityChange> iQueryable)
        {
            IQueryable<EntityChangeList> result = from a in iQueryable
                                                  select new EntityChangeList()
                                                  {
                                                      Id = a.Id,
                                                      Tenant = a.Tenant,
                                                      AutomationConditionFieldsXml = a.AutomationConditionFieldsXml,
                                                      CheckStartDate = a.CheckStartDate,
                                                      CreateByUserId = a.CreateByUserId,
                                                      CreateDate = a.CreateDate,
                                                      DoneDate = a.DoneDate,
                                                      EntityId = a.EntityId,
                                                      HasExecutedRecord = a.HasExecutedRecord,
                                                      ObjectTableId = a.ObjectTableId,
                                                      ExecutionTime = a.ExecutionTime,

                                                  };
            return result;
        }

    }
}
