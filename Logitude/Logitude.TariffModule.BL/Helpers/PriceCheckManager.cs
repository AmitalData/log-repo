using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.TariffModule.BL.DataContracts;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.Repositories;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Server.Tools.StorageService;
using Logitude.TariffModule.Data;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using System.Reflection;

namespace Logitude.TariffModule.BL.Helpers
{
    public class PriceCheckManager
    {
        private int tenant;
        private TariffSearchArgs args;
        private ICommonDataContext commonContext;
        private AirlineRepository airlineRepository;
        private AirlineQuery airlineQuery;
        private ShippingLineRepository shippingLineRepository;
        private ShippingLineQuery shippingLineQuery;
        private CardRepository cardQuery;
        private TariffRepository tariffRepository;
        private ITariffModuleContext tariffContext;
        private PackageTypeRepository packageTypeRepository;
        private string fromPort;
        private string toPort;
        private DateTime? betweenDate;
        private double? weight;
        private string weightCode;
        private double? grossWeight;
        private string grossWeightCode;
        private double? volume;
        private string volumeCode;
        private string currencyId;
        private string tariffType;
        private string tarrifSellerName;
        private string documentId;
        private int quantity1;
        private int quantity2;
        private int quantity3;
        private int quantity4;
        private int quantity5;
        private string product;
        private List<TariffSearchSummary> tariffSearchSummaries;
        private IQueryable<TariffLine> tariffLines_IQueryable;
        private List<string> tariffids;
        private List<TariffLinesContainersPrice> tariffLinesContainersPrices;
        private List<Tariff> tariffList;
        private List<TariffLine> tariffLinesList;
        private List<TariffVersion> tariffVersionList;
        private Dictionary<string, string> currencies;
        private List<TariffVersionAllInCharge> tariffVersionAllInChargesList;
        private List<Tariff> surchargeTariffList;
        private List<Measurement> usedMeasurements;
        private List<ChargesType> chargesTypes;
        private List<TariffWithContainerNumber> tariffWithContainerNumbers;
        private Dictionary<string, List<TariffLine>> surchargeTariffLines;
        private Dictionary<string, List<TariffLine>> surchargeTariffLinesFiltered;
        private TariffSearchSummary tariffsSummary;
        private decimal? Sum;
        private decimal? Sum_WithoutAllIn;
        private SurchargeSummary SurchargeItem;
        private Tariff CurrentSurcharge;
        private string freightTariffId;
        private string containerType1Id;
        private string containerType2Id;
        private string containerType3Id;
        private string containerType4Id;
        private string containerType5Id;
        private string containerType1;
        private string containerType2;
        private string containerType3;
        private string containerType4;
        private string containerType5;

        public PriceCheckManager(TariffSearchArgs args, int tenant)
        {
            this.tenant = tenant;
            this.args = args;
            this.commonContext = CommonDataContext.GetContext(tenant);
            this.airlineRepository = new AirlineRepository(commonContext);
            this.shippingLineRepository = new ShippingLineRepository(commonContext);
            this.cardQuery = new CardRepository(commonContext);
            this.airlineQuery = new AirlineQuery(airlineRepository);
            this.shippingLineQuery = new ShippingLineQuery(shippingLineRepository);
            this.tariffContext = TariffModuleContext.GetContext(tenant);
            this.tariffRepository = new TariffRepository(tenant);
            this.packageTypeRepository = new PackageTypeRepository(tenant);
            this.SetSearchProperties();
        }

        public PriceCheckManager(string freightTariffId, string shipmentId, string tariffType, int tenant)
        {
            this.tenant = tenant;            
            this.commonContext = CommonDataContext.GetContext(tenant);
            this.airlineRepository = new AirlineRepository(commonContext);
            this.shippingLineRepository = new ShippingLineRepository(commonContext);
            this.cardQuery = new CardRepository(commonContext);
            this.airlineQuery = new AirlineQuery(airlineRepository);
            this.shippingLineQuery = new ShippingLineQuery(shippingLineRepository);
            this.tariffContext = TariffModuleContext.GetContext(tenant);
            this.tariffRepository = new TariffRepository(tenant);
            this.freightTariffId = freightTariffId;

            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            ShipmentPM shipment = shipmentQuery.GetSinglePMWithoutComposition(shipmentId, tenant);

            if (shipment != null)
            {
                betweenDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                if (shipment.MainCarriageATD != null)
                {
                    betweenDate = shipment.MainCarriageATD;
                }
                else if (shipment.MainCarriageETD != null)
                {
                    betweenDate = shipment.MainCarriageETD;
                }

                fromPort = shipment.FromPortId;
                toPort = shipment.ToPortId;
                weight = shipment.ChargeableWeight;
                weightCode = shipment.ChargeableWeightUnitCode;
                grossWeight = shipment.GrossWeight;
                grossWeightCode = shipment.GrossWeightUnitCode;
                volume = shipment.Volume;
                volumeCode = shipment.VolumeUnitCode;
                //quantity1 = shipment.Quantity1 != null ? shipment.Quantity1.Value : 0;
                //quantity2 = shipment.Quantity2 != null ? shipment.Quantity2.Value : 0;
                //quantity3 = shipment.Quantity3 != null ? shipment.Quantity3.Value : 0;
                //quantity4 = shipment.Quantity4 != null ? shipment.Quantity4.Value : 0;
                //quantity5 = shipment.Quantity5 != null ? shipment.Quantity5.Value : 0;
                //containerType1Id = shipment.PackageTypeId1;
                //containerType2Id = shipment.PackageTypeId2;
                //containerType3Id = shipment.PackageTypeId3;
                //containerType4Id = shipment.PackageTypeId4;
                //containerType5Id = shipment.PackageTypeId5;

                if (tariffType == "OFC")
                {
                    this.SetContainersInitialValues(shipment);
                }
            }

            this.tariffType = tariffType;            
        }
        private void SetContainersInitialValues(ShipmentPM shipment)
        {
            List<ByPckageType> BCNTGroupedList = new List<ByPckageType>();

            ShipmentPackageQuery shipmentPackageQuery = new ShipmentPackageQuery(tenant);
            List<ShipmentPackagePM> shipmentPackages = shipmentPackageQuery.GetShipmentPackages(shipment.Id, shipment.ShipmentNumber, tenant);
            if (shipmentPackages.Count > 0)
            {
                foreach (ShipmentPackagePM item in shipmentPackages.Where(f => f.IsContainer == true))
                {
                    var itemGrouped = BCNTGroupedList.Where(f => f.PackageTypeId == item.PackageTypeId).FirstOrDefault();
                    if (itemGrouped == null)
                    {
                        itemGrouped = new ByPckageType();
                        itemGrouped.PackageTypeId = item.PackageTypeId;
                        itemGrouped.Quantity = item.Quantity;

                        if (itemGrouped.Quantity == null)
                        {
                            itemGrouped.Quantity = 0;
                        }

                        BCNTGroupedList.Add(itemGrouped);
                    }

                    else {
                        if (item.Quantity != null) {
                            itemGrouped.Quantity += item.Quantity;
                        }
                    }
                }
            }

            ByPckageType[] BCNTGrouped = new ByPckageType[5];
            for (int i = 0; i< BCNTGroupedList.Count; i ++)
            {
                BCNTGrouped[i] = BCNTGroupedList.ElementAt(i);
            }

            containerType1Id = BCNTGrouped[0] != null ? BCNTGrouped[0].PackageTypeId : null;
            containerType2Id = BCNTGrouped[1] != null ? BCNTGrouped[1].PackageTypeId : null;
            containerType3Id = BCNTGrouped[2] != null ? BCNTGrouped[2].PackageTypeId : null;
            containerType4Id = BCNTGrouped[3] != null ? BCNTGrouped[3].PackageTypeId : null;
            containerType5Id = BCNTGrouped[4] != null ? BCNTGrouped[4].PackageTypeId : null;
            containerType1 = (containerType1Id == null) ? null :  packageTypeRepository.GetSinglePackageType(containerType1Id, tenant).Code;
            containerType2 = (containerType2Id == null) ? null : packageTypeRepository.GetSinglePackageType(containerType2Id, tenant).Code;
            containerType3 = (containerType3Id == null) ? null : packageTypeRepository.GetSinglePackageType(containerType3Id, tenant).Code;
            containerType4 = (containerType4Id == null) ? null : packageTypeRepository.GetSinglePackageType(containerType4Id, tenant).Code;
            containerType5 = (containerType5Id == null) ? null : packageTypeRepository.GetSinglePackageType(containerType5Id, tenant).Code;
            quantity1 = BCNTGrouped[0] != null ? (BCNTGrouped[0].Quantity == null ? 0 : BCNTGrouped[0].Quantity.Value) : 0;
            quantity2 = BCNTGrouped[1] != null ? (BCNTGrouped[1].Quantity == null ? 0 : BCNTGrouped[1].Quantity.Value) : 0;
            quantity3 = BCNTGrouped[2] != null ? (BCNTGrouped[2].Quantity == null ? 0 : BCNTGrouped[2].Quantity.Value) : 0;
            quantity4 = BCNTGrouped[3] != null ? (BCNTGrouped[3].Quantity == null ? 0 : BCNTGrouped[3].Quantity.Value) : 0;
            quantity5 = BCNTGrouped[4] != null ? (BCNTGrouped[4].Quantity == null ? 0 : BCNTGrouped[4].Quantity.Value) : 0;
        }

