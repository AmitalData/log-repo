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
    public partial class CashBookDomainService : LogitudeDomainService
    {
        IDomainServiceUpdateClass<CashBookPM> service;
        IAccountingContext MyContext;
        public CashBookDomainService()
        {
            //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<CashBookPM>), "AccountingDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<CashBookPM>;
        }

        public CashBookPM GetSingleCashBookPM(string id, int tenant)
        {
            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            }

            CashBookQueryService cashBookQuery = new CashBookQueryService(MyContext);
            CashBookPM cashBookPM = cashBookQuery.GetSingle(id, true, false);
            return cashBookPM;

        }


        public CashBookList GetSingleCashBookList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CashBook", "READ", tenant);

            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            }


            CashBookListQueryService listService = new CashBookListQueryService(MyContext);
            return listService.GetSingle(id);
        }

        public List<CashBookList> GetCashBookLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CashBook", "READ", tenant);
            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            }
            CashBookListQueryService listService = new CashBookListQueryService(MyContext);
            return listService.GetList(tenant);
        }

        public List<CashBookList> GetCashBooksFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CashBook", "READ", tenant);

            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            };
            CashBookListQueryService listService = new CashBookListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCashBookFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("CashBook", "READ", tenant);
            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(tenant);
            };
            CashBookListQueryService queryService = new CashBookListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertCashBook(CashBookPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("CashBook", "NEW", entityPM.Tenant);
            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(entityPM.Tenant);
            };
            CashBookUpdateService service = new CashBookUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            List<CashBookLinePM> CashBookLinesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.CashBookLines).Cast<CashBookLinePM>().ToList();
            foreach (CashBookLinePM CashBookLine in CashBookLinesChangeSet)
            {
                entityPM.CashBookLines.Where(d => d.CashBookId == CashBookLine.CashBookId && d.ARPChequeId == CashBookLine.ARPChequeId).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;
            }

            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("CashBook", 0, true);
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
            }
        }


        public void UpdateCashBook(CashBookPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("CashBook", "UPDATE", entityPM.Tenant);
            if (MyContext == null)
            {
                MyContext = AccountingContext.GetContext(entityPM.Tenant);
            };
            CashBookUpdateService service = new CashBookUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            SetCashBookLineChangeSet(entityPM);
            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("CashBook", 0, true);
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
            }
        }



        private void SetCashBookLineChangeSet(CashBookPM entityPM)
        {
            List<CashBookLinePM> CashBookLinesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.CashBookLines).Cast<CashBookLinePM>().ToList();

            foreach (CashBookLinePM itemPM in CashBookLinesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            CashBookLinePM currentItemPM = entityPM.CashBookLines.Where(d => d.CashBookId == itemPM.CashBookId && d.ARPChequeId == itemPM.ARPChequeId).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            CashBookLinePM currentItemPM = entityPM.CashBookLines.Where(d => d.CashBookId == itemPM.CashBookId && d.ARPChequeId == itemPM.ARPChequeId).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            CashBookLinePM currentItemPM = new CashBookLinePM()
                            {
                                ChangeSetOp = ChangeSetOperation.Delete,
                                CashBookId = itemPM.CashBookId,
                                ARPChequeId = itemPM.ARPChequeId,

                            };

                            entityPM.DeletedCashBookLines.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                            CashBookLinePM currentItemPM = entityPM.CashBookLines.Where(d => d.CashBookId == itemPM.CashBookId && d.ARPChequeId == itemPM.ARPChequeId).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        public void UpdateCashBookList(CashBookList entity)
        {

        }

    }
}
