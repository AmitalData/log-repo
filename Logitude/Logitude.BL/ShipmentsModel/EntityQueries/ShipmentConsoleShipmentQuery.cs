using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentConsoleShipmentQuery
    {
        private int tenant;
        private IShipmentsContext myContext;
        private ShipmentRepository myRepository;
        public ShipmentConsoleShipmentQuery(IShipmentsContext myContext)
        {
            this.myContext = myContext;
            this.myRepository = new ShipmentRepository(myContext);
        }

        public void BuildConsoleShipments(ShipmentPM shipmentPM)
        {
            this.tenant = shipmentPM.Tenant;
            shipmentPM.ShipmentConsoleShipments = new List<ConsoleShipmentPM>();

            FHLStatusRepository fHLStatusRepository = new FHLStatusRepository(this.myContext);

            List<Shipment> allConnectedHouses = myRepository.GetHouseShipmentsForMaster(shipmentPM.Id, this.tenant);
            List<string> allConnectedHousesIds = new List<string>();

            foreach (Shipment item in allConnectedHouses)
            {
                #region
                allConnectedHousesIds.Add(item.Id);

                ConsoleShipmentPM consoleShipmentPM = new ConsoleShipmentPM()
                {
                    Id = item.Id,
                    MasterShipmentDataId = shipmentPM.Id,
                    ShipmentNumber = item.ShipmentNumber,
                    TEU = item.TEU,
                    Volume = item.Volume,
                    GrossWeight = item.GrossWeight,
                    GrossWeightInKG = item.GrossWeightInKG,
                    GrossWeightPerTon = item.GrossWeightPerTon,
                    ChargeableWeight = item.ChargeableWeight,
                    VolumetricWeight = item.VolumetricWeight,
                    ValueOfGoods = item.ValueOfGoods,
                    FNAReason = item.FNAReason,
                    FHLStatusCode = item.FHLStatusCode,
                    CargonautFHLStatusCode = item.CargonautFHLStatusCode,
                    OAMTPayables_Local = item.OpenPayablesInLocalCurrency,
                    ACCTPayables_Local = item.AccountedPayablesInLocalCurrency,
                    OAMTPayables_Profit = item.OpenPayablesInProfitCurrency,
                    ACCTPayables_Profit = item.AccountedPayablesInProfitCurrency,
                    OAMTReceivables_Local = item.OpenReceivablesInLocalCurrency,
                    ACCTReceivables_Local = item.AccountedReceivablesInLocalCurrency,
                    OAMTReceivables_Profit = item.OpenReceivablesInProfitCurrency,
                    ACCTReceivables_Profit = item.AccountedReceivablesInProfitCurrency,
                    NumberOfPackages = item.NumberOfPackages,
                    NumberOfContainers = item.NumberOfContainers,
                    IsFCL = ((item.TransportModeId == "O" && item.ShipmentTypeId == "FCLD") || (item.TransportModeId == "I" && item.ShipmentTypeId == "FTL")),
                    IsLCL = (item.TransportModeId == "A" || (item.TransportModeId == "O" && item.ShipmentTypeId == "LCLD") || (item.TransportModeId == "I" && item.ShipmentTypeId == "LTL")),
                    GrossWeightPerStorageDays = item.GrossWeightPerStorageDays,
                    House = item.House,
                    DescriptionOfGoods = item.DescriptionOfGoods,
                    PreForwardingFromPortId = item.PreForwardingFromPortId,
                    PreForwardingToPortId = item.PreForwardingToPortId,
                    OnForwardingFromPortId = item.OnForwardingFromPortId,
                    OnForwardingToPortId = item.OnForwardingToPortId,
                };

                if (consoleShipmentPM.IsFCL)
                {
                    ShipmentPackageRepository myShipmentPackageRepository = new ShipmentPackageRepository(myContext);
                    ShipmentPackageQuery shipmentPackageQuery = new ShipmentPackageQuery(myShipmentPackageRepository);
                    consoleShipmentPM.FCLDataList = shipmentPackageQuery.GetFCLPackagesData(item.Id, tenant);
                }

                if (!string.IsNullOrEmpty(consoleShipmentPM.FHLStatusCode))
                {
                    FHLStatus fHLStatus = fHLStatusRepository.GetSingleFHLStatus(consoleShipmentPM.FHLStatusCode);
                    if (fHLStatus != null)
                    {
                        consoleShipmentPM.FHLStatusName = fHLStatus.Name;
                    }
                }

                if (!string.IsNullOrEmpty(consoleShipmentPM.CargonautFHLStatusCode))
                {
                    FHLStatus fHLStatus = fHLStatusRepository.GetSingleFHLStatus(consoleShipmentPM.CargonautFHLStatusCode);
                    if (fHLStatus != null)
                    {
                        consoleShipmentPM.CargonautFHLStatusName = fHLStatus.Name;
                    }
                }

                shipmentPM.ShipmentConsoleShipments.Add(consoleShipmentPM);
                #endregion
            }

            shipmentPM.ConnectedShipments = allConnectedHouses.Count;

            if (shipmentPM.ConnectedShipments == 0)
            {
                shipmentPM.ConnectedShipmentsPayablesCount = 0;
                shipmentPM.ConnectedShipmentsReceivablesCount = 0;
            }

            else
            {
                #region
                var allPayablesData = (from d in this.myContext.ShipmentPayables.Include("ChargesType")
                                       where d.Tenant == tenant
                                       && allConnectedHousesIds.Contains(d.ShipmentId)
                                       select new
                                       {
                                           ShipmentId = d.ShipmentId,
                                           ExpectedAmount = d.ExpectedAmount,
                                           IsFreight = d.ChargesType == null ? false : (d.ChargesType.ChargesGroupCode == "FRT" ? true : false)
                                       }).ToList();

                var allReceivablesData = (from d in this.myContext.ShipmentReceivables.Include("ChargesType")
                                          where d.Tenant == tenant
                                          && allConnectedHousesIds.Contains(d.ShipmentId)
                                          select new
                                          {
                                              ShipmentId = d.ShipmentId,
                                              TotalAmount = d.TotalAmount,
                                              TotalAmountLocal = d.TotalAmountLocal,
                                              AmountInProfitCurrency = d.AmountInProfitCurrency,
                                              ShipmentReceivableLineStatusCode = d.ShipmentReceivableLineStatusCode,
                                              ShipmentReceivableParentId = d.ShipmentReceivableParentId,
                                              IsFreight = d.ChargesType == null ? false : (d.ChargesType.ChargesGroupCode == "FRT" ? true : false)
                                          }).ToList();

                shipmentPM.ConnectedShipmentsPayablesCount = allPayablesData.Count;
                shipmentPM.ConnectedShipmentsReceivablesCount = allReceivablesData.Count;

                foreach (ConsoleShipmentPM item in shipmentPM.ShipmentConsoleShipments)
                {
                    item.FreightPayablesAmount = allPayablesData.Where(d => d.ShipmentId == item.Id && d.IsFreight).Sum(s => s.ExpectedAmount);
                    item.FreightReceivablesAmount = allReceivablesData.Where(d => d.ShipmentId == item.Id && d.IsFreight).Sum(s => s.TotalAmount);

                    var myReceivables = allReceivablesData.Where(d => d.ShipmentId == item.Id && d.ShipmentReceivableParentId == null).ToList();
                    if (myReceivables.Count > 0)
                    {
                        //item.OAMTReceivables_Local_NoParent = myReceivables.Where(d => d.ShipmentReceivableLineStatusCode == "EMPT" || d.ShipmentReceivableLineStatusCode == "OAMT").Sum(s => s.TotalAmountLocal);
                        //item.ACCTReceivables_Local_NoParent = myReceivables.Where(d => d.ShipmentReceivableLineStatusCode == "DRFT" || d.ShipmentReceivableLineStatusCode == "ACCT").Sum(s => s.TotalAmountLocal);
                        //item.OAMTReceivables_Profit_NoParent = myReceivables.Where(d => d.ShipmentReceivableLineStatusCode == "EMPT" || d.ShipmentReceivableLineStatusCode == "OAMT").Sum(s => s.AmountInProfitCurrency);
                        //item.ACCTReceivables_Profit_NoParent = myReceivables.Where(d => d.ShipmentReceivableLineStatusCode == "DRFT" || d.ShipmentReceivableLineStatusCode == "ACCT").Sum(s => s.AmountInProfitCurrency);

                        item.OAMTReceivables_Local_NoParent = myReceivables.Where(d => d.ShipmentReceivableLineStatusCode != "ACCT").Sum(s => s.TotalAmountLocal);
                        item.ACCTReceivables_Local_NoParent = myReceivables.Where(d => d.ShipmentReceivableLineStatusCode == "ACCT").Sum(s => s.TotalAmountLocal);
                        item.OAMTReceivables_Profit_NoParent = myReceivables.Where(d => d.ShipmentReceivableLineStatusCode != "ACCT").Sum(s => s.AmountInProfitCurrency);
                        item.ACCTReceivables_Profit_NoParent = myReceivables.Where(d => d.ShipmentReceivableLineStatusCode == "ACCT").Sum(s => s.AmountInProfitCurrency);
                    }
                }
                #endregion
            }

        }

        public List<Shipment> GetMasterConnectedHouseShipments(string entityId, int tenant)
        {
            List<Shipment> allConnectedHouses = myRepository.GetHouseShipmentsForMaster(entityId, tenant);

            return allConnectedHouses;
        }
    }
}
