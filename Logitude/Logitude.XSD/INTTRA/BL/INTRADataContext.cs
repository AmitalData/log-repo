using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.XSD.INTTRA.BL
{
    public class INTTRADataContext
    {
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public bool IsValid { get; set; }
        public bool IsLimited { get; set; }
        public bool IsDemoTenant { get; set; }
        public bool IsStockPrepaid { get; set; }
        public bool IsGroupageEntity { get; set; }
        public bool IsCarrierRegisteredToINTTRA { get; set; }
        public bool IsCarrierRegisteredToBranch { get; set; }
        public List<string> Errors { get; set; }
        public ICommonDataContext CommonContext;
        private ComputingPartnerTranslationHelper computingPartnerHelper;
        public INTTRAGeneralMethods iNTTRAGeneralMethods;
        public INTTRADataContext(int teannt, string shipmentId, Simplog.Data.CommonDataModel.EntityPOCOs.Contact loggedContact, ICommonDataContext CommonContext)
        {
            this.Tenant = teannt;
            this.ShipmentId = shipmentId;
            this.LoggedContact = loggedContact;
            this.CommonContext = CommonContext;
            this.computingPartnerHelper = new ComputingPartnerTranslationHelper(Tenant);
            this.iNTTRAGeneralMethods = new INTTRAGeneralMethods();
            this.GetObjects();
            this.GetProperties();
        }

        // Objects & Validating
        public Shipment Shipment { get; set; }
        public ShipmentMasterData MasterData { get; set; }
        public string INTTRA_Alias { get; set; }
        public string INTTRA_OutSettingsId { get; set; }
        private Tenant TenantObject;
        private Address TenantAddress;
        public Port FromPort;
        private Port FinalPort;
        private Branch Branch;
        private MoveType MoveType;
        private string MoveType_Name;
        private Contact LoggedContact;
        private ContactRepository contactRepository;
        private Contact BranchContact;
        private Contact EmergencyContact;
        public Country FromPortCountry;
        private Country FinalPortCountry;
        public ShippingLine MainShippingLine;
        public List<ShipmentPackage> ShipmentPackages = new List<ShipmentPackage>();
        private List<InsideShipmentPackage> InsidePackages = new List<InsideShipmentPackage>();
        private List<ShipmentPackageHarmonize> AllHarmonizes = new List<ShipmentPackageHarmonize>();
        private List<ShipmentPackageHarmonize> AllInsideHarmonizes = new List<ShipmentPackageHarmonize>();
        public IShipmentsContext shipmentContext;
        public ShipmentRepository shipmentRepository;
        private ShipmentMasterDataRepository shipmentMasterDataRepository;
        private void GetObjects()
        {
            this.Errors = new List<string>();
            this.shipmentContext = ShipmentsContext.GetContext(Tenant);
            this.shipmentRepository = new ShipmentRepository(shipmentContext);
            this.shipmentMasterDataRepository = new ShipmentMasterDataRepository(shipmentContext);
            this.Shipment = shipmentRepository.GetSingleShipment(ShipmentId, Tenant);
            this.MasterData = shipmentMasterDataRepository.GetSingleMasterDataWithTransShipmentReferences(Shipment.MasterShipmentDataId);
            this.contactRepository = new ContactRepository(this.CommonContext);


            if (!string.IsNullOrEmpty(this.Shipment.ShipmentTypeId))
            {
                if (this.Shipment.ShipmentTypeId.ToUpper().Contains("MYG"))
                {
                    this.IsGroupageEntity = true;
                }
            }

            this.GetGlobalVariables();
            this.GetObjects_Tenant();
            this.GetObjects_Branch();
            this.GetObjects_MoveType();
            this.GetObjects_INTTRASetting();
            this.GetObjects_ShipmentFields();
            this.GetObjects_ShipmentPorts();
            this.GetObjects_ShipmentCarrier();
            this.GetObjects_ShipmentPackages();
            this.GetObjects_Partners();

            this.IsValid = this.Errors.Count == 0 ? true : false;
        }
        private void GetGlobalVariables()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                IGlobalContext globalContext = GlobalContext.GetContext();
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository(globalContext);
                SettingRepository mySettingRepository = new SettingRepository();
                var isDemoTenant = mySettingRepository.IsDemoTenant(Tenant.ToString());

                bool isINTTRAOnlyDemo = false;
                TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(Tenant);
                if (tenantManagement != null)
                {
                    isINTTRAOnlyDemo = tenantManagement.IsINTTRAOnlyDemo;
                    this.IsStockPrepaid = tenantManagement.IsINTTRAStockPrepaid;

                    if (!string.IsNullOrEmpty(this.Shipment.INTTRASIStatusCode))
                    {
                        if (this.Shipment.INTTRASIStatusCode != "NSEN" && this.Shipment.INTTRASIStatusCode != "RJIN")
                        {
                            List<TenantManagementLicense> Licenses = new List<TenantManagementLicense>();

                            if (tenantManagement.IsMultiPackage)
                            {
                                Licenses = (from d in globalContext.TenantManagementLicenses where d.Tenant == Tenant select d).ToList();
                            }

                            bool isDevelopment = false;
                            if (tenantManagement != null)
                            {
                                if (tenantManagement.IsMultiPackage)
                                {
                                    List<string> myGroupedList = (from d in Licenses
                                                                  group d by d.PackageCode into g
                                                                  select g.Key).ToList();

                                    if (myGroupedList.Count == 1)
                                    {
                                        if (myGroupedList[0] == "DVMT")
                                        {
                                            isDevelopment = true;
                                        }
                                    }
                                }

                                else
                                {
                                    if (tenantManagement.PackageCode == "DVMT")
                                    {
                                        isDevelopment = true;
                                    }
                                }
                            }

                            if (!isDevelopment)
                            {
                                this.IsLimited = true;
                            }
                        }
                    }

                }

                if (isDemoTenant || isINTTRAOnlyDemo)
                {
                    this.IsDemoTenant = true;
                }

                scope.Complete();
            }
        }

        private void GetObjects_Tenant()
        {
            this.TenantObject = (from d in CommonContext.Tenants where d.Id == this.Tenant select d).FirstOrDefault();

            if (this.TenantObject.AddressId == null)
            {
                this.Errors.Add("Tenant Address is required");
            }

            else
            {
                this.TenantAddress = (from d in CommonContext.Addresses where d.Id == this.TenantObject.AddressId select d).FirstOrDefault();

                if (this.TenantAddress != null)
                {
                    if (string.IsNullOrEmpty(this.TenantAddress.Address1) && string.IsNullOrEmpty(this.TenantAddress.Address2))
                    {
                        this.Errors.Add("Tenant Address 1 or Address 2 is required");
                    }
                }
            }
        }
        private void GetObjects_Branch()
        {
            if (this.Shipment.BranchId == null)
            {
                this.Errors.Add("Branch is required");
            }

            else
            {
                this.Branch = (from d in CommonContext.Branches where d.Id == this.Shipment.BranchId select d).FirstOrDefault();

                if (this.Branch.INTTRAId == null)
                {
                    this.Errors.Add("Branch INTTRA ID is required");
                }

                if (this.Branch.INTTRAAlias == null)
                {
                    this.Errors.Add("Branch INTTRA Alias is required");
                }

                if (this.Branch.INTTRAContactId == null)
                {
                    this.Errors.Add("Branch INTTRA Contact is required");
                }

                else
                {
                    this.BranchContact = (from d in CommonContext.Contacts where d.Id == this.Branch.INTTRAContactId select d).FirstOrDefault();

                    if (this.BranchContact.Email == null)
                    {
                        this.Errors.Add("Branch INTTRA Contact Email is required");
                    }
                }
            }
        }
        private void GetObjects_MoveType()
        {
            if (!this.IsGroupageEntity)
            {
                if (this.Shipment.MoveTypeId == null)
                {
                    this.Errors.Add("Move type is required");
                }

                else
                {
                    IWebFreightContext webFreightContext = WebFreightContext.GetContext(this.Tenant);

                    this.MoveType = (from d in webFreightContext.MoveTypes where d.Id == this.Shipment.MoveTypeId select d).FirstOrDefault();
                    if (this.MoveType != null)
                    {
                        this.MoveType_Name = this.MoveType.MoveTypeEnglishName;

                        string myTranslatedCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(this.MoveType.Code, "G-INTTRA", "MoveType");
                        if (!string.IsNullOrEmpty(myTranslatedCode))
                        {
                            this.MoveType_Name = myTranslatedCode;
                        }

                        if (this.MasterData != null)
                        {
                            switch (this.MoveType_Name.ToLower())
                            {
                                case "doortodoor":
                                    {
                                        if (this.MasterData.PreCarriageFromPortId == null || this.MasterData.PreCarriageToPortId == null)
                                        {
                                            this.Errors.Add("Pre Carriage is required");
                                        }

                                        if (this.MasterData.OnCarriageFromPortId == null || this.MasterData.OnCarriageToPortId == null)
                                        {
                                            this.Errors.Add("On Carriage is required");
                                        }

                                        break;
                                    }

                                case "doortoport":
                                    {
                                        if (this.MasterData.PreCarriageFromPortId == null || this.MasterData.PreCarriageToPortId == null)
                                        {
                                            this.Errors.Add("Pre Carriage is required");
                                        }

                                        break;
                                    }

                                case "porttodoor":
                                    {
                                        if (this.MasterData.OnCarriageFromPortId == null || this.MasterData.OnCarriageToPortId == null)
                                        {
                                            this.Errors.Add("On Carriage is required");
                                        }

                                        break;
                                    }

                                case "porttoport":
                                    {
                                        break;
                                    }

                                default:
                                    {
                                        this.Errors.Add("Illegal value in move type");
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }
        private void GetObjects_INTTRASetting()
        {
            INTTRASetting iSetting = (from d in CommonContext.INTTRASettings where d.Tenant == this.Tenant select d).FirstOrDefault();
            if (iSetting != null)
            {
                this.INTTRA_Alias = iSetting.INTTRAAlias;
                this.INTTRA_OutSettingsId = iSetting.OutSettingsId;
            }

            if (string.IsNullOrEmpty(this.INTTRA_Alias) || string.IsNullOrEmpty(this.INTTRA_OutSettingsId))
            {
                iSetting = (from d in CommonContext.INTTRASettings where d.Tenant == 0 select d).FirstOrDefault();

                if (iSetting != null)
                {
                    if (string.IsNullOrEmpty(this.INTTRA_Alias))
                    {
                        this.INTTRA_Alias = iSetting.INTTRAAlias;
                    }

                    if (string.IsNullOrEmpty(this.INTTRA_OutSettingsId))
                    {
                        this.INTTRA_OutSettingsId = iSetting.OutSettingsId;
                    }
                }
            }

            if (string.IsNullOrEmpty(this.INTTRA_Alias))
            {
                this.Errors.Add("INTTRA Alias is required");
            }

            if (string.IsNullOrEmpty(this.INTTRA_OutSettingsId))
            {
                this.Errors.Add("Out Settings is required");
            }
        }
        private void GetObjects_ShipmentFields()
        {
            if (this.Shipment.ShipmentLevelCode == "H")
            {
                this.Errors.Add("Can't send house shipments");
            }

            bool isOceanFCL = false;
            if (this.Shipment.TransportModeId != null && this.Shipment.ShipmentTypeId != null)
            {
                if (this.Shipment.TransportModeId.ToUpper() == "O" && (this.Shipment.ShipmentTypeId.ToUpper() == "FCLD" || this.Shipment.ShipmentTypeId.ToUpper() == "MYGO"))
                {
                    isOceanFCL = true;
                }
            }

            if (!isOceanFCL)
            {
                this.Errors.Add("Only allowed for Ocean FCL shipments");
            }

            if (this.Shipment.ShipperId == null)
            {
                this.Errors.Add("Shipper is required");
            }

            if (this.Shipment.ConsigneeId == null)
            {
                this.Errors.Add("Consignee is required");
            }

            if (this.MasterData.BookingConfirmationNumber == null)
            {
                this.Errors.Add("Booking Confirmation Number is required");
            }

            if (this.Shipment.FreightPrepaidCollectId == null)
            {
                this.Errors.Add("Freight Prepaid Collect is required");
            }

            if (this.Shipment.BasicFreightId == null)
            {
                this.Errors.Add("Basic Freight is required");
            }

            if (this.MasterData.MainCarriageVesselName == null)
            {
                this.Errors.Add("Main Carriage Vessel is required");
            }

            if (this.Shipment.EmergencyContactId != null)
            {
                this.EmergencyContact = (from d in CommonContext.Contacts where d.Id == this.Shipment.EmergencyContactId select d).FirstOrDefault();

                if (string.IsNullOrEmpty(this.EmergencyContact.BusinessPhone))
                {
                    this.Errors.Add("Emergency Contact Phone is required");
                }
            }
        }
        private void GetObjects_ShipmentPorts()
        {
            if (this.MasterData.MainCarriageFromPortId == null)
            {
                this.Errors.Add("From Port is required");
            }

            else
            {
                this.FromPort = (from d in CommonContext.Ports where d.Id == this.MasterData.MainCarriageFromPortId select d).FirstOrDefault();
                this.FromPortCountry = (from d in CommonContext.Countries where d.Id == this.FromPort.CountryId select d).FirstOrDefault();
            }

            if (this.MasterData.MainCarriageFinalDestinationPortId == null)
            {
                this.Errors.Add("Final Destination Port is required");
            }

            else
            {
                this.FinalPort = (from d in CommonContext.Ports where d.Id == this.MasterData.MainCarriageFinalDestinationPortId select d).FirstOrDefault();
                this.FinalPortCountry = (from d in CommonContext.Countries where d.Id == this.FinalPort.CountryId select d).FirstOrDefault();
            }
        }
        private void GetObjects_ShipmentCarrier()
        {
            if (this.MasterData.MainCarriageCarrierId == null)
            {
                this.Errors.Add("Main Carriage Carrier is required");
            }

            else
            {
                this.MainShippingLine = (from d in CommonContext.ShippingLines where d.Id == this.MasterData.MainCarriageCarrierId select d).FirstOrDefault();

                if (this.MainShippingLine != null)
                {
                    this.IsCarrierRegisteredToINTTRA = this.MainShippingLine.IsINTTRARegistered;

                    if (this.IsCarrierRegisteredToINTTRA)
                    {
                        INTTRABranchRegisteredCarrier BranchRegisteredCarrier = (from a in CommonContext.INTTRABranchRegisteredCarriers
                                                                                 where a.Tenant == Tenant
                                                                                 && a.ShippingLineId == this.MasterData.MainCarriageCarrierId
                                                                                 && a.BranchId == this.Shipment.BranchId
                                                                                 select a).FirstOrDefault();

                        this.IsCarrierRegisteredToBranch = BranchRegisteredCarrier == null ? false : true;
                    }
                }
            }
        }
        private void GetObjects_ShipmentPackages()
        {
            this.ShipmentPackages = (from d in shipmentContext.ShipmentPackages
                                     where d.Tenant == this.Tenant
                                     && d.ShipmentId == this.ShipmentId
                                     select d).ToList();

            if (this.ShipmentPackages.Count == 0)
            {
                this.Errors.Add("Shipment Containers are required");
            }

            else if (this.ShipmentPackages.Where(d => string.IsNullOrEmpty(d.ContainerNumber)).Any())
            {
                this.Errors.Add("All Containers should have Container Number");
            }

            else if (this.ShipmentPackages.Where(d => d.Weight == null || d.Weight == 0).Any())
            {
                this.Errors.Add("All Containers should have Gross Weight");
            }

            else
            {
                if (this.ShipmentPackages.Where(d => d.IsDangerous).Any())
                {
                    if (string.IsNullOrEmpty(this.Shipment.EmergencyContactId))
                    {
                        string msg = TranslateTextsClass.Translate("Shipment.F.EmergencyContactId", this.Tenant) + " is required";
                        this.Errors.Add(msg);
                    }
                }

                List<string> ShipmentPackagesIds = this.ShipmentPackages.Select(s => s.Id).ToList();

                this.AllHarmonizes = (from d in shipmentContext.ShipmentPackageHarmonizes
                                      where d.Tenant == this.Tenant
                                      && string.IsNullOrEmpty(d.InsidePackageId)
                                      && ShipmentPackagesIds.Contains(d.PackageId)
                                      select d).ToList();

                this.InsidePackages = (from d in shipmentContext.InsideShipmentPackages
                                       where d.Tenant == this.Tenant
                                       && ShipmentPackagesIds.Contains(d.ShipmentPackageId)
                                       select d).ToList();

                List<string> InsideShipmentPackagesIds = this.InsidePackages.Select(s => s.Id).ToList();

                this.AllInsideHarmonizes = (from d in shipmentContext.ShipmentPackageHarmonizes
                                            where d.Tenant == this.Tenant
                                            && InsideShipmentPackagesIds.Contains(d.InsidePackageId)
                                            select d).ToList();

                bool allContainersHasInsides = true;

                foreach (ShipmentPackage item in this.ShipmentPackages)
                {
                    if (item.IsMultiHarmonize)
                    {
                        foreach (ShipmentPackageHarmonize itemHarmonize in this.AllHarmonizes)
                        {
                            if (itemHarmonize.Harmonize.Length > 35)
                            {
                                this.Errors.Add("Harmonize Field max length must be 35");
                            }
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(item.Harmonize))
                        {
                            if (item.Harmonize.Length > 35)
                            {
                                this.Errors.Add("Harmonize Field max length must be 35");
                            }
                        }
                    }

                    if (item.Temperature != null)
                    {
                        item.Temperature = item.Temperature.Trim();
                    }

                    if (!string.IsNullOrEmpty(item.Temperature))
                    {
                        if (!this.iNTTRAGeneralMethods.IsDecimalFormat(item.Temperature))
                        {
                            string msg = TranslateTextsClass.Translate("ShipmentPackage.F.Temperature", this.Tenant) + " invalid format";
                            this.Errors.Add(msg);
                        }
                    }

                    if (item.IsDangerous)
                    {
                        if (item.IMDGCode != null)
                        {
                            item.IMDGCode = item.IMDGCode.Trim();
                        }

                        if (item.FlashPoint != null)
                        {
                            item.FlashPoint = item.FlashPoint.Trim();
                        }

                        if (string.IsNullOrEmpty(item.IMDGCode))
                        {
                            string msg = TranslateTextsClass.Translate("ShipmentPackage.F.IMDGCode", this.Tenant) + " is required";
                            this.Errors.Add(msg);
                        }

                        if (!string.IsNullOrEmpty(item.FlashPoint))
                        {
                            if (!this.iNTTRAGeneralMethods.IsDecimalFormat(item.FlashPoint))
                            {
                                string msg = TranslateTextsClass.Translate("ShipmentPackage.F.FlashPoint", this.Tenant) + " invalid format";
                                this.Errors.Add(msg);
                            }
                        }
                    }

                    List<InsideShipmentPackage> itemInsidePackages = this.InsidePackages.Where(d => d.ShipmentPackageId == item.Id).ToList();
                    if (itemInsidePackages.Count == 0)
                    {
                        allContainersHasInsides = false;
                    }
                }

                foreach (InsideShipmentPackage item in this.InsidePackages)
                {
                    if (item.IsMultiHarmonize)
                    {
                        foreach (ShipmentPackageHarmonize itemHarmonize in this.AllInsideHarmonizes)
                        {
                            if (itemHarmonize.Harmonize.Length > 35)
                            {
                                this.Errors.Add("Harmonize Field max length must be 35");
                            }
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(item.Harmonize))
                        {
                            if (item.Harmonize.Length > 35)
                            {
                                this.Errors.Add("Harmonize Field max length must be 35");
                            }
                        }
                    }
                }

                if (!allContainersHasInsides)
                {
                    this.Errors.Add("All Containers should have inside Packages");
                }

                else if (this.InsidePackages.Where(d => string.IsNullOrEmpty(d.PackageTypeId)).Any())
                {
                    this.Errors.Add("All Inside packages should have Package Type");
                }

                else if (this.InsidePackages.Where(d => string.IsNullOrEmpty(d.Description)).Any())
                {
                    this.Errors.Add("All Inside packages should have Description");
                }

                // Validate the Computing Partner of Packages
                List<string> ids = this.ShipmentPackages.Select(s => s.PackageTypeId).ToList();
                var allPackageTypes = (from d in CommonContext.PackageTypes
                                       where d.Tenant == this.Tenant
                                       && ids.Contains(d.Id)
                                       select d).ToList();

                foreach (var item in allPackageTypes)
                {
                    string myTranslatedCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(item.Code, "G-INTTRA", "PackageType");
                    if (string.IsNullOrEmpty(myTranslatedCode))
                    {
                        this.Errors.Add("Package Type : " + item.EnglishName + " has no translation in the computing partner");
                    }
                }

                this.ValidateInsidePackages_ComputingPartnerTranslation();

            }
        }

        private void ValidateInsidePackages_ComputingPartnerTranslation()
        {
            // Validate the Computing Partner of Packages
            List<string> ids = this.InsidePackages.Select(s => s.PackageTypeId).ToList();
            var packageTypesOfsidePackages = (from d in CommonContext.PackageTypes
                                              where d.Tenant == this.Tenant
                                              && ids.Contains(d.Id)
                                              select d).ToList();
            foreach (var item in packageTypesOfsidePackages)
            {
                string myTranslatedCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(item.Code, "G-INTTRA", "PackageType");
                if (string.IsNullOrEmpty(myTranslatedCode))
                {
                    this.Errors.Add("Inside Package Type : " + item.EnglishName + " has no translation in the computing partner");
                }
            }
        }

        private Address AgentAddress;
        private Address ShipperAddress;
        private Address ConsigneeAddress;
        private Address Notify1Address;
        private Address Notify2Address;
        private Address FreightForwarderAddress;
        private Address FreightPayerAddress;
        private void GetObjects_Partners()
        {
            if (!string.IsNullOrEmpty(this.Shipment.ShipperAddressId))
            {
                this.ShipperAddress = (from d in CommonContext.Addresses where d.Id == this.Shipment.ShipperAddressId select d).FirstOrDefault();

                if (this.ShipperAddress != null)
                {
                    if (string.IsNullOrEmpty(this.ShipperAddress.Address1) && string.IsNullOrEmpty(this.ShipperAddress.Address2))
                    {
                        this.Errors.Add("Shipper Address 1 or Address 2 is required");
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.Shipment.ConsigneeAddressId))
            {
                this.ConsigneeAddress = (from d in CommonContext.Addresses where d.Id == this.Shipment.ConsigneeAddressId select d).FirstOrDefault();

                if (this.ConsigneeAddress != null)
                {
                    if (string.IsNullOrEmpty(this.ConsigneeAddress.Address1) && string.IsNullOrEmpty(this.ConsigneeAddress.Address2))
                    {
                        this.Errors.Add("Consignee Address 1 or Address 2 is required");
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.Shipment.Notify1AddressId))
            {
                this.Notify1Address = (from d in CommonContext.Addresses where d.Id == this.Shipment.Notify1AddressId select d).FirstOrDefault();

                if (this.Notify1Address != null)
                {
                    if (string.IsNullOrEmpty(this.Notify1Address.Address1) && string.IsNullOrEmpty(this.Notify1Address.Address2))
                    {
                        this.Errors.Add("Notify1 Address 1 or Address 2 is required");
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.Shipment.Notify2AddressId))
            {
                this.Notify2Address = (from d in CommonContext.Addresses where d.Id == this.Shipment.Notify2AddressId select d).FirstOrDefault();

                if (this.Notify2Address != null)
                {
                    if (string.IsNullOrEmpty(this.Notify2Address.Address1) && string.IsNullOrEmpty(this.Notify2Address.Address2))
                    {
                        this.Errors.Add("Notify2 Address 1 or Address 2 is required");
                    }
                }
            }


            if (!string.IsNullOrEmpty(this.Shipment.FreightForwarderAddressId))
            {
                this.FreightForwarderAddress = (from d in CommonContext.Addresses where d.Id == this.Shipment.FreightForwarderAddressId select d).FirstOrDefault();

                if (this.FreightForwarderAddress != null)
                {
                    if (string.IsNullOrEmpty(this.FreightForwarderAddress.Address1) && string.IsNullOrEmpty(this.FreightForwarderAddress.Address2))
                    {
                        this.Errors.Add("Freight Forwarder Address 1 or Address 2 is required");
                    }
                }
            }


            if (this.Shipment.FreightPrepaidCollectId != null)
            {
                if (this.Shipment.FreightPrepaidCollectId.ToUpper() == "C")
                {
                    if (this.Shipment.AgentId != null)
                    {
                        if (!string.IsNullOrEmpty(this.Shipment.AgentAddressId))
                        {
                            this.AgentAddress = (from d in CommonContext.Addresses where d.Id == this.Shipment.AgentAddressId select d).FirstOrDefault();

                            if (this.AgentAddress != null)
                            {
                                if (string.IsNullOrEmpty(this.AgentAddress.Address1) && string.IsNullOrEmpty(this.AgentAddress.Address2))
                                {
                                    this.Errors.Add("Agent Address 1 or Address 2 is required");
                                }
                            }
                        }
                    }
                }
            }

            if (this.Shipment.FreightPayerId != null)
            {
                if (string.IsNullOrEmpty(this.Shipment.FreightPayerAddressId))
                {
                    this.Errors.Add("Freight Payer Address required");
                }

                else
                {
                    this.FreightPayerAddress = (from d in CommonContext.Addresses where d.Id == this.Shipment.FreightPayerAddressId select d).FirstOrDefault();

                    if (this.FreightPayerAddress != null)
                    {
                        if (string.IsNullOrEmpty(this.FreightPayerAddress.Address1) && string.IsNullOrEmpty(this.FreightPayerAddress.Address2))
                        {
                            this.Errors.Add("Freight Payer Address 1 or Address 2 is required");
                        }
                    }
                }
            }
        }

        public System.DateTime TodayDate { get; set; }
        public System.DateTime TodayDateTime { get; set; }
        public string ShipmentNumber { get; set; }
        public string VolumeUnitCode { get; set; }
        public string GrossWeightUnitCode { get; set; }
        public long XMLCreateDate { get; set; }
        public long XMLCreateDate_Long { get; set; }
        private void GetProperties()
        {
            this.TodayDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant).Date;
            this.TodayDateTime = TenantServerConfigration.GetCurrentDateTime(this.Tenant);
            this.XMLCreateDate = this.iNTTRAGeneralMethods.GetDateShortFormat(this.TodayDateTime);
            this.XMLCreateDate_Long = this.iNTTRAGeneralMethods.GetDateLongFormat(this.TodayDateTime);

            this.ShipmentNumber = this.Shipment.ShipmentNumber;
            this.VolumeUnitCode = this.Shipment.VolumeUnitCode.ToUpper();
            this.GrossWeightUnitCode = this.Shipment.GrossWeightUnitCode.ToUpper();
        }

        public string CommunicationLogIdCounter { get; set; }
        public void Build()
        {
            this.CommunicationLogIdCounter = IdCounter.GetNumber("CommunicationLog", Tenant);
            this.BuildMessageHeader();
            this.BuildMessageProperties();
            this.BuildMessageDetails();
            //this.HandelOneHouse();

        }

        // Message Header
        public INTTRA_Out.PartnerInformation Sender;
        public INTTRA_Out.PartnerInformation Recipient;
        public List<INTTRA_Out.PartnerInformation> MessageHeaderParties;
        private void BuildMessageHeader()
        {
            this.BuildMessageHeader_Sender();
            this.BuildMessageHeader_Recipient();

            this.MessageHeaderParties = new List<INTTRA_Out.PartnerInformation>();
            this.MessageHeaderParties.Add(this.Sender);
            this.MessageHeaderParties.Add(this.Recipient);
        }
        private void BuildMessageHeader_Sender()
        {
            this.Sender = new INTTRA_Out.PartnerInformation()
            {
                PartnerRole = INTTRA_Out.PartnerInformationPartnerRole.Sender,
                PartnerName = this.iNTTRAGeneralMethods.GetStringList(this.TenantObject.Company, 2, 35).ToArray<string>(),

                PartnerIdentifier = new INTTRA_Out.PartnerIdentifier()
                {
                    Agency = INTTRA_Out.PartnerIdentifierAgency.AssignedBySender,
                    Value = this.INTTRA_Alias,
                },
            };

            if (this.TenantAddress != null)
            {
                this.Sender.AddressInformation = this.GetAddressInformation(this.TenantAddress);
            }

            if (this.BranchContact != null)
            {
                this.Sender.ContactInformation = this.GetContactInformation(this.BranchContact);
            }
        }
        private void BuildMessageHeader_Recipient()
        {
            string RecipientINTTRA_Id = "INTTRA";

            this.Recipient = new INTTRA_Out.PartnerInformation()
            {
                PartnerRole = INTTRA_Out.PartnerInformationPartnerRole.Recipient,

                PartnerIdentifier = new INTTRA_Out.PartnerIdentifier()
                {
                    Agency = INTTRA_Out.PartnerIdentifierAgency.AssignedByRecipient,
                    Value = RecipientINTTRA_Id,
                },
            };
        }

        // Message Properties
        private Card Shipper;
        public INTTRA_Out.HaulageDetails HaulageDetails;
        public List<INTTRA_Out.ChargeCategory> ChargeCategories;
        public List<INTTRA_Out.Location> BlLocations;
        public List<INTTRA_Out.ReferenceInformation> ReferenceInformations;
        public List<INTTRA_Out.ShipmentComments> Instructions;
        public INTTRA_Out.TransportationDetails TransportationDetails;
        public List<INTTRA_Out.PartnerInformation> MessagePropertiesParties;
        public INTTRA_Out.ShipmentIndicator ShipmentIndicator;
        public List<INTTRA_Out.HeaderCustomsFilerInstruction> HeaderCustomsInformation;

        private void BuildMessageProperties()
        {
            this.BuildMessageProperties_HaulageDetails();
            this.BuildMessageProperties_ChargeCategories();
            this.BuildMessageProperties_BlLocations();
            this.BuildMessageProperties_ReferenceInformations();
            this.BuildMessageProperties_Instructions();
            this.BuildMessageProperties_TransportationDetails();
            this.BuildMessageProperties_Parties();
        }
        private void BuildMessageProperties_HaulageDetails()
        {
            if (this.IsGroupageEntity)
            {
                this.HaulageDetails = new INTTRA_Out.HaulageDetails
                {
                    MovementType = INTTRA_Out.HaulageDetailsMovementType.PortToPort,
                    ServiceType = INTTRA_Out.HaulageDetailsServiceType.FullLoad,
                };

            }

            else if (this.MoveType != null)
            {
                this.HaulageDetails = new INTTRA_Out.HaulageDetails
                {
                    //MovementType = INTTRA.HaulageDetailsMovementType.PortToPort,
                    ServiceType = INTTRA_Out.HaulageDetailsServiceType.FullLoad,
                };

                switch (this.MoveType_Name.ToLower())
                {
                    case "doortodoor":
                        {
                            this.HaulageDetails.MovementType = INTTRA_Out.HaulageDetailsMovementType.DoorToDoor;
                            break;
                        }

                    case "doortoport":
                        {
                            this.HaulageDetails.MovementType = INTTRA_Out.HaulageDetailsMovementType.DoorToPort;
                            break;
                        }

                    case "porttoport":
                        {
                            this.HaulageDetails.MovementType = INTTRA_Out.HaulageDetailsMovementType.PortToPort;
                            break;
                        }

                    case "porttodoor":
                        {
                            this.HaulageDetails.MovementType = INTTRA_Out.HaulageDetailsMovementType.PortToDoor;
                            break;
                        }
                }
            }
        }
        private void BuildMessageProperties_ChargeCategories()
        {
            this.ChargeCategories = new List<INTTRA_Out.ChargeCategory>();

            //if (this.Shipment.FreightPrepaidCollectId != null)
            //{
            //    INTTRA_Out.ChargeCategory item = new INTTRA_Out.ChargeCategory()
            //    {
            //        ChargeType = INTTRA_Out.ChargeCategoryChargeType.BasicFreight,
            //        PrepaidorCollectIndicator = this.Shipment.FreightPrepaidCollectId.ToUpper() == "P" ? INTTRA_Out.ChargeCategoryPrepaidorCollectIndicator.Prepaid : INTTRA_Out.ChargeCategoryPrepaidorCollectIndicator.Collect,
            //    };

            //    this.ChargeCategories.Add(item);
            //}


            if (this.Shipment.BasicFreightId != null)
            {
                INTTRA_Out.ChargeCategory item = new INTTRA_Out.ChargeCategory()
                {
                    ChargeType = INTTRA_Out.ChargeCategoryChargeType.BasicFreight,
                    PrepaidorCollectIndicator = this.Shipment.BasicFreightId.ToUpper() == "P" ? INTTRA_Out.ChargeCategoryPrepaidorCollectIndicator.Prepaid : INTTRA_Out.ChargeCategoryPrepaidorCollectIndicator.Collect,
                };

                this.ChargeCategories.Add(item);
            }

            if (this.Shipment.DestinationPortChargesId != null)
            {
                INTTRA_Out.ChargeCategory item = new INTTRA_Out.ChargeCategory()
                {
                    ChargeType = INTTRA_Out.ChargeCategoryChargeType.DestinationPortCharges,
                    PrepaidorCollectIndicator = this.Shipment.DestinationPortChargesId.ToUpper() == "P" ? INTTRA_Out.ChargeCategoryPrepaidorCollectIndicator.Prepaid : INTTRA_Out.ChargeCategoryPrepaidorCollectIndicator.Collect,
                };

                this.ChargeCategories.Add(item);
            }

            if (this.Shipment.DestinationHaulageChargesId != null)
            {
                INTTRA_Out.ChargeCategory item = new INTTRA_Out.ChargeCategory()
                {
                    ChargeType = INTTRA_Out.ChargeCategoryChargeType.DestinationHaulageCharges,
                    PrepaidorCollectIndicator = this.Shipment.DestinationHaulageChargesId.ToUpper() == "P" ? INTTRA_Out.ChargeCategoryPrepaidorCollectIndicator.Prepaid : INTTRA_Out.ChargeCategoryPrepaidorCollectIndicator.Collect,
                };

                this.ChargeCategories.Add(item);
            }

            if (this.Shipment.AdditionalChargesId != null)
            {
                INTTRA_Out.ChargeCategory item = new INTTRA_Out.ChargeCategory()
                {
                    ChargeType = INTTRA_Out.ChargeCategoryChargeType.AdditionalCharges,
                    PrepaidorCollectIndicator = this.Shipment.AdditionalChargesId.ToUpper() == "P" ? INTTRA_Out.ChargeCategoryPrepaidorCollectIndicator.Prepaid : INTTRA_Out.ChargeCategoryPrepaidorCollectIndicator.Collect,
                };

                this.ChargeCategories.Add(item);
            }
        }
        private void BuildMessageProperties_BlLocations()
        {
            this.BlLocations = new List<INTTRA_Out.Location>();

            INTTRA_Out.Location item_BillOfLadingRelease = new INTTRA_Out.Location()
            {
                LocationType = INTTRA_Out.LocationLocationType.BillOfLadingRelease,

                LocationCode = new INTTRA_Out.LocationCode()
                {
                    Agency = "UN",
                    Value = this.FromPortCountry.Code.ToUpper() + this.FromPort.Code.ToUpper(),
                },

                LocationName = this.iNTTRAGeneralMethods.FormatString(this.FromPort.EnglishName, 256),

                LocationCountry = this.FromPortCountry.Code.ToUpper(),

                DateTime = new INTTRA_Out.DateTime()
                {
                    DateType = INTTRA_Out.DateTimeDateType.BlReleaseDate,
                    Value = this.XMLCreateDate_Long,
                },
            };

            this.BlLocations.Add(item_BillOfLadingRelease);


            if (this.Shipment.FreightPrepaidCollectId != null)
            {
                switch (this.Shipment.FreightPrepaidCollectId.ToUpper())
                {
                    case "P":
                        {
                            INTTRA_Out.Location item_FreightPaymentLocation = new INTTRA_Out.Location()
                            {
                                LocationType = INTTRA_Out.LocationLocationType.FreightPaymentLocation,

                                LocationCode = new INTTRA_Out.LocationCode()
                                {
                                    Agency = "UN",
                                    Value = this.FromPortCountry.Code.ToUpper() + this.FromPort.Code.ToUpper(),
                                },

                                LocationName = this.iNTTRAGeneralMethods.FormatString(this.FromPort.EnglishName, 256),

                                //LocationCountry = this.FromPortCountry.Code.ToUpper(),

                                //DateTime = new DateTime()
                                //{
                                //    DateType = DateTimeDateType.BlReleaseDate,
                                //    Value = this.XMLCreateDate_Long,
                                //},
                            };

                            this.BlLocations.Add(item_FreightPaymentLocation);

                            break;
                        }


                    case "C":
                        {
                            INTTRA_Out.Location item_FreightPaymentLocation = new INTTRA_Out.Location()
                            {
                                LocationType = INTTRA_Out.LocationLocationType.FreightPaymentLocation,

                                LocationCode = new INTTRA_Out.LocationCode()
                                {
                                    Agency = "UN",
                                    Value = this.FinalPortCountry.Code.ToUpper() + this.FinalPort.Code.ToUpper(),
                                },

                                LocationName = this.iNTTRAGeneralMethods.FormatString(this.FinalPort.EnglishName, 256),

                                //LocationCountry = this.FinalPortCountry.Code.ToUpper(),

                                //DateTime = new DateTime()
                                //{
                                //    DateType = DateTimeDateType.BlReleaseDate,
                                //    Value = this.XMLCreateDate_Long,
                                //},
                            };

                            this.BlLocations.Add(item_FreightPaymentLocation);

                            break;
                        }
                }
            }
        }
        private void BuildMessageProperties_ReferenceInformations()
        {
            this.ReferenceInformations = new List<INTTRA_Out.ReferenceInformation>();

            this.ReferenceInformations.Add(new INTTRA_Out.ReferenceInformation()
            {
                ReferenceType = INTTRA_Out.ReferenceInformationReferenceType.BookingNumber,
                Value = this.iNTTRAGeneralMethods.FormatString(this.MasterData.BookingConfirmationNumber, 99),
            });

            if (!string.IsNullOrEmpty(this.Shipment.ShipperReference1))
            {
                this.ReferenceInformations.Add(new INTTRA_Out.ReferenceInformation()
                {
                    ReferenceType = INTTRA_Out.ReferenceInformationReferenceType.InvoiceNumber,
                    Value = this.iNTTRAGeneralMethods.FormatString(this.Shipment.ShipperReference1, 99),
                });
            }

            if (!string.IsNullOrEmpty(this.Shipment.ShipperReference2))
            {
                this.ReferenceInformations.Add(new INTTRA_Out.ReferenceInformation()
                {
                    ReferenceType = INTTRA_Out.ReferenceInformationReferenceType.InvoiceNumber,
                    Value = this.iNTTRAGeneralMethods.FormatString(this.Shipment.ShipperReference2, 99),
                });
            }

            if (!string.IsNullOrEmpty(this.Shipment.ShipmentNumber))
            {
                this.ReferenceInformations.Add(new INTTRA_Out.ReferenceInformation()
                {
                    ReferenceType = INTTRA_Out.ReferenceInformationReferenceType.FreightForwarderReference,
                    Value = this.iNTTRAGeneralMethods.FormatString(this.Shipment.ShipmentNumber, 99),
                });
            }

            if (this.MasterData != null)
            {
                if (!string.IsNullOrEmpty(this.MasterData.Master))
                {
                    this.ReferenceInformations.Add(new INTTRA_Out.ReferenceInformation()
                    {
                        ReferenceType = INTTRA_Out.ReferenceInformationReferenceType.BillOfLadingNumber,
                        Value = this.iNTTRAGeneralMethods.FormatString(this.MasterData.Master, 99),
                    });
                }
            }

            if (!string.IsNullOrEmpty(this.Shipment.INTTRAContractNumber))
            {
                this.ReferenceInformations.Add(new INTTRA_Out.ReferenceInformation()
                {
                    ReferenceType = INTTRA_Out.ReferenceInformationReferenceType.ContractNumber,
                    Value = this.iNTTRAGeneralMethods.FormatString(this.Shipment.INTTRAContractNumber, 99),
                });
            }
        }
        private void BuildMessageProperties_Instructions()
        {
            this.Instructions = new List<INTTRA_Out.ShipmentComments>();

            if (!string.IsNullOrEmpty(this.Shipment.INTTRAInstructions))
            {
                string myString = this.Shipment.INTTRAInstructions;

                List<string> list = this.iNTTRAGeneralMethods.GetStringList(myString, 99, 35);

                foreach (string item in list)
                {
                    INTTRA_Out.ShipmentComments itemComment = new INTTRA_Out.ShipmentComments()
                    {
                        CommentType = INTTRA_Out.ShipmentCommentsCommentType.BlClause,
                        Value = item,
                    };

                    this.Instructions.Add(itemComment);
                }
            }

            if (!string.IsNullOrEmpty(this.Shipment.INTTRAComments))
            {
                string myString = this.Shipment.INTTRAComments;

                List<string> list = this.iNTTRAGeneralMethods.GetStringList(myString, 99, 35);

                foreach (string item in list)
                {
                    if (this.Instructions.Count < 99)
                    {
                        INTTRA_Out.ShipmentComments itemComment = new INTTRA_Out.ShipmentComments()
                        {
                            CommentType = INTTRA_Out.ShipmentCommentsCommentType.General,
                            Value = item,
                        };

                        this.Instructions.Add(itemComment);
                    }
                }
            }
        }
        private void BuildMessageProperties_TransportationDetails()
        {
            this.TransportationDetails = new INTTRA_Out.TransportationDetails()
            {
                TransportStage = INTTRA_Out.TransportationDetailsTransportStage.Main,
                TransportMode = INTTRA_Out.TransportationDetailsTransportMode.Maritime,

                ConveyanceInformation = new INTTRA_Out.ConveyanceInformation()
                {
                    ConveyanceName = this.iNTTRAGeneralMethods.FormatString(this.MasterData.MainCarriageVesselName, 35),

                    //TransportIdentification = new TransportIdentification()
                    //{
                    //    TransportIdentificationType = TransportIdentificationTransportIdentificationType.LloydsCode,
                    //    Value = "AYMAN",
                    //},                    
                },
            };

            if (this.MasterData.MainCarriageCarrierNumber != null)
            {
                this.TransportationDetails.ConveyanceInformation.VoyageTripNumber = this.iNTTRAGeneralMethods.FormatString(this.MasterData.MainCarriageCarrierNumber, 35);
            }

            if (this.MainShippingLine != null)
            {
                if (this.MainShippingLine.SCACCode != null)
                {
                    this.TransportationDetails.ConveyanceInformation.CarrierSCAC = this.iNTTRAGeneralMethods.FormatString(this.MainShippingLine.SCACCode, 35);
                }
            }

            List<INTTRA_Out.Location> locations = new List<INTTRA_Out.Location>();

            // From
            locations.Add(new INTTRA_Out.Location()
            {
                LocationType = INTTRA_Out.LocationLocationType.PortOfLoading,

                LocationCode = new INTTRA_Out.LocationCode()
                {
                    Agency = "UN",
                    Value = this.FromPortCountry.Code.ToUpper() + this.FromPort.Code.ToUpper(),
                },

                LocationName = this.iNTTRAGeneralMethods.FormatString(this.FromPort.EnglishName, 256),

                LocationCountry = this.FromPortCountry.Code.ToUpper(),
            });

            // Final
            locations.Add(new INTTRA_Out.Location()
            {
                LocationType = INTTRA_Out.LocationLocationType.PortOfDischarge,

                LocationCode = new INTTRA_Out.LocationCode()
                {
                    Agency = "UN",
                    Value = this.FinalPortCountry.Code.ToUpper() + this.FinalPort.Code.ToUpper(),
                },

                LocationName = this.iNTTRAGeneralMethods.FormatString(this.FinalPort.EnglishName, 256),

                LocationCountry = this.FinalPortCountry.Code.ToUpper(),
            });

            #region PlaceOfReceipt
            if (this.MasterData != null && this.MasterData.PreCarriageFromPortId != null)
            {
                Port PreCarriagePort = (from d in CommonContext.Ports where d.Id == this.MasterData.PreCarriageFromPortId select d).FirstOrDefault();
                Country PreCarriageCountry = (from d in CommonContext.Countries where d.Id == PreCarriagePort.CountryId select d).FirstOrDefault();

                locations.Add(new INTTRA_Out.Location()
                {
                    LocationType = INTTRA_Out.LocationLocationType.PlaceOfReceipt,

                    LocationCode = new INTTRA_Out.LocationCode()
                    {
                        Agency = "UN",
                        Value = PreCarriageCountry.Code.ToUpper() + PreCarriagePort.Code.ToUpper(),
                    },

                    LocationName = this.iNTTRAGeneralMethods.FormatString(PreCarriagePort.EnglishName, 256),

                    LocationCountry = PreCarriageCountry.Code.ToUpper(),
                });
            }

            //else
            //{
            //    locations.Add(new INTTRA_Out.Location()
            //    {
            //        LocationType = INTTRA_Out.LocationLocationType.PlaceOfReceipt,

            //        LocationCode = new INTTRA_Out.LocationCode()
            //        {
            //            Agency = "UN",
            //            Value = this.FromPortCountry.Code.ToUpper() + this.FromPort.Code.ToUpper(),
            //        },

            //        LocationName = this.iNTTRAGeneralMethods.FormatString(this.FromPort.EnglishName, 256),

            //        LocationCountry = this.FromPortCountry.Code.ToUpper(),
            //    });
            //}
            #endregion

            #region PlaceOfDelivery
            if (this.MasterData != null && this.MasterData.OnCarriageToPortId != null)
            {
                Port OnCarriagePort = (from d in CommonContext.Ports where d.Id == this.MasterData.OnCarriageToPortId select d).FirstOrDefault();
                Country OnCarriageCountry = (from d in CommonContext.Countries where d.Id == OnCarriagePort.CountryId select d).FirstOrDefault();

                locations.Add(new INTTRA_Out.Location()
                {
                    LocationType = INTTRA_Out.LocationLocationType.PlaceOfDelivery,

                    LocationCode = new INTTRA_Out.LocationCode()
                    {
                        Agency = "UN",
                        Value = OnCarriageCountry.Code.ToUpper() + OnCarriagePort.Code.ToUpper(),
                    },

                    LocationName = this.iNTTRAGeneralMethods.FormatString(OnCarriagePort.EnglishName, 256),

                    LocationCountry = OnCarriageCountry.Code.ToUpper(),
                });
            }

            //else
            //{
            //    locations.Add(new INTTRA_Out.Location()
            //    {
            //        LocationType = INTTRA_Out.LocationLocationType.PlaceOfDelivery,

            //        LocationCode = new INTTRA_Out.LocationCode()
            //        {
            //            Agency = "UN",
            //            Value = this.FinalPortCountry.Code.ToUpper() + this.FinalPort.Code.ToUpper(),
            //        },

            //        LocationName = this.iNTTRAGeneralMethods.FormatString(this.FinalPort.EnglishName, 256),

            //        LocationCountry = this.FinalPortCountry.Code.ToUpper(),
            //    });
            //}
            #endregion

            this.TransportationDetails.Location = locations.ToArray<INTTRA_Out.Location>();
        }
        private void BuildMessageProperties_Parties()
        {
            this.MessagePropertiesParties = new List<INTTRA_Out.PartnerInformation>();

            #region Requestor
            if (this.TenantObject != null)
            {
                INTTRA_Out.PartnerInformation item = new INTTRA_Out.PartnerInformation()
                {
                    PartnerRole = INTTRA_Out.PartnerInformationPartnerRole.Requestor,
                    PartnerName = this.iNTTRAGeneralMethods.GetStringList(this.TenantObject.Company, 2, 35).ToArray<string>(),

                    PartnerIdentifier = new INTTRA_Out.PartnerIdentifier()
                    {
                        Agency = INTTRA_Out.PartnerIdentifierAgency.AssignedBySender,
                        Value = this.Branch.INTTRAAlias,
                    },
                };

                if (this.TenantAddress != null)
                {
                    item.AddressInformation = this.GetAddressInformation(this.TenantAddress);
                }

                if (this.Shipment.INTTRADocumentTypeCode != null)
                {
                    INTTRADocumentType myDocumentType = shipmentContext.INTTRADocumentTypes.Where(d => d.Code == this.Shipment.INTTRADocumentTypeCode).FirstOrDefault();
                    if (myDocumentType != null)
                    {
                        List<INTTRA_Out.DocumentationRequirements> list = new List<INTTRA_Out.DocumentationRequirements>();

                        INTTRA_Out.DocumentationRequirements listItem = new INTTRA_Out.DocumentationRequirements()
                        {
                            Documents = new INTTRA_Out.Documents()
                            {
                                Freighted = this.Shipment.INTTRAIsFreighted ? INTTRA_Out.DocumentsFreighted.True : INTTRA_Out.DocumentsFreighted.False,
                            },
                        };

                        if (this.Shipment.INTTRADocumentQTY != null)
                        {
                            listItem.Quantity = this.Shipment.INTTRADocumentQTY.ToString();
                        }

                        switch (myDocumentType.Code)
                        {
                            case "BILL":
                                {
                                    listItem.Documents.DocumentType = INTTRA_Out.DocumentsDocumentType.SeaWaybill;
                                    break;
                                }

                            case "COPY":
                                {
                                    listItem.Documents.DocumentType = INTTRA_Out.DocumentsDocumentType.BillOfLadingCopy;
                                    break;
                                }

                            case "LADN":
                                {
                                    listItem.Documents.DocumentType = INTTRA_Out.DocumentsDocumentType.HouseBillOfLading;
                                    break;
                                }

                            case "ORIG":
                                {
                                    listItem.Documents.DocumentType = INTTRA_Out.DocumentsDocumentType.BillOfLadingOriginal;
                                    break;
                                }
                        }

                        list.Add(listItem);

                        item.DocumentationRequirements = list.ToArray<INTTRA_Out.DocumentationRequirements>();
                    }
                }

                this.MessagePropertiesParties.Add(item);
            }
            #endregion

            #region Shipper
            if (this.Shipment.ShipperId != null)
            {
                this.Shipper = (from d in CommonContext.Cards where d.Id == this.Shipment.ShipperId select d).FirstOrDefault();

                if (this.Shipper != null)
                {
                    Contact shipperContact = this.contactRepository.GetSingleContact(this.Shipment.ShipperContactId,this.Tenant);

                    INTTRA_Out.PartnerInformation item = new INTTRA_Out.PartnerInformation()
                    {
                        PartnerRole = INTTRA_Out.PartnerInformationPartnerRole.Shipper,
                        PartnerName = this.iNTTRAGeneralMethods.GetStringList(this.Shipper.EnglishName, 2, 35).ToArray<string>(),
                        ContactInformation = shipperContact != null ? this.GetContactInformation(shipperContact) : null,
                    };

                    if (this.ShipperAddress != null)
                    {
                        item.AddressInformation = this.GetAddressInformation(this.ShipperAddress);
                    }

                    this.MessagePropertiesParties.Add(item);
                }
            }
            #endregion

            #region Consignee
            if (this.Shipment.ConsigneeId != null)
            {
                Card myCard = (from d in CommonContext.Cards where d.Id == this.Shipment.ConsigneeId select d).FirstOrDefault();
                if (myCard != null)
                {
                    Contact consigneeContact = this.contactRepository.GetSingleContact(this.Shipment.ConsigneeContactId, this.Tenant);

                    INTTRA_Out.PartnerInformation item = new INTTRA_Out.PartnerInformation()
                    {
                        PartnerRole = INTTRA_Out.PartnerInformationPartnerRole.Consignee,
                        PartnerName = this.iNTTRAGeneralMethods.GetStringList(myCard.EnglishName, 2, 35).ToArray<string>(),
                        ContactInformation = consigneeContact != null ? this.GetContactInformation(consigneeContact) : null,
                    };

                    if (this.ConsigneeAddress != null)
                    {
                        item.AddressInformation = this.GetAddressInformation(this.ConsigneeAddress);
                    }

                    this.MessagePropertiesParties.Add(item);
                }
            }
            #endregion

            #region Carrier
            if (this.MasterData != null && this.MasterData.MainCarriageCarrierId != null)
            {
                Card myCard = (from d in CommonContext.Cards where d.Id == this.MasterData.MainCarriageCarrierId select d).FirstOrDefault();
                if (myCard != null)
                {
                    INTTRA_Out.PartnerInformation item = new INTTRA_Out.PartnerInformation()
                    {
                        PartnerRole = INTTRA_Out.PartnerInformationPartnerRole.Carrier,
                        PartnerName = this.iNTTRAGeneralMethods.GetStringList(myCard.EnglishName, 2, 35).ToArray<string>(),
                    };

                    if (this.MainShippingLine != null)
                    {
                        if (this.MainShippingLine.SCACCode != null)
                        {
                            item.PartnerIdentifier = new INTTRA_Out.PartnerIdentifier()
                            {
                                Agency = INTTRA_Out.PartnerIdentifierAgency.AssignedBySender,
                                Value = this.MainShippingLine.SCACCode,
                            };
                        }
                    }

                    this.MessagePropertiesParties.Add(item);
                }
            }
            #endregion

            #region Notify
            if (this.Shipment.Notify1Id != null || this.Shipment.Notify2Id != null)
            {
                if (this.Shipment.Notify1Id != null)
                {
                    Card myCard = (from d in CommonContext.Cards where d.Id == this.Shipment.Notify1Id select d).FirstOrDefault();
                    if (myCard != null)
                    {
                        INTTRA_Out.PartnerInformation item = new INTTRA_Out.PartnerInformation()
                        {
                            PartnerRole = INTTRA_Out.PartnerInformationPartnerRole.NotifyParty,
                            PartnerName = this.iNTTRAGeneralMethods.GetStringList(myCard.EnglishName, 2, 35).ToArray<string>(),
                        };

                        if (this.Notify1Address != null)
                        {
                            item.AddressInformation = this.GetAddressInformation(this.Notify1Address);
                        }

                        this.MessagePropertiesParties.Add(item);
                    }
                }

                if (this.Shipment.Notify2Id != null)
                {
                    Card myCard = (from d in CommonContext.Cards where d.Id == this.Shipment.Notify2Id select d).FirstOrDefault();
                    if (myCard != null)
                    {
                        INTTRA_Out.PartnerInformation item = new INTTRA_Out.PartnerInformation()
                        {
                            PartnerRole = INTTRA_Out.PartnerInformationPartnerRole.NotifyParty1,
                            PartnerName = this.iNTTRAGeneralMethods.GetStringList(myCard.EnglishName, 2, 35).ToArray<string>(),
                        };

                        if (this.Notify2Address != null)
                        {
                            item.AddressInformation = this.GetAddressInformation(this.Notify2Address);
                        }

                        this.MessagePropertiesParties.Add(item);
                    }
                }
            }

            else if (this.Shipment.ConsigneeId != null)
            {
                Card myCard = (from d in CommonContext.Cards where d.Id == this.Shipment.ConsigneeId select d).FirstOrDefault();
                if (myCard != null)
                {
                    INTTRA_Out.PartnerInformation item = new INTTRA_Out.PartnerInformation()
                    {
                        PartnerRole = INTTRA_Out.PartnerInformationPartnerRole.NotifyParty,
                        PartnerName = this.iNTTRAGeneralMethods.GetStringList(myCard.EnglishName, 2, 35).ToArray<string>(),
                    };

                    if (this.ConsigneeAddress != null)
                    {
                        item.AddressInformation = this.GetAddressInformation(this.ConsigneeAddress);
                    }

                    this.MessagePropertiesParties.Add(item);
                }
            }
            #endregion

            #region FreightPayer
            if (this.Shipment.FreightPayerId != null)
            {
                Card myCard = (from d in CommonContext.Cards where d.Id == this.Shipment.FreightPayerId select d).FirstOrDefault();
                if (myCard != null)
                {
                    INTTRA_Out.PartnerInformation item = new INTTRA_Out.PartnerInformation()
                    {
                        PartnerRole = INTTRA_Out.PartnerInformationPartnerRole.FreightPayer,
                        PartnerName = this.iNTTRAGeneralMethods.GetStringList(myCard.EnglishName, 2, 35).ToArray<string>(),
                    };

                    Address FreightPayerAddress = (from d in CommonContext.Addresses where d.Id == this.Shipment.FreightPayerAddressId select d).FirstOrDefault();

                    if (FreightPayerAddress != null)
                    {
                        item.AddressInformation = this.GetAddressInformation(FreightPayerAddress);
                    }

                    this.MessagePropertiesParties.Add(item);
                }
            }

            //if (this.Shipment.FreightPrepaidCollectId != null)
            //{
            //    if (this.Shipment.FreightPrepaidCollectId.ToUpper() == "P")
            //    {
            //        if (this.Shipper != null)
            //        {
            //            INTTRA_Out.PartnerInformation item = new INTTRA_Out.PartnerInformation()
            //            {
            //                PartnerRole = INTTRA_Out.PartnerInformationPartnerRole.FreightPayer,
            //                PartnerName = this.iNTTRAGeneralMethods.GetStringList(this.Shipper.EnglishName, 2, 35).ToArray<string>(),
            //            };

            //            if (this.ShipperAddress != null)
            //            {
            //                item.AddressInformation = this.GetAddressInformation(this.ShipperAddress);
            //            }

            //            this.MessagePropertiesParties.Add(item);
            //        }
            //    }

            //    else if (this.Shipment.FreightPrepaidCollectId.ToUpper() == "C")
            //    {
            //        if (this.Shipment.AgentId != null)
            //        {
            //            Card myCard = (from d in CommonContext.Cards where d.Id == this.Shipment.AgentId select d).FirstOrDefault();
            //            if (myCard != null)
            //            {
            //                INTTRA_Out.PartnerInformation item = new INTTRA_Out.PartnerInformation()
            //                {
            //                    PartnerRole = INTTRA_Out.PartnerInformationPartnerRole.FreightPayer,
            //                    PartnerName = this.iNTTRAGeneralMethods.GetStringList(myCard.EnglishName, 2, 35).ToArray<string>(),
            //                };

            //                if (this.AgentAddress != null)
            //                {
            //                    item.AddressInformation = this.GetAddressInformation(this.AgentAddress);
            //                }

            //                this.MessagePropertiesParties.Add(item);
            //            }
            //        }
            //    }
            //}
            #endregion

            #region MessageRecipient
            if (this.LoggedContact != null)
            {
                INTTRA_Out.PartnerInformation item = new INTTRA_Out.PartnerInformation()
                {
                    PartnerRole = INTTRA_Out.PartnerInformationPartnerRole.MessageRecipient,

                    PartnerName = this.iNTTRAGeneralMethods.GetStringList(LoggedContact.EnglishName, 2, 35).ToArray<string>(),

                    ContactInformation = this.GetContactInformation(this.LoggedContact, INTTRA_Out.ContactNameContactType.SINotification),
                };

                this.MessagePropertiesParties.Add(item);
            }
            #endregion

            #region FreightForwarder
            if (this.Shipment.FreightForwarderId != null)
            {
                Card myCard = (from d in CommonContext.Cards where d.Id == this.Shipment.FreightForwarderId select d).FirstOrDefault();
                if (myCard != null)
                {
                    INTTRA_Out.PartnerInformation item = new INTTRA_Out.PartnerInformation()
                    {
                        PartnerRole = INTTRA_Out.PartnerInformationPartnerRole.FreightForwarder,
                        PartnerName = this.iNTTRAGeneralMethods.GetStringList(myCard.EnglishName, 2, 35).ToArray<string>(),
                    };

                    if (this.FreightForwarderAddress != null)
                    {
                        item.AddressInformation = this.GetAddressInformation(this.FreightForwarderAddress);
                    }

                    this.MessagePropertiesParties.Add(item);
                }
            }

            else if (this.TenantObject != null)
            {
                INTTRA_Out.PartnerInformation item = new INTTRA_Out.PartnerInformation()
                {
                    PartnerRole = INTTRA_Out.PartnerInformationPartnerRole.FreightForwarder,
                    PartnerName = this.iNTTRAGeneralMethods.GetStringList(this.TenantObject.Company, 2, 35).ToArray<string>(),
                };

                if (this.TenantAddress != null)
                {
                    item.AddressInformation = this.GetAddressInformation(this.TenantAddress);
                }

                this.MessagePropertiesParties.Add(item);
            }
            #endregion
        }

        // Message Details
        private int lineNumber_Goods = 1;
        public List<INTTRA_Out.GoodsDetails> GoodsDetails;
        public List<INTTRA_Out.EquipmentDetails> EquipmentDetails;
        private List<PackageType> AllPackageTypes;
        private void BuildMessageDetails()
        {
            this.AllPackageTypes = new List<PackageType>();
            List<string> ids1 = this.ShipmentPackages.Where(d => d.PackageTypeId != null).Select(s => s.PackageTypeId).ToList();
            List<string> ids2 = this.InsidePackages.Where(d => d.PackageTypeId != null).Select(s => s.PackageTypeId).ToList();
            List<string> ids = ids1.Concat(ids2).ToList();

            if (ids.Count > 0)
            {
                this.AllPackageTypes = (from d in CommonContext.PackageTypes
                                        where d.Tenant == this.Tenant
                                        && ids.Contains(d.Id)
                                        select d).ToList();
            }


            this.BuildMessageDetails_EquipmentDetails();
        }
        private void BuildMessageDetails_EquipmentDetails()
        {
            this.GoodsDetails = new List<INTTRA_Out.GoodsDetails>();
            this.EquipmentDetails = new List<INTTRA_Out.EquipmentDetails>();

            int lineNumber = 1;
            foreach (ShipmentPackage item in this.ShipmentPackages)
            {
                INTTRA_Out.EquipmentDetails itemDetails = new INTTRA_Out.EquipmentDetails()
                {
                    LineNumber = lineNumber.ToString(),

                    EquipmentIdentifier = new INTTRA_Out.EquipmentIdentifier()
                    {
                        EquipmentSupplier = INTTRA_Out.EquipmentIdentifierEquipmentSupplier.Carrier,
                        EquipmentSupplierSpecified = true,
                        Value = item.ContainerNumber.ToUpper(),
                    },

                    //EquipmentComments = "",
                    //EquipmentLocation = "",
                    //EquipmentReferenceInformation = "",

                };

                itemDetails.EquipmentType = new INTTRA_Out.EquipmentType();

                if (item.PackageTypeId != null)
                {
                    PackageType myPackageType = this.AllPackageTypes.Where(d => d.Id == item.PackageTypeId).FirstOrDefault();
                    if (myPackageType != null)
                    {
                        itemDetails.EquipmentType.EquipmentTypeCode = myPackageType.Code;

                        string myTranslatedCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(myPackageType.Code, "G-INTTRA", "PackageType"); ;
                        if (!string.IsNullOrEmpty(myTranslatedCode))
                        {
                            itemDetails.EquipmentType.EquipmentTypeCode = myTranslatedCode;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(item.Description))
                {
                    itemDetails.EquipmentType.EquipmentDescription = this.iNTTRAGeneralMethods.FormatString(item.Description, 35);
                }

                if (item.Volume != null)
                {
                    itemDetails.EquipmentGrossVolume = new INTTRA_Out.EquipmentGrossVolume()
                    {
                        UOM = INTTRA_Out.EquipmentGrossVolumeUOM.MTQ,
                        Value = this.GetVolumeInCBM(item.Volume),
                    };
                }

                if (item.Weight != null)
                {
                    itemDetails.EquipmentGrossWeight = new INTTRA_Out.EquipmentGrossWeight()
                    {
                        UOM = INTTRA_Out.EquipmentGrossWeightUOM.KGM,
                        Value = this.GetWeightInKG(item.Weight),
                    };
                }

                if (item.Tare != null)
                {
                    itemDetails.EquipmentTareWeight = new INTTRA_Out.EquipmentTareWeight()
                    {
                        UOM = INTTRA_Out.EquipmentTareWeightUOM.KGM,
                        Value = this.GetWeightInKG(item.Tare),
                    };
                }

                if (item.ShipperSeal != null || item.CarrierSeal != null)
                {
                    List<INTTRA_Out.EquipmentSeal> mySeals = new List<INTTRA_Out.EquipmentSeal>();

                    if (item.ShipperSeal != null)
                    {
                        mySeals.Add(new INTTRA_Out.EquipmentSeal()
                        {
                            SealingParty = INTTRA_Out.EquipmentSealSealingParty.Shipper,
                            Value = item.ShipperSeal,
                        });
                    }

                    if (item.CarrierSeal != null)
                    {
                        mySeals.Add(new INTTRA_Out.EquipmentSeal()
                        {
                            SealingParty = INTTRA_Out.EquipmentSealSealingParty.Carrier,
                            Value = item.CarrierSeal,
                        });
                    }

                    itemDetails.EquipmentSeal = mySeals.ToArray<INTTRA_Out.EquipmentSeal>();
                }

                if (item.Ventilation != null)
                {
                    itemDetails.EquipmentAirFlow = new INTTRA_Out.EquipmentAirFlow()
                    {
                        UOM = INTTRA_Out.EquipmentAirFlowUOM.CBM,
                        Value = (float)(decimal)item.Ventilation.Value,
                    };
                }

                if (item.NonActiveContainer)
                {
                    itemDetails.EquipmentTemperature = new INTTRA_Out.EquipmentTemperature()
                    {
                        UOM = INTTRA_Out.EquipmentTemperatureUOM.CEL, 
                        Value = "999",
                    };

                    if (item.TemperatureUnitCode != "CEL")
                    {
                        itemDetails.EquipmentTemperature.UOM = INTTRA_Out.EquipmentTemperatureUOM.FAH;
                    }
                }

                else if (item.Temperature != null)
                {
                    itemDetails.EquipmentTemperature = new INTTRA_Out.EquipmentTemperature()
                    {
                        UOM = INTTRA_Out.EquipmentTemperatureUOM.CEL,
                        Value = item.Temperature,
                    };

                    if (item.TemperatureUnitCode != "CEL")
                    {
                        itemDetails.EquipmentTemperature.UOM = INTTRA_Out.EquipmentTemperatureUOM.FAH;
                    }
                }

                this.EquipmentDetails.Add(itemDetails);
                this.BuildMessageDetails_GoodsDetails(item, itemDetails);
                lineNumber++;
            }
        }
        private void BuildMessageDetails_GoodsDetails(ShipmentPackage myShipmentPackage, INTTRA_Out.EquipmentDetails itemEquipmentDetails)
        {
            List<InsideShipmentPackage> insidePackages = this.InsidePackages.Where(d => d.ShipmentPackageId == myShipmentPackage.Id).ToList();

            if (insidePackages.Count > 0)
            {
                foreach (InsideShipmentPackage item in insidePackages)
                {
                    string itemQuantity = item.Quantity == null ? "0" : item.Quantity.ToString();

                    INTTRA_Out.GoodsDetails itemGoodsDetails = new INTTRA_Out.GoodsDetails()
                    {
                        LineNumber = this.lineNumber_Goods.ToString(),
                        //LineNumber = itemEquipmentDetails.LineNumber,

                        PackageDetail = new INTTRA_Out.PackageDetail()
                        {
                            Level = INTTRA_Out.PackageDetailLevel.Outer,

                            NumberOfPackages = itemQuantity,
                        },

                        //ProductId="",
                        //PackageMarks = "",
                    };

                    if (item.PackageTypeId != null)
                    {
                        PackageType myPackageType = this.AllPackageTypes.Where(d => d.Id == item.PackageTypeId).FirstOrDefault();
                        if (myPackageType != null)
                        {
                            itemGoodsDetails.PackageDetail.PackageTypeCode = myPackageType.Code;
                            itemGoodsDetails.PackageDetail.PackageTypeDescription = this.iNTTRAGeneralMethods.FormatString(myPackageType.EnglishName, 35);

                            string myTranslatedCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(myPackageType.Code, "G-INTTRA", "PackageType"); ;
                            if (!string.IsNullOrEmpty(myTranslatedCode))
                            {
                                itemGoodsDetails.PackageDetail.PackageTypeCode = myTranslatedCode;
                            }
                        }
                    }

                    if (item.Description != null)
                    {
                        List<INTTRA_Out.PackageDetailComments> list = new List<INTTRA_Out.PackageDetailComments>();

                        INTTRA_Out.PackageDetailComments itemDescription = new INTTRA_Out.PackageDetailComments()
                        {
                            CommentType = INTTRA_Out.PackageDetailCommentsCommentType.GoodsDescription,
                            Value = item.Description,
                        };

                        list.Add(itemDescription);

                        if (this.AllInsideHarmonizes.Count > 0 || !string.IsNullOrEmpty(item.Harmonize))
                        {
                            if (item.IsMultiHarmonize)
                            {
                                List<ShipmentPackageHarmonize> iHarmonizes = this.AllInsideHarmonizes.Where(d => d.InsidePackageId == item.Id).ToList();

                                if (iHarmonizes.Count > 0)
                                {
                                    string iHarmonizeDescription = null;

                                    foreach (ShipmentPackageHarmonize itemHarmonize in iHarmonizes)
                                    {
                                        if (iHarmonizeDescription == null)
                                        {
                                            iHarmonizeDescription = "HS Code: " + itemHarmonize.Harmonize;
                                        }

                                        else
                                        {
                                            iHarmonizeDescription += ", " + itemHarmonize.Harmonize;
                                        }
                                    }

                                    list.Add(new INTTRA_Out.PackageDetailComments()
                                    {
                                        CommentType = INTTRA_Out.PackageDetailCommentsCommentType.GoodsDescription,
                                        Value = iHarmonizeDescription,
                                    });
                                }
                            }

                            else
                            {
                                if (!string.IsNullOrEmpty(item.Harmonize))
                                {
                                    string iHarmonizeDescription = "HS Code: " + item.Harmonize;

                                    list.Add(new INTTRA_Out.PackageDetailComments()
                                    {
                                        CommentType = INTTRA_Out.PackageDetailCommentsCommentType.GoodsDescription,
                                        Value = iHarmonizeDescription,
                                    });
                                }
                            }
                        }

                        else
                        {
                            if (myShipmentPackage.IsMultiHarmonize)
                            {
                                List<ShipmentPackageHarmonize> iHarmonizes = this.AllHarmonizes.Where(d => d.PackageId == myShipmentPackage.Id).ToList();

                                if (iHarmonizes.Count > 0)
                                {
                                    string iHarmonizeDescription = null;

                                    foreach (ShipmentPackageHarmonize itemHarmonize in iHarmonizes)
                                    {
                                        if (iHarmonizeDescription == null)
                                        {
                                            iHarmonizeDescription = "HS Code: " + itemHarmonize.Harmonize;
                                        }

                                        else
                                        {
                                            iHarmonizeDescription += ", " + itemHarmonize.Harmonize;
                                        }
                                    }

                                    list.Add(new INTTRA_Out.PackageDetailComments()
                                    {
                                        CommentType = INTTRA_Out.PackageDetailCommentsCommentType.GoodsDescription,
                                        Value = iHarmonizeDescription,
                                    });
                                }
                            }

                            else
                            {
                                if (!string.IsNullOrEmpty(myShipmentPackage.Harmonize))
                                {
                                    string iHarmonizeDescription = "HS Code: " + myShipmentPackage.Harmonize;

                                    list.Add(new INTTRA_Out.PackageDetailComments()
                                    {
                                        CommentType = INTTRA_Out.PackageDetailCommentsCommentType.GoodsDescription,
                                        Value = iHarmonizeDescription,
                                    });
                                }
                            }
                        }

                        itemGoodsDetails.PackageDetailComments = list.ToArray<INTTRA_Out.PackageDetailComments>();
                    }

                    if (item.Volume != null)
                    {
                        itemGoodsDetails.PackageDetailGrossVolume = new INTTRA_Out.PackageDetailGrossVolume()
                        {
                            UOM = INTTRA_Out.PackageDetailGrossVolumeUOM.MTQ,
                            Value = this.GetVolumeInCBM(item.Volume),
                        };
                    }

                    if (item.Weight != null)
                    {
                        itemGoodsDetails.PackageDetailGrossWeight = new INTTRA_Out.PackageDetailGrossWeight()
                        {
                            UOM = INTTRA_Out.PackageDetailGrossWeightUOM.KGM,
                            Value = this.GetWeightInKG(item.Weight),
                        };
                    }

                    if (myShipmentPackage.IsDangerous)
                    {
                        #region
                        if (!string.IsNullOrEmpty(myShipmentPackage.IMDGCode))
                        {
                            List<INTTRA_Out.HazardousGoods> HazardousGoodsList = new List<INTTRA_Out.HazardousGoods>();

                            INTTRA_Out.HazardousGoods HazardousGoodsItem = new INTTRA_Out.HazardousGoods()
                            {
                                IMOClassCode = this.iNTTRAGeneralMethods.FormatString(myShipmentPackage.ClassNumber, 7),
                            };

                            if (!string.IsNullOrEmpty(myShipmentPackage.IMDGCode))
                            {
                                HazardousGoodsItem.IMDGPageNumber = this.iNTTRAGeneralMethods.FormatString(myShipmentPackage.IMDGCode, 7);
                            }

                            if (myShipmentPackage.UnNumber != null)
                            {
                                HazardousGoodsItem.UNDGNumber = this.iNTTRAGeneralMethods.FormatString(myShipmentPackage.UnNumber, 4);
                            }

                            if (myShipmentPackage.EMS != null)
                            {
                                HazardousGoodsItem.EMSNumber = this.iNTTRAGeneralMethods.FormatString(myShipmentPackage.EMS, 6);
                            }

                            if (myShipmentPackage.FlashPoint != null)
                            {
                                HazardousGoodsItem.FlashpointTemperature = new INTTRA_Out.FlashpointTemperature()
                                {
                                    UOM = myShipmentPackage.FlashPointTemperatureUnitCode == "CEL" ? INTTRA_Out.FlashpointTemperatureUOM.CEL : INTTRA_Out.FlashpointTemperatureUOM.FAH,
                                    Value = myShipmentPackage.FlashPoint
                                };
                            }

                            if (myShipmentPackage.ProperShippingName != null)
                            {
                                INTTRA_Out.HazardousGoodsComments GoodsCommentsItem = new INTTRA_Out.HazardousGoodsComments()
                                {
                                    CommentType = INTTRA_Out.HazardousGoodsCommentsCommentType.ProperShippingName,
                                    Value = myShipmentPackage.ProperShippingName,
                                };

                                List<INTTRA_Out.HazardousGoodsComments> GoodsCommentsList = new List<INTTRA_Out.HazardousGoodsComments>();
                                GoodsCommentsList.Add(GoodsCommentsItem);

                                HazardousGoodsItem.HazardousGoodsComments = GoodsCommentsList.ToArray<INTTRA_Out.HazardousGoodsComments>();
                            }

                            if (this.Shipment.EmergencyContactId != null)
                            {
                                if (this.EmergencyContact != null)
                                {
                                    List<INTTRA_Out.EmergencyResponseContactContactInformation> listEmergency = new List<INTTRA_Out.EmergencyResponseContactContactInformation>();

                                    INTTRA_Out.EmergencyResponseContactContactInformation itemEmergency = new INTTRA_Out.EmergencyResponseContactContactInformation()
                                    {
                                        ContactName = new INTTRA_Out.EmergencyResponseContactContactInformationContactName()
                                        {
                                            ContactType = INTTRA_Out.EmergencyResponseContactContactInformationContactNameContactType.Emergency,
                                            Value = this.EmergencyContact.EnglishName,
                                        },
                                    };

                                    string Telephone = null;
                                    if (!string.IsNullOrEmpty(EmergencyContact.BusinessPhone))
                                    {
                                        Telephone = this.EmergencyContact.BusinessPhone;
                                    }

                                    else if (!string.IsNullOrEmpty(EmergencyContact.Mobile))
                                    {
                                        Telephone = this.EmergencyContact.Mobile;
                                    }

                                    if (!string.IsNullOrEmpty(Telephone))
                                    {
                                        itemEmergency.CommunicationValue = new INTTRA_Out.EmergencyResponseContactContactInformationCommunicationValue()
                                        {
                                            CommunicationType = INTTRA_Out.EmergencyResponseContactContactInformationCommunicationValueCommunicationType.Telephone,
                                            Value = Telephone,
                                        };
                                    }

                                    listEmergency.Add(itemEmergency);
                                    HazardousGoodsItem.EmergencyResponseContact = listEmergency.ToArray<INTTRA_Out.EmergencyResponseContactContactInformation>();
                                }
                            }

                            HazardousGoodsList.Add(HazardousGoodsItem);
                            itemGoodsDetails.HazardousGoods = HazardousGoodsList.ToArray<INTTRA_Out.HazardousGoods>();
                        }

                        #endregion
                    }

                    if (this.AllInsideHarmonizes.Count > 0 || !string.IsNullOrEmpty(item.Harmonize))
                    {
                        if (item.IsMultiHarmonize)
                        {
                            List<ShipmentPackageHarmonize> iHarmonizes = this.AllInsideHarmonizes.Where(d => d.InsidePackageId == item.Id).ToList();

                            if (iHarmonizes.Count > 0)
                            {
                                List<INTTRA_Out.ProductId> list = new List<INTTRA_Out.ProductId>();

                                foreach (ShipmentPackageHarmonize itemHarmonize in iHarmonizes)
                                {
                                    list.Add(new INTTRA_Out.ProductId()
                                    {
                                        ItemTypeIdCode = INTTRA_Out.ProductIdItemTypeIdCode.HarmonizedSystem,
                                        Value = itemHarmonize.Harmonize,
                                    });
                                }

                                itemGoodsDetails.ProductId = list.ToArray<INTTRA_Out.ProductId>();
                            }
                        }

                        else
                        {
                            if (!string.IsNullOrEmpty(item.Harmonize))
                            {
                                INTTRA_Out.ProductId itemProductId = new INTTRA_Out.ProductId()
                                {
                                    ItemTypeIdCode = INTTRA_Out.ProductIdItemTypeIdCode.HarmonizedSystem,
                                    Value = item.Harmonize
                                };

                                List<INTTRA_Out.ProductId> list = new List<INTTRA_Out.ProductId>();
                                list.Add(itemProductId);

                                itemGoodsDetails.ProductId = list.ToArray<INTTRA_Out.ProductId>();
                            }
                        }
                    }

                    else
                    {
                        if (myShipmentPackage.IsMultiHarmonize)
                        {
                            List<ShipmentPackageHarmonize> iHarmonizes = this.AllHarmonizes.Where(d => d.PackageId == myShipmentPackage.Id).ToList();

                            if (iHarmonizes.Count > 0)
                            {
                                List<INTTRA_Out.ProductId> list = new List<INTTRA_Out.ProductId>();

                                foreach (ShipmentPackageHarmonize itemHarmonize in iHarmonizes)
                                {
                                    list.Add(new INTTRA_Out.ProductId()
                                    {
                                        ItemTypeIdCode = INTTRA_Out.ProductIdItemTypeIdCode.HarmonizedSystem,
                                        Value = itemHarmonize.Harmonize,
                                    });
                                }

                                itemGoodsDetails.ProductId = list.ToArray<INTTRA_Out.ProductId>();
                            }
                        }

                        else
                        {
                            if (!string.IsNullOrEmpty(myShipmentPackage.Harmonize))
                            {
                                INTTRA_Out.ProductId itemProductId = new INTTRA_Out.ProductId()
                                {
                                    ItemTypeIdCode = INTTRA_Out.ProductIdItemTypeIdCode.HarmonizedSystem,
                                    Value = myShipmentPackage.Harmonize
                                };

                                List<INTTRA_Out.ProductId> list = new List<INTTRA_Out.ProductId>();
                                list.Add(itemProductId);

                                itemGoodsDetails.ProductId = list.ToArray<INTTRA_Out.ProductId>();
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(myShipmentPackage.MarksAndNumbers))
                    {
                        List<string> list = this.iNTTRAGeneralMethods.GetStringList(myShipmentPackage.MarksAndNumbers, 10, 35);

                        itemGoodsDetails.PackageMarks = list.ToArray<string>();
                    }

                    List<INTTRA_Out.SplitGoodsDetails> splitGoodsList = new List<INTTRA_Out.SplitGoodsDetails>();
                    splitGoodsList.Add(new INTTRA_Out.SplitGoodsDetails()
                    {
                        EquipmentIdentifier = myShipmentPackage.ContainerNumber.ToUpper(),
                        SplitGoodsNumberOfPackages = itemQuantity,

                        //SplitGoodsGrossVolume = new SplitGoodsGrossVolume()
                        //{
                        //    UOM = SplitGoodsGrossVolumeUOM.MTQ,
                        //    Value = this.GetVolumeInCBM(myShipmentPackage.Volume),
                        //},

                        //SplitGoodsGrossWeight = new SplitGoodsGrossWeight()
                        //{
                        //    UOM = SplitGoodsGrossWeightUOM.KGM,
                        //    Value = this.GetWeightInKG(myShipmentPackage.Weight) + "",
                        //},
                    });

                    // Houses 
                    AddHousesToGoodsDetails(itemGoodsDetails, myShipmentPackage.ContainerNumber);
                    itemGoodsDetails.SplitGoodsDetails = splitGoodsList.ToArray<INTTRA_Out.SplitGoodsDetails>();

                    this.GoodsDetails.Add(itemGoodsDetails);
                    this.lineNumber_Goods++;
                }
            }
        }

        private void AddHousesToGoodsDetails(INTTRA_Out.GoodsDetails itemGoodsDetails, string containerNumber)
        {
            if (this.Shipment.ShipmentLevelCode != "C" && this.MasterData.Transshipment1FromPort?.CountryCode != "US")
            {
                return;
            }
            
            this.AddHouses(itemGoodsDetails, containerNumber);
            
        }

        private void AddHouses(INTTRA_Out.GoodsDetails itemGoodsDetails, string containerNumber)
        {
            var houses = (from shipment in shipmentContext.Shipments.Where(t => t.MasterShipmentDataId == this.ShipmentId && t.ShipmentLevelCode == "H" && !t.IsCancelled)
                          where shipment.Tenant == this.Tenant
                          select shipment);

            if (houses?.Count() == 0)
            {
                return;
            }

            this.HandelMultiHouses(houses, itemGoodsDetails, containerNumber);
            this.AddShipmentIndicator();
        }

        private void HandelMultiHouses(IQueryable<Shipment> masterhouses, INTTRA_Out.GoodsDetails itemGoodsDetails, string containerNumber)
        {
            
            var houses = (from shipment in masterhouses
                          join shipmentPackage in shipmentContext.ShipmentPackages.Where(a => a.ContainerNumber == containerNumber)
                          on shipment.Id equals shipmentPackage.ShipmentId
                          where shipment.Tenant == this.Tenant
                          select shipment).ToList();

            if (houses?.Count() == 0)
            {
                return;
            }
            this.AddShipToPartner(this.Shipment);
            List<INTTRA_Out.HousePartiesPartnerInformation> houseParties = new List<INTTRA_Out.HousePartiesPartnerInformation>();
            List<INTTRA_Out.DetailsCustomsFilerInstruction> detailsCustomsInformation = new List<INTTRA_Out.DetailsCustomsFilerInstruction>();
            List<INTTRA_Out.DetailsReferenceInformation> detailsReferenceInformation = new List<INTTRA_Out.DetailsReferenceInformation>();
            this.AddDetailsCustomsInformation(detailsCustomsInformation);
            foreach (var house in houses)
            {
                this.AddHouseParties(houseParties, house);
                this.AddHouseDetailsReferenceInformation(detailsReferenceInformation, house);
            }
            itemGoodsDetails.HouseParties = houseParties.ToArray<INTTRA_Out.HousePartiesPartnerInformation>();
            itemGoodsDetails.DetailsCustomsInformation = detailsCustomsInformation.ToArray<INTTRA_Out.DetailsCustomsFilerInstruction>();
            itemGoodsDetails.DetailsReferenceInformation = detailsReferenceInformation.ToArray<INTTRA_Out.DetailsReferenceInformation>();
        }
        private void AddShipToPartner(Shipment shipment)
        {
            Card myCard = (from d in CommonContext.Cards where d.Id == shipment.ConsigneeId select d).FirstOrDefault();
            if (myCard == null)
            {
                return;
            }
            Contact consigneeContact = this.contactRepository.GetSingleContact(shipment.ConsigneeContactId, this.Tenant);
            INTTRA_Out.PartnerInformation item = new INTTRA_Out.PartnerInformation()
            {
                PartnerRole = INTTRA_Out.PartnerInformationPartnerRole.ShipTo,
                PartnerName = this.iNTTRAGeneralMethods.GetStringList(myCard.EnglishName, 2, 35).ToArray<string>(),
                ContactInformation = consigneeContact != null ? this.GetContactInformation(consigneeContact) : null,
            };
            if (this.ConsigneeAddress != null)
            {
                item.AddressInformation = this.GetAddressInformation(this.ConsigneeAddress);
            }
            this.MessagePropertiesParties.Add(item);
        }
        private void AddDetailsCustomsInformation(List<INTTRA_Out.DetailsCustomsFilerInstruction> detailsCustomsInformation)
        {
            detailsCustomsInformation.Add(new INTTRA_Out.DetailsCustomsFilerInstruction()
            {
                ManifestFilerStatus = INTTRA_Out.DetailsCustomsFilerInstructionManifestFilerStatus.Carrier,
                ManifestFilingCountryCode = new INTTRA_Out.ManifestFilingCountryCode
                {
                    Agency = INTTRA_Out.ManifestFilingCountryCodeAgency.UN,
                    Value = "US",
                }
            });
        }
        private void AddHouseParties(List<INTTRA_Out.HousePartiesPartnerInformation> houseParties, Shipment house)
        {
            var houseShipper = (from d in CommonContext.Cards where d.Id == house.ShipperId select d).FirstOrDefault();
            var houseConsignee = (from d in CommonContext.Cards where d.Id == house.ConsigneeId select d).FirstOrDefault();

            houseParties.Add(new INTTRA_Out.HousePartiesPartnerInformation()
            {
                PartnerRole = INTTRA_Out.HousePartiesPartnerInformationPartnerRole.OriginalShipper,
                PartnerName = this.iNTTRAGeneralMethods.GetStringList(houseShipper?.EnglishName, 2, 35).ToArray<string>(),
                AddressInformation = house.ShipperAddress != null ? this.GetAddressLines(house.ShipperAddress) : null,
            });
            houseParties.Add(new INTTRA_Out.HousePartiesPartnerInformation()
            {
                PartnerRole = INTTRA_Out.HousePartiesPartnerInformationPartnerRole.UltimateConsignee,
                PartnerName = this.iNTTRAGeneralMethods.GetStringList(houseConsignee?.EnglishName, 2, 35).ToArray<string>(),
                AddressInformation = house.ConsigneeAddress != null ? this.GetAddressLines(house.ConsigneeAddress) : null,
            });
        }
        private void AddHouseDetailsReferenceInformation(List<INTTRA_Out.DetailsReferenceInformation> detailsReferenceInformation, Shipment house)
        {
            detailsReferenceInformation.Add(new INTTRA_Out.DetailsReferenceInformation()
            {
                ReferenceType = INTTRA_Out.DetailsReferenceInformationReferenceType.HouseBillNumber,
                Value= house.ShipmentNumber,
                ReferenceTypeSpecified = true,
            });
        }
        private void AddShipmentIndicator()
        {
            if (this.ShipmentIndicator == null)
                this.ShipmentIndicator = new INTTRA_Out.ShipmentIndicator()
                {
                    IndicatorType = INTTRA_Out.ShipmentIndicatorIndicatorType.SingleMessage,
                    IndicatorTypeSpecified = true,
                };
        }

        // Tools
        private float GetVolumeInCBM(double? volume)
        {
            float myResult = 0;

            if (volume != null)
            {
                double? factorOfConvert = 1;

                if (!string.IsNullOrEmpty(this.VolumeUnitCode))
                {
                    switch (this.VolumeUnitCode.ToUpper())
                    {
                        case "CBM": { factorOfConvert = 1; break; }
                        case "CBI": { factorOfConvert = 61024; break; }      // 1m³ = 61024in³
                        case "CBF": { factorOfConvert = 35.315; break; }     // 1m³ = 35.315ft³
                    }
                }

                double? myComputedField = MethodHelper.Round(volume / factorOfConvert, 3);

                myResult = (float)myComputedField.Value;
            }

            return myResult;
        }
        private float GetWeightInKG(double? weight)
        {
            float myResult = 0;

            if (weight != null)
            {
                double? factorOfConvert = 1;

                if (!string.IsNullOrEmpty(this.GrossWeightUnitCode))
                {
                    switch (this.GrossWeightUnitCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }     // 1 LB = 0.45359237 KG
                        case "MT": { factorOfConvert = 1000; break; }           // 1 mt = 1000 KG
                    }
                }

                double? myComputedField = MethodHelper.Round(weight * factorOfConvert, 3);

                myResult = (float)myComputedField.Value;
            }

            return myResult;
        }

        private INTTRA_Out.AddressInformation GetAddressInformation(Address myAddress)
        {
            INTTRA_Out.AddressInformation myResult = null;

            if (myAddress != null)
            {
                string iCountryName = null;

                myResult = new INTTRA_Out.AddressInformation()
                {
                    //AddressLine = this.iNTTRAGeneralMethods.GetStringList(myAddress.Address1, 4, 35).ToArray<string>(),
                    City = this.iNTTRAGeneralMethods.FormatString(myAddress.City, 35),
                };

                if (!string.IsNullOrEmpty(myAddress.Address2))
                {
                    myResult.Street = this.iNTTRAGeneralMethods.GetStringList(myAddress.Address2, 2, 35).ToArray<string>();
                }

                if (!string.IsNullOrEmpty(myAddress.ZipCode))
                {
                    myResult.PostalCode = this.iNTTRAGeneralMethods.FormatString(myAddress.ZipCode, 19);
                }

                if (myAddress.CountryId != null)
                {
                    Country myCountry = (from d in CommonContext.Countries where d.Id == myAddress.CountryId select d).FirstOrDefault();
                    if (myCountry != null)
                    {
                        myResult.CountryCode = myCountry.Code;
                        iCountryName = myCountry.EnglishName;
                    }
                }

                if (myAddress.StateId != null)
                {
                    State myState = (from d in CommonContext.States where d.Id == myAddress.StateId select d).FirstOrDefault();
                    if (myState != null)
                    {
                        myResult.StateProvince = this.iNTTRAGeneralMethods.FormatString(myState.EnglishName, 9);
                    }
                }

                List<string> AddressLines = new List<string>();

                if (!string.IsNullOrEmpty(myAddress.Address1))
                {
                    if (AddressLines.Count < 4)
                    {
                        AddressLines.Add(this.iNTTRAGeneralMethods.FormatString(myAddress.Address1, 35));
                    }
                }

                if (!string.IsNullOrEmpty(myAddress.Address2))
                {
                    if (AddressLines.Count < 4)
                    {
                        AddressLines.Add(this.iNTTRAGeneralMethods.FormatString(myAddress.Address2, 35));
                    }
                }

                if (!string.IsNullOrEmpty(myAddress.City) || !string.IsNullOrEmpty(myAddress.ZipCode))
                {
                    if (AddressLines.Count < 4)
                    {
                        string iField = myAddress.City;

                        if (!string.IsNullOrEmpty(myAddress.ZipCode))
                        {
                            iField += "," + myAddress.ZipCode;
                        }

                        AddressLines.Add(this.iNTTRAGeneralMethods.FormatString(iField, 35));
                    }
                }

                if (!string.IsNullOrEmpty(iCountryName))
                {
                    if (AddressLines.Count < 4)
                    {
                        AddressLines.Add(this.iNTTRAGeneralMethods.FormatString(iCountryName, 35));
                    }
                }



                myResult.AddressLine = AddressLines.ToArray<string>();







                //string iAddressString = this.GetAddress_OneLine(myAddress);

                //myResult.AddressLine = this.iNTTRAGeneralMethods.GetStringList(iAddressString, 4, 35).ToArray<string>();
            }

            return myResult;
        }

        private string [] GetAddressLines(Address myAddress)
        {
            List<string> AddressLines = new List<string>();
            string iCountryName = null;
            if (!string.IsNullOrEmpty(myAddress.Address1))
            {
                if (AddressLines.Count < 4)
                {
                    AddressLines.Add(this.iNTTRAGeneralMethods.FormatString(myAddress.Address1, 35));
                }
            }

            if (!string.IsNullOrEmpty(myAddress.Address2))
            {
                if (AddressLines.Count < 4)
                {
                    AddressLines.Add(this.iNTTRAGeneralMethods.FormatString(myAddress.Address2, 35));
                }
            }

            if (!string.IsNullOrEmpty(myAddress.City) || !string.IsNullOrEmpty(myAddress.ZipCode))
            {
                if (AddressLines.Count < 4)
                {
                    string iField = myAddress.City;

                    if (!string.IsNullOrEmpty(myAddress.ZipCode))
                    {
                        iField += "," + myAddress.ZipCode;
                    }

                    AddressLines.Add(this.iNTTRAGeneralMethods.FormatString(iField, 35));
                }
            }
            if (myAddress.CountryId != null)
            {
                Country myCountry = (from d in CommonContext.Countries where d.Id == myAddress.CountryId select d).FirstOrDefault();
                if (myCountry != null)
                {
                    iCountryName = myCountry.EnglishName;
                }
            }
            if (!string.IsNullOrEmpty(iCountryName))
            {
                if (AddressLines.Count < 4)
                {
                    AddressLines.Add(this.iNTTRAGeneralMethods.FormatString(iCountryName, 35));
                }
            }

            return AddressLines.ToArray<string>();
        }
        private INTTRA_Out.ContactInformation[] GetContactInformation(Contact myContact, INTTRA_Out.ContactNameContactType iContactType = INTTRA_Out.ContactNameContactType.Informational)
        {
            List<INTTRA_Out.ContactInformation> ContactInformationList = new List<INTTRA_Out.ContactInformation>();

            if (myContact != null)
            {
                INTTRA_Out.ContactInformation item = new INTTRA_Out.ContactInformation()
                {
                    ContactName = new INTTRA_Out.ContactName()
                    {
                        ContactType = iContactType,
                        Value = myContact.EnglishName,
                    },
                };

                List<INTTRA_Out.CommunicationValue> CommunicationList = new List<INTTRA_Out.CommunicationValue>();

                CommunicationList.Add(new INTTRA_Out.CommunicationValue()
                {
                    CommunicationType = INTTRA_Out.CommunicationValueCommunicationType.Email,
                    Value = myContact.Email,
                });

                if (myContact.Fax != null)
                {
                    CommunicationList.Add(new INTTRA_Out.CommunicationValue()
                    {
                        CommunicationType = INTTRA_Out.CommunicationValueCommunicationType.Fax,
                        Value = myContact.Fax,
                    });
                }

                if (myContact.BusinessPhone != null)
                {
                    CommunicationList.Add(new INTTRA_Out.CommunicationValue()
                    {
                        CommunicationType = INTTRA_Out.CommunicationValueCommunicationType.Telephone,
                        Value = myContact.BusinessPhone,
                    });
                }

                item.CommunicationValue = CommunicationList.ToArray<INTTRA_Out.CommunicationValue>();

                ContactInformationList.Add(item);
            }

            return ContactInformationList.ToArray<INTTRA_Out.ContactInformation>();
        }

        private void HandelOneHouse()
        {
            if (this.Shipment.ShipmentLevelCode != "C" && this.MasterData.Transshipment1FromPort?.CountryCode != "US")
            {
                return;
            }
            var houses = (from shipment in shipmentContext.Shipments.Where(t => t.MasterShipmentDataId == this.ShipmentId && t.ShipmentLevelCode == "H" && !t.IsCancelled)
                          where shipment.Tenant == this.Tenant
                          select shipment);

            if (houses.Count() != 1)
            {
                return;
            }
            var house = houses.FirstOrDefault();
            this.AddHeaderCustomsInformation();
            this.AddSingleHouseParties(house);
        }
        private void AddHeaderCustomsInformation()
        {
            this.HeaderCustomsInformation = new List<INTTRA_Out.HeaderCustomsFilerInstruction>();

            this.HeaderCustomsInformation.Add(new INTTRA_Out.HeaderCustomsFilerInstruction()
            {
                ManifestFilerStatus = INTTRA_Out.HeaderCustomsFilerInstructionManifestFilerStatus.Carrier,
                ManifestFilingCountryCode = new INTTRA_Out.ManifestFilingCountryCode
                {
                    Agency = INTTRA_Out.ManifestFilingCountryCodeAgency.UN,
                    Value = "US",
                },
            });
        }

        private void AddSingleHouseParties(Shipment house)
        {
            this.AddShipToPartner(house);
            this.AddSupplierManufacturerPartner(house);
            this.AddUltimateConsignee(house);
        }
        private void AddSupplierManufacturerPartner(Shipment house)
        {
            Card myCard = (from d in CommonContext.Cards where d.Id == house.ConsigneeId select d).FirstOrDefault();
            if (myCard == null)
            {
                return;
            }
            Contact consigneeContact = this.contactRepository.GetSingleContact(house.ConsigneeContactId, this.Tenant);
            INTTRA_Out.PartnerInformation item = new INTTRA_Out.PartnerInformation()
            {
                PartnerRole = INTTRA_Out.PartnerInformationPartnerRole.SupplierManufacturer,
                PartnerName = this.iNTTRAGeneralMethods.GetStringList(myCard.EnglishName, 2, 35).ToArray<string>(),
                ContactInformation = consigneeContact != null ? this.GetContactInformation(consigneeContact) : null,
            };
            if (this.ConsigneeAddress != null)
            {
                item.AddressInformation = this.GetAddressInformation(this.ConsigneeAddress);
            }
            this.MessagePropertiesParties.Add(item);
        }
        private void AddUltimateConsignee(Shipment house)
        {
            Card myCard = (from d in CommonContext.Cards where d.Id == house.ConsigneeId select d).FirstOrDefault();
            if (myCard == null)
            {
                return;
            }
            Contact consigneeContact = this.contactRepository.GetSingleContact(house.ConsigneeContactId, this.Tenant);
            INTTRA_Out.PartnerInformation item = new INTTRA_Out.PartnerInformation()
            {
                PartnerRole = INTTRA_Out.PartnerInformationPartnerRole.UltimateConsignee,
                PartnerName = this.iNTTRAGeneralMethods.GetStringList(myCard.EnglishName, 2, 35).ToArray<string>(),
                ContactInformation = consigneeContact != null ? this.GetContactInformation(consigneeContact) : null,
            };
            if (this.ConsigneeAddress != null)
            {
                item.AddressInformation = this.GetAddressInformation(this.ConsigneeAddress);
            }
            this.MessagePropertiesParties.Add(item);
        }
    }
}
