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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Customs.Data.Utils;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class PaymentOrderListQueryService
    {
	    private IQueryable<PaymentOrderList> GetIqueryableList(IQueryable<PaymentOrder> iQueryable)
        {
            IQueryable<PaymentOrderList> query = (from a in iQueryable.Include("CustomerCard").Include("PaymentStatus").Include("OperationalStatus").Include("PaymentOrderType").Include("PaymentProcess")

                                                  select new PaymentOrderList()
                                                      {

                                                          ActualPayDate = a.ActualPayDate,
                                                          CreateDate = a.CreateDate,
                                                          CustomerActivityTypeCode = a.CustomerActivityTypeCode,
                                                          //CustomerId = a.CustomerId,
                                                          CustomsHouseCode = a.CustomsHouseCode,
                                                          Id = a.Id,
                                                          InternalNotes = a.InternalNotes,
                                                          LastPayDate = a.LastPayDate,
                                                          IsClosed = a.IsClosed,
                                                          PaymentNumber = a.PaymentNumber,
                                                          PaymentOrderTypeCode = a.PaymentOrderTypeCode,
                                                          PaymentProcessCode = a.PaymentProcessCode,
                                                          PaymentStatusCode = a.PaymentStatusCode,
                                                          Reason = a.Reason,
                                                          SearchFields = a.SearchFields,
                                                          Tenant = a.Tenant,
                                                          TotalSumToPay = a.TotalSumToPay,
                                                          UpdateDate = a.UpdateDate,
                                                          CustomerName = a.CustomerCard.LocalName != null ? a.CustomerCard.LocalName : a.CustomerCard.EnglishName,
                                                          PaymentStatusName = a.PaymentStatus != null ? a.PaymentStatus.LocalName : null,
                                                          CustomsEntityTypeCode = a.CustomsEntityTypeCode,
                                                          FirstEntityID = a.FirstEntityID,
                                                          SecondEntityID = a.SecondEntityID,
                                                          ThirdEntityID = a.ThirdEntityID,
                                                          ImporterName = a.Client.FullName != null ? a.Client.FullName : a.Client.LocalFirstName,
                                                        //  ImporterId = a.ImporterId,
                                                          CustomerActivityTypeName = a.CustomerActivityType == null ? null : a.CustomerActivityType.LocalName,
                                                          CustomsEntityTypeName = a.EntityTypeLookup == null ? null : a.EntityTypeLookup.LocalName,
                                                          CustomsHouseName = a.CustomsHouseType == null ? null : a.CustomsHouseType.LocalName,
                                                          PaymentOrderTypeName = a.PaymentOrderType != null? a.PaymentOrderType.LocalName : null,
                                                          PaymentProcessName = a.PaymentProcess != null? a.PaymentProcess.LocalName : null,
                                                          CustomFiles = a.CustomFiles,
                                                      });
            return query;
		}


        public List<PaymentOrderList> GetPaymentOrderList(List<PaymentOrderConnectionTableList> connections )
        {

            List<PaymentOrderList> paymentOrders = new List<PaymentOrderList>();
            PaymentOrderList paymentorder = null;
            foreach (PaymentOrderConnectionTableList item in connections)
            {
                 paymentorder =

                    (from a in context.PaymentOrders.Include("CustomerCard").Include("PaymentStatus").Include("OperationalStatus").Include("PaymentProcess")
                     where a.Id == item.PaymentOrderId
                     select new PaymentOrderList()
                     {
                         
                         ActualPayDate = a.ActualPayDate,
                         CreateDate = a.CreateDate,
                         CustomerActivityTypeCode = a.CustomerActivityTypeCode,
                         CustomsHouseCode = a.CustomsHouseCode,
                         Id = a.Id,
                         InternalNotes = a.InternalNotes,
                         LastPayDate = a.LastPayDate,
                         IsClosed = a.IsClosed,
                         PaymentNumber = a.PaymentNumber,
                         PaymentOrderTypeCode = a.PaymentOrderTypeCode,
                         PaymentProcessCode = a.PaymentProcessCode,
                         PaymentStatusCode = a.PaymentStatusCode,
                         Reason = a.Reason,
                         SearchFields = a.SearchFields,
                         Tenant = a.Tenant,
                         TotalSumToPay = a.TotalSumToPay,
                         UpdateDate = a.UpdateDate,
                         CustomerName = a.CustomerCard.LocalName,
                         PaymentStatusName = a.PaymentStatus.LocalName,
                         CustomsEntityTypeCode = a.CustomsEntityTypeCode,
                         ImporterName = a.Client.LocalFirstName != null ? a.Client.LocalFirstName : a.Client.EnglishFirstName,
                         CustomerActivityTypeName = a.CustomerActivityType == null ? null : a.CustomerActivityType.LocalName,
                         CustomsEntityTypeName = a.EntityTypeLookup == null ? null : a.EntityTypeLookup.LocalName,
                         CustomsHouseName = a.CustomsHouseType == null ? null : a.CustomsHouseType.LocalName,
                         PaymentProcessName = a.PaymentProcess != null ? a.PaymentProcess.LocalName : null,

                     }).FirstOrDefault();

                 paymentOrders.Add(paymentorder);
            }


            return paymentOrders;
        }



        private IQueryable<PaymentOrder> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<PaymentOrder> iQueryable, int tenant)
        {
            iQueryable = GetFreelancerQuery(iQueryable, tenant);
            return iQueryable;
        }

        public IQueryable<PaymentOrder> GetFreelancerQuery(IQueryable<PaymentOrder> queryableData, int tenant)
        {
            FreelancerCustomersUtil frlUtil = new FreelancerCustomersUtil(tenant);
            if (frlUtil.user.IsFreelancer)
            {
                List<string> customersIds = frlUtil.GetConnectedCustomersIds(tenant);
                if (customersIds.Count > 0)
                {
                    queryableData = queryableData.Where(d => customersIds.Contains(d.CustomerId));
                }
            }

            return queryableData;
        }


    }


}
	