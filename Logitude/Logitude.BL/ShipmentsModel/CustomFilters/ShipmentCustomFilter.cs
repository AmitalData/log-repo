using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.DataContracts;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System.Data.Entity.Core.Objects;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityLists;

namespace Logitude.BL.ShipmentsModel.CustomFilters
{
    public class ShipmentCustomFilter
    {
        int tenant;
        public ShipmentCustomFilter(int tenant)
        {
            this.tenant = tenant;
        }

        public IQueryable<ShipmentDataView> GetFilteredQuery(QueryOperations operations, IQueryable<ShipmentDataView> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            bool showIsCancelled = false;
            bool showIsStandalonePickupDelivery = false;
            bool isMasterConnectedHouses = false;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "ActualDataDateYearMonth")
                    {
                        int year = Convert.ToInt32(item.FieldValue);
                        int month = Convert.ToInt32(item.FieldValue2);
                        if (year != 0)
                        {
                            queryableData = queryableData.Where(d =>  d.CreateDateTime.Year == year);
                        }
                        if (month != 0)
                        {
                            queryableData = queryableData.Where(d => d.CreateDateTime.Month == month);
                        }
                    }

                    if (item.FieldName == "IsNewARInvoiceBlocked")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            queryableData = queryableData.Where(d => d.IsNewARInvoiceBlocked);
                        }
                    }

                    if (item.FieldName == "MasterConnectedHouses")
                    {
                        isMasterConnectedHouses = true;
                    }

                    if (item.FieldName == "IsCancelled")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            showIsCancelled = true;
                        }
                    }

                    if (item.FieldName == "IsStandalonePickupDelivery")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            showIsStandalonePickupDelivery = true;
                        }
                    }
                    

                    if (item.FieldName == "Partner")
                    {
                        string value = item.FieldValue as string;
                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d => d.AgentName.ToUpper().StartsWith(value.ToUpper())
                                   || d.CustomAgentImportName.ToUpper().StartsWith(value.ToUpper()) ||
                                          d.ConsigneeName.ToUpper().StartsWith(value.ToUpper())
                                           );
                        }
                    }

                    if (item.FieldName == "Reference")
                    {
                        string value = item.FieldValue as string;
                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(
                                d => d.ShipperReference1.StartsWith(value)
                                    || d.ShipperReference2.StartsWith(value)
                                    || d.ConsigneeReference1.StartsWith(value)
                                    || d.ConsigneeReference2.StartsWith(value)
                                    || d.ShipmentNumber.StartsWith(value)
                                    || d.House.StartsWith(value)
                                    || d.Master.StartsWith(value)
                                    );
                        }
                    }

                    if (item.FieldName == "FromOrToPort")
                    {
                        string value = item.FieldValue as string;

                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d => d.MainCarriageFromPortCode.ToUpper().StartsWith(value.ToUpper()) || d.MainCarriageToPortCode.ToUpper().StartsWith(value.ToUpper()));
                        }
                    }


                    if (item.FieldName == "ShipmentReceivableStatusFilter")
                    {
                        string value = item.FieldValue as string;

                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d => d.ShipmentReceivableStatusCode.StartsWith(value.ToUpper()) || d.ShipmentReceivableStatusName.StartsWith(value.ToUpper()));
                        }
                    }

                    if (item.FieldName == "ShipmentPayableStatusFilter")
                    {
                        string value = item.FieldValue as string;

                        if (queryableData.Count() != 0)
                        {
                            queryableData.Where(d => d.ShipmentPayableStatusCode.StartsWith(value.ToUpper()) || d.ShipmentPayableStatusName.StartsWith(value.ToUpper()));
                        }
                    }

                    if (item.FieldName == "Client")
                    {
                        string value = item.FieldValue as string;
                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d => d.ShipperName.ToUpper().StartsWith(value.ToUpper()));
                        }
                    }

                    if (item.FieldName == "OpenShipments" || item.FieldName == "OpenMasters")
                    {
                        queryableData = queryableData.Where(d => d.IsOperationalClosed == false);
                    }

                    if (item.FieldName == "ClosedShipments")
                    {
                        queryableData = queryableData.Where(d => d.IsOperationalClosed == true);
                    }

                    if (item.FieldName == "AccountingOpen")
                    {
                        queryableData = queryableData.Where(d => d.IsOperationalClosed == true && d.IsAccountingClosed == false);
                    }

                    if (item.FieldName == "AllShipments")
                    {
                        queryableData = queryableData.Where(d => d.ShipmentLevelCode != "C" && d.IsCancelled == false);
                    }

                    if (item.FieldName == "AllMasters")
                    {
                        queryableData = queryableData.Where(d => d.ShipmentLevelCode != "H" && d.ShipmentLevelCode != "A" && d.IsCancelled == false);
                    }

                    if (item.FieldName == "OperationalOpenHousesDirects")
                    {
                        queryableData = queryableData.Where(d => d.IsOperationalClosed == false);
                        queryableData = queryableData.Where(d => d.ShipmentLevelCode == "D" || d.ShipmentLevelCode == "H" || d.ShipmentLevelCode == "A");
                    }

                    if (item.FieldName == "OperationalOpenMastersDirects")
                    {
                        queryableData = queryableData.Where(d => d.IsOperationalClosed == false);
                        queryableData = queryableData.Where(d => d.ShipmentLevelCode == "D" || d.ShipmentLevelCode == "C" || d.ShipmentLevelCode == "A");
                    }

                    if (item.FieldName == "AccountingOpenHousesDirects")
                    {
                        queryableData = queryableData.Where(d => d.IsAccountingClosed == false);
                        queryableData = queryableData.Where(d => d.ShipmentLevelCode == "D" || d.ShipmentLevelCode == "H");
                        queryableData = queryableData.Where(d => d.ShipmentReceivableStatusCode == "OPEN");
                    }

                    if (item.FieldName == "AccountingOpenMastersDirects")
                    {
                        queryableData = queryableData.Where(d => d.IsAccountingClosed == false);
                        queryableData = queryableData.Where(d => d.ShipmentLevelCode == "D" || d.ShipmentLevelCode == "C");
                        queryableData = queryableData.Where(d => d.ShipmentPayableStatusCode == "OPEN");
                    }

                    if (item.FieldName == "ExpectedDepartures")
                    {
                        queryableData = queryableData.Where(d => d.ShipmentLevelCode != "H" && d.IsCancelled == false && d.MainCarriageATD == null && d.MainCarriageATA == null && d.MainCarriageETD != null && d.DirectionId == "E" && d.TransportModeId == "A");

                        EntityStatusRepository statusRepository = new EntityStatusRepository(tenant);
                        EntityStatus status_Arrived = statusRepository.GetSingleEntityStatusByCode("SARR", tenant);
                        EntityStatus status_Delivered = statusRepository.GetSingleEntityStatusByCode("SDLD", tenant);

                        if (status_Arrived != null)
                        {
                            queryableData = queryableData.Where(d => d.ShipmentStatusId != status_Arrived.Id);
                        }

                        if (status_Delivered != null)
                        {
                            queryableData = queryableData.Where(d => d.ShipmentStatusId != status_Delivered.Id);
                        }

                        DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                        DateTime lastWeekDate = todayDate.AddDays(-7);

                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETD) > lastWeekDate);
                    }

                    if (item.FieldName == "AirlinesUpdates")
                    {
                        DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                        DateTime lastWeekDate = todayDate.AddDays(-7);

                        queryableData = queryableData.Where(d => d.ShipmentLevelCode != "H" && d.TransportModeId == "A" && d.CarrierLastStatusDate >= lastWeekDate && d.IsCancelled == false);
                    }

                    if (item.FieldName == "ExpectedDeparturesNotTransmitted")
                    {
                        queryableData = from d in queryableData
                                        where
                                        d.ShipmentLevelCode != "H"
                                        && d.DirectionId == "E"
                                        && d.TransportModeId == "O"
                                        && d.INTTRASIStatusCode == "NSEN"
                                        && (d.ShipmentTypeId == "FCLD" || d.ShipmentTypeId == "MYGO")
                                        && d.MainCarriageATD == null
                                        && d.MainCarriageETD != null
                                        select d;
                    }

                    if (item.FieldName == "ShippingInstructionsLast7Days")
                    {
                        DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                        DateTime lastWeekDate = todayDate.AddDays(-7);

                        queryableData = from d in queryableData
                                        where d.INTTRASIStatusCode != "NSEN"
                                        && (d.INTTRASIStatusDate >= lastWeekDate || d.INTTRALastStatusDate >= lastWeekDate)
                                        select d;
                    }

                    if (item.FieldName == "ContainerStatusLast7Days")
                    {
                        DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                        DateTime lastWeekDate = todayDate.AddDays(-7);

                        queryableData = from d in queryableData
                                        where d.INTTRASIStatusCode != "NSEN"
                                        && (d.INTTRALastStatusDate >= lastWeekDate)
                                        select d;
                    }

                    if (item.FieldName == "EBookingInProgress")
                    {
                        queryableData = from d in queryableData
                                        where 
                                        ( d.ShipmentLevelCode == "H" || d.ShipmentLevelCode == "D")
                                        && d.DirectionId == "E"
                                        && d.TransportModeId == "O"
                                        && d.INTTRABookingTransStatusCode != "NST" && d.INTTRABookingStatusCode != "SI"
                                        && d.MainCarriageATD == null
                                        select d;
                    }


                    if (item.FieldName == "SentFSR")
                    {
                        DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                        DateTime lastWeekDate = todayDate.AddDays(-7);

                        queryableData = queryableData.Where(d => d.ShipmentLevelCode != "H" && d.TransportModeId == "A" && d.LastFSRStatusRequestDate >= lastWeekDate && d.IsCancelled == false);
                    }

                    if (item.FieldName == "ImportShipments")
                    {
                        queryableData = queryableData.Where(d => d.ShipmentLevelCode != "C" && ((d.DirectionId == "C" && d.NoFreightFile == true) || d.DirectionId == "I") && d.IsCancelled == false && d.IsOperationalClosed == false);
                    }

                    if (item.FieldName == "DeparturesArrivalsFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            string myFieldValue = item.FieldValue.ToString();
                            List<string> ids = myFieldValue.Split('.').ToList();
                            queryableData = queryableData.Where(d => ids.Contains(d.Id));
                        }
                    }

                    /////////////////////////////

                    if (item.FieldName == "DeparturesArrivalsMobileFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            string code = item.FieldValue.ToString();
                            string directionId = "";

                            if (item.FieldValue2 != null)
                            {
                                if (!string.IsNullOrEmpty(item.FieldValue2.ToString()))
                                {
                                    directionId = item.FieldValue2.ToString();



                                    if (directionId == "I")
                                    {
                                        queryableData = queryableData.Where(d => (d.DirectionId == "I" || d.DirectionId == "C")  && (d.ShipmentLevelCode != "H" || (d.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(d.ShipmentMasterDataId))));
                                       
                                    }

                                    else
                                    {
                                        queryableData = queryableData.Where(d => d.DirectionId == directionId && (d.ShipmentLevelCode != "H" || (d.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(d.ShipmentMasterDataId))));
                                    }
                              
                                }
                                else
                                {
                                    queryableData = queryableData.Where(d => d.ShipmentLevelCode != "H" || (d.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(d.ShipmentMasterDataId)));
                                }


                            }
                            else
                            {
                                queryableData = queryableData.Where(d => d.ShipmentLevelCode != "H" || (d.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(d.ShipmentMasterDataId)));
                                //queryableData = queryableData.Where(d => d.ShipmentLevelCode != "H");
                            }

                            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;



                            DateTime date1 = todayDate;
                            DateTime date2 = todayDate;

                            // Note: the filter works as date1 >= && date2 <=
                            //       so, for tommorow & yesterday i use AddDays(1); && AddDays(1);

                            switch (code)
                            {
                                case "LSW_EXP":
                                case "LSW_ACT":
                                    {
                                        date1 = todayDate.AddDays(-7);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }

                                case "YES_EXP":
                                case "YES_ACT":
                                    {
                                        date1 = todayDate.AddDays(-1);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }

                                case "TOD_EXP":
                                case "TOD_ACT":
                                    {
                                        date1 = todayDate.AddDays(0);
                                        date2 = todayDate.AddDays(0);
                                        break;
                                    }

                                case "TOM_EXP":
                                case "TOM_ACT":
                                    {
                                        date1 = todayDate.AddDays(1);
                                        date2 = todayDate.AddDays(1);
                                        break;
                                    }

                                case "NXW_EXP":
                                case "NXW_ACT":
                                    {
                                        date1 = todayDate.AddDays(2);
                                        date2 = todayDate.AddDays(7);
                                        break;
                                    }
                            }

                            if ((code == "LSW_EXP" || code == "TOD_EXP"))
                            {
                                queryableData = queryableData.Where(d => d.MainCarriageATA == null || (d.DirectionId != "E" && d.DirectionId != "R" && d.DirectionId != "D"));
                            }


                            switch (code)
                            {
                                case "LSW_ACT":
                                case "YES_ACT":
                                case "TOD_ACT":
                                case "TOM_ACT":
                                case "NXW_ACT":
                                    {


                                        if (!string.IsNullOrEmpty(directionId))
                                        {
                                            if (directionId == "E" || directionId == "R")
                                            {
                                                queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATD) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATD) <= date2);
                                            }

                                            else
                                            {
                                                queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATA) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATA) <= date2);
                                            }
                                        }
                                        else
                                        {
                                            queryableData = queryableData.Where(d => (System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATD) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATD) <= date2 && (d.DirectionId == "E" || d.DirectionId == "R")) || (System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATA) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATA) <= date2) && (d.DirectionId != "E" && d.DirectionId != "R"));
                                        }


                                        break;
                                    }

                                default:
                                    {
                                        if (!string.IsNullOrEmpty(directionId))
                                        {
                                            if (directionId == "E" || directionId == "R")
                                            {
                                                queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETD) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETD) <= date2 && d.MainCarriageATD == null);
                                            }

                                            else
                                            {
                                                queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETA) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETA) <= date2 && d.MainCarriageATA == null);
                                            }

                                        }
                                        else
                                        {
                                            queryableData = queryableData.Where(d => (System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETD) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETD) <= date2 && d.MainCarriageATD == null && (d.DirectionId == "E" || d.DirectionId == "R")) || (System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETA) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETA) <= date2 && d.MainCarriageATA == null && (d.DirectionId != "E" && d.DirectionId != "R")));
                                        }

                                        break;
                                    }
                            }
                        }
                    }




                    if (item.FieldName == "ConnectedToOtherMastersFilter")
                    {
                        bool? value = item.FieldValue as bool?;
                        if (value == false)
                        {
                            queryableData = queryableData.Where(d => d.MasterShipmentDataId == null || d.ShipmentLevelCode == "D");
                        }
                    }

                    if (item.FieldName == "StatusId")
                    {
                        string value = Convert.ToString(item.FieldValue);
                        if (value != null)
                        {
                            queryableData = queryableData.Where(d => d.ShipmentStatusId == value || d.ShipmentMasterDataStatusId == value);
                        }
                    }

                    if (item.FieldName == "ContainerNumber")
                    {
                        string value = Convert.ToString(item.FieldValue);
                        if (value != null)
                        {
                            ShipmentPackageRepository shipmentPackageRepository = new ShipmentPackageRepository(tenant);
                            ShipmentDataView dataview = queryableData.FirstOrDefault();
                            List<ShipmentPackage> shipmentPackages = shipmentPackageRepository.GetShipmentPackages(dataview.Tenant).Where(d => d.ContainerNumber.StartsWith(value)).ToList();
                            List<string> shipmentIds = (from a in shipmentPackages
                                                        select a.ShipmentId).ToList();

                            queryableData = queryableData.Where(d => shipmentIds.Contains(d.Id));
                        }
                    }

                    if (item.FieldName == "ActualDataDate")
                    {
                        if (item.FieldValue != null)
                        {
                            DateTime myDateTime = Convert.ToDateTime(item.FieldValue);//(DateTime)item.FieldValue;
                            if (myDateTime != null)
                            {
                                queryableData = queryableData.Where(d => d.CreateDateTime.Month == myDateTime.Month && d.CreateDateTime.Year == myDateTime.Year);
                            }
                        }
                    }

                    if (item.FieldName == "InvoiceNumber")
                    {
                        string value = Convert.ToString(item.FieldValue);
                        if (value != null)
                        {
                            ARInvoiceEntityRepository invoiceRep = new ARInvoiceEntityRepository(tenant);
                            ShipmentDataView dataview = queryableData.FirstOrDefault();
                            List<ARInvoiceEntity> invoiceEntities = invoiceRep.GetInvoiceEntities(dataview.Tenant).Where(d => d.ARInvoice.InvoiceNumber.StartsWith(value)).ToList();
                            List<string> shipmentIds = (from a in invoiceEntities select a.EntityId).ToList();

                            queryableData = queryableData.Where(d => shipmentIds.Contains(d.Id));
                        }
                    }

                    if (item.FieldName == "ConnectedToQuote")
                    {
                        bool? value = item.FieldValue as bool?;

                        if (value == true)
                        {
                            queryableData = queryableData.Where(d => !string.IsNullOrEmpty(d.QuoteId));
                        }

                        else if (value == false)
                        {
                            queryableData = queryableData.Where(d => string.IsNullOrEmpty(d.QuoteId));
                        }
                    }

                    if (item.FieldName == "LastMonthShipments")
                    {
                        if (item.FieldValue != null)
                        {
                            bool? value = item.FieldValue as bool?;
                            if (value == true)
                            {
                                DateTime? currentDateTime = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                                int year = currentDateTime.Value.Year;
                                int month = currentDateTime.Value.Month;
                                if (month == 1)
                                {
                                    year = year - 1;
                                    month = 12;
                                }

                                else
                                {
                                    month = month - 1;
                                }

                                queryableData = queryableData.Where(d => d.CreateDateTime.Month == month && d.CreateDateTime.Year == year);
                            }
                        }
                    }

                    if (item.FieldName == "DailySpotlightFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            string code = item.FieldValue.ToString();

                            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                            DateTime date1 = todayDate;
                            DateTime date2 = todayDate;

                            switch (code)
                            {
                                case "SH_TD":
                                    {
                                        date1 = todayDate.AddDays(0);
                                        date2 = todayDate.AddDays(0);
                                        break;
                                    }

                                case "SH_YS":
                                    {
                                        date1 = todayDate.AddDays(-1);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }

                                case "SH_LW":
                                    {
                                        date1 = todayDate.AddDays(-7);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }
                            }

                            queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) <= date2);
                        }
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
                        //string value = item.FieldValue as string;

                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d => d.ForwarderShipmentNumber != null);
                        }
                    }

                    if (item.FieldName == "NotForwarderShipmentsFilter")
                    {
                        //string value = item.FieldValue as string;

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

                    if (item.FieldName == "AMANACShipmentsFilter")
                    {
                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d => !string.IsNullOrEmpty(d.MasterShipmentDataId));
                        }
                    }
                }
            }

            if (isMasterConnectedHouses)
            {
                return queryableData;
            }

            else
            {


                queryableData = queryableData.Where(d => d.IsCancelled == showIsCancelled && d.IsStandalonePickupDelivery == showIsStandalonePickupDelivery);

                return queryableData;
            }
        }
        public IQueryable<ShipmentList> GetFilteredQuery(QueryOperations operations, IQueryable<ShipmentList> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            bool showIsCancelled = false;
            bool showIsStandalonePickupDelivery = false;
            bool isMasterConnectedHouses = false;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "MasterConnectedHouses")
                    {
                        isMasterConnectedHouses = true;
                    }

                    if (item.FieldName == "IsCancelled")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            showIsCancelled = true;
                        }
                    }

                    if (item.FieldName == "IsStandalonePickupDelivery")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            showIsStandalonePickupDelivery= true;
                        }
                    }

                    if (item.FieldName == "Partner")
                    {
                        //string value = item.FieldValue as string;
                        //if (queryableData.Count() != 0)
                        //{
                        //    queryableData = queryableData.Where(d => d.AgentName.ToUpper().StartsWith(value.ToUpper())
                        //           || d.CustomAgentImportName.ToUpper().StartsWith(value.ToUpper()) ||
                        //                  d.ConsigneeName.ToUpper().StartsWith(value.ToUpper())
                        //                   );
                        //}
                    }

                    if (item.FieldName == "Reference")
                    {
                        string value = item.FieldValue as string;
                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(
                                d => d.ShipperReference1.StartsWith(value)
                                    || d.ShipperReference2.StartsWith(value)
                                    || d.ConsigneeReference1.StartsWith(value)
                                    || d.ConsigneeReference2.StartsWith(value)
                                    || d.ShipmentNumber.StartsWith(value)
                                    || d.House.StartsWith(value)
                                    || d.Master.StartsWith(value)
                                    );
                        }
                    }

                    if (item.FieldName == "FromOrToPort")
                    {
                        string value = item.FieldValue as string;

                        //if (queryableData.Count() != 0)
                        //{
                        //    queryableData = queryableData.Where(d => d.MainCarriageFromPortCode.ToUpper().StartsWith(value.ToUpper()) || d.MainCarriageToPortCode.ToUpper().StartsWith(value.ToUpper()));
                        //}
                    }


                    if (item.FieldName == "ShipmentReceivableStatusFilter")
                    {
                        string value = item.FieldValue as string;

                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d => d.ShipmentReceivableStatusCode.StartsWith(value.ToUpper()) || d.ShipmentReceivableStatusName.StartsWith(value.ToUpper()));
                        }
                    }

                    if (item.FieldName == "ShipmentPayableStatusFilter")
                    {
                        string value = item.FieldValue as string;

                        if (queryableData.Count() != 0)
                        {
                            queryableData.Where(d => d.ShipmentPayableStatusCode.StartsWith(value.ToUpper()) || d.ShipmentPayableStatusName.StartsWith(value.ToUpper()));
                        }
                    }

                    if (item.FieldName == "Client")
                    {
                        string value = item.FieldValue as string;
                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d => d.ShipperName.ToUpper().StartsWith(value.ToUpper()));
                        }
                    }

                    if (item.FieldName == "OpenShipments" || item.FieldName == "OpenMasters")
                    {
                        queryableData = queryableData.Where(d => d.IsOperationalClosed == false);
                    }

                    if (item.FieldName == "ClosedShipments")
                    {
                        queryableData = queryableData.Where(d => d.IsOperationalClosed == true);
                    }

                    if (item.FieldName == "AccountingOpen")
                    {
                        queryableData = queryableData.Where(d => d.IsOperationalClosed == true && d.IsAccountingClosed == false);
                    }

                    if (item.FieldName == "AllShipments")
                    {
                        queryableData = queryableData.Where(d => d.ShipmentLevelCode != "C" && d.IsCancelled == false);
                    }

                    if (item.FieldName == "AllMasters")
                    {
                        queryableData = queryableData.Where(d => d.ShipmentLevelCode != "H" && d.ShipmentLevelCode != "A" && d.IsCancelled == false);
                    }

                    if (item.FieldName == "OperationalOpenHousesDirects")
                    {
                        queryableData = queryableData.Where(d => d.IsOperationalClosed == false);
                        queryableData = queryableData.Where(d => d.ShipmentLevelCode == "D" || d.ShipmentLevelCode == "H");
                    }

                    if (item.FieldName == "OperationalOpenMastersDirects")
                    {
                        queryableData = queryableData.Where(d => d.IsOperationalClosed == false);
                        queryableData = queryableData.Where(d => d.ShipmentLevelCode == "D" || d.ShipmentLevelCode == "C" || d.ShipmentLevelCode == "A");
                    }

                    if (item.FieldName == "AccountingOpenHousesDirects")
                    {
                        queryableData = queryableData.Where(d => d.IsAccountingClosed == false);
                        queryableData = queryableData.Where(d => d.ShipmentLevelCode == "D" || d.ShipmentLevelCode == "H");
                        queryableData = queryableData.Where(d => d.ShipmentReceivableStatusCode == "OPEN");
                    }

                    if (item.FieldName == "AccountingOpenMastersDirects")
                    {
                        queryableData = queryableData.Where(d => d.IsAccountingClosed == false);
                        queryableData = queryableData.Where(d => d.ShipmentLevelCode == "D" || d.ShipmentLevelCode == "C");
                        queryableData = queryableData.Where(d => d.ShipmentPayableStatusCode == "OPEN");
                    }

                    if (item.FieldName == "ExpectedDepartures")
                    {
                        queryableData = queryableData.Where(d => d.ShipmentLevelCode != "H" && d.IsCancelled == false && d.MainCarriageATD == null && d.MainCarriageATA == null && d.MainCarriageETD != null && d.DirectionId == "E" && d.TransportModeId == "A");

                        EntityStatusRepository statusRepository = new EntityStatusRepository(tenant);
                        EntityStatus status_Arrived = statusRepository.GetSingleEntityStatusByCode("SARR", tenant);
                        EntityStatus status_Delivered = statusRepository.GetSingleEntityStatusByCode("SDLD", tenant);

                        if (status_Arrived != null)
                        {
                            queryableData = queryableData.Where(d => d.StatusId != status_Arrived.Id);
                        }

                        if (status_Delivered != null)
                        {
                            queryableData = queryableData.Where(d => d.StatusId != status_Delivered.Id);
                        }

                        DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                        DateTime lastWeekDate = todayDate.AddDays(-7);

                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETD) > lastWeekDate);
                    }

                    if (item.FieldName == "AirlinesUpdates")
                    {
                        DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                        DateTime lastWeekDate = todayDate.AddDays(-7);

                        queryableData = queryableData.Where(d => d.ShipmentLevelCode != "H" && d.TransportModeId == "A" && d.CarrierLastStatusDate >= lastWeekDate && d.IsCancelled == false);
                    }

                    if (item.FieldName == "ExpectedDeparturesNotTransmitted")
                    {
                        queryableData = from d in queryableData
                                        where
                                        d.ShipmentLevelCode != "H"
                                        && d.DirectionId == "E"
                                        && d.TransportModeId == "O"
                                        && d.INTTRASIStatusCode == "NSEN"
                                        && (d.ShipmentTypeId == "FCLD" || d.ShipmentTypeId == "MYGO")
                                        && d.MainCarriageATD == null
                                        && d.MainCarriageETD != null
                                        select d;
                    }

                    if (item.FieldName == "ShippingInstructionsLast7Days")
                    {
                        DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                        DateTime lastWeekDate = todayDate.AddDays(-7);

                        queryableData = from d in queryableData
                                        where d.INTTRASIStatusCode != "NSEN"
                                        && (d.INTTRASIStatusDate >= lastWeekDate || d.INTTRALastStatusDate >= lastWeekDate)
                                        select d;
                    }

                    if (item.FieldName == "SentFSR")
                    {
                        DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                        DateTime lastWeekDate = todayDate.AddDays(-7);

                        queryableData = queryableData.Where(d => d.ShipmentLevelCode != "H" && d.TransportModeId == "A" && d.LastFSRStatusRequestDate >= lastWeekDate && d.IsCancelled == false);
                    }

                    if (item.FieldName == "ImportShipments")
                    {
                        queryableData = queryableData.Where(d => d.ShipmentLevelCode != "C" && ((d.DirectionId == "C" && d.NoFreightFile == true) || d.DirectionId == "I") && d.IsCancelled == false && d.IsOperationalClosed == false);
                    }

                    if (item.FieldName == "DeparturesArrivalsFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            string myFieldValue = item.FieldValue.ToString();
                            List<string> ids = myFieldValue.Split('.').ToList();
                            queryableData = queryableData.Where(d => ids.Contains(d.Id));
                        }
                    }

                    /////////////////////////////

                    if (item.FieldName == "DeparturesArrivalsMobileFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            string code = item.FieldValue.ToString();
                            string directionId = "";

                            if (item.FieldValue2 != null)
                            {
                                if (!string.IsNullOrEmpty(item.FieldValue2.ToString()))
                                {
                                    directionId = item.FieldValue2.ToString();
                                    if (directionId == "I")
                                    {
                                        queryableData = queryableData.Where(d => (d.DirectionId == "I" || d.DirectionId == "C") && (d.ShipmentLevelCode != "H" || (d.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(d.ShipmentMasterDataId))));

                                    }

                                    else
                                    {
                                        queryableData = queryableData.Where(d => d.DirectionId == directionId && (d.ShipmentLevelCode != "H" || (d.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(d.ShipmentMasterDataId))));
                                    }

                                }
                                else
                                {
                                    queryableData = queryableData.Where(d => d.ShipmentLevelCode != "H" || (d.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(d.ShipmentMasterDataId)));
                                }


                            }
                            else
                            {
                                queryableData = queryableData.Where(d => d.ShipmentLevelCode != "H" || (d.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(d.ShipmentMasterDataId)));
                                //queryableData = queryableData.Where(d => d.ShipmentLevelCode != "H");
                            }

                            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;



                            DateTime date1 = todayDate;
                            DateTime date2 = todayDate;

                            // Note: the filter works as date1 >= && date2 <=
                            //       so, for tommorow & yesterday i use AddDays(1); && AddDays(1);

                            switch (code)
                            {
                                case "LSW_EXP":
                                case "LSW_ACT":
                                    {
                                        date1 = todayDate.AddDays(-7);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }

                                case "YES_EXP":
                                case "YES_ACT":
                                    {
                                        date1 = todayDate.AddDays(-1);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }

                                case "TOD_EXP":
                                case "TOD_ACT":
                                    {
                                        date1 = todayDate.AddDays(0);
                                        date2 = todayDate.AddDays(0);
                                        break;
                                    }

                                case "TOM_EXP":
                                case "TOM_ACT":
                                    {
                                        date1 = todayDate.AddDays(1);
                                        date2 = todayDate.AddDays(1);
                                        break;
                                    }

                                case "NXW_EXP":
                                case "NXW_ACT":
                                    {
                                        date1 = todayDate.AddDays(2);
                                        date2 = todayDate.AddDays(7);
                                        break;
                                    }
                            }

                            if ((code == "LSW_EXP" || code == "TOD_EXP"))
                            {
                                queryableData = queryableData.Where(d => d.MainCarriageATA == null || (d.DirectionId != "E" && d.DirectionId != "R" && d.DirectionId != "D"));
                            }


                            switch (code)
                            {
                                case "LSW_ACT":
                                case "YES_ACT":
                                case "TOD_ACT":
                                case "TOM_ACT":
                                case "NXW_ACT":
                                    {


                                        if (!string.IsNullOrEmpty(directionId))
                                        {
                                            if (directionId == "E" || directionId == "R")
                                            {
                                                queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATD) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATD) <= date2);
                                            }

                                            else
                                            {
                                                queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATA) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATA) <= date2);
                                            }
                                        }
                                        else
                                        {
                                            queryableData = queryableData.Where(d => (System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATD) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATD) <= date2 && (d.DirectionId == "E" || d.DirectionId == "R")) || (System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATA) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATA) <= date2) && (d.DirectionId != "E" && d.DirectionId != "R"));
                                        }


                                        break;
                                    }

                                default:
                                    {
                                        if (!string.IsNullOrEmpty(directionId))
                                        {
                                            if (directionId == "E" || directionId == "R")
                                            {
                                                queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETD) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETD) <= date2 && d.MainCarriageATD == null);
                                            }

                                            else
                                            {
                                                queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETA) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETA) <= date2 && d.MainCarriageATA == null);
                                            }

                                        }
                                        else
                                        {
                                            queryableData = queryableData.Where(d => (System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETD) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETD) <= date2 && d.MainCarriageATD == null && (d.DirectionId == "E" || d.DirectionId == "R")) || (System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETA) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETA) <= date2 && d.MainCarriageATA == null && (d.DirectionId != "E" && d.DirectionId != "R")));
                                        }

                                        break;
                                    }
                            }
                        }
                    }




                    if (item.FieldName == "ConnectedToOtherMastersFilter")
                    {
                        bool? value = item.FieldValue as bool?;
                        if (value == false)
                        {
                            queryableData = queryableData.Where(d => d.MasterShipmentDataId == null || d.ShipmentLevelCode == "D");
                        }
                    }

                    if (item.FieldName == "StatusId")
                    {
                        //string value = Convert.ToString(item.FieldValue);
                        //if (value != null)
                        //{
                        //    queryableData = queryableData.Where(d => d.StatusId == value || d.ShipmentMasterDataEntityStatusId == value);
                        //}
                    }

                    if (item.FieldName == "ContainerNumber")
                    {
                        string value = Convert.ToString(item.FieldValue);
                        if (value != null)
                        {
                            ShipmentPackageRepository shipmentPackageRepository = new ShipmentPackageRepository(tenant);
                            ShipmentList dataview = queryableData.FirstOrDefault();
                            List<ShipmentPackage> shipmentPackages = shipmentPackageRepository.GetShipmentPackages(dataview.Tenant).Where(d => d.ContainerNumber.StartsWith(value)).ToList();
                            List<string> shipmentIds = (from a in shipmentPackages
                                                        select a.ShipmentId).ToList();

                            queryableData = queryableData.Where(d => shipmentIds.Contains(d.Id));
                        }
                    }

                    if (item.FieldName == "ActualDataDate")
                    {
                        if (item.FieldValue != null)
                        {
                            DateTime myDateTime = (DateTime)item.FieldValue;
                            if (myDateTime != null)
                            {
                                queryableData = queryableData.Where(d => d.CreateDateTime.Month == myDateTime.Month && d.CreateDateTime.Year == myDateTime.Year);
                            }
                        }
                    }

                    if (item.FieldName == "InvoiceNumber")
                    {
                        string value = Convert.ToString(item.FieldValue);
                        if (value != null)
                        {
                            ARInvoiceEntityRepository invoiceRep = new ARInvoiceEntityRepository(tenant);
                            ShipmentList dataview = queryableData.FirstOrDefault();
                            List<ARInvoiceEntity> invoiceEntities = invoiceRep.GetInvoiceEntities(dataview.Tenant).Where(d => d.ARInvoice.InvoiceNumber.StartsWith(value)).ToList();
                            List<string> shipmentIds = (from a in invoiceEntities select a.EntityId).ToList();

                            queryableData = queryableData.Where(d => shipmentIds.Contains(d.Id));
                        }
                    }

                    if (item.FieldName == "ConnectedToQuote")
                    {
                        bool? value = item.FieldValue as bool?;

                        if (value == true)
                        {
                            queryableData = queryableData.Where(d => !string.IsNullOrEmpty(d.QuoteId));
                        }

                        else if (value == false)
                        {
                            queryableData = queryableData.Where(d => string.IsNullOrEmpty(d.QuoteId));
                        }
                    }

                    if (item.FieldName == "LastMonthShipments")
                    {
                        if (item.FieldValue != null)
                        {
                            bool? value = item.FieldValue as bool?;
                            if (value == true)
                            {
                                DateTime? currentDateTime = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                                int year = currentDateTime.Value.Year;
                                int month = currentDateTime.Value.Month;
                                if (month == 1)
                                {
                                    year = year - 1;
                                    month = 12;
                                }

                                else
                                {
                                    month = month - 1;
                                }

                                queryableData = queryableData.Where(d => d.CreateDateTime.Month == month && d.CreateDateTime.Year == year);
                            }
                        }
                    }

                    if (item.FieldName == "DailySpotlightFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            string code = item.FieldValue.ToString();

                            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                            DateTime date1 = todayDate;
                            DateTime date2 = todayDate;

                            switch (code)
                            {
                                case "SH_TD":
                                    {
                                        date1 = todayDate.AddDays(0);
                                        date2 = todayDate.AddDays(0);
                                        break;
                                    }

                                case "SH_YS":
                                    {
                                        date1 = todayDate.AddDays(-1);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }

                                case "SH_LW":
                                    {
                                        date1 = todayDate.AddDays(-7);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }
                            }

                            queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) <= date2);
                        }
                    }

                    if (item.FieldName == "ImportersFilter")
                    {
                        string value = item.FieldValue as string;

                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d => d.SearchFields.ToUpper().Contains(value.ToUpper()) || d.DocumentsSearchFields.ToUpper().Contains(value.ToUpper()));
                        }
                    }
                }
            }

            if (isMasterConnectedHouses)
            {
                return queryableData;
            }

            else
            {

                queryableData = queryableData.Where(d => d.IsCancelled == showIsCancelled && d.IsStandalonePickupDelivery == showIsStandalonePickupDelivery);

                return queryableData;
            }
        }

    }
}
