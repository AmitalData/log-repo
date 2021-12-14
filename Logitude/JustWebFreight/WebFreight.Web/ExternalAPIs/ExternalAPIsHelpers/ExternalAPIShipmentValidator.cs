using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Validators;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Simplog.Data.ShipmentsModel;

namespace WebFreight.Web.ExternalAPIs.ExternalAPIsHelpers
{
    public class ExternalAPIShipmentValidator
    {
        private int tenant;
        private ShipmentPM shipmentPM;
        public ExternalAPIShipmentValidator(ShipmentPM shipmentPM, int tenant)
        {
            this.tenant = tenant;
            this.shipmentPM = shipmentPM;
        }

        public void ValidateUnitCodes()
        {
            if (string.IsNullOrEmpty(shipmentPM.VolumeUnitCode))
            {
                throw new ApplicationException("Missing volume unit code");
            }

            if (string.IsNullOrEmpty(shipmentPM.DimensionsUnitCode))
            {
                throw new ApplicationException("Missing dimensions unit code");
            }

            if (string.IsNullOrEmpty(shipmentPM.GrossWeightUnitCode))
            {
                throw new ApplicationException("Missing gross weight unit code");
            }

            if (string.IsNullOrEmpty(shipmentPM.ChargeableWeightUnitCode))
            {
                throw new ApplicationException("Missing chargeable weight unit code");
            }

            switch (shipmentPM.VolumeUnitCode)
            {
                case "CBF":
                    {
                        if (shipmentPM.DimensionsUnitCode == "Cm")
                        {
                            throw new ApplicationException("When volume unit is CBF, dimensions unit should be Inch or Ft");
                        }
                        break;
                    }

                case "CBI":
                    {
                        if (shipmentPM.DimensionsUnitCode != "Inc")
                        {
                            throw new ApplicationException("When volume unit is CBI, dimensions unit should be Inch");
                        }
                        break;
                    }

                case "CBM":
                    {
                        if (shipmentPM.DimensionsUnitCode != "Cm")
                        {
                            throw new ApplicationException("When volume unit is CBM, dimensions unit should be Cm");
                        }
                        break;
                    }
            }
        }
        public void ValidateAirShipmentCarrier()
        {
            if (shipmentPM.TransportModeId == "A")
            {
                if (string.IsNullOrEmpty(shipmentPM.MainCarriageCarrierId))
                {
                    if (!string.IsNullOrEmpty(shipmentPM.Master) || !string.IsNullOrEmpty(shipmentPM.MainCarriageCarrierNumber))
                    {
                        throw new ApplicationException("Missing Main carriage carrier");
                    }
                }
            }
        }
        public void ValidateShipmentClosure()
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                this.ValidateOperationalClosed();
                this.ValidateAccountingClosed();
                scope.Complete();
            }
        }
        public void ValidatePickupDeliveryPackages()
        {
            if (this.IsOceanInsightFeatureToggleExistInTenant())
            {
                if (this.IsShipmentHasPickup())
                {
                    foreach (ShipmentPickUpPM pickUp in shipmentPM.ShipmentPickUps)
                    {
                        if (pickUp.ShipmentPickUpDeliveryPackages != null && pickUp.ShipmentPickUpDeliveryPackages.Count > 0)
                        {
                            throw new ApplicationException("Creating Pickup package details is not permitted from the API");
                        }
                    }
                }
                if (this.IsShipmentHasDelivery())
                {
                    foreach (ShipmentDeliveryPM delivery in shipmentPM.ShipmentDeliveries)
                    {
                        if (delivery.ShipmentPickUpDeliveryPackages != null && delivery.ShipmentPickUpDeliveryPackages.Count > 0)
                        {
                            throw new ApplicationException("Creating Delivery package details is not permitted from the API");
                        }
                    }
                }
            }
        }
        public void ValidateConnectedHouses(Master masterEntity, IShipmentsContext MyContext)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                if (masterEntity.Houses.Count > 0)
                {
                    ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                    foreach (House item in masterEntity.Houses)
                    {
                        bool isValid = true;
                        Shipment myDataBaseShipment = null;

                        if (!string.IsNullOrEmpty(item.Id))
                        {
                            myDataBaseShipment = shipmentRepository.GetSingleShipment(item.Id, tenant);
                        }

                        if (myDataBaseShipment == null)
                        {
                            if (!string.IsNullOrEmpty(item.ShipmentNumber))
                            {
                                myDataBaseShipment = shipmentRepository.GetSingleShipmentByShipmentNumber(item.ShipmentNumber, tenant);
                            }
                        }

                        if (myDataBaseShipment == null)
                        {
                            isValid = false;
                            throw new ApplicationException("Shipment with ShipmentNumber " + item.ShipmentNumber + " doesn't exist");
                        }

                        if (!string.IsNullOrEmpty(item.Id) && !string.IsNullOrEmpty(item.ShipmentNumber))
                        {
                            if (myDataBaseShipment != null)
                            {
                                if (myDataBaseShipment.ShipmentNumber != item.ShipmentNumber)
                                {
                                    isValid = false;
                                    throw new ApplicationException("The sent Id and Shipment Number are not matching");
                                }
                            }
                        }

                        else if (myDataBaseShipment.ShipmentLevelCode != "H")
                        {
                            isValid = false;
                            throw new ApplicationException("The sent shipment is not house");
                        }

                        else if (!string.IsNullOrEmpty(myDataBaseShipment.MasterShipmentDataId))
                        {
                            isValid = false;
                            throw new ApplicationException("The sent shipment connected to another master");
                        }

                        else if (myDataBaseShipment.DirectionId != shipmentPM.DirectionId)
                        {
                            isValid = false;
                            throw new ApplicationException("The sent shipment direction not matches the master direction");
                        }

                        else if (myDataBaseShipment.TransportModeId != shipmentPM.TransportModeId)
                        {
                            isValid = false;
                            throw new ApplicationException("The sent shipment transport mode not matches the master transport mode");
                        }

                        else if (myDataBaseShipment.FromPortId != shipmentPM.FromPortId)
                        {
                            isValid = false;
                            throw new ApplicationException("The sent shipment from port not matches the master from port");
                        }

                        else if (myDataBaseShipment.ToPortId != shipmentPM.ToPortId)
                        {
                            isValid = false;
                            throw new ApplicationException("The sent shipment to port not matches the master to port");
                        }

                        else if (myDataBaseShipment.BranchId != shipmentPM.BranchId)
                        {
                            isValid = false;
                            throw new ApplicationException("The sent shipment branch not matches the master branch");
                        }

                        else if (myDataBaseShipment.IsCancelled)
                        {
                            isValid = false;
                            throw new ApplicationException("You can't connect cancelled house");
                        }

                        else if (myDataBaseShipment.IsOperationalClosed)
                        {
                            isValid = false;
                            throw new ApplicationException("You can't connect operational closed house");
                        }

                        else if (!string.IsNullOrEmpty(shipmentPM.ShipmentTypeId))
                        {
                            switch (shipmentPM.ShipmentTypeId.ToUpper())
                            {
                                case "MYGO":
                                    {
                                        if (myDataBaseShipment.ShipmentTypeId != "LCLD")
                                        {
                                            isValid = false;
                                            throw new ApplicationException("The sent shipment type should be LCL");
                                        }
                                        break;
                                    }

                                case "MYGI":
                                    {
                                        if (myDataBaseShipment.ShipmentTypeId != "LTL")
                                        {
                                            isValid = false;
                                            throw new ApplicationException("The sent shipment type should be LTL");
                                        }
                                        break;
                                    }
                            }
                        }

                        bool hasOpenPayables = false;
                        bool hasOpenReceivables = false;
                        if (masterEntity.IsAccountingClosed && masterEntity.IsOperationalClosed)
                        {
                            List<ShipmentReceivable> houseReceivables = MyContext.ShipmentReceivables.Where(d => d.ShipmentId == myDataBaseShipment.Id).ToList();
                            List<ShipmentPayable> housePayables = MyContext.ShipmentPayables.Where(d => d.ShipmentId == myDataBaseShipment.Id).ToList();

                            if (!hasOpenReceivables)
                            {
                                #region
                                if (houseReceivables.Count > 0)
                                {
                                    foreach (ShipmentReceivable recitem in houseReceivables)
                                    {
                                        if (recitem.ShipmentReceivableLineStatusCode != "ACCT" && recitem.ShipmentReceivableLineStatusCode != "EMPT")
                                        {
                                            if (recitem.TotalAmount != null && recitem.TotalAmount != 0)
                                            {
                                                hasOpenReceivables = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }

                            if (!hasOpenPayables)
                            {
                                #region
                                if (housePayables.Count > 0)
                                {
                                    foreach (ShipmentPayable payaitem in housePayables)
                                    {
                                        if (payaitem.ShipmentPayableLineStatusCode != "ACCT" && payaitem.ShipmentPayableLineStatusCode != "EMPT" && payaitem.ShipmentPayableParentId == null)
                                        {
                                            if (payaitem.ShipmentPayableAmountTypeCode == "NEXP")
                                            {
                                                if (payaitem.AccountedAmount != null && payaitem.AccountedAmount != 0)
                                                {
                                                    hasOpenPayables = true;
                                                    break;
                                                }
                                            }

                                            else
                                            {
                                                if (payaitem.ExpectedAmount != null && payaitem.ExpectedAmount != 0)
                                                {
                                                    hasOpenPayables = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }

                        if (hasOpenPayables || hasOpenReceivables)
                        {
                            isValid = false;
                            throw new ApplicationException("Can’t close for accounting: House #" + myDataBaseShipment.ShipmentNumber + " has open receivables/ payables");
                        }

                        if (isValid)
                        {
                            ConsoleShipmentPM myConsole = new ConsoleShipmentPM()
                            {
                                Id = myDataBaseShipment.Id,
                            };

                            shipmentPM.ShipmentConsoleShipments.Add(myConsole);
                        }
                    }
                }
                scope.Complete();
            }
        }
        public void ValidatePartnersDueToDirection()
        {
            switch (shipmentPM.DirectionId)
            {
                case "I":
                    {
                        if (string.IsNullOrEmpty(shipmentPM.ConsigneeId))
                        {
                            throw new ApplicationException("Consignee is required for import houses");
                        }

                        break;
                    }

                case "E":
                    {
                        if (string.IsNullOrEmpty(shipmentPM.ShipperId))
                        {
                            throw new ApplicationException("Shipper is required for export houses");
                        }

                        break;
                    }

                case "D":
                    {
                        if (string.IsNullOrEmpty(shipmentPM.ShipperId))
                        {
                            throw new ApplicationException("Shipper is required for domestic houses");
                        }

                        break;
                    }

                case "R":
                    {
                        if (string.IsNullOrEmpty(shipmentPM.ShipperId))
                        {
                            throw new ApplicationException("Shipper is required for drop houses");
                        }

                        break;
                    }
            }
        }
        public void ValidatePreAndOnCarrageFields()
        {
            this.ValidatePreCarrageFields();
            this.ValidateOnCarrageFields();
            this.ValidatePreCarrageToPortField();
            this.ValidateOnCarrageFromPortField();
            this.ValidateOnCarriageDates();
            this.ValidatePreCarriageDates();
            this.ValidatePreCarriageVessel();
            this.ValidateOnCarriageVessel();
        }
        private void ValidateOperationalClosed()
        {
            if (shipmentPM.IsOperationalClosed)
            {
                string errorMessage = "";

                RulesValidator validator = new RulesValidator();
                validator.Initialize(tenant);
                List<ObjectTableRuleField> requiredFields = validator.ValidateAllRequiredFieldRules(shipmentPM, "Shipment", tenant);

                IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);
                ObjectFieldRepository ObjectFieldRepository = new ObjectFieldRepository(webFreightContext);
                if (requiredFields.Count > 0)
                {
                    foreach (ObjectTableRuleField field in requiredFields)
                    {
                        ObjectField f = ObjectFieldRepository.GetSingleObjectFieldByCode(field.ObjectFieldCode, tenant);
                        errorMessage = errorMessage + ", " + TranslateTextsClass.GetTranslation("General.M.FieldIsRequired", f.FullNameTextCode.Code, null, null, field.Tenant);
                    }
                }

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = errorMessage.TrimStart(',');
                    throw new ApplicationException("Due to operational closed: " + errorMessage);
                }
            }
        }
        private void ValidateAccountingClosed()
        {
            if (shipmentPM.IsAccountingClosed)
            {
                AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(tenant);
                AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountingSetting(tenant);

                bool hasOpenPayables = false;
                bool hasOpenReceivables = false;
                if (shipmentPM.ShipmentReceivables.Count() > 0)
                {
                    if (shipmentPM.ShipmentReceivables.Where(p => p.TotalAmount != null && p.TotalAmount != 0).Any())
                    {
                        hasOpenReceivables = true;
                    }
                }

                if (accountingSetting != null && !accountingSetting.AllowClosureWithoutPayables)
                {
                    if (shipmentPM.ShipmentPayables.Count() > 0)
                    {
                        if (shipmentPM.ShipmentPayables.Where(p => p.ExpectedAmount != null && p.ExpectedAmount != 0).Any())
                        {
                            hasOpenPayables = true;
                        }
                    }
                }

                if (hasOpenPayables || hasOpenReceivables)
                {
                    throw new ApplicationException("can’t close for accounting if there are any open payables/receivables.");
                }

                if (!shipmentPM.IsOperationalClosed)
                {
                    throw new ApplicationException("Shipment shoud be closed operationally");
                }
            }
        }        
        private bool IsOceanInsightFeatureToggleExistInTenant()
        {
            string ocaenInsightFeatureToggleCode = "OIC";
            IInfrastructureContext context = InfrastructureContext.GetContext(0);
            FeatureToggleRepository repository = new FeatureToggleRepository(context);
            IQueryable<FeatureToggle> featureToggles = repository.GetAll(0);
            List<FeatureToggle> featureTogglesList = featureToggles.ToList();
            if (featureTogglesList != null)
            {
                return IsFeatureToggleExistInMultiOrSingleTenant(featureTogglesList.Find(a => a.ToggleCode == ocaenInsightFeatureToggleCode && !a.Inactive), tenant);
            }
            return false;
        }
        private bool IsFeatureToggleExistInMultiOrSingleTenant(FeatureToggle ocaenInsightFeatureToggle, int tenant)
        {
            if (ocaenInsightFeatureToggle == null)
                return false;

            if (ocaenInsightFeatureToggle.IsMultiTenant)
            {
                return ((tenant >= ocaenInsightFeatureToggle.FromTenantNumber) && (ocaenInsightFeatureToggle.ToTenantNumber <= tenant));
            }
            else
            {
                return (tenant == ocaenInsightFeatureToggle.TenantNumber);
            }
        }
        private bool IsShipmentHasPickup()
        {
            return shipmentPM.ShipmentPickUps != null && shipmentPM.ShipmentPickUps.Count > 0;
        }
        private bool IsShipmentHasDelivery()
        {
            return shipmentPM.ShipmentDeliveries != null && shipmentPM.ShipmentDeliveries.Count > 0;
        }
        private void ValidatePreCarrageFields()
        {
            if (!IsAnyFieldOfPreCarrageNotNull())
                return;

            if (string.IsNullOrEmpty(shipmentPM.PreCarriageTransportModeId))
                this.ThrowRequiredFieldExcption("PreCarriageTransportMode");

            if (string.IsNullOrEmpty(shipmentPM.PreCarriageFromPortId))
                this.ThrowRequiredFieldExcption("PreCarriageFromPort");

            if (string.IsNullOrEmpty(shipmentPM.PreCarriageToPortId))
                this.ThrowRequiredFieldExcption("PreCarriageToPort");
        }
        private void ValidateOnCarrageFields()
        {
            if (!IsAnyFieldOfOnCarrageNotNull())
                return;

            if (string.IsNullOrEmpty(shipmentPM.OnCarriageTransportModeId))
                this.ThrowRequiredFieldExcption("OnCarriageTransportMode");

            if (string.IsNullOrEmpty(shipmentPM.OnCarriageFromPortId))
                this.ThrowRequiredFieldExcption("OnCarriageFromPort");

            if (string.IsNullOrEmpty(shipmentPM.OnCarriageToPortId))
                this.ThrowRequiredFieldExcption("OnCarriageToPort");
        }
        private bool IsAnyFieldOfPreCarrageNotNull()
        {
            if (!string.IsNullOrEmpty(shipmentPM.PreCarriageTransportModeId))
                return true;

            if (!string.IsNullOrEmpty(shipmentPM.PreCarriageFromPortId))
                return true;

            if (!string.IsNullOrEmpty(shipmentPM.PreCarriageToPortId))
                return true;

            if (!string.IsNullOrEmpty(shipmentPM.PreCarriageCarrierId))
                return true;

            if (!string.IsNullOrEmpty(shipmentPM.PreCarriageCarrierNumber))
                return true;

            if (shipmentPM.PreCarriageETA != null)
                return true;

            if (shipmentPM.PreCarriageETD != null)
                return true;

            if (shipmentPM.PreCarriageATA != null)
                return true;

            if (shipmentPM.PreCarriageATD != null)
                return true;

            return false;
        }
        private bool IsAnyFieldOfOnCarrageNotNull()
        {
            if (!string.IsNullOrEmpty(shipmentPM.OnCarriageTransportModeId))
                return true;

            if (!string.IsNullOrEmpty(shipmentPM.OnCarriageFromPortId))
                return true;

            if (!string.IsNullOrEmpty(shipmentPM.OnCarriageToPortId))
                return true;

            if (!string.IsNullOrEmpty(shipmentPM.OnCarriageCarrierId))
                return true;

            if (!string.IsNullOrEmpty(shipmentPM.OnCarriageCarrierNumber))
                return true;

            if (shipmentPM.OnCarriageETA != null)
                return true;

            if (shipmentPM.OnCarriageETD != null)
                return true;

            if (shipmentPM.OnCarriageATA != null)
                return true;

            if (shipmentPM.OnCarriageATD != null)
                return true;

            return false;
        }
        private void ThrowRequiredFieldExcption(string fieldName)
        {
            throw new ApplicationException(fieldName + " Is Required");
        }
        private void ValidatePreCarrageToPortField()
        {
            if (string.IsNullOrEmpty(shipmentPM.PreCarriageToPortId) || string.IsNullOrEmpty(shipmentPM.PreCarriageFromPortId))
                return;

            string mainCarriageFromPortId = GetMainCarriageFromPortId();
            if (shipmentPM.PreCarriageToPortId != mainCarriageFromPortId)
                throw new ApplicationException("PreCarriageToPort Must Be Same As MainCarriageFromPort");
        }

        private string GetMainCarriageFromPortId()
        {
            if (!string.IsNullOrEmpty(shipmentPM.MainCarriageFromPortId))
                return shipmentPM.MainCarriageFromPortId;

            if(shipmentPM.MainCarriageLegs.Count > 0)
            {
                var mainCarriageLeg = shipmentPM.MainCarriageLegs.FirstOrDefault();
                return mainCarriageLeg?.FromPortId;
            }

            return "";
        }


        private string GetMainCarriageToPortId()
        {
            if (!string.IsNullOrEmpty(shipmentPM.MainCarriageToPortId))
                return shipmentPM.MainCarriageToPortId;

            if (shipmentPM.MainCarriageLegs.Count > 0)
            {
                var mainCarriageLeg = shipmentPM.MainCarriageLegs.LastOrDefault();
                return mainCarriageLeg?.ToPortId;
            }

            return "";
        }

        private void ValidateOnCarrageFromPortField()
        {
            if (string.IsNullOrEmpty(shipmentPM.OnCarriageFromPortId) || string.IsNullOrEmpty(shipmentPM.OnCarriageFromPortId))
                return;

            string mainCarriageToPortId = this.GetMainCarriageToPortId();
            if (shipmentPM.OnCarriageFromPortId != mainCarriageToPortId)
                throw new ApplicationException("OnCarriageFromPort Must Be Same As Final Main Carriage To Port");
        }
        private void ValidateOnCarriageDates()
        {
            if (!this.IsRoutingLegDatesValid(shipmentPM.OnCarriageETD, shipmentPM.OnCarriageETA))
            {
                throw new ApplicationException("On Carriage expected departure must be less than On Carriage expected arrival");
            }

            if (!this.IsRoutingLegDatesValid(shipmentPM.OnCarriageATD, shipmentPM.OnCarriageATA))
            {
                throw new ApplicationException("On Carriage actual departure must be less than On Carriage actual arrival");
            }
        }
        private void ValidatePreCarriageDates()
        {
            if (!this.IsRoutingLegDatesValid(shipmentPM.PreCarriageETD, shipmentPM.PreCarriageETA))
            {
                throw new ApplicationException("Pre Carriage expected departure must be less than Pre Carriage expected arrival");
            }

            if (!this.IsRoutingLegDatesValid(shipmentPM.PreCarriageATD, shipmentPM.PreCarriageATA))
            {
                throw new ApplicationException("Pre Carriage actual departure must be less than Pre Carriage actual arrival");
            }
        }
        private bool IsRoutingLegDatesValid(DateTime? fisrtDate, DateTime? secondeDate)
        {
            bool isValid = true;

            if (fisrtDate != null && secondeDate != null)
            {
                if (fisrtDate > secondeDate.Value.AddHours(24))
                {
                    isValid = false;
                }
            }
            return isValid;
        }
        private void ValidatePreCarriageVessel()
        {
            if (string.IsNullOrEmpty(shipmentPM.PreCarriageToPortId) || string.IsNullOrEmpty(shipmentPM.PreCarriageFromPortId))
                return;

            if (shipmentPM.PreCarriageTransportModeId == "O")
                return;

            if(!string.IsNullOrEmpty(shipmentPM.PreCarriageVesselId))
                throw new ApplicationException("PreCarriage should not have Vessel");

        }
        private void ValidateOnCarriageVessel()
        {
            if (string.IsNullOrEmpty(shipmentPM.OnCarriageFromPortId) || string.IsNullOrEmpty(shipmentPM.OnCarriageFromPortId))
                return;

            if (shipmentPM.OnCarriageTransportModeId == "O")
                return;

            if (!string.IsNullOrEmpty(shipmentPM.OnCarriageVesselId))
                throw new ApplicationException("OnCarriage should not have Vessel");
        }
    }
}