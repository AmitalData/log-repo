using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.BL.EntityQueryServices;

namespace WebFreight.Web.AccountingModel.DomainServices
{


    [EnableClientAccess()]
    public partial class RevaluationDomainService : LogitudeDomainService
    {
        IDomainServiceUpdateClass<RevaluationPM> service;
        IAccountingContext MyContext;
        public RevaluationDomainService()
        {
            //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<RevaluationPM>), "AccountingDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<RevaluationPM>;
        }

        public RevaluationPM GetSingleRevaluationPM(string id, int tenant)
        {
            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            }

            RevaluationQueryService revaluationQuery = new RevaluationQueryService(MyContext);
            RevaluationPM revaluationPM = revaluationQuery.GetSingle(id, true, false);
            return revaluationPM;

        }
 


        public RevaluationList GetSingleRevaluationList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Revaluation", "READ", tenant);

            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            }


            RevaluationListQueryService listService = new RevaluationListQueryService(MyContext);
            return listService.GetSingle(id);
        }

        public List<RevaluationList> GetRevaluationLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Revaluation", "READ", tenant);
            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            }
            RevaluationListQueryService listService = new RevaluationListQueryService(MyContext);
            return listService.GetList(tenant);
        }

        public List<RevaluationList> GetRevaluationsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Revaluation", "READ", tenant);

            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            };
            RevaluationListQueryService listService = new RevaluationListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetRevaluationFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Revaluation", "READ", tenant);
            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            };
            RevaluationListQueryService queryService = new RevaluationListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertRevaluation(RevaluationPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("Revaluation", "NEW", entityPM.Tenant);
            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(entityPM.Tenant);
            };
            RevaluationUpdateService service = new RevaluationUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            //List<RevaluationLinePM> RevaluationLinesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.RevaluationLines).Cast<RevaluationLinePM>().ToList();
            //foreach (RevaluationLinePM RevaluationLine in RevaluationLinesChangeSet)
            //{
            //    entityPM.RevaluationLines.Where(d => d.RevaluationId == RevaluationLine.RevaluationId && d.ARPChequeId == RevaluationLine.ARPChequeId).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;
            //}

            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Revaluation", 0, true);
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
            }
        }


        public void UpdateRevaluation(RevaluationPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("Revaluation", "UPDATE", entityPM.Tenant);
            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(entityPM.Tenant);
            };
            RevaluationUpdateService service = new RevaluationUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            SetRevaluationLineChangeSet(entityPM);
            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Revaluation", 0, true);
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
            }
        }



        private void SetRevaluationLineChangeSet(RevaluationPM entityPM)
        {
            //List<RevaluationLinePM> RevaluationLinesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.RevaluationLines).Cast<RevaluationLinePM>().ToList();

            //foreach (RevaluationLinePM itemPM in RevaluationLinesChangeSet)
            //{
            //    switch (ChangeSet.GetChangeOperation(itemPM))
            //    {
            //        case ChangeOperation.Insert:
            //            {
            //                RevaluationLinePM currentItemPM = entityPM.RevaluationLines.Where(d => d.RevaluationId == itemPM.RevaluationId && d.ARPChequeId == itemPM.ARPChequeId).FirstOrDefault();
            //                currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
            //                break;
            //            }

            //        case ChangeOperation.Update:
            //            {
            //                RevaluationLinePM currentItemPM = entityPM.RevaluationLines.Where(d => d.RevaluationId == itemPM.RevaluationId && d.ARPChequeId == itemPM.ARPChequeId).FirstOrDefault();
            //                currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
            //                break;
            //            }

            //        case ChangeOperation.Delete:
            //            {
            //                RevaluationLinePM currentItemPM = new RevaluationLinePM()
            //                {
            //                    ChangeSetOp = ChangeSetOperation.Delete,
            //                    RevaluationId = itemPM.RevaluationId,
            //                    ARPChequeId = itemPM.ARPChequeId,

            //                };

            //                entityPM.DeletedRevaluationLines.Add(currentItemPM);
            //                break;
            //            }

            //        default:
            //            {
            //                RevaluationLinePM currentItemPM = entityPM.RevaluationLines.Where(d => d.RevaluationId == itemPM.RevaluationId && d.ARPChequeId == itemPM.ARPChequeId).FirstOrDefault();
            //                currentItemPM.ChangeSetOp = ChangeSetOperation.None;
            //                break;
            //            }
            //    }
            //}
        }

        public void UpdateRevaluationList(RevaluationList entity)
        {

        }

    }
}
