using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Logitude.TariffModule.BL.DataContracts;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.Data;
using Logitude.TariffModule.Data.EntityKeys;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.Repositories;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace Logitude.TariffModule.BL.EntityQueryServices
{
    public partial class TariffQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, TariffPM entityPM)
        {
            ITariffModuleContext context = MainContext as ITariffModuleContext;
            TariffKeys tariffKeys = entityKeys as TariffKeys;

            TariffVersionQueryService tariffVersionQueryService = new TariffVersionQueryService(context);
            entityPM.TariffVersions = tariffVersionQueryService.GetDraftVersion(tariffKeys, true);
            entityPM.ActiveVersions = tariffVersionQueryService.GetActiveVersions(entityPM.Id, entityPM.Tenant, entityPM.TypeCode);
        }

        public TariffsSummary GetCount(int tenant)
        {
            TariffsSummary tariffsSummary = new TariffsSummary() { Id = tenant };
            tariffsSummary.AirFreightCount = this.repository.GetAll(tenant).Where(p => p.TypeCode == "AFC").Count();
            tariffsSummary.AirSurchargeCount = this.repository.GetAll(tenant).Where(p => p.TypeCode == "ASC").Count();
            tariffsSummary.OceanSurchargeCount = this.repository.GetAll(tenant).Where(p => p.TypeCode == "OSC").Count();
            tariffsSummary.OceanLCLFreightCount = this.repository.GetAll(tenant).Where(p => p.TypeCode == "OLC").Count();
            tariffsSummary.OceanFCLFreightCount = this.repository.GetAll(tenant).Where(p => p.TypeCode == "OFC").Count();
            tariffsSummary.OceanFCLSurchargesCount = this.repository.GetAll(tenant).Where(p => p.TypeCode == "OFS").Count();
            tariffsSummary.ImportCustomsChargesCount = this.repository.GetAll(tenant).Where(p => p.TypeCode == "ICC").Count();
            tariffsSummary.ExportCustomsChargesCount = this.repository.GetAll(tenant).Where(p => p.TypeCode == "ECC").Count();
            tariffsSummary.InlandFTLTariffsCount = this.repository.GetAll(tenant).Where(p => p.TypeCode == "IFT").Count();

            return tariffsSummary;
        }
        public TariffsSummary GetSaleCount(int tenant)
        {
            TariffsSummary tariffsSummary = new TariffsSummary() { Id = tenant };
            tariffsSummary.ImportSaleCount = this.repository.GetAll(tenant).Where(p => p.TypeCode == "ILCS").Count();
            tariffsSummary.ExportSaleCount = this.repository.GetAll(tenant).Where(p => p.TypeCode == "ELCS").Count();
            return tariffsSummary;
        }
        public List<TariffSearchSummary> GetTariffSearchSummary(TariffSearchArgs args, int tenant) {

            string fromport = args.OriginPortId;
            string toport = args.DestinationPortId;
            string viaPort = args.ViaPortId;
            DateTime? BetweenDate = args.BetweenDate;
            double weight = args.Weight;
            string Weightcode = args.WeightCode;
            double? GrossWeight = args.GrossWeight;
            string GrossWeightCode = args.GrossWeightCode;
            double? Volume = args.Volume;
            string VolumeCode = args.VolumeUnitCode;
            string currencyId = args.CurrencyId;
            string typeCode = args.TariffType;


            AirlineRepository airlineRepository = new AirlineRepository(tenant);
            AirlineQuery airlineQuery = new AirlineQuery(airlineRepository);
            ShippingLineRepository shippingLineRepository = new ShippingLineRepository(tenant);
            ShippingLineQuery shippingLineQuery = new ShippingLineQuery(shippingLineRepository);

            List<TariffSearchSummary> tariffSearchSummaries = new List<TariffSearchSummary>();
            IQueryable<TariffLine> iQueryable = this.repository.GetAllTariffLines(tenant);
            iQueryable = iQueryable.Where(p => p.OriginPortId == fromport && p.DestinationPortId == toport && System.Data.Entity.DbFunctions.TruncateTime(p.StartDate) <= BetweenDate && (p.ExpirationDate != null ? (System.Data.Entity.DbFunctions.TruncateTime(p.ExpirationDate) >= BetweenDate):true));
            List<string> tariffids = iQueryable.Select(p => p.TariffId).Distinct().ToList();//.ToDictionary(p=>p.Key,p=>p);
            TariffSettingRepository tariffSettingRepository = new TariffSettingRepository(tenant);
            List<TariffSetting> setting = tariffSettingRepository.GetAll(tenant).ToList();
            List<string> Steps = new List<string>();
            ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);
            this.GetRates(tenant);
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


            List<Tariff> TariffListTemp = this.repository.GetAllTariff(tariffids.ToArray(), tenant).Where(p => !p.InActive && p.TypeCode == typeCode).ToList();


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


            List<Tariff> TariffList = this.repository.GetAllTariff(items.Select(p => p.tariffid).ToArray(), tenant).Where(p => !p.InActive && p.TypeCode == typeCode).ToList();
            if (typeCode == "OFC")
            {
                TariffList = this.FilterTariffsByContainers(TariffList, args);
            }

            List<TariffVersion> TariffVersionList = this.repository.GetAllTariffVersionsByTariffIds(items.Select(p => p.tariffid).ToArray(), tenant).ToList();
            List<int> VersionIds = TariffVersionList.Select(a => a.Version).ToList();

            Dictionary<string, List<TariffLine>> TariffLines = this.repository.GetAllTariffLinesByTariffIds(TariffList.Select(p => p.Id).ToArray(), tenant).Where(p => VersionIds.Contains(p.Version)).Where(p => System.Data.Entity.DbFunctions.TruncateTime(p.StartDate) <= BetweenDate && p.ExpirationDate != null ? (System.Data.Entity.DbFunctions.TruncateTime(p.ExpirationDate) >= BetweenDate) : true).GroupBy(p => p.TariffId).ToDictionary(o => o.Key, o => o.ToList());

            Dictionary<string, string> Currencies = myCommonContext.Currencies.Where(p => p.Tenant == tenant).ToDictionary(p => p.Id, p => p.Code);
            List<TariffVersionAllInCharge> TariffVersionAllInChargesList = this.repository.GetAllTariffAllInOnVersionsByTariffIds(items.Select(p => p.tariffid).ToArray(), TariffVersionList.Select(p => p.Version).ToArray(), tenant).ToList();
            List<Tariff> SurchargeTariffList = this.repository.GetSurchargeTariffsByCodeAndSellerId(TariffList.Select(p => p.SellerId).ToArray(),typeCode, tenant).Where(p => !p.InActive).ToList();
            List<Measurement> UsedMeasurements = myCommonContext.Measurements.Where(p => p.Tenant == tenant).ToList();
            List<ChargesType> chargesTypes = myCommonContext.ChargesTypes.Where(p => p.Tenant == tenant).ToList();

            List<TariffVersion> TariffSurchargeVersionList = this.repository.GetAllTariffVersionsByTariffIds(SurchargeTariffList.Select(p => p.Id).ToArray(), tenant).ToList();
            List<int> VersionsSurchargeIds = TariffSurchargeVersionList.Select(a => a.Version).ToList();
            Dictionary<string, List<TariffLine>> SurchargeTariffLines = this.repository.GetAllTariffLinesByTariffIds(SurchargeTariffList.Select(p => p.Id).ToArray(), tenant).Where(p => VersionsSurchargeIds.Contains(p.Version)).Where(p => System.Data.Entity.DbFunctions.TruncateTime(p.StartDate) <= BetweenDate && p.ExpirationDate != null ? (System.Data.Entity.DbFunctions.TruncateTime(p.ExpirationDate) >= BetweenDate) : true).GroupBy(p => p.TariffId).ToDictionary(o => o.Key, o => o.ToList());
            Dictionary<string, List<TariffLine>> SurchargeTariffLinesFiltered = new Dictionary<string, List<TariffLine>>();// this.repository.GetAllTariffLinesByTariffIds(SurchargeTariffList.Select(p => p.Id).ToArray(), tenant).Where(p => VersionsSurchargeIds.Contains(p.Version)).Where(p => System.Data.Entity.DbFunctions.TruncateTime(p.StartDate) <= BetweenDate && p.ExpirationDate != null ? (System.Data.Entity.DbFunctions.TruncateTime(p.ExpirationDate) >= BetweenDate) : true).GroupBy(p => p.TariffId).ToDictionary(o => o.Key, o => o.ToList());

            foreach (KeyValuePair<string, List<TariffLine>> entry in SurchargeTariffLines)
            {
                List<TariffLine> filteredLines = new List<TariffLine>();
                filteredLines = entry.Value.ToList().Where(p => p.OriginPortId == fromport && p.DestinationPortId == toport).ToList();
                if (filteredLines.Count() == 0)
                {
                    filteredLines = entry.Value.ToList().Where(p => p.OriginPortId == fromport && p.IsToAllOtherPorts == true).ToList();

                    if (filteredLines.Count() == 0)
                    {
                        filteredLines = entry.Value.ToList().Where(p => p.DestinationPortId == toport && p.IsFromAllOtherPorts == true).ToList();

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
                    TariffSearchSummary tariffsSummary = new TariffSearchSummary() { TariffId = result.Id };
                    tariffsSummary.SurchargesWithoutAllIn = new List<SurchargeSummary>();
                    //tariffsSummary.price = Math.Round((double)item.price, 2).ToString("0.00");
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

                    tariffsSummary = GetViaPortIdAndCode(SelectedLine, tariffsSummary,tenant);

                    if (item.PriceIndex != 0)
                    {
                        if ((item.Price * (decimal)weight) < minprice)
                        {
                            item.Price = minprice;
                            tariffsSummary.IsMinIconVisible = true;
                        }
                        else
                        {
                            item.Price = item.Price * (decimal)weight;
                        }
                    }
                    else
                    {
                        if (minprice != null)
                        {
                            item.Price = minprice;
                            tariffsSummary.IsMinIconVisible = true;
                        }
                        else
                        {
                            item.Price = 0;
                        }
                    }

                    //tariffsSummary.price = Math.Round((double)item.price, 2).ToString("0.00");
                    tariffsSummary.Price = Math.Round((double)CalculateLocalAmount(item.Price != null ? item.Price.Value : 0, currencyId, result.CurrencyId, tenant), 2).ToString("0.00");
                    tariffsSummary.ActualPrice = item.Price;
                    List<TariffVersionAllInCharge> allinList = TariffVersionAllInChargesList.Where(p => p.TariffId == item.tariffid && p.Version == item.TariffVersion).ToList();
                    if (allinList != null && allinList.Count > 0)
                    {
                        List<string> AllInChargesIds = TariffVersionAllInChargesList.Where(p => p.TariffId == item.tariffid && p.Version == item.TariffVersion).Select(p => p.ChargesTypeId).ToList();
                        List<string> AllInChargesNames = chargesTypes.Where(p => AllInChargesIds.Contains(p.Id)).Select(p => p.EnglishName).ToList();
                        tariffsSummary.AllIn = string.Join(", ", AllInChargesNames);
                        List<string> AllInIds_Charges = chargesTypes.Where(p => AllInChargesIds.Contains(p.Id)).Select(p => p.Id).ToList();
                        tariffsSummary.AllInIds = string.Join(", ", AllInIds_Charges);
                    }
                    Tariff CurrentSurcharge = SurchargeTariffList.Where(p => p.SellerId == result.SellerId).FirstOrDefault();

                    string chargeCode = null;
                    string sellerName = "";
                    string documentId = null;
                    AirlinePM airline = null;
                    ShippingLinePM shippingLine = null;
                    if (typeCode == "AFC")
                    {
                        airline = airlineQuery.GetSinglePM(result.SellerId, tenant);
                        sellerName = airline != null && airline.Card != null ? airline.Card.EnglishName : "";
                        documentId = airline.ImageDetailId;
                        chargeCode = "AFT";
                    }
                    else if (typeCode == "OLC" || typeCode == "OFC")
                    {
                        shippingLine = shippingLineQuery.GetSinglePM(result.SellerId, tenant);
                        sellerName = shippingLine != null && shippingLine.Card != null ? shippingLine.Card.EnglishName : "";
                        chargeCode = "OFT";
                    }

                    if (CurrentSurcharge != null)
                    {
                        if (SurchargeTariffLinesFiltered.ContainsKey(CurrentSurcharge.Id))
                        {
                            TariffLine ChargesfilteredLines = SurchargeTariffLinesFiltered[CurrentSurcharge.Id].FirstOrDefault();

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
                                                surchargeName = CurrentCharge.EnglishName;// (string)ChargesfilteredLines.GetType().GetProperty("Surcharge" + i + "Price").GetValue(ChargesfilteredLines);
                                                surchargeCode = CurrentCharge.Code;// (string)ChargesfilteredLines.GetType().GetProperty("Surcharge" + i + "Price").GetValue(ChargesfilteredLines);
                                                surchargeChargeTypeId = CurrentCharge.Id;
                                            }
                                            //   TariffLine surchargeLine= SurchargeTariffLines.Min(p=>p.)
                                            if (valueofSurcharge != null)
                                            {
                                                decimal? CurrentSurchargePriceCalculation = 0;

                                               
                                                SurchargeItem.Code = surchargeCode;
                                                SurchargeItem.Name = surchargeName;
                                                SurchargeItem.ChargeTypeId = surchargeChargeTypeId;
                                                SurchargeItem.UnitOfMesurmentCode = UsedMesurment.Code;
                                                SurchargeItem.UnitOfMesurmentId = UsedMesurment.Id;
                                                switch (UsedMesurment.Code)
                                                {
                                                    case "GRWT": { myQuantity = (decimal?)GrossWeight; break; }
                                                    case "CHWT": { myQuantity = (decimal?)weight; break; }
                                                    case "VOLU": { myQuantity = (decimal?)Volume; break; }
                                                    case "BTEU": { myQuantity = 1; break; }
                                                    case "FIXD": { myQuantity = 1; break; }
                                                    case "PRVL": { myQuantity = 1; break; }
                                                    case "PRFR": { myQuantity = 1; break; }
                                                    case "GWTN": { myQuantity = (decimal?)this.ComputeGrossWeigh_Kg_Ton(GrossWeight, GrossWeightCode, "ton"); break; }
                                                    case "CWKG": { myQuantity = (decimal?)this.ComputeChargeableWeight_Kg(weight, Weightcode); break; }
                                                    case "GWKG": { myQuantity = (decimal?)this.ComputeGrossWeigh_Kg_Ton(GrossWeight, GrossWeightCode, "kg"); break; }
                                                    case "QTY": { myQuantity = 1; break; }
                                                    case "VCBM": { myQuantity = (decimal?)ComputeVolumeInCBM(Volume, VolumeCode); break; }
                                                    default: { break; }
                                                }
                                                if (myQuantity == null)
                                                    myQuantity = 1;

                                                if (UsedMesurment.Code == "PRVL" || UsedMesurment.Code == "PRFR")
                                                {
                                                    CurrentSurchargePriceCalculation = ((valueofSurcharge * myQuantity * item.Price) / 100);
                                                }
                                                else
                                                {
                                                    CurrentSurchargePriceCalculation = (valueofSurcharge * myQuantity);
                                                }



                                                string CurrencyId = ChargesfilteredLines.CurrencyId != null ? ChargesfilteredLines.CurrencyId : CurrentSurcharge.CurrencyId;
                                                var LinePrice = CalculateLocalAmount(CurrentSurchargePriceCalculation.Value, currencyId, CurrencyId, tenant);

                                                decimal? minPriceSurcharge = null;
                                                if (valueofSurchargeMin != null)
                                                {
                                                    decimal minimumPrice = (decimal)valueofSurchargeMin;
                                                    minPriceSurcharge = CalculateLocalAmount(minimumPrice, currencyId, CurrencyId, tenant);
                                                    if (minPriceSurcharge > LinePrice)
                                                    {
                                                        LinePrice = minPriceSurcharge.Value;
                                                        SurchargeItem.IsMinIconVisible = true;
                                                    }
                                                }

                                                SurchargeItem.Price = LinePrice;
                                                SurchargeItem.ActualPrice = CurrentSurchargePriceCalculation.Value;
                                                Sum += SurchargeItem.Price;
                                                SurchargeItem.TariffId = CurrentSurcharge.Id;
                                                SurchargeItem.CurrencyId = CurrentSurcharge.CurrencyId;
                                                SurchargeItem.TariffNumber = CurrentSurcharge.TariffNumber;
                                                SurchargeItem.VersionId = ChargesfilteredLines.Version + "";
                                                SurchargeItem.LineId = ChargesfilteredLines.Id;
                                                SurchargeItem.SellerId = CurrentSurcharge.SellerId;
                                                SurchargeItem.SellerName = sellerName;
                                                SurchargeItem.MinPrice = minPriceSurcharge;

                                                surchargesList.Add(SurchargeItem);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }

                                }

                                tariffsSummary.AllInSurcharges = surchargesList.Where(a=>a.IsAllIn).ToList();
                                tariffsSummary.SurchargesWithoutAllIn = surchargesList.Where(a => !a.IsAllIn).ToList();
                            }
                        }
                    }

                    tariffsSummary.SellerName = sellerName;
                    tariffsSummary.EffictiveDate = result.ExpirationDate;
                    tariffsSummary.Remarks = result.Notes;
                    var calculatedLocalAmount = item.Price != null ? CalculateLocalAmount((item.Price).Value, currencyId, result.CurrencyId, tenant): 0;
                    tariffsSummary.decimalprice = (decimal?)Sum + calculatedLocalAmount;
                    tariffsSummary.VersionId = item.TariffVersion + "";
                    tariffsSummary.TariffId = item.tariffid;
                    tariffsSummary.TariffNumber = result.TariffNumber;

                
                    var airChrageType = chargesTypes.Where(p => p.Code == chargeCode).Select(p => p).FirstOrDefault();
                    tariffsSummary.ChargeTypeId = airChrageType.Id;
                    tariffsSummary.TotalSurcharge = Sum + "";
                    tariffsSummary.WholePrice = (decimal?)Sum + calculatedLocalAmount + "";
                    tariffsSummary.UnitOfMesurmentId = airChrageType.MeasurementId;
                    tariffsSummary.UnitOfMesurmentCode = UsedMeasurements.Where(p => p.Id == airChrageType.MeasurementId).Select(p => p.Code).FirstOrDefault();
                    tariffsSummary.SellerId = result.SellerId;
                    tariffsSummary.MinPrice = minprice;
                 
                    byte[] filedata = DownloadFile(documentId, "jpg", tenant, "images");
                    string resultImage = "";
                    if (filedata != null)
                    {
                        resultImage = "data:image/" + "jpg" + ";base64," + Convert.ToBase64String(filedata);
                    }

                    tariffsSummary.ImageId = resultImage;

                    if (!string.IsNullOrEmpty(currencyId))
                    {
                        tariffsSummary.CurrencyCode = Currencies.Keys.Contains(currencyId) ? Currencies[currencyId] : null;
                        tariffsSummary.CurrencyId = result.CurrencyId;
                    }

                    tariffSearchSummaries.Add(tariffsSummary);

                }
            }

            tariffSearchSummaries = tariffSearchSummaries.OrderBy(p => p.decimalprice).ToList();
            return tariffSearchSummaries;
        }

        private string fromport ;
        private string toport ;
        private string viaPort;
        private DateTime? BetweenDate;
        private string currencyId ;
        private string typeCode ;
        private int quantity1  ;
        private int quantity2 ;
        private int quantity3 ;
        private int quantity4 ;
        private int quantity5 ;
        private AirlineRepository airlineRepository ;
        private AirlineQuery airlineQuery ;
        private ShippingLineRepository shippingLineRepository ;
        private ShippingLineQuery shippingLineQuery ;
        private ICommonDataContext myCommonContext ;
        private List<TariffSearchSummary> tariffSearchSummaries ;
        private IQueryable<TariffLine> tariffLines_IQueryable;
        private List<string> tariffids;
        private List<TariffLinesContainersPrice> tariffLinesContainersPrices ;
        private List<Tariff> tariffList;
        private List<TariffLine> tariffLinesList;
        private List<TariffVersion> tariffVersionList;
        private Dictionary<string, string> currencies;
        private List<TariffVersionAllInCharge> tariffVersionAllInChargesList;
        private List<Tariff> surchargeTariffList;
        private List<Measurement> usedMeasurements;
        private List<ChargesType> chargesTypes;
        private Dictionary<string, List<TariffLine>> surchargeTariffLines;
        private Dictionary<string, List<TariffLine>> surchargeTariffLinesFiltered;
        private TariffSearchSummary tariffsSummary;
        private int tenant;
        private decimal? Sum;
        private string sellerName;
        private SurchargeSummary SurchargeItem;
        public List<TariffSearchSummary> GetTariffSearchFCLSummary(TariffSearchArgs args, int tenant)
        {
            this.Initialization(args, tenant);
            this.FillSurchargeTariffLinesFiltered();
            foreach (Tariff trariff in tariffList)
            {
                List<TariffLine> resultItems = tariffLinesList.Where(x => x.TariffId == trariff.Id && tariffVersionList.Where(a => a.Version == x.Version && a.TariffId == trariff.Id).FirstOrDefault() != null).ToList();
                foreach (TariffLine tariffLine in resultItems)
                {
                    this.Sum = 0;
                    this.tariffsSummary = new TariffSearchSummary() { TariffId = trariff.Id };
                    tariffsSummary.SurchargesWithoutAllIn = new List<SurchargeSummary>();
                    tariffsSummary.ContainersPrices = new List<ContainersPrice>();

                    var price  = this.CalculateContainerPrice(args, trariff, tariffLine);
                    tariffsSummary = GetViaPortIdAndCode(tariffLine, tariffsSummary, tenant);

                    tariffsSummary.Price = Math.Round((double)CalculateLocalAmount(price, currencyId, trariff.CurrencyId, tenant), 2).ToString("0.00");
                    tariffsSummary.ActualPrice = price;
                    tariffsSummary.LineId = tariffLine.Id;

                    ShippingLinePM shippingLine = shippingLineQuery.GetSinglePM(trariff.SellerId, tenant);
                    this.sellerName = shippingLine != null && shippingLine.Card != null ? shippingLine.Card.EnglishName : "";

                    this.FillAllInList(tariffLine);
                    this.FillSurchargeData(args,trariff, tariffLine);

                    string documentId = null;
                    string chargeCode = "OFT";

                    tariffsSummary.SellerName = sellerName;
                    tariffsSummary.EffictiveDate = trariff.ExpirationDate;
                    tariffsSummary.Remarks = trariff.Notes;
                    var calculatedLocalAmount = CalculateLocalAmount(price, currencyId, trariff.CurrencyId, tenant);
                    tariffsSummary.decimalprice = (decimal?)Sum + calculatedLocalAmount;
                    tariffsSummary.VersionId = tariffLine.Version + "";
                    tariffsSummary.TariffId = tariffLine.TariffId;
                    tariffsSummary.TariffNumber = trariff.TariffNumber;

                    var airChrageType = chargesTypes.Where(p => p.Code == chargeCode).Select(p => p).FirstOrDefault();
                    tariffsSummary.ChargeTypeId = airChrageType.Id;
                    tariffsSummary.TotalSurcharge = Sum + "";
                    tariffsSummary.WholePrice = (decimal?)Sum + calculatedLocalAmount + "";
                    tariffsSummary.UnitOfMesurmentId = airChrageType.MeasurementId;
                    tariffsSummary.UnitOfMesurmentCode = usedMeasurements.Where(p => p.Id == airChrageType.MeasurementId).Select(p => p.Code).FirstOrDefault();
                    tariffsSummary.SellerId = trariff.SellerId;

                    byte[] filedata = DownloadFile(documentId, "jpg", tenant, "images");
                    string resultImage = "";
                    if (filedata != null)
                    {
                        resultImage = "data:image/" + "jpg" + ";base64," + Convert.ToBase64String(filedata);
                    }

                    tariffsSummary.ImageId = resultImage;

                    if (!string.IsNullOrEmpty(currencyId))
                    {
                        tariffsSummary.CurrencyCode = currencies.Keys.Contains(currencyId) ? currencies[currencyId] : null;
                        tariffsSummary.CurrencyId = trariff.CurrencyId;
                    }
                    tariffSearchSummaries.Add(tariffsSummary);
                }
            }
            tariffSearchSummaries = tariffSearchSummaries.OrderBy(p => p.decimalprice).ToList();
            return tariffSearchSummaries;
        }

        private decimal CalculateContainerPrice(TariffSearchArgs args, Tariff trariff, TariffLine tariffLine)
        {
            decimal? price1 = null; decimal? price2 = null; decimal? price3 = null; decimal? price4 = null; decimal? price5 = null;
            int containerQuantity = 0;
            if (trariff.ContainerType1Id != null)
            {
                containerQuantity = trariff.ContainerType1Id == args.ContainerType1Id ? quantity1 : (trariff.ContainerType1Id == args.ContainerType2Id ? quantity2 : (trariff.ContainerType1Id == args.ContainerType3Id ? quantity3 : (trariff.ContainerType1Id == args.ContainerType4Id ? quantity4 : (trariff.ContainerType1Id == args.ContainerType5Id ? quantity5 : 0))));
                price1 = ((tariffLine.Surcharge1Price != null ? tariffLine.Surcharge1Price : 0) * containerQuantity);
                this.tariffsSummary.ContainersPrices.Add(new ContainersPrice
                {
                    TariffId = trariff.Id,
                    ContainerId = trariff.ContainerType1Id,
                    Price = price1, 
                    Quantity = containerQuantity
                });
            }
            if (trariff.ContainerType2Id != null)
            {
                containerQuantity = trariff.ContainerType2Id == args.ContainerType1Id ? quantity1 : (trariff.ContainerType2Id == args.ContainerType2Id ? quantity2 : (trariff.ContainerType2Id == args.ContainerType3Id ? quantity3 : (trariff.ContainerType2Id == args.ContainerType4Id ? quantity4 : (trariff.ContainerType2Id == args.ContainerType5Id ? quantity5 : 0))));
                price2 = ((tariffLine.Surcharge2Price != null ? tariffLine.Surcharge2Price : 0) * containerQuantity);
                this.tariffsSummary.ContainersPrices.Add(new ContainersPrice
                {
                    TariffId = trariff.Id,
                    ContainerId = trariff.ContainerType2Id,
                    Price = price2,
                    Quantity = containerQuantity
                });
            }
            if (trariff.ContainerType3Id != null)
            {
                containerQuantity = trariff.ContainerType3Id == args.ContainerType1Id ? quantity1 : (trariff.ContainerType3Id == args.ContainerType2Id ? quantity2 : (trariff.ContainerType3Id == args.ContainerType3Id ? quantity3 : (trariff.ContainerType3Id == args.ContainerType4Id ? quantity4 : (trariff.ContainerType3Id == args.ContainerType5Id ? quantity5 : 0))));
                price3 = ((tariffLine.Surcharge3Price != null ? tariffLine.Surcharge3Price : 0) * containerQuantity);
                this.tariffsSummary.ContainersPrices.Add(new ContainersPrice
                {
                    TariffId = trariff.Id,
                    ContainerId = trariff.ContainerType3Id,
                    Price = price3,
                    Quantity = containerQuantity
                });
            }
            if (trariff.ContainerType4Id != null)
            {
                containerQuantity = trariff.ContainerType4Id == args.ContainerType1Id ? quantity1 : (trariff.ContainerType4Id == args.ContainerType2Id ? quantity2 : (trariff.ContainerType4Id == args.ContainerType3Id ? quantity3 : (trariff.ContainerType4Id == args.ContainerType4Id ? quantity4 : (trariff.ContainerType4Id == args.ContainerType5Id ? quantity5 : 0))));

                price4 = ((tariffLine.Surcharge4Price != null ? tariffLine.Surcharge4Price : 0) * containerQuantity);
                this.tariffsSummary.ContainersPrices.Add(new ContainersPrice
                {
                    TariffId = trariff.Id,
                    ContainerId = trariff.ContainerType4Id,
                    Price = price4,
                    Quantity = containerQuantity
                });
            }
            if (trariff.ContainerType5Id != null)
            {
                containerQuantity = trariff.ContainerType5Id == args.ContainerType1Id ? quantity1 : (trariff.ContainerType5Id == args.ContainerType2Id ? quantity2 : (trariff.ContainerType5Id == args.ContainerType3Id ? quantity3 : (trariff.ContainerType5Id == args.ContainerType4Id ? quantity4 : (trariff.ContainerType5Id == args.ContainerType5Id ? quantity5 : 0))));

                price5 = ((tariffLine.Surcharge5Price != null ? tariffLine.Surcharge5Price : 0) * containerQuantity);
                this.tariffsSummary.ContainersPrices.Add(new ContainersPrice
                {
                    TariffId = trariff.Id,
                    ContainerId = trariff.ContainerType5Id,
                    Price = price5,
                    Quantity = containerQuantity
                });
            }

            var price = ((price1 != null) ? price1.Value : 0) +
                       ((price2 != null) ? price2.Value : 0) +
                       ((price3 != null) ? price3.Value : 0) +
                       ((price4 != null) ? price4.Value : 0) +
                       ((price5 != null) ? price5.Value : 0);

            return price;
        }


        private decimal CalculateContainerPriceFromTariffLinesContainers(TariffSearchArgs args, Tariff trariff, TariffLinesContainersPrice tariffLinesContainersPrice)
        {
            decimal? price1 = null; decimal? price2 = null; decimal? price3 = null; decimal? price4 = null; decimal? price5 = null;
            int containerQuantity = 0;
            if (trariff.ContainerType1Id != null)
            {
                containerQuantity = trariff.ContainerType1Id == args.ContainerType1Id ? quantity1 : (trariff.ContainerType1Id == args.ContainerType2Id ? quantity2 : (trariff.ContainerType1Id == args.ContainerType3Id ? quantity3 : (trariff.ContainerType1Id == args.ContainerType4Id ? quantity4 : (trariff.ContainerType1Id == args.ContainerType5Id ? quantity5 : 0))));
                price1 = ((tariffLinesContainersPrice.Price1 != null ? tariffLinesContainersPrice.Price1 : 0) * containerQuantity);
                this.SurchargeItem.ContainersPrices.Add(new ContainersPrice
                {
                    TariffId = trariff.Id,
                    ContainerId = trariff.ContainerType1Id,
                    Price = price1,
                    Quantity = containerQuantity
                });
            }
            if (trariff.ContainerType2Id != null)
            {
                containerQuantity = trariff.ContainerType2Id == args.ContainerType1Id ? quantity1 : (trariff.ContainerType2Id == args.ContainerType2Id ? quantity2 : (trariff.ContainerType2Id == args.ContainerType3Id ? quantity3 : (trariff.ContainerType2Id == args.ContainerType4Id ? quantity4 : (trariff.ContainerType2Id == args.ContainerType5Id ? quantity5 : 0))));
                price2 = ((tariffLinesContainersPrice.Price2!= null ? tariffLinesContainersPrice.Price2 : 0) * containerQuantity);
                this.SurchargeItem.ContainersPrices.Add(new ContainersPrice
                {
                    TariffId = trariff.Id,
                    ContainerId = trariff.ContainerType2Id,
                    Price = price2,
                    Quantity = containerQuantity
                });
            }
            if (trariff.ContainerType3Id != null)
            {
                containerQuantity = trariff.ContainerType3Id == args.ContainerType1Id ? quantity1 : (trariff.ContainerType3Id == args.ContainerType2Id ? quantity2 : (trariff.ContainerType3Id == args.ContainerType3Id ? quantity3 : (trariff.ContainerType3Id == args.ContainerType4Id ? quantity4 : (trariff.ContainerType3Id == args.ContainerType5Id ? quantity5 : 0))));

                price3 = ((tariffLinesContainersPrice.Price3 != null ? tariffLinesContainersPrice.Price3: 0) * containerQuantity);
                this.SurchargeItem.ContainersPrices.Add(new ContainersPrice
                {
                    TariffId = trariff.Id,
                    ContainerId = trariff.ContainerType3Id,
                    Price = price3,
                    Quantity = containerQuantity
                });
            }
            if (trariff.ContainerType4Id != null)
            {
                containerQuantity = trariff.ContainerType4Id == args.ContainerType1Id ? quantity1 : (trariff.ContainerType4Id == args.ContainerType2Id ? quantity2 : (trariff.ContainerType4Id == args.ContainerType3Id ? quantity3 : (trariff.ContainerType4Id == args.ContainerType4Id ? quantity4 : (trariff.ContainerType4Id == args.ContainerType5Id ? quantity5 : 0))));

                price4 = ((tariffLinesContainersPrice.Price4 != null ? tariffLinesContainersPrice.Price4 : 0) * containerQuantity);
                this.SurchargeItem.ContainersPrices.Add(new ContainersPrice
                {
                    TariffId = trariff.Id,
                    ContainerId = trariff.ContainerType4Id,
                    Price = price4,
                    Quantity = containerQuantity
                });
            }
            if (trariff.ContainerType5Id != null)
            {
                containerQuantity = trariff.ContainerType5Id == args.ContainerType1Id ? quantity1 : (trariff.ContainerType5Id == args.ContainerType2Id ? quantity2 : (trariff.ContainerType5Id == args.ContainerType3Id ? quantity3 : (trariff.ContainerType5Id == args.ContainerType4Id ? quantity4 : (trariff.ContainerType5Id == args.ContainerType5Id ? quantity5 : 0))));

                price5 = ((tariffLinesContainersPrice.Price5 != null ? tariffLinesContainersPrice.Price5 : 0) * containerQuantity);
                this.SurchargeItem.ContainersPrices.Add(new ContainersPrice
                {
                    TariffId = trariff.Id,
                    ContainerId = trariff.ContainerType5Id,
                    Price = price5,
                    Quantity = containerQuantity
                });
            }

            var price = ((price1 != null) ? price1.Value : 0) +
                       ((price2 != null) ? price2.Value : 0) +
                       ((price3 != null) ? price3.Value : 0) +
                       ((price4 != null) ? price4.Value : 0) +
                       ((price5 != null) ? price5.Value : 0);

            return price;
        }

        private void Initialization(TariffSearchArgs args, int tenant)
        {
            this.fromport = args.OriginPortId;
            this.toport = args.DestinationPortId;
            this.viaPort = args.ViaPortId;
            this.BetweenDate = args.BetweenDate;
            this.currencyId = args.CurrencyId;
            this.typeCode = args.TariffType;
            this.quantity1 = args.Quantity1 != null ? args.Quantity1.Value : 0;
            this.quantity2 = args.Quantity2 != null ? args.Quantity2.Value : 0;
            this.quantity3 = args.Quantity3 != null ? args.Quantity3.Value : 0;
            this.quantity4 = args.Quantity4 != null ? args.Quantity4.Value : 0;
            this.quantity5 = args.Quantity5 != null ? args.Quantity5.Value : 0;
            this.tenant = tenant;
            this.airlineRepository = new AirlineRepository(tenant);
            this.airlineQuery = new AirlineQuery(airlineRepository);
            this.shippingLineRepository = new ShippingLineRepository(tenant);
            this.shippingLineQuery = new ShippingLineQuery(shippingLineRepository);
            this.myCommonContext = CommonDataContext.GetContext(tenant);

            this.tariffSearchSummaries = new List<TariffSearchSummary>();
            this.tariffLines_IQueryable = this.repository.GetAllTariffLines(tenant).Where(p => p.OriginPortId == fromport && p.DestinationPortId == toport && System.Data.Entity.DbFunctions.TruncateTime(p.StartDate) <= BetweenDate && (p.ExpirationDate != null ? (System.Data.Entity.DbFunctions.TruncateTime(p.ExpirationDate) >= BetweenDate) : true));
            this.tariffids = tariffLines_IQueryable.Select(p => p.TariffId).Distinct().ToList();

            this.tariffLinesContainersPrices = (from d in this.context.TariffLinesContainersPrices
                                                where d.Tenant == tenant
                                                select d).ToList();

            List<Tariff> TariffListTemp = this.repository.GetAllTariff(tariffids.ToArray(), tenant).Where(p => !p.InActive && p.TypeCode == typeCode).ToList();
            this.tariffList = FilterTariffsByContainers(TariffListTemp, args);

            List<string> tempTariffIds = tariffList.Select(a => a.Id).ToList();
            this.tariffLinesList = tariffLines_IQueryable.Where(p => tempTariffIds.Contains(p.TariffId)).ToList();

            this.tariffVersionList = this.repository.GetAllTariffVersionsByTariffIds(tariffids.ToArray(), tenant).ToList();
            this.GetRates(tenant);

            this.currencies = myCommonContext.Currencies.Where(p => p.Tenant == tenant).ToDictionary(p => p.Id, p => p.Code);
            this.tariffVersionAllInChargesList = this.repository.GetAllTariffAllInOnVersionsByTariffIds(tariffids.ToArray(), tariffVersionList.Select(p => p.Version).ToArray(), tenant).ToList();
            this.surchargeTariffList = this.repository.GetSurchargeTariffsByCodeAndSellerId(tariffList.Select(p => p.SellerId).ToArray(), typeCode, tenant).Where(p => !p.InActive).ToList();
            this.surchargeTariffList = FilterTariffsByContainers(surchargeTariffList, args);

            this.usedMeasurements = myCommonContext.Measurements.Where(p => p.Tenant == tenant).ToList();
            this.chargesTypes = myCommonContext.ChargesTypes.Where(p => p.Tenant == tenant).ToList();
            List<TariffVersion> TariffSurchargeVersionList = this.repository.GetAllTariffVersionsByTariffIds(surchargeTariffList.Select(p => p.Id).ToArray(), tenant).ToList();
            List<int> VersionsSurchargeIds = TariffSurchargeVersionList.Select(a => a.Version).ToList();
            this.surchargeTariffLines = this.repository.GetAllTariffLinesByTariffIds(surchargeTariffList.Select(p => p.Id).ToArray(), tenant).Where(p => VersionsSurchargeIds.Contains(p.Version)).Where(p => System.Data.Entity.DbFunctions.TruncateTime(p.StartDate) <= BetweenDate && p.ExpirationDate != null ? (System.Data.Entity.DbFunctions.TruncateTime(p.ExpirationDate) >= BetweenDate) : true).GroupBy(p => p.TariffId).ToDictionary(o => o.Key, o => o.ToList());
            this.surchargeTariffLinesFiltered = new Dictionary<string, List<TariffLine>>();
        }
        private void FillSurchargeData(TariffSearchArgs args, Tariff trariff, TariffLine tariffLine)
        {
            Tariff CurrentSurcharge = surchargeTariffList.Where(p => p.SellerId == trariff.SellerId).FirstOrDefault();
            if (CurrentSurcharge != null)
            {
                if (surchargeTariffLinesFiltered.ContainsKey(CurrentSurcharge.Id))
                {
                    TariffLine ChargesfilteredLines = surchargeTariffLinesFiltered[CurrentSurcharge.Id].FirstOrDefault();

                    if (ChargesfilteredLines != null)
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            string chargeId = (string)CurrentSurcharge.GetType().GetProperty("Surcharge" + i + "Id").GetValue(CurrentSurcharge);
                            if (tariffVersionAllInChargesList.Where(p => p.TariffId == tariffLine.TariffId && p.Version == tariffLine.Version && p.ChargesTypeId == chargeId).FirstOrDefault() == null)
                            {
                                string measurementId = (string)CurrentSurcharge.GetType().GetProperty("Surcharge" + i + "UOM").GetValue(CurrentSurcharge);
                                if (!string.IsNullOrEmpty(measurementId))
                                {
                                    Measurement UsedMesurment = usedMeasurements.Where(p => p.Id == measurementId).FirstOrDefault();

                                    if (UsedMesurment != null)
                                    {
                                        var currentTariffLinesContainersPrice = tariffLinesContainersPrices.Where(a => a.TariffId == CurrentSurcharge.Id && a.SurchargeId == chargeId && a.TariffLine == ChargesfilteredLines).FirstOrDefault();
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
                                            SurchargeItem = new SurchargeSummary();
                                            SurchargeItem.ContainersPrices = new List<ContainersPrice>();

                                            decimal currentSurchargePriceCalculation = this.CalculateContainerPriceFromTariffLinesContainers(args, CurrentSurcharge, currentTariffLinesContainersPrice);

                                            
                                            SurchargeItem.Code = surchargeCode;
                                            SurchargeItem.Name = surchargeName;
                                            SurchargeItem.ChargeTypeId = surchargeChargeTypeId;
                                            SurchargeItem.UnitOfMesurmentCode = UsedMesurment.Code;
                                            SurchargeItem.UnitOfMesurmentId = UsedMesurment.Id;

                                            string CurrencyId = ChargesfilteredLines.CurrencyId != null ? ChargesfilteredLines.CurrencyId : CurrentSurcharge.CurrencyId;
                                            var LinePrice = CalculateLocalAmount(currentSurchargePriceCalculation, currencyId, CurrencyId, tenant);

                                            SurchargeItem.Price = LinePrice;
                                            SurchargeItem.ActualPrice = currentSurchargePriceCalculation;
                                            Sum += SurchargeItem.Price;
                                            SurchargeItem.TariffId = CurrentSurcharge.Id;
                                            SurchargeItem.CurrencyId = CurrencyId;
                                            SurchargeItem.TariffNumber = CurrentSurcharge.TariffNumber;
                                            SurchargeItem.VersionId = ChargesfilteredLines.Version + "";
                                            SurchargeItem.SellerId = CurrentSurcharge.SellerId;
                                            SurchargeItem.SellerName = sellerName;
                                            SurchargeItem.LineId = ChargesfilteredLines.Id;
                                            tariffsSummary.SurchargesWithoutAllIn.Add(SurchargeItem);
                                        }
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
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
                filteredLines = entry.Value.ToList().Where(p => p.OriginPortId == fromport && p.DestinationPortId == toport).ToList();
                if (filteredLines.Count() == 0)
                {
                    filteredLines = entry.Value.ToList().Where(p => p.OriginPortId == fromport && p.IsToAllOtherPorts == true).ToList();

                    if (filteredLines.Count() == 0)
                    {
                        filteredLines = entry.Value.ToList().Where(p => p.DestinationPortId == toport && p.IsFromAllOtherPorts == true).ToList();

                        if (filteredLines.Count() == 0)
                        {
                            filteredLines = entry.Value.ToList().Where(p => p.IsToAllOtherPorts == true && p.IsFromAllOtherPorts == true).ToList();
                        }
                    }
                }
                this.surchargeTariffLinesFiltered.Add(entry.Key, filteredLines);
            }
        }
        private List<Tariff> FilterTariffsByContainers(List<Tariff> tariffList, TariffSearchArgs args)
        {
            if (!string.IsNullOrEmpty(args.ContainerType1Id))
            {
                tariffList = tariffList.Where(a => a.ContainerType1Id == args.ContainerType1Id || a.ContainerType2Id == args.ContainerType1Id
               || a.ContainerType3Id == args.ContainerType1Id || a.ContainerType4Id == args.ContainerType1Id || a.ContainerType5Id == args.ContainerType1Id).ToList();
            }

            if (!string.IsNullOrEmpty(args.ContainerType2Id))
            {
                tariffList = tariffList.Where(a => a.ContainerType1Id == args.ContainerType2Id || a.ContainerType2Id == args.ContainerType2Id
             || a.ContainerType3Id == args.ContainerType2Id || a.ContainerType4Id == args.ContainerType2Id || a.ContainerType5Id == args.ContainerType2Id).ToList();
            }

            if (!string.IsNullOrEmpty(args.ContainerType3Id))
            {
                tariffList = tariffList.Where(a => a.ContainerType1Id == args.ContainerType3Id || a.ContainerType2Id == args.ContainerType3Id
              || a.ContainerType3Id == args.ContainerType3Id || a.ContainerType4Id == args.ContainerType3Id || a.ContainerType5Id == args.ContainerType3Id).ToList();
            }

            if (!string.IsNullOrEmpty(args.ContainerType4Id))
            {
                tariffList = tariffList.Where(a => a.ContainerType1Id == args.ContainerType4Id || a.ContainerType2Id == args.ContainerType4Id
             || a.ContainerType3Id == args.ContainerType4Id || a.ContainerType4Id == args.ContainerType4Id || a.ContainerType5Id == args.ContainerType4Id).ToList();
            }

            if (!string.IsNullOrEmpty(args.ContainerType5Id))
            {
                tariffList = tariffList.Where(a => a.ContainerType1Id == args.ContainerType5Id || a.ContainerType2Id == args.ContainerType5Id
              || a.ContainerType3Id == args.ContainerType5Id || a.ContainerType4Id == args.ContainerType5Id || a.ContainerType5Id == args.ContainerType5Id).ToList();
            }

            return tariffList;
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

        List<RatesTableList> RatesList;
        private void GetRates(int tenant)
        {
            IWebFreightContext MyContext = WebFreightContext.GetContext(tenant);
            RatesTableRepository ratesTableRepository = new RatesTableRepository(MyContext);
            IQueryable<RatesTable> entityPocos = ratesTableRepository.GetRatesTables(tenant);

            RatesTableQuery ratesTableQuery = new RatesTableQuery(ratesTableRepository);
            IQueryable<RatesTableList> entityLists = ratesTableQuery.GetIQueryableEntityList(entityPocos);
            entityLists = entityLists.OrderByDescending(r => r.ValueDate);

            this.RatesList = entityLists.ToList();
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
        private bool ValidatePreviousLineDates(dynamic previousLineDatesArgs)
        {
            bool isExpirationDateValid = true;

            if (previousLineDatesArgs.DateField == "expiration")
            {
                if (previousLineDatesArgs.TariffLineExpirationDateItem.ExpirationDate < previousLineDatesArgs.PreviousLine.StartDate)
                {

                    isExpirationDateValid = false;
                }
            }

            else if (previousLineDatesArgs.DateField == "start")
            {
                if (previousLineDatesArgs.TariffLinePM.StartDate < previousLineDatesArgs.PreviousLine.StartDate)
                {
                    isExpirationDateValid = false;
                }
            }

            return isExpirationDateValid;
        }

        public bool CheckDatesValidity(string FromPort, string ToPort, DateTime? ToDate, string TariffId)
        {
           TariffPM entityPM = this.GetSingle(TariffId, true, false);
            TariffLineRepository iTariffLineRepository = new TariffLineRepository(entityPM.Tenant);
            if (entityPM.TypeCode == "ASC" || entityPM.TypeCode == "OSC" || entityPM.TypeCode == "OFS" || entityPM.TypeCode == "IFT")
            {
                TariffVersionPM iPreviousVersion = entityPM.ActiveVersions.OrderByDescending(o => o.CreateDate).FirstOrDefault();
                if (iPreviousVersion != null)
                {
                    List<TariffLine> iPreviousVersionLines = iTariffLineRepository.GetTariffLinesByTariffAndVersion(entityPM.Id, iPreviousVersion.Version, entityPM.Tenant);                  
                    TariffLine previousLine = iPreviousVersionLines.Where(d => d.OriginPortId == FromPort && d.DestinationPortId == ToPort).FirstOrDefault();
                    if (previousLine != null)
                    {
                        if (previousLine.StartDate >= ToDate)
                        {
                            throw new ApplicationException("Expiration date can't be less than start date in the previous version line");
                        }

                    }                    
                }
            }

            return true;
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
                        case "CBI": { factorOfConvert = 61024; break; }      // 1m³ = 61024in³
                        case "CBF": { factorOfConvert = 35.315; break; }     // 1m³ = 35.315ft³
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
          

        private double? ComputeGrossWeigh_Kg_Ton(double? GrossWeight,string GrossWeightUnitCode,string type)
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

        private byte[] DownloadFile(string documentId, string type, int tenant, string fileLocation)
        {
            string fileName = documentId + ".jpg";
            string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), "fileLocation");

            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = documentId,
                FolderName = fileLocation,
                Extension = "jpg",
                Tenant = tenant,
            };

            byte[] datainByte = storageservice.Read(fileInfo);
            return datainByte;
        }

        public int GetActiveTariffCountByTenantAndSeller(int tenant, string sellerId)
        {
            List<Tariff> query = new List<Tariff>();
            query= (from a in context.Tariffs
                                  where a.Tenant == tenant && a.SellerId == sellerId
                                  select a).ToList();
            return query.Count();
        }

        private TariffSearchSummary GetViaPortIdAndCode(TariffLine tariffLine, TariffSearchSummary tariffSummary, int tenant)
        {
            if (tariffLine != null)
            {
                if (tariffLine.ViaPortId != null)
                {
                    PortPM viaPort = PortQuery.GetSinglePort(tenant, tariffLine.ViaPortId, true);
                    tariffSummary.ViaPortId = tariffLine.ViaPortId;
                    tariffSummary.ViaPortCode = viaPort != null ? viaPort.Code : "";
                    tariffSummary.ViaPortName = viaPort != null ? viaPort.EnglishName : "";
                    this.tariffsSummary.ViaPortCountryCode = viaPort != null ? viaPort.CountryCode : "";
                    this.tariffsSummary.ViaPortCountryName = viaPort != null ? viaPort.CountryName : "";
                }
            }
            return tariffSummary;
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
}
	 