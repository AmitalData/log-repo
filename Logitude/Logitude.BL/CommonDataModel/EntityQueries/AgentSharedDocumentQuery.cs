using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class AgentSharedDocumentQuery
    {
        AgentSharedDocumentRepository repository;

        public AgentSharedDocumentQuery()
        {
            repository = new AgentSharedDocumentRepository();
        }

        public AgentSharedDocumentQuery(int tenant)
        {
            repository = new AgentSharedDocumentRepository(tenant);
        }

        public AgentSharedDocumentQuery(AgentSharedDocumentRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<AgentSharedDocumentList> GetIQueryableEntityList(IQueryable<AgentSharedDocument> iQueryable)
        {
            IQueryable<AgentSharedDocumentList> result = from a in iQueryable
                                                         select new AgentSharedDocumentList()
                                                         {

                                                             Tenant = a.Tenant,
                                                             Id = a.Id,
                                                             AgentReference = a.AgentReference,
                                                             CreateDate = a.CreateDate,
                                                             UpdateDate = a.UpdateDate,
                                                             AgentId = a.AgentId,
                                                             StatusCode = a.StatusCode,
                                                             ShipmentLevelCode = a.ShipmentLevelCode,
                                                             DocumentXML = a.DocumentXML,
                                                             AgentSharedManifestRef = a.AgentSharedManifestRef,

                                                         };


            return result;
        }


        public AgentSharedDocumentPM GetSinglePM(string id, int tenant)
        {
            AgentSharedDocumentPM entity = (from a in repository.context.AgentSharedDocuments
                                            where a.Tenant == tenant
                                            && a.Id == id
                                            select new AgentSharedDocumentPM()
                                            {
                                                Tenant = a.Tenant,
                                                Id = a.Id,
                                                AgentReference = a.AgentReference,
                                                CreateDate = a.CreateDate,
                                                UpdateDate = a.UpdateDate,
                                                AgentId = a.AgentId,
                                                StatusCode = a.StatusCode,
                                                ShipmentLevelCode = a.ShipmentLevelCode,
                                                DocumentXML = a.DocumentXML,
                                                AgentSharedManifestRef = a.AgentSharedManifestRef,
                                            }).FirstOrDefault();
            return entity;
        }

        public IQueryable<AgentSharedDocumentPM> GetAgentSharedDocumentPMsByTenant(int tenant)
        {
            IQueryable<AgentSharedDocumentPM> agentSharedDocumentPMs = from a in repository.context.AgentSharedDocuments
                                                                       where a.Tenant == tenant
                                                                       select new AgentSharedDocumentPM()
                                                                       {
                                                                           Tenant = a.Tenant,
                                                                           Id = a.Id,
                                                                           AgentReference = a.AgentReference,
                                                                           CreateDate = a.CreateDate,
                                                                           UpdateDate = a.UpdateDate,
                                                                           AgentId = a.AgentId,
                                                                           StatusCode = a.StatusCode,
                                                                           ShipmentLevelCode = a.ShipmentLevelCode,
                                                                           DocumentXML = a.DocumentXML,
                                                                           AgentSharedManifestRef = a.AgentSharedManifestRef,
                                                                       };
            return agentSharedDocumentPMs;
        }

        public IQueryable<AgentSharedDocumentList> GetAgentSharedDocumentListsByTenant(int tenant)
        {
            IQueryable<AgentSharedDocumentList> agentSharedDocumentLists = from a in repository.context.AgentSharedDocuments
                                                                           where a.Tenant == tenant
                                                                           select new AgentSharedDocumentList()
                                                                           {
                                                                               Tenant = a.Tenant,
                                                                               Id = a.Id,
                                                                               AgentReference = a.AgentReference,
                                                                               CreateDate = a.CreateDate,
                                                                               UpdateDate = a.UpdateDate,
                                                                               AgentId = a.AgentId,
                                                                               StatusCode = a.StatusCode,
                                                                               ShipmentLevelCode = a.ShipmentLevelCode,
                                                                               DocumentXML = a.DocumentXML,
                                                                               AgentSharedManifestRef = a.AgentSharedManifestRef,

                                                                           };
            return agentSharedDocumentLists;
        }

        public IQueryable<AgentSharedDocumentPM> GetAgentSharedDocumentPMsByAgentSharedManifestRef(string agentSharedManifestRef , int tenant)
        {
            IQueryable<AgentSharedDocumentPM> agentSharedDocumentPMs = from a in repository.context.AgentSharedDocuments
                                                                       where a.Tenant == tenant && a.AgentSharedManifestRef == agentSharedManifestRef && a.StatusCode== "WAIT"
                                                                       select new AgentSharedDocumentPM()
                                                                       {
                                                                           Tenant = a.Tenant,
                                                                           Id = a.Id,
                                                                           AgentReference = a.AgentReference,
                                                                           CreateDate = a.CreateDate,
                                                                           UpdateDate = a.UpdateDate,
                                                                           AgentId = a.AgentId,
                                                                           StatusCode = a.StatusCode,
                                                                           ShipmentLevelCode = a.ShipmentLevelCode,
                                                                           DocumentXML = a.DocumentXML,
                                                                           AgentSharedManifestRef = a.AgentSharedManifestRef,
                                                                       };
            return agentSharedDocumentPMs;
        }

    }
}