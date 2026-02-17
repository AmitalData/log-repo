using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{
    public partial class DirectQueryService
    {
        public Direct DirectCustomDataMapping(ShipmentPM MyEntityPM, int Tenant)
        {
            try
            {
                var temp = DirectDataMapping(MyEntityPM, Tenant);
                return temp;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetShipmentIdByNumber(string shipmentNumber, int tenant)
        {
            return query.GetEntitiyIdByShipmentNumber(shipmentNumber, tenant);
        }

        public ShipmentPM DirectCustomDataMappingAndValidatin(Direct MyEntity, int Tenant, string ComputingPartnerCode = "")
        {
            try
            {
                TenantQuery tenantQuery = new TenantQuery(Tenant);
                UserQuery userQuery = new UserQuery(Tenant);
                TenantPM MyTenantPM = tenantQuery.GetSinglePM(Tenant);
                UserPM MyUserPM = userQuery.GetSingleUserPMByEmail("system@tenant" + Tenant + ".com", Tenant, false);

                ShipmentPM temp =DirectDataMappingAndValidatin(MyEntity, Tenant, ComputingPartnerCode);
                temp.NewConcurrencyGUID = Guid.NewGuid().ToString();
                temp.Tenant = Tenant;
                temp.ShipmentLevelCode = "D";
                temp.MainCarriageFromPortId = temp.FromPortId;
                temp.MainCarriageToPortId = temp.ToPortId;
                temp.FHLStatusCode = "NSEN";
                temp.FWBStatusCode = "NSEN";
                temp.FHLStatusName = "Not Sent";
                temp.FWBStatusName = "Not Sent";
                temp.ManifestStatusCode = "NSEN";
                temp.LocalCustomsTransmissionsStatusCode = "NSEN";
                temp.CreateDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant);
                temp.LastUpdateDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                temp.StatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                temp.AWBCurrencyId =  MyTenantPM.FreightCurrencyId;
                temp.ProfitCurrencyId = MyTenantPM.ProfitCurrencyId;
                temp.Master = MyEntity.Master;
                temp.OnCarriageAdditionalTransportModeCode = "BYTR";

                if (string.IsNullOrEmpty(temp.VolumeUnitCode))
                {
                    temp.VolumeUnitCode = MyTenantPM.VolumeUnitCode;
                }

                if (string.IsNullOrEmpty(temp.DimensionsUnitCode))
                {
                    temp.DimensionsUnitCode = MyTenantPM.DimensionsUnitCode;
                }

                if (string.IsNullOrEmpty(temp.GrossWeightUnitCode))
                {
                    temp.GrossWeightUnitCode = MyTenantPM.GrossWeightUnitCode;
                }

                if (string.IsNullOrEmpty(temp.ChargeableWeightUnitCode))
                {
                    temp.ChargeableWeightUnitCode = MyTenantPM.ChargeableWeightUnitCode;
                }
                
                if (string.IsNullOrEmpty(temp.ValueOfGoodsCurrencyId))
                {
                    temp.ValueOfGoodsCurrencyId = MyTenantPM.FreightCurrencyId;
                }

                switch (temp.DirectionId)
                {
                    case "E":
                    case "R":
                        {
                            temp.FreightPrepaidCollectId = MyTenantPM.ExportFreightPrepaidCollectId;
                            temp.OtherPrepaidCollectId = MyTenantPM.ExportOtherPrepaidCollectId;
                            break;
                        }

                    case "I":
                        {
                            temp.FreightPrepaidCollectId = MyTenantPM.ImportFreightPrepaidCollectId;
                            temp.OtherPrepaidCollectId = MyTenantPM.ImportOtherPrepaidCollectId;
                            break;
                        }

                    case "D":
                        {
                            temp.FreightPrepaidCollectId = "P";
                            temp.OtherPrepaidCollectId = "P";

                            if (!string.IsNullOrEmpty(MyTenantPM.CountryCode))
                            {
                                if (MyTenantPM.CountryCode.ToUpper() == "US")
                                {
                                    temp.DimensionsUnitCode = "Inc";
                                    temp.VolumeUnitCode = "CBI";
                                    temp.GrossWeightUnitCode = "LB";
                                    temp.ChargeableWeightUnitCode = "LB";
                                }
                            }

                            break;
                        }
                }

                temp.Ratio = this.GetRatio(temp.DirectionId, temp.TransportModeId, temp.ShipmentTypeId, MyTenantPM.CountryCode);
                temp.DimFactor = this.GetDimFactorFromRatio(temp.Ratio, temp.DimensionsUnitCode, temp.ChargeableWeightUnitCode);

                if (string.IsNullOrEmpty(temp.CreatedByUserId))
                {
                    temp.CreatedByUserId = MyUserPM.Id;
                }

                if (string.IsNullOrEmpty(temp.UpdatedByUserId))
                {
                    temp.UpdatedByUserId = MyUserPM.Id;
                }

                if (string.IsNullOrEmpty(temp.BranchId))
                {
                    temp.BranchId = MyUserPM.BranchId;
                }

                if (string.IsNullOrEmpty(temp.DepartmentId))
                {
                    temp.DepartmentId = MyUserPM.DepartmentId;
                }

                //if (string.IsNullOrEmpty(temp.MainCarriageCarrierId))
                //{
                //    this.MainCarriageCarrierChanged(temp, null);

                //    if (temp.TransportModeId == "A")
                //    {
                //        if (string.IsNullOrEmpty(temp.InterlineId))
                //        {
                //            temp.CarrierIsCheckDigit = false;
                //            temp.CarrierIsLimitedLength = false;
                //            temp.AirlinePrefix = null;
                //        }

                //        temp.CarrierIsChampRegistered = false;
                //        temp.CarrierIsGLSHKRegistered = false;
                //        MapTenantZeroAirline(temp, null);
                //    }
                //}
                //else
                //{
                //    ICommonDataContext MyContext = CommonDataContext.GetContext(Tenant);
                //    CardRepository cardRepository = new CardRepository(MyContext);
                //    CardList list = null;
                //    Card entityPoco = cardRepository.GetSingleCard(temp.MainCarriageCarrierId, Tenant);

                //    if (entityPoco != null)
                //    {
                //        CardQuery cardQuery = new CardQuery(cardRepository);
                //        list = cardQuery.GetSingleCardList(entityPoco);
                //    }

                //    if (list != null)
                //    {
                //        if (temp.TransportModeId == "A")
                //        {
                //            temp.MainCarriageCarrierPrefix = list.Code;
                //        }

                //        MainCarriageCarrierChanged(temp, list);

                //        if (temp.TransportModeId == "A")
                //        {
                //            AirlineRepository airlineRepository = new AirlineRepository(MyContext);
                //            AirlineList entityList = null;
                //            Airline airLine = airlineRepository.GetSingleAirline(temp.MainCarriageCarrierId, Tenant);

                //            if (airLine != null)
                //            {
                //                temp.MainCarriageCarrierCode = airLine.Card.Code;
                //                List<Airline> singleEntityList = new List<Airline>();
                //                singleEntityList.Add(airLine);

                //                AirlineQuery airlineQuery = new AirlineQuery(airlineRepository);
                //                IQueryable<Airline> iQueryable = singleEntityList.AsQueryable();
                //                IQueryable<AirlineList> iQueryableEntityList = airlineQuery.GetIQueryableEntityList(iQueryable);
                //                entityList = iQueryableEntityList.FirstOrDefault();
                //            }

                //            if (entityList != null)
                //            {
                //                if (string.IsNullOrEmpty(temp.InterlineId))
                //                {
                //                    temp.CarrierIsCheckDigit = entityList.CheckDigit;
                //                    temp.CarrierIsLimitedLength = entityList.LimitedLength;

                //                    string myPrefix = null;
                //                    if (!string.IsNullOrEmpty(entityList.Prefix))
                //                    {
                //                        myPrefix = entityList.Prefix.ToString().Trim();
                //                        myPrefix = PadLeft(myPrefix, 3, "0");
                //                    }

                //                    temp.AirlinePrefix = myPrefix;
                //                }

                //                temp.CarrierIsChampRegistered = entityList.IsChampRegistered;
                //                temp.CarrierIsGLSHKRegistered = entityList.IsGLSHKRegistered;
                //                AirlineQuery entityQuery = new AirlineQuery(Tenant);
                //                AirlinePM myResult = entityQuery.GetSinglePMByCode(entityList.Code, Tenant);

                //                MapTenantZeroAirline(temp, myResult);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        throw new ApplicationException("Invalid MainCarriageCarrier Code");
                //    }

                //    string errorMaster=  ValidateMasterField(temp);
                //    if (!string.IsNullOrEmpty(errorMaster))
                //    {
                //        temp.Master = null;
                //        throw new ApplicationException(errorMaster);
                //    }
                //    else
                //    {
                //        ValidateMasterStack(temp, Tenant);
                //    }                  
                //}

                foreach (ShipmentPickUpPM item in temp.ShipmentPickUps)
                {
                    item.PickUpDeliveryTypeCode = "PICK";

                    if (!string.IsNullOrEmpty(item.FromPartnerCardId))
                    {
                        item.PickUpDeliveryFromTypeCode = "PART";
                    }
                    else if (!string.IsNullOrEmpty(item.FromPortId))
                    {
                        item.PickUpDeliveryFromTypeCode = "PORT";
                    }

                    if (!string.IsNullOrEmpty(item.ToPartnerCardId))
                    {
                        item.PickUpDeliveryToTypeCode = "PART";
                    }
                    else if (!string.IsNullOrEmpty(item.ToPortId))
                    {
                        item.PickUpDeliveryToTypeCode = "PORT";
                    }
                }

                foreach (ShipmentDeliveryPM item in temp.ShipmentDeliveries)
                {
                    item.PickUpDeliveryTypeCode = "DELV";

                    if (!string.IsNullOrEmpty(item.FromPartnerCardId))
                    {
                        item.PickUpDeliveryFromTypeCode = "PART";
                    }
                    else if (!string.IsNullOrEmpty(item.FromPortId))
                    {
                        item.PickUpDeliveryFromTypeCode = "PORT";
                    }

                    if (!string.IsNullOrEmpty(item.ToPartnerCardId))
                    {
                        item.PickUpDeliveryToTypeCode = "PART";
                    }
                    else if (!string.IsNullOrEmpty(item.ToPortId))
                    {
                        item.PickUpDeliveryToTypeCode = "PORT";
                    }
                }

                return temp;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private void ValidateMasterStack(ShipmentPM temp, int Tenant)
        //{
        //    if (temp.TransportModeId == "A")
        //    {
        //        if (temp != null)
        //        {
        //            if (!string.IsNullOrEmpty(temp.Master))
        //            {
        //                if (temp.Master.Length == 8 && !temp.MainCarriageIsFromStack && !temp.MAWBTakenFromStack)
        //                {
        //                    if (IsNumeric(temp.Master))
        //                    {
        //                        MAWBStackQuery entityQuery = new MAWBStackQuery(Tenant);
        //                        MAWBStackPM myStackPM = entityQuery.GetSingleMAWBStackPMByNumber(int.Parse(temp.Master), Tenant);
        //                        if (myStackPM != null)
        //                        {
        //                            string myAirlineId = temp.MainCarriageCarrierId;
        //                            if (!string.IsNullOrEmpty(temp.InterlineId))
        //                            {
        //                                myAirlineId = temp.InterlineId;
        //                            }

        //                            if (myStackPM.AirlineId == myAirlineId)
        //                            {
        //                                if (!string.IsNullOrEmpty(myStackPM.AssignedToId) && myStackPM.AssignedToId != temp.ShipperId)
        //                                {
        //                                    temp.Master = null;
        //                                }

        //                                else
        //                                {
        //                                    temp.MAWBTakenFromStack = true;
        //                                    temp.MAWBStackNumber = PadLeft(myStackPM.Number.ToString(), 8, "0");
        //                                    temp.MAWBOBLDate = DateTime.UtcNow;
        //                                    SetMAWBAirline(temp);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //private void SetMAWBAirline(ShipmentPM temp)
        //{
        //    if (temp.MAWBStackAirlineId != temp.MainCarriageCarrierId)
        //    {
        //        temp.MAWBStackAirlineId = temp.MainCarriageCarrierId;
        //    }

        //    if (!string.IsNullOrEmpty(temp.InterlineId))
        //    {
        //        if (temp.MAWBStackAirlineId != temp.InterlineId)
        //        {
        //            temp.MAWBStackAirlineId = temp.InterlineId;
        //        }
        //    }
        //}

        //private string ValidateMasterField(ShipmentPM entity)
        //{
        //    if (entity.TransportModeId == "A")
        //    {              
        //        if(!string.IsNullOrEmpty(entity.Master))
        //        {
        //            string myResult  = ValidateMasterField(entity.Master, entity.TransportModeId, entity.CarrierIsCheckDigit, entity.CarrierIsLimitedLength);
        //            return myResult;                   
        //        }
        //    }

        //    return null;
        //}

        //public bool IsNumeric(string input)
        //{
        //    var myResult = true;

        //    if (!string.IsNullOrEmpty(input))
        //    {
        //        input = input.Trim().ToUpper();
        //        Regex regix = new Regex("^[0-9]*$", RegexOptions.IgnoreCase);

        //        if (!regix.IsMatch(input))
        //            myResult = false;
        //    }

        //    return myResult;
        //}

        //public string ValidateMasterField(string myMasterField, string myTransportModeId, bool isCheckDigit,bool isLimitedLength)
        //{
        //    var myResult = "";

        //    if (!string.IsNullOrEmpty(myMasterField) && myTransportModeId == "A")
        //    {
        //        if (!IsNumeric(myMasterField))
        //        {
        //            myResult = "Master Field must be all digits";
        //        }

        //        else
        //        {
        //            if (isLimitedLength && myMasterField.Length != 8)
        //            {
        //                myResult = "Master Field length must be 8 digits";
        //            }

        //            else
        //            {
        //                if (isCheckDigit)
        //                {
        //                    string myPrefix  = myMasterField.Substring(0, 7);
        //                    string myCheckDegit  = myMasterField.Substring(7, 1);

        //                    int myPrefixInteger = int.Parse(myPrefix);


        //                    int  myMod = myPrefixInteger % 7;

        //                    if (myMod >= 7)
        //                    {
        //                        myMod = myMod % 7;
        //                    }

        //                    if (myMod.ToString() != myCheckDegit)
        //                    {
        //                        myResult = "Master Field invalid check digit";
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return myResult;
        //}

        //public string PadLeft(string myString,int myCount,string myChar)
        //{
        //    if (myCount==0)
        //    {
        //        myCount = 0;
        //    }

        //    if (string.IsNullOrEmpty(myChar))
        //    {
        //        myChar = "";
        //    }

        //    if (string.IsNullOrEmpty(myString))
        //    {
        //        myString = "";
        //    }

        //    while (myString.Length < myCount)
        //    {
        //        myString = myChar + myString;
        //    }

        //    return myString;
        //}

        //public static void MapTenantZeroAirline(ShipmentPM entityPM,AirlinePM myAirlinePM)
        //{
        //    if (myAirlinePM == null)
        //    {
        //        if (entityPM.TenantZeroAirlineId != null)
        //        {
        //            entityPM.TenantZeroAirlineId = null;
        //        }

        //        if (entityPM.TenantZeroAirlineTTY != null)
        //        {
        //            entityPM.TenantZeroAirlineTTY = null;
        //        }

        //        if (entityPM.TenantZeroAirlinePIMA != null)
        //        {
        //            entityPM.TenantZeroAirlinePIMA = null;
        //        }

        //        if (entityPM.TenantZeroAirlineChampFWB != false)
        //        {
        //            entityPM.TenantZeroAirlineChampFWB = false;
        //        }

        //        if (entityPM.TenantZeroAirlineChampFHL != false)
        //        {
        //            entityPM.TenantZeroAirlineChampFHL = false;
        //        }

        //        if (entityPM.TenantZeroAirlineChampFSU != false)
        //        {
        //            entityPM.TenantZeroAirlineChampFSU = false;
        //        }

        //        if (entityPM.TenantZeroAirlineChampFVRFVA != false)
        //        {
        //            entityPM.TenantZeroAirlineChampFVRFVA = false;
        //        }

        //        if (entityPM.TenantZeroAirlineChampFSRFSA != false)
        //        {
        //            entityPM.TenantZeroAirlineChampFSRFSA = false;
        //        }

        //        if (entityPM.TenantZeroAirlineChampNeedsRegistration != false)
        //        {
        //            entityPM.TenantZeroAirlineChampNeedsRegistration = false;
        //        }

        //        if (entityPM.TenantZeroAirlineGLSHKFWB != false)
        //        {
        //            entityPM.TenantZeroAirlineGLSHKFWB = false;
        //        }

        //        if (entityPM.TenantZeroAirlineGLSHKFHL != false)
        //        {
        //            entityPM.TenantZeroAirlineGLSHKFHL = false;
        //        }

        //        if (entityPM.TenantZeroAirlineGLSHKFSU != false)
        //        {
        //            entityPM.TenantZeroAirlineGLSHKFSU = false;
        //        }

        //        if (entityPM.TenantZeroAirlineGLSHKFVRFVA != false)
        //        {
        //            entityPM.TenantZeroAirlineGLSHKFVRFVA = false;
        //        }

        //        if (entityPM.TenantZeroAirlineGLSHKFSRFSA != false)
        //        {
        //            entityPM.TenantZeroAirlineGLSHKFSRFSA = false;
        //        }

        //        if (entityPM.TenantZeroAirlineGLSHKNeedsRegistration != false)
        //        {
        //            entityPM.TenantZeroAirlineGLSHKNeedsRegistration = false;
        //        }
        //    }

        //    else
        //    {
        //        if (entityPM.TenantZeroAirlineId != myAirlinePM.Id)
        //        {
        //            entityPM.TenantZeroAirlineId = myAirlinePM.Id;
        //        }

        //        if (entityPM.TenantZeroAirlineTTY != myAirlinePM.TTY)
        //        {
        //            entityPM.TenantZeroAirlineTTY = myAirlinePM.TTY;
        //        }

        //        if (entityPM.TenantZeroAirlinePIMA != myAirlinePM.GLSHKPIMA)
        //        {
        //            entityPM.TenantZeroAirlinePIMA = myAirlinePM.GLSHKPIMA;
        //        }

        //        if (entityPM.TenantZeroAirlineChampFWB != myAirlinePM.ChampFWB)
        //        {
        //            entityPM.TenantZeroAirlineChampFWB = myAirlinePM.ChampFWB;
        //        }

        //        if (entityPM.TenantZeroAirlineChampFHL != myAirlinePM.ChampFHL)
        //        {
        //            entityPM.TenantZeroAirlineChampFHL = myAirlinePM.ChampFHL;
        //        }

        //        if (entityPM.TenantZeroAirlineChampFSU != myAirlinePM.ChampFSU)
        //        {
        //            entityPM.TenantZeroAirlineChampFSU = myAirlinePM.ChampFSU;
        //        }

        //        if (entityPM.TenantZeroAirlineChampFVRFVA != myAirlinePM.ChampFVRFVA)
        //        {
        //            entityPM.TenantZeroAirlineChampFVRFVA = myAirlinePM.ChampFVRFVA;
        //        }

        //        if (entityPM.TenantZeroAirlineChampFSRFSA != myAirlinePM.ChampFSRFSA)
        //        {
        //            entityPM.TenantZeroAirlineChampFSRFSA = myAirlinePM.ChampFSRFSA;
        //        }

        //        if (entityPM.TenantZeroAirlineChampNeedsRegistration != myAirlinePM.ChampNeedsRegistration)
        //        {
        //            entityPM.TenantZeroAirlineChampNeedsRegistration = myAirlinePM.ChampNeedsRegistration;
        //        }

        //        if (entityPM.TenantZeroAirlineGLSHKFWB != myAirlinePM.GLSHKFWB)
        //        {
        //            entityPM.TenantZeroAirlineGLSHKFWB = myAirlinePM.GLSHKFWB;
        //        }

        //        if (entityPM.TenantZeroAirlineGLSHKFHL != myAirlinePM.GLSHKFHL)
        //        {
        //            entityPM.TenantZeroAirlineGLSHKFHL = myAirlinePM.GLSHKFHL;
        //        }

        //        if (entityPM.TenantZeroAirlineGLSHKFSU != myAirlinePM.GLSHKFSU)
        //        {
        //            entityPM.TenantZeroAirlineGLSHKFSU = myAirlinePM.GLSHKFSU;
        //        }

        //        if (entityPM.TenantZeroAirlineGLSHKFVRFVA != myAirlinePM.GLSHKFVRFVA)
        //        {
        //            entityPM.TenantZeroAirlineGLSHKFVRFVA = myAirlinePM.GLSHKFVRFVA;
        //        }

        //        if (entityPM.TenantZeroAirlineGLSHKFSRFSA != myAirlinePM.GLSHKFSRFSA)
        //        {
        //            entityPM.TenantZeroAirlineGLSHKFSRFSA = myAirlinePM.GLSHKFSRFSA;
        //        }

        //        if (entityPM.TenantZeroAirlineGLSHKNeedsRegistration != myAirlinePM.GLSHKNeedsRegistration)
        //        {
        //            entityPM.TenantZeroAirlineGLSHKNeedsRegistration = myAirlinePM.GLSHKNeedsRegistration;
        //        }
        //    }
        //}

        //private void MainCarriageCarrierChanged(ShipmentPM entityPM, CardList list)
        //{
        //    string myCardCode = null;
        //    string myCardName  = null;
        //    string myCardPrefix = null;
        //    string myCardWebSite = null;

        //    if (list == null)
        //    {
        //        //if (entityPM.MainCarriageCarrierNumber != null)
        //        //{
        //        //    entityPM.MainCarriageCarrierNumber = null;
        //        //}

        //        //if (!string.IsNullOrEmpty(entityPM.Master))
        //        //{
        //        //    entityPM.Master = null;
        //        //}
        //    }

        //    else
        //    {
        //        myCardCode = list.Code;
        //        myCardName = list.EnglishName;
        //        myCardWebSite = list.WebSite;

        //        if (entityPM.TransportModeId == "A")
        //        {
        //            myCardPrefix = list.Code;
        //        }
        //    }

        //    if (entityPM.MainCarriageCarrierCode != myCardCode)
        //    {
        //        entityPM.MainCarriageCarrierCode = myCardCode;
        //    }

        //    if (entityPM.MainCarriageCarrierName != myCardName)
        //    {
        //        entityPM.MainCarriageCarrierName = myCardName;
        //    }

        //    if (entityPM.MainCarriageCarrierPrefix != myCardPrefix)
        //    {
        //        entityPM.MainCarriageCarrierPrefix = myCardPrefix;
        //    }

        //    if (entityPM.MainCarriageCarrierWebSite != myCardWebSite)
        //    {
        //        entityPM.MainCarriageCarrierWebSite = myCardWebSite;
        //    }
        //}

        private double? GetRatio(string directionId, string transportModeId, string shipmentTypeId, string countryCode)
        {
            double? myResult = null;

            if (!string.IsNullOrEmpty(countryCode))
            {
                if (countryCode.ToUpper() == "US")
                {
                    if (directionId == "D")
                    {
                        if (transportModeId == "A")
                        {
                            myResult = 7;
                        }

                        else if (transportModeId == "I")
                        {
                            if (shipmentTypeId == "LTL")
                            {
                                myResult = 9;
                            }
                        }
                    }
                }
            }

            if (myResult == null)
            {
                {
                    switch (transportModeId)
                    {
                        case "A": { myResult = 6; break; }
                        case "O": { myResult = 1; break; }
                        case "I":
                            {
                                if (shipmentTypeId == "LTL")
                                {
                                    myResult = 3.3;
                                }

                                else
                                {
                                    myResult = 1;
                                }

                                break;
                            }
                        default:
                            break;
                    }
                }
            }

            return myResult;
        }
        private double? GetDimFactorFromRatio(double? ratio, string dimentionCode, string weightCode)
        {
            double? myRatio = null;

            if (ratio != null)
            {
                myRatio = Convert.ToDouble(ratio);
            }

            double? myResult = null;

            if (myRatio != null)
            {
                double WeightFactorOfConvert = 1;
                double DimensiosFactorOfConvert = 1;

                if (!string.IsNullOrEmpty(weightCode))
                {
                    switch (weightCode.ToUpper())
                    {
                        case "KG": { WeightFactorOfConvert = 1; break; }
                        case "LB": { WeightFactorOfConvert = 0.45359237; break; }
                        case "MT": { WeightFactorOfConvert = 1000; break; }
                    }
                }

                if (!string.IsNullOrEmpty(dimentionCode))
                {
                    switch (dimentionCode.ToUpper())
                    {
                        case "CM": { DimensiosFactorOfConvert = 1; break; }
                        case "INC": { DimensiosFactorOfConvert = 2.54; break; }
                        case "FT": { DimensiosFactorOfConvert = 30.48; break; }
                    }
                }

                myResult = WeightFactorOfConvert * 1000 * myRatio / Math.Pow(DimensiosFactorOfConvert, 3);
            }

            if (myResult != null)
            {
                string toString = myResult.ToString();
                string[] myArray = toString.Split('.');

                if (myArray.Length > 1)
                {
                    string strDigits = "0." + myArray[1];
                    double? digits = Convert.ToDouble(strDigits);

                    if (digits < 0.5)
                    {
                        myResult = Math.Floor(myResult.Value);
                    }

                    else
                    {
                        myResult = Math.Ceiling(myResult.Value);
                    }
                }
            }

            return myResult;
        }
    }
}