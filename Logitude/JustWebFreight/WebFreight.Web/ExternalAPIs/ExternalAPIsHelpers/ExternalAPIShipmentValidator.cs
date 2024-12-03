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
using Logitude.BL.ShipmentsModel.EntityQueries;
using WebFreight.Web.Helpers.APIHelpers;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace WebFreight.Web.ExternalAPIs.ExternalAPIsHelpers
{
    public class ExternalAPIShipmentValidator
    {
        private int tenant;
        private ShipmentPM shipmentPM;

        private VesselRepository vesselRepository;
        private CardQuery cardQuery;
        private CommodityRepository commodityRepository;

        public ExternalAPIShipmentValidator(ShipmentPM shipmentPM, int tenant)
        {
            this.tenant = tenant;
            this.shipmentPM = shipmentPM;
            this.vesselRepository = new VesselRepository(tenant);
            this.cardQuery = new CardQuery(tenant);
            this.commodityRepository = new CommodityRepository(tenant);
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

                //case "CBI":
                //    {
                //        if (shipmentPM.DimensionsUnitCode != "Inc")
                //        {
                //            throw new ApplicationException("When volume unit is CBI, dimensions unit should be Inch");
                //        }
                //        break;
                //    }

                //case "CBM":
                //    {
                //        if (shipmentPM.DimensionsUnitCode != "Cm")
                //        {
                //            throw new ApplicationException("When volume unit is CBM, dimensions unit should be Cm");
                //        }
                //        break;
                //    }
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
            if (!this.IsOceanInsightFeatureToggleExistInTenant())
            {
                return;
            }
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

        public void ValidateInActiveCarriers(ShipmentPM entityPM)
        {
            this.ValidateInActiveCard(entityPM.MainCarriageCarrierId, "MainCarriageCarrier");
            this.ValidateInActiveCard(entityPM.Transshipment1CarrierId, "Transshipment1Carrier");
            this.ValidateInActiveCard(entityPM.Transshipment2CarrierId, "Transshipment2Carrier");
            this.ValidateInActiveCard(entityPM.Transshipment3CarrierId, "Transshipment3Carrier");
            this.ValidateInActiveCard(entityPM.PreCarriageCarrierId, "PreCarriageCarrier");
            this.ValidateInActiveCard(entityPM.OnCarriageCarrierId, "OnCarriageCarrier");
            this.ValidateInActiveCard(entityPM.PreForwardingCarrierId, "PreForwardingCarrier");
            this.ValidateInActiveCard(entityPM.OnForwardingCarrierId, "OnForwardingCarrier");
            this.ValidateInActiveCarriersPickUps(entityPM);
            this.ValidateInActiveCarriersDeliveries(entityPM);
        }

        private void ValidateInActiveCard(string carrierId, string carrierFieldName, string partnerType = null)
        {
            CarrierCard card = this.cardQuery.GetSingleCarrierCard(carrierId, tenant, partnerType);
            if(card == null)
            {
                return;
            }
            var inActive = card.InActive;
            var carrierCode = card.Code;
            if (inActive)
                throw new ApplicationException("The " + carrierFieldName + " with the code " + carrierCode + " is inactive and cannot be used.");
        }
        private void ValidateInActiveCarriersPickUps(ShipmentPM entityPM)
        {
            foreach (ShipmentPickUpPM shipmentPickUp in entityPM.ShipmentPickUps)
            {
                this.ValidateInActiveCard(shipmentPickUp.CarrierId,  "PickUpCarrier", "TR");
            }
        }
        private void ValidateInActiveCarriersDeliveries(ShipmentPM entityPM)
        {
            foreach (ShipmentDeliveryPM shipmentDelivery in entityPM.ShipmentDeliveries)
            {
                this.ValidateInActiveCard(shipmentDelivery.CarrierId, "DeliveryCarrier", "TR");
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
            this.MapPreCarriageVesselName();
            this.MapOnCarriageVesselName();
        }
        public void UpdatePickupDeliveryPackagesChangeSet(ShipmentPM entityPM)
        {
            this.UpdatePickupPackagesChangeSet(entityPM);
            this.UpdateDeliveryPackagesChangeSet(entityPM);
        }
        public void ValidateUpdateShipmentPackages(ShipmentPM entityPM)
        {
            if (!(entityPM.ShipmentPackages.Count > 0))
            {
                return;
            }

            foreach (ShipmentPackagePM item in entityPM.ShipmentPackages)
            {
                item.ChangeSetOp = this.GetChangeSet(item.ChangeSet);
                this.ValidateShipmentPackageItem(item, entityPM);
            }
        }
        public void UpdatePayablesChangeSet(ShipmentPM entityPM)
        {
            if (entityPM.ShipmentPayables.Count == 0)
            {
                return;
            }

            foreach (ShipmentPayablePM payable in entityPM.ShipmentPayables)
            {
                payable.ChangeSetOp = this.GetChangeSet(payable.ChangeSet);
            }
        }
        public void UpdateReceivablesChangeSet(ShipmentPM entityPM)
        {
            if (entityPM.ShipmentReceivables.Count == 0)
            {
                return;
            }

            foreach (ShipmentReceivablePM receivable in entityPM.ShipmentReceivables)
            {
                receivable.ChangeSetOp = this.GetChangeSet(receivable.ChangeSet);
            }
        }
        public ShipmentPM ValidateInlandDomesticShipment(ShipmentPM entityPM)
        {
            ValidateInlandDomesticShipmentFromTypeCode(entityPM);
            ValidateInlandDomesticShipmentToTypeCode(entityPM);
            entityPM = SetInlandDomesticShipmentFromPartners(entityPM);
            entityPM = SetInlandDomesticShipmentToPartners(entityPM);
            ValidateInlandDomesticMainCarriageDates(entityPM);

            return entityPM;
        }
        public void ValidateCustomsFields(ShipmentPM shipmentPM)
        {
            if (!this.IsAddingCustomsFields(shipmentPM))
            {
                return;
            }
            if (shipmentPM.CustomsClearanceDate != null)
            {
                shipmentPM.IncludesCustoms = true;
                return;
            }
            if (shipmentPM.DeclarationDate == null && !string.IsNullOrEmpty(shipmentPM.DeclarationNumber))
            {
                throw new ApplicationException("Declaration Date Field Is Required");
            }
            shipmentPM.IncludesCustoms = true;
        }
        public  void ValidateShipmentPackageDimensionsAndVolume(ShipmentPackagePM shipmentPackagePM, ShipmentPM entityPM)
        {
            if (IsOneOfTheDimensionsNotNull(shipmentPackagePM))
            {
                shipmentPackagePM.Volume = ComputeHelper.ComputeVolume(shipmentPackagePM, entityPM);
            }
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
                    if (shipmentPM.ShipmentReceivables.Where(p => p.TotalAmount != null && p.TotalAmount != 0 
                                                       && p.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).Any())
                    {
                        hasOpenReceivables = true;
                    }
                }

                if (accountingSetting != null && !accountingSetting.AllowClosureWithoutPayables)
                {
                    if (shipmentPM.ShipmentPayables.Count() > 0)
                    {
                        if (shipmentPM.ShipmentPayables.Where(p => p.ExpectedAmount != null && p.ExpectedAmount != 0 
                                                        && p.ChangeSetOp !=Simplog.Server.Infrastructure.ChangeSetOperation.Delete).Any())
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
            IInfrastructureContext context = InfrastructureContext.GetContext(tenant);
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
            if (shipmentPM.ShipmentPickUps == null)
                return false;

            if (shipmentPM.ShipmentPickUps.Count == 0)
                return false;

            return true;
        }
        private bool IsShipmentHasDelivery()
        {
            if (shipmentPM.ShipmentDeliveries == null)
                return false;

            if (shipmentPM.ShipmentDeliveries.Count == 0)
                return false;

            return true;
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
            if(shipmentPM.MainCarriageLegs.Count > 0)
            {
                var mainCarriageLeg = shipmentPM.MainCarriageLegs.FirstOrDefault();
                return mainCarriageLeg?.FromPortId;
            }

            if (!string.IsNullOrEmpty(shipmentPM.MainCarriageFromPortId))
                return shipmentPM.MainCarriageFromPortId; 

            return "";
        }
        private string GetMainCarriageToPortId()
        {
            if (shipmentPM.MainCarriageLegs.Count > 0)
            {
                var mainCarriageLeg = shipmentPM.MainCarriageLegs.LastOrDefault();
                return mainCarriageLeg?.ToPortId;
            }

            if (!string.IsNullOrEmpty(shipmentPM.MainCarriageToPortId))
                return shipmentPM.MainCarriageToPortId;

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

        private void MapPreCarriageVesselName()
        {
            if (string.IsNullOrEmpty(shipmentPM.PreCarriageToPortId) || string.IsNullOrEmpty(shipmentPM.PreCarriageFromPortId))
                return;

            if (string.IsNullOrEmpty(shipmentPM.PreCarriageVesselId))
                return;
            
            Vessel preCarriageVessel = this.vesselRepository.GetSingleVessel(shipmentPM.PreCarriageVesselId,tenant);
            if (preCarriageVessel == null)
                return;

            if (string.IsNullOrEmpty(preCarriageVessel.EnglishName))
                return;

            shipmentPM.PreCarriageVesselName = preCarriageVessel.EnglishName;
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

        private void MapOnCarriageVesselName()
        {
            if (string.IsNullOrEmpty(shipmentPM.OnCarriageFromPortId) || string.IsNullOrEmpty(shipmentPM.OnCarriageFromPortId))
                return;

            if (string.IsNullOrEmpty(shipmentPM.OnCarriageVesselId))
                return;

            Vessel onCarriageVessel = this.vesselRepository.GetSingleVessel(shipmentPM.OnCarriageVesselId, tenant);
            if (onCarriageVessel == null)
                return;

            if (string.IsNullOrEmpty(onCarriageVessel.EnglishName))
                return;

            shipmentPM.OnCarriageVesselName = onCarriageVessel.EnglishName;
        }

        private void UpdateDeliveryPackagesChangeSet(ShipmentPM entityPM)
        {
            if (entityPM.ShipmentDeliveries.Count == 0)
            {
                return;
            }

            foreach (ShipmentDeliveryPM shipmentDelivery in entityPM.ShipmentDeliveries)
            {
                shipmentDelivery.ChangeSetOp = this.GetChangeSet(shipmentDelivery.ChangeSet);

                foreach (ShipmentPickUpDeliveryPackagePM item in shipmentDelivery.ShipmentPickUpDeliveryPackages)
                {
                    item.ChangeSetOp = this.GetChangeSet(item.ChangeSet);
                }
            }
        }
        private void UpdatePickupPackagesChangeSet(ShipmentPM entityPM)
        {
            if (entityPM.ShipmentPickUps.Count == 0)
            {
                return;
            }

            foreach (ShipmentPickUpPM shipmentPickUp in entityPM.ShipmentPickUps)
            {
                shipmentPickUp.ChangeSetOp = this.GetChangeSet(shipmentPickUp.ChangeSet);

                foreach (ShipmentPickUpDeliveryPackagePM item in shipmentPickUp.ShipmentPickUpDeliveryPackages)
                {
                    item.ChangeSetOp = this.GetChangeSet(item.ChangeSet);
                }
            }
        }
        private void ValidateShipmentPackageItem(ShipmentPackagePM item, ShipmentPM entityPM)
        {
            if (item.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Delete)
                return;

            this.ValidateShipmentPackageDimensionsAndVolume(item, entityPM);

            if (!item.IsContainer)
            {
                if (item.InsideShipmentPackages != null && item.InsideShipmentPackages.Count > 0)
                {
                    throw new ApplicationException("Inside Packages allowed in FCL/FTL shipments only");
                }
            }
            else
            {
                this.ValidateInsidePackage(item, entityPM);
            }
            item.VolumetricWeight = ComputeHelper.ComputeVolumetricWeight(item, entityPM);
            this.MapCommodityName(item);
        }
        private void ValidateInsidePackage(ShipmentPackagePM item, ShipmentPM entityPM)
        {
            if (!(item.InsideShipmentPackages != null && item.InsideShipmentPackages.Count > 0))
            {
                return;
            }

            item.Weight = 0;
            item.Volume = 0;
            item.InsideShipmentPackages.ForEach(inside =>
            {
                if (inside.Weight != null)
                {
                    item.Weight += inside.Weight;
                }

                if (inside.Volume != null)
                {
                    item.Volume += inside.Volume;
                }

                if (inside.Quantity == null)
                {
                    throw new ApplicationException("Inside Packages Quantity is required");
                }

                inside.Volume = ComputeHelper.ComputeInsideVolume(inside, entityPM);
                inside.VolumetricWeight = ComputeHelper.ComputeInsideVolumetricWeight(inside, entityPM);
            });
        }
        private bool IsOneOfTheDimensionsNotNull(ShipmentPackagePM shipmentPackagePM)
        {
            if (shipmentPackagePM.Width != null)
            {
                return true;
            }
            if (shipmentPackagePM.Height != null)
            {
                return true;
            }
            if (shipmentPackagePM.Length != null)
            {
                return true;
            }
            return false;
        }
        private Simplog.Server.Infrastructure.ChangeSetOperation GetChangeSet(string ChangeSet)
        {
            Simplog.Server.Infrastructure.ChangeSetOperation changeSetOperation = Simplog.Server.Infrastructure.ChangeSetOperation.None;
            if (!string.IsNullOrEmpty(ChangeSet))
            {
                switch (ChangeSet.ToLower())
                {
                    case "insert":
                        {
                            changeSetOperation = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                            break;
                        }

                    case "update":
                        {
                            changeSetOperation = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                            break;
                        }

                    case "delete":
                        {
                            changeSetOperation = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                            break;
                        }
                }

            }
            else
            {
                changeSetOperation = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            }

            return changeSetOperation;
        }
        private void MapCommodityName(ShipmentPackagePM item)
        {
            if (string.IsNullOrEmpty(item.CommodityNumber) && string.IsNullOrEmpty(item.CommodityName))
                return;

            string commodityName = commodityRepository.GetSingleCommodityNameByCode(item.CommodityNumber, tenant);
            item.CommodityName = commodityName;
        } 
        private void ValidateInlandDomesticShipmentFromTypeCode(ShipmentPM entityPM)
        {
            bool isCityOrCountryNull = string.IsNullOrEmpty(entityPM.InlandDomesticFromCity) || string.IsNullOrEmpty(entityPM.InlandDomesticFromCountryId);
            string[] inlandDomesticFromTypeCodes = { "CASL", "PART", "PORT" };
            if (string.IsNullOrEmpty(entityPM.InlandDomesticFromTypeCode))
            {
                throw new ApplicationException("InlandDomesticFromTypeCode Field is Required");
            }
            if (entityPM.InlandDomesticFromTypeCode == "CASL" && isCityOrCountryNull)
            {
                throw new ApplicationException("InlandDomesticFrom City And Country Fields are Required");
            }
            else if (entityPM.InlandDomesticFromTypeCode == "PART" && (string.IsNullOrEmpty(entityPM.MainCarriageFromPartnerId)))
            {
                throw new ApplicationException("MainCarriageFromPartner Field is Required");
            }
            else if (entityPM.InlandDomesticFromTypeCode == "PORT" && string.IsNullOrEmpty(entityPM.MainCarriageFromPortId))
            {
                throw new ApplicationException("FromPort Field is Required");
            }
            else if (!inlandDomesticFromTypeCodes.Contains(entityPM.InlandDomesticFromTypeCode))
            {
                throw new ApplicationException("Invalid InlandDomesticFromTypeCode");
            }
        }
        private void ValidateInlandDomesticShipmentToTypeCode(ShipmentPM entityPM)
        {
            bool isCityOrCountryNull = string.IsNullOrEmpty(entityPM.InlandDomesticToCity) || string.IsNullOrEmpty(entityPM.InlandDomesticToCountryId);
            string[] inlandDomesticToTypeCodes = { "CASL", "PART", "PORT" };
            if (string.IsNullOrEmpty(entityPM.InlandDomesticToTypeCode))
            {
                throw new ApplicationException("InlandDomesticToTypeCode Field is Required");
            }
            if (entityPM.InlandDomesticToTypeCode == "CASL" && isCityOrCountryNull)
            {
                throw new ApplicationException("InlandDomestic To City And Country Fields are Required");
            }
            else if (entityPM.InlandDomesticToTypeCode == "PART" && string.IsNullOrEmpty(entityPM.MainCarriageToPartnerId))
            {
                throw new ApplicationException("MainCarriageToPartner Field is Required");
            }
            else if (entityPM.InlandDomesticToTypeCode == "PORT" && string.IsNullOrEmpty(entityPM.MainCarriageToPortId))
            {
                throw new ApplicationException("ToPort Field is Required");
            }
            else if (!inlandDomesticToTypeCodes.Contains(entityPM.InlandDomesticToTypeCode))
            {
                throw new ApplicationException("Invalid InlandDomesticToTypeCode");
            }
        }
        private ShipmentPM SetInlandDomesticShipmentFromPartners(ShipmentPM entityPM)
        {
            if (entityPM.InlandDomesticFromTypeCode == "CASL")
            {
                entityPM.MainCarriageFromPartnerId = null;
                entityPM.MainCarriageFromPortId = null;
            }
            else if (entityPM.InlandDomesticFromTypeCode == "PART")
            {
                entityPM.InlandDomesticFromCity = null;
                entityPM.InlandDomesticFromCountryId = null;
                entityPM.MainCarriageFromPortId = null;
            }
            else if (entityPM.InlandDomesticFromTypeCode == "PORT")
            {
                entityPM.InlandDomesticFromCity = null;
                entityPM.InlandDomesticFromCountryId = null;
                entityPM.MainCarriageFromPartnerId = null;
            }
            return entityPM;
        }
        private ShipmentPM SetInlandDomesticShipmentToPartners(ShipmentPM entityPM)
        {
            if (entityPM.InlandDomesticToTypeCode == "CASL")
            {
                entityPM.MainCarriageToPartnerId = null;
                entityPM.MainCarriageToPortId = null;
            }
            else if (entityPM.InlandDomesticToTypeCode == "PART")
            {
                entityPM.InlandDomesticToCity = null;
                entityPM.InlandDomesticToCountryId = null;
                entityPM.MainCarriageToPortId = null;
            }
            else if (entityPM.InlandDomesticToTypeCode == "PORT")
            {
                entityPM.InlandDomesticToCity = null;
                entityPM.InlandDomesticToCountryId = null;
                entityPM.MainCarriageToPartnerId = null;
            }
            return entityPM;
        }
        private void ValidateInlandDomesticMainCarriageDates(ShipmentPM entityPM)
        {
            if (!this.IsRoutingLegDatesValid(entityPM.MainCarriageETD, entityPM.MainCarriageETA))
            {
                throw new ApplicationException("Main-Carriage expected departure must be less than Main-Carriage expected arrival");
            }

            if (!this.IsRoutingLegDatesValid(entityPM.MainCarriageATD, entityPM.MainCarriageATA))
            {
                throw new ApplicationException("Main-Carriage actual departure must be less than Main-Carriage actual arrival");
            }

        }
        private bool IsAddingCustomsFields(ShipmentPM shipmentPM)
        {
            if (shipmentPM.CustomsClearanceDate != null)
            {
                return true;
            }
            if (shipmentPM.DeclarationDate != null)
            {
                return true;
            }
            if (!string.IsNullOrEmpty(shipmentPM.DeclarationNumber))
            {
                return true;
            }

            return false;
        }

    }
}