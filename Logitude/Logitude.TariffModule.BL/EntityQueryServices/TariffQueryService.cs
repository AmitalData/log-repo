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
            
            return tariffsSummary;
        }

        public List<TariffSearchSummary> GetTariffSearchSummary(string fromport, string toport, DateTime? BetweenDate, double weight, int tenant, string Weightcode, double? GrossWeight, string GrossWeightCode, double? Volume, string VolumeCode, string currencyId)
        {
            AirlineRepository airlineRepository = new AirlineRepository(tenant);
            AirlineQuery airlineQuery = new AirlineQuery(airlineRepository);
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


            List<Tariff> TariffListTemp = this.repository.GetAllTariff(tariffids.ToArray(), tenant).Where(p => !p.InActive && p.TypeCode == "AFC").ToList();


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



            List<Tariff> TariffList = this.repository.GetAllTariff(items.Select(p => p.tariffid).ToArray(), tenant).Where(p => !p.InActive && p.TypeCode == "AFC").ToList();
            List<TariffVersion> TariffVersionList = this.repository.GetAllTariffVersionsByTariffIds(items.Select(p => p.tariffid).ToArray(), tenant).ToList();
            List<int> VersionIds = TariffVersionList.Select(a => a.Version).ToList();

            Dictionary<string, List<TariffLine>> TariffLines = this.repository.GetAllTariffLinesByTariffIds(TariffList.Select(p => p.Id).ToArray(), tenant).Where(p => VersionIds.Contains(p.Version)).Where(p => System.Data.Entity.DbFunctions.TruncateTime(p.StartDate) <= BetweenDate && p.ExpirationDate != null ? (System.Data.Entity.DbFunctions.TruncateTime(p.ExpirationDate) >= BetweenDate) : true).GroupBy(p => p.TariffId).ToDictionary(o => o.Key, o => o.ToList());

            Dictionary<string, string> Currencies = myCommonContext.Currencies.Where(p => p.Tenant == tenant).ToDictionary(p => p.Id, p => p.Code);
            List<TariffVersionAllInCharge> TariffVersionAllInChargesList = this.repository.GetAllTariffAllInOnVersionsByTariffIds(items.Select(p => p.tariffid).ToArray(), TariffVersionList.Select(p => p.Version).ToArray(), tenant).ToList();
            List<Tariff> SurchargeTariffList = this.repository.GetSurchargeTariffsByAirline(TariffList.Select(p => p.SellerId).ToArray(), tenant).Where(p => !p.InActive).ToList();
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
                    tariffsSummary.Surcharges = new List<SurchargeSummary>();
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
                    }
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
                    }
                    Tariff CurrentSurcharge = SurchargeTariffList.Where(p => p.SellerId == result.SellerId).FirstOrDefault();
                    AirlinePM airline = airlineQuery.GetSinglePM(result.SellerId, tenant);
                    if (CurrentSurcharge != null)
                    {
                        if (SurchargeTariffLinesFiltered.ContainsKey(CurrentSurcharge.Id))
                        {
                            TariffLine ChargesfilteredLines = SurchargeTariffLinesFiltered[CurrentSurcharge.Id].FirstOrDefault();

                            if (ChargesfilteredLines != null)
                            {

                                for (int i = 1; i <= 10; i++)
                                {

                                    decimal? myQuantity = 0;
                                    string chargeId = (string)CurrentSurcharge.GetType().GetProperty("Surcharge" + i + "Id").GetValue(CurrentSurcharge);
                                    if (TariffVersionAllInChargesList.Where(p => p.TariffId == item.tariffid && p.Version == item.TariffVersion && p.ChargesTypeId == chargeId).FirstOrDefault() == null)
                                    {
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

                                                    SurchargeSummary SurchargeItem = new SurchargeSummary();
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
                                                    SurchargeItem.TariffId =  CurrentSurcharge.Id;
                                                    SurchargeItem.CurrencyId = CurrentSurcharge.CurrencyId;
                                                    SurchargeItem.TariffNumber = CurrentSurcharge.TariffNumber;
                                                    SurchargeItem.VersionId = ChargesfilteredLines.Version + "";
                                                    SurchargeItem.SellerId = CurrentSurcharge.SellerId;
                                                    SurchargeItem.SellerName= airline.Card != null ? airline.Card.EnglishName : "";
                                                    SurchargeItem.MinPrice = minPriceSurcharge;
                                                    tariffsSummary.Surcharges.Add(SurchargeItem);
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


                  
                    tariffsSummary.SellerName = airline.Card != null ? airline.Card.EnglishName : "";
                    tariffsSummary.EffictiveDate = result.ExpirationDate;
                    tariffsSummary.Remarks = result.Notes;
                    var calculatedLocalAmount = item.Price != null ? CalculateLocalAmount((item.Price).Value, currencyId, result.CurrencyId, tenant): 0;
                    tariffsSummary.decimalprice = (decimal?)Sum + calculatedLocalAmount;
                    tariffsSummary.VersionId = item.TariffVersion + "";
                    tariffsSummary.TariffId = item.tariffid;
                    tariffsSummary.TariffNumber = result.TariffNumber;
                    var airChrageType = chargesTypes.Where(p => p.Code == "AFT").Select(p => p).FirstOrDefault();
                    tariffsSummary.ChargeTypeId = airChrageType.Id;
                    tariffsSummary.TotalSurcharge = Sum + "";
                    tariffsSummary.WholePrice = (decimal?)Sum + calculatedLocalAmount + "";
                    tariffsSummary.UnitOfMesurmentId = airChrageType.MeasurementId;
                    tariffsSummary.UnitOfMesurmentCode = UsedMeasurements.Where(p => p.Id == airChrageType.MeasurementId).Select(p => p.Code).FirstOrDefault();
                    tariffsSummary.SellerId = result.SellerId;
                    tariffsSummary.MinPrice = minprice;
                    byte[] filedata = DownloadFile(airline.ImageDetailId, "jpg", tenant, "images");
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
            if (entityPM.TypeCode == "ASC" || entityPM.TypeCode == "OSC" || entityPM.TypeCode == "OFS")
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
	 