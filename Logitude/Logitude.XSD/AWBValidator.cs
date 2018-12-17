using Logitude.Server.Tools.Helpers;
using Logitude.XSD.DataContracts;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.XSD
{
    public class AWBValidator
    {
        private int tenant;
        private bool isFHL;
        private bool isFWB;
        private Shipment myEntityPOCO;
        private ShipmentMasterData myMasterData;
        private AWBResultClass myAWBResultClass;
        private IShipmentsContext myShipmentContext;
        private ShipmentRepository myShipmentRepository;
        private ICommonDataContext myCommonContext;
        private AddressRepository myAddressRepository;
        public AWBValidator(int tenant)
        {
            this.tenant = tenant;
            this.myCommonContext = CommonDataContext.GetContext(tenant);
            this.myAddressRepository = new AddressRepository(myCommonContext);
            this.myShipmentContext= ShipmentsContext.GetContext(tenant);
            this.myShipmentRepository = new ShipmentRepository(myShipmentContext);
        }

        public AWBResultClass GetSendingValidating(string shipmentId, string myRecipient, bool isSendingFHLs, bool isSendingCargonaut, bool isSendingDEXX, string mainCarriageCarrierId)
        {
            myAWBResultClass = new AWBResultClass()
            {
                Id = tenant,
                Tenant = tenant,
                ShipmentId = shipmentId,
                IsSendingFHLs = isSendingFHLs,
                IsDEXXSending = isSendingDEXX,
                IsCargonautSending = isSendingCargonaut,
                Recipient = myRecipient,
                IsAWBStockPrepaid = false,
                IsValid = true,
                IsDemoTenant = false,
                IsUpgradingChamp = false,
            };

            if (myAWBResultClass.IsCargonautEnabled && myAWBResultClass.IsCargonautSending)
            {
                // TTY (Old)
                //myAWBResultClass.Recipient = "REUCGNP";
                
                // PIMA (New)
                myAWBResultClass.Recipient = "CGNCCS88CGN";                
            }

            else if (myAWBResultClass.IsDEXXEnabled && myAWBResultClass.IsDEXXSending)
            {
                // TTY (Old)
                //myAWBResultClass.Recipient = "REUBCSP";

                // PIMA (New)
                myAWBResultClass.Recipient = "BCSSYS03AWBCPY";
            }

            this.GetStockTypeCodes();
            this.GetGlobalVariables();

            if (this.myAWBResultClass.IsValid)
            {
                this.GetShipmentObjects();
                this.CheckDemoTenantData();
                this.CheckStockValidity();
                this.Validate();
            }

            return myAWBResultClass;
        }

        private void GetStockTypeCodes()
        {
            string myFHLCode = "FHL";
            string myFWBCode = "FWB";

            if (myAWBResultClass.IsDEXXSending)
            {
                myFHLCode = "FHL DEXX";
                myFWBCode = "FWB DEXX";
            }

            else if (myAWBResultClass.IsCargonautSending)
            {
                myFHLCode = "FHL Cargonaut";
                myFWBCode = "FWB Cargonaut";
            }

            myAWBResultClass.StockFHLCode = myFHLCode;
            myAWBResultClass.StockFWBCode = myFWBCode;
        }
        private void GetGlobalVariables()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                if (this.tenant != 290)
                {
                    SettingRepository settingRepository = new SettingRepository();
                    Setting setting = settingRepository.GetSingleSetting("1");
                    if (setting != null)
                    {
                        if (setting.IsUpgradingChamp)
                        {
                            this.myAWBResultClass.IsValid = false;
                            this.myAWBResultClass.IsUpgradingChamp = setting.IsUpgradingChamp;
                        }
                    }
                }

                if (this.myAWBResultClass.IsValid)
                {
                    TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                    TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(myAWBResultClass.Tenant);

                    if (tenantManagement != null)
                    {
                        myAWBResultClass.TTY = tenantManagement.TTY;
                        myAWBResultClass.PIMA = tenantManagement.PIMA;
                        myAWBResultClass.IsAWBStockPrepaid = tenantManagement.IsAWBStockPrepaid;
                        myAWBResultClass.IsCargonautEnabled = tenantManagement.IsCargonautEnabled;
                        myAWBResultClass.IsDEXXEnabled = tenantManagement.IsDEXXConnectionEnabled;
                        myAWBResultClass.AWBMessagesCCSTypeCode = tenantManagement.AWBMessagesCCSTypeCode;
                        myAWBResultClass.IsEAWBOnlyDemo = tenantManagement.IsEAWBOnlyDemo;
                    }
                }

                scope.Complete();
            }
        }
        private void GetShipmentObjects()
        {
            myEntityPOCO = myShipmentRepository.GetSingleShipment(myAWBResultClass.ShipmentId, myAWBResultClass.Tenant);

            if (myEntityPOCO != null)
            {
                ShipmentMasterDataRepository shipmentMasterDataRepository = new ShipmentMasterDataRepository(myShipmentContext);
                myMasterData = shipmentMasterDataRepository.GetSingleMasterData(myEntityPOCO.MasterShipmentDataId);

                this.isFHL = myEntityPOCO.ShipmentLevelCode == "H" ? true : false;
                this.isFWB = !isFHL;

                if (myEntityPOCO.ShipmentLevelCode == "C" && myAWBResultClass.IsSendingFHLs)
                {
                    List<Shipment> allMasterHouses = myShipmentRepository.GetHouseShipmentsForMaster(myAWBResultClass.ShipmentId, myAWBResultClass.Tenant);

                    List<FHLShipmentValidator> validFHLsList = this.GetFHLsValidation(myAWBResultClass.ShipmentId);

                    foreach (Shipment item in allMasterHouses.OrderBy(o => o.ShipmentNumber))
                    {
                        if (validFHLsList.Where(d => d.IsFHLValid && d.ShipmentId == item.Id).Any())
                        {
                            if (myAWBResultClass.ValidFHLsDataStringList == null)
                            {
                                myAWBResultClass.ValidFHLsDataStringList = new List<string>();
                            }

                            string myItemData = item.Id + ":" + item.ShipmentNumber;
                            myAWBResultClass.ValidFHLsDataStringList.Add(myItemData);
                        }
                    }

                    myAWBResultClass.AllHousesCount = allMasterHouses.Count;
                    myAWBResultClass.ValidHousesCount = myAWBResultClass.ValidFHLsDataStringList.Count;
                }
            }
        }
        private void CheckDemoTenantData()
        {
            if (myAWBResultClass.Tenant == 65 || myAWBResultClass.IsEAWBOnlyDemo)
            {
                myAWBResultClass.IsDemoTenant = true;
            }
        }
        private void CheckStockValidity()
        {
            if (!myAWBResultClass.IsDemoTenant)
            {
                if (myAWBResultClass.IsAWBStockPrepaid)
                {                    
                    DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(myAWBResultClass.Tenant).Date;
                    MessagingStockRepository stockRepository = new MessagingStockRepository(myShipmentContext);
                    MessagingStockUsageHistoryRepository usageHistoryRepository = new MessagingStockUsageHistoryRepository(myShipmentContext);
                    IQueryable<MessagingStock> myStocksData = stockRepository.GetMessagingStocksByTenant(myAWBResultClass.Tenant, "Champ");
                    IQueryable<MessagingStockUsageHistory> myUsageHistoryData = usageHistoryRepository.GetTenantMessagingStockUsageHistory(myAWBResultClass.Tenant);

                    myStocksData = myStocksData.Where(d => d.StartDate <= todayDate && d.EndDate > todayDate && d.Remaining > 0 && !d.IsCancelled);

                    int sendingCount = 0;
                    int? myStocksRemaining = 0;
                    if (myStocksData.Count() > 0)
                    {
                        myStocksRemaining = myStocksData.Sum(s => s.Remaining);
                    }

                    if (myEntityPOCO.ShipmentLevelCode == "H")
                    {
                        if (!myUsageHistoryData.Where(d => d.EntityId == myAWBResultClass.ShipmentId && d.MessageType == myAWBResultClass.StockFHLCode).Any())
                        {
                            sendingCount = 1;
                        }
                    }

                    else if (myEntityPOCO.ShipmentLevelCode == "C" && myAWBResultClass.IsSendingFHLs)
                    {
                        foreach (string itemDataString in myAWBResultClass.ValidFHLsDataStringList)
                        {
                            string[] itemStringArray = itemDataString.Split(':');

                            string itemId = itemStringArray[0];

                            if (!myUsageHistoryData.Where(d => d.EntityId == itemId && d.MessageType == myAWBResultClass.StockFHLCode).Any())
                            {
                                sendingCount += 1;
                            }
                        }
                    }

                    else
                    {
                        if (myEntityPOCO.BookingId != null)
                        {
                            if (!myUsageHistoryData.Where(d => d.EntityId == myEntityPOCO.BookingId && d.MessageType == "FFR").Any())
                            {
                                if (!myUsageHistoryData.Where(d => d.EntityId == myAWBResultClass.ShipmentId && d.MessageType == myAWBResultClass.StockFHLCode).Any())
                                {
                                    sendingCount = 1;
                                }
                            }
                        }

                        else
                        {
                            if (!myUsageHistoryData.Where(d => d.EntityId == myAWBResultClass.ShipmentId && d.MessageType == myAWBResultClass.StockFWBCode).Any())
                            {
                                sendingCount = 1;
                            }
                        }
                    }

                    myAWBResultClass.SendingCount = sendingCount;
                    myAWBResultClass.StockRemainingBefore = myStocksRemaining == null ? 0 : myStocksRemaining.Value;

                    if (sendingCount > 0)
                    {
                        if (myStocksRemaining < sendingCount)
                        {
                            myAWBResultClass.IsValid = false;
                            myAWBResultClass.HasStockErrors = true;
                        }
                    }
                }
            }
        }

        string msg = "";
        private void Validate()
        {
            this.msg = TranslateTextsClass.Translate("General.M.FieldIsRequired", tenant);

            if (string.IsNullOrEmpty(myAWBResultClass.Recipient))
            {
                myAWBResultClass.ErrorsList.Add("Recipient field is required");
            }

            if (!myAWBResultClass.IsEAWBOnlyDemo)
            {
                if (myAWBResultClass.AWBMessagesCCSTypeCode == "GLSHK")
                {
                    if (string.IsNullOrEmpty(myAWBResultClass.PIMA))
                    {
                        myAWBResultClass.ErrorsList.Add("PIMA field is required");
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(myAWBResultClass.TTY))
                    {
                        myAWBResultClass.ErrorsList.Add("TTY field is required");
                    }
                }
            }

            this.Validate_PAR();
            this.Validate_ROU();
            this.Validate_PAC();
            this.Validate_FRE();

            if (myAWBResultClass.ErrorsList.Count > 0)
            {
                myAWBResultClass.IsValid = false;
                myAWBResultClass.HasMainErrors = true;
            }
        }
        private void Validate_PAR()
        {
            #region Shipper
            if (string.IsNullOrEmpty(myEntityPOCO.ShipperId))
            {
                myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Shipper"));
            }

            else
            {
                if (!IsTextFormatted(myEntityPOCO.ShipperName))
                {
                    myAWBResultClass.ErrorsList.Add(GetWrongTextFormatMessage("Shipper Name"));
                }

                if (string.IsNullOrEmpty(myEntityPOCO.ShipperAddressId))
                {
                    myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Shipper Address"));
                }

                else
                {
                    Address address = myAddressRepository.GetSingleAddress(myEntityPOCO.ShipperAddressId, tenant);
                    if (address != null)
                    {
                        string myAddress1 = string.IsNullOrEmpty(address.Address1) ? null : address.Address1.Trim();
                        string myAddress2 = string.IsNullOrEmpty(address.Address2) ? null : address.Address2.Trim();
                        string myZipCode = string.IsNullOrEmpty(address.ZipCode) ? null : address.ZipCode.Trim();
                        string myCity = string.IsNullOrEmpty(address.City) ? null : address.City.Trim();

                        if (!IsTextFormatted(myAddress1))
                        {
                            myAWBResultClass.ErrorsList.Add(GetWrongTextFormatMessage("Shipper Address1"));
                        }

                        if (!IsTextFormatted(myAddress2))
                        {
                            myAWBResultClass.ErrorsList.Add(GetWrongTextFormatMessage("Shipper Address2"));
                        }

                        if (string.IsNullOrEmpty(myAddress1) && string.IsNullOrEmpty(myAddress2))
                        {
                            myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Shipper Address1 Or Address2"));
                        }

                        if (string.IsNullOrEmpty(myZipCode))
                        {
                            myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Shipper Zip Code"));
                        }

                        else if (!IsTextFormatted(myZipCode))
                        {
                            myAWBResultClass.ErrorsList.Add(GetWrongTextFormatMessage("Shipper Zip Code"));
                        }

                        if (string.IsNullOrEmpty(myCity))
                        {
                            myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Shipper City"));
                        }

                        else if (!IsTextFormatted(myCity))
                        {
                            myAWBResultClass.ErrorsList.Add(GetWrongTextFormatMessage("Shipper City"));
                        }

                        if (string.IsNullOrEmpty(address.StateId))
                        {
                            if (myCommonContext.States.Where(s => s.CountryId == address.CountryId).Any())
                            {
                                myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Shipper address state"));
                            }
                        }
                    }
                }
            }
            #endregion

            #region Consignee
            if (string.IsNullOrEmpty(myEntityPOCO.ConsigneeId))
            {
                myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Consignee"));
            }

            else
            {
                if (!IsTextFormatted(myEntityPOCO.ConsigneeName))
                {
                    myAWBResultClass.ErrorsList.Add(GetWrongTextFormatMessage("Consignee Name"));
                }

                if (string.IsNullOrEmpty(myEntityPOCO.ConsigneeAddressId))
                {
                    myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Consignee Address"));
                }

                else
                {
                    Address address = myAddressRepository.GetSingleAddress(myEntityPOCO.ConsigneeAddressId, tenant);
                    if (address != null)
                    {
                        string myAddress1 = string.IsNullOrEmpty(address.Address1) ? null : address.Address1.Trim();
                        string myAddress2 = string.IsNullOrEmpty(address.Address2) ? null : address.Address2.Trim();
                        string myZipCode = string.IsNullOrEmpty(address.ZipCode) ? null : address.ZipCode.Trim();
                        string myCity = string.IsNullOrEmpty(address.City) ? null : address.City.Trim();

                        if (!IsTextFormatted(myAddress1))
                        {
                            myAWBResultClass.ErrorsList.Add(GetWrongTextFormatMessage("Consignee Address1"));
                        }

                        if (!IsTextFormatted(myAddress2))
                        {
                            myAWBResultClass.ErrorsList.Add(GetWrongTextFormatMessage("Consignee Address2"));
                        }

                        if (string.IsNullOrEmpty(myAddress1) && string.IsNullOrEmpty(myAddress2))
                        {
                            myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Consignee Address1 Or Address2"));
                        }

                        if (string.IsNullOrEmpty(myZipCode))
                        {
                            myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Consignee Zip Code"));
                        }

                        else if (!IsTextFormatted(myZipCode))
                        {
                            myAWBResultClass.ErrorsList.Add(GetWrongTextFormatMessage("Consignee Zip Code"));
                        }

                        if (string.IsNullOrEmpty(myCity))
                        {
                            myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Consignee City"));
                        }

                        else if (!IsTextFormatted(myCity))
                        {
                            myAWBResultClass.ErrorsList.Add(GetWrongTextFormatMessage("Consignee City"));
                        }

                        if (string.IsNullOrEmpty(address.StateId))
                        {
                            if (myCommonContext.States.Where(s => s.CountryId == address.CountryId).Any())
                            {
                                myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Consignee address state"));
                            }
                        }
                    }
                }
            }
            #endregion

            #region Issuing Agent
            if (this.isFWB)
            {
                if (string.IsNullOrEmpty(myEntityPOCO.IssuingCarrierAgentId))
                {
                    myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Issuing Carrier Agent"));
                }

                else
                {
                    //if (!AWBUtilities.IsTextFormatted(myEntityPOCO.IssuingCarrierAgentName))
                    //{
                    //    screenWarningsPar.Add(new ValidationResult(AWBUtilities.GetWrongTextFormatMessage("Issuing Carrier Agent Name")));
                    //}

                    if (!string.IsNullOrEmpty(myEntityPOCO.IssuingCarrierIATACode))
                    {
                        if (!this.FormateValidate_IATACode(myEntityPOCO.IssuingCarrierIATACode))
                        {
                            string fieldName = TranslateTextsClass.Translate("Shipment.F.IssuingCarrierIATACode", tenant);
                            myAWBResultClass.ErrorsList.Add(fieldName + " wrong format: must be 7 numeric digits max");
                        }
                    }

                    if (!string.IsNullOrEmpty(myEntityPOCO.CASSCode))
                    {
                        if (!this.FormateValidate_CASSCode(myEntityPOCO.CASSCode))
                        {
                            string fieldName = TranslateTextsClass.Translate("Shipment.F.CASSCode", tenant);
                            myAWBResultClass.ErrorsList.Add(fieldName + " wrong format: must be 4 numeric digits max");
                        }
                    }

                    if (string.IsNullOrEmpty(myEntityPOCO.IssuingCarrierAddressId))
                    {
                        myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Issuing Carrier Agent Address"));
                    }

                    else
                    {
                        Address address = myAddressRepository.GetSingleAddress(myEntityPOCO.IssuingCarrierAddressId, tenant);
                        if (address != null)
                        {
                            string myCity = string.IsNullOrEmpty(address.City) ? null : address.City.Trim();

                            if (string.IsNullOrEmpty(myCity))
                            {
                                myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Issuing Carrier Agent City"));
                            }

                            else if (!this.IsTextFormatted(myCity))
                            {
                                myAWBResultClass.ErrorsList.Add(this.GetWrongTextFormatMessage("Issuing Carrier Agent City"));
                            }
                        }
                    }
                }

                if (myAWBResultClass.AWBMessagesCCSTypeCode == "GLSHK")
                {
                    if (myEntityPOCO.ViaColoader)
                    {
                        if (string.IsNullOrEmpty(myEntityPOCO.IssuingCarrierReference1))
                        {
                            myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Issuing Carrier Reference 1"));
                        }
                    }
                }
            }
            #endregion

            #region Notify1
            if (this.isFWB)
            {
                if (!string.IsNullOrEmpty(myEntityPOCO.Notify1Id))
                {
                    if (string.IsNullOrEmpty(myEntityPOCO.Notify1AddressId))
                    {
                        myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Notify1 Address"));
                    }

                    else
                    {
                        Address address = myAddressRepository.GetSingleAddress(myEntityPOCO.Notify1AddressId, tenant);
                        if (address != null)
                        {
                            string myAddress1 = string.IsNullOrEmpty(address.Address1) ? null : address.Address1.Trim();
                            string myAddress2 = string.IsNullOrEmpty(address.Address2) ? null : address.Address2.Trim();
                            string myZipCode = string.IsNullOrEmpty(address.ZipCode) ? null : address.ZipCode.Trim();
                            string myCity = string.IsNullOrEmpty(address.City) ? null : address.City.Trim();

                            if (!IsTextFormatted(myAddress1))
                            {
                                myAWBResultClass.ErrorsList.Add(GetWrongTextFormatMessage("Notify1 Address1"));
                            }

                            if (!IsTextFormatted(myAddress2))
                            {
                                myAWBResultClass.ErrorsList.Add(GetWrongTextFormatMessage("Notify1 Address2"));
                            }

                            if (string.IsNullOrEmpty(myAddress1) && string.IsNullOrEmpty(myAddress2))
                            {
                                myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Notify1 Address1 Or Address2"));
                            }

                            if (string.IsNullOrEmpty(myZipCode))
                            {
                                myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Notify1 Zip Code"));
                            }

                            else if (!IsTextFormatted(myZipCode))
                            {
                                myAWBResultClass.ErrorsList.Add(GetWrongTextFormatMessage("Notify1 Zip Code"));
                            }

                            if (string.IsNullOrEmpty(myCity))
                            {
                                myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Notify1 City"));
                            }

                            else if (!IsTextFormatted(myCity))
                            {
                                myAWBResultClass.ErrorsList.Add(GetWrongTextFormatMessage("Notify1 City"));
                            }

                            if (string.IsNullOrEmpty(address.StateId))
                            {
                                if (myCommonContext.States.Where(s => s.CountryId == address.CountryId).Any())
                                {
                                    myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Notify1 address state"));
                                }
                            }
                        }
                    }
                }
            }
            #endregion
        }
        private void Validate_ROU()
        {
            if (myMasterData != null)
            {
                if (this.isFWB)
                {
                    if (string.IsNullOrEmpty(myMasterData.MainCarriageFromPortId))
                    {
                        myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", TranslateTextsClass.Translate("Shipment.O.Routings.Departure", tenant)));
                    }

                    if (string.IsNullOrEmpty(myMasterData.MainCarriageToPortId))
                    {
                        myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", TranslateTextsClass.Translate("Shipment.O.Routings.Destination", tenant)));
                    }

                    if (string.IsNullOrEmpty(myMasterData.MainCarriageCarrierId))
                    {
                        myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", TranslateTextsClass.Translate("Shipment.O.Routings.Airline", tenant)));
                    }

                    else
                    {
                        string codePrefix = string.IsNullOrEmpty(myMasterData.MainCarriageCarrierPrefix) ? myMasterData.MainCarriageCarrierPrefix : myMasterData.MainCarriageCarrierPrefix.Trim();
                        if (string.IsNullOrEmpty(codePrefix))
                        {
                            myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Main Carriage Carrier Prefix"));
                        }

                        else if (codePrefix.Length != 2)
                        {
                            myAWBResultClass.ErrorsList.Add("Main Carriage Carrier Prefix length must be 2");
                        }
                    }

                    if (string.IsNullOrEmpty(myMasterData.MainCarriageCarrierNumber))
                    {
                        myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", TranslateTextsClass.Translate("Shipment.O.Routings.FlightNo", tenant)));
                    }

                    else
                    {
                        if (!this.FormateValidate_FlightNumber(myMasterData.MainCarriageCarrierNumber))
                        {
                            string fieldName = TranslateTextsClass.Translate("Shipment.O.Routings.FlightNo", tenant);
                            myAWBResultClass.ErrorsList.Add(fieldName + " wrong format: must be [3-4 numerics] Or [4 numerics plus 1 Alpha]");
                        }
                    }

                    if (string.IsNullOrEmpty(myMasterData.Master))
                    {
                        myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", TranslateTextsClass.Translate("Shipment.O.Routings.MAWB", tenant)));
                    }

                    if (myMasterData.MAWBOBLDate == null)
                    {
                        myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", TranslateTextsClass.Translate("Shipment.O.Routings.MAWBDate", tenant)));
                    }

                    if (myMasterData.MainCarriageETD == null)
                    {
                        myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Main Carriage ETD"));
                    }


                    if (!string.IsNullOrEmpty(myMasterData.Transshipment1FromPortId) && !string.IsNullOrEmpty(myMasterData.Transshipment1ToPortId))
                    {
                        if (string.IsNullOrEmpty(myMasterData.Transshipment1CarrierId))
                        {
                            myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Main carriage leg 2 carrier"));
                        }
                    }

                    if (!string.IsNullOrEmpty(myMasterData.Transshipment2FromPortId) && !string.IsNullOrEmpty(myMasterData.Transshipment2ToPortId))
                    {
                        if (string.IsNullOrEmpty(myMasterData.Transshipment2CarrierId))
                        {
                            myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "Main carriage leg 3 carrier"));
                        }
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(myMasterData.MainCarriageFromPortId))
                    {
                        myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", TranslateTextsClass.Translate("Shipment.O.Routings.Departure", tenant)));
                    }

                    if (string.IsNullOrEmpty(myMasterData.MainCarriageToPortId))
                    {
                        myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", TranslateTextsClass.Translate("Shipment.O.Routings.Destination", tenant)));
                    }

                    if (string.IsNullOrEmpty(myEntityPOCO.House))
                    {
                        myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", "House"));
                    }
                }
            }
        }
        private void Validate_PAC()
        {
            if (myEntityPOCO.GrossWeight == null || myEntityPOCO.GrossWeight == 0)
            {
                string msgField = TranslateTextsClass.Translate("Shipment.F.GrossWeight", tenant);
                msgField = msgField.Replace("%GrossWeightCode", myEntityPOCO.GrossWeightUnitCode);
                myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", msgField));
            }

            //if (!TenantContext.Current.tenant.AllowEAWBMoreThanTenPackages)
            //{
            //    string myError = AWBUtilities.ValidateAddedPackagesCount(shipmentPM);

            //    if (!string.IsNullOrEmpty(myError))
            //    {
            //        screenErrorsPac.Add(new ValidationResult(myError));
            //    }
            //}

            if (isFWB)
            {
                if (myEntityPOCO.ShipmentLevelCode == "C")
                {
                    //if (shipmentPM.MainCarriageCarrierCode == "AR")
                    //{
                    //    string myFieldName = TextCodeTranslator.Translate(targetEntityName + ".F.DescriptionOfGoods");

                    //    if (string.IsNullOrEmpty(shipmentPM.DescriptionOfGoods))
                    //    {
                    //        screenWarningsPac.Add(new ValidationResult(msg.Replace("%FieldName", myFieldName)));
                    //    }
                    //}
                }

                if (myEntityPOCO.ChargeableWeight == null || myEntityPOCO.ChargeableWeight == 0)
                {
                    string msgField = TranslateTextsClass.Translate("Shipment.F.ChargeableWeight", tenant);
                    msgField = msgField.Replace("%ChargWeightCode", myEntityPOCO.ChargeableWeightUnitCode);
                    myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", msgField));
                }

                if (myEntityPOCO.IsMultipleCommodities)
                {
                    //foreach (ShipmentCommodityPM item in shipmentPM.ShipmentCommodities)
                    //{
                    //    if (!string.IsNullOrEmpty(item.CommodityNumber))
                    //    {
                    //        if (!AWBUtilities.FormateValidate_CommodityNo(item.CommodityNumber))
                    //        {
                    //            string fieldName = TextCodeTranslator.Translate("ShipmentCommodity.F.CommodityNumber");
                    //            screenWarningsPac.Add(new ValidationResult(fieldName + " must be 4-7 numeric"));
                    //            break;
                    //        }
                    //    }

                    //    if (string.IsNullOrEmpty(item.RateClassCode))
                    //    {
                    //        screenWarningsPac.Add(new ValidationResult(msg.Replace("%FieldName", TextCodeTranslator.Translate("ShipmentCommodity.F.RateClassCode"))));
                    //    }

                    //    if (!shipmentPM.AsAgreedFreight)
                    //    {
                    //        string rateClassGroupCode = AWBUtilities.GetRateClassGroupCode(item.RateClassCode);

                    //        if (rateClassGroupCode != "S")
                    //        {
                    //            if (item.ChargeRate == null || item.ChargeRate == 0)
                    //            {
                    //                screenWarningsPac.Add(new ValidationResult(msg.Replace("%FieldName", TextCodeTranslator.Translate("ShipmentCommodity.F.ChargeRate"))));
                    //            }
                    //        }

                    //        if (item.ChargeAmount == null || item.ChargeAmount == 0)
                    //        {
                    //            screenWarningsPac.Add(new ValidationResult(msg.Replace("%FieldName", TextCodeTranslator.Translate("ShipmentCommodity.F.AWBChargeAmount"))));
                    //        }
                    //    }
                    //}
                }

                else
                {
                    //if (!string.IsNullOrEmpty(this.myMasterData.awbc .a .aw .AWBCommodityItemNumber))
                    //{
                    //    if (!AWBUtilities.FormateValidate_CommodityNo(shipmentPM.AWBCommodityItemNumber))
                    //    {
                    //        string fieldName = TextCodeTranslator.Translate(targetEntityName + ".F." + "AWBCommodityItemNumber");
                    //        screenWarningsPac.Add(new ValidationResult(fieldName + " must be 4-7 numeric"));
                    //    }
                    //}
                }
            }

            else if (isFHL)
            {
                if (string.IsNullOrEmpty(myEntityPOCO.DescriptionOfGoods))
                {
                    myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", TranslateTextsClass.Translate("Shipment.F.DescriptionOfGoods", tenant)));

                }
            }
        }
        private void Validate_FRE()
        {
            if (isFWB)
            {
                if (string.IsNullOrEmpty(myEntityPOCO.AWBCurrencyId))
                {
                    myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", TranslateTextsClass.Translate("Shipment.F.AWBCurrencyId", tenant)));
                }

                if (string.IsNullOrEmpty(myEntityPOCO.AWBChargesCodeCode))
                {
                    myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", TranslateTextsClass.Translate("Shipment.F.AWBChargesCodeCode", tenant)));
                }

                if (!myEntityPOCO.IsMultipleCommodities)
                {
                    ShipmentCommodity myShipmentCommodity = (from d in myShipmentContext.ShipmentCommodities
                                                             where d.ShipmentId == myEntityPOCO.Id && d.Tenant == tenant
                                                             orderby d.Id select d).FirstOrDefault();

                    if(myShipmentCommodity != null)
                    {
                        if (string.IsNullOrEmpty(myShipmentCommodity.RateClassCode))
                        {
                            myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", TranslateTextsClass.Translate("Shipment.F.RateClassCode", tenant)));
                        }

                        if (!myEntityPOCO.AsAgreedFreight)
                        {
                            string rateClassGroupCode = this.GetRateClassGroupCode(myShipmentCommodity.RateClassCode);

                            if (rateClassGroupCode != "S")
                            {
                                if (myShipmentCommodity.ChargeRate == null || myShipmentCommodity.ChargeRate == 0)
                                {
                                    myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", TranslateTextsClass.Translate("Shipment.F.AWBChargeRate", tenant)));
                                }
                            }

                            if (myShipmentCommodity.ChargeAmount == null || myShipmentCommodity.ChargeAmount == 0)
                            {
                                myAWBResultClass.ErrorsList.Add(msg.Replace("%FieldName", TranslateTextsClass.Translate("Shipment.F.AWBChargeAmount", tenant)));
                            }
                        }
                    }
                }
            }
        }

        private bool IsTextFormatted(string myText)
        {
            bool myResult = false;

            if (string.IsNullOrEmpty(myText))
            {
                myResult = true;
            }

            else
            {
                string textFormat = @"[^A-Z0-9\-\. ]*";

                myText = Regex.Replace(myText, textFormat, string.Empty, RegexOptions.IgnoreCase);

                if (!string.IsNullOrEmpty(myText))
                {
                    myResult = true;
                }
            }

            return myResult;
        }
        private bool FormateValidate_IATACode(string input)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(input))
            {
                Regex isInteger = new Regex("^[0-9]{0,7}$");
                if (isInteger.IsMatch(input))
                {
                    result = true;
                }
            }

            return result;
        }
        private bool FormateValidate_CASSCode(string input)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(input))
            {
                Regex isInteger = new Regex("^[0-9]{0,4}$");
                if (isInteger.IsMatch(input))
                {
                    result = true;
                }
            }

            return result;
        }
        private bool FormateValidate_FlightNumber(string input)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(input))
            {
                Regex isMatch1 = new Regex("^[0-9]{3,4}$");
                Regex isMatch2 = new Regex("^[0-9]{4}[A-Z]{1}$");

                if (isMatch1.IsMatch(input))
                {
                    result = true;
                }

                else if (isMatch2.IsMatch(input.ToUpper()))
                {
                    result = true;
                }
            }

            return result;

        }
        private string GetWrongTextFormatMessage(string fieldName)
        {
            string myResult = "Invalid format. Can contain [a-z/0-9/./-]";

            if (!string.IsNullOrEmpty(fieldName))
            {
                myResult = fieldName + " " + myResult;
            }

            return myResult;
        }
        private string GetRateClassGroupCode(string rateClassCode)
        {
            string code = "";

            switch (rateClassCode)
            {
                case "M":
                case "B":
                    {
                        code = "M";
                        break;
                    }

                case "R":
                case "S":
                case "X":
                case "Y":
                    {
                        code = "S";
                        break;
                    }

                case "C":
                case "E":
                case "K":
                case "N":
                case "P":
                case "Q":
                case "U":
                    {
                        code = "R";
                        break;
                    }

                default: { break; }
            }

            return code;
        }

        public List<FHLShipmentValidator> GetFHLsValidation(string masterId)
        {
            int id = 0;
            List<FHLShipmentValidator> myResult = new List<FHLShipmentValidator>();

            List<Shipment> allHouses = myShipmentRepository.GetHouseShipmentsForMaster(masterId, tenant);

            foreach (Shipment item in allHouses)
            {
                FHLShipmentValidator validator = new FHLShipmentValidator();
                validator.Id = id;
                validator.ShipmentId = item.Id;
                validator.ShipmentNumber = item.ShipmentNumber;
                validator.Shipper = item.ShipperCard == null ? null : item.ShipperCard.EnglishName;
                validator.FHLStatusCode = item.FHLStatusCode;
                validator.FHLStatusName = item.FHLStatus == null ? "" : item.FHLStatus.Name;
                validator.CargonautFHLStatusCode = item.CargonautFHLStatusCode;
                validator.CargonautFHLStatusName = item.CargonautFHLStatus == null ? "" : item.CargonautFHLStatus.Name;

                validator.FHLErrors = new List<string>();

                if (string.IsNullOrEmpty(item.ShipperId))
                {
                    validator.FHLErrors.Add("Shipper field is required");
                }

                else
                {
                    if (string.IsNullOrEmpty(item.ShipperAddressId))
                    {
                        validator.FHLErrors.Add("Shipper Address field is required");
                    }

                    else
                    {
                        Address address = myAddressRepository.GetSingleAddress(item.ShipperAddressId, tenant);
                        if (address != null)
                        {
                            if (string.IsNullOrEmpty(address.Address1) && string.IsNullOrEmpty(address.Address2))
                            {
                                validator.FHLErrors.Add("Shipper Address1 Or Address2 is required");
                            }

                            if (string.IsNullOrEmpty(address.StateId))
                            {
                                if (myCommonContext.States.Where(s => s.CountryId == address.CountryId).Any())
                                {
                                    validator.FHLErrors.Add("Shipper address state is required");
                                }
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(item.ConsigneeId))
                {
                    validator.FHLErrors.Add("Consignee field is required");
                }

                else
                {
                    if (string.IsNullOrEmpty(item.ConsigneeAddressId))
                    {
                        validator.FHLErrors.Add("Consignee Address field is required");
                    }

                    else
                    {
                        Address address = myAddressRepository.GetSingleAddress(item.ConsigneeAddressId, tenant);
                        if (address != null)
                        {
                            if (string.IsNullOrEmpty(address.Address1) && string.IsNullOrEmpty(address.Address2))
                            {
                                validator.FHLErrors.Add("Consignee Address1 Or Address2 is required");
                            }

                            if (string.IsNullOrEmpty(address.StateId))
                            {
                                if (myCommonContext.States.Where(s => s.CountryId == address.CountryId).Any())
                                {
                                    validator.FHLErrors.Add("Consignee address state is required");
                                }
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(item.DescriptionOfGoods))
                {
                    validator.FHLErrors.Add("Description Of Goods field is required");
                }

                if (item.GrossWeight == 0 || item.GrossWeight == null)
                {
                    validator.FHLErrors.Add("Gross Weight field is required");
                }

                if (item.ChargeableWeight == 0 || item.ChargeableWeight == null)
                {
                    validator.FHLErrors.Add("Chargeable Weight field is required");
                }

                validator.IsFHLValid = validator.FHLErrors.Count == 0;
                myResult.Add(validator);
                id++;
            }

            return myResult;
        }

    }

    public class AWBResultClass
    {
        [Key]
        public int Id { get; set; }
        public int Tenant { get; set; }
        public string TTY { get; set; }
        public string PIMA { get; set; }
        public string ShipmentId { get; set; }
        public string Recipient { get; set; }
        public string StockFHLCode { get; set; }
        public string StockFWBCode { get; set; }
        public int SendingCount { get; set; }
        public bool IsSendingFHLs { get; set; }
        public int AllHousesCount { get; set; }
        public int ValidHousesCount { get; set; }
        public bool IsAWBStockPrepaid { get; set; }
        public int StockRemainingBefore { get; set; }
        public int StockRemainingAfter { get; set; }
        public bool IsCargonautEnabled { get; set; }
        public bool IsCargonautSending { get; set; }
        public bool IsDEXXEnabled { get; set; }
        public bool IsDEXXSending { get; set; }
        public bool IsDemoTenant { get; set; }
        public bool IsValid { get; set; }
        public bool HasMainErrors { get; set; }
        public bool HasStockErrors { get; set; }
        public bool IsEAWBOnlyDemo { get; set; }                
        public bool IsUpgradingChamp { get; set; }
        public string AWBMessagesCCSTypeCode { get; set; }
        public List<string> ErrorsList { get; set; }
        public List<string> ValidFHLsDataStringList { get; set; }
        public AWBResultClass()
        {
            this.ErrorsList = new List<string>();
            this.ValidFHLsDataStringList = new List<string>();
        }
    }
}
