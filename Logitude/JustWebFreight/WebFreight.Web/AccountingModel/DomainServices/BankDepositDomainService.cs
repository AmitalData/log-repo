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
        public partial class BankDepositDomainService : LogitudeDomainService
        {
            IDomainServiceUpdateClass<BankDepositPM> service;
            IAccountingContext MyContext;
            public BankDepositDomainService()
            {
                //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<BankDepositPM>), "AccountingDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<BankDepositPM>;
            }

            public BankDepositPM GetSingleBankDepositPM(string id, int tenant)
            {
                if (MyContext == null)
                {
                    MyContext = AccountingContext.GetContext(tenant);
                }

                BankDepositQueryService bankDepositQuery = new BankDepositQueryService(MyContext);
                BankDepositPM bankDepositPM = bankDepositQuery.GetSingle(id, true, false);
                return bankDepositPM;

            }


            public BankDepositList GetSingleBankDepositList(string id, int tenant)
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("BankDeposit", "READ", tenant);

                if (MyContext == null)
                {
                    MyContext = AccountingContext.GetContext(tenant);
                }


                BankDepositListQueryService listService = new BankDepositListQueryService(MyContext);
                return listService.GetSingle(id);
            }

            public List<BankDepositList> GetBankDepositLists(int tenant)
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("BankDeposit", "READ", tenant);
                if (MyContext == null)
                {
                    MyContext = AccountingContext.GetContext(tenant);
                }
                BankDepositListQueryService listService = new BankDepositListQueryService(MyContext);
                return listService.GetList(tenant);
            }

            public List<BankDepositList> GetBankDepositsFilters(byte[] xmlFilters, int tenant)
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("BankDeposit", "READ", tenant);

                if (MyContext == null)
                {
                    MyContext = AccountingContext.GetContext(tenant);
                };
                BankDepositListQueryService listService = new BankDepositListQueryService(MyContext);
                QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
                return listService.GetList(queryOperations, tenant);

            }

            public int GetBankDepositFiltersCount(byte[] xmlFilters, int tenant)
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("BankDeposit", "READ", tenant);
                if (MyContext == null)
                {
                    MyContext = AccountingContext.GetContext(tenant);
                };
                BankDepositListQueryService queryService = new BankDepositListQueryService(MyContext);
                QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
                return queryService.GetListCount(queryOperations, tenant);

            }

            public void InsertBankDeposit(BankDepositPM entityPM)
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("BankDeposit", "NEW", entityPM.Tenant);
                if (MyContext == null)
                {
                    MyContext = AccountingContext.GetContext(entityPM.Tenant);
                };
                BankDepositUpdateService service = new BankDepositUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

                List<BankDepositLinePM> BankDepositLinesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.BankDepositLines).Cast<BankDepositLinePM>().ToList();
                foreach (BankDepositLinePM BankDepositLine in BankDepositLinesChangeSet)
                {
                    entityPM.BankDepositLines.Where(d => d.DepositId == BankDepositLine.DepositId && d.Line == BankDepositLine.Line).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;
                }

                service.Update(entityPM, true);

                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
                ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("BankDeposit", 0, true);
                string email = HttpContext.Current.User.Identity.Name;
                ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
                if (loggedContact != null)
                {
                    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
                }
            }


            public void UpdateBankDeposit(BankDepositPM entityPM)
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("BankDeposit", "UPDATE", entityPM.Tenant);
                if (MyContext == null)
                {
                    MyContext = AccountingContext.GetContext(entityPM.Tenant);
                };
                BankDepositUpdateService service = new BankDepositUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                SetBankDepositLineChangeSet(entityPM);
                service.Update(entityPM, true);

                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
                ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("BankDeposit", 0, true);
                string email = HttpContext.Current.User.Identity.Name;
                ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
                if (loggedContact != null)
                {
                    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
                }
            }

            public void UpdateBankDepositList(BankDepositList entity)
            {

            }


            private void SetBankDepositLineChangeSet(BankDepositPM entityPM)
            {
                List<BankDepositLinePM> BankDepositLinesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.BankDepositLines).Cast<BankDepositLinePM>().ToList();

                foreach (BankDepositLinePM itemPM in BankDepositLinesChangeSet)
                {
                    switch (ChangeSet.GetChangeOperation(itemPM))
                    {
                        case ChangeOperation.Insert:
                            {
                                BankDepositLinePM currentItemPM = entityPM.BankDepositLines.Where(d => d.DepositId == itemPM.DepositId && d.Line == itemPM.Line).FirstOrDefault();
                                currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                                break;
                            }

                        case ChangeOperation.Update:
                            {
                                BankDepositLinePM currentItemPM = entityPM.BankDepositLines.Where(d => d.DepositId == itemPM.DepositId && d.Line == itemPM.Line).FirstOrDefault();
                                currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                                break;
                            }

                        case ChangeOperation.Delete:
                            {
                                BankDepositLinePM currentItemPM = new BankDepositLinePM()
                                {
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                    DepositId = itemPM.DepositId,
                                    Line = itemPM.Line,

                                };

                                entityPM.DeletedBankDepositLines.Add(currentItemPM);
                                break;
                            }

                        default:
                            {
                                BankDepositLinePM currentItemPM = entityPM.BankDepositLines.Where(d => d.DepositId == itemPM.DepositId && d.Line == itemPM.Line).FirstOrDefault();
                                currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                                break;
                            }
                    }
                }
            }


        }
    
}