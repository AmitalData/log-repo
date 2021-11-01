using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.ExternalAPIs.ExternalAPIsHelpers
{
    public class ExternalAPIXMLEntityValidator
    {
        private int tenant;
        private Direction direction;
        private TransportMode transportMode;
        private ShipmentType shipmentType;
        private Port fromPort;
        private Port toPort;
        private List<MainCarriageLeg> mainCarriageLegs;
        private List<Receivable> receivables;
        private List<Payable> payables;
        private List<OceanOrInlandPackage> oceanOrInlandPackages;
        public ExternalAPIXMLEntityValidator(int tenant)
        {
            this.tenant = tenant;
        }

        public void ValidateDirectEntity(Direct directEntity)
        {
            this.SetCommonDataFromDirectEntity(directEntity);

            if (this.transportMode != null)
            {
                this.ValidateShipmentPackagesDueToShipmentType();
                this.ValidateMasterNumberAndCarrier(directEntity.Master, directEntity.MainCarriageCarrier);
                this.ValidateMissingShipmentType();
            }

            this.ValidateReceivables();
            this.ValidatePayables();
            this.ValidatePorts();
        }

        public void ValidateHouseEntity(House houseEntity, IShipmentsContext context)
        {
            this.SetCommonDataFromHouseEntity(houseEntity);
            this.ValidateHouseNumber(houseEntity.HouseNo, context);            

            if (this.transportMode != null)
            {
                this.ValidateShipmentPackagesDueToShipmentType();
                this.ValidateMissingShipmentType();
            }

            this.ValidateReceivables();
            this.ValidatePayables();  
        }

        public void ValidateMasterEntity(Master masterEntity)
        {
            this.SetCommonDataFromMasterEntity(masterEntity);

            if (this.transportMode != null)
            {
                this.ValidateMasterNumberAndCarrier(masterEntity.MasterNumber, masterEntity.MainCarriageCarrier);
                this.ValidateMissingShipmentType();               
            }

            this.ValidateReceivables();
            this.ValidatePayables();
            this.ValidatePorts();
        }

        private void SetCommonDataFromDirectEntity(Direct directEntity)
        {
            this.direction = directEntity.Direction;
            this.transportMode = directEntity.TransportMode;
            this.shipmentType = directEntity.ShipmentType;
            this.fromPort = directEntity.FromPort;
            this.toPort = directEntity.ToPort;
            this.mainCarriageLegs = directEntity.MainCarriageLegs;
            this.receivables = directEntity.Receivables;
            this.payables = directEntity.Payables;
            this.oceanOrInlandPackages = directEntity.OceanOrInlandPackages;
        }
        private void SetCommonDataFromHouseEntity(House houseEntity)
        {
            this.direction = houseEntity.Direction;
            this.transportMode = houseEntity.TransportMode;
            this.shipmentType = houseEntity.ShipmentType;
            this.fromPort = houseEntity.FromPort;
            this.toPort = houseEntity.ToPort;
            this.receivables = houseEntity.Receivables;
            this.payables = houseEntity.Payables;
            this.oceanOrInlandPackages = houseEntity.OceanOrInlandPackages;
        }
        private void SetCommonDataFromMasterEntity(Master masterEntity)
        {
            this.direction = masterEntity.Direction;
            this.transportMode = masterEntity.TransportMode;
            this.shipmentType = masterEntity.ShipmentType;
            this.fromPort = masterEntity.FromPort;
            this.toPort = masterEntity.ToPort;
            this.mainCarriageLegs = masterEntity.MainCarriageLegs;
            this.receivables = masterEntity.Receivables;
            this.payables = masterEntity.Payables;
        }
        private void ValidateShipmentPackagesDueToShipmentType()
        {
            if (this.IsValidatingPackages())
            {
                foreach (OceanOrInlandPackage item in oceanOrInlandPackages)
                {
                    this.ValidateInsidePackages(item.InsidePackages);
                    this.ValidatePackageType(item.PackageType);                    
                }
            }
        }
        private void ValidateInsidePackages(List<InsidePackage> insidePackages)
        {
            if (insidePackages != null && insidePackages.Count > 0)
            {
                foreach (InsidePackage itemInside in insidePackages)
                {
                    if (itemInside.PackageType != null)
                    {
                        PackageTypeQueryService PackageTypeService0 = new PackageTypeQueryService(tenant);
                        PackageTypePM PackageTypePM = PackageTypeService0.PackageTypeDataMappingAndValidatin(itemInside.PackageType, tenant);
                        if (PackageTypePM != null)
                        {
                            if (PackageTypePM.IsContainer == true)
                            {
                                throw new ApplicationException("Invalid Inside Package Type Code");
                            }
                        }
                        else
                        {
                            itemInside.PackageType = null;
                        }
                    }
                }
            }
        }
        private void ValidatePackageType(PackageType packageType)
        {
            if (packageType == null)
            {
                string message = shipmentType.Code.Contains("LCL") ? "Package Type is required" : "Container Type is required";
                throw new ApplicationException(message);
            }
        }
        private void ValidateHouseNumber(string houseNo, IShipmentsContext context)
        {
            if (!string.IsNullOrEmpty(houseNo))
            {
                bool exist = (from a in context.Shipments
                              where a.Tenant == tenant
                              && a.ShipmentLevelCode == "H"
                              && !string.IsNullOrEmpty(a.House)
                              && a.House == houseNo
                              select a).Any();

                if (exist)
                {
                    throw new ApplicationException("A House with the given house number already exists");
                }
            }
        }
        private void ValidateReceivables()
        {
            if (this.IsShipmentHasReceivables())
            {
                foreach (Receivable item in this.receivables)
                {
                    if (item.ChargesType == null)
                    {
                        throw new ApplicationException("Receivable Charges Type is required");
                    }

                    if (item.Currency == null)
                    {
                        string currencyId = null;
                        if (item.ChargesType != null)
                        {
                            currencyId = this.GetChargesTypeCurrency(item.ChargesType.Code, "R");
                        }

                        if (string.IsNullOrEmpty(currencyId))
                        {
                            throw new ApplicationException("Receivable Currency is required");
                        }
                    }
                }
            }
        }
        private void ValidatePayables()
        {
            if (IsShipmentHasPayables())
            {
                foreach (Payable item in payables)
                {
                    if (item.ChargesType == null)
                    {
                        throw new ApplicationException("Payable Charges Type is required");
                    }


                    if (item.Currency == null)
                    {
                        string currencyId = null;
                        if (item.ChargesType != null)
                        {
                            currencyId = this.GetChargesTypeCurrency(item.ChargesType.Code, "P");
                        }

                        if (string.IsNullOrEmpty(currencyId))
                        {
                            throw new ApplicationException("Payable Currency is required");
                        }
                    }
                }
            }
        }
        private void ValidatePorts()
        {
            if (fromPort != null && toPort != null)
            {
                if (IsShipmentHasMainCarriageLegs())
                {
                    throw new ApplicationException("You can't use the From Port/ To Port with the Main Carriage Legs");
                }
            }
            else
            {
                if (mainCarriageLegs == null || mainCarriageLegs.Count == 0)
                {
                    throw new ApplicationException("You must send the From Port/ To Port or the Main Carriage Legs");
                }
            }
        }
        private string GetChargesTypeCurrency(string chargeTypeCode, string indicator)
        {
            string currency = null;
            if (!string.IsNullOrEmpty(chargeTypeCode))
            {
                ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(tenant);
                var chargesType = chargesTypeRepository.GetSingleChargesTypeByCode(chargeTypeCode, tenant);
                if (chargesType != null)
                {
                    if (indicator == "R" && !string.IsNullOrEmpty(chargesType.ReceivablesDefaultCurrencyId))
                    {
                        currency = chargesType.ReceivablesDefaultCurrencyId;
                    }

                    else if (indicator == "P" && !string.IsNullOrEmpty(chargesType.PayablesDefaultCurrencyId))
                    {
                        currency = chargesType.PayablesDefaultCurrencyId;
                    }
                }                
            }

            return currency;
        }
        private void ValidateMissingShipmentType()
        {
            if (this.transportMode.Code != "A")
            {
                if (this.shipmentType == null || (this.shipmentType != null && string.IsNullOrEmpty(this.shipmentType.Code)))
                {
                    throw new ApplicationException("Missing Shipment Type");
                }
            }
        }
        private void ValidateMasterNumberAndCarrier(string master, Card carrier)
        {
            if (this.transportMode.Code == "A")
            {
                bool validate = false;
                if (!string.IsNullOrEmpty(master))
                {
                    if (carrier == null)
                    {
                        validate = true;
                    }

                    else
                    {
                        if (string.IsNullOrEmpty(carrier.Code))
                        {
                            validate = true;
                        }
                    }
                }

                if (validate)
                {
                    throw new ApplicationException("Main Carriage Carrier is required when sending MAWB");
                }
            }
        }
        private bool IsInlandDomesticShipment()
        {
            bool isInland = false;
            bool isDomestic = false;
            if (this.direction != null)
            {
                isDomestic = this.direction.Code == "D" ? true : false;
            }

            if (this.transportMode != null)
            {
                isInland = this.transportMode.Code == "I" ? true : false;
            }

            return isDomestic && isInland;
        }
        private bool IsValidatingPackages()
        {
            if (this.shipmentType == null)
            {
                return false;
            }

            else if (transportMode.Code == "A")
            {
                return false;
            }

            else if (this.IsInlandDomesticShipment())
            {
                return false;
            }            

            else if (!IsShipmentHasPackages())
            {
                return false;
            }

            else
            {
                return true;
            }
        }
        private bool IsShipmentHasReceivables()
        {
            return this.receivables != null && this.receivables.Count > 0;
        }
        private bool IsShipmentHasPayables()
        {
            return this.payables != null && this.payables.Count > 0;
        }
        private bool IsShipmentHasMainCarriageLegs()
        {
            return this.mainCarriageLegs != null && this.mainCarriageLegs.Count > 0;
        }
        private bool IsShipmentHasPackages()
        {
            return this.oceanOrInlandPackages != null && this.oceanOrInlandPackages.Count > 0;
        }
    }
}