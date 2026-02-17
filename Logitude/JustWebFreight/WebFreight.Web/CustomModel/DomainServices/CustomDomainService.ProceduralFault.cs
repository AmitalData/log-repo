using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Simplog.Server.Infrastructure;
using System.ServiceModel.DomainServices.Server;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public ProceduralFaultPM GetSingleProceduralFaultPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            proceduralFaultQuery = new ProceduralFaultQueryService(customContext);
            ProceduralFaultPM ProceduralFault = proceduralFaultQuery.GetSingle(id, true, false);
            return ProceduralFault;
        }

        public ProceduralFaultList GetSingleProceduralFaultList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ProceduralFault", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ProceduralFaultListQueryService listService = new ProceduralFaultListQueryService(customContext);
            return listService.GetSingle(id);
        }

     

        public List<ProceduralFaultList> GetProceduralFaultLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ProceduralFault", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ProceduralFaultListQueryService listService = new ProceduralFaultListQueryService(customContext);
            return listService.GetList(tenant);

        }


        public List<ProceduralFaultList> GetProceduralFaultFilters(byte[] xmlFilters, int tenant)
        {


            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ProceduralFault", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            ProceduralFaultListQueryService listService = new ProceduralFaultListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetListFromView(queryOperations, tenant);

        }




        public int GetProceduralFaultFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.ProceduralFault", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ProceduralFaultListQueryService queryService = new ProceduralFaultListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCountFromView(queryOperations, tenant);

        }

        public void InsertProceduralFault(ProceduralFaultPM entityPm)
        {
            SecurityUtility.CheckContactFeature("Customs.ProceduralFault", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            ProceduralFaultUpdateService service = new ProceduralFaultUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            foreach (ProceduralFaultsConnEntityPM ProceduralFaultsConnectedEntity in entityPm.ProceduralFaultsConnEntities)
            {
                ProceduralFaultsConnectedEntity.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            }
            service.Update(entityPm, true);



        }

        public void UpdateProceduralFault(ProceduralFaultPM currententityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.ProceduralFault", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            ProceduralFaultUpdateService service = new ProceduralFaultUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            SetProceduralFaultsConnectedEntityChangeSet(currententityPm);
            service.Update(currententityPm, true);

        }

        private void SetProceduralFaultsConnectedEntityChangeSet(ProceduralFaultPM currententityPm)
        {
            List<ProceduralFaultsConnEntityPM> ProceduralFaultsConnectedEntitychangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.ProceduralFaultsConnEntities).Cast<ProceduralFaultsConnEntityPM>().ToList();
            foreach (ProceduralFaultsConnEntityPM itemPM in ProceduralFaultsConnectedEntitychangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            ProceduralFaultsConnEntityPM currentItemPM = currententityPm.ProceduralFaultsConnEntities.Where(d => d.ProceduralFaultId == itemPM.ProceduralFaultId && d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            ProceduralFaultsConnEntityPM currentItemPM = currententityPm.ProceduralFaultsConnEntities.Where(d => d.ProceduralFaultId == itemPM.ProceduralFaultId && d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                         

                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            ProceduralFaultsConnEntityPM currentItemPM = new ProceduralFaultsConnEntityPM() { ChangeSetOp = ChangeSetOperation.Delete, ProceduralFaultId = itemPM.ProceduralFaultId, Id = itemPM.Id };
                            currententityPm.DeletedProceduralFaultsConnEntities.Add(currentItemPM);

                            break;
                        }
                    default:
                        {
                            ProceduralFaultsConnEntityPM currentItemPM = currententityPm.ProceduralFaultsConnEntities.Where(d => d.ProceduralFaultId == itemPM.ProceduralFaultId && d.Id == itemPM.Id).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

       
        public void UpdateProceduralFaultList(ProceduralFaultList list)
        {

        }

    }
}