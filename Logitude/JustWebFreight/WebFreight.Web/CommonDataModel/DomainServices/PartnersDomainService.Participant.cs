using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class PartnersDomainService
    {
        public bool DoesParticipantCodeExist(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            participantRepository = new ParticipantRepository(tenant);
            return (participantRepository.GetParticipants(tenant).Where(d => d.Card.Code == code && d.Tenant == tenant)).Any();
        }

        public IQueryable<ParticipantPM> GetParticipantsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Participant", "READ", tenant);


            participantQuery = new ParticipantQuery(tenant);
            return participantQuery.GetParticipantPMsByTenant(tenant);
        }

        public IQueryable<ParticipantPM> GetParticipantsSearch(string code, string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Participant", "READ", tenant);

            participantQuery = new ParticipantQuery(tenant);
            IQueryable<ParticipantPM> q = participantQuery.GetParticipantsByNameOrCode(code, name, tenant).Where(d => d.Tenant == tenant);
            return q;
        }

        public ParticipantPM GetParticipantById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Participant", "READ", tenant);

            participantQuery = new ParticipantQuery(tenant);
            ParticipantPM Participant = participantQuery.GetSinglePM(id,tenant);
            return Participant;
        }

        public ParticipantList GetSingleParticipantList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Participant", "READ", tenant);

            participantRepository = new ParticipantRepository(tenant);
            ParticipantList ParticipantList = null;
            Participant Participant = participantRepository.GetSingleParticipant(id, tenant);

            if (Participant != null)
            {
                List<Participant> singleEntityList = new List<Participant>();
                singleEntityList.Add(Participant);

                IQueryable<Participant> iQueryable = singleEntityList.AsQueryable();
                participantQuery = new ParticipantQuery(participantRepository);
                IQueryable<ParticipantList> iQueryableEntityList = participantQuery.GetIQueryableEntityList(iQueryable);
                ParticipantList = iQueryableEntityList.FirstOrDefault();
            }
            return ParticipantList;
        }

        public IQueryable<ParticipantList> GetParticipantLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Participant", "READ", tenant);

            participantRepository = new ParticipantRepository(tenant);
            IQueryable<Participant> Participants = participantRepository.GetParticipants(tenant);
            participantQuery = new ParticipantQuery(participantRepository);
            IQueryable<ParticipantList> query2 = participantQuery.GetIQueryableEntityList(Participants);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ParticipantList> GetParticipantFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Participant", "READ", tenant);

            participantRepository = new ParticipantRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Participant> Participants = participantRepository.GetParticipants(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            ParticipantCustomFilter customfilters = new ParticipantCustomFilter(tenant);
            Participants = customfilters.GetFilteredQuery(queryOperations, Participants);
            Participants = filter.GetFilteredQuery<Participant>(nonListQueryOperation, Participants);
            int skippedPorts = queryOperations.PageIndex;
            participantQuery = new ParticipantQuery(participantRepository);
            IQueryable<ParticipantList> query2 = participantQuery.GetIQueryableEntityList(Participants);
            query2 = filter.GetFilteredQuery<ParticipantList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ParticipantList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Participant", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ParticipantList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ParticipantList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ParticipantList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ParticipantList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<ParticipantList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ParticipantList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetParticipantFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Participant", "READ", tenant);

            participantRepository = new ParticipantRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Participant> Participants = participantRepository.GetParticipants(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            ParticipantCustomFilter customfilters = new ParticipantCustomFilter(tenant);
            Participants = customfilters.GetFilteredQuery(queryOperations, Participants);
            Participants = filter.GetFilteredQuery<Participant>(nonListQueryOperation, Participants);
            participantQuery = new ParticipantQuery(participantRepository);
            IQueryable<ParticipantList> query2 = participantQuery.GetIQueryableEntityList(Participants);
            query2 = filter.GetFilteredQuery<ParticipantList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertParticipant(ParticipantPM entityPm)
        {
            SecurityUtility.CheckContactFeature("Participant", "NEW", entityPm.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPm.Tenant);
            }

            ParticipantService service = new ParticipantService(objectContext, entityPm.Tenant);
            service.Create(entityPm);
            TableLastUpdateClass.UpdateTableHistory(entityPm.Tenant, "Participant");
        }

        public void UpdateParticipant(ParticipantPM currentEntity)
        {
            SecurityUtility.CheckContactFeature("Participant", "UPDATE", currentEntity.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentEntity.Tenant);
            }

            ParticipantService service = new ParticipantService(objectContext, currentEntity.Tenant);
            service.Update(currentEntity);
            TableLastUpdateClass.UpdateTableHistory(currentEntity.Tenant, "Participant");
        }

        public void UpdateParticipantList(ParticipantList currentEntity)
        {
        }

        public void DeleteParticipant(ParticipantPM Participant)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(Participant.Tenant);
            }

            participantRepository = new ParticipantRepository(objectContext);
            Participant entity = participantRepository.GetSingleParticipant(Participant.Id, Participant.Tenant);
            participantRepository.Remove(entity);
        }

        [Invoke]
        public bool GetIsDirect(int forwarderTenantId, int airlineTenantId, int currenctTenant)
        {
            SecurityUtility.AuthenticationOnTenant(currenctTenant);
            //SecurityUtility.CheckContactFeature("Participant", "READ", airlineTenantId);

            participantRepository = new ParticipantRepository(airlineTenantId);
            Participant participant = participantRepository.GetSingleParticipantByForwarderandAirlineTenant(forwarderTenantId, airlineTenantId);

            if (participant == null)
            {
                return false;
            }

            else
            {
                return participant.IsDirect;
            }
        }
    }

    public class ForworderTenantClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}