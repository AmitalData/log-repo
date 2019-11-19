using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Services;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataProviders;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using WebFreight.Web.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Server.Infrastructure.Helpers;
using System.Text.RegularExpressions;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for ShippingDeclaration
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ShippingDeclarationWebService : System.Web.Services.WebService
    {
        private int tenant;
        private WebServiceHelper myServicHelper;

        [WebMethod]
        public byte[] GetShippingDeclarationData(string shipmentId, int tenant, string documentTypeCode)
        {
            this.tenant = tenant;
            this.myServicHelper = new WebServiceHelper(tenant);

            ShippingDeclarationDataProvider myDataProvider = GetShippingDeclarationDataProvider(shipmentId, tenant, documentTypeCode);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ShippingDeclarationDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, myDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(memoryStream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

        private ShippingDeclarationDataProvider GetShippingDeclarationDataProvider(string shipmentId, int tenant, string documentTypeCode)
        {
            ShippingDeclarationDataProvider myDataProvider = new ShippingDeclarationDataProvider();

            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            IWebFreightContext webfreightContext = WebFreightContext.GetContext(tenant);

            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
            WarehouseRepository warehouseRep = new WarehouseRepository(commonContext);
            WarehouseQuery warehouseQuery = new WarehouseQuery(warehouseRep);
            ShipmentPickUpDeliveryRepository shipmentPickUpDeliveryRepository = new ShipmentPickUpDeliveryRepository(shipmentsContext);
            ShipmentPickUpQuery shipmentPickUpQuery = new ShipmentPickUpQuery(shipmentPickUpDeliveryRepository);
            ShipmentDeliveryQuery shipmentDeliveryQuery = new ShipmentDeliveryQuery(shipmentPickUpDeliveryRepository);

            PortRepository portRepository = new PortRepository(commonContext);
            AddressRepository addressRepository = new AddressRepository(commonContext);
            ContactRepository contactRepository = new ContactRepository(commonContext);
            TenantRepository tenantRepository = new TenantRepository(commonContext);
            CountryRepository countryRepository = new CountryRepository(commonContext);
            CardQuery cardQuery = new CardQuery(tenant);

            ShipmentPM shipment = shipmentQuery.GetSinglePM(shipmentId, tenant);

            Tenant currentTenant = tenantRepository.GetSingleTenant(tenant);
            if (currentTenant != null)
            {
                if (!string.IsNullOrEmpty(currentTenant.AddressId))
                {
                    Address address = addressRepository.GetSingleAddress(currentTenant.AddressId, tenant);
                    if (address != null)
                    {
                        myDataProvider.TenantName = address.Name;
                        myDataProvider.TenantPhone = address.PhoneNumber;
                    }
                }

                myDataProvider.TenantCAAT = currentTenant.CAAT;
                myDataProvider.TenantCBSA = currentTenant.CBSA;
            }

            ContactQuery contactQuery = new ContactQuery(tenant);
            string email = AuthenticationUtil.GetLoggedUserEmail(tenant);
            ContactPM loggedContact = contactQuery.GetContactByEmailOnly(email, tenant);
            if (loggedContact != null)
            {
                myDataProvider.IssuedByUser = loggedContact.EnglishName;
            }

            if (shipment != null)
            {
                string localCurrencyCode = "";
                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                string volumeUnitCode = shipment.VolumeUnitCode != null ? shipment.VolumeUnitCode : "";
                string volumetricUnitCode = shipment.ChargeableWeightUnitCode != null ? shipment.ChargeableWeightUnitCode : "";

                myDataProvider.CustomsDeclarationNumber = shipment.CustomsDeclarationNumber != null ? shipment.CustomsDeclarationNumber : "";
                myDataProvider.InsidePackagesDetails = shipment.NumberOfInsidePackagesDetails;
                myDataProvider.ShipmentType = shipment.ShipmentTypeName != null ? shipment.ShipmentTypeName : "";
                myDataProvider.Incoterm = shipment.IncotermName;
                myDataProvider.Salesman = shipment.SalesmanUserName;
                myDataProvider.TotalPayables = shipment.OpenPayablesInLocalCurrency + shipment.AccountedPayablesInLocalCurrency;
                myDataProvider.ValueOfGoods = shipment.ValueOfGoods;
                myDataProvider.ENSNumber = shipment.ENSNumber;
                myDataProvider.ENSDate = shipment.ENSDate;
                myDataProvider.FreightRelease = shipment.FreightRelease;
                myDataProvider.TerminalAvailable = shipment.TerminalAvailable;
                myDataProvider.ISFNumber = shipment.ISFNumber;
                myDataProvider.ISFDate = shipment.ISFDate;
                myDataProvider.ITNumber = shipment.ITNumber;
                myDataProvider.ITDate = shipment.ITDate;
                myDataProvider.ConfirmationNotes = shipment.BookingConfirmationNotes;
                myDataProvider.MainCarriageATD = shipment.MainCarriageATD;
                myDataProvider.MasterInternalNumber = shipment.MasterShipmentNumber;
                myDataProvider.CompleteShipmentType = shipment.TransportModeName + " " + shipment.DirectionName;
                myDataProvider.ChargeableWeight = shipment.ChargeableWeight;
                myDataProvider.ChargeableWeightUnitCode = shipment.ChargeableWeightUnitCode;
                myDataProvider.ProjectNumber = shipment.ProjectNumber;
                myDataProvider.ARInvoices = shipment.ARInvoices;
                myDataProvider.IsDangerous = shipment.IsDangerous;
                myDataProvider.SpecialServicesTypeName = shipment.SpecialServicesTypeName;

                if (shipment.DocumentsClosingDate != null)
                {
                    myDataProvider.DocumentsClosingDate = shipment.DocumentsClosingDate;
                    myDataProvider.DocumentsClosingTime = shipment.DocumentsClosingDate.Value.TimeOfDay;
                }

                if (!string.IsNullOrEmpty(shipment.OBLTypeCode))
                {
                    OBLType type = shipmentsContext.OBLTypes.Where(d => d.Code == shipment.OBLTypeCode).FirstOrDefault();

                    if (type != null)
                    {
                        myDataProvider.OBLType = type.Name;
                    }
                }

                if (!string.IsNullOrEmpty(shipment.ValueOfGoodsCurrencyId))
                {
                    Currency currency = CurrencyRepository.GetSingleCurrency(shipment.ValueOfGoodsCurrencyId, tenant, true);

                    if (currency != null)
                    {
                        myDataProvider.ValueOfGoodsCurrency = currency.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(shipment.CreatedByUserId))
                {
                    Contact myCreatedByContact = contactRepository.GetSingleContact(shipment.CreatedByUserId, tenant);
                    if (myCreatedByContact != null)
                    {
                        myDataProvider.CreatedByUserName = myCreatedByContact.EnglishName;
                        myDataProvider.CreatedByUserEmail = myCreatedByContact.Email;
                    }
                }

                #region Cards
                Card notify1 = null;
                Card notify2 = null;
                Card myConsigneePartner = null;
                Card consigneeNotImporter = null;

                if (shipment.Notify1Id != null)
                {
                    notify1 = (from a in commonContext.Cards where a.Id == shipment.Notify1Id select a).FirstOrDefault();
                }

                if (shipment.Notify2Id != null)
                {
                    notify2 = (from a in commonContext.Cards where a.Id == shipment.Notify2Id select a).FirstOrDefault();
                }

                if (shipment.ConsigneeId != null)
                {
                    myConsigneePartner = (from a in commonContext.Cards where a.Id == shipment.ConsigneeId select a).FirstOrDefault();
                }

                if (shipment.ConsigneeNotImporterId != null)
                {
                    consigneeNotImporter = (from a in commonContext.Cards where a.Id == shipment.ConsigneeNotImporterId select a).FirstOrDefault();
                }
                #endregion

                #region Ports
                Port preCarriageFromPort = null;
                Port preCarriageToPort = null;
                Port mainCarriageFromPort = null;
                Port mainCarriageToPort = null;
                Port onCarriageToPort = null;
                Port finalDestination = null;

                if (shipment.PreCarriageFromPortId != null)
                {
                    preCarriageFromPort = (from a in commonContext.Ports where a.Id == shipment.PreCarriageFromPortId select a).FirstOrDefault();
                }

                if (shipment.PreCarriageToPortId != null)
                {
                    preCarriageToPort = (from a in commonContext.Ports where a.Id == shipment.PreCarriageToPortId select a).FirstOrDefault();
                }

                if (shipment.MainCarriageFromPortId != null)
                {
                    mainCarriageFromPort = (from a in commonContext.Ports where a.Id == shipment.MainCarriageFromPortId select a).FirstOrDefault();
                }

                if (shipment.MainCarriageToPortId != null)
                {
                    mainCarriageToPort = (from a in commonContext.Ports where a.Id == shipment.MainCarriageToPortId select a).FirstOrDefault();
                }

                if (shipment.OnCarriageToPortId != null)
                {
                    onCarriageToPort = (from a in commonContext.Ports where a.Id == shipment.OnCarriageToPortId select a).FirstOrDefault();
                }

                if (shipment.FinalDistenationPortId != null)
                {
                    finalDestination = (from a in commonContext.Ports where a.Id == shipment.FinalDistenationPortId select a).FirstOrDefault();
                }
                #endregion

                myDataProvider.CustomsDeclarationNumber = shipment.CustomsDeclarationNumber != null ? shipment.CustomsDeclarationNumber : "";
                myDataProvider.InsidePackagesDetails = shipment.NumberOfInsidePackagesDetails;
                myDataProvider.Incoterm = shipment.IncotermName;
                myDataProvider.Salesman = shipment.SalesmanUserName;
                myDataProvider.TotalPayables = shipment.OpenPayablesInLocalCurrency + shipment.AccountedPayablesInLocalCurrency;
                myDataProvider.ValueOfGoods = shipment.ValueOfGoods;
                myDataProvider.ENSNumber = shipment.ENSNumber;
                myDataProvider.ENSDate = shipment.ENSDate;
                myDataProvider.FreightRelease = shipment.FreightRelease;
                myDataProvider.TerminalAvailable = shipment.TerminalAvailable;
                myDataProvider.ISFNumber = shipment.ISFNumber;
                myDataProvider.ISFDate = shipment.ISFDate;
                myDataProvider.ITNumber = shipment.ITNumber;
                myDataProvider.ITDate = shipment.ITDate;
                myDataProvider.DocumentsClosingDate = shipment.DocumentsClosingDate;
                myDataProvider.ConfirmationNotes = shipment.BookingConfirmationNotes;
                myDataProvider.MainCarriageATD = shipment.MainCarriageATD;

                if (!string.IsNullOrEmpty(shipment.OBLTypeCode))
                {
                    OBLType type = shipmentsContext.OBLTypes.Where(d => d.Code == shipment.OBLTypeCode).FirstOrDefault();

                    if (type != null)
                    {
                        myDataProvider.OBLType = type.Name;
                    }
                }

                if (!string.IsNullOrEmpty(shipment.ValueOfGoodsCurrencyId))
                {
                    Currency currency = CurrencyRepository.GetSingleCurrency(shipment.ValueOfGoodsCurrencyId, tenant, true);

                    if (currency != null)
                    {
                        myDataProvider.ValueOfGoodsCurrency = currency.EnglishName;
                    }
                }

                #region Tenant
                Tenant myTenant = (from a in commonContext.Tenants where a.Id == tenant select a).FirstOrDefault();
                if (myTenant != null)
                {
                    myDataProvider.CompanyName = myTenant.Company != null ? myTenant.Company : "";
                    myDataProvider.LocalCustomsCode = myTenant.LocalCustomsCode;
                    myDataProvider.Signature = myTenant.Signature != null ? myTenant.Signature : "";
                    myDataProvider.Logo = DataProviders.General.GetLogo(myTenant.Id);
                    myDataProvider.FMCNumber = myTenant.FMCNumber;

                    Currency localCurrency = CurrencyRepository.GetSingleCurrency(myTenant.CurrencyId, tenant, true);
                    if (localCurrency != null)
                    {
                        localCurrencyCode = localCurrency.Code;
                    }

                    if (!string.IsNullOrEmpty(myTenant.AddressId))
                    {
                        Address address = addressRepository.GetSingleAddress(myTenant.AddressId, tenant);

                        if (address != null)
                        {
                            myDataProvider.TenantCity = address.City != null ? address.City : "";

                            if (address.State != null)
                            {
                                myDataProvider.StateCode = address.State.Code;
                            }

                            if (address.IsLocalLanguage)
                            {
                                myDataProvider.TenantCountryName = address.Country.LocalName;
                            }
                            else
                            {
                                myDataProvider.TenantCountryName = address.Country.EnglishName;
                            }

                            myDataProvider.TenantAddress = myDataProvider.CompanyName + Environment.NewLine + DataProviders.General.GetAddress(address);
                            myDataProvider.TenantAddressWithPhone = myDataProvider.CompanyName + Environment.NewLine + DataProviders.General.GetAddress(address);

                            if (!string.IsNullOrEmpty(address.PhoneNumber))
                            {
                                myDataProvider.TenantAddressWithPhone = myDataProvider.TenantAddressWithPhone + Environment.NewLine + "Phone: " + address.PhoneNumber;

                                if (!string.IsNullOrEmpty(address.FaxNumber))
                                {
                                    myDataProvider.TenantAddressWithPhone = myDataProvider.TenantAddressWithPhone + "   Fax: " + address.FaxNumber;
                                }
                            }

                            else
                            {
                                if (!string.IsNullOrEmpty(address.FaxNumber))
                                {
                                    myDataProvider.TenantAddressWithPhone = myDataProvider.TenantAddressWithPhone + Environment.NewLine + "Fax: " + address.FaxNumber;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Shipment Properties

                if (!string.IsNullOrEmpty(shipment.BranchId))
                {
                    BranchRepository branchRepository = new BranchRepository(tenant);
                    Branch branch = branchRepository.GetSingleBranch(shipment.BranchId, tenant);

                    if (branch != null)
                    {
                        myDataProvider.BranchSignature = branch.Signature;

                        if (!string.IsNullOrEmpty(branch.AddressId))
                        {
                            Address branchAddress = addressRepository.GetSingleAddress(branch.AddressId, tenant);
                            myDataProvider.BranchAddress = DataProviders.General.GetAddress(branchAddress);
                        }
                    }
                }

                myDataProvider.Date = todayDate.ToShortDateString();
                myDataProvider.ShipmentNumber = shipment.ShipmentNumber != null ? shipment.ShipmentNumber : "";
                myDataProvider.Branch = shipment.BranchName != null ? shipment.BranchName : "";
                myDataProvider.DeliveryOrder = shipment.DeliveryOrder;
                myDataProvider.ImportManifest = shipment.ImportManifest;
                myDataProvider.FreightLocationId = shipment.FreightLocationId;
                myDataProvider.TransportDocumentNumber = shipment.TransportDocumentNumber;
                myDataProvider.CarrierTransportDocumentNumber = shipment.CarrierTransportDocumentNumber;
                myDataProvider.AMSBL = shipment.AMSBL;
                myDataProvider.HouseNumber = shipment.House != null ? shipment.House : "";
                myDataProvider.SubNumber = shipment.House != null ? shipment.House : "";
                myDataProvider.GeneralDescriptionOfGoods = shipment.DescriptionOfGoods != null ? shipment.DescriptionOfGoods : "";
                myDataProvider.Notes = shipment.Notes;
                myDataProvider.PreCarriageCarrierName = shipment.PreCarriageCarrierName != null ? shipment.PreCarriageCarrierName : "";
                myDataProvider.SwornDate = String.Format("{0:dd MMM yyyy}", DateTime.Now.Date);
                myDataProvider.TodayDate = String.Format("{0:dd MMM yyyy}", DateTime.Now.Date);
                myDataProvider.TodayDate_DateTime = todayDate;
                myDataProvider.MainCarriageETA = shipment.MainCarriageETA != null ? String.Format("{0:dd MMM yyyy}", shipment.MainCarriageETA) : "";
                myDataProvider.MainCarriageETA_DateTime = shipment.MainCarriageETA;
                myDataProvider.MainCarriageETD = shipment.MainCarriageETD != null ? String.Format("{0:dd MMM yyyy}", shipment.MainCarriageETD) : "";
                myDataProvider.MainCarriageETD_DateTime = shipment.MainCarriageETD;
                myDataProvider.PreCarriageETD = shipment.PreCarriageETD;
                myDataProvider.PreCarriageETA = shipment.PreCarriageETA;
                myDataProvider.MainCarriageATA = shipment.MainCarriageATA != null ? String.Format("{0:dd MMM yyyy}", shipment.MainCarriageATA) : "";
                myDataProvider.OnCarriageETA = shipment.OnCarriageETA != null ? String.Format("{0:dd MMM yyyy}", shipment.OnCarriageETA) : "";
                myDataProvider.OnCarriageETA_DateTime = shipment.OnCarriageETA;
                myDataProvider.TenantCountryCode = shipment.House != null ? shipment.House : "";
                myDataProvider.TransportationType = shipment.TransportModeName;
                myDataProvider.Transshipment1ETA = shipment.Transshipment1ETA;

                int numberofpackages = shipment.NumberOfPackages != null ? shipment.NumberOfPackages.Value : 0;
                int numberofcontainers = shipment.NumberOfContainers != null ? shipment.NumberOfContainers.Value : 0;

                myDataProvider.TotalQuantity = MethodHelper.IsLCLEntity(shipment.TransportModeId, shipment.ShipmentTypeId) ? numberofpackages.ToString() : numberofcontainers.ToString();
                myDataProvider.TotalVolume = shipment.Volume != null && shipment.Volume != 0 ? shipment.Volume + " " + (volumeUnitCode) : "";
                myDataProvider.TotalVolumetricWeight = shipment.VolumetricWeight != null && shipment.VolumetricWeight != 0 ? shipment.VolumetricWeight + " " + (volumetricUnitCode) : "";

                myDataProvider.TotalWeight = shipment.GrossWeight != null && shipment.GrossWeight != 0 ? String.Format("{0:0,0.00}", shipment.GrossWeight.Value) + " " + (shipment.GrossWeightUnitCode != null ? shipment.GrossWeightUnitCode : "") : "";

                if (shipment.ShipmentTypeId != "FCL" && shipment.ShipmentTypeId != "FTL" && shipment.ShipmentTypeId != "FCLD")
                {
                    myDataProvider.Dimensions = myDataProvider.TotalVolumetricWeight;
                }
                #endregion

                #region Prepaid Collect
                if (!string.IsNullOrEmpty(shipment.FreightPrepaidCollectId))
                {
                    PrepaidCollect shipmentprepaidcollect = (from a in webfreightContext.PrepaidCollects
                                                             where a.Id == shipment.FreightPrepaidCollectId
                                                             select a).FirstOrDefault();

                    myDataProvider.PrepaidCollect = shipmentprepaidcollect != null ? shipmentprepaidcollect.Name : "";
                }
                #endregion

                #region Freight Location
                Card freightLocationWarehouse = null;
                Address freightLocationWarehouseAddress = null;
                if (!string.IsNullOrEmpty(shipment.FreightLocationId))
                {
                    freightLocationWarehouse = (from a in commonContext.Cards where a.Id == shipment.FreightLocationId select a).FirstOrDefault();
                    freightLocationWarehouseAddress = addressRepository.GetMainAddressByCardId(shipment.FreightLocationId, tenant);

                    if (freightLocationWarehouse != null)
                    {
                        myDataProvider.FreightLocation = freightLocationWarehouse.EnglishName;
                        myDataProvider.FreightLocationName = freightLocationWarehouse.EnglishName;
                        myDataProvider.FreightLocationLocalName = freightLocationWarehouse.LocalName;
                        myDataProvider.FreightLocationCode = freightLocationWarehouse.Code;
                    }
                }

                if (!string.IsNullOrEmpty(shipment.WarehouseLegWarehouseId))
                {
                    Card WarehouseLeg = (from a in commonContext.Cards
                                         where a.Id == shipment.WarehouseLegWarehouseId
                                         select a).FirstOrDefault();

                    if (WarehouseLeg != null)
                    {
                        myDataProvider.FreightLocationAddress = WarehouseLeg.EnglishName;

                        if (!string.IsNullOrEmpty(shipment.WarehouseLegAddressId))
                        {
                            Address warehouseAddress = addressRepository.GetSingleAddress(shipment.WarehouseLegAddressId, tenant);

                            if (warehouseAddress != null)
                            {
                                myDataProvider.FreightLocationAddress += Environment.NewLine + DataProviders.General.GetAddress(warehouseAddress);
                            }
                        }
                    }
                }

                else if (!string.IsNullOrEmpty(shipment.FreightLocationId))
                {
                    if (freightLocationWarehouse != null)
                    {
                        myDataProvider.FreightLocationAddress = freightLocationWarehouse.EnglishName != null ? freightLocationWarehouse.EnglishName : "";
                    }

                    if (freightLocationWarehouseAddress != null)
                    {
                        if (freightLocationWarehouseAddress.IsLocalLanguage && !string.IsNullOrEmpty(myDataProvider.FreightLocationLocalName))
                        {
                            myDataProvider.FreightLocationAddress = myDataProvider.FreightLocationLocalName;
                        }

                        myDataProvider.FreightLocationAddress = myDataProvider.FreightLocationAddress + Environment.NewLine + DataProviders.General.GetAddress(freightLocationWarehouseAddress);

                        if (freightLocationWarehouseAddress.PhoneNumber != null || freightLocationWarehouseAddress.FaxNumber != null)
                        {
                            myDataProvider.FreightLocationAddress = myDataProvider.FreightLocationAddress + Environment.NewLine + (freightLocationWarehouseAddress.PhoneNumber != null ? "Tel: " + freightLocationWarehouseAddress.PhoneNumber + " " : "") + (freightLocationWarehouseAddress.FaxNumber != null ? "Fax: " + freightLocationWarehouseAddress.FaxNumber + " " : "");
                        }
                    }
                }
                #endregion

                #region Move Type
                if (!string.IsNullOrEmpty(shipment.MoveTypeId))
                {
                    MoveType moveType = webfreightContext.MoveTypes.Where(m => m.Id == shipment.MoveTypeId).FirstOrDefault();

                    if (moveType != null)
                    {
                        myDataProvider.MoveTypeCode = moveType.Code;
                        myDataProvider.MoveTypeName = moveType.MoveTypeEnglishName;
                        myDataProvider.MoveType = moveType.MoveTypeEnglishName;
                    }
                }
                #endregion

                #region Shipper

                if (!string.IsNullOrEmpty(shipment.ShipperId))
                {
                    Card shipperClient = (from a in commonContext.Cards
                                          where a.Id == shipment.ShipperId
                                          select a).FirstOrDefault();

                    myDataProvider.ShipperName = shipperClient != null ? shipperClient.EnglishName : "";
                    myDataProvider.ClientNumber = shipperClient != null ? shipperClient.Code : "";
                    myDataProvider.ShipperVAT = shipperClient != null ? shipperClient.VatNumber : "";

                    myDataProvider.ShipperReference = shipment.ShipperReference1;

                    myDataProvider.ShipperAddress = shipperClient != null ? shipperClient.EnglishName : "";
                    myDataProvider.ShipperAddress_NoTel = shipperClient != null ? shipperClient.EnglishName : "";

                    if (!string.IsNullOrEmpty(shipment.ShipperAddressId))
                    {
                        Address shipperClientAddress = addressRepository.GetSingleAddress(shipment.ShipperAddressId, tenant);

                        if (shipperClientAddress != null)
                        {
                            if (shipperClientAddress.IsLocalLanguage)
                            {
                                if (shipperClient != null && !string.IsNullOrEmpty(shipperClient.LocalName))
                                {
                                    myDataProvider.ShipperName = shipperClient.LocalName;
                                    myDataProvider.ShipperAddress = shipperClient.LocalName;
                                    myDataProvider.ShipperAddress_NoTel = shipperClient.LocalName;
                                }
                            }

                            myDataProvider.ShipperAddress = myDataProvider.ShipperAddress + Environment.NewLine + DataProviders.General.GetAddress(shipperClientAddress);
                            myDataProvider.ShipperAddress_NoTel = myDataProvider.ShipperAddress_NoTel + Environment.NewLine + DataProviders.General.GetAddress(shipperClientAddress);

                            if (shipperClientAddress.PhoneNumber != null || shipperClientAddress.FaxNumber != null)
                            {
                                myDataProvider.ShipperAddress = myDataProvider.ShipperAddress + Environment.NewLine + (shipperClientAddress.PhoneNumber != null ? "Tel: " + shipperClientAddress.PhoneNumber + " " : "") + (shipperClientAddress.FaxNumber != null ? "Fax: " + shipperClientAddress.FaxNumber + " " : "");
                            }

                            if (shipperClientAddress.FaxNumber != null)
                            {
                                myDataProvider.ShipperAddress_NoTel = myDataProvider.ShipperAddress_NoTel + Environment.NewLine + "Fax: " + shipperClientAddress.FaxNumber;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(shipment.ShipperContactId))
                    {
                        Contact shipperContact = contactRepository.GetSingleContact(shipment.ShipperContactId, tenant);

                        if (shipperContact != null)
                        {
                            myDataProvider.ShipperContactDetails = shipperContact.EnglishName;

                            if (!string.IsNullOrEmpty(shipperContact.BusinessPhone))
                            {
                                myDataProvider.ShipperContactDetails = myDataProvider.ShipperContactDetails + Environment.NewLine + "Ph: " + shipperContact.BusinessPhone;

                                if (!string.IsNullOrEmpty(shipperContact.Fax))
                                {
                                    myDataProvider.ShipperContactDetails = myDataProvider.ShipperContactDetails + " - Fx: " + shipperContact.Fax;
                                }
                            }

                            else
                            {
                                if (!string.IsNullOrEmpty(shipperContact.Fax))
                                {
                                    myDataProvider.ShipperContactDetails = myDataProvider.ShipperContactDetails + Environment.NewLine + "Fx: " + shipperContact.Fax;
                                }
                            }

                            if (!string.IsNullOrEmpty(shipperContact.Email))
                            {
                                myDataProvider.ShipperContactDetails = myDataProvider.ShipperContactDetails + Environment.NewLine + "Email: " + shipperContact.Email;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(shipment.Notify1ContactId))
                    {
                        Contact myContact = contactRepository.GetSingleContact(shipment.Notify1ContactId, tenant);

                        if (myContact != null)
                        {
                            myDataProvider.Notify1ContactDetails = myContact.EnglishName;

                            if (!string.IsNullOrEmpty(myContact.BusinessPhone))
                            {
                                myDataProvider.Notify1ContactDetails = myDataProvider.Notify1ContactDetails + Environment.NewLine + "Ph: " + myContact.BusinessPhone;

                                if (!string.IsNullOrEmpty(myContact.Fax))
                                {
                                    myDataProvider.Notify1ContactDetails = myDataProvider.Notify1ContactDetails + " - Fx: " + myContact.Fax;
                                }
                            }

                            else
                            {
                                if (!string.IsNullOrEmpty(myContact.Fax))
                                {
                                    myDataProvider.Notify1ContactDetails = myDataProvider.Notify1ContactDetails + Environment.NewLine + "Fx: " + myContact.Fax;
                                }
                            }

                            if (!string.IsNullOrEmpty(myContact.Email))
                            {
                                myDataProvider.Notify1ContactDetails = myDataProvider.Notify1ContactDetails + Environment.NewLine + "Email: " + myContact.Email;
                            }
                        }
                    }


                    if (!string.IsNullOrEmpty(shipment.Notify2ContactId))
                    {
                        Contact myContact = contactRepository.GetSingleContact(shipment.Notify2ContactId, tenant);

                        if (myContact != null)
                        {
                            myDataProvider.Notify2ContactDetails = myContact.EnglishName;

                            if (!string.IsNullOrEmpty(myContact.BusinessPhone))
                            {
                                myDataProvider.Notify2ContactDetails = myDataProvider.Notify2ContactDetails + Environment.NewLine + "Ph: " + myContact.BusinessPhone;

                                if (!string.IsNullOrEmpty(myContact.Fax))
                                {
                                    myDataProvider.Notify2ContactDetails = myDataProvider.Notify2ContactDetails + " - Fx: " + myContact.Fax;
                                }
                            }

                            else
                            {
                                if (!string.IsNullOrEmpty(myContact.Fax))
                                {
                                    myDataProvider.Notify2ContactDetails = myDataProvider.Notify2ContactDetails + Environment.NewLine + "Fx: " + myContact.Fax;
                                }
                            }

                            if (!string.IsNullOrEmpty(myContact.Email))
                            {
                                myDataProvider.Notify2ContactDetails = myDataProvider.Notify2ContactDetails + Environment.NewLine + "Email: " + myContact.Email;
                            }
                        }
                    }

                }
                #endregion

                #region Consignee

                if (!string.IsNullOrEmpty(shipment.ConsigneeId))
                {
                    Card consignee = (from a in commonContext.Cards
                                      where a.Id == shipment.ConsigneeId
                                      select a).FirstOrDefault();

                    myDataProvider.ConsigneeName = consignee != null ? consignee.EnglishName : "";
                    myDataProvider.ConsigneeVAT = consignee != null ? consignee.VatNumber : "";

                    if (!string.IsNullOrEmpty(shipment.ConsigneeContactId))
                    {
                        Contact consigneeContact = contactRepository.GetSingleContact(shipment.ConsigneeContactId, tenant);

                        if (consigneeContact != null)
                        {
                            myDataProvider.ConsigneeContactDetails = consigneeContact.EnglishName;

                            if (!string.IsNullOrEmpty(consigneeContact.BusinessPhone))
                            {
                                myDataProvider.ConsigneeContactDetails = myDataProvider.ConsigneeContactDetails + Environment.NewLine + "Ph: " + consigneeContact.BusinessPhone;

                                if (!string.IsNullOrEmpty(consigneeContact.Fax))
                                {
                                    myDataProvider.ConsigneeContactDetails = myDataProvider.ConsigneeContactDetails + " - Fx: " + consigneeContact.Fax;
                                }
                            }

                            else
                            {
                                if (!string.IsNullOrEmpty(consigneeContact.Fax))
                                {
                                    myDataProvider.ConsigneeContactDetails = myDataProvider.ConsigneeContactDetails + Environment.NewLine + "Fx: " + consigneeContact.Fax;
                                }
                            }

                            if (!string.IsNullOrEmpty(consigneeContact.Email))
                            {
                                myDataProvider.ConsigneeContactDetails = myDataProvider.ConsigneeContactDetails + Environment.NewLine + "Email: " + consigneeContact.Email;
                            }
                        }
                    }
                }
                #endregion

                #region Custom Clearance Point
                if (shipment.CustomClearancePointId != null)
                {
                    WarehousePM CustomClearancePoint = warehouseQuery.GetSingleWarehousePM(shipment.CustomClearancePointId, shipment.Tenant);

                    if (CustomClearancePoint != null)
                    {
                        myDataProvider.CustomClearencePointCode = CustomClearancePoint.Card.Code;
                        myDataProvider.CustomClearencePointName = CustomClearancePoint.Card.EnglishName;
                    }
                }
                #endregion

                #region Coloader
                if (!string.IsNullOrEmpty(shipment.ColoaderReference1))
                {
                    myDataProvider.ColoaderRefLable = "SUB B/L";
                    myDataProvider.ColoaderRef = shipment.ColoaderReference1;
                }
                #endregion

                #region Carrier

                myDataProvider.MainCarriageCarrierName = shipment.MainCarriageCarrierName;
                myDataProvider.VoyageNumber = shipment.MainCarriageCarrierNumber;

                if (shipment.TransportModeId == "A")
                {
                    myDataProvider.CarrierNumberLabel = "AIRLINE/ FLIGHT NO.";
                }
                else if (shipment.TransportModeId == "I")
                {
                    myDataProvider.CarrierNumberLabel = "TRUCKER/ TRUCK NO.";
                }
                else if (shipment.TransportModeId == "O")
                {
                    myDataProvider.CarrierNumberLabel = "VESSEL/ VOYAGE NO.";
                }

                if (shipment.TransportModeId == "O")
                {
                    if (shipment.MainCarriageVesselId != null)
                    {
                        Vessel maincarriagevessel = (from a in commonContext.Vessels
                                                     where a.Id == shipment.MainCarriageVesselId
                                                     select a).FirstOrDefault();

                        if (maincarriagevessel != null)
                        {
                            myDataProvider.CarrierNumber = maincarriagevessel.EnglishName + " " + shipment.MainCarriageCarrierNumber;
                        }
                    }
                }

                else if (shipment.TransportModeId == "A")
                {
                    myDataProvider.CarrierNumber = (shipment.MainCarriageCarrierNumber != null && shipment.MainCarriageCarrierCode != null) ? shipment.MainCarriageCarrierCode + shipment.MainCarriageCarrierNumber : null;
                }

                else if (shipment.TransportModeId == "I")
                {
                    Trucker trucker = (from a in commonContext.Truckers.Include("Card")
                                       where a.Id == shipment.MainCarriageCarrierId
                                       select a).FirstOrDefault();

                    if (trucker != null)
                    {
                        myDataProvider.CarrierNumber = trucker.Card.EnglishName + " " + shipment.MainCarriageCarrierNumber;
                    }
                }
                #endregion

                #region IssuingCarrier
                if (!string.IsNullOrEmpty(shipment.IssuingCarrierAgentId))
                {
                    Card myIssuingCarrier = (from a in commonContext.Cards where a.Id == shipment.IssuingCarrierAgentId select a).FirstOrDefault();
                    if (myIssuingCarrier != null)
                    {
                        myDataProvider.IssuingCarrierAgentName = myIssuingCarrier.EnglishName;
                    }
                }
                #endregion

                #region Custom Agent Import|Broker
                if (!string.IsNullOrEmpty(shipment.CustomAgentImportId))
                {
                    Card customAgentImport = (from a in commonContext.Cards
                                              where a.Id == shipment.CustomAgentImportId
                                              select a).FirstOrDefault();
                    myDataProvider.CustomsAgent = customAgentImport.EnglishName;
                    myDataProvider.Broker = customAgentImport != null ? customAgentImport.EnglishName : "";
                    myDataProvider.BrokerName = customAgentImport != null ? customAgentImport.EnglishName : "";
                    if (customAgentImport != null)
                    {
                        myDataProvider.TotalPayablesForCustomsAgent = shipment.ShipmentPayables.Where(s => s.VendorId == customAgentImport.Id).Sum(p => p.OpenAmountInLocalCurrency);

                    }

                    if (shipment.CustomAgentImportAddressId != null)
                    {
                        Address customAgentImportAddress = addressRepository.GetSingleAddress(shipment.CustomAgentImportAddressId, tenant);
                        if (customAgentImportAddress != null)
                        {
                            if (customAgentImportAddress.IsLocalLanguage)
                            {
                                if (customAgentImport != null && !string.IsNullOrEmpty(customAgentImport.LocalName))
                                {
                                    myDataProvider.Broker = customAgentImport.LocalName;
                                }
                            }

                            myDataProvider.Broker = myDataProvider.Broker + Environment.NewLine + DataProviders.General.GetAddress(customAgentImportAddress);

                            if (customAgentImportAddress.PhoneNumber != null || customAgentImportAddress.FaxNumber != null)
                            {
                                myDataProvider.Broker = myDataProvider.Broker + Environment.NewLine + (customAgentImportAddress.PhoneNumber != null ? "Tel: " + customAgentImportAddress.PhoneNumber + " " : "") + (customAgentImportAddress.FaxNumber != null ? "Fax: " + customAgentImportAddress.FaxNumber + " " : "");
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(shipment.CustomAgentImportContactId))
                    {
                        Contact customAgentImportContact = contactRepository.GetSingleContact(shipment.CustomAgentImportContactId, tenant);

                        if (customAgentImportContact != null)
                        {
                            myDataProvider.BrokerEmail = customAgentImportContact.Email;
                        }
                    }
                }
                #endregion

                #region Custom Agent Export
                if (!string.IsNullOrEmpty(shipment.CustomAgentExportId))
                {
                    Card customAgentExport = (from a in commonContext.Cards
                                              where a.Id == shipment.CustomAgentExportId
                                              select a).FirstOrDefault();

                    myDataProvider.CustomsAgent = customAgentExport.EnglishName + "";
                    if (customAgentExport != null)
                    {
                        //myDataProvider.TotalPayablesForCustomsAgent = shipment.ShipmentPayables.Where(s => s.VendorId == customAgentExport.Id).Sum(p => p.OpenAmountInLocalCurrency) + shipment.ShipmentPayables.Where(s => s.VendorId == customAgentExport.Id).Sum(p => p.AccountedAmountInLocalCurrency);
                        myDataProvider.TotalPayablesForCustomsAgent = shipment.ShipmentPayables.Where(s => s.VendorId == customAgentExport.Id).Sum(p => p.OpenAmountInLocalCurrency + p.AccountedAmountInLocalCurrency);
                    }

                }
                #endregion

                #region Consignee
                if (consigneeNotImporter != null)
                {
                    string myResult = "";

                    myResult = consigneeNotImporter.EnglishName != null ? consigneeNotImporter.EnglishName : "";

                    if (shipment.ConsigneeNotImporterAddressId != null)
                    {
                        Address consigneeNotImporterAddress = addressRepository.GetSingleAddress(shipment.ConsigneeNotImporterAddressId, tenant);


                        if (consigneeNotImporterAddress != null)
                        {
                            if (consigneeNotImporterAddress.IsLocalLanguage && !string.IsNullOrEmpty(consigneeNotImporter.LocalName))
                            {
                                myResult = consigneeNotImporter.LocalName;
                            }

                            myResult = myResult + Environment.NewLine + DataProviders.General.GetAddress(consigneeNotImporterAddress);

                            if (consigneeNotImporterAddress.PhoneNumber != null || consigneeNotImporterAddress.FaxNumber != null)
                            {
                                myResult = myResult + Environment.NewLine + (consigneeNotImporterAddress.PhoneNumber != null ? "Tel: " + consigneeNotImporterAddress.PhoneNumber + " " : "") + (consigneeNotImporterAddress.FaxNumber != null ? "Fax: " + consigneeNotImporterAddress.FaxNumber + " " : "");
                            }
                        }
                    }

                    myDataProvider.ConsigneeAddress = myResult;
                }

                else if (myConsigneePartner != null)
                {
                    string myResult = "";

                    myResult = myConsigneePartner.EnglishName != null ? myConsigneePartner.EnglishName : "";

                    if (shipment.ConsigneeAddressId != null)
                    {
                        Address myConsigneePartnerAddress = addressRepository.GetSingleAddress(shipment.ConsigneeAddressId, tenant);

                        if (myConsigneePartnerAddress != null)
                        {
                            if (myConsigneePartnerAddress.IsLocalLanguage && !string.IsNullOrEmpty(myConsigneePartner.LocalName))
                            {
                                myResult = myConsigneePartner.LocalName;
                            }

                            myResult = myResult + Environment.NewLine + DataProviders.General.GetAddress(myConsigneePartnerAddress);

                            if (myConsigneePartnerAddress.PhoneNumber != null || myConsigneePartnerAddress.FaxNumber != null)
                            {
                                myResult = myResult + Environment.NewLine + (myConsigneePartnerAddress.PhoneNumber != null ? "Tel: " + myConsigneePartnerAddress.PhoneNumber + " " : "") + (myConsigneePartnerAddress.FaxNumber != null ? "Fax: " + myConsigneePartnerAddress.FaxNumber + " " : "");
                            }
                        }
                    }

                    myDataProvider.ConsigneeAddress = myResult;
                }
                #endregion

                #region MainCarriageCarrier | Messers
                if (!string.IsNullOrEmpty(shipment.MainCarriageCarrierId))
                {
                    Card mainCarriageCarrier = (from a in commonContext.Cards
                                                where a.Id == shipment.MainCarriageCarrierId
                                                select a).FirstOrDefault();

                    if (mainCarriageCarrier != null)
                    {
                        myDataProvider.Messers = mainCarriageCarrier.EnglishName;
                        myDataProvider.TotalPayablesForMainCarriageCarrier = shipment.ShipmentPayables.Where(s => s.VendorId == mainCarriageCarrier.Id).Sum(p => p.OpenAmountInLocalCurrency);
                        if (mainCarriageCarrier.ShippingLine != null)
                        {
                            ShippingAgentRepository shippingAgentRepository = new ShippingAgentRepository(tenant);
                            ShippingAgent shippingAgent = shippingAgentRepository.GetSingleShippingAgent(tenant, mainCarriageCarrier.ShippingLine.ShippingAgentId);

                            if (shippingAgent != null)
                            {
                                myDataProvider.Messers = shippingAgent.Card.EnglishName;
                                myDataProvider.ShippingAgentLocalCustomsCode = shippingAgent.LocalCustomsCode;
                            }
                        }
                    }

                    Address mainCarriageCarrierAddress = addressRepository.GetMainAddressByCardId(shipment.MainCarriageCarrierId, tenant);

                    if (mainCarriageCarrierAddress != null)
                    {
                        myDataProvider.MainCarriageCarrierAddress = DataProviders.General.GetAddress(mainCarriageCarrierAddress);

                        if (mainCarriageCarrierAddress.PhoneNumber != null || mainCarriageCarrierAddress.FaxNumber != null)
                        {
                            myDataProvider.MainCarriageCarrierAddress = myDataProvider.MainCarriageCarrierAddress + Environment.NewLine + (mainCarriageCarrierAddress.PhoneNumber != null ? "Tel: " + mainCarriageCarrierAddress.PhoneNumber + " " : "") + (mainCarriageCarrierAddress.FaxNumber != null ? "Fax: " + mainCarriageCarrierAddress.FaxNumber + " " : "");
                        }
                    }


                    if (mainCarriageCarrier.PartnerTypeId == "SL")
                    {
                        ShippingLineRepository shippingLineRepository = new ShippingLineRepository(tenant);
                        ShippingLine shippingLine = shippingLineRepository.GetSingleShippingLine(mainCarriageCarrier.Id, tenant);
                        myDataProvider.CarrierCAAT = shippingLine != null ? shippingLine.CAAT : null;
                        myDataProvider.CarrierCBSA = shippingLine != null ? shippingLine.CBSA : null;
                    }
                }

                myDataProvider.Messers = shipment.BookingConfirmationNumber != null ? myDataProvider.Messers + Environment.NewLine + "Booking: " + shipment.BookingConfirmationNumber : myDataProvider.Messers;
                myDataProvider.Messers = shipment.ShipmentNumber != null ? myDataProvider.Messers + Environment.NewLine + "Shipment No. " + shipment.ShipmentNumber : myDataProvider.Messers;
                myDataProvider.BookingNumber = shipment.BookingConfirmationNumber != null ? shipment.BookingConfirmationNumber : "";
                #endregion

                #region User

                string contactEmail = AuthenticationUtil.GetLoggedUserEmail(tenant);


                if (!string.IsNullOrEmpty(contactEmail))
                {
                    Contact currentContact = (from a in commonContext.Contacts
                                              where a.Email == contactEmail && a.Tenant == tenant
                                              select a).FirstOrDefault();

                    if (currentContact != null)
                    {
                        myDataProvider.UserName = currentContact.EnglishName;
                        myDataProvider.UserEmail = currentContact.Email != null ? currentContact.Email : "";
                    }
                }
                #endregion

                #region Agent
                if (!string.IsNullOrEmpty(shipment.AgentId))
                {
                    Card agent = (from a in commonContext.Cards
                                  where a.Id == shipment.AgentId
                                  select a).FirstOrDefault();

                    if (agent != null)
                    {
                        string agentDetails = "";

                        myDataProvider.AgentVAT = agent.VatNumber;
                        myDataProvider.AgentInfo = agent.EnglishName;
                        agentDetails = agent.EnglishName;
                        myDataProvider.TotalPayablesForAgent = shipment.ShipmentPayables.Where(s => s.VendorId == agent.Id).Sum(p => p.OpenAmountInLocalCurrency);
                        Address agentAddress = addressRepository.GetSingleAddress(shipment.AgentAddressId, tenant);

                        myDataProvider.AgentInfo = myDataProvider.AgentInfo + ", " + (agentAddress != null ? agentAddress.City : "") + ", " + (agentAddress != null ? (agentAddress.Country != null ? agentAddress.Country.Code : "") : "") + Environment.NewLine;

                        if (agentAddress != null)
                        {
                            if (!string.IsNullOrEmpty(agentAddress.Address1))
                            {
                                agentDetails = agentDetails + Environment.NewLine + agentAddress.Address1;
                            }

                            if (!string.IsNullOrEmpty(agentAddress.Address2))
                            {
                                agentDetails = agentDetails + Environment.NewLine + agentAddress.Address2;
                            }

                            if (!string.IsNullOrEmpty(agentAddress.ZipCode))
                            {
                                agentDetails = agentDetails + Environment.NewLine + agentAddress.ZipCode;

                                if (!string.IsNullOrEmpty(agentAddress.City))
                                {
                                    agentDetails = agentDetails + " " + agentAddress.City;
                                }

                                if (agentAddress.Country != null)
                                {
                                    agentDetails = agentDetails + " - " + agentAddress.Country.EnglishName;
                                }
                            }
                            else
                            {
                                if (agentAddress.Country == null)
                                {
                                    agentDetails = agentDetails + Environment.NewLine + agentAddress.City;
                                }
                                else
                                {
                                    agentDetails = agentDetails + Environment.NewLine + agentAddress.City + " - " + agentAddress.Country.EnglishName;
                                }
                            }

                            if (!string.IsNullOrEmpty(agentAddress.PhoneNumber) && !string.IsNullOrEmpty(agentAddress.FaxNumber))
                            {
                                agentDetails = agentDetails + Environment.NewLine + "Ph: " + agentAddress.PhoneNumber + " - Fx: " + agentAddress.FaxNumber;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(agentAddress.PhoneNumber))
                                {
                                    agentDetails = agentDetails + Environment.NewLine + "Ph: " + agentAddress.PhoneNumber;
                                }
                                else if (!string.IsNullOrEmpty(agentAddress.FaxNumber))
                                {
                                    agentDetails = agentDetails + Environment.NewLine + "Fx: " + agentAddress.FaxNumber;
                                }
                            }
                        }

                        CardContact agentCardContact = (from cardcon in commonContext.CardContacts
                                                        where cardcon.CardId == agent.Id
                                                        select cardcon).FirstOrDefault();
                        if (agentCardContact != null)
                        {
                            Contact agentContact = (from cont in commonContext.Contacts
                                                    where cont.Id == agentCardContact.ContactId
                                                    select cont).FirstOrDefault();
                            if (agentContact != null)
                            {
                                myDataProvider.AgentInfo = myDataProvider.AgentInfo + agentContact.EnglishName + ", " + agentContact.BusinessPhone;

                                if (!string.IsNullOrEmpty(agentContact.Email))
                                {
                                    agentDetails = agentDetails + Environment.NewLine + "E-mail: " + agentContact.Email;
                                }
                            }
                        }

                        myDataProvider.AgentFullDetails = agentDetails;
                    }
                }
                #endregion

                #region FreightForwarder
                if (!string.IsNullOrEmpty(shipment.FreightForwarderId))
                {
                    if (!string.IsNullOrEmpty(shipment.FreightForwarderAddressId))
                    {
                        Address freightForwarderAddress = addressRepository.GetSingleAddress(shipment.FreightForwarderAddressId, tenant);
                        if (freightForwarderAddress != null)
                        {
                            myDataProvider.FreightForwardedAddress = freightForwarderAddress.Name;
                            myDataProvider.FreightForwardedAddressWithoutCountry = freightForwarderAddress.Name;

                            if (!string.IsNullOrEmpty(freightForwarderAddress.Address1))
                            {
                                myDataProvider.FreightForwardedAddress = myDataProvider.FreightForwardedAddress + Environment.NewLine + freightForwarderAddress.Address1;
                                myDataProvider.FreightForwardedAddressWithoutCountry = myDataProvider.FreightForwardedAddressWithoutCountry + Environment.NewLine + freightForwarderAddress.Address1;
                            }

                            if (!string.IsNullOrEmpty(freightForwarderAddress.Address2))
                            {
                                myDataProvider.FreightForwardedAddress = myDataProvider.FreightForwardedAddress + Environment.NewLine + freightForwarderAddress.Address2;
                                myDataProvider.FreightForwardedAddressWithoutCountry = myDataProvider.FreightForwardedAddressWithoutCountry + Environment.NewLine + freightForwarderAddress.Address2;
                            }

                            if (!string.IsNullOrEmpty(freightForwarderAddress.City))
                            {
                                myDataProvider.FreightForwardedAddress = myDataProvider.FreightForwardedAddress + Environment.NewLine + freightForwarderAddress.City;
                                myDataProvider.FreightForwardedAddressWithoutCountry = myDataProvider.FreightForwardedAddressWithoutCountry + Environment.NewLine + freightForwarderAddress.City;
                            }

                            if (freightForwarderAddress.State != null)
                            {
                                myDataProvider.FreightForwardedAddress = myDataProvider.FreightForwardedAddress + ", " + freightForwarderAddress.State.Code;
                                myDataProvider.FreightForwardedAddressWithoutCountry = myDataProvider.FreightForwardedAddressWithoutCountry + ", " + freightForwarderAddress.State.Code;
                            }

                            if (!string.IsNullOrEmpty(freightForwarderAddress.ZipCode))
                            {
                                myDataProvider.FreightForwardedAddress = myDataProvider.FreightForwardedAddress + ", " + freightForwarderAddress.ZipCode;
                                myDataProvider.FreightForwardedAddressWithoutCountry = myDataProvider.FreightForwardedAddressWithoutCountry + ", " + freightForwarderAddress.ZipCode;
                            }

                            if (freightForwarderAddress.Country != null)
                            {
                                myDataProvider.FreightForwardedAddress = myDataProvider.FreightForwardedAddress + Environment.NewLine + freightForwarderAddress.Country.EnglishName;
                            }

                            if (!string.IsNullOrEmpty(freightForwarderAddress.PhoneNumber))
                            {
                                myDataProvider.FreightForwardedAddress = myDataProvider.FreightForwardedAddress + Environment.NewLine + "Tel: " + freightForwarderAddress.PhoneNumber;
                                myDataProvider.FreightForwardedAddressWithoutCountry = myDataProvider.FreightForwardedAddressWithoutCountry + Environment.NewLine + "Tel: " + freightForwarderAddress.PhoneNumber;
                            }
                        }
                    }
                }
                #endregion

                #region MainCarriageOBL
                Card MainCarriageCarrier = CardRepository.GetSingleCard(shipment.MainCarriageCarrierId, tenant, false);
                if (shipment.TransportModeId == "A")
                {
                    if (MainCarriageCarrier != null)
                    {
                        myDataProvider.MainCarriageOBL = shipment.Master != null ? shipment.AirlinePrefix + "-" + shipment.Master : "";
                    }

                }

                else
                {
                    myDataProvider.MainCarriageOBL = shipment.Master != null ? shipment.Master : "";
                }
                #endregion

                #region Notify1
                if (notify1 != null)
                {
                    myDataProvider.NotifyAddress = "Notify 1 :" + notify1.EnglishName != null ? notify1.EnglishName : "";

                    if (shipment.Notify1AddressId != null)
                    {
                        Address notify1Address = addressRepository.GetSingleAddress(shipment.Notify1AddressId, tenant);

                        if (notify1Address != null)
                        {
                            if (notify1Address.IsLocalLanguage && !string.IsNullOrEmpty(notify1.LocalName))
                            {
                                myDataProvider.NotifyAddress = "Notify 1 :" + notify1.LocalName != null ? notify1.LocalName : "";
                            }

                            myDataProvider.NotifyAddress = myDataProvider.NotifyAddress + Environment.NewLine + DataProviders.General.GetAddress(notify1Address);

                            if (notify1Address.PhoneNumber != null || notify1Address.FaxNumber != null)
                            {
                                myDataProvider.NotifyAddress = myDataProvider.NotifyAddress + Environment.NewLine + (notify1Address.PhoneNumber != null ? "Tel: " + notify1Address.PhoneNumber + " " : "") + (notify1Address.FaxNumber != null ? "Fax: " + notify1Address.FaxNumber + " " : "");
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(shipment.Notify1ContactId))
                    {
                        Contact notifyContact = contactRepository.GetSingleContact(shipment.Notify1ContactId, tenant);

                        if (notifyContact != null)
                        {
                            myDataProvider.NotifyContactDetails = notifyContact.EnglishName;

                            if (!string.IsNullOrEmpty(notifyContact.BusinessPhone))
                            {
                                myDataProvider.NotifyContactDetails = myDataProvider.NotifyContactDetails + Environment.NewLine + "Ph: " + notifyContact.BusinessPhone;

                                if (!string.IsNullOrEmpty(notifyContact.Fax))
                                {
                                    myDataProvider.NotifyContactDetails = myDataProvider.NotifyContactDetails + " - Fx: " + notifyContact.Fax;
                                }
                            }

                            else
                            {
                                if (!string.IsNullOrEmpty(notifyContact.Fax))
                                {
                                    myDataProvider.NotifyContactDetails = myDataProvider.NotifyContactDetails + Environment.NewLine + "Fx: " + notifyContact.Fax;
                                }
                            }

                            if (!string.IsNullOrEmpty(notifyContact.Email))
                            {
                                myDataProvider.NotifyContactDetails = myDataProvider.NotifyContactDetails + Environment.NewLine + "Email: " + notifyContact.Email;
                            }
                        }
                    }
                }

                else
                {
                    if (myConsigneePartner != null)
                    {
                        string myResult = "";

                        myResult = myConsigneePartner.EnglishName != null ? myConsigneePartner.EnglishName : "";

                        if (shipment.ConsigneeAddressId != null)
                        {
                            Address myAddress = addressRepository.GetSingleAddress(shipment.ConsigneeAddressId, tenant);

                            if (myAddress != null)
                            {
                                if (myAddress.IsLocalLanguage && !string.IsNullOrEmpty(myConsigneePartner.LocalName))
                                {
                                    myResult = myConsigneePartner.LocalName;
                                }

                                myResult = myResult + Environment.NewLine + DataProviders.General.GetAddress(myAddress);

                                if (myAddress.PhoneNumber != null || myAddress.FaxNumber != null)
                                {
                                    myResult = myResult + Environment.NewLine + (myAddress.PhoneNumber != null ? "Tel: " + myAddress.PhoneNumber + " " : "") + (myAddress.FaxNumber != null ? "Fax: " + myAddress.FaxNumber + " " : "");
                                }
                            }
                        }

                        myDataProvider.NotifyAddress = myResult;

                        if (!string.IsNullOrEmpty(shipment.ConsigneeContactId))
                        {
                            Contact consigneeContact = contactRepository.GetSingleContact(shipment.ConsigneeContactId, tenant);

                            if (consigneeContact != null)
                            {
                                myDataProvider.NotifyContactDetails = consigneeContact.EnglishName;

                                if (!string.IsNullOrEmpty(consigneeContact.BusinessPhone))
                                {
                                    myDataProvider.NotifyContactDetails = myDataProvider.NotifyContactDetails + Environment.NewLine + "Ph: " + consigneeContact.BusinessPhone;

                                    if (!string.IsNullOrEmpty(consigneeContact.Fax))
                                    {
                                        myDataProvider.NotifyContactDetails = myDataProvider.NotifyContactDetails + " - Fx: " + consigneeContact.Fax;
                                    }
                                }

                                else
                                {
                                    if (!string.IsNullOrEmpty(consigneeContact.Fax))
                                    {
                                        myDataProvider.NotifyContactDetails = myDataProvider.NotifyContactDetails + Environment.NewLine + "Fx: " + consigneeContact.Fax;
                                    }
                                }

                                if (!string.IsNullOrEmpty(consigneeContact.Email))
                                {
                                    myDataProvider.NotifyContactDetails = myDataProvider.NotifyContactDetails + Environment.NewLine + "Email: " + consigneeContact.Email;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Notify2
                if (notify2 != null)
                {
                    myDataProvider.Notify2Address = "Notify 2:" + notify2.EnglishName != null ? notify2.EnglishName : "";

                    if (shipment.Notify2AddressId != null)
                    {
                        Address notify2Address = addressRepository.GetSingleAddress(shipment.Notify2AddressId, tenant);

                        if (notify2Address != null)
                        {
                            if (notify2Address.IsLocalLanguage && !string.IsNullOrEmpty(notify2.LocalName))
                            {
                                myDataProvider.Notify2Address = "Notify 2:" + notify2.LocalName != null ? notify2.LocalName : "";
                            }

                            myDataProvider.Notify2Address = myDataProvider.Notify2Address + Environment.NewLine + DataProviders.General.GetAddress(notify2Address);

                            if (notify2Address.PhoneNumber != null || notify2Address.FaxNumber != null)
                            {
                                myDataProvider.Notify2Address = myDataProvider.Notify2Address + Environment.NewLine + (notify2Address.PhoneNumber != null ? "Tel: " + notify2Address.PhoneNumber + " " : "") + (notify2Address.FaxNumber != null ? "Fax: " + notify2Address.FaxNumber + " " : "");
                            }
                        }
                    }

                    //Add * when more data exist and no place to show ( they will be shown in other place)
                    myDataProvider.NotifyAddress = myDataProvider.NotifyAddress + " *";
                    myDataProvider.Notify2Address = "* " + myDataProvider.Notify2Address;
                }
                #endregion

                #region Master Number
                if (shipment.TransportModeId == "A")
                {
                    string myPrefix = shipment.AirlinePrefix;

                    if (!string.IsNullOrEmpty(myPrefix))
                    {
                        myDataProvider.MasterNumber = shipment.Master != null ? myPrefix + "-" + shipment.Master : "";
                    }

                    else
                    {
                        myDataProvider.MasterNumber = shipment.Master != null ? shipment.Master : "";
                    }
                }

                else
                {
                    myDataProvider.MasterNumber = shipment.Master != null ? shipment.Master : "";
                }

                myDataProvider.MainCarriageMAWBOBLBL = shipment.Master != null ? shipment.Master : "";

                if (!string.IsNullOrEmpty(shipment.AirlinePrefix) && !string.IsNullOrEmpty(myDataProvider.MainCarriageMAWBOBLBL))
                {
                    myDataProvider.MainCarriageMAWBOBLBL = shipment.AirlinePrefix + "-" + myDataProvider.MainCarriageMAWBOBLBL;
                }
                #endregion

                #region finalDistinationPort
                Port finalDistinationPort = (from a in commonContext.Ports.Include("Country")
                                             where a.Id == shipment.MainCarriageFinalDestinationPortId
                                             select a).FirstOrDefault();

                if (shipment.Transshipment1FromPortId == null && shipment.Transshipment2FromPortId == null && shipment.Transshipment3FromPortId == null && finalDistinationPort == null)
                {
                    myDataProvider.MainCarriageLastDestination_Label = "";
                }

                if (shipment.TransportModeId == "A")
                {
                    myDataProvider.MainCarriageMAWBOBLBL_Label = "M.A.W.B";
                    myDataProvider.MainCarriageCarrierType_Label = "Flight";
                    myDataProvider.MainCarriageCarrier_Label = "Airline";
                    myDataProvider.MainCarriageLastDestination_Label = "Final Destination";
                }
                else if (shipment.TransportModeId == "O")
                {
                    myDataProvider.MainCarriageMAWBOBLBL_Label = "MBL";
                    myDataProvider.MainCarriageCarrierType_Label = "Voyage";
                    myDataProvider.MainCarriageCarrier_Label = "Shipping line";
                    myDataProvider.MainCarriageLastDestination_Label = "Discharge Port";
                    myDataProvider.MainCarriageVessel_Label = "Vessel";
                }
                else if (shipment.TransportModeId == "I")
                {
                    myDataProvider.MainCarriageMAWBOBLBL_Label = "CMR";
                    myDataProvider.MainCarriageCarrierType_Label = "Truck";
                    myDataProvider.MainCarriageCarrier_Label = "Trucker";
                    myDataProvider.MainCarriageLastDestination_Label = "Final Destination";
                    myDataProvider.MainCarriageVessel_Label = "";
                }

                #endregion

                #region ReleasingAgent
                string myReleasingAgentId = shipment.ReleasingAgentId;
                string myReleasingAgentAddressId = shipment.ReleasingAgentAddressId;
                if (!string.IsNullOrEmpty(myReleasingAgentId))
                {
                    Card myPartnerCard = CardRepository.GetSingleCard(myReleasingAgentId, tenant, true);

                    if (myPartnerCard != null)
                    {
                        myDataProvider.ReleasingAgentAddress = myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";
                        myDataProvider.ReleasingAgentName = myPartnerCard.EnglishName;
                        if (!string.IsNullOrEmpty(myReleasingAgentAddressId))
                        {
                            Address myPartnerAddress = addressRepository.GetSingleAddress(myReleasingAgentAddressId, tenant);

                            if (myPartnerAddress != null)
                            {
                                if (myPartnerAddress.IsLocalLanguage && !string.IsNullOrEmpty(myPartnerCard.LocalName))
                                {
                                    myDataProvider.ReleasingAgentAddress = myPartnerCard.LocalName + Environment.NewLine;
                                }

                                myDataProvider.ReleasingAgentAddress = myDataProvider.ReleasingAgentAddress + DataProviders.General.GetAddress(myPartnerAddress);

                                if (myPartnerAddress.PhoneNumber != null || myPartnerAddress.FaxNumber != null)
                                {
                                    myDataProvider.ReleasingAgentAddress = myDataProvider.ReleasingAgentAddress + Environment.NewLine + (myPartnerAddress.PhoneNumber != null ? "Tel: " + myPartnerAddress.PhoneNumber + " " : "") + (myPartnerAddress.FaxNumber != null ? "Fax: " + myPartnerAddress.FaxNumber + " " : "");
                                }
                            }
                        }
                    }
                }
                #endregion

                #region PlaceOfReceipt
                ShipmentPickUpDelivery myFirstPickup =
                    (from d in shipmentsContext.ShipmentPickUpDeliveries
                     where d.ShipmentId == shipment.Id && d.PickUpDeliveryTypeCode == "PICK"
                     select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();

                if (myFirstPickup != null)
                {
                    if (myFirstPickup.PickUpDeliveryFromTypeCode == "PART")
                    {
                        if (!string.IsNullOrEmpty(myFirstPickup.FromPartnerCardId))
                        {
                            Address myPartnerAddress = addressRepository.GetMainAddressByCardId(myFirstPickup.FromPartnerCardId, tenant);
                            if (myPartnerAddress != null)
                            {
                                myDataProvider.FullPickupAddress = DataProviders.General.GetAddress(myPartnerAddress);
                            }
                        }
                    }

                    myDataProvider.FirstPickupETD = myFirstPickup.ETD;
                    PlaceOfReceiptData data = myServicHelper.GetPlaceOfReceiptData(myFirstPickup);

                    if (data != null)
                    {
                        myDataProvider.PlaceOfReceipt = data.City;
                        myDataProvider.PlaceOfReceiptCountryCode = data.CountryCode;
                        myDataProvider.PlaceOfReceiptCountryName = data.CountryName;
                        myDataProvider.PlaceOfReceiptStateCode = data.StateCode;
                    }

                    if (!string.IsNullOrEmpty(myFirstPickup.CarrierId))
                    {
                        Card pickupTrucker = commonContext.Cards.Where(d => d.Id == myFirstPickup.CarrierId).FirstOrDefault();
                        if (pickupTrucker != null)
                        {
                            myDataProvider.PickupTruckerName = pickupTrucker.EnglishName;
                            myDataProvider.PickupTruckerInfo = pickupTrucker.EnglishName;

                            Address pickupTruckerAddress = addressRepository.GetMainAddressByCardId(pickupTrucker.Id, tenant);
                            if (pickupTruckerAddress != null)
                            {
                                myDataProvider.PickupTruckerInfo = myDataProvider.PickupTruckerInfo + Environment.NewLine + DataProviders.General.GetAddress(pickupTruckerAddress);
                            }
                        }
                    }
                }

                else
                {
                    if (preCarriageFromPort != null)
                    {
                        myDataProvider.PlaceOfReceipt = preCarriageFromPort.EnglishName;

                        PortPM myPort = PortQuery.GetSinglePort(tenant, shipment.PreCarriageFromPortId, true);
                        if (myPort != null)
                        {
                            myDataProvider.PlaceOfReceiptCountryCode = myPort.CountryCode;
                            myDataProvider.PlaceOfReceiptCountryName = myPort.CountryName;
                            myDataProvider.PlaceOfReceiptStateCode = myPort.StateCode;
                        }
                    }

                    else if (!string.IsNullOrEmpty(shipment.ShipperAddressId))
                    {
                        Address myPartnerAddress = addressRepository.GetSingleAddress(shipment.ShipperAddressId, tenant);
                        if (myPartnerAddress != null)
                        {
                            myDataProvider.PlaceOfReceipt = myPartnerAddress.City;
                            myDataProvider.PlaceOfReceiptCountryCode = myPartnerAddress.Country == null ? null : myPartnerAddress.Country.Code;
                            myDataProvider.PlaceOfReceiptCountryName = myPartnerAddress.Country == null ? null : myPartnerAddress.Country.EnglishName;
                            myDataProvider.PlaceOfReceiptStateCode = myPartnerAddress.State == null ? null : myPartnerAddress.State.Code;
                        }
                    }
                }
                #endregion

                #region PreCarriage
                if (!string.IsNullOrEmpty(shipment.PreCarriageCarrierId))
                {
                    Card preCarriageCarrier = (from a in commonContext.Cards
                                               where a.Id == shipment.PreCarriageCarrierId
                                               select a).FirstOrDefault();

                    if (preCarriageCarrier != null)
                    {
                        myDataProvider.PreCarriageCarrierAddress = preCarriageCarrier.EnglishName;

                        Address preCarriageCarrierAddress = (from a in commonContext.Addresses
                                                             where a.CardId == shipment.PreCarriageCarrierId && a.AddressTypeId == "M"
                                                             select a).FirstOrDefault();

                        if (preCarriageCarrierAddress != null)
                        {
                            if (preCarriageCarrierAddress.IsLocalLanguage && !string.IsNullOrEmpty(preCarriageCarrier.LocalName))
                            {
                                myDataProvider.PreCarriageCarrierAddress = preCarriageCarrier.LocalName;
                            }

                            myDataProvider.PreCarriageCarrierAddress = myDataProvider.PreCarriageCarrierAddress + Environment.NewLine + DataProviders.General.GetAddress(preCarriageCarrierAddress);

                            if (preCarriageCarrierAddress.PhoneNumber != null || preCarriageCarrierAddress.FaxNumber != null)
                            {
                                myDataProvider.PreCarriageCarrierAddress = myDataProvider.PreCarriageCarrierAddress + Environment.NewLine + (preCarriageCarrierAddress.PhoneNumber != null ? "Tel: " + preCarriageCarrierAddress.PhoneNumber + " " : "") + (preCarriageCarrierAddress.FaxNumber != null ? "Fax: " + preCarriageCarrierAddress.FaxNumber + " " : "");
                            }
                        }
                    }
                }
                #endregion

                myDataProvider.Transshipment2ETD = shipment.Transshipment2ETD;
                myDataProvider.Transshipment2ETA = shipment.Transshipment2ETA;

                if (!string.IsNullOrEmpty(shipment.Transshipment1FromPortId))
                {
                    myDataProvider.Transshipment1FromPortCode = shipment.Transshipment1FromPortCode;
                }

                if (!string.IsNullOrEmpty(shipment.Transshipment1ToPortId))
                {
                    myDataProvider.Transshipment1ToPortCode = shipment.Transshipment1ToPortCode;
                }

                if (!string.IsNullOrEmpty(shipment.Transshipment2FromPortId))
                {
                    myDataProvider.Transshipment2FromPortCode = shipment.Transshipment2FromPortCode;
                }

                if (!string.IsNullOrEmpty(shipment.Transshipment2ToPortId))
                {
                    myDataProvider.Transshipment2ToPortCode = shipment.Transshipment2ToPortCode;
                }

                //Discharge port
                string DischargePortCountryCode = "";
                string DischargePortCountryName = "";
                string DischargePortStateCode = "";
                if (shipment.Transshipment3ToPortId != null)
                {
                    myDataProvider.DischargePortName = shipment.Transshipment3ToPortName;
                    myDataProvider.DischargePortCode = shipment.Transshipment3ToPortCode;

                    DischargePortCountryCode = shipment.Transshipment3ToPortCountryCode;
                    DischargePortCountryName = shipment.Transshipment3ToPortCountryName;
                    DischargePortStateCode = shipment.Transshipment3ToPortStateCode;
                }
                else if (shipment.Transshipment2ToPortId != null)
                {
                    myDataProvider.DischargePortName = shipment.Transshipment2ToPortName;
                    myDataProvider.DischargePortCode = shipment.Transshipment2ToPortCode;

                    DischargePortCountryCode = shipment.Transshipment2ToPortCountryCode;
                    DischargePortCountryName = shipment.Transshipment2ToPortCountryName;
                    DischargePortStateCode = shipment.Transshipment2ToPortStateCode;
                }
                else if (shipment.Transshipment1ToPortId != null)
                {
                    myDataProvider.DischargePortName = shipment.Transshipment1ToPortName;
                    myDataProvider.DischargePortCode = shipment.Transshipment1ToPortCode;

                    DischargePortCountryCode = shipment.Transshipment1ToPortCountryCode;
                    DischargePortCountryName = shipment.Transshipment1ToPortCountryName;
                    DischargePortStateCode = shipment.Transshipment1ToPortStateCode;
                }
                else if (mainCarriageToPort != null)
                {
                    myDataProvider.DischargePortName = mainCarriageToPort.EnglishName;
                    myDataProvider.DischargePortCode = mainCarriageToPort.Code;

                    DischargePortCountryCode = mainCarriageToPort.Country == null ? "" : mainCarriageToPort.Country.Code;
                    DischargePortCountryName = mainCarriageToPort.Country == null ? "" : mainCarriageToPort.Country.EnglishName;
                    DischargePortStateCode = mainCarriageToPort.State == null ? "" : mainCarriageToPort.State.Code;
                }

                ShipmentPickUpDelivery delivery = shipmentsContext.ShipmentPickUpDeliveries.Where(a => a.PickUpDeliveryTypeCode == "DELV" && a.PickUpDeliveryNumber == shipment.ShipmentNumber + "/" + shipment.ShipmentDeliveryIndex).FirstOrDefault();

                if (delivery != null)
                {
                    myDataProvider.DeliveryATA = delivery.ATA != null ? String.Format("{0:dd.MMM.yy}", delivery.ATA) : "";
                }

                // First Delivert or Last ??
                #region PlaceOfDelivery | Final Destination

                ShipmentPickUpDelivery myDelivery = (from d in shipmentsContext.ShipmentPickUpDeliveries
                                                     where d.ShipmentId == shipment.Id && d.PickUpDeliveryTypeCode == "DELV"
                                                     select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();

                ShipmentPickUpDelivery myLastDelivery = (from d in shipmentsContext.ShipmentPickUpDeliveries
                                                         where d.ShipmentId == shipment.Id && d.PickUpDeliveryTypeCode == "DELV"
                                                         select d).OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();

                if (myLastDelivery != null)
                {
                    #region To
                    switch (myLastDelivery.PickUpDeliveryToTypeCode)
                    {
                        case "PART":
                            {
                                if (!string.IsNullOrEmpty(myLastDelivery.ToPartnerCardId))
                                {
                                    Card toPartner = CardRepository.GetSingleCard(myLastDelivery.ToPartnerCardId, tenant, false);
                                    Address toAddress = addressRepository.GetSingleAddress(myLastDelivery.ToAddressId, tenant);

                                    if (toPartner != null)
                                    {
                                        myDataProvider.DeliveryToName = toPartner.EnglishName;

                                        if (!string.IsNullOrEmpty(toPartner.PrimaryContactId))
                                        {
                                            Contact toPartnerContact = contactRepository.GetSingleContact(toPartner.PrimaryContactId, tenant);
                                            if (toPartnerContact != null)
                                            {
                                                myDataProvider.DeliveryToContactName = toPartnerContact.EnglishName;
                                                myDataProvider.DeliveryToContactEmail = toPartnerContact.Email;
                                                myDataProvider.DeliveryToContactPhone = toPartnerContact.BusinessPhone;
                                                myDataProvider.DeliveryToContactMobile = toPartnerContact.Mobile;
                                            }
                                        }
                                    }

                                    if (toAddress != null)
                                    {
                                        myDataProvider.DeliveryToAddress = DataProviders.General.GetAddress(toAddress);
                                    }
                                }

                                break;
                            }

                        case "CASL":
                            {
                                if (!string.IsNullOrEmpty(myLastDelivery.ToAddressCity))
                                {
                                    myDataProvider.DeliveryToName = myLastDelivery.ToAddressCity;
                                    myDataProvider.DeliveryToAddress = myLastDelivery.ToAddressCity;
                                }

                                if (!string.IsNullOrEmpty(myLastDelivery.ToAddressCountryId))
                                {
                                    Country toAddressCountry = CountryRepository.GetSingleCountry(myLastDelivery.ToAddressCountryId, tenant, false);
                                    if (toAddressCountry != null)
                                    {
                                        if (string.IsNullOrEmpty(myDataProvider.DeliveryToAddress))
                                        {
                                            myDataProvider.DeliveryToAddress = toAddressCountry.EnglishName;
                                        }

                                        else
                                        {
                                            myDataProvider.DeliveryToAddress = myDataProvider.DeliveryToAddress + ", " + toAddressCountry.EnglishName;
                                        }
                                    }
                                }

                                break;
                            }

                        case "PORT":
                            {
                                if (!string.IsNullOrEmpty(myLastDelivery.ToPortId))
                                {
                                    Port myPort = portRepository.GetSinglePort(tenant, myLastDelivery.ToPortId);
                                    if (myPort != null)
                                    {
                                        myDataProvider.DeliveryToName = myPort.EnglishName;
                                        myDataProvider.DeliveryToAddress = myPort.Code + " " + myPort.EnglishName;
                                    }
                                }

                                break;
                            }
                    }
                    #endregion

                    #region From
                    switch (myLastDelivery.PickUpDeliveryFromTypeCode)
                    {
                        case "PART":
                            {
                                if (!string.IsNullOrEmpty(myLastDelivery.FromPartnerCardId))
                                {
                                    Card fromPartner = CardRepository.GetSingleCard(myLastDelivery.FromPartnerCardId, tenant, false);
                                    Address fromAddress = addressRepository.GetSingleAddress(myLastDelivery.FromAddressId, tenant);

                                    if (fromPartner != null)
                                    {
                                        myDataProvider.DeliveryFromName = fromPartner.EnglishName;
                                    }

                                    if (fromAddress != null)
                                    {
                                        myDataProvider.DeliveryFromAddress = DataProviders.General.GetAddress(fromAddress);
                                    }
                                }

                                break;
                            }

                        case "CASL":
                            {
                                if (!string.IsNullOrEmpty(myLastDelivery.FromAddressCity))
                                {
                                    myDataProvider.DeliveryFromName = myLastDelivery.FromAddressCity;
                                    myDataProvider.DeliveryFromAddress = myLastDelivery.FromAddressCity;
                                }

                                if (!string.IsNullOrEmpty(myLastDelivery.FromAddressCountryId))
                                {
                                    Country fromAddressCountry = CountryRepository.GetSingleCountry(myLastDelivery.FromAddressCountryId, tenant, false);
                                    if (fromAddressCountry != null)
                                    {
                                        if (string.IsNullOrEmpty(myDataProvider.DeliveryFromAddress))
                                        {
                                            myDataProvider.DeliveryFromAddress = fromAddressCountry.EnglishName;
                                        }

                                        else
                                        {
                                            myDataProvider.DeliveryFromAddress = myDataProvider.DeliveryFromAddress + ", " + fromAddressCountry.EnglishName;
                                        }
                                    }
                                }

                                break;
                            }

                        case "PORT":
                            {
                                if (!string.IsNullOrEmpty(myLastDelivery.FromPortId))
                                {
                                    Port myPort = portRepository.GetSinglePort(tenant, myLastDelivery.FromPortId);
                                    if (myPort != null)
                                    {
                                        myDataProvider.DeliveryFromName = myPort.EnglishName;
                                        myDataProvider.DeliveryFromAddress = myPort.Code + " " + myPort.EnglishName;
                                    }
                                }

                                break;
                            }
                    }
                    #endregion
                }

                if (myDelivery != null)
                {
                    myDataProvider.DeliveryNotes = myDelivery.Notes;
                    myDataProvider.FinalDestinationETA = myDelivery.ETA != null ? String.Format("{0:dd MMM yyyy}", myDelivery.ETA) : "";
                    myDataProvider.DeliveryETA_DateTime = myDelivery.ETA != null ? myDelivery.ETA : null;
                    myDataProvider.FinalDestinationETA_DateTime = myDelivery.ETA != null ? myDelivery.ETA : null;
                    myDataProvider.DeliveryDriverName = myDelivery.Driver;
                    myDataProvider.DeliveryTruckNumber = myDelivery.TruckNumber;
                    myDataProvider.DeliveryTrailerNumber = myDelivery.TrailerNumber;

                    myDataProvider.DeliveryTo = myServicHelper.GetToDeliveryName(shipment, myDelivery);

                    if (!string.IsNullOrEmpty(myDelivery.CarrierId))
                    {
                        Card deliveryTrucker = commonContext.Cards.Where(d => d.Id == myDelivery.CarrierId).FirstOrDefault();
                        if (deliveryTrucker != null)
                        {
                            myDataProvider.DeliveryTruckerName = deliveryTrucker.EnglishName;
                            myDataProvider.DeliveryTruckerInfo = deliveryTrucker.EnglishName;

                            Address deliveryTruckerAddress = addressRepository.GetMainAddressByCardId(deliveryTrucker.Id, tenant);
                            if (deliveryTruckerAddress != null)
                            {
                                myDataProvider.DeliveryTruckerInfo = myDataProvider.DeliveryTruckerInfo + Environment.NewLine + DataProviders.General.GetAddress(deliveryTruckerAddress);
                            }
                        }
                    }

                    switch (myDelivery.PickUpDeliveryToTypeCode)
                    {
                        case "PART":
                            {
                                Card toPartner = CardRepository.GetSingleCard(myDelivery.ToPartnerCardId, tenant, false);
                                if (!string.IsNullOrEmpty(toPartner.PrimaryContactId))
                                {
                                    Contact toPartnerContact = contactRepository.GetSingleContact(toPartner.PrimaryContactId, tenant);
                                    if (toPartnerContact != null)
                                    {
                                        myDataProvider.FirstDeliveryToContactPhone = toPartnerContact.BusinessPhone;
                                    }
                                }

                                if (!string.IsNullOrEmpty(myDelivery.ToAddressId))
                                {
                                    Address myPartnerAddress = addressRepository.GetSingleAddress(myDelivery.ToAddressId, tenant);
                                    if (myPartnerAddress != null)
                                    {
                                        myDataProvider.FinalDestination = myPartnerAddress.City;
                                        myDataProvider.PlaceOfDelivery = myPartnerAddress.City;
                                        myDataProvider.PlaceOfDeliveryCountryCode = myPartnerAddress.Country == null ? "" : myPartnerAddress.Country.Code;
                                        myDataProvider.PlaceOfDeliveryCountryName = myPartnerAddress.Country == null ? "" : myPartnerAddress.Country.EnglishName;
                                        myDataProvider.PlaceOfDeliveryStateCode = myPartnerAddress.State == null ? "" : myPartnerAddress.State.Code;                                        
                                    }

                                    if (!string.IsNullOrEmpty(myDataProvider.DeliveryTo))
                                    {
                                        myDataProvider.DeliveryTo = myDataProvider.DeliveryTo + Environment.NewLine + DataProviders.General.GetAddress(myPartnerAddress);
                                    }
                                }

                                break;
                            }

                        case "PORT":
                            {
                                if (!string.IsNullOrEmpty(myDelivery.ToPortId))
                                {
                                    Port myPort = portRepository.GetSinglePort(tenant, myDelivery.ToPortId);
                                    if (myPort != null)
                                    {
                                        myDataProvider.FinalDestination = myPort.EnglishName;
                                        myDataProvider.PlaceOfDelivery = myPort.EnglishName;
                                        myDataProvider.PlaceOfDeliveryCountryCode = myPort.Country == null ? "" : myPort.Country.Code;
                                        myDataProvider.PlaceOfDeliveryCountryName = myPort.Country == null ? "" : myPort.Country.EnglishName;
                                        myDataProvider.PlaceOfDeliveryStateCode = myPort.State == null ? "" : myPort.State.Code;

                                        if (!string.IsNullOrEmpty(myDataProvider.DeliveryTo))
                                        {
                                            if (myPort.Country != null)
                                            {
                                                myDataProvider.DeliveryTo = myDataProvider.DeliveryTo + Environment.NewLine + myPort.Country.EnglishName;
                                            }
                                        }
                                    }
                                }

                                break;
                            }

                        case "CASL":
                            {
                                string myCity = myDelivery.ToAddressCity;
                                if (!string.IsNullOrEmpty(myCity))
                                {
                                    myDataProvider.FinalDestination = myCity;
                                    myDataProvider.PlaceOfDelivery = myCity;

                                    Country country = countryRepository.GetSingleCountry(myDelivery.ToAddressCountryId, tenant);
                                    if (country != null)
                                    {
                                        myDataProvider.PlaceOfDeliveryCountryCode = country.Code;
                                        myDataProvider.PlaceOfDeliveryCountryName = country.EnglishName;
                                    }
                                }

                                break;
                            }
                    }
                }

                else if (onCarriageToPort != null)
                {
                    myDataProvider.FinalDestinationETA = shipment.OnCarriageETA != null ? String.Format("{0:dd MMM yyyy}", shipment.OnCarriageETA) : "";
                    myDataProvider.FinalDestinationETA_DateTime = shipment.OnCarriageETA != null ? shipment.OnCarriageETA : null;
                    myDataProvider.FinalDestination = onCarriageToPort.EnglishName;
                    myDataProvider.PlaceOfDelivery = onCarriageToPort.EnglishName;

                    PortPM myPort = PortQuery.GetSinglePort(tenant, shipment.OnCarriageToPortId, true);
                    if (myPort != null)
                    {
                        myDataProvider.PlaceOfDeliveryCountryCode = myPort.CountryCode;
                        myDataProvider.PlaceOfDeliveryCountryName = myPort.CountryName;
                        myDataProvider.PlaceOfDeliveryStateCode = myPort.StateCode;
                    }

                    myDataProvider.PlaceOfDeliveryCountryCode = onCarriageToPort.Country == null ? "" : onCarriageToPort.Country.Code;
                    myDataProvider.PlaceOfDeliveryCountryName = onCarriageToPort.Country == null ? "" : onCarriageToPort.Country.EnglishName;
                    myDataProvider.PlaceOfDeliveryStateCode = onCarriageToPort.State == null ? "" : onCarriageToPort.State.Code;
                }

                else
                {
                    myDataProvider.FinalDestinationETA = shipment.MainCarriageETA != null ? String.Format("{0:dd MMM yyyy}", shipment.MainCarriageETA) : "";
                    myDataProvider.FinalDestinationETA_DateTime = shipment.MainCarriageETA != null ? shipment.MainCarriageETA : null;
                    myDataProvider.FinalDestination = myDataProvider.DischargePortName;
                    myDataProvider.PlaceOfDelivery = myDataProvider.DischargePortName;
                    myDataProvider.PlaceOfDeliveryCountryCode = DischargePortCountryCode;
                    myDataProvider.PlaceOfDeliveryCountryName = DischargePortCountryName;
                    myDataProvider.PlaceOfDeliveryStateCode = DischargePortStateCode;
                }

                #endregion

                if (shipment.ShipmentTypeId == "FCL" || shipment.ShipmentTypeId == "FTL" || shipment.ShipmentTypeId == "FCLD")
                {
                    myDataProvider.Containerized = true;
                }

                else
                {
                    myDataProvider.NotContainerized = true;
                }

                myDataProvider.FromPartnerReference = !string.IsNullOrEmpty(shipment.ShipperReference1) ? shipment.ShipperReference1 : "";
                myDataProvider.ShipperRef2 = !string.IsNullOrEmpty(shipment.ShipperReference2) ? shipment.ShipperReference2 : "";
                myDataProvider.ToPartnerReference = !string.IsNullOrEmpty(shipment.ConsigneeReference1) ? shipment.ConsigneeReference1 : "";
                myDataProvider.ConsigneeRef2 = !string.IsNullOrEmpty(shipment.ConsigneeReference2) ? shipment.ConsigneeReference2 : "";

                // Inland + Domestic
                if (shipment.DirectionId == "D" && shipment.TransportModeId == "I")
                {
                    #region
                    Address fromAddress = addressRepository.GetSingleAddress(shipment.MainCarriageFromAddressId, shipment.Tenant);
                    Address toAddress = addressRepository.GetSingleAddress(shipment.MainCarriageToAddressId, shipment.Tenant);

                    if (fromAddress != null)
                    {
                        myDataProvider.FromLocation = fromAddress.City + " " + (fromAddress.Country != null ? fromAddress.Country.Code : "");
                        if (!string.IsNullOrEmpty(myDataProvider.FromLocation))
                        {
                            myDataProvider.FromLocation_Label = "Place of Loading";
                        }
                        else
                        {
                            myDataProvider.FromLocation_Label = "";
                        }
                    }

                    if (toAddress != null)
                    {
                        myDataProvider.ToLocation = toAddress.City + " " + (toAddress.Country != null ? toAddress.Country.Code : "");
                        if (!string.IsNullOrEmpty(myDataProvider.ToLocation))
                        {
                            myDataProvider.ToLocation_Label = "Place of Discharge";
                        }
                        else
                        {
                            myDataProvider.ToLocation_Label = "";
                        }

                        myDataProvider.FinalLocation = toAddress.City + " " + (toAddress.Country != null ? toAddress.Country.Code : "");
                    }

                    Country fromCountry = commonContext.Countries.Where(a => a.Id == fromAddress.CountryId).FirstOrDefault();
                    Country toCountry = commonContext.Countries.Where(a => a.Id == toAddress.CountryId).FirstOrDefault();

                    myDataProvider.FromLocationCountryCode = fromCountry != null ? fromCountry.Code : "";
                    myDataProvider.ToLocationCountryCode = toCountry != null ? toCountry.Code : "";

                    Card fromPartner = commonContext.Cards.Where(d => d.Id == shipment.MainCarriageFromPartnerId && d.Tenant == shipment.Tenant).FirstOrDefault();
                    Card toPartner = commonContext.Cards.Where(d => d.Id == shipment.MainCarriageToPartnerId && d.Tenant == shipment.Tenant).FirstOrDefault();

                    myDataProvider.FromPartnerName = fromPartner.EnglishName;
                    myDataProvider.FromPartnerFullAddress = DataProviders.General.GetAddress(fromAddress);

                    if (fromAddress.IsLocalLanguage && !string.IsNullOrEmpty(fromPartner.LocalName))
                    {
                        myDataProvider.FromPartnerName = fromPartner.LocalName;
                    }

                    myDataProvider.ToPartnerName = toPartner.EnglishName;
                    myDataProvider.ToPartnerFullAddress = DataProviders.General.GetAddress(toAddress);

                    if (toAddress.IsLocalLanguage && !string.IsNullOrEmpty(toPartner.LocalName))
                    {
                        myDataProvider.ToPartnerName = toPartner.LocalName;
                    }

                    myDataProvider.DeliveryTruckNumber = shipment.TruckNumber;
                    myDataProvider.DeliveryTrailerNumber = shipment.TrailerNumber;
                    myDataProvider.InlandDriver = shipment.Driver;
                    #endregion
                }

                else if (shipment.TransportModeId == "A")
                {
                    #region
                    myDataProvider.FromLocation = mainCarriageFromPort.Code + " " + mainCarriageFromPort.EnglishName;
                    myDataProvider.ToLocation = mainCarriageToPort.Code + " " + mainCarriageToPort.EnglishName;
                    myDataProvider.FinalLocation = finalDestination != null ? (finalDestination.Code + " " + finalDestination.EnglishName) : "";
                    myDataProvider.FromLocation_Label = "Airport Of Departure";
                    myDataProvider.ToLocation_Label = "Airport Of Arrival";

                    Country fromCountry = commonContext.Countries.Where(a => a.Id == mainCarriageFromPort.CountryId).FirstOrDefault();
                    myDataProvider.FromLocationCountryCode = fromCountry != null ? fromCountry.Code : "";

                    if (finalDestination != null)
                    {
                        Country toCountry = commonContext.Countries.Where(a => a.Id == finalDestination.CountryId).FirstOrDefault();
                        myDataProvider.ToLocationCountryCode = toCountry != null ? toCountry.Code : "";
                    }

                    else
                    {
                        Country toCountry = commonContext.Countries.Where(a => a.Id == mainCarriageToPort.CountryId).FirstOrDefault();
                        myDataProvider.ToLocationCountryCode = toCountry != null ? toCountry.Code : "";
                    }
                    #endregion
                }

                else
                {
                    #region
                    myDataProvider.FromLocation = mainCarriageFromPort.Code + " " + mainCarriageFromPort.EnglishName;
                    myDataProvider.FromLocation_Label = "Port of Loading";
                    myDataProvider.ToLocation = mainCarriageToPort.Code + " " + mainCarriageToPort.EnglishName;
                    myDataProvider.ToLocation_Label = "Port of Discharge";
                    myDataProvider.FinalLocation = finalDestination != null ? (finalDestination.Code + " " + finalDestination.EnglishName) : "";

                    Country fromCountry = commonContext.Countries.Where(a => a.Id == mainCarriageFromPort.CountryId).FirstOrDefault();
                    myDataProvider.FromLocationCountryCode = fromCountry != null ? fromCountry.Code : "";

                    if (finalDestination != null)
                    {
                        Country toCountry = commonContext.Countries.Where(a => a.Id == finalDestination.CountryId).FirstOrDefault();
                        myDataProvider.ToLocationCountryCode = toCountry != null ? toCountry.Code : "";
                    }

                    else
                    {
                        Country toCountry = commonContext.Countries.Where(a => a.Id == mainCarriageToPort.CountryId).FirstOrDefault();
                        myDataProvider.ToLocationCountryCode = toCountry != null ? toCountry.Code : "";
                    }
                    #endregion
                }

                #region ETA
                myDataProvider.ETA =
                    String.Format("{0:dd MMM yyyy}", shipment.Transshipment3ETA != null ? shipment.Transshipment3ETA :
                    (shipment.Transshipment2ETA != null ? shipment.Transshipment2ETA :
                    (shipment.Transshipment1ETA != null ? shipment.Transshipment1ETA : shipment.MainCarriageETA)
                    ));

                myDataProvider.ETA_DateTime =
                    shipment.Transshipment3ETA != null ? shipment.Transshipment3ETA :
                    (shipment.Transshipment2ETA != null ? shipment.Transshipment2ETA :
                    (shipment.Transshipment1ETA != null ? shipment.Transshipment1ETA : shipment.MainCarriageETA)
                    );

                #endregion

                #region ETD
                DateTime? myDateField = null;

                if (shipment.Transshipment1ATD != null)
                {
                    myDateField = shipment.Transshipment1ATD;
                }

                else if (shipment.Transshipment1ETD != null)
                {
                    myDateField = shipment.Transshipment1ETD;
                }

                else if (shipment.Transshipment2ATD != null)
                {
                    myDateField = shipment.Transshipment2ATD;
                }

                else if (shipment.Transshipment2ETD != null)
                {
                    myDateField = shipment.Transshipment2ETD;
                }

                else if (shipment.Transshipment3ATD != null)
                {
                    myDateField = shipment.Transshipment3ATD;
                }

                else if (shipment.Transshipment3ETD != null)
                {
                    myDateField = shipment.Transshipment3ETD;
                }

                else if (shipment.MainCarriageATD != null)
                {
                    myDateField = shipment.MainCarriageATD;
                }

                else if (shipment.MainCarriageETD != null)
                {
                    myDateField = shipment.MainCarriageETD;
                }

                if (myDateField != null)
                {
                    myDataProvider.ETD = String.Format("{0:dd MMM yyyy}", myDateField);
                }

                myDataProvider.ETD_DateTime =
                    shipment.Transshipment3ETD != null ? shipment.Transshipment3ETD :
                    (shipment.Transshipment2ETD != null ? shipment.Transshipment2ETD :
                    (shipment.Transshipment1ETD != null ? shipment.Transshipment1ETD : shipment.MainCarriageETD)
                    );
                #endregion

                //Ports
                if (mainCarriageFromPort != null)
                {
                    myDataProvider.LoadingPortName = mainCarriageFromPort.EnglishName;
                    myDataProvider.LoadingPortCode = mainCarriageFromPort.Code;
                }

                myDataProvider.PlaceAndDateOfIssue = myDataProvider.LoadingPortName;

                if (preCarriageFromPort != null)
                {
                    myDataProvider.PreCarriageFromPort = preCarriageFromPort.EnglishName + " " + preCarriageFromPort.Code;
                }

                if (preCarriageToPort != null)
                {
                    myDataProvider.PreCarriageToPort = preCarriageToPort.EnglishName;
                }

                if (onCarriageToPort != null)
                {
                    myDataProvider.OnCarriageToPort = onCarriageToPort.EnglishName;
                }

                if (finalDestination != null)
                {
                    myDataProvider.ForeignPortOfUnloading = finalDestination.EnglishName + " " + finalDestination.Code;
                }

                #region Vessel
                if (shipment.MainCarriageVesselId != null)
                {
                    Vessel maincarriagevessel = (from a in commonContext.Vessels
                                                 where a.Id == shipment.MainCarriageVesselId
                                                 select a).FirstOrDefault();
                    if (maincarriagevessel != null)
                    {
                        myDataProvider.MainCarriageVesselNameAndNumber = maincarriagevessel.EnglishName + " \\ " + shipment.MainCarriageCarrierNumber;
                        myDataProvider.MainCarriageVesselName = maincarriagevessel.EnglishName;
                        myDataProvider.MainCarriageVesselCode = maincarriagevessel.Code;
                    }
                }

                //Last Vessel
                string vesselName = "";
                string vesselCode = "";
                string vesselNameAndNumber = "";

                if (!string.IsNullOrEmpty(shipment.Transshipment3FromPortId))
                {
                    Vessel vessel = (from a in commonContext.Vessels
                                     where a.Id == shipment.Transshipment3VesselId
                                     select a).FirstOrDefault();
                    if (vessel != null)
                    {
                        vesselNameAndNumber = vessel.EnglishName + " \\ " + shipment.MainCarriageCarrierNumber;
                        vesselName = vessel.EnglishName;
                        vesselCode = vessel.Code;
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(shipment.Transshipment2FromPortId))
                    {
                        Vessel vessel = (from a in commonContext.Vessels
                                         where a.Id == shipment.Transshipment2VesselId
                                         select a).FirstOrDefault();
                        if (vessel != null)
                        {
                            vesselNameAndNumber = vessel.EnglishName + " \\ " + shipment.MainCarriageCarrierNumber;
                            vesselName = vessel.EnglishName;
                            vesselCode = vessel.Code;
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(shipment.Transshipment1FromPortId))
                        {
                            Vessel vessel = (from a in commonContext.Vessels
                                             where a.Id == shipment.Transshipment1VesselId
                                             select a).FirstOrDefault();
                            if (vessel != null)
                            {
                                vesselNameAndNumber = vessel.EnglishName + " \\ " + shipment.MainCarriageCarrierNumber;
                                vesselName = vessel.EnglishName;
                                vesselCode = vessel.Code;
                            }
                        }

                        else
                        {
                            Vessel vessel = (from a in commonContext.Vessels
                                             where a.Id == shipment.MainCarriageVesselId
                                             select a).FirstOrDefault();
                            if (vessel != null)
                            {
                                vesselNameAndNumber = vessel.EnglishName + " \\ " + shipment.MainCarriageCarrierNumber;
                                vesselName = vessel.EnglishName;
                                vesselCode = vessel.Code;
                            }
                        }
                    }
                }

                myDataProvider.LastMainCarriageVesselCode = vesselCode;
                myDataProvider.LastMainCarriageVesselName = vesselName;
                myDataProvider.LastMainCarriageVesselNameAndNumber = vesselNameAndNumber;
                #endregion

                #region Pickup Details                
                ShipmentPickUpPM myPickup = shipmentPickUpQuery.GetShipmentPickUpPMsByTenantAndShipment(shipmentId, tenant).Where(a => a.PickUpDeliveryNumber == shipment.ShipmentNumber + "/" + shipment.ShipmentPickUpIndex).FirstOrDefault();
                myDataProvider.Instructions = this.GetInstructionsField(shipment, myPickup, cardQuery);

                if (myPickup == null)
                {
                    if (!string.IsNullOrEmpty(shipment.ShipperNotExporterAddressId))
                    {
                        Address myAddress = addressRepository.GetSingleAddress(shipment.ShipperNotExporterAddressId, tenant);
                        if (myAddress != null)
                        {
                            myDataProvider.PickUpAddress = myAddress.City != null ? myAddress.City : "";
                        }
                    }

                    else if (preCarriageFromPort != null)
                    {
                        myDataProvider.PickUpAddress = preCarriageFromPort.EnglishName;
                    }
                }

                else
                {
                    myDataProvider.PickUpAddress = myServicHelper.GetPickUpDeliveryFromCityOrPortName(myPickup);

                    if (myPickup.ToAddressId != null)
                    {
                        Address toAddress = addressRepository.GetSingleAddress(myPickup.ToAddressId, tenant);

                        if (toAddress != null)
                        {
                            myDataProvider.DeliveryAddress = DataProviders.General.GetAddress(toAddress);
                        }
                    }

                    else
                    {
                        myDataProvider.DeliveryAddress = myPickup.ToAddress != null ? myPickup.ToAddress : "";
                    }
                }

                #endregion

                #region EmptyContainer
                if (myFirstPickup != null)
                {
                    if (!string.IsNullOrEmpty(myFirstPickup.EmptyPickupContainerPartnerId))
                    {
                        CardPM cardPM = cardQuery.GetSinglePM(myFirstPickup.EmptyPickupContainerPartnerId, tenant);

                        if (cardPM != null)
                        {
                            string myEmptyContainer = null;
                            string myEmptyContainerName = null;
                            string myEmptyContainerAddress = null;

                            myEmptyContainer = cardPM.EnglishName != null ? cardPM.EnglishName : "";
                            myEmptyContainerName = cardPM.EnglishName != null ? cardPM.EnglishName : "";

                            if (cardPM.MainAddressId != null)
                            {
                                Address theAddress = addressRepository.GetSingleAddress(cardPM.MainAddressId, tenant);

                                if (theAddress != null)
                                {
                                    if (theAddress.IsLocalLanguage && !string.IsNullOrEmpty(cardPM.LocalName))
                                    {
                                        myEmptyContainer = cardPM.LocalName;
                                        myEmptyContainerName = cardPM.LocalName;
                                    }

                                    myEmptyContainer = myEmptyContainer + Environment.NewLine + DataProviders.General.GetAddress(theAddress);
                                    myEmptyContainerAddress = DataProviders.General.GetAddress(theAddress);

                                    if (theAddress.PhoneNumber != null)
                                    {
                                        myEmptyContainer = myEmptyContainer + Environment.NewLine + "Phone No. : " + theAddress.PhoneNumber;
                                        myEmptyContainerAddress = myEmptyContainerAddress + Environment.NewLine + "Phone No. : " + theAddress.PhoneNumber;
                                    }

                                    if (theAddress.FaxNumber != null)
                                    {
                                        myEmptyContainer = myEmptyContainer + "   Fax No. : " + theAddress.FaxNumber;
                                        myEmptyContainerAddress = myEmptyContainerAddress + "   Fax No. : " + theAddress.FaxNumber;
                                    }
                                }
                            }

                            myDataProvider.EmptyContainer = myEmptyContainer;
                            myDataProvider.EmptyContainerName = myEmptyContainerName;
                            myDataProvider.EmptyContainerAddress = myEmptyContainerAddress;
                        }
                    }
                }
                #endregion

                #region CuttOff
                if (shipment.CutoffDate != null)
                {
                    myDataProvider.CuttOffDateTime_Date = shipment.CutoffDate;
                    myDataProvider.CuttOffDateTime = String.Format("{0:dd MMM yyyy}", shipment.CutoffDate);
                    myDataProvider.CuttOffTime = String.Format("{0:t}", shipment.CutoffDate);
                }
                #endregion

                #region Customer

                if (!string.IsNullOrEmpty(shipment.CustomerId))
                {
                    if (shipment.CustomerId == shipment.ShipperId)
                    {
                        myDataProvider.CustomerReferenceNumber = shipment.ShipperReference1 != null ? shipment.ShipperReference1 : "";
                    }

                    else if (shipment.CustomerId == shipment.ConsigneeId)
                    {
                        myDataProvider.CustomerReferenceNumber = shipment.ConsigneeReference1 != null ? shipment.ConsigneeReference1 : "";
                    }

                    CustomerRepository customerRepository = new CustomerRepository(tenant);
                    Customer customer = customerRepository.GetSingleCustomer(shipment.CustomerId, tenant, false);
                    if (customer != null)
                    {
                        myDataProvider.CustomerVat = customer.Card.VatNumber;
                        myDataProvider.IRSPlace = customer.Card.IRSPlace;
                        myDataProvider.IRSNumber = customer.Card.IRSNumber;
                        myDataProvider.CustomerName = customer.Card.EnglishName;
                    }

                    Address customerAddress = addressRepository.GetSingleAddress(shipment.CustomerAddressId, tenant);
                    if (customerAddress != null)
                    {
                        myDataProvider.CustomerAddress = DataProviders.General.GetAddress(customerAddress);

                        if (customerAddress.PhoneNumber != null || customerAddress.FaxNumber != null)
                        {
                            myDataProvider.CustomerAddress = myDataProvider.CustomerAddress + Environment.NewLine + (customerAddress.PhoneNumber != null ? "Tel: " + customerAddress.PhoneNumber + " " : "") + (customerAddress.FaxNumber != null ? "Fax: " + customerAddress.FaxNumber + " " : "");
                        }
                    }

                    Contact customerContact = contactRepository.GetSingleContact(shipment.CustomerContactId, tenant);
                    if (customerContact != null)
                    {
                        myDataProvider.ContactDetails = customerContact.EnglishName;

                        if (!string.IsNullOrEmpty(customerContact.Email))
                        {
                            myDataProvider.ContactDetails = myDataProvider.ContactDetails + ", " + customerContact.Email;
                        }

                        if (!string.IsNullOrEmpty(customerContact.Mobile))
                        {
                            myDataProvider.ContactDetails = myDataProvider.ContactDetails + ", " + customerContact.Mobile;
                        }

                        if (!string.IsNullOrEmpty(customerContact.Fax))
                        {
                            myDataProvider.ContactDetails = myDataProvider.ContactDetails + ", " + customerContact.Fax;
                        }
                    }
                }

                #endregion

                #region Document Type
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
                DocumentTypePM documentTypePM = documentTypeQuery.GetSinglePMByCodeAndTenant(documentTypeCode, tenant);

                if (documentTypePM != null)
                {
                    List<FormCustomField> customfieldsList = commonContext.FormCustomFields.Where(fc => fc.DocumentTypeId == documentTypePM.Id).ToList();

                    List<DocumentTypeCustomField> documentCustomfieldsList = commonContext.DocumentTypeCustomFields.Where(fc => fc.DocumentTypeId == documentTypePM.Id).ToList();

                    FormCustomField copyOrOriginalCustomField = (from a in customfieldsList
                                                                 where a.FieldCode == "OriginalsOrCopiesNo" && a.EntityId == shipment.Id
                                                                 select a).FirstOrDefault();

                    DocumentTypeCustomField copyOrOriginalDocumentCustom = (from a in documentCustomfieldsList
                                                                            where a.FieldCode == "OriginalsOrCopiesNo"
                                                                            select a).FirstOrDefault();

                    FormCustomField hasAttachmentListCustomField = (from a in customfieldsList
                                                                    where a.FieldCode == "HasAttachmentList" && a.EntityId == shipment.Id
                                                                    select a).FirstOrDefault();

                    DocumentTypeCustomField hasAttachmentListDocumentCustom = (from a in documentCustomfieldsList
                                                                               where a.FieldCode == "HasAttachmentList"
                                                                               select a).FirstOrDefault();

                    FormCustomField InstructionsCustomField = (from a in customfieldsList
                                                               where a.FieldCode == "Instructions" && a.EntityId == shipment.Id
                                                               select a).FirstOrDefault();

                    DocumentTypeCustomField InstructionsDocumentCustom = (from a in documentCustomfieldsList
                                                                          where a.FieldCode == "Instructions"
                                                                          select a).FirstOrDefault();

                    FormCustomField remarkCustomField = (from a in customfieldsList
                                                         where a.FieldCode == "Remark" && a.EntityId == shipment.Id
                                                         select a).FirstOrDefault();

                    DocumentTypeCustomField remarkDocumentCustom = (from a in documentCustomfieldsList
                                                                    where a.FieldCode == "Remark"
                                                                    select a).FirstOrDefault();

                    FormCustomField sDdataproviderCustomField = (from a in customfieldsList
                                                                 where a.FieldCode == "LastFreeDate" && a.EntityId == shipment.Id
                                                                 select a).FirstOrDefault();

                    DocumentTypeCustomField sDdataproviderDocumentCustom = (from a in documentCustomfieldsList
                                                                            where a.FieldCode == "LastFreeDate"
                                                                            select a).FirstOrDefault();

                    FormCustomField valueCustomField = (from a in customfieldsList
                                                        where a.FieldCode == "Value" && a.EntityId == shipment.Id
                                                        select a).FirstOrDefault();

                    DocumentTypeCustomField valueDocumentCustom = (from a in documentCustomfieldsList
                                                                   where a.FieldCode == "Value"
                                                                   select a).FirstOrDefault();

                    FormCustomField shipper2CustomField = (from a in customfieldsList
                                                           where a.FieldCode == "Shipper2" && a.EntityId == shipment.Id
                                                           select a).FirstOrDefault();

                    DocumentTypeCustomField shipper2DocumentCustom = (from a in documentCustomfieldsList
                                                                      where a.FieldCode == "Shipper2"
                                                                      select a).FirstOrDefault();

                    FormCustomField shipper3CustomField = (from a in customfieldsList
                                                           where a.FieldCode == "Shipper3" && a.EntityId == shipment.Id
                                                           select a).FirstOrDefault();

                    DocumentTypeCustomField shipper3DocumentCustom = (from a in documentCustomfieldsList
                                                                      where a.FieldCode == "Shipper3"
                                                                      select a).FirstOrDefault();

                    FormCustomField shipper4CustomField = (from a in customfieldsList
                                                           where a.FieldCode == "Shipper4" && a.EntityId == shipment.Id
                                                           select a).FirstOrDefault();

                    DocumentTypeCustomField shipper4DocumentCustom = (from a in documentCustomfieldsList
                                                                      where a.FieldCode == "Shipper4"
                                                                      select a).FirstOrDefault();

                    FormCustomField shipper5CustomField = (from a in customfieldsList
                                                           where a.FieldCode == "Shipper5" && a.EntityId == shipment.Id
                                                           select a).FirstOrDefault();

                    DocumentTypeCustomField shipper5DocumentCustom = (from a in documentCustomfieldsList
                                                                      where a.FieldCode == "Shipper5"
                                                                      select a).FirstOrDefault();

                    FormCustomField HAWB2CustomField = (from a in customfieldsList
                                                        where a.FieldCode == "HAWB2" && a.EntityId == shipment.Id
                                                        select a).FirstOrDefault();

                    DocumentTypeCustomField HAWB2DocumentCustom = (from a in documentCustomfieldsList
                                                                   where a.FieldCode == "HAWB2"
                                                                   select a).FirstOrDefault();

                    FormCustomField HAWB3CustomField = (from a in customfieldsList
                                                        where a.FieldCode == "HAWB3" && a.EntityId == shipment.Id
                                                        select a).FirstOrDefault();

                    DocumentTypeCustomField HAWB3DocumentCustom = (from a in documentCustomfieldsList
                                                                   where a.FieldCode == "HAWB3"
                                                                   select a).FirstOrDefault();

                    FormCustomField HAWB4CustomField = (from a in customfieldsList
                                                        where a.FieldCode == "HAWB4" && a.EntityId == shipment.Id
                                                        select a).FirstOrDefault();

                    DocumentTypeCustomField HAWB4DocumentCustom = (from a in documentCustomfieldsList
                                                                   where a.FieldCode == "HAWB4"
                                                                   select a).FirstOrDefault();

                    FormCustomField HAWB5CustomField = (from a in customfieldsList
                                                        where a.FieldCode == "HAWB5" && a.EntityId == shipment.Id
                                                        select a).FirstOrDefault();

                    DocumentTypeCustomField HAWB5DocumentCustom = (from a in documentCustomfieldsList
                                                                   where a.FieldCode == "HAWB5"
                                                                   select a).FirstOrDefault();

                    myDataProvider.HasAttachmentList = this.GetHasAttachmentListField(shipment, hasAttachmentListCustomField, hasAttachmentListDocumentCustom);
                    myDataProvider.OriginalsOrCopiesNo = this.GetOriginalsOrCopiesNoField(shipment, copyOrOriginalCustomField, copyOrOriginalDocumentCustom, shipmentsContext);

                    myDataProvider.SendersInstructions = InstructionsCustomField != null ? InstructionsCustomField.Value : (InstructionsDocumentCustom != null ? InstructionsDocumentCustom.DefaultValue : "");
                    myDataProvider.MoveType = myDataProvider.MoveTypeName;
                    myDataProvider.Remark = remarkCustomField != null ? remarkCustomField.Value : (remarkDocumentCustom != null ? remarkDocumentCustom.DefaultValue : "");
                    myDataProvider.LastFreeDate = sDdataproviderCustomField != null ? sDdataproviderCustomField.Value : (sDdataproviderDocumentCustom != null ? sDdataproviderDocumentCustom.DefaultValue : "");
                    myDataProvider.Shipper2 = shipper2CustomField != null ? shipper2CustomField.Value : (shipper2DocumentCustom != null ? shipper2DocumentCustom.DefaultValue : "");
                    myDataProvider.Shipper3 = shipper3CustomField != null ? shipper3CustomField.Value : (shipper3DocumentCustom != null ? shipper3DocumentCustom.DefaultValue : "");
                    myDataProvider.Shipper4 = shipper4CustomField != null ? shipper4CustomField.Value : (shipper4DocumentCustom != null ? shipper4DocumentCustom.DefaultValue : "");
                    myDataProvider.Shipper5 = shipper5CustomField != null ? shipper5CustomField.Value : (shipper5DocumentCustom != null ? shipper5DocumentCustom.DefaultValue : "");
                    myDataProvider.HAWB2 = HAWB2CustomField != null ? HAWB2CustomField.Value : (HAWB2DocumentCustom != null ? HAWB2DocumentCustom.DefaultValue : "");
                    myDataProvider.HAWB3 = HAWB3CustomField != null ? HAWB3CustomField.Value : (HAWB3DocumentCustom != null ? HAWB3DocumentCustom.DefaultValue : "");
                    myDataProvider.HAWB4 = HAWB4CustomField != null ? HAWB4CustomField.Value : (HAWB4DocumentCustom != null ? HAWB4DocumentCustom.DefaultValue : "");
                    myDataProvider.HAWB5 = HAWB5CustomField != null ? HAWB5CustomField.Value : (HAWB5DocumentCustom != null ? HAWB5DocumentCustom.DefaultValue : "");
                }
                #endregion

                #region Receivables Lines

                ShipmentReceivableRepository receivableRepository = new ShipmentReceivableRepository(tenant);
                List<ShipmentReceivable> receivables = receivableRepository.GetShipmentReceivablesByShipmentId(shipment.Id, tenant);

                myDataProvider.ReceivablesLines = new List<ReceivableLine>();

                CurrencyRepository currencyRepository = new CurrencyRepository(tenant);
                Currency profitCurrency = currencyRepository.GetSingleCurrency(shipment.ProfitCurrencyId, tenant);
                string profitcurrencyCode = profitCurrency == null ? "" : profitCurrency.Code;

                double? totalInLocal = 0;
                double? totalInProfit = 0;
                if (shipment.AccountedReceivablesInProfitCurrency != null)
                {
                    totalInProfit = totalInProfit + shipment.AccountedReceivablesInProfitCurrency;
                }

                if (shipment.OpenReceivablesInProfitCurrency != null)
                {
                    totalInProfit = totalInProfit + shipment.OpenReceivablesInProfitCurrency;
                }

                if (shipment.AccountedReceivablesInLocalCurrency != null)
                {
                    totalInLocal = totalInLocal + shipment.AccountedReceivablesInLocalCurrency;
                }

                if (shipment.OpenReceivablesInLocalCurrency != null)
                {
                    totalInLocal = totalInLocal + shipment.OpenReceivablesInLocalCurrency;
                }

                myDataProvider.LocalCurrencyCode = localCurrencyCode;
                myDataProvider.ProfitCurrencyCode = profitcurrencyCode;
                myDataProvider.TotalReceivablesAmountInLocal = String.Format("{0:0,0.00}", totalInLocal);
                myDataProvider.TotalReceivablesAmountInProfit = String.Format("{0:0,0.00}", totalInProfit);

                foreach (ShipmentReceivable receivableItem in receivables)
                {
                    string amount = "0";
                    string amountLocal = "0";
                    string amountProfit = "0";

                    if (receivableItem.TotalAmount != null)
                    {
                        amount = String.Format("{0:0,0.00}", receivableItem.TotalAmount);
                    }

                    if (receivableItem.TotalAmountLocal != null)
                    {
                        amountLocal = String.Format("{0:0,0.00}", receivableItem.TotalAmountLocal);
                    }

                    if (receivableItem.AmountInProfitCurrency != null)
                    {
                        amountProfit = String.Format("{0:0,0.00}", receivableItem.AmountInProfitCurrency);
                    }

                    ReceivableLine newLine = new ReceivableLine()
                    {
                        Id = receivableItem.Id,
                        Name = receivableItem.ChargesType.EnglishName,
                        LocalName = receivableItem.ChargesType.LocalName,
                        CurrencyCode = receivableItem.Currency.Code,
                        LocalCurrencyCode = localCurrencyCode,
                        ProfitCurrencyCode = profitcurrencyCode,
                        Amount = amount,
                        AmountInLocal = amountLocal,
                        AmountInProfit = amountProfit,
                        UnitPrice = receivableItem.UnitPrice,
                        UOM = receivableItem.Measurement == null ? null : receivableItem.Measurement.Name,
                        Quantity = receivableItem.Quantity,
                        PrepaidCollect = receivableItem.PrepaidCollectId == "P" ? "Prepaid" : "Collect",
                        AmountInProfit_Double = receivableItem.AmountInProfitCurrency,
                    };

                    if (receivableItem.Measurement != null)
                    {
                        if (receivableItem.Measurement.Code == "" || receivableItem.Measurement.Code == "")
                        {
                            newLine.UOMPercentage = "%";
                        }
                    }

                    myDataProvider.ReceivablesLines.Add(newLine);
                }
                #endregion

                #region Payables Lines
                ShipmentPayableRepository payableRepository = new ShipmentPayableRepository(tenant);
                List<ShipmentPayable> payables = payableRepository.GetShipemntPayablesByShipmentId(shipment.Id, tenant);

                myDataProvider.PayablesLines = new List<PayableLine>();

                foreach (ShipmentPayable payableItem in payables)
                {
                    PayableLine payableLine = new PayableLine()
                    {
                        Id = payableItem.Id,
                        Name = payableItem.ChargesType == null ? null : payableItem.ChargesType.EnglishName,
                        LocalName = payableItem.ChargesType == null ? null : payableItem.ChargesType.LocalName,
                        CurrencyCode = payableItem.Currency == null ? null : payableItem.Currency.Code,
                        LocalCurrencyCode = localCurrencyCode,
                        ProfitCurrencyCode = profitcurrencyCode,
                        OpenAmount = payableItem.OpenAmount,
                        OpenAmountInLocal = payableItem.OpenAmountInLocalCurrency,
                        OpenAmountInProfit = payableItem.OpenAmountInProfitCurrency,
                        ExpectedAmount = payableItem.ExpectedAmount,
                        ExpectedAmountInLocal = payableItem.ExpectedAmountLocal,
                        ExpectedAmountInProfit = payableItem.ExpectedAmountInProfitCurrency,
                        AccountedAmount = payableItem.AccountedAmount,
                        AccountedAmountInLocal = payableItem.AccountedAmountInLocalCurrency,
                        AccountedAmountInProfit = payableItem.AccountedAmountInProfitCurrency,
                        UnitPrice = payableItem.UnitPrice,
                        UOM = payableItem.Measurement == null ? null : payableItem.Measurement.Name,
                        Quantity = payableItem.Quantity,
                    };

                    if (payableItem.Measurement != null)
                    {
                        if (payableItem.Measurement.Code == "")
                        {
                            payableLine.UOMPercentage = "%";
                        }
                    }

                    if (!string.IsNullOrEmpty(payableItem.VendorId))
                    {
                        Card vendor = (from a in commonContext.Cards
                                       where a.Id == payableItem.VendorId
                                       select a).FirstOrDefault();

                        if (vendor != null)
                        {
                            payableLine.VendorName = vendor.EnglishName;
                        }
                    }

                    myDataProvider.PayablesLines.Add(payableLine);
                }
                #endregion

                #region Packages Lines
                List<ShipmentPackage> packages = shipmentsContext.ShipmentPackages.Where(d => d.ShipmentId == shipment.Id && d.Tenant == tenant).ToList();
                myDataProvider.PackagesLines = new List<PackageLine>();
                myDataProvider.AttachmentList = new List<PackageLine>();
                myDataProvider.DangerousPackages = new List<PackageLine>();

                int counter = 1;

                myDataProvider.GeneralPackageslinesDescriptionOfGoods = "";
                myDataProvider.Dimensions = "";

                StringBuilder str = new StringBuilder();
                StringBuilder str2 = new StringBuilder();

                foreach (ShipmentPackage package in packages)
                {
                    counter++;

                    PackageLine packageline = new PackageLine();
                    packageline.InsidePackagesLines = new List<InsidePackageLine>();

                    List<InsideShipmentPackage> insidePackages = shipmentsContext.InsideShipmentPackages.Where(d => d.ShipmentPackageId == package.Id && d.Tenant == package.Tenant).ToList();

                    PackageType packagetype = (from pa in commonContext.PackageTypes
                                               where pa.Id == package.PackageTypeId
                                               select pa).FirstOrDefault();

                    packageline.InsidePackagesDetails = this.ComputeInsidePackagesDetailsPerPackage(package);
                    packageline.PackageDescriptionOfGoods = package.Description != null ? package.Description : "";
                    packageline.VGM = package.VGM;
                    packageline.MethodUsed = package.MethodUsed;
                    packageline.ContainerNumber = package.ContainerNumber;
                    packageline.SealNumber = package.ShipperSeal;
                    packageline.CeficClass = package.CeficClass;
                    packageline.IMDGCode = package.IMDGCode;
                    packageline.KemlerCode = package.KelmerCode;
                    packageline.UNCode = package.UnNumber;
                    packageline.MarinePollutant = package.MarinePollutant ? "Y" : "N";
                    packageline.PackingGroup = package.PackagingGroup;
                    packageline.EMS = package.EMS;
                    packageline.ProperShippingName = package.ProperShippingName;
                    packageline.PackingCode = "";
                    packageline.FlashPoint = package.FlashPoint;
                    packageline.NetWeight = package.Weight - package.Tare;
                    packageline.Description = package.Description;
                    packageline.Notes = package.Notes;
                    packageline.PackageTare = package.Tare != null ? String.Format("{0:0,0.00}", package.Tare.Value) : null;
                    packageline.MarksAndNumbersOnly = package.MarksAndNumbers;

                    #region Car Details
                    packageline.Make = package.Make;
                    packageline.Model = package.Model;
                    packageline.Year = package.Year;
                    packageline.Color = package.Color;
                    packageline.ChassisNumber = package.ChassisNumber;
                    packageline.RegistrationNumber = package.RegistrationNumber;

                    if (!string.IsNullOrEmpty(package.CountryId))
                    {
                        Country country = countryRepository.GetSingleCountry(package.CountryId, tenant);
                        if (country != null)
                        {
                            packageline.CountryName = country.EnglishName;
                        }
                    }
                    #endregion

                    #region Harmonize
                    if (package.IsMultiHarmonize)
                    {
                        List<ShipmentPackageHarmonize> allHarmonizes = shipmentsContext.ShipmentPackageHarmonizes.Where(d => d.PackageId == package.Id && d.Tenant == package.Tenant).ToList();
                        foreach (ShipmentPackageHarmonize itemHarmonize in allHarmonizes)
                        {
                            if (string.IsNullOrEmpty(packageline.HSCode))
                            {
                                packageline.HSCode = itemHarmonize.Harmonize;
                            }

                            else
                            {
                                packageline.HSCode += "," + itemHarmonize.Harmonize;
                            }
                        }
                    }

                    else
                    {
                        packageline.HSCode = package.Harmonize;
                    }
                    #endregion

                    if (package.Length != null && package.Width != null && package.Height != null)
                    {
                        packageline.Dimensions = package.Length + "x" + package.Width + "x" + package.Height;
                    }

                    if (package.IsDangerous)
                    {
                        packageline.PackageDescriptionOfGoods +=
                            "CONTAINS DANGEROUS GOODS: " + Environment.NewLine +
                            (package.MaterialDescription != null ? package.MaterialDescription : "") +
                            " - Class: " + (package.ClassNumber != null ? package.ClassNumber : "") +
                            ", UN-N: " + (package.UnNumber != null ? package.ClassNumber : "") +
                            ", PACKING GROUP " + (package.PackagingGroup != null ? package.PackagingGroup : "");
                        packageline.IsDangerous = "Yes";
                    }
                    else
                    {
                        packageline.IsDangerous = "No";
                    }

                    if (!string.IsNullOrEmpty(packageline.HSCode))
                    {
                        if (!string.IsNullOrEmpty(packageline.PackageDescriptionOfGoods))
                        {
                            packageline.PackageDescriptionOfGoods += Environment.NewLine;
                        }

                        packageline.PackageDescriptionOfGoods += "HS Code:" + packageline.HSCode;
                    }

                    if (string.IsNullOrEmpty(myDataProvider.GeneralPackageslinesDescriptionOfGoods))
                    {
                        myDataProvider.GeneralPackageslinesDescriptionOfGoods = packageline.PackageDescriptionOfGoods;
                    }
                    else
                    {
                        myDataProvider.GeneralPackageslinesDescriptionOfGoods = myDataProvider.GeneralPackageslinesDescriptionOfGoods + " " + packageline.PackageDescriptionOfGoods;
                    }

                    packageline.PackageGrossWeight = package.Weight != null ? String.Format("{0:0,0.00}", package.Weight.Value) + " " + (shipment.GrossWeightUnitCode != null ? shipment.GrossWeightUnitCode : "") : "";
                    packageline.PackageVolumetricWeight = package.VolumetricWeight != null ? String.Format("{0:0,0.00}", package.VolumetricWeight) + (shipment.ChargeableWeightUnitCode != null ? shipment.ChargeableWeightUnitCode : "") : "";
                    packageline.PackageGrossWeight_Double = package.Weight;

                    if (shipment.ShipmentTypeId == "FCLD")
                    {
                        if (insidePackages.Count != 0)
                        {
                            packageline.PackageQuantity = insidePackages.Sum(d => d.Quantity).ToString(); //String.Format("{0:#0.00}", insidePackages.Sum(d => d.Quantity));
                        }
                        else
                        {
                            packageline.PackageQuantity = package.Quantity != null ? package.Quantity.Value.ToString() : "";
                        }
                    }
                    else
                    {
                        packageline.PackageQuantity = package.Quantity != null ? package.Quantity.Value.ToString() : "";
                    }

                    packageline.PackageVolume = package.Volume != null ? (package.Volume) + " " + volumeUnitCode : "";
                    packageline.PackageVolume_Double = package.Volume;

                    if (packagetype != null)
                    {
                        packageline.PackageTypeCode = packagetype.Code;

                        if (packagetype.IsContainer)
                        {
                            packageline.PackageType = packagetype != null ? packagetype.PrintAs : "";

                            if (string.IsNullOrEmpty(myDataProvider.Dimensions))
                            {
                                myDataProvider.Dimensions = packageline.PackageType + "_" + packagetype.TEU.ToString();
                            }
                            else
                            {
                                myDataProvider.Dimensions = myDataProvider.Dimensions + ", " + packageline.PackageType + "_" + packagetype.TEU.ToString();
                            }
                        }

                        else
                        {
                            packageline.PackageType = packagetype != null ? packagetype.EnglishName : "";//package.PackageType.IsContainer != true ? "Package" : "Container";                    
                        }

                        if (!string.IsNullOrEmpty(package.ContainerNumber))
                        {
                            string containerNo = package.ContainerNumber;
                            str.Append(containerNo);
                            str.Append(',');

                            string type = !string.IsNullOrEmpty(packagetype.PrintAs) ? packagetype.PrintAs : packagetype.Code;

                            str2.Append(containerNo);
                            str2.Append(' ');
                            str2.Append(type);
                            str2.Append(',');
                        }

                        string packageCode = packagetype.PrintAs;
                        packageline.PackageQuantityAndType = packageline.PackageQuantity + "x" + packageCode;

                        if (package.MarksAndNumbers == null)
                        {
                            if (packagetype.IsContainer)
                            {
                                if (package.ContainerNumber != null)
                                {
                                    if (package.ContainerNumber.Length > 10)
                                    {
                                        packageline.PackageMarksAndNumbers = !string.IsNullOrEmpty(package.ContainerNumber) ? package.ContainerNumber.Substring(0, 4) + " " + package.ContainerNumber.Substring(4, 6) + "/" + package.ContainerNumber.Substring(10, 1) + Environment.NewLine : "";
                                    }
                                    else
                                    {
                                        packageline.PackageMarksAndNumbers = package.ContainerNumber;
                                    }
                                }

                                if (!string.IsNullOrEmpty(package.ShipperSeal))
                                {
                                    packageline.PackageMarksAndNumbers = packageline.PackageMarksAndNumbers + "SEAL:" + package.ShipperSeal + Environment.NewLine;
                                }

                                if (package.Tare != null)
                                {
                                    packageline.PackageGrossWeight = packageline.PackageGrossWeight + Environment.NewLine + "TARE:" + package.Tare + (shipment.GrossWeightUnitCode != null ? shipment.GrossWeightUnitCode : "");
                                }
                            }
                        }
                        else
                        {
                            packageline.PackageMarksAndNumbers = package.MarksAndNumbers != null ? package.MarksAndNumbers : "";
                        }
                    }

                    //InsidePackages
                    foreach (InsideShipmentPackage insideItem in insidePackages)
                    {
                        PackageType insidePackageType = (from pa in commonContext.PackageTypes
                                                         where pa.Id == insideItem.PackageTypeId
                                                         select pa).FirstOrDefault();

                        InsidePackageLine insidePackage = new InsidePackageLine();

                        insidePackage.PackageType = insidePackageType == null ? "" : insidePackageType.EnglishName;
                        insidePackage.Quantity = insideItem.Quantity;

                        if (insideItem.Length != null && insideItem.Width != null && insideItem.Height != null)
                        {
                            insidePackage.Dimensions = insideItem.Length + "x" + insideItem.Width + "x" + insideItem.Height;
                        }

                        insidePackage.Volume = insideItem.Volume;
                        insidePackage.VolumetricWeight = insideItem.VolumetricWeight;
                        insidePackage.Weight = insideItem.Weight;
                        insidePackage.Description = insideItem.Description;
                        insidePackage.Reference1 = insideItem.Reference1;
                        insidePackage.Reference2 = insideItem.Reference2;
                        insidePackage.Reference3 = insideItem.Reference3;
                        insidePackage.CommodityNumber = insideItem.CommodityNumber;

                        #region Car Details
                        insidePackage.Make = insideItem.Make;
                        insidePackage.Model = insideItem.Model;
                        insidePackage.Year = insideItem.Year;
                        insidePackage.Color = insideItem.Color;
                        insidePackage.ChassisNumber = insideItem.ChassisNumber;
                        insidePackage.RegistrationNumber = insideItem.RegistrationNumber;

                        if (!string.IsNullOrEmpty(insideItem.CountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(insideItem.CountryId, tenant);
                            if (country != null)
                            {
                                insidePackage.CountryName = country.EnglishName;
                            }
                        }
                        #endregion

                        packageline.InsidePackagesLines.Add(insidePackage);

                        if (string.IsNullOrEmpty(packageline.InsidePackagesDescription))
                        {
                            packageline.InsidePackagesDescription = insidePackage.Quantity + insidePackage.PackageType;
                        }

                        else
                        {
                            packageline.InsidePackagesDescription = packageline.InsidePackagesDescription + " - " + insidePackage.Quantity + insidePackage.PackageType;
                        }
                    }

                    packageline.Reference1 = package.Reference1;
                    packageline.Reference2 = package.Reference2;
                    packageline.Reference3 = package.Reference3;
                    packageline.Reference4 = package.Reference4;
                    packageline.CommodityNumber = package.CommodityNumber;

                    if (myDataProvider.HasAttachmentList == "True")
                    {
                        if (shipment.ShipmentTypeName == "My Groupage")
                        {
                            PackageLine newLine = new PackageLine();
                            this.GetInsidePackagesData(shipmentsContext, commonContext, package, shipment, newLine);
                            myDataProvider.AttachmentList.Add(newLine);
                        }
                        else
                        {
                            myDataProvider.AttachmentList.Add(packageline);
                        }
                    }
                    else
                    {
                        myDataProvider.PackagesLines.Add(packageline);
                    }

                    this.ComputeDangerousFeild(packageline);
                    myDataProvider.DangerousPackages.Add(packageline);
                }

                string str_String = str.ToString();
                if (!string.IsNullOrEmpty(str_String))
                {
                    str_String = str_String.TrimEnd(',');
                }

                string str2_String = str2.ToString();
                if (!string.IsNullOrEmpty(str2_String))
                {
                    str2_String = str2_String.TrimEnd(',');
                }

                myDataProvider.ContainersNumbersArray = str_String;
                myDataProvider.ContainersNumbersAndTypesArray = str2_String;
                #endregion

                #region Attachment List
                if (myDataProvider.HasAttachmentList == "True" && packages.Count > 0)
                {
                    var resultquery = from att in packages
                                      join sm in commonContext.PackageTypes
                                      on att.PackageTypeId equals sm.Id into packageTypeJoin
                                      from m in packageTypeJoin.DefaultIfEmpty()
                                      group new { att.Quantity, att.Weight, att.Volume } by m.EnglishName into newGroup
                                      orderby newGroup.Sum(s => Convert.ToInt32(s.Quantity)) descending
                                      select new { Type = newGroup.Key, Count = newGroup.Sum(s => Convert.ToInt32(s.Quantity != null ? s.Quantity.Value : 0)), Weight = newGroup.Sum(s => s.Weight != null ? s.Weight.Value : 0), Volume = newGroup.Sum(s => s.Volume != null ? s.Volume.Value : 0) };

                    StringBuilder packageNumberstrbuilder = new StringBuilder();
                    StringBuilder packageTypestrbuilder = new StringBuilder();
                    StringBuilder packageGrossweighttrbuilder = new StringBuilder();
                    StringBuilder packageVolumeBuilder = new StringBuilder();

                    foreach (var res in resultquery)
                    {
                        packageNumberstrbuilder.AppendLine(res.Count.ToString());

                        packageTypestrbuilder.AppendLine(res.Type.ToString());
                        packageGrossweighttrbuilder.AppendLine((String.Format("{0:0,0.00}", res.Weight)) + (shipment.GrossWeightUnitCode != null ? shipment.GrossWeightUnitCode : ""));
                        packageVolumeBuilder.AppendLine((String.Format("{0:0,0.00}", res.Volume)) + (shipment.VolumeUnitCode != null ? shipment.VolumeUnitCode : ""));
                    }

                    myDataProvider.CustomPackagesNumber = packageNumberstrbuilder.ToString();
                    myDataProvider.CustomPackageType = packageTypestrbuilder.ToString();
                    myDataProvider.CustomWeight = packageGrossweighttrbuilder.ToString();
                    myDataProvider.CustomVolume = packageVolumeBuilder.ToString().StartsWith("00.00") == false ? packageVolumeBuilder.ToString() : "";
                }
                #endregion

                #region Transshipments Carrier Number
                if (!string.IsNullOrEmpty(shipment.Transshipment1CarrierNumber))
                {
                    string pre = "";
                    if (shipment.TransportModeId == "A")
                    {
                        pre = shipment.Transshipment1CarrierPrefix;
                    }

                    myDataProvider.Transshipment1CarrierNumber = pre + shipment.Transshipment1CarrierNumber;
                }

                if (!string.IsNullOrEmpty(shipment.Transshipment2CarrierNumber))
                {
                    string pre = "";
                    if (shipment.TransportModeId == "A")
                    {
                        pre = shipment.Transshipment2CarrierPrefix;
                    }

                    myDataProvider.Transshipment2CarrierNumber = pre + shipment.Transshipment2CarrierNumber;
                }

                if (!string.IsNullOrEmpty(shipment.Transshipment3CarrierNumber))
                {
                    string pre = "";
                    if (shipment.TransportModeId == "A")
                    {
                        pre = shipment.Transshipment3CarrierPrefix;
                    }

                    myDataProvider.Transshipment3CarrierNumber = pre + shipment.Transshipment3CarrierNumber;
                }
                #endregion

                #region Assemblies
                if (shipment.ShipmentAssemblies.Count > 0)
                {
                    myDataProvider.Assemblies = new List<ShipmentAssemblyLine>();

                    foreach (ShipmentAssemblyPM assembly in shipment.ShipmentAssemblies)
                    {
                        myDataProvider.Assemblies.Add(new ShipmentAssemblyLine()
                        {
                            House = assembly.House,
                            ShipperName = assembly.ShipperName
                        });
                    }
                }
                #endregion

                #region Pickups
                List<ShipmentPickUpPM> pickUps = shipmentPickUpQuery.GetShipmentPickUpPMsByTenantAndShipment(shipment.Id, tenant);

                if (pickUps.Count > 0)
                {
                    myDataProvider.PickUpsLines = new List<PickUpDeliveryLine>();

                    foreach (ShipmentPickUpPM pickup in pickUps)
                    {
                        PickUpDeliveryLine newItem = new PickUpDeliveryLine();
                        newItem.ETD = pickup.ETD;
                        newItem.ETA = pickup.ETA;
                        newItem.ATD = pickup.ATD;
                        newItem.ATA = pickup.ATA;
                        newItem.Notes = pickup.Notes;
                        newItem.TransportMode = pickup.TransportModeName;
                        newItem.Weight = pickup.ShipmentPickUpDeliveryPackages.Sum(s => s.Weight);
                        myServicHelper.GetPickUpFromAddress(pickup, newItem, addressRepository, tenant);

                        foreach (ShipmentPickUpDeliveryPackagePM package in pickup.ShipmentPickUpDeliveryPackages)
                        {
                            PackageLine newPackage = new PackageLine();
                            newPackage.PackageDescriptionOfGoods = package.Description;
                            newPackage.ContainerNumber = package.ContainerNumber;
                            newPackage.SealNumber = package.ShipperSeal;
                            newPackage.PackageQuantity = package.Quantity.ToString();
                            newPackage.PackageTypeName = package.PackageTypeName;
                            newPackage.PackageVolume_Double = package.Volume;
                            newPackage.PackageGrossWeight = String.Format("{0:0,0.00}", package.Weight);

                            if (package.Length != null && package.Width != null && package.Height != null)
                            {
                                newPackage.Dimensions = package.Length + "x" + package.Width + "x" + package.Height;
                            }

                            #region Car Details
                            newPackage.Make = package.Make;
                            newPackage.Model = package.Model;
                            newPackage.Year = package.Year;
                            newPackage.Color = package.Color;
                            newPackage.ChassisNumber = package.ChassisNumber;
                            newPackage.RegistrationNumber = package.RegistrationNumber;

                            if (!string.IsNullOrEmpty(package.CountryId))
                            {
                                Country country = countryRepository.GetSingleCountry(package.CountryId, tenant);
                                if (country != null)
                                {
                                    newPackage.CountryName = country.EnglishName;
                                }
                            }
                            #endregion

                            #region Harmonize
                            if (package.IsMultiHarmonize)
                            {
                                List<PickUpDeliveryPackageHarmonize> allHarmonizes = shipmentsContext.PickUpDeliveryPackageHarmonizes.Where(d => d.PackageId == package.Id && d.Tenant == package.Tenant).ToList();
                                foreach (PickUpDeliveryPackageHarmonize itemHarmonize in allHarmonizes)
                                {
                                    if (string.IsNullOrEmpty(newPackage.HSCode))
                                    {
                                        newPackage.HSCode = itemHarmonize.Harmonize;
                                    }
                                    else
                                    {
                                        newPackage.HSCode += "," + itemHarmonize.Harmonize;
                                    }
                                }
                            }
                            else
                            {
                                newPackage.HSCode = package.Harmonize;
                            }
                            #endregion

                            newItem.PickUpDeliveryPackages.Add(newPackage);
                        }

                        myDataProvider.PickUpsLines.Add(newItem);
                    }
                }
                #endregion

                #region Deliveries
                List<ShipmentDeliveryPM> deliveries = shipmentDeliveryQuery.GetShipmentDeliveryPMsByTenantAndShipment(shipment.Id, tenant);

                if (deliveries.Count > 0)
                {
                    myDataProvider.DeliveriesLines = new List<PickUpDeliveryLine>();

                    foreach (ShipmentDeliveryPM deliv in deliveries)
                    {
                        PickUpDeliveryLine newItem = new PickUpDeliveryLine();
                        newItem.ETD = deliv.ETD;
                        newItem.ETA = deliv.ETA;
                        newItem.ATD = deliv.ATD;
                        newItem.ATA = deliv.ATA;
                        newItem.Notes = deliv.Notes;
                        newItem.TransportMode = deliv.TransportModeName;
                        newItem.Weight = deliv.ShipmentPickUpDeliveryPackages.Sum(s => s.Weight);
                        myServicHelper.GetDeliveryToAddress(deliv, newItem, addressRepository, tenant);

                        #region Empty Container
                        newItem.EmptyContainerReturnRef = deliv.EmptyDeliveryDepotReference;

                        if (!string.IsNullOrEmpty(deliv.EmptyDeliveryContainerPartnerId))
                        {
                            CardPM cardPM = cardQuery.GetSinglePM(deliv.EmptyDeliveryContainerPartnerId, tenant);

                            if (cardPM != null)
                            {
                                string myEmptyContainer = null;
                                string myEmptyContainerName = null;
                                string myEmptyContainerAddress = null;

                                myEmptyContainer = cardPM.EnglishName != null ? cardPM.EnglishName : "";
                                myEmptyContainerName = cardPM.EnglishName != null ? cardPM.EnglishName : "";

                                if (cardPM.MainAddressId != null)
                                {
                                    Address theAddress = addressRepository.GetSingleAddress(cardPM.MainAddressId, tenant);

                                    if (theAddress != null)
                                    {
                                        if (theAddress.IsLocalLanguage && !string.IsNullOrEmpty(cardPM.LocalName))
                                        {
                                            myEmptyContainer = cardPM.LocalName;
                                            myEmptyContainerName = cardPM.LocalName;
                                        }

                                        myEmptyContainer = myEmptyContainer + Environment.NewLine + DataProviders.General.GetAddress(theAddress);
                                        myEmptyContainerAddress = DataProviders.General.GetAddress(theAddress);

                                        if (theAddress.PhoneNumber != null)
                                        {
                                            myEmptyContainer = myEmptyContainer + Environment.NewLine + "Phone No. : " + theAddress.PhoneNumber;
                                            myEmptyContainerAddress = myEmptyContainerAddress + Environment.NewLine + "Phone No. : " + theAddress.PhoneNumber;
                                        }

                                        if (theAddress.FaxNumber != null)
                                        {
                                            myEmptyContainer = myEmptyContainer + "   Fax No. : " + theAddress.FaxNumber;
                                            myEmptyContainerAddress = myEmptyContainerAddress + "   Fax No. : " + theAddress.FaxNumber;
                                        }
                                    }
                                }

                                newItem.EmptyContainerReturn = myEmptyContainer;
                                newItem.EmptyContainerReturnName = myEmptyContainerName;
                                newItem.EmptyContainerReturnAddress = myEmptyContainerAddress;
                            }
                        }
                        #endregion

                        #region Trucker Contact
                        if (deliv.CarrierId != null)
                        {
                            Card iCard = (from d in commonContext.Cards where d.Id == deliv.CarrierId select d).FirstOrDefault();
                            if (iCard != null)
                            {
                                if (!string.IsNullOrEmpty(iCard.PrimaryContactId))
                                {
                                    Contact iContact = contactRepository.GetSingleContact(iCard.PrimaryContactId, tenant);
                                    if (iContact != null)
                                    {
                                        newItem.TruckerContactName = iContact.EnglishName;
                                    }

                                }
                            }
                        }
                        #endregion

                        foreach (ShipmentPickUpDeliveryPackagePM package in deliv.ShipmentPickUpDeliveryPackages)
                        {
                            PackageLine newPackage = new PackageLine();
                            newPackage.PackageDescriptionOfGoods = package.Description;
                            newPackage.ContainerNumber = package.ContainerNumber;
                            newPackage.SealNumber = package.ShipperSeal;
                            newPackage.PackageQuantity = package.Quantity.ToString();
                            newPackage.PackageTypeName = package.PackageTypeName;
                            newPackage.PackageVolume_Double = package.Volume;
                            newPackage.PackageGrossWeight = String.Format("{0:0,0.00}", package.Weight);

                            if (package.Length != null && package.Width != null && package.Height != null)
                            {
                                newPackage.Dimensions = package.Length + "x" + package.Width + "x" + package.Height;
                            }

                            #region Car Details
                            newPackage.Make = package.Make;
                            newPackage.Model = package.Model;
                            newPackage.Year = package.Year;
                            newPackage.Color = package.Color;
                            newPackage.ChassisNumber = package.ChassisNumber;
                            newPackage.RegistrationNumber = package.RegistrationNumber;

                            if (!string.IsNullOrEmpty(package.CountryId))
                            {
                                Country country = countryRepository.GetSingleCountry(package.CountryId, tenant);
                                if (country != null)
                                {
                                    newPackage.CountryName = country.EnglishName;
                                }
                            }
                            #endregion

                            #region Harmonize
                            if (package.IsMultiHarmonize)
                            {
                                List<PickUpDeliveryPackageHarmonize> allHarmonizes = shipmentsContext.PickUpDeliveryPackageHarmonizes.Where(d => d.PackageId == package.Id && d.Tenant == package.Tenant).ToList();
                                foreach (PickUpDeliveryPackageHarmonize itemHarmonize in allHarmonizes)
                                {
                                    if (string.IsNullOrEmpty(newPackage.HSCode))
                                    {
                                        newPackage.HSCode = itemHarmonize.Harmonize;
                                    }
                                    else
                                    {
                                        newPackage.HSCode += "," + itemHarmonize.Harmonize;
                                    }
                                }
                            }
                            else
                            {
                                newPackage.HSCode = package.Harmonize;
                            }
                            #endregion

                            newItem.PickUpDeliveryPackages.Add(newPackage);
                        }

                        myDataProvider.DeliveriesLines.Add(newItem);
                    }


                    string totalEmptyContainerReturn = "";
                    string totalEmptyContainerReturnRef = "";
                    string totalEmptyContainerReturnName = "";
                    string totalEmptyContainerReturnAddress = "";

                    foreach (PickUpDeliveryLine item in myDataProvider.DeliveriesLines)
                    {
                        if (!string.IsNullOrEmpty(item.EmptyContainerReturn))
                        {
                            if (string.IsNullOrEmpty(totalEmptyContainerReturn))
                            {
                                totalEmptyContainerReturn = item.EmptyContainerReturn;
                            }

                            else
                            {
                                totalEmptyContainerReturn += "," + item.EmptyContainerReturn;
                            }
                        }

                        if (!string.IsNullOrEmpty(item.EmptyContainerReturnRef))
                        {
                            if (string.IsNullOrEmpty(totalEmptyContainerReturnRef))
                            {
                                totalEmptyContainerReturnRef = item.EmptyContainerReturnRef;
                            }

                            else
                            {
                                totalEmptyContainerReturnRef += "," + item.EmptyContainerReturnRef;
                            }
                        }

                        if (!string.IsNullOrEmpty(item.EmptyContainerReturnName))
                        {
                            if (string.IsNullOrEmpty(totalEmptyContainerReturnName))
                            {
                                totalEmptyContainerReturnName = item.EmptyContainerReturnName;
                            }

                            else
                            {
                                totalEmptyContainerReturnName += "," + item.EmptyContainerReturnName;
                            }
                        }

                        if (!string.IsNullOrEmpty(item.EmptyContainerReturnAddress))
                        {
                            if (string.IsNullOrEmpty(totalEmptyContainerReturnAddress))
                            {
                                totalEmptyContainerReturnAddress = item.EmptyContainerReturnAddress;
                            }

                            else
                            {
                                totalEmptyContainerReturnAddress += "," + item.EmptyContainerReturnAddress;
                            }
                        }
                    }

                    myDataProvider.EmptyContainerReturn = totalEmptyContainerReturn;
                    myDataProvider.EmptyContainerReturnRef = totalEmptyContainerReturnRef;
                    myDataProvider.EmptyContainerReturnName = totalEmptyContainerReturnName;
                    myDataProvider.EmptyContainerReturnAddress = totalEmptyContainerReturnAddress;
                }
                #endregion

                if (string.IsNullOrEmpty(myDataProvider.GeneralPackageslinesDescriptionOfGoods))
                {
                    myDataProvider.GeneralPackageslinesDescriptionOfGoods = myDataProvider.GeneralDescriptionOfGoods;
                }

                if (shipment.MAWBOBLDate != null)
                {
                    myDataProvider.PlaceAndDateOfIssue = myDataProvider.PlaceAndDateOfIssue + " " + String.Format("{0:dd MMM yyyy}", shipment.MAWBOBLDate.Value);
                }

                else
                {
                    myDataProvider.PlaceAndDateOfIssue = myDataProvider.PlaceAndDateOfIssue + " " + String.Format("{0:dd MMM yyyy}", TenantServerConfigration.GetCurrentDateTime(tenant));
                }

                CustomFieldResolver customFieldResolver = new CustomFieldResolver();
                customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, shipment, myDataProvider);

                if (myFirstPickup != null)
                    myDataProvider.FirstFrom = myServicHelper.GetPickUpDeliveryFromCityOrPortName(myFirstPickup);
                else if (shipment.PreCarriageFromPortId != null)
                    myDataProvider.FirstFrom = shipment.PreCarriageFromPortName + " - " + shipment.PreCarriageFromPortCountryName;
                else
                    myDataProvider.FirstFrom = shipment.MainCarriageFromPortName + " - " + shipment.MainCarriageFromPortCountryName;

                if (myLastDelivery != null)
                    myDataProvider.LastTo = myServicHelper.GetToDeliveryName(shipment, myLastDelivery);
                else if (shipment.OnCarriageToPortId != null)
                    myDataProvider.LastTo = shipment.OnCarriageToPortName + " - " + shipment.OnCarriageToPortCountryName;
                else if (shipment.Transshipment3ToPortId != null)
                    myDataProvider.LastTo = shipment.Transshipment3ToPortName + " - " + shipment.Transshipment3ToPortCountryName;
                else if (shipment.Transshipment2ToPortId != null)
                    myDataProvider.LastTo = shipment.Transshipment2ToPortName + " - " + shipment.Transshipment2ToPortCountryName;
                else if (shipment.Transshipment1ToPortId != null)
                    myDataProvider.LastTo = shipment.Transshipment1ToPortName + " - " + shipment.Transshipment1ToPortCountryName;
                else if (shipment.MainCarriageToPortId != null)
                    myDataProvider.LastTo = shipment.MainCarriageToPortName + " - " + shipment.MainCarriageToPortCountryName;

                #region Warehouse Leg
                myDataProvider.WarehouseLegExpectedEntryDate = shipment.WarehouseLegExpectedEntryDate;
                myDataProvider.WarehouseLegActualEntryDate = shipment.WarehouseLegActualEntryDate;
                myDataProvider.WarehouseLegExpectedReleaseDate = shipment.WarehouseLegExpectedReleaseDate;
                myDataProvider.WarehouseLegActualReleaseDate = shipment.WarehouseLegActualReleaseDate;
                myDataProvider.WarehouseLegLastFreeDate = shipment.WarehouseLegLastFreeDate;
                myDataProvider.WarehouseLegCutOffDate = shipment.WarehouseLegCutOffDate;
                myDataProvider.WarehouseLegVGMCutOffDate = shipment.WarehouseLegVGMCutOffDate;
                myDataProvider.WarehouseLegRemarks = shipment.WarehouseLegRemarks;
                myDataProvider.WarehouseLegReference = shipment.WarehouseLegReference;
                myDataProvider.WarehouseLegTerminalName = shipment.WarehouseLegTerminalName;
                if (shipment.WarehouseLegAddressId != null)
                {
                    Address warehouseAddress = addressRepository.GetSingleAddress(shipment.WarehouseLegAddressId, tenant);
                    myDataProvider.WarehouseLegAddress = DataProviders.General.GetAddress(warehouseAddress);
                }
                myDataProvider.WarehouseLegEntryDate = shipment.WarehouseLegEntryDate;
                myDataProvider.WarehouseLegReleaseDate = shipment.WarehouseLegReleaseDate;
                myDataProvider.WarehouseLegTerminalCode = shipment.WarehouseLegTerminalCode;
                #endregion
            }

            try
            {
                Type sdType = myDataProvider.GetType();
                PropertyInfo[] properties = sdType.GetProperties();
                foreach (PropertyInfo pi in properties)
                {

                    if (pi.GetValue(myDataProvider, null) == null || pi.GetValue(myDataProvider, null).ToString() == "0" || pi.GetValue(myDataProvider, null).ToString() == "00.00")
                    {
                        pi.SetValue(myDataProvider, "", null);
                    }
                }
            }

            catch
            {

            }

            return myDataProvider;
        }

        private void GetInsidePackagesData(IShipmentsContext context, ICommonDataContext commonContext, ShipmentPackage package, ShipmentPM shipment, PackageLine line)
        {
            List<InsideShipmentPackage> insidePackages = context.InsideShipmentPackages.Where(d => d.ShipmentPackageId == package.Id && d.Tenant == package.Tenant).ToList();
            PackageType mainPackageType = commonContext.PackageTypes.Where(p => p.Id == package.PackageTypeId).FirstOrDefault();

            StringBuilder qty = new StringBuilder();
            StringBuilder type = new StringBuilder();
            StringBuilder desc = new StringBuilder();
            StringBuilder weight = new StringBuilder();
            StringBuilder volume = new StringBuilder();
            StringBuilder marks = new StringBuilder();
            StringBuilder Volumetricweight = new StringBuilder();
            int count = 0;
            int totalCount = 0;

            foreach (InsideShipmentPackage insPackage in insidePackages)
            {
                PackageType packageType = commonContext.PackageTypes.Where(p => p.Id == insPackage.PackageTypeId).FirstOrDefault();

                count = myServicHelper.GetStringLinesCount(insPackage.Description);
                totalCount = totalCount + count + 1;

                qty.Append(insPackage.Quantity.ToString());

                if (packageType.IsContainer)
                {
                    type.Append(packageType != null ? packageType.PrintAs : "");
                }
                else
                {
                    type.Append(packageType != null ? packageType.EnglishName : "");
                }

                desc.Append(insPackage.Description != null ? insPackage.Description : "");
                weight.Append(String.Format("{0:#0.00}", (insPackage.Weight == null ? 0 : insPackage.Weight.Value)) + " " + (shipment.GrossWeightUnitCode != null ? shipment.GrossWeightUnitCode : ""));
                volume.Append(String.Format("{0:#0.00}", (insPackage.Volume == null ? 0 : insPackage.Volume.Value)) + " " + (shipment.VolumeUnitCode != null ? shipment.VolumeUnitCode : ""));
                Volumetricweight.Append(String.Format("{0:#0.00}", (insPackage.VolumetricWeight == null ? 0 : insPackage.VolumetricWeight.Value)) + " " + (shipment.ChargeableWeightUnitCode != null ? shipment.ChargeableWeightUnitCode : ""));

                for (int i = 0; i <= count; i++)
                {
                    qty.Append('\n');
                    type.Append('\n');
                    weight.Append('\n');
                    volume.Append('\n');
                    Volumetricweight.Append('\n');
                }
                desc.Append('\n');
                desc.Append('\n');
            }

            //in the package QTY in the FCL case, show the inside packages (ask Zaki)
            if (shipment.ShipmentTypeId == "FCLD")
            {
                line.PackageQuantity = insidePackages.Sum(d => d.Quantity).ToString(); //String.Format("{0:#0.00}", insidePackages.Sum(d => d.Quantity));
            }
            else
            {
                line.PackageQuantity = qty.ToString() + package.Quantity;
            }

            if (mainPackageType.IsContainer)
            {
                line.PackageType = type.ToString() + (mainPackageType != null ? mainPackageType.PrintAs : "");
            }
            else
            {
                line.PackageType = type.ToString() + (mainPackageType != null ? mainPackageType.EnglishName : "");
            }

            line.PackageDescriptionOfGoods = desc.ToString() + (package.Description != null ? package.Description : "");
            line.PackageGrossWeight = weight.ToString() + " " + String.Format("{0:#0.00}", package.Weight.Value) + " " + (shipment.GrossWeightUnitCode != null ? shipment.GrossWeightUnitCode : "");
            line.PackageVolume = volume.ToString() + String.Format("{0:#0.00}", package.Volume.Value) + " " + (shipment.VolumeUnitCode != null ? shipment.VolumeUnitCode : "");
            line.PackageVolumetricWeight = Volumetricweight.ToString() + " " + String.Format("{0:#0.00}", package.VolumetricWeight.Value) + " " + (shipment.ChargeableWeightUnitCode != null ? shipment.ChargeableWeightUnitCode : "");

            #region Car Details
            line.Make = package.Make;
            line.Model = package.Model;
            line.Year = package.Year;
            line.Color = package.Color;
            line.ChassisNumber = package.ChassisNumber;
            line.RegistrationNumber = package.RegistrationNumber;

            if (!string.IsNullOrEmpty(package.CountryId))
            {
                CountryRepository countryRepository = new CountryRepository(tenant);
                Country country = countryRepository.GetSingleCountry(package.CountryId, tenant);
                if (country != null)
                {
                    line.CountryName = country.EnglishName;
                }
            }
            #endregion
            
            if (package.MarksAndNumbers == null)
            {
                for (int i = 0; i < totalCount; i++)
                {
                    marks.Append('\n');
                }

                if (mainPackageType.IsContainer)
                {
                    if (package.ContainerNumber != null)
                    {
                        if (package.ContainerNumber.Length > 10)
                        {
                            marks.Append(!string.IsNullOrEmpty(package.ContainerNumber) ? package.ContainerNumber.Substring(0, 4) + " " + package.ContainerNumber.Substring(4, 6) + "/" + package.ContainerNumber.Substring(10, 1) : "").Append('\n');
                        }
                        else
                        {
                            marks.Append(package.ContainerNumber);
                        }
                    }
                    if (!string.IsNullOrEmpty(package.ShipperSeal))
                    {
                        marks.Append("SEAL:" + package.ShipperSeal);
                    }

                    if (package.Tare != null)
                    {
                        line.PackageGrossWeight += Environment.NewLine + "TARE:" + package.Tare + (shipment.GrossWeightUnitCode != null ? shipment.GrossWeightUnitCode : "");
                    }
                }
                line.PackageMarksAndNumbers = marks.ToString() + "\n" + (package.ContainerNumber != null ? "Total For " + package.ContainerNumber : "");
            }
            else
            {
                line.PackageMarksAndNumbers = package.MarksAndNumbers != null ? package.MarksAndNumbers : "";
                line.PackageMarksAndNumbers = marks.ToString() + "\n" + (package.ContainerNumber != null ? "Total For " + package.ContainerNumber : "");
            }
        }
        private void ComputeDangerousFeild(PackageLine packageline)
        {

            if (!string.IsNullOrEmpty(packageline.InsidePackagesDescription))
            {
                packageline.DangerousDescription = packageline.InsidePackagesDescription;
            }

            if (!string.IsNullOrEmpty(packageline.Description))
            {
                if (string.IsNullOrEmpty(packageline.DangerousDescription))
                {
                    packageline.DangerousDescription = packageline.Description;
                }

                else
                {
                    packageline.DangerousDescription = packageline.DangerousDescription + Environment.NewLine + packageline.Description;
                }
            }

            if (!string.IsNullOrEmpty(packageline.HSCode))
            {
                if (string.IsNullOrEmpty(packageline.DangerousDescription))
                {
                    packageline.DangerousDescription = "HS Code " + packageline.HSCode;
                }

                else
                {
                    packageline.DangerousDescription = packageline.DangerousDescription + Environment.NewLine + "HS Code " + packageline.HSCode;
                }
            }

            if (!string.IsNullOrEmpty(packageline.CeficClass))
            {
                if (string.IsNullOrEmpty(packageline.DangerousDescription))
                {
                    packageline.DangerousDescription = "Cefic Class " + packageline.CeficClass;
                }

                else
                {
                    packageline.DangerousDescription = packageline.DangerousDescription + Environment.NewLine + "Cefic Class " + packageline.CeficClass;
                }
            }

            if (!string.IsNullOrEmpty(packageline.IMDGCode))
            {
                if (string.IsNullOrEmpty(packageline.DangerousDescription))
                {
                    packageline.DangerousDescription = "IMDG Code " + packageline.IMDGCode;
                }

                else
                {
                    packageline.DangerousDescription = packageline.DangerousDescription + Environment.NewLine + "IMDG Code " + packageline.IMDGCode;
                }
            }

            if (!string.IsNullOrEmpty(packageline.KemlerCode))
            {
                if (string.IsNullOrEmpty(packageline.DangerousDescription))
                {
                    packageline.DangerousDescription = "Kemler Code " + packageline.KemlerCode;
                }

                else
                {
                    packageline.DangerousDescription = packageline.DangerousDescription + Environment.NewLine + "Kemler Code " + packageline.KemlerCode;
                }
            }

            if (!string.IsNullOrEmpty(packageline.UNCode))
            {
                if (string.IsNullOrEmpty(packageline.DangerousDescription))
                {
                    packageline.DangerousDescription = "UN Code " + packageline.UNCode;
                }

                else
                {
                    packageline.DangerousDescription = packageline.DangerousDescription + Environment.NewLine + "UN Code " + packageline.UNCode;
                }
            }

            if (!string.IsNullOrEmpty(packageline.MarinePollutant))
            {
                if (string.IsNullOrEmpty(packageline.DangerousDescription))
                {
                    packageline.DangerousDescription = "Marine Pollutant " + packageline.MarinePollutant;
                }

                else
                {
                    packageline.DangerousDescription = packageline.DangerousDescription + Environment.NewLine + "Marine Pollutant " + packageline.MarinePollutant;
                }
            }

            if (!string.IsNullOrEmpty(packageline.PackingGroup))
            {
                if (string.IsNullOrEmpty(packageline.DangerousDescription))
                {
                    packageline.DangerousDescription = "Packing Group " + packageline.PackingGroup;
                }

                else
                {
                    packageline.DangerousDescription = packageline.DangerousDescription + Environment.NewLine + "Packing Group " + packageline.PackingGroup;
                }
            }

            if (!string.IsNullOrEmpty(packageline.EMS))
            {
                if (string.IsNullOrEmpty(packageline.DangerousDescription))
                {
                    packageline.DangerousDescription = "EMS " + packageline.EMS;
                }

                else
                {
                    packageline.DangerousDescription = packageline.DangerousDescription + Environment.NewLine + "EMS " + packageline.EMS;
                }
            }

            if (!string.IsNullOrEmpty(packageline.ProperShippingName))
            {
                if (string.IsNullOrEmpty(packageline.DangerousDescription))
                {
                    packageline.DangerousDescription = "Proper Shipping Name " + packageline.ProperShippingName;
                }

                else
                {
                    packageline.DangerousDescription = packageline.DangerousDescription + Environment.NewLine + "Proper Shipping Name " + packageline.ProperShippingName;
                }
            }

            if (!string.IsNullOrEmpty(packageline.PackingCode))
            {
                if (string.IsNullOrEmpty(packageline.DangerousDescription))
                {
                    packageline.DangerousDescription = "Packing Code " + packageline.PackingCode;
                }

                else
                {
                    packageline.DangerousDescription = packageline.DangerousDescription + Environment.NewLine + "Packing Code " + packageline.PackingCode;
                }
            }

            if (!string.IsNullOrEmpty(packageline.FlashPoint))
            {
                if (string.IsNullOrEmpty(packageline.DangerousDescription))
                {
                    packageline.DangerousDescription = "Flashpoint " + packageline.FlashPoint;
                }

                else
                {
                    packageline.DangerousDescription = packageline.DangerousDescription + Environment.NewLine + "Flashpoint " + packageline.FlashPoint;
                }
            }

            if (!string.IsNullOrEmpty(packageline.DangerousDescription))
            {
                packageline.DangerousDescription = packageline.DangerousDescription + Environment.NewLine;
            }
        }
        private string ComputeInsidePackagesDetailsPerPackage(ShipmentPackage package)
        {
            string result = "";

            InsideShipmentPackageQuery query = new InsideShipmentPackageQuery(package.Tenant);
            List<InsideShipmentPackagePM> insidePackages = query.GetInsideShipmentPackages(package.Id, package.Tenant);

            if (insidePackages.Count > 0)
            {
                List<InsidePackageGroup> groupedList = (from b in insidePackages
                                                        group b by new { b.PackageTypeName } into g
                                                        select new InsidePackageGroup()
                                                        {
                                                            PackageTypeName = g.Key.PackageTypeName,
                                                            Count = g.Sum(b => b.Quantity),
                                                        }).ToList();

                foreach (InsidePackageGroup item in groupedList)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result = item.Count + " " + item.PackageTypeName;
                    }

                    else
                    {
                        result = result + " + " + item.Count + " " + item.PackageTypeName;
                    }
                }
            }

            return result;
        }
        private string GetInstructionsField(ShipmentPM shipment, ShipmentPickUpPM myPickup, CardQuery cardQuery)
        {
            string myResult = null;

            if (!string.IsNullOrEmpty(shipment.INTTRAInstructions))
            {
                myResult = shipment.INTTRAInstructions;
            }

            else if (myPickup != null)
            {
                if (string.IsNullOrEmpty(shipment.INTTRAInstructions))
                {
                    if (!string.IsNullOrEmpty(myPickup.Notes))
                    {
                        myResult = myPickup.Notes;
                    }

                    if (!string.IsNullOrEmpty(myPickup.CarrierId))
                    {
                        CardPM trucker = cardQuery.GetSinglePM(myPickup.CarrierId, tenant);
                        if (trucker != null)
                        {
                            myResult = "Trucker : " + (trucker.EnglishName != null ? trucker.EnglishName : "") + Environment.NewLine + (myResult != null ? myResult : "");
                        }
                    }
                }
            }

            return myResult;
        }
        private string GetHasAttachmentListField(ShipmentPM shipment, FormCustomField hasAttachmentListCustomField, DocumentTypeCustomField hasAttachmentListDocumentCustom)
        {
            string myResult = "False";

            if (shipment.SIHasAttachList)
            {
                myResult = "True";
            }

            else if (hasAttachmentListCustomField != null)
            {
                myResult = hasAttachmentListCustomField.Value;
            }

            else if (hasAttachmentListDocumentCustom != null)
            {
                myResult = hasAttachmentListDocumentCustom.DefaultValue;
            }

            return myResult;
        }
        private string GetOriginalsOrCopiesNoField(ShipmentPM shipment, FormCustomField copyOrOriginalCustomField, DocumentTypeCustomField copyOrOriginalDocumentCustom, IShipmentsContext shipmentsContext)
        {
            string myResult = null;

            if (shipment.INTTRADocumentQTY != null && shipment.INTTRADocumentTypeCode != null)
            {
                INTTRADocumentType myINTTRADocumentType = shipmentsContext.INTTRADocumentTypes.Where(d => d.Code == shipment.INTTRADocumentTypeCode).FirstOrDefault();
                if (myINTTRADocumentType != null)
                {
                    myResult = myINTTRADocumentType.Name + shipment.INTTRADocumentQTY;
                }
            }

            else if (copyOrOriginalCustomField != null)
            {
                myResult = copyOrOriginalCustomField.Value;
            }

            else if (copyOrOriginalDocumentCustom != null)
            {
                myResult = copyOrOriginalDocumentCustom.DefaultValue;
            }

            return myResult;
        }
    }
}
