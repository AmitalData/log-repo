using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityLists;

namespace Logitude.BL.ShipmentsModel.CustomFilters
{
    public class LogBoxShipmentCustomFilter
    {
        private int _tenant;

        public LogBoxShipmentCustomFilter(int tenant)
        {
            _tenant = tenant;
        }

        public IQueryable<LogBoxShipmentDataView> GetFilteredQuery(QueryOperations operations, IQueryable<LogBoxShipmentDataView> queryableData, ShipmentRepository shipmentRepository = null)
        {
            //ask chana
            //& d.IsStandalonePickupDelivery == showIsStandalonePickupDelivery

            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "IsCancelled")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        queryableData = queryableData.Where(d => d.IsCancelled == value);
                    }
                    if (item.FieldName == "ImportersFilter")
                    {
                        string value = item.FieldValue as string;

                        if (queryableData.Count() != 0)
                        {
                            value = value.Replace("%20", " ");
                            queryableData = queryableData.Where(d => d.SearchFields.ToUpper().Contains(value.ToUpper()) || d.DocumentsSearchFields.ToUpper().Contains(value.ToUpper()));
                        }
                    }

                    if (item.FieldName == "ForwarderShipmentsFilter")
                    {
                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d => d.ForwarderShipmentNumber != null);
                        }
                    }

                    if (item.FieldName == "NotForwarderShipmentsFilter")
                    {
                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d => d.ForwarderShipmentNumber == null);
                        }
                    }

                    if (item.FieldName == "PrivateLabelActionRequired")
                    {
                        string value = item.FieldValue as string;

                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d => d.IsRequestedDocuments == true || d.RequestedDocumentsCount > 0 || d.IsDigitalSignRequired == true || d.IsDepositionRequired == true || (d.IsImporterApprovalRequried == true && string.IsNullOrEmpty(d.ApprovedByUserName)));
                        }
                    }

                    if (item.FieldName == "NotPrivateLabelActionRequired")
                    {
                        string value = item.FieldValue as string;

                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d => d.IsRequestedDocuments == true || d.RequestedDocumentsCount > 0 || d.IsDigitalSignRequired == true || d.IsDepositionRequired == true || (d.IsImporterApprovalRequried == true && string.IsNullOrEmpty(d.ApprovedByUserName)));
                        }
                    }
                }
            }
            return queryableData;
        }
    }
}
