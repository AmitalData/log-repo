using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.CustomFilters
{
    public class ContainerFollowUpCustomFilter
    {
        int tenant;
        public ContainerFollowUpCustomFilter(int tenant)
        {
            this.tenant = tenant;
        }

        public IQueryable<ContainerFollowUpList> GetFilteredQuery(QueryOperations operations, IQueryable<ContainerFollowUpList> iQueryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "ArrivedNotDelivered")
                    {
                        IWebFreightContext myFreightContext = WebFreightContext.GetContext(tenant);

                        //List<string> allStatusedCodes_ARR = new List<string>();
                        //allStatusedCodes_ARR.Add("SARR");
                        //allStatusedCodes_ARR.Add("SAR2");
                        //allStatusedCodes_ARR.Add("SAR3");
                        //allStatusedCodes_ARR.Add("SAR4");
                        //allStatusedCodes_ARR.Add("SAR5");

                        List<string> allStatusedCodes_ARR_UP = new List<string>();
                        allStatusedCodes_ARR_UP.Add("SARR");
                        allStatusedCodes_ARR_UP.Add("SAR2");
                        allStatusedCodes_ARR_UP.Add("SAR3");
                        allStatusedCodes_ARR_UP.Add("SAR4");
                        allStatusedCodes_ARR_UP.Add("SAR5");
                        allStatusedCodes_ARR_UP.Add("CERT");
                        allStatusedCodes_ARR_UP.Add("DTCA");
                        allStatusedCodes_ARR_UP.Add("ICCL");
                        allStatusedCodes_ARR_UP.Add("SHCL");
                        allStatusedCodes_ARR_UP.Add("SDL2");
                        allStatusedCodes_ARR_UP.Add("SDLY");
                        allStatusedCodes_ARR_UP.Add("SDLD");
                        allStatusedCodes_ARR_UP.Add("INPR");
                        allStatusedCodes_ARR_UP.Add("DDAP");
                        allStatusedCodes_ARR_UP.Add("DDDE");

                        string myObjectTableId = (from d in myFreightContext.ObjectTables where d.Name == "Shipment" select d.Id).FirstOrDefault();
                        //List<string> allStatusedIds_ARR = (from d in myFreightContext.EntityStatus where d.Tenant == tenant && d.ObjectTableId == myObjectTableId && allStatusedCodes_ARR.Contains(d.Code) select d.Id).ToList();
                        List<string> allStatusedIds_ARR_UP = (from d in myFreightContext.EntityStatus where d.Tenant == tenant && d.ObjectTableId == myObjectTableId && allStatusedCodes_ARR_UP.Contains(d.Code) select d.Id).ToList();

                        iQueryableData = (from d in iQueryableData
                                          where d.Tenant == tenant
                                          && allStatusedIds_ARR_UP.Contains(d.StatusId)
                                          &&
                                          (
                                          d.IsDeliveryFU && d.DeliveryATA == null
                                          //||
                                          //d.IsEmptyContainerReturnFU && d.EmptyContainerReturnATA == null
                                          )
                                          select d);
                    }

                    if (item.FieldName == "DeliveredNotReturned")
                    {
                        iQueryableData = (from d in iQueryableData
                                          where d.Tenant == tenant                                                         
                                          && (d.IsDeliveryFU && d.DeliveryATA != null)                                          
                                          && (d.IsEmptyContainerReturnFU && d.EmptyContainerReturnATA == null)
                                          select d);
                    }

                    if (item.FieldName == "InTransit")
                    {
                        IWebFreightContext myFreightContext = WebFreightContext.GetContext(tenant);

                        List<string> allStatusedCodes_DEP = new List<string>();
                        allStatusedCodes_DEP.Add("SDEP");
                        allStatusedCodes_DEP.Add("SDE2");
                        allStatusedCodes_DEP.Add("SDE3");
                        allStatusedCodes_DEP.Add("SDE4");
                        allStatusedCodes_DEP.Add("ONCD");

                        string myObjectTableId = (from d in myFreightContext.ObjectTables where d.Name == "Shipment" select d.Id).FirstOrDefault();
                        List<string> allStatusedIds_DEP = (from d in myFreightContext.EntityStatus where d.Tenant == tenant && d.ObjectTableId == myObjectTableId && allStatusedCodes_DEP.Contains(d.Code) select d.Id).ToList();

                        iQueryableData = (from d in iQueryableData
                                          where d.Tenant == tenant                                      
                                          && allStatusedIds_DEP.Contains(d.StatusId)                                      
                                          &&                                      
                                          (                                      
                                          d.IsDeliveryFU                                      
                                          ||                                      
                                          d.IsEmptyContainerReturnFU                                      
                                          )
                                          select d);
                    }
                }
            }

            return iQueryableData;
        }
    }
}
