using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

using System.ServiceModel.DomainServices.Server;
using Simplog.Server.Infrastructure;
using System.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public PaymentOrderPM GetSinglePaymentOrderPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            paymentOrderQueryService = new PaymentOrderQueryService(customContext);
            PaymentOrderPM PaymentOrder = paymentOrderQueryService.GetSingle(id, true, false);
            return PaymentOrder;
        }

        public PaymentOrderList GetSinglePaymentOrderList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("Customs.PaymentOrder", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            PaymentOrderListQueryService listService = new PaymentOrderListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<PaymentOrderList> GetPaymentOrderLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.PaymentOrder", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentOrderListQueryService listService = new PaymentOrderListQueryService(customContext);
            return listService.GetList(tenant);
        }

       
        public List<PaymentOrderList> GetPaymentOrderByPaymentOrderConnection(string ConnectedEntityCode, string ConnectedEntityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.PaymentOrder", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentOrderConnectionTableQueryService connectionTableQueryService = new PaymentOrderConnectionTableQueryService(customContext);
            List<PaymentOrderConnectionTableList> connectionTables = connectionTableQueryService.GetPaymentOrderConnectionTable(ConnectedEntityCode, ConnectedEntityId, tenant);

            PaymentOrderListQueryService listService = new PaymentOrderListQueryService(customContext);
            return listService.GetPaymentOrderList(connectionTables);
        }

        public List<PaymentOrderList> GetPaymentOrderFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.PaymentOrder", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            PaymentOrderListQueryService listService = new PaymentOrderListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetPaymentOrderFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.PaymentOrder", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            PaymentOrderListQueryService queryService = new PaymentOrderListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertPaymentOrder(PaymentOrderPM entityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.PaymentOrder", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            PaymentOrderUpdateService service = new PaymentOrderUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            foreach (PaymentOrderLinePM paymentOrderLine in entityPm.PaymentOrderLines)
            {
                paymentOrderLine.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            }
            service.Update(entityPm, true);

        }

        public void UpdatePaymentOrder(PaymentOrderPM currententityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.PaymentOrder", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }
          
            PaymentOrderUpdateService service = new PaymentOrderUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
          
            SetPaymentOrderMethodChangeSet(currententityPm);
            SetPaymentOrderProtestChangeSet(currententityPm);
            SetPaymentOrderConnetionTableChangeSet(currententityPm);
            service.Update(currententityPm, true);
        }

        private void SetPaymentOrderMethodChangeSet(PaymentOrderPM currententityPm)
        {
            List<PaymentOrderMethodPM> entityChangeSet = ChangeSet.GetAssociatedChanges(currententityPm, d => d.PaymentOrderMethods).Cast<PaymentOrderMethodPM>().ToList();

            foreach (PaymentOrderMethodPM itemPM in entityChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            PaymentOrderMethodPM currentItemPM = currententityPm.PaymentOrderMethods.Where(d => d.PaymentOrderId == itemPM.PaymentOrderId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            PaymentOrderMethodPM currentItemPM = currententityPm.PaymentOrderMethods.Where(d => d.PaymentOrderId == itemPM.PaymentOrderId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            PaymentOrderMethodPM currentItemPM = new PaymentOrderMethodPM() { ChangeSetOp = ChangeSetOperation.Delete, PaymentOrderId = itemPM.PaymentOrderId, Line = itemPM.Line };
                            currententityPm.DeletedPaymentOrderMethods.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            PaymentOrderMethodPM currentItemPM = currententityPm.PaymentOrderMethods.Where(d => d.PaymentOrderId == itemPM.PaymentOrderId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }
        private void SetPaymentOrderProtestChangeSet(PaymentOrderPM currententityPm)
        {
            List<PaymentOrderProtestReasonPM> entityChangeSet = ChangeSet.GetAssociatedChanges(currententityPm, d => d.PaymentOrderProtestReasons).Cast<PaymentOrderProtestReasonPM>().ToList();

            foreach (PaymentOrderProtestReasonPM itemPM in entityChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            PaymentOrderProtestReasonPM currentItemPM = currententityPm.PaymentOrderProtestReasons.Where(d => d.PaymentOrderId == itemPM.PaymentOrderId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            PaymentOrderProtestReasonPM currentItemPM = currententityPm.PaymentOrderProtestReasons.Where(d => d.PaymentOrderId == itemPM.PaymentOrderId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            PaymentOrderProtestReasonPM currentItemPM = new PaymentOrderProtestReasonPM() { ChangeSetOp = ChangeSetOperation.Delete, PaymentOrderId = itemPM.PaymentOrderId, Line = itemPM.Line };
                            currententityPm.DeletedPaymentOrderProtestReasons.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            PaymentOrderProtestReasonPM currentItemPM = currententityPm.PaymentOrderProtestReasons.Where(d => d.PaymentOrderId == itemPM.PaymentOrderId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }
        private void SetPaymentOrderConnetionTableChangeSet(PaymentOrderPM currententityPm)
        {
            List<PaymentOrderConnectionTablePM> entityChangeSet = ChangeSet.GetAssociatedChanges(currententityPm, d => d.PaymentOrderConnectionTables).Cast<PaymentOrderConnectionTablePM>().ToList();

            foreach (PaymentOrderConnectionTablePM itemPM in entityChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            PaymentOrderConnectionTablePM currentItemPM = currententityPm.PaymentOrderConnectionTables.Where(d => d.PaymentOrderId == itemPM.PaymentOrderId && d.ConnectedEntityId == itemPM.ConnectedEntityId).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            PaymentOrderConnectionTablePM currentItemPM = currententityPm.PaymentOrderConnectionTables.Where(d => d.PaymentOrderId == itemPM.PaymentOrderId && d.ConnectedEntityId == itemPM.ConnectedEntityId).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            PaymentOrderConnectionTablePM currentItemPM = new PaymentOrderConnectionTablePM() { ChangeSetOp = ChangeSetOperation.Delete, PaymentOrderId = itemPM.PaymentOrderId, ConnectedEntityId = itemPM.ConnectedEntityId };
                            currententityPm.DeletedPaymentOrderConnectionTables.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            PaymentOrderConnectionTablePM currentItemPM = currententityPm.PaymentOrderConnectionTables.Where(d => d.PaymentOrderId == itemPM.PaymentOrderId && d.ConnectedEntityId == itemPM.ConnectedEntityId).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        public void UpdatePaymentOrderList(PaymentOrderList list)
        {

        }

     
    }
}