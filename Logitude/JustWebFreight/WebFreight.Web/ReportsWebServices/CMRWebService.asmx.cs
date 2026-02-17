using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Services;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using WebFreight.Web.DataProviders;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityQueries;
using WebFreight.Web.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.Helpers;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.ReportsWebServices
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class CMRWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public byte[] GetDeliveryData(string entityId, string childentityId, int tenant, string userId, string documentTypeCopyId)
        {
            CMRDataProvider cmrDataProvider = GetDeliveryDataProvider(entityId, childentityId, tenant, userId, documentTypeCopyId);

            XmlSerializer serializer = new XmlSerializer(typeof(CMRDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, cmrDataProvider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        private CMRDataProvider GetDeliveryDataProvider(string entityId, string childentityId, int tenant, string userId, string documentTypeCopyId)
        {
            CMRDataProvider cmrDataProvider = new CMRDataProvider();
            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            IWebFreightContext webfreightcontext = WebFreightContext.GetContext(tenant);
            AddressRepository addressRepository = new AddressRepository(commonContext);
            CountryRepository countryRepository = new CountryRepository(commonContext);
            PortRepository portRepository = new PortRepository(commonContext);
            CardRepository cardRepository = new CardRepository(commonContext);

            Shipment shipment = null;
            shipment = (from a in shipmentsContext.Shipments.Include("ShipmentMasterData")
                        where a.Id == entityId
                        select a).FirstOrDefault();

            ShipmentPickUpDelivery myPickUpDelivery = null;
            myPickUpDelivery = (from a in shipmentsContext.ShipmentPickUpDeliveries
                                where a.Id == childentityId
                                select a).FirstOrDefault();

            Tenant tenantSettings = (from a in commonContext.Tenants
                                     where a.Id == tenant
                                     select a).FirstOrDefault();

            User currentUser = (from a in commonContext.Users
                                where a.Id == userId
                                select a).FirstOrDefault();

            if (currentUser == null)
            {
                if (tenant != 0)
                {
                    currentUser = (from a in commonContext.Users
                                   where a.Contact.Email == User.Identity.Name && a.Tenant == tenant
                                   select a).FirstOrDefault();
                }
                else
                {
                    currentUser = (from a in commonContext.Users
                                   where a.Contact.Email == User.Identity.Name && a.Tenant == 0
                                   select a).FirstOrDefault();
                }
            }

            if (shipment != null && tenantSettings != null)
            {
                cmrDataProvider.ProjectNumber = shipment.ProjectNumber;

                //------------partners-------------------------//
                if (shipment.ConsigneeId != null)
                {
                    Card consigneeClientAbroad = (from a in commonContext.Cards
                                                  where a.Id == shipment.ConsigneeId
                                                  select a).FirstOrDefault();

                    cmrDataProvider.ConsigneeName = consigneeClientAbroad.EnglishName;

                    Address consigneePickupAddress = addressRepository.GetSingleAddressByCardIdAndTypeId(shipment.ConsigneeId, "P", tenant);
                    if (consigneePickupAddress != null)
                    {
                        cmrDataProvider.ConsigneePickUpAddress = DataProviders.General.GetAddress(consigneePickupAddress);
                    }
                    else
                    {
                        Address consigneeMainAddress = addressRepository.GetSingleAddressByCardIdAndTypeId(shipment.ConsigneeId, "M", tenant);
                        if (consigneeMainAddress != null)
                        {
                            cmrDataProvider.ConsigneePickUpAddress = DataProviders.General.GetAddress(consigneeMainAddress);
                        }
                    }
                }

                if (shipment.ShipperId != null)
                {
                    Card shipperClient = (from a in commonContext.Cards
                                          where a.Id == shipment.ShipperId
                                          select a).FirstOrDefault();

                    cmrDataProvider.ShipperName = shipperClient != null ? shipperClient.EnglishName : "";

                    if (shipment.ShipperAddressId != null)
                    {
                        Address shipperClientAddress = addressRepository.GetSingleAddress(shipment.ShipperAddressId, tenant);


                        if (shipperClientAddress != null)
                        {
                            if (shipperClientAddress.IsLocalLanguage && !string.IsNullOrEmpty(shipperClient.LocalName))
                            {
                                cmrDataProvider.ShipperName = shipperClient != null ? shipperClient.LocalName : "";
                            }

                            cmrDataProvider.ShipperAddress = cmrDataProvider.ShipperAddress + DataProviders.General.GetAddress(shipperClientAddress);
                        }
                    }
                }

                if (shipment.ConsigneeAddressId != null)
                {
                    Address consigneeClientAbroadAddress = addressRepository.GetSingleAddress(shipment.ConsigneeAddressId, tenant);

                    if (consigneeClientAbroadAddress != null)
                    {
                        cmrDataProvider.ConsigneeAddress = DataProviders.General.GetAddress(consigneeClientAbroadAddress);
                    }
                }

                if (tenantSettings.AddressId != null)
                {
                    cmrDataProvider.CompanyName = tenantSettings.Company;

                    Address tenantAddress = addressRepository.GetSingleAddress(tenantSettings.AddressId, tenant);

                    if (tenantAddress != null)
                    {
                        cmrDataProvider.TenantAddress = DataProviders.General.GetAddress(tenantAddress);
                    }
                }

                cmrDataProvider.ShipmentNumber = shipment.ShipmentNumber ;
                cmrDataProvider.HAWBNumber = shipment.House;

                ShipmentMasterData masterData = (from a in shipmentsContext.ShipmentMasterDatas
                                                 where a.Id == shipment.MasterShipmentDataId
                                                 select a).FirstOrDefault();

                cmrDataProvider.MAWBNumber  = EntityFieldsHelper.GetLongMasterField(shipment, masterData);

                if (myPickUpDelivery != null)
                {
                    #region

                    if(myPickUpDelivery.TransportModeCode != null)
                    {
                        PickUpDeliveryTransportMode myTransportMode = shipmentsContext.PickUpDeliveryTransportModes.Where(d => d.Code == myPickUpDelivery.TransportModeCode).FirstOrDefault();
                        if(myTransportMode != null)
                        {
                            cmrDataProvider.PickUpOrDeliveryTransportMode = myTransportMode.Name;
                        }
                    }

                    #region EmptyContainer
                    cmrDataProvider.EmptyContainerRef = myPickUpDelivery.EmptyPickupDepotReference;
                    cmrDataProvider.EmptyContainerReturnRef = myPickUpDelivery.EmptyDeliveryDepotReference;

                    if (!string.IsNullOrEmpty(myPickUpDelivery.EmptyPickupContainerPartnerId))
                    {
                        Card myCard = CardRepository.GetSingleCard(myPickUpDelivery.EmptyPickupContainerPartnerId, tenant, true);

                        if (myCard != null)
                        {
                            string myEmptyContainer = null;

                            myEmptyContainer = myCard.EnglishName != null ? myCard.EnglishName : "";

                            Address theAddress = addressRepository.GetMainAddressByCardId(myCard.Id, tenant);

                            if (theAddress != null)
                            {
                                if (theAddress.IsLocalLanguage && !string.IsNullOrEmpty(myCard.LocalName))
                                {
                                    myEmptyContainer = myCard.LocalName;
                                }

                                myEmptyContainer = myEmptyContainer + Environment.NewLine + DataProviders.General.GetAddress(theAddress);

                                if (theAddress.PhoneNumber != null)
                                {
                                    myEmptyContainer = myEmptyContainer + Environment.NewLine + "Phone No. : " + theAddress.PhoneNumber;
                                }

                                if (theAddress.FaxNumber != null)
                                {
                                    myEmptyContainer = myEmptyContainer + "   Fax No. : " + theAddress.FaxNumber;
                                }
                            }

                            cmrDataProvider.EmptyContainer = myEmptyContainer;
                        }
                    }

                    if (!string.IsNullOrEmpty(myPickUpDelivery.EmptyDeliveryContainerPartnerId))
                    {
                        Card myCard = CardRepository.GetSingleCard(myPickUpDelivery.EmptyDeliveryContainerPartnerId, tenant, true);

                        if (myCard != null)
                        {
                            string myEmptyContainer = null;

                            myEmptyContainer = myCard.EnglishName != null ? myCard.EnglishName : "";

                            Address theAddress = addressRepository.GetMainAddressByCardId(myCard.Id, tenant);

                            if (theAddress != null)
                            {
                                if (theAddress.IsLocalLanguage && !string.IsNullOrEmpty(myCard.LocalName))
                                {
                                    myEmptyContainer = myCard.LocalName;
                                }

                                myEmptyContainer = myEmptyContainer + Environment.NewLine + DataProviders.General.GetAddress(theAddress);

                                if (theAddress.PhoneNumber != null)
                                {
                                    myEmptyContainer = myEmptyContainer + Environment.NewLine + "Phone No. : " + theAddress.PhoneNumber;
                                }

                                if (theAddress.FaxNumber != null)
                                {
                                    myEmptyContainer = myEmptyContainer + "   Fax No. : " + theAddress.FaxNumber;
                                }
                            }

                            cmrDataProvider.EmptyContainerReturn = myEmptyContainer;
                        }
                    }
                    #endregion

                    cmrDataProvider.Note = myPickUpDelivery.Notes != null ? myPickUpDelivery.Notes : "";
                    cmrDataProvider.ShipmentWeightUnit = shipment.GrossWeightUnitCode != null ? shipment.GrossWeightUnitCode : "";
                    cmrDataProvider.ShipmentVolumUnit = shipment.VolumeUnitCode != null ? shipment.VolumeUnitCode : "";

                    #region From

                    #region From PART
                    if (myPickUpDelivery.PickUpDeliveryFromTypeCode == "PART")
                    {
                        if (string.IsNullOrEmpty(myPickUpDelivery.FromPartnerCardId))
                        {
                            cmrDataProvider.PickUpAddressAndDate = myPickUpDelivery.FromAddress != null ? myPickUpDelivery.FromAddress : "";
                        }

                        else
                        {
                            if (string.IsNullOrEmpty(myPickUpDelivery.FromAddressId))
                            {
                                cmrDataProvider.PickUpAddressAndDate = myPickUpDelivery.FromAddress != null ? myPickUpDelivery.FromAddress : "";
                            }

                            else
                            {
                                Address address = addressRepository.GetSingleAddress(myPickUpDelivery.FromAddressId, tenant);
                                if (address != null)
                                {

                                    cmrDataProvider.PickUpAddressAndDate = DataProviders.General.GetAddress(address)
                                        + (!string.IsNullOrEmpty(address.ATTN) ? (Environment.NewLine + "Contact : " + address.ATTN) : "")
                                        + (!string.IsNullOrEmpty(address.PhoneNumber) ? (Environment.NewLine + "Phone : " + address.PhoneNumber) : "");
                                }
                            }
                        }
                    }
                    #endregion

                    #region From PORT
                    else if (myPickUpDelivery.PickUpDeliveryFromTypeCode == "PORT")
                    {
                        cmrDataProvider.PickUpAddressAndDate = myPickUpDelivery.FromAddress != null ? myPickUpDelivery.FromAddress : "";
                    }
                    #endregion

                    #region From CASL
                    else
                    {
                        string location = "";

                        if (!string.IsNullOrEmpty(myPickUpDelivery.FromAddressCity))
                        {
                            location = myPickUpDelivery.FromAddressCity;
                        }

                        if (!string.IsNullOrEmpty(myPickUpDelivery.FromAddressZipCode))
                        {
                            location = location + " " + myPickUpDelivery.FromAddressZipCode;
                        }

                        if (!string.IsNullOrEmpty(myPickUpDelivery.FromAddressCountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(myPickUpDelivery.FromAddressCountryId, tenant);
                            if (country != null)
                            {
                                location = location + Environment.NewLine + country.EnglishName;
                            }
                        }

                        cmrDataProvider.PickUpAddressAndDate = location;
                    }
                    #endregion

                    #endregion

                    #region To

                    #region To PART
                    if (myPickUpDelivery.PickUpDeliveryToTypeCode == "PART")
                    {
                        if (string.IsNullOrEmpty(myPickUpDelivery.ToPartnerCardId))
                        {
                            cmrDataProvider.DeliveryToAddress = myPickUpDelivery.ToAddress != null ? myPickUpDelivery.ToAddress : "";
                        }

                        else
                        {
                            if (string.IsNullOrEmpty(myPickUpDelivery.ToAddressId))
                            {
                                cmrDataProvider.DeliveryToAddress = myPickUpDelivery.ToAddress != null ? myPickUpDelivery.ToAddress : "";
                            }

                            else
                            {
                                Address address = addressRepository.GetSingleAddress(myPickUpDelivery.ToAddressId, tenant);
                                if (address != null)
                                {

                                    cmrDataProvider.DeliveryToAddress = DataProviders.General.GetAddress(address)
                                        + (!string.IsNullOrEmpty(address.ATTN) ? (Environment.NewLine + "Contact : " + address.ATTN) : "")
                                        + (!string.IsNullOrEmpty(address.PhoneNumber) ? (Environment.NewLine + "Phone : " + address.PhoneNumber) : "");
                                }
                            }
                        }
                    }
                    #endregion

                    #region To PORT
                    else if (myPickUpDelivery.PickUpDeliveryToTypeCode == "PORT")
                    {
                        cmrDataProvider.DeliveryToAddress = myPickUpDelivery.ToAddress != null ? myPickUpDelivery.ToAddress : "";
                    }
                    #endregion

                    #region From CASL
                    else
                    {
                        string location = "";

                        if (!string.IsNullOrEmpty(myPickUpDelivery.ToAddressCity))
                        {
                            location = myPickUpDelivery.ToAddressCity;
                        }

                        if (!string.IsNullOrEmpty(myPickUpDelivery.ToAddressZipCode))
                        {
                            location = location + " " + myPickUpDelivery.ToAddressZipCode;
                        }

                        if (!string.IsNullOrEmpty(myPickUpDelivery.ToAddressCountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(myPickUpDelivery.ToAddressCountryId, tenant);
                            if (country != null)
                            {
                                location = location + Environment.NewLine + country.EnglishName;
                            }
                        }

                        cmrDataProvider.DeliveryToAddress = location;
                    }
                    #endregion

                    #endregion

                    List<ShipmentPickUpDeliveryPackage> packages = shipmentsContext.ShipmentPickUpDeliveryPackages.Where(d => d.ShipmentPickUpDeliveryId == myPickUpDelivery.Id && d.Tenant == tenant).ToList();

                    int counter = 1;
                    int numberofpackages = 0;
                    double? totalWeight = 0;
                    double? totalVolume = 0;
                    cmrDataProvider.HsCode = "";

                    #region Packaegs

                    cmrDataProvider.ContainersList = new List<ContainerData>();

                    foreach (ShipmentPickUpDeliveryPackage package in packages)
                    {
                        ContainerData containerRecord = new ContainerData();

                        counter++;

                        PackageType packtype = (from pa in commonContext.PackageTypes
                                                where pa.Id == package.PackageTypeId
                                                select pa).FirstOrDefault();

                        numberofpackages += package.Quantity.Value;
                        totalWeight += (package.Weight != null ? package.Weight : 0);
                        totalVolume += (package.Volume != null ? package.Volume : 0);

                        cmrDataProvider.HsCode = cmrDataProvider.HsCode + (package.Harmonize != null ? "," + package.Harmonize : "");

                        containerRecord.HsCode = package.Harmonize != null ? "," + package.Harmonize : "";
                        containerRecord.Weight = (package.Weight != null ? package.Weight.ToString() : "0") + "  " + cmrDataProvider.ShipmentWeightUnit;
                        containerRecord.Volume = (package.Volume != null ? package.Volume.ToString() : "0") + "  " + cmrDataProvider.ShipmentVolumUnit;
                        containerRecord.Description = package.Description != null ? package.Description : " ";
                        containerRecord.Pieces = package.Quantity != null ? package.Quantity : 0;

                        if (packtype != null)
                        {
                            if (packtype.IsContainer)
                            {
                                containerRecord.MarksAndNumbers = "1 X " + packtype.PrintAs + Environment.NewLine + package.ContainerNumber;

                                if (!string.IsNullOrEmpty(package.ShipperSeal))
                                {
                                    containerRecord.MarksAndNumbers = containerRecord.MarksAndNumbers + Environment.NewLine + "Seal: " + package.ShipperSeal;
                                }

                                containerRecord.MarksAndNumbers = containerRecord.MarksAndNumbers + Environment.NewLine + package.Description;
                            }

                            else
                            {
                                containerRecord.MarksAndNumbers = package.Quantity + " X " + packtype.EnglishName + Environment.NewLine + package.Description;
                            }

                            containerRecord.PackageType = packtype.EnglishName;
                        }

                        // Dimensions
                        containerRecord.Dimensions = GetDimensions(package);

                        cmrDataProvider.ContainersList.Add(containerRecord);
                    }

                    #endregion

                    if (cmrDataProvider.HsCode != null)
                    {
                        cmrDataProvider.HsCode = cmrDataProvider.HsCode.TrimStart(',');
                    }

                    cmrDataProvider.NumberOfPackages = numberofpackages.ToString();
                    cmrDataProvider.WeightUnit = tenantSettings.GrossWeightUnitCode;
                    cmrDataProvider.TotalWeight = totalWeight.ToString();
                    cmrDataProvider.Volume = totalVolume.ToString();

                    if (shipment.IncotermId != null)
                    {
                        Incoterm incoterm = (from a in commonContext.Incoterms
                                             where a.Id == shipment.IncotermId
                                             select a).FirstOrDefault();
                        if (incoterm != null)
                        {
                            cmrDataProvider.SendersInstructions = cmrDataProvider.SendersInstructions + Environment.NewLine + "Incoterm: " + incoterm.Code + "-" + incoterm.Name;
                        }
                    }

                    cmrDataProvider.TruckNumberAndTrailerNumber = myPickUpDelivery.CarrierNumber + "/" + myPickUpDelivery.TrailerNumber;
                    if (!string.IsNullOrEmpty(myPickUpDelivery.CarrierId))
                    {
                        Card trucker = (from a in commonContext.Cards
                                        where a.Id == myPickUpDelivery.CarrierId
                                        select a).FirstOrDefault();
                        cmrDataProvider.Trucker = trucker != null ? trucker.EnglishName : "";

                        Address truckerAddress = addressRepository.GetSingleAddressByCardIdAndTypeId(myPickUpDelivery.CarrierId, "M", tenant);
                        cmrDataProvider.TruckerMainAddress = truckerAddress != null ? DataProviders.General.GetAddress(truckerAddress) : "";
                    }
                    cmrDataProvider.TruckerNumber = myPickUpDelivery.CarrierNumber != null ? myPickUpDelivery.CarrierNumber : "";

                    if (!string.IsNullOrEmpty(myPickUpDelivery.FromPartnerCardId))
                    {
                        Card FromPartner = (from a in commonContext.Cards
                                            where a.Id == myPickUpDelivery.FromPartnerCardId
                                            select a).FirstOrDefault();
                        cmrDataProvider.PickupFromPartnerName = FromPartner != null ? FromPartner.EnglishName : "";
                    }

                    if (!string.IsNullOrEmpty(myPickUpDelivery.ToPartnerCardId))
                    {
                        Card ToPartner = (from a in commonContext.Cards
                                          where a.Id == myPickUpDelivery.ToPartnerCardId
                                          select a).FirstOrDefault();
                        cmrDataProvider.DeliveryToPartnerName = ToPartner != null ? ToPartner.EnglishName : "";
                    }

                    cmrDataProvider.ETD = myPickUpDelivery.ETD;
                    cmrDataProvider.ATD = myPickUpDelivery.ATD;
                    cmrDataProvider.ETA = myPickUpDelivery.ETA;
                    cmrDataProvider.ATA = myPickUpDelivery.ATA;

                    Branch branch = (from a in commonContext.Branches
                                     where a.Id == currentUser.BranchId
                                     select a).FirstOrDefault();

                    if (branch != null)
                    {
                        cmrDataProvider.IssueBranchAndDate = branch.EnglishName + "," + DateTime.Now.Date.ToString();
                        cmrDataProvider.IssueBranch = branch.EnglishName;
                    }

                    cmrDataProvider.IssueDate = DateTime.Now.Date;
                    cmrDataProvider.PickUpOrDeliveryNumber = myPickUpDelivery.PickUpDeliveryNumber;

                    if (!string.IsNullOrEmpty(shipment.MoveTypeId))
                    {
                        MoveType moveType = webfreightcontext.MoveTypes.Where(m => m.Id == shipment.MoveTypeId).FirstOrDefault();

                        if (moveType != null)
                        {
                            cmrDataProvider.MoveTypeCode = moveType.Code;
                            cmrDataProvider.MoveTypeName = moveType.MoveTypeEnglishName;
                        }
                    }

                    // custom fields//
                    List<FormCustomField> customfieldsList = commonContext.FormCustomFields.ToList();
                    List<DocumentTypeCustomField> documentCustomfieldsList = commonContext.DocumentTypeCustomFields.ToList();

                    //----cash on delivery---//
                    FormCustomField cashOnDeliveryField = (from a in customfieldsList
                                                           where a.FieldCode == "CashOnDelivery" && a.EntityId == shipment.Id
                                                           select a).FirstOrDefault();

                    DocumentTypeCustomField cashOnDeliveryDocumentCustom = (from a in documentCustomfieldsList
                                                                            where a.FieldCode == "CashOnDelivery"
                                                                            select a).FirstOrDefault();
                    if (cashOnDeliveryField != null)
                    {
                        cmrDataProvider.CashOnDelivery = cashOnDeliveryField.Value;
                    }
                    else if (cashOnDeliveryDocumentCustom != null)
                    {
                        cmrDataProvider.CashOnDelivery = cashOnDeliveryDocumentCustom.DefaultValue;
                    }

                    //--Documents attached--//
                    FormCustomField documentsAttachedField = (from a in customfieldsList
                                                              where a.FieldCode == "DocumentsAttached" && a.EntityId == shipment.Id
                                                              select a).FirstOrDefault();

                    DocumentTypeCustomField documentsAttachedDocumentCustom = (from a in documentCustomfieldsList
                                                                               where a.FieldCode == "DocumentsAttached"
                                                                               select a).FirstOrDefault();

                    if (documentsAttachedField != null)
                    {
                        cmrDataProvider.DocumentsAttached = documentsAttachedField.Value;
                    }
                    else if (documentsAttachedDocumentCustom != null)
                    {
                        cmrDataProvider.DocumentsAttached = documentsAttachedDocumentCustom.DefaultValue;
                    }

                    //----Instructions---//
                    FormCustomField instructionsField = (from a in customfieldsList
                                                         where a.FieldCode == "Instructions" && a.EntityId == shipment.Id
                                                         select a).FirstOrDefault();

                    DocumentTypeCustomField instructionsDocumentCustom = (from a in documentCustomfieldsList
                                                                          where a.FieldCode == "Instructions"
                                                                          select a).FirstOrDefault();

                    if (instructionsField != null)
                    {
                        cmrDataProvider.SendersInstructions = instructionsField.Value;
                    }
                    else if (instructionsDocumentCustom != null)
                    {
                        cmrDataProvider.SendersInstructions = instructionsDocumentCustom.DefaultValue;
                    }

                    DocumentTypeCopy documenttypecopy = (from copy in commonContext.DocumentTypeCopies
                                                         where copy.Id == documentTypeCopyId
                                                         select copy).FirstOrDefault();

                    if (documenttypecopy != null)
                    {
                        string[] copyNameandNumber = new string[5];
                        copyNameandNumber = GetCopyNameAndNumber(documenttypecopy.Code);

                        cmrDataProvider.CopyNumber = copyNameandNumber[0];
                        cmrDataProvider.CopyNameFirstLanguage = copyNameandNumber[1];
                        cmrDataProvider.CopyNameSecondLanguage = copyNameandNumber[2];
                        cmrDataProvider.EnglishLanguage = copyNameandNumber[3];
                        cmrDataProvider.RussianLanguage = copyNameandNumber[4];
                    }

                    #endregion
                }

                #region Warehouse Leg
                cmrDataProvider.WarehouseLegExpectedEntryDate = shipment.WarehouseLegExpectedEntryDate;
                cmrDataProvider.WarehouseLegActualEntryDate = shipment.WarehouseLegActualEntryDate;
                cmrDataProvider.WarehouseLegExpectedReleaseDate = shipment.WarehouseLegExpectedReleaseDate;
                cmrDataProvider.WarehouseLegActualReleaseDate = shipment.WarehouseLegActualReleaseDate;
                cmrDataProvider.WarehouseLegLastFreeDate = shipment.WarehouseLegLastFreeDate;
                cmrDataProvider.WarehouseLegRemarks = shipment.WarehouseLegRemarks;
                cmrDataProvider.WarehouseLegReference = shipment.WarehouseLegReference;
                if (shipment.WarehouseLegWarehouseId != null)
                {
                    Card warehouseCard = cardRepository.GetSingleCard(shipment.WarehouseLegWarehouseId, tenant);
                    cmrDataProvider.WarehouseLegTerminalName = warehouseCard != null ? warehouseCard.EnglishName : "";
                }

                if (shipment.WarehouseLegAddressId != null)
                {
                    Address warehouseAddress = addressRepository.GetSingleAddress(shipment.WarehouseLegAddressId, tenant);
                    cmrDataProvider.WarehouseLegAddress = DataProviders.General.GetAddress(warehouseAddress);
                }
                cmrDataProvider.WarehouseLegEntryDate = shipment.WarehouseLegActualEntryDate != null ? shipment.WarehouseLegActualEntryDate : shipment.WarehouseLegExpectedEntryDate;
                cmrDataProvider.WarehouseLegReleaseDate = shipment.WarehouseLegActualReleaseDate != null ? shipment.WarehouseLegActualReleaseDate : shipment.WarehouseLegExpectedReleaseDate;
                cmrDataProvider.WarehouseLegTerminalCode = shipment.WarehouseLegTerminalCode;
                #endregion

                CustomFieldResolver customFieldResolver = new CustomFieldResolver();
                customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, shipment, cmrDataProvider);
            }

            try
            {
                Type cmrType = cmrDataProvider.GetType();
                PropertyInfo[] properties = cmrType.GetProperties();

                foreach (PropertyInfo pi in properties)
                {
                    Type piType = pi.PropertyType;

                    if (piType.Name != "Double")
                    {
                        if (pi.GetValue(cmrDataProvider, null) == null || pi.GetValue(cmrDataProvider, null).ToString() == "0" || pi.GetValue(cmrDataProvider, null).ToString() == "00.00")
                        {
                            pi.SetValue(cmrDataProvider, "", null);
                        }
                    }
                }
            }

            catch { }

            return cmrDataProvider;
        }
        private string[] GetCopyNameAndNumber(string doccopycode)
        {
            string[] result = new string[5];

            switch (doccopycode)
            {
                case "SC":
                    result[0] = "1"; // copy number
                    result[1] = "Izvod za pošiljatelj"; // first language
                    result[2] = "exemplaire de l'expéditeur"; // second language
                    result[3] = "Copy for Sender";
                    result[4] = "Копия для Отправителя";
                    break;

                case "RC":
                    result[0] = "2";
                    result[1] = "Izvod za prejemnika";
                    result[2] = "Exemplaire du destinataire";
                    result[3] = "Copy for Consignee";
                    result[4] = "Копия для Получателя";
                    break;

                case "CC":
                    result[0] = "3";
                    result[1] = "Izvod za prevoznika";
                    result[2] = "Exemplaire du transporteur";
                    result[3] = "Copy for Carrier";
                    result[4] = "Копия для Перевозчика";
                    break;

                case "3A":
                    result[0] = "3A";
                    break;

                case "3B":
                    result[0] = "3B";
                    break;

                case "3C":
                    result[0] = "3C";
                    break;

                case "3D":
                    result[0] = "3D";
                    break;
            }
            return result;
        }

        private string GetDimensions(ShipmentPickUpDeliveryPackage package)
        {
            string myDimensions = ""; 

            if (package.Length == null && package.Width == null && package.Height == null)
            {
                myDimensions = " - - ";
            }
            else
            {
                double? myLength = 0;
                double? myWidth = 0;
                double? myHeight = 0;

                if (package.Length != null)
                {
                    myLength = package.Length;
                }

                if (package.Width != null)
                {
                    myWidth = package.Width;
                }

                if (package.Height != null)
                {
                    myHeight = package.Height;
                }

                myDimensions = myLength + "-" + myWidth + "-" + myHeight;
            }
            return myDimensions;
        }
    }
}