        private void SetSearchProperties()
        {
            fromPort = args.OriginPortId;
            toPort = args.DestinationPortId;
            betweenDate = args.BetweenDate;
            weight = args.Weight;
            weightCode = args.WeightCode;
            grossWeight = args.GrossWeight;
            grossWeightCode = args.GrossWeightCode;
            volume = args.Volume;
            volumeCode = args.VolumeUnitCode;
            currencyId = args.CurrencyId;
            tariffType = args.TariffType;
            quantity1 = args.Quantity1 != null ? args.Quantity1.Value : 0;
            quantity2 = args.Quantity2 != null ? args.Quantity2.Value : 0;
            quantity3 = args.Quantity3 != null ? args.Quantity3.Value : 0;
            quantity4 = args.Quantity4 != null ? args.Quantity4.Value : 0;
            quantity5 = args.Quantity5 != null ? args.Quantity5.Value : 0;
            product = args.ProductId;
            containerType1Id = args.ContainerType1Id;
            containerType1 = (containerType1Id == null) ? null : packageTypeRepository.GetSinglePackageType(containerType1Id, tenant).Code;
            containerType2Id = args.ContainerType2Id;
            containerType2= (containerType2Id == null) ? null : packageTypeRepository.GetSinglePackageType(containerType2Id, tenant).Code;
            containerType3Id = args.ContainerType3Id;
            containerType3 = (containerType3Id == null) ? null : packageTypeRepository.GetSinglePackageType(containerType3Id, tenant).Code;
            containerType4Id = args.ContainerType4Id;
            containerType4 = (containerType4Id == null) ? null : packageTypeRepository.GetSinglePackageType(containerType4Id, tenant).Code;
            containerType5Id = args.ContainerType5Id;
            containerType5 = (containerType5Id == null) ? null : packageTypeRepository.GetSinglePackageType(containerType5Id, tenant).Code;
        }

        public List<TariffSearchSummary> GetSummary()
        {
            List<TariffSearchSummary> myResult = new List<TariffSearchSummary>();

            IQueryable<TariffLine> iQueryable = this.tariffRepository.GetAllTariffLines(tenant);

            if (tariffType == "OFC")
            {
                myResult = this.GetTariffSearchSummary_FCL(iQueryable);
            }
            else
            {
                myResult = this.GetTariffSearchSummary(iQueryable);
            }            

            return myResult;
        }
        public List<TariffSearchSummary> GetSummaryForExistedTariff()
        {
            List<TariffSearchSummary> myResult = new List<TariffSearchSummary>();

            IQueryable<TariffLine> iQueryable = this.tariffRepository.GetAllTariffLinesByTariffId(freightTariffId, tenant);
            int count = iQueryable.Count();

            if (tariffType == "OFC")
            {
                myResult = this.GetTariffSearchSummary_FCL(iQueryable);
            }
            else
            {
                myResult = this.GetTariffSearchSummary(iQueryable);
            }

            return myResult;
        }
        
        private List<TariffSearchSummary> GetTariffSearchSummary(IQueryable<TariffLine> iQueryable)
        {
            List<TariffSearchSummary> tariffSearchSummaries = new List<TariffSearchSummary>();            
            iQueryable = iQueryable.Where(p => p.OriginPortId == fromPort && p.DestinationPortId == toPort && System.Data.Entity.DbFunctions.TruncateTime(p.StartDate) <= System.Data.Entity.DbFunctions.TruncateTime(betweenDate) && (p.ExpirationDate != null ? (System.Data.Entity.DbFunctions.TruncateTime(p.ExpirationDate) >= System.Data.Entity.DbFunctions.TruncateTime(betweenDate)) : true));            
            List<string> tariffids = iQueryable.Select(p => p.TariffId).Distinct().ToList();
            TariffSettingRepository tariffSettingRepository = new TariffSettingRepository(tenant);
            List<TariffSetting> setting = tariffSettingRepository.GetAll(tenant).ToList();

            List<string> Steps = new List<string>();
            this.GetRates();
            List<TariffResult> items = new List<TariffResult>();
            
            int propIndex = -1;
            if (setting != null)
            {
                if (setting.Count > 0)
                {
                    Steps = setting[0].DefaultPriceSteps.Split(',').ToList();
                    int index = 0;
                    Steps.ForEach(item =>
                    {
                        if (float.Parse(item) > weight)
                        {
                            propIndex = index;
                            return;
                        }

                        else if (float.Parse(item) == weight)
                        {
                            propIndex = ++index;
                            return;
                        }

                        else
                        {
                            index++;
                        }
                    });

                    if (propIndex == -1)
                    {
                        propIndex = Steps.Count;
                    }
                    else if (propIndex == 0)
                    {
                        // propIndex = 1;
                    }
                }
            }

            List<Tariff> TariffListTemp = this.tariffRepository.GetAllTariff(tariffids.ToArray(), tenant).Where(p => !p.InActive && p.TypeCode == tariffType).ToList();

            if (TariffListTemp.Count > 0)
            {
                if (string.IsNullOrEmpty(currencyId))
                {
                    currencyId = TariffListTemp.FirstOrDefault().CurrencyId;
                }
            }

            if (!string.IsNullOrEmpty(product) && tariffType == "AFC")
            {
                TariffListTemp = TariffListTemp.Where(p => p.TariffProductId == product).ToList();
            }

            if (TariffListTemp != null && TariffListTemp.Count > 0)
            {
                TariffListTemp.ForEach(itemStep =>
                {
                    int initialPropIndex = -1;

                    if (!string.IsNullOrEmpty(itemStep.PriceSteps))
                    {
                        List<string> initialSteps = itemStep.PriceSteps.Split(',').ToList();
                        int index = 0;
                        initialSteps.ForEach(item =>
                        {
                            if (float.Parse(item) > weight)
                            {
                                initialPropIndex = index;
                                return;
                            }

                            else if (float.Parse(item) == weight)
                            {
                                initialPropIndex = ++index;
                                return;
                            }

                            else
                            {
                                index++;
                            }
                        });

                        if (initialPropIndex == -1)
                        {
                            initialPropIndex = initialSteps.Count;
                        }
                    }
                    else
                    {
                        initialPropIndex = propIndex;
                    }

                    if (initialPropIndex == 0)
                    {
                        items = items.Concat((from item in iQueryable
                                              group iQueryable by new
                                              {
                                                  item.TariffId,
                                                  item.MinPrice,
                                                  item.Version,
                                              } into g
                                              select new TariffResult()
                                              {
                                                  Price = g.Min(p => g.Key.MinPrice),
                                                  tariffid = g.Key.TariffId,
                                                  TariffVersion = g.Key.Version,
                                                  PriceIndex = 0,

                                              })).ToList();
                    }

                    if (initialPropIndex == 1)
                    {
                        items = items.Concat((from item in iQueryable
                                              group iQueryable by new
                                              {
                                                  item.TariffId,
                                                  item.Step1Price,
                                                  item.Version,
                                              } into g
                                              select new TariffResult()
                                              {
                                                  Price = g.Min(p => g.Key.Step1Price),
                                                  tariffid = g.Key.TariffId,
                                                  TariffVersion = g.Key.Version,
                                                  PriceIndex = 1,
                                              })).ToList();
                    }

                    else if (initialPropIndex == 2)
                    {
                        items = items.Concat((from item in iQueryable
                                              group iQueryable by new
                                              {
                                                  item.TariffId,
                                                  item.Step2Price,
                                                  item.Version,
                                              } into g
                                              select new TariffResult()
                                              {
                                                  Price = g.Min(p => g.Key.Step2Price),
                                                  tariffid = g.Key.TariffId,
                                                  TariffVersion = g.Key.Version,
                                                  PriceIndex = 2,
                                              })).ToList();
                    }

                    else if (initialPropIndex == 3)
                    {
                        items = items.Concat((from item in iQueryable
                                              group iQueryable by new
                                              {
                                                  item.TariffId,
                                                  item.Step3Price,
                                                  item.Version,
                                              } into g
                                              select new TariffResult()
                                              {
                                                  Price = g.Min(p => g.Key.Step3Price),
                                                  tariffid = g.Key.TariffId,
                                                  TariffVersion = g.Key.Version,
                                                  PriceIndex = 3,
                                              })).ToList();
                    }

                    else if (initialPropIndex == 4)
                    {
                        items = items.Concat((from item in iQueryable
                                              group iQueryable by new
                                              {
                                                  item.TariffId,
                                                  item.Step4Price,
                                                  item.Version,
                                              } into g
                                              select new TariffResult()
                                              {
                                                  Price = g.Min(p => g.Key.Step4Price),
                                                  tariffid = g.Key.TariffId,
                                                  TariffVersion = g.Key.Version,
                                                  PriceIndex = 4,
                                              })).ToList();
                    }

                    else if (initialPropIndex == 5)
                    {
                        items = items.Concat((from item in iQueryable
                                              group iQueryable by new
                                              {
                                                  item.TariffId,
                                                  item.Step5Price,
                                                  item.Version,
                                              } into g
                                              select new TariffResult()
                                              {
                                                  Price = g.Min(p => g.Key.Step5Price),
                                                  tariffid = g.Key.TariffId,
                                                  TariffVersion = g.Key.Version,
                                                  PriceIndex = 5,
                                              })).ToList();
                    }

                    else if (initialPropIndex == 6)
                    {
                        items = items.Concat((from item in iQueryable
                                              group iQueryable by new
                                              {
                                                  item.TariffId,
                                                  item.Step6Price,
                                                  item.Version,
                                              } into g
                                              select new TariffResult()
                                              {
                                                  Price = g.Min(p => g.Key.Step6Price),
                                                  tariffid = g.Key.TariffId,
                                                  TariffVersion = g.Key.Version,
                                                  PriceIndex = 6,
                                              })).ToList();
                    }

                    else if (initialPropIndex == 7)
                    {
                        items = items.Concat((from item in iQueryable
                                              group iQueryable by new
                                              {
                                                  item.TariffId,
                                                  item.Step7Price,
                                                  item.Version,
                                              } into g
                                              select new TariffResult()
                                              {
                                                  Price = g.Min(p => g.Key.Step7Price),
                                                  tariffid = g.Key.TariffId,
                                                  TariffVersion = g.Key.Version,
                                                  PriceIndex = 7,
                                              })).ToList();
                    }

                    else if (initialPropIndex == 8)
                    {
                        items = items.Concat((from item in iQueryable
                                              group iQueryable by new
                                              {
                                                  item.TariffId,
                                                  item.Step8Price,
                                                  item.Version,
                                              } into g
                                              select new TariffResult()
                                              {
                                                  Price = g.Min(p => g.Key.Step8Price),
                                                  tariffid = g.Key.TariffId,
                                                  TariffVersion = g.Key.Version,
                                                  PriceIndex = 8,
                                              })).ToList();
                    }
                });
            }
            
            List<Tariff> TariffList = this.tariffRepository.GetAllTariff(items.Select(p => p.tariffid).ToArray(), tenant).Where(p => !p.InActive && p.TypeCode == tariffType).ToList();
            if (!string.IsNullOrEmpty(product) && tariffType == "AFC")
            {
                TariffList = TariffList.Where(p => p.TariffProductId == product).ToList();
            }

            List<TariffVersion> TariffVersionList = tariffRepository.GetAllTariffVersionsByTariffIds(items.Select(p => p.tariffid).ToArray(), tenant).ToList();
            List<int> VersionIds = TariffVersionList.Select(a => a.Version).ToList();

            Dictionary<string, List<TariffLine>> TariffLines = tariffRepository.GetAllTariffLinesByTariffIds(TariffList.Select(p => p.Id).ToArray(), tenant).Where(p => VersionIds.Contains(p.Version)).Where(p => System.Data.Entity.DbFunctions.TruncateTime(p.StartDate) <= System.Data.Entity.DbFunctions.TruncateTime(betweenDate) && p.ExpirationDate != null ? (System.Data.Entity.DbFunctions.TruncateTime(p.ExpirationDate) >= System.Data.Entity.DbFunctions.TruncateTime(betweenDate)) : true).GroupBy(p => p.TariffId).ToDictionary(o => o.Key, o => o.ToList());
            Dictionary<string, List<TariffLine>> TariffLinesFiltered = new Dictionary<string, List<TariffLine>>();

            foreach (KeyValuePair<string, List<TariffLine>> entry in TariffLines)
            {
                List<TariffLine> filteredLines = new List<TariffLine>();
                filteredLines = entry.Value.ToList().Where(p => p.OriginPortId == fromPort && p.DestinationPortId == toPort).ToList();
                if (filteredLines.Count() == 0)
                {
                    filteredLines = entry.Value.ToList().Where(p => p.OriginPortId == fromPort && p.IsToAllOtherPorts == true).ToList();

                    if (filteredLines.Count() == 0)
                    {
                        filteredLines = entry.Value.ToList().Where(p => p.DestinationPortId == toPort && p.IsFromAllOtherPorts == true).ToList();

                        if (filteredLines.Count() == 0)
                        {
                            filteredLines = entry.Value.ToList().Where(p => p.IsToAllOtherPorts == true && p.IsFromAllOtherPorts == true).ToList();
                        }
                    }
                }
                TariffLinesFiltered.Add(entry.Key, filteredLines);
            }

            TariffLines = TariffLinesFiltered;

            Dictionary<string, string> Currencies = commonContext.Currencies.Where(p => p.Tenant == tenant).ToDictionary(p => p.Id, p => p.Code + "," + p.Sign);
            List<TariffVersionAllInCharge> TariffVersionAllInChargesList = tariffRepository.GetAllTariffAllInOnVersionsByTariffIds(items.Select(p => p.tariffid).ToArray(), TariffVersionList.Select(p => p.Version).ToArray(), tenant).ToList();
            List<Tariff> SurchargeTariffList = tariffRepository.GetSurchargeTariffsByCodeAndSellerId(TariffList.Select(p => p.SellerId).ToArray(), tariffType, tenant).Where(p => !p.InActive).ToList();
            List<Measurement> UsedMeasurements = commonContext.Measurements.Where(p => p.Tenant == tenant).ToList();
            List<ChargesType> chargesTypes = commonContext.ChargesTypes.Where(p => p.Tenant == tenant).ToList();

            List<TariffVersion> TariffSurchargeVersionList = tariffRepository.GetAllTariffVersionsByTariffIds(SurchargeTariffList.Select(p => p.Id).ToArray(), tenant).ToList();
            List<int> VersionsSurchargeIds = TariffSurchargeVersionList.Select(a => a.Version).ToList();
            Dictionary<string, List<TariffLine>> SurchargeTariffLines = tariffRepository.GetAllTariffLinesByTariffIds(SurchargeTariffList.Select(p => p.Id).ToArray(), tenant).Where(p => VersionsSurchargeIds.Contains(p.Version)).Where(p => System.Data.Entity.DbFunctions.TruncateTime(p.StartDate) <= System.Data.Entity.DbFunctions.TruncateTime(betweenDate) && p.ExpirationDate != null ? (System.Data.Entity.DbFunctions.TruncateTime(p.ExpirationDate) >= System.Data.Entity.DbFunctions.TruncateTime(betweenDate)) : true).GroupBy(p => p.TariffId).ToDictionary(o => o.Key, o => o.ToList());
            Dictionary<string, List<TariffLine>> SurchargeTariffLinesFiltered = new Dictionary<string, List<TariffLine>>();
            

            foreach (KeyValuePair<string, List<TariffLine>> entry in SurchargeTariffLines)
            {
                List<TariffLine> filteredLines = new List<TariffLine>();
                filteredLines = entry.Value.ToList().Where(p => p.OriginPortId == fromPort && p.DestinationPortId == toPort).ToList();
                if (filteredLines.Count() == 0)
                {
                    filteredLines = entry.Value.ToList().Where(p => p.OriginPortId == fromPort && p.IsToAllOtherPorts == true).ToList();

                    if (filteredLines.Count() == 0)
                    {
                        filteredLines = entry.Value.ToList().Where(p => p.DestinationPortId == toPort && p.IsFromAllOtherPorts == true).ToList();

                        if (filteredLines.Count() == 0)
                        {
                            filteredLines = entry.Value.ToList().Where(p => p.IsToAllOtherPorts == true && p.IsFromAllOtherPorts == true).ToList();
                        }
                    }
                }
                SurchargeTariffLinesFiltered.Add(entry.Key, filteredLines);
            }

            foreach (Tariff result in TariffList)
            {
                List<TariffResult> resultItems = items.Where(x => x.tariffid == result.Id && TariffVersionList.Where(a => a.Version == x.TariffVersion && a.TariffId == result.Id).FirstOrDefault() != null).ToList();
                TariffResult item = resultItems.Where(x => x.Price == resultItems.Min(y => y.Price)).FirstOrDefault();
                if (item != null)
                {
                    decimal? Sum = 0;
                    decimal? Sum_WithoutAllIn = 0;
                    this.tariffsSummary = new TariffSearchSummary() { TariffId = result.Id };
                    tariffsSummary.SurchargesWithoutAllIn = new List<SurchargeSummary>();
                    decimal? minprice = 1;
                    TariffLine SelectedLine = null;

                    if (TariffLines.ContainsKey(result.Id))
                    {
                        List<TariffLine> Temp = TariffLines[result.Id].Where(p => p.Version == item.TariffVersion).ToList();// && (decimal?)(p.GetType().GetProperty("Step"+item.PriceIndex+"Price").GetValue(p))==item.price).FirstOrDefault();
                        if (item.PriceIndex != 0)
                        {
                            SelectedLine = Temp.Where(p => (decimal?)(p.GetType().GetProperty("Step" + (item.PriceIndex) + "Price").GetValue(p)) == item.Price).FirstOrDefault();
                        }
                        else
                        {
                            SelectedLine = Temp.Where(p => (decimal?)(p.GetType().GetProperty("MinPrice").GetValue(p)) == item.Price).FirstOrDefault();
                        }
                    }
                    if (SelectedLine != null)
                    {
                        minprice = SelectedLine.MinPrice;
                        tariffsSummary.LineId = SelectedLine.Id;
                    }

                    this.CalculateTariffActualPrice(item, minprice);
                    this.CalculateTariffMinPrice(item, minprice);
                    tariffsSummary.TransitTime = SelectedLine.TransitTime;
                    tariffsSummary.Price = Math.Round((double)CalculateLocalAmount(item.Price != null ? item.Price.Value : 0, currencyId, result.CurrencyId), 2).ToString("0.00");
                    List<TariffVersionAllInCharge> allinList = TariffVersionAllInChargesList.Where(p => p.TariffId == item.tariffid && p.Version == item.TariffVersion).ToList();
                    if (allinList != null && allinList.Count > 0)
                    {
                        List<string> AllInChargesIds = TariffVersionAllInChargesList.Where(p => p.TariffId == item.tariffid && p.Version == item.TariffVersion).Select(p => p.ChargesTypeId).ToList();
                        List<string> AllInChargesNames = chargesTypes.Where(p => AllInChargesIds.Contains(p.Id)).Select(p => p.EnglishName).ToList();
                        tariffsSummary.AllIn = string.Join(", ", AllInChargesNames);
                        List<string> AllInIds_Charges = chargesTypes.Where(p => AllInChargesIds.Contains(p.Id)).Select(p => p.Id).ToList();
                        tariffsSummary.AllInIds = string.Join(", ", AllInIds_Charges);
                    }
                    this.CurrentSurcharge = SurchargeTariffList.Where(p => p.SellerId == result.SellerId).FirstOrDefault();

                    GetSellerInformation(result.SellerId,tenant);

                    tariffsSummary.SurchargesPrice = "0.00";
                    if (CurrentSurcharge != null)
                    {
                        if (SurchargeTariffLinesFiltered.ContainsKey(CurrentSurcharge.Id))
                        {
                            TariffLine ChargesfilteredLines = SurchargeTariffLinesFiltered[CurrentSurcharge.Id].OrderByDescending(d => d.Version).FirstOrDefault();

                            if (ChargesfilteredLines != null)
                            {
                                List<SurchargeSummary> surchargesList = new List<SurchargeSummary>();
                                for (int i = 1; i <= 10; i++)
                                {
                                    decimal? myQuantity = 0;
                                    string chargeId = (string)CurrentSurcharge.GetType().GetProperty("Surcharge" + i + "Id").GetValue(CurrentSurcharge);
                                    SurchargeSummary SurchargeItem = new SurchargeSummary();

                                    if (TariffVersionAllInChargesList.Where(p => p.TariffId == item.tariffid && p.Version == item.TariffVersion && p.ChargesTypeId == chargeId).FirstOrDefault() == null)
                                    {
                                        SurchargeItem.IsAllIn = false;
                                    }
                                    else
                                    {
                                        SurchargeItem.IsAllIn = true;
                                    }

                                    string measurementId = (string)CurrentSurcharge.GetType().GetProperty("Surcharge" + i + "UOM").GetValue(CurrentSurcharge);
                                    if (!string.IsNullOrEmpty(measurementId))
                                    {
                                        Measurement UsedMesurment = UsedMeasurements.Where(p => p.Id == measurementId).FirstOrDefault();

                                        if (UsedMesurment != null)
                                        {
                                            decimal? valueofSurcharge = (decimal?)ChargesfilteredLines.GetType().GetProperty("Surcharge" + i + "Price").GetValue(ChargesfilteredLines);
                                            decimal? valueofSurchargeMin = (decimal?)ChargesfilteredLines.GetType().GetProperty("Surcharge" + i + "MinPrice").GetValue(ChargesfilteredLines);

                                            ChargesType CurrentCharge = chargesTypes.Where(p => p.Id == chargeId).FirstOrDefault();

                                            string surchargeName = "";
                                            string surchargeCode = "";
                                            string surchargeChargeTypeId = "";
                                            if (CurrentCharge != null)
                                            {
                                                surchargeName = CurrentCharge.EnglishName;
                                                surchargeCode = CurrentCharge.Code;
                                                surchargeChargeTypeId = CurrentCharge.Id;
                                            }

                                            if (valueofSurcharge != null)
                                            {
                                                decimal ? CurrentSurchargePriceCalculation = 0;

                                                SurchargeItem.Code = surchargeCode;
                                                SurchargeItem.Name = surchargeName;
                                                SurchargeItem.ChargeTypeId = surchargeChargeTypeId;
                                                SurchargeItem.UnitOfMesurmentCode = UsedMesurment.Code;
                                                SurchargeItem.UnitOfMesurmentId = UsedMesurment.Id;
                                                switch (UsedMesurment.Code)
                                                {
                                                    case "GRWT": { myQuantity = (decimal?)grossWeight; break; }
                                                    case "CHWT": { myQuantity = (decimal?)weight; break; }
                                                    case "VOLU": { myQuantity = (decimal?)volume; break; }
                                                    case "BTEU": { myQuantity = 1; break; }
                                                    case "FIXD": { myQuantity = 1; break; }
                                                    case "PRVL": { myQuantity = 1; break; }
                                                    case "PRFR": { myQuantity = 1; break; }
                                                    case "GWTN": { myQuantity = (decimal?)this.ComputeGrossWeigh_Kg_Ton(grossWeight, grossWeightCode, "ton"); break; }
                                                    case "CWKG": { myQuantity = (decimal?)this.ComputeChargeableWeight_Kg(weight, weightCode); break; }
                                                    case "GWKG": { myQuantity = (decimal?)this.ComputeGrossWeigh_Kg_Ton(grossWeight, grossWeightCode, "kg"); break; }
                                                    case "QTY": { myQuantity = 1; break; }
                                                    case "VCBM": { myQuantity = (decimal?)ComputeVolumeInCBM(volume, volumeCode); break; }
                                                    default: { break; }
                                                }
                                                if (myQuantity == null)
                                                {
                                                    myQuantity = 1;
                                                }

                                                if (UsedMesurment.Code == "PRVL" || UsedMesurment.Code == "PRFR")
                                                {
                                                    CurrentSurchargePriceCalculation = ((valueofSurcharge * myQuantity * item.Price) / 100);
                                                }
                                                else
                                                {
                                                    CurrentSurchargePriceCalculation = (valueofSurcharge * myQuantity);
                                                }

                                                string tariffLineCurrencyId = ChargesfilteredLines.CurrencyId != null ? ChargesfilteredLines.CurrencyId : CurrentSurcharge.CurrencyId;
                                               
                                                if (ChargesfilteredLines.IsDifferentCurrenciesPerCharge)
                                                {
                                                    tariffLineCurrencyId = (string)ChargesfilteredLines.GetType().GetProperty("Surcharge" + i + "CurrencyId").GetValue(ChargesfilteredLines);
                                                }

                                                var LinePrice = CalculateLocalAmount((CurrentSurchargePriceCalculation == null ? 0 : CurrentSurchargePriceCalculation.Value), currencyId, tariffLineCurrencyId);

                                                decimal? minPriceSurcharge = null;
                                                decimal? actualMinimumPrice = null;
                                                if (valueofSurchargeMin != null)
                                                {
                                                    actualMinimumPrice = (decimal)valueofSurchargeMin;
                                                    minPriceSurcharge = CalculateLocalAmount(actualMinimumPrice.Value, currencyId, tariffLineCurrencyId);
                                                    if (minPriceSurcharge > LinePrice)
                                                    {
                                                        LinePrice = minPriceSurcharge.Value;
                                                        SurchargeItem.IsMinIconVisible = true;
                                                    }
                                                }

                                                SurchargeItem.Price = LinePrice;
                                                SurchargeItem.ActualPrice = CurrentSurchargePriceCalculation == null ? 0 : CurrentSurchargePriceCalculation.Value;
                                                Sum += SurchargeItem.Price;
                                                Sum_WithoutAllIn += SurchargeItem.IsAllIn ? 0 : SurchargeItem.Price;
                                                SurchargeItem.TariffId = CurrentSurcharge.Id;
                                                SurchargeItem.CurrencyId = tariffLineCurrencyId;
                                                SurchargeItem.TariffNumber = CurrentSurcharge.TariffNumber;
                                                SurchargeItem.VersionId = ChargesfilteredLines.Version + "";
                                                SurchargeItem.LineId = ChargesfilteredLines.Id;
                                                SurchargeItem.SellerId = CurrentSurcharge.SellerId;
                                                SurchargeItem.SellerName = tarrifSellerName;
                                                SurchargeItem.MinPrice = minPriceSurcharge;
                                                SurchargeItem.ActualMinPrice = actualMinimumPrice;
                                                SurchargeItem.IsDifferentCurrency = ChargesfilteredLines.IsDifferentCurrenciesPerCharge;
                                                SurchargeItem.CurrencySign = AssignSignCode(Currencies, currencyId, SurchargeItem.UnitOfMesurmentCode);
                                                surchargesList.Add(SurchargeItem);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                tariffsSummary.AllInSurcharges = surchargesList.Where(a => a.IsAllIn).ToList();
                                tariffsSummary.SurchargesWithoutAllIn = surchargesList.Where(a => !a.IsAllIn).ToList();
                                tariffsSummary.SurchargesPrice = tariffsSummary.SurchargesWithoutAllIn.Sum(s => s.Price).ToString();                                
                            }
                        }
                    }

                    tariffsSummary.SellerName = tarrifSellerName;
                    tariffsSummary.UpdateDate = result.UpdateDate;
                    tariffsSummary.LastUsedDate = result.LastUsedDate;
                    tariffsSummary.EffictiveDate = result.ExpirationDate;

                    if (result.StartDate != null)
                    {
                        tariffsSummary.ValidityDate = String.Format("{0:dd/MM/yyyy}", result.StartDate.Value);
                    }

                    if (result.ExpirationDate != null)
                    {
                        if (string.IsNullOrEmpty(tariffsSummary.ValidityDate))
                        {
                            tariffsSummary.ValidityDate = String.Format("{0:dd/MM/yyyy}", result.ExpirationDate.Value);
                        }

                        else
                        {
                            tariffsSummary.ValidityDate = tariffsSummary.ValidityDate + " - " + String.Format("{0:dd/MM/yyyy}", result.ExpirationDate.Value);
                        }
                    }

                    tariffsSummary.Remarks = result.Notes;
                    if(CurrentSurcharge != null)
                    {
                        if (!string.IsNullOrEmpty(tariffsSummary.Remarks) && !string.IsNullOrEmpty(CurrentSurcharge.Notes))
                            tariffsSummary.Remarks = tariffsSummary.Remarks + " , " + CurrentSurcharge.Notes;
                        else if (!string.IsNullOrEmpty(CurrentSurcharge.Notes))
                            tariffsSummary.Remarks = CurrentSurcharge.Notes;
                        else if (!string.IsNullOrEmpty(tariffsSummary.Remarks))
                            tariffsSummary.Remarks = tariffsSummary.Remarks;
                    }   

                    var calculatedLocalAmount = item.Price != null ? CalculateLocalAmount((item.Price).Value, currencyId, result.CurrencyId) : 0;
                    tariffsSummary.decimalprice = (decimal?)Sum + calculatedLocalAmount;
                    tariffsSummary.VersionId = item.TariffVersion + "";
                    tariffsSummary.TariffId = item.tariffid;
                    tariffsSummary.TariffNumber = result.TariffNumber;

                    var airChrageType = chargesTypes.Where(p => p.Id == result.FreightChargeId).Select(p => p).FirstOrDefault();
                    tariffsSummary.ChargeTypeId = airChrageType.Id;
                    tariffsSummary.TotalSurcharge = Sum + "";
                    tariffsSummary.WholePrice = (decimal?)Sum + calculatedLocalAmount + "";
                    tariffsSummary.WholePriceWithoutAllIn = (decimal?)Sum_WithoutAllIn + calculatedLocalAmount + "";
                    tariffsSummary.UnitOfMesurmentId = airChrageType.MeasurementId;
                    tariffsSummary.UnitOfMesurmentCode = UsedMeasurements.Where(p => p.Id == airChrageType.MeasurementId).Select(p => p.Code).FirstOrDefault();
                    tariffsSummary.SellerId = result.SellerId;
                    tariffsSummary.MinPrice = minprice;

                    byte[] filedata = DownloadFile(documentId, "images");
                    string resultImage = "";
                    if (filedata != null)
                    {
                        resultImage = "data:image/" + ImageExtension + ";base64," + Convert.ToBase64String(filedata);
                    }

                    tariffsSummary.ImageId = resultImage;

                    if (!string.IsNullOrEmpty(currencyId))
                    {
                        string code = null;
                        string sign = null;
                        string code_sign = Currencies.Keys.Contains(currencyId) ? Currencies[currencyId] : null;


                        if (!string.IsNullOrEmpty(code_sign))
                        {
                            string[] code_sign_array = code_sign.Split(',');
                            code = code_sign_array[0];

                            if (tariffsSummary.UnitOfMesurmentCode == "PRFR")
                            {
                                sign = "%";
                            }
                            else
                            {
                                if (code_sign_array.Count() > 1)
                                {
                                    sign = code_sign_array[1];
                                }
                            }
                        }


                        tariffsSummary.CurrencyCode = code;
                        tariffsSummary.CurrencySign = sign;
                        tariffsSummary.CurrencyId = result.CurrencyId;
                    }

                    tariffSearchSummaries.Add(tariffsSummary);
                }
            }

            tariffSearchSummaries = tariffSearchSummaries.OrderBy(p => p.decimalprice).ToList();
            return tariffSearchSummaries;
        }

        private void CalculateTariffActualPrice(TariffResult tariff, decimal? minprice)
        {
            decimal? actualPrice = 0;
            if (tariff.PriceIndex != 0)
            {
                actualPrice = tariff.Price * (decimal)weight;
            }
            tariffsSummary.ActualPrice = actualPrice;
        }

        private void CalculateTariffMinPrice(TariffResult tariff, decimal? minprice)
        {
            if (tariff.PriceIndex != 0)
            {
                tariff.Price = tariff.Price * (decimal)weight;
                if (tariff.Price < minprice)
                {
                    tariff.Price = minprice;
                    tariffsSummary.IsMinIconVisible = true;
                }
            }
            else
            {
                tariff.Price = 0;
                if (minprice != null)
                {
                    tariff.Price = minprice;
                    tariffsSummary.IsMinIconVisible = true;
                }
            }
        }

        private string AssignSignCode(Dictionary<string, string> currencies, string currencyId, string uom)
        {
            string code_sign = currencies.Keys.Contains(currencyId) ? currencies[currencyId] : null;
            string sign = "";
            if (!string.IsNullOrEmpty(code_sign))
            {
                string[] code_sign_array = code_sign.Split(',');


                    if (code_sign_array.Count() > 1)
                    {
                        sign = code_sign_array[1];
                    }
             
            }
            return sign;
        }

        private List<TariffSearchSummary> GetTariffSearchSummary_FCL(IQueryable<TariffLine> iQueryable)
        {
            this.Initialization(args, iQueryable);
            this.FillSurchargeTariffLinesFiltered();
            Tariff trariff = new Tariff();
            foreach (TariffWithContainerNumber trariffwithnumber in tariffWithContainerNumbers)
            {
                trariff = trariffwithnumber.Tariff;
                List<TariffLine> resultItems = tariffLinesList.Where(x => x.TariffId == trariff.Id && tariffVersionList.Where(a => a.Version == x.Version && a.TariffId == trariff.Id).FirstOrDefault() != null).ToList();
                foreach (TariffLine tariffLine in resultItems)
                {
                    this.Sum = 0;
                    this.Sum_WithoutAllIn = 0;
                    this.tariffsSummary = new TariffSearchSummary() { TariffId = trariff.Id };
                    tariffsSummary.ContainerNumber = trariffwithnumber.ContainerNumber;
                    tariffsSummary.NoteMissingContainers = trariffwithnumber.NoteMissingContainers;
                    tariffsSummary.SurchargesWithoutAllIn = new List<SurchargeSummary>();
                    tariffsSummary.ContainersPrices = new List<ContainersPrice>();

                    var price = this.CalculateContainerPrice(trariff, tariffLine);

                    tariffsSummary.Price = Math.Round((double)CalculateLocalAmount(price, currencyId, trariff.CurrencyId, tenant), 2).ToString("0.00");
                    tariffsSummary.ActualPrice = price;
                    tariffsSummary.LineId = tariffLine.Id;

                    GetSellerInformation(trariff.SellerId, tenant);

                    this.FillAllInList(tariffLine);
                    this.FillSurchargeData(args, trariff, tariffLine);

                    tariffsSummary.SellerName = tarrifSellerName;
                    tariffsSummary.EffictiveDate = trariff.ExpirationDate;

                    if (trariff.StartDate != null)
                    {
                        tariffsSummary.ValidityDate = String.Format("{0:dd/MM/yyyy}", trariff.StartDate.Value);
                    }

                    if (trariff.ExpirationDate != null)
                    {
                        if (string.IsNullOrEmpty(tariffsSummary.ValidityDate))
                        {
                            tariffsSummary.ValidityDate = String.Format("{0:dd/MM/yyyy}", trariff.ExpirationDate.Value);
                        }

                        else
                        {
                            tariffsSummary.ValidityDate = tariffsSummary.ValidityDate + " - " + String.Format("{0:dd/MM/yyyy}", trariff.ExpirationDate.Value);
                        }
                    }
                    tariffsSummary.Remarks = trariff.Notes;
                    if (CurrentSurcharge != null)
                    {
                        if (!string.IsNullOrEmpty(tariffsSummary.Remarks) && !string.IsNullOrEmpty(CurrentSurcharge.Notes))
                            tariffsSummary.Remarks = tariffsSummary.Remarks + " , " + CurrentSurcharge.Notes;
                        else if (!string.IsNullOrEmpty(CurrentSurcharge.Notes))
                            tariffsSummary.Remarks = CurrentSurcharge.Notes;
                        else if (!string.IsNullOrEmpty(tariffsSummary.Remarks))
                            tariffsSummary.Remarks = tariffsSummary.Remarks;
                    }
                    var calculatedLocalAmount = CalculateLocalAmount(price, currencyId, trariff.CurrencyId, tenant);
                    tariffsSummary.decimalprice = (decimal?)Sum + calculatedLocalAmount;
                    tariffsSummary.VersionId = tariffLine.Version + "";
                    tariffsSummary.TariffId = tariffLine.TariffId;
                    tariffsSummary.TariffNumber = trariff.TariffNumber;
                    tariffsSummary.TransitTime = tariffLine.TransitTime;
                    var airChrageType = chargesTypes.Where(p => p.Id == trariff.FreightChargeId).Select(p => p).FirstOrDefault();
                    tariffsSummary.ChargeTypeId = airChrageType.Id;
                    tariffsSummary.TotalSurcharge = Sum + "";
                    tariffsSummary.WholePrice = (decimal?)Sum + calculatedLocalAmount + "";
                    tariffsSummary.WholePriceWithoutAllIn = (decimal?)Sum_WithoutAllIn + calculatedLocalAmount + "";
                    tariffsSummary.UnitOfMesurmentId = airChrageType.MeasurementId;
                    tariffsSummary.UnitOfMesurmentCode = usedMeasurements.Where(p => p.Id == airChrageType.MeasurementId).Select(p => p.Code).FirstOrDefault();
                    tariffsSummary.SellerId = trariff.SellerId;
                    byte[] filedata = this.DownloadFile(documentId, "images");
                    string resultImage = "";
                    if (filedata != null)
                    {
                        resultImage = "data:image/" + ImageExtension + ";base64," + Convert.ToBase64String(filedata);
                    }

                    tariffsSummary.ImageId = resultImage;

                    if (!string.IsNullOrEmpty(currencyId))
                    {
                        string code = null;
                        string sign = null;
                        string code_sign = currencies.Keys.Contains(currencyId) ? currencies[currencyId] : null;
                        if (!string.IsNullOrEmpty(code_sign))
                        {
                            string[] code_sign_array = code_sign.Split(',');
                            code = code_sign_array[0];
                            if (tariffsSummary.UnitOfMesurmentCode == "PRFR")
                            {
                                sign = "%";
                            }
                            else
                            {
                                if (code_sign_array.Count() > 1)
                                {
                                    sign = code_sign_array[1];
                                }
                            }
                        }

                        tariffsSummary.CurrencyCode = code;
                        tariffsSummary.CurrencySign = sign;                        
                        tariffsSummary.CurrencyId = trariff.CurrencyId;
                    }

                    tariffSearchSummaries.Add(tariffsSummary);
                }
            }
            tariffSearchSummaries = tariffSearchSummaries.GroupBy(e => e.ContainerNumber)
                                                   .OrderByDescending(e => e.Key)
                                                   .Select(group => new { Item = group.OrderBy(o => o.decimalprice).ToList() }).SelectMany(list => list.Item)
                                                   .ToList();                                                            
            return tariffSearchSummaries;
        }

        private void Initialization(TariffSearchArgs args, IQueryable<TariffLine> iQueryable)
        {
            this.tariffSearchSummaries = new List<TariffSearchSummary>();
            this.tariffLines_IQueryable = iQueryable.Where(p => p.OriginPortId ==  fromPort && p.DestinationPortId == toPort && System.Data.Entity.DbFunctions.TruncateTime(p.StartDate) <= System.Data.Entity.DbFunctions.TruncateTime(betweenDate) && (p.ExpirationDate != null ? (System.Data.Entity.DbFunctions.TruncateTime(p.ExpirationDate) >= System.Data.Entity.DbFunctions.TruncateTime(betweenDate)) : true));
            this.tariffids = tariffLines_IQueryable.Select(p => p.TariffId).Distinct().ToList();

             this.tariffLinesContainersPrices = (from d in this.tariffContext.TariffLinesContainersPrices
                                                where d.Tenant == tenant
                                                select d).ToList();

            List<Tariff> TariffListTemp = this.tariffRepository.GetAllTariff(tariffids.ToArray(), tenant).Where(p => !p.InActive && p.TypeCode ==  tariffType).ToList();

            if (TariffListTemp.Count > 0)
            {
                if (string.IsNullOrEmpty(currencyId))
                {
                    currencyId = TariffListTemp.FirstOrDefault().CurrencyId;
                }
            }

            this.tariffList = FilterTariffsByContainers(TariffListTemp);

            List<string> tempTariffIds = tariffList.Select(a => a.Id).ToList();
            this.tariffLinesList = tariffLines_IQueryable.Where(p => tempTariffIds.Contains(p.TariffId)).ToList();

            this.tariffVersionList = this.tariffRepository.GetAllTariffVersionsByTariffIds(tariffids.ToArray(), tenant).ToList();
            this.GetRates();

            this.currencies = commonContext.Currencies.Where(p => p.Tenant == tenant).ToDictionary(p => p.Id, p => p.Code + "," + p.Sign);

            this.tariffVersionAllInChargesList = this.tariffRepository.GetAllTariffAllInOnVersionsByTariffIds(tariffids.ToArray(), tariffVersionList.Select(p => p.Version).ToArray(), tenant).ToList();
            this.surchargeTariffList = this.tariffRepository.GetSurchargeTariffsByCodeAndSellerId(tariffList.Select(p => p.SellerId).ToArray(),  tariffType, tenant).Where(p => !p.InActive).ToList();
            this.surchargeTariffList = FilterSurchargeTariffsByContainers(surchargeTariffList);

            this.usedMeasurements = commonContext.Measurements.Where(p => p.Tenant == tenant).ToList();
            this.chargesTypes = commonContext.ChargesTypes.Where(p => p.Tenant == tenant).ToList();
            List<TariffVersion> TariffSurchargeVersionList = this.tariffRepository.GetAllTariffVersionsByTariffIds(surchargeTariffList.Select(p => p.Id).ToArray(), tenant).ToList();
            List<int> VersionsSurchargeIds = TariffSurchargeVersionList.Select(a => a.Version).ToList();
            this.surchargeTariffLines = this.tariffRepository.GetAllTariffLinesByTariffIds(surchargeTariffList.Select(p => p.Id).ToArray(), tenant).Where(p => VersionsSurchargeIds.Contains(p.Version)).Where(p => System.Data.Entity.DbFunctions.TruncateTime(p.StartDate) <= System.Data.Entity.DbFunctions.TruncateTime(betweenDate) && p.ExpirationDate != null ? (System.Data.Entity.DbFunctions.TruncateTime(p.ExpirationDate) >= System.Data.Entity.DbFunctions.TruncateTime(betweenDate)) : true).GroupBy(p => p.TariffId).ToDictionary(o => o.Key, o => o.ToList());
            this.surchargeTariffLinesFiltered = new Dictionary<string, List<TariffLine>>();
        }
        private void FillSurchargeData(TariffSearchArgs args, Tariff trariff, TariffLine tariffLine)
        {
            this.CurrentSurcharge = surchargeTariffList.Where(p => p.SellerId == trariff.SellerId).FirstOrDefault();
            tariffsSummary.SurchargesPrice = "0.00";
            if (CurrentSurcharge != null)
            {
                if (surchargeTariffLinesFiltered.ContainsKey(CurrentSurcharge.Id))
                {
                    TariffLine ChargesfilteredLines = surchargeTariffLinesFiltered[CurrentSurcharge.Id].OrderByDescending(d => d.Version).FirstOrDefault();

                    if (ChargesfilteredLines != null)
                    {
                        List<SurchargeSummary> surchargesList = new List<SurchargeSummary>();
                        for (int i = 1; i <= 10; i++)
                        {
                            string chargeId = (string)CurrentSurcharge.GetType().GetProperty("Surcharge" + i + "Id").GetValue(CurrentSurcharge);
                            SurchargeItem = new SurchargeSummary();
                          
                            if (tariffVersionAllInChargesList.Where(p => p.TariffId == tariffLine.TariffId && p.Version == tariffLine.Version && p.ChargesTypeId == chargeId).FirstOrDefault() == null)
                            {
                                SurchargeItem.IsAllIn = false;
                            }
                            else
                            {
                                SurchargeItem.IsAllIn = true;
                            }

                            string measurementId = (string)CurrentSurcharge.GetType().GetProperty("Surcharge" + i + "UOM").GetValue(CurrentSurcharge);
                            if (!string.IsNullOrEmpty(measurementId))
                            {
                                Measurement UsedMesurment = usedMeasurements.Where(p => p.Id == measurementId).FirstOrDefault();
                                if (UsedMesurment != null)
                                {
                                    var currentTariffLinesContainersPrice = tariffLinesContainersPrices.Where(a => a.TariffId == CurrentSurcharge.Id && a.SurchargeId == chargeId && a.TariffLineId == ChargesfilteredLines.Id).FirstOrDefault();
                                    ChargesType CurrentCharge = chargesTypes.Where(p => p.Id == chargeId).FirstOrDefault();

                                    string surchargeName = "";
                                    string surchargeCode = "";
                                    string surchargeChargeTypeId = "";
                                    if (CurrentCharge != null)
                                    {
                                        surchargeName = CurrentCharge.EnglishName;
                                        surchargeCode = CurrentCharge.Code;
                                        surchargeChargeTypeId = CurrentCharge.Id;
                                    }

                                    if (currentTariffLinesContainersPrice != null)
                                    {
                                        SurchargeItem.ContainersPrices = new List<ContainersPrice>();

                                        decimal currentSurchargePriceCalculation = this.CalculateContainerPriceFromTariffLinesContainers(CurrentSurcharge, currentTariffLinesContainersPrice, UsedMesurment.Code);

                                        SurchargeItem.Code = surchargeCode;
                                        SurchargeItem.Name = surchargeName;
                                        SurchargeItem.ChargeTypeId = surchargeChargeTypeId;
                                        SurchargeItem.UnitOfMesurmentCode = UsedMesurment.Code;
                                        SurchargeItem.UnitOfMesurmentId = UsedMesurment.Id;

                                        string tariffLineCurrencyId = ChargesfilteredLines.CurrencyId != null ? ChargesfilteredLines.CurrencyId : CurrentSurcharge.CurrencyId;
                                        if (ChargesfilteredLines.IsDifferentCurrenciesPerCharge)
                                        {
                                            tariffLineCurrencyId = currentTariffLinesContainersPrice.CurrencyId;
                                        }

                                        var LinePrice = CalculateLocalAmount(currentSurchargePriceCalculation, currencyId, tariffLineCurrencyId, tenant);

                                        SurchargeItem.Price = LinePrice;
                                        SurchargeItem.ActualPrice = currentSurchargePriceCalculation;
                                        Sum += SurchargeItem.Price;
                                        Sum_WithoutAllIn += SurchargeItem.IsAllIn ? 0 : SurchargeItem.Price;
                                        SurchargeItem.TariffId = CurrentSurcharge.Id;
                                        SurchargeItem.CurrencyId = tariffLineCurrencyId;
                                        SurchargeItem.TariffNumber = CurrentSurcharge.TariffNumber;
                                        SurchargeItem.VersionId = ChargesfilteredLines.Version + "";
                                        SurchargeItem.SellerId = CurrentSurcharge.SellerId;
                                        SurchargeItem.SellerName = tarrifSellerName;
                                        SurchargeItem.LineId = ChargesfilteredLines.Id;
                                        SurchargeItem.CurrencySign = AssignSignCode(currencies, currencyId, SurchargeItem.UnitOfMesurmentCode);
                                        surchargesList.Add(SurchargeItem);
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        tariffsSummary.AllInSurcharges = surchargesList.Where(a => a.IsAllIn).ToList();
                        tariffsSummary.SurchargesWithoutAllIn = surchargesList.Where(a => !a.IsAllIn).ToList();
                        tariffsSummary.SurchargesPrice = tariffsSummary.SurchargesWithoutAllIn.Sum(s => s.Price).ToString();      
                    }
                }
            }
        }
        private void FillAllInList(TariffLine item)
        {
            List<TariffVersionAllInCharge> allinList = tariffVersionAllInChargesList.Where(p => p.TariffId == item.TariffId && p.Version == item.Version).ToList();
            if (allinList != null && allinList.Count > 0)
            {
                List<string> AllInChargesIds = tariffVersionAllInChargesList.Where(p => p.TariffId == item.TariffId && p.Version == item.Version).Select(p => p.ChargesTypeId).ToList();
                List<string> AllInChargesNames = chargesTypes.Where(p => AllInChargesIds.Contains(p.Id)).Select(p => p.EnglishName).ToList();
                this.tariffsSummary.AllIn = string.Join(", ", AllInChargesNames);
            }
        }
        private void FillSurchargeTariffLinesFiltered()
        {
            foreach (KeyValuePair<string, List<TariffLine>> entry in surchargeTariffLines)
            {
                List<TariffLine> filteredLines = new List<TariffLine>();
                filteredLines = entry.Value.ToList().Where(p => p.OriginPortId ==  fromPort && p.DestinationPortId == toPort).ToList();
                if (filteredLines.Count() == 0)
                {
                    filteredLines = entry.Value.ToList().Where(p => p.OriginPortId ==  fromPort && p.IsToAllOtherPorts == true).ToList();

                    if (filteredLines.Count() == 0)
                    {
                        filteredLines = entry.Value.ToList().Where(p => p.DestinationPortId == toPort && p.IsFromAllOtherPorts == true).ToList();

                        if (filteredLines.Count() == 0)
                        {
                            filteredLines = entry.Value.ToList().Where(p => p.IsToAllOtherPorts == true && p.IsFromAllOtherPorts == true).ToList();
                        }
                    }
                }
                this.surchargeTariffLinesFiltered.Add(entry.Key, filteredLines);
            }
        }
        private List<Tariff> FilterTariffsByContainers(List<Tariff> tariffList)
        {
            Dictionary<string, string> filterContainer = new Dictionary<string, string>();
            if (containerType1Id != null) filterContainer.Add(containerType1Id, containerType1);
            if (containerType2Id != null) filterContainer.Add(containerType2Id, containerType2);
            if (containerType3Id != null) filterContainer.Add(containerType3Id, containerType3);
            if (containerType4Id != null) filterContainer.Add(containerType4Id, containerType4);
            if (containerType5Id != null) filterContainer.Add(containerType5Id, containerType5);
            this.tariffWithContainerNumbers = new List<TariffWithContainerNumber>();
            foreach (var tariff in tariffList)
            {
                var tariffContainers = new List<string>(new string[] { tariff.ContainerType1Id, tariff.ContainerType2Id, tariff.ContainerType3Id, tariff.ContainerType4Id, tariff.ContainerType5Id });
                var count = 0;
                var noteMissingContainers = "Price Doesn't Include: ";
                bool missingExist = false;
                foreach (KeyValuePair<string, string> keyWithValue in filterContainer)
                {
                    if (!tariffContainers.Contains(keyWithValue.Key))
                    {
                        noteMissingContainers += keyWithValue.Value + ", ";
                        missingExist = true;
                    }
                    else
                        count++;
                }
                if (!missingExist) 
                    noteMissingContainers = null;
                else
                    noteMissingContainers = noteMissingContainers.Remove(noteMissingContainers.Length - 2);

                if (count > 0)
                    tariffWithContainerNumbers.Add(new TariffWithContainerNumber { Tariff = tariff, ContainerNumber = count, NoteMissingContainers= noteMissingContainers });
            }
            tariffList = tariffWithContainerNumbers.Select(e => e.Tariff).ToList();
            return tariffList;
        }
        private List<Tariff> FilterSurchargeTariffsByContainers(List<Tariff> tariffList)
        {
            List<string> filterContainer = new List<string>(new string[] { containerType1Id, containerType2Id, containerType3Id, containerType4Id, containerType5Id });
            filterContainer = filterContainer.Where(e => e != null).ToList();
            tariffList = tariffList.Where(a => filterContainer.Contains(a.ContainerType1Id) || filterContainer.Contains(a.ContainerType2Id)
              || filterContainer.Contains(a.ContainerType3Id) || filterContainer.Contains(a.ContainerType4Id) || filterContainer.Contains(a.ContainerType5Id)).ToList();

            return tariffList;
        }
        private decimal CalculateContainerPrice(Tariff trariff, TariffLine tariffLine)
        {
            decimal? price1 = null; decimal? price_WithoutQuantity = null; decimal? price2 = null; decimal? price3 = null; decimal? price4 = null; decimal? price5 = null;
            int containerQuantity = 0;
            if (trariff.ContainerType1Id != null)
            {
                containerQuantity = trariff.ContainerType1Id == containerType1Id ? quantity1 : (trariff.ContainerType1Id == containerType2Id ? quantity2 : (trariff.ContainerType1Id == containerType3Id ? quantity3 : (trariff.ContainerType1Id == containerType4Id ? quantity4 : (trariff.ContainerType1Id == containerType5Id ? quantity5 : 0))));
                price_WithoutQuantity = (tariffLine.Surcharge1Price != null ? tariffLine.Surcharge1Price : 0);
                price1 = (price_WithoutQuantity * containerQuantity);
                this.tariffsSummary.ContainersPrices.Add(new ContainersPrice
                {
                    TariffId = trariff.Id,
                    ContainerId = trariff.ContainerType1Id,
                    Price = price1,
                    Quantity = containerQuantity,
                    Price_WithoutQuantity = price_WithoutQuantity
                });
            }
            if (trariff.ContainerType2Id != null)
            {
                containerQuantity = trariff.ContainerType2Id == containerType1Id ? quantity1 : (trariff.ContainerType2Id == containerType2Id ? quantity2 : (trariff.ContainerType2Id == containerType3Id ? quantity3 : (trariff.ContainerType2Id == containerType4Id ? quantity4 : (trariff.ContainerType2Id == containerType5Id ? quantity5 : 0))));
                price_WithoutQuantity = (tariffLine.Surcharge2Price != null ? tariffLine.Surcharge2Price : 0);
                price2 = (price_WithoutQuantity * containerQuantity);
                this.tariffsSummary.ContainersPrices.Add(new ContainersPrice
                {
                    TariffId = trariff.Id,
                    ContainerId = trariff.ContainerType2Id,
                    Price = price2,
                    Quantity = containerQuantity,
                    Price_WithoutQuantity = price_WithoutQuantity
                });
            }
            if (trariff.ContainerType3Id != null)
            {
                containerQuantity = trariff.ContainerType3Id == containerType1Id ? quantity1 : (trariff.ContainerType3Id == containerType2Id ? quantity2 : (trariff.ContainerType3Id == containerType3Id ? quantity3 : (trariff.ContainerType3Id == containerType4Id ? quantity4 : (trariff.ContainerType3Id == containerType5Id ? quantity5 : 0))));
                price_WithoutQuantity = (tariffLine.Surcharge3Price != null ? tariffLine.Surcharge3Price : 0);
                price3 = (price_WithoutQuantity * containerQuantity);
                this.tariffsSummary.ContainersPrices.Add(new ContainersPrice
                {
                    TariffId = trariff.Id,
                    ContainerId = trariff.ContainerType3Id,
                    Price = price3,
                    Quantity = containerQuantity,
                    Price_WithoutQuantity = price_WithoutQuantity
                });
            }
            if (trariff.ContainerType4Id != null)
            {
                containerQuantity = trariff.ContainerType4Id == containerType1Id ? quantity1 : (trariff.ContainerType4Id == containerType2Id ? quantity2 : (trariff.ContainerType4Id == containerType3Id ? quantity3 : (trariff.ContainerType4Id == containerType4Id ? quantity4 : (trariff.ContainerType4Id == containerType5Id ? quantity5 : 0))));
                price_WithoutQuantity = (tariffLine.Surcharge4Price != null ? tariffLine.Surcharge4Price : 0);
                price4 = (price_WithoutQuantity * containerQuantity);
                this.tariffsSummary.ContainersPrices.Add(new ContainersPrice
                {
                    TariffId = trariff.Id,
                    ContainerId = trariff.ContainerType4Id,
                    Price = price4,
                    Quantity = containerQuantity,
                    Price_WithoutQuantity = price_WithoutQuantity
                });
            }
            if (trariff.ContainerType5Id != null)
            {
                containerQuantity = trariff.ContainerType5Id == containerType1Id ? quantity1 : (trariff.ContainerType5Id == containerType2Id ? quantity2 : (trariff.ContainerType5Id == containerType3Id ? quantity3 : (trariff.ContainerType5Id == containerType4Id ? quantity4 : (trariff.ContainerType5Id == containerType5Id ? quantity5 : 0))));
                price_WithoutQuantity = (tariffLine.Surcharge5Price != null ? tariffLine.Surcharge5Price : 0);
                price5 = (price_WithoutQuantity * containerQuantity);
                this.tariffsSummary.ContainersPrices.Add(new ContainersPrice
                {
                    TariffId = trariff.Id,
                    ContainerId = trariff.ContainerType5Id,
                    Price = price5,
                    Quantity = containerQuantity,
                    Price_WithoutQuantity = price_WithoutQuantity
                });
            }

            var price = ((price1 != null) ? price1.Value : 0) +
                       ((price2 != null) ? price2.Value : 0) +
                       ((price3 != null) ? price3.Value : 0) +
                       ((price4 != null) ? price4.Value : 0) +
                       ((price5 != null) ? price5.Value : 0);

            return price;
        }
        private decimal CalculateContainerPriceFromTariffLinesContainers(Tariff trariff, TariffLinesContainersPrice tariffLinesContainersPrice, string measurementCode)
        {
            decimal? price_WithoutQuantity = null;
            decimal totalPrice = 0;
            int containerQuantity = 0;
            decimal? price = null;
            decimal? teuPrice = 0;

            for (int i = 1; i <= 5; i++)
            {
                PropertyInfo tariffLinesContainersPricePropInfo = tariffLinesContainersPrice.GetType().GetProperty("Price" + i);
                var tariffLinesContainersPricevalue = (decimal?)tariffLinesContainersPricePropInfo.GetValue(tariffLinesContainersPrice);

                PropertyInfo tariffContainerPropInfo = trariff.GetType().GetProperty("ContainerType" + i + "Id");
                var tariffContainerPropValue = (string)tariffContainerPropInfo.GetValue(trariff);

                if (tariffContainerPropValue != null)
                {
                    containerQuantity = tariffContainerPropValue == containerType1Id ? quantity1 : (tariffContainerPropValue == containerType2Id ? quantity2 : (tariffContainerPropValue == containerType3Id ? quantity3 : (tariffContainerPropValue == containerType4Id ? quantity4 : (tariffContainerPropValue == containerType5Id ? quantity5 : 0))));
                    if (measurementCode == "FIXD")
                    {
                        price = (tariffLinesContainersPrice.CostPrice != null ? tariffLinesContainersPrice.CostPrice : 0);
                        price_WithoutQuantity = price;
                    }
                    else if (measurementCode == "BTEU")
                    {
                        var teu = GetMeasurement(tariffContainerPropValue, trariff.Tenant);
                        price_WithoutQuantity = (tariffLinesContainersPrice.CostPrice != null ? tariffLinesContainersPrice.CostPrice : 0);
                        teuPrice = teuPrice + (containerQuantity * teu);
                    }
                    else
                    {
                        price_WithoutQuantity = (tariffLinesContainersPricevalue != null ? tariffLinesContainersPricevalue : 0);
                        price = (price_WithoutQuantity * containerQuantity);
                        totalPrice = totalPrice + ((price != null) ? price.Value : 0);
                    }

                    this.SurchargeItem.ContainersPrices.Add(new ContainersPrice
                    {
                        TariffId = trariff.Id,
                        ContainerId = tariffContainerPropValue,
                        Price = price,
                        Quantity = containerQuantity,
                        Price_WithoutQuantity = price_WithoutQuantity
                    });
                }
            }

            if (measurementCode == "BTEU")
            {
                totalPrice = (teuPrice != null ? teuPrice.Value : 0) * (tariffLinesContainersPrice.CostPrice != null ? tariffLinesContainersPrice.CostPrice.Value : 0);
            }
            else if (measurementCode == "FIXD")
            {
                totalPrice = price != null ? price.Value : 0;
            }

            return totalPrice;
        }
        private decimal GetMeasurement(string id, int tenant)
        {
            PackageTypeQuery packageTypeQuery = new PackageTypeQuery(tenant);
            var packageType = packageTypeQuery.GetSinglePM(id, tenant);
            return (decimal) (packageType != null ? packageType.TEU : 0);
        }
        private decimal CalculateLocalAmount(decimal amount, string convertedCurrencyId, string currencyId, int tenant)
        {
            var tenantCurrency = GetTenantCurrency(tenant);
            decimal amountInTariffCurr, amountInConvertedCurr;

            if (convertedCurrencyId == currencyId)
            {
                amountInTariffCurr = amount;
            }

            else
            {
                if (tenantCurrency == currencyId)
                    amountInTariffCurr = amount;
                else
                {
                    RatesTableList rateList = RatesList.Find(d => d.BaseCurrencyId == tenantCurrency && d.ForeignCurrencyId == currencyId);
                    var rate = rateList == null ? 0 : rateList.Rate;
                    amountInTariffCurr = amount * (decimal)rate;

                }

                if (tenantCurrency == convertedCurrencyId)
                    amountInConvertedCurr = amountInTariffCurr;

                else
                {
                    RatesTableList rateList = RatesList.Find(d => d.BaseCurrencyId == tenantCurrency && d.ForeignCurrencyId == convertedCurrencyId);
                    var rate = rateList == null ? 0 : rateList.Rate;
                    amountInTariffCurr = amountInTariffCurr / (decimal)rate;
                }
            }

            return amountInTariffCurr;
        }
        private string GetTenantCurrency(int tenant)
        {
            TenantRepository tRepo = new TenantRepository(tenant);
            Tenant t = tRepo.GetSingleByTenant(tenant);
            var tenantCurrency = (t == null ? null : t.CurrencyId);
            return tenantCurrency;
        }
        private List<RatesTableList> RatesList;
        private void GetRates()
        {
            IWebFreightContext MyContext = WebFreightContext.GetContext(tenant);
            RatesTableRepository ratesTableRepository = new RatesTableRepository(MyContext);
            IQueryable<RatesTable> entityPocos = ratesTableRepository.GetRatesTables(tenant);

            RatesTableQuery ratesTableQuery = new RatesTableQuery(ratesTableRepository);
            IQueryable<RatesTableList> entityLists = ratesTableQuery.GetIQueryableEntityList(entityPocos);
            entityLists = entityLists.OrderByDescending(r => r.ValueDate);

            this.RatesList = entityLists.ToList();
        }
        private decimal CalculateLocalAmount(decimal amount, string convertedCurrencyId, string tariffLineCurrencyId)
        {
            var tenantCurrency = GetTenantCurrency();
            decimal amountInTariffCurr, amountInConvertedCurr;

            if (convertedCurrencyId == tariffLineCurrencyId)
            {
                amountInTariffCurr = amount;
            }

            else
            {
                if (tenantCurrency == tariffLineCurrencyId)
                    amountInTariffCurr = amount;
                else
                {
                    RatesTableList rateList = RatesList.Find(d => d.BaseCurrencyId == tenantCurrency && d.ForeignCurrencyId == tariffLineCurrencyId);
                    var rate = rateList == null ? 0 : rateList.Rate;
                    amountInTariffCurr = amount * (decimal)rate;

                }

                if (tenantCurrency == convertedCurrencyId)
                    amountInConvertedCurr = amountInTariffCurr;

                else
                {
                    RatesTableList rateList = RatesList.Find(d => d.BaseCurrencyId == tenantCurrency && d.ForeignCurrencyId == convertedCurrencyId);
                    var rate = rateList == null ? 0 : rateList.Rate;
                    amountInTariffCurr = amountInTariffCurr / (decimal)rate;
                }
            }

            return amountInTariffCurr;
        }
        private string GetTenantCurrency()
        {
            TenantRepository tRepo = new TenantRepository(tenant);
            Tenant t = tRepo.GetSingleByTenant(tenant);
            var tenantCurrency = (t == null ? null : t.CurrencyId);
            return tenantCurrency;
        }
        private double? ComputeVolumeInCBM(double? Volume, string VolumeCode)
        {
            double? volumeInCBM = null;

            if (Volume != null)
            {
                double factorOfConvert = 1;

                if (!String.IsNullOrEmpty(VolumeCode))
                {
                    switch (VolumeCode.ToUpper())
                    {
                        case "CBM": { factorOfConvert = 1; break; }
                        case "CBI": { factorOfConvert = 61024; break; }
                        case "CBF": { factorOfConvert = 35.315; break; }
                    }
                }
                volumeInCBM = Volume * factorOfConvert;
            }

            if (volumeInCBM != null)
            {
                volumeInCBM = Round(volumeInCBM, 3);
            }
            return volumeInCBM;
        }
        private double? ComputeGrossWeigh_Kg_Ton(double? GrossWeight, string GrossWeightUnitCode, string type)
        {
            double? weigh_Kg = null;
            double? weigh_Ton = null;

            if (GrossWeight != null)
            {
                double factorOfConvert = 1;

                if (!string.IsNullOrEmpty(GrossWeightUnitCode))
                {
                    switch (GrossWeightUnitCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }
                        case "MT": { factorOfConvert = 1000; break; }
                    }
                }

                weigh_Kg = GrossWeight * factorOfConvert;
            }

            if (weigh_Kg != null)
            {
                weigh_Kg = Round(weigh_Kg, 3);

                weigh_Ton = weigh_Kg / 1000;
            }

            if (weigh_Ton != null)
            {
                weigh_Ton = Round(weigh_Ton, 3);
            }

            if (type == "kg")
                return weigh_Kg;
            return weigh_Ton;
        }
        private double? ComputeChargeableWeight_Kg(double? ChargeableWeight, string ChargeableWeightUnitCode)
        {
            double? weigh_Kg = null;

            if (ChargeableWeight != null)
            {
                double factorOfConvert = 1;

                if (!String.IsNullOrEmpty(ChargeableWeightUnitCode))
                {
                    switch (ChargeableWeightUnitCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }
                        case "MT": { factorOfConvert = 1000; break; }
                    }
                }

                weigh_Kg = ChargeableWeight * factorOfConvert;
            }

            if (weigh_Kg != null)
            {
                weigh_Kg = Round(weigh_Kg, 3);
            }
            return weigh_Kg;
        }
        public double? Round(double? value, int digits)
        {
            double? myValue = null;

            if (value != null)
            {
                myValue = Convert.ToDouble(value);
            }

            double? myResult = myValue;

            if (myValue != null && digits >= 1 && digits <= 15)
            {
                string mySTR = String.Format("{0:N" + digits + "}", myValue);

                myResult = Convert.ToDouble(mySTR);
            }

            return myResult;
        }

        private string ImageExtension = "";
        private byte[] DownloadFile(string documentId, string fileLocation)
        {
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            ImageExtension = "jpg";
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = documentId,
                FolderName = fileLocation,
                Extension = ImageExtension,
                Tenant = tenant,
            };

            byte[] datainByte = storageservice.Read(fileInfo);

            if(datainByte == null)
            {
                ImageExtension = "png";
                fileInfo = new BlobFileInfo()
                {
                    FileName = documentId,
                    FolderName = fileLocation,
                    Extension = ImageExtension,
                    Tenant = tenant,
                };
                datainByte = storageservice.Read(fileInfo);
            }
            return datainByte;
        }
        private void GetSellerInformation(string id, int tenant)
        {
            Card seller = cardQuery.GetSingleCard(id, tenant);
            if (seller != null)
            {
                tarrifSellerName = seller.EnglishName;
                documentId = seller.ImageDetailId;
            }
        }
    }

    public class TariffResult
    {
        public string tariffid { get; set; }
        public string TariffNumber { get; set; }
        public int TariffVersion { get; set; }
        public decimal? Price { get; set; }
        public int PriceIndex { get; set; }
    }

    public class ByPckageType
    {
        public int? Quantity { get; set; }
        public string PackageTypeId { get; set; }
        public string MeasurementId { get; set; }
        public string MeasurementCode { get; set; }
        public string MeasurementShortName { get; set; }
    }
    public class TariffWithContainerNumber
    {
        public Tariff Tariff { get; set; }
        public int ContainerNumber { get; set; }
        public string NoteMissingContainers { get; set; }
    }
}
