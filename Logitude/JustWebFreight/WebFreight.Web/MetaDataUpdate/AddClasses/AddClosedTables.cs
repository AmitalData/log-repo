using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.BookingLib.Data.Repositories;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.BL;
using Logitude.CRM.BL.CLoseTable;
using Logitude.Server.Tools.Counters;
using Logitude.Accounting.BL.CloseTables;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddClosedTables
    {
        public static void AddWeightUnits(WeightUnitDetails weightUnitDetails, WeightUnitRepository weightUnitRepository)
        {
            Dictionary<string, WeightUnit> tenantWeightUnits = weightUnitRepository.GetWeightUnits().ToDictionary(d => d.Code, a => a);

            if (tenantWeightUnits.Keys.Contains(weightUnitDetails.Code))
            {
                WeightUnit weightUnit = weightUnitRepository.GetSingleWeightUnit(weightUnitDetails.Code);
                weightUnit.Name = weightUnitDetails.Name;
                weightUnit.SearchFields = (weightUnitDetails.Code +","+ weightUnitDetails.Name).ToLower();
                weightUnitRepository.Update(weightUnit);
            }
            else
            {
                WeightUnit newWeightUnit = new WeightUnit() { Code = weightUnitDetails.Code, Name = weightUnitDetails.Name, SearchFields = (weightUnitDetails.Code + "," + weightUnitDetails.Name).ToLower() };
                weightUnitRepository.Add(newWeightUnit);
            }
        }

        public static void AddRateClasses(RateClassDetails rateClassDetails, RateClassRepository rateClassRepository)
        {
            Dictionary<string, RateClass> tenantRateClasses = rateClassRepository.GetRateClasses().ToDictionary(d => d.Code, a => a);

            if (tenantRateClasses.Keys.Contains(rateClassDetails.Code))
            {
                RateClass rateClass = rateClassRepository.GetSingleRateClass(rateClassDetails.Code);
                rateClass.Name = rateClassDetails.Name;
                rateClass.SearchFields = (rateClassDetails.Code + "," + rateClassDetails.Name).ToLower();
                rateClassRepository.Update(rateClass);
            }
            else
            {
                RateClass newRateClass = new RateClass() { Code = rateClassDetails.Code, Name = rateClassDetails.Name, SearchFields = (rateClassDetails.Code + "," + rateClassDetails.Name).ToLower() };
                rateClassRepository.Add(newRateClass);
            }
        }

        public static void AddPaymentMethods(PaymentMethodDetails paymentMethodDetails, PaymentMethodRepository paymentMethodRepository)
        {
            Dictionary<string, PaymentMethod> tenantPaymentMethods = paymentMethodRepository.GetPaymentMethods().ToDictionary(d => d.Code, a => a);

            if (tenantPaymentMethods.Keys.Contains(paymentMethodDetails.Code))
            {
                PaymentMethod paymentMethod = paymentMethodRepository.GetSinglePaymentMethod(paymentMethodDetails.Code);
                paymentMethod.Name = paymentMethodDetails.Name;
                paymentMethod.SearchFields = paymentMethodDetails.Code + "," + paymentMethodDetails.Name;
                paymentMethodRepository.Update(paymentMethod);
            }
            else
            {
                PaymentMethod paymentMethod = new PaymentMethod() { Code = paymentMethodDetails.Code, Name = paymentMethodDetails.Name, SearchFields = paymentMethodDetails.Code + "," + paymentMethodDetails.Name };
                paymentMethodRepository.Add(paymentMethod);
            }
        }

        public static void AddDimensionsUnits(DimensionsUnitDetails dimensionsUnitDetails, DimensionsUnitRepository dimensionsUnitRepository)
        {
            Dictionary<string, DimensionsUnit> tenantDimensionsUnits = dimensionsUnitRepository.GetDimensionsUnits().ToDictionary(d => d.Code, a => a);

            if (tenantDimensionsUnits.Keys.Contains(dimensionsUnitDetails.Code))
            {
                DimensionsUnit dimensionsUnit = dimensionsUnitRepository.GetSingleDimensionsUnit(dimensionsUnitDetails.Code);
                dimensionsUnit.Name = dimensionsUnitDetails.Name;
                dimensionsUnit.SearchFields = (dimensionsUnitDetails.Code + "," + dimensionsUnitDetails.Name).ToLower();
                dimensionsUnitRepository.Update(dimensionsUnit);
            }
            else
            {
                DimensionsUnit newDimensionsUnit = new DimensionsUnit() { Code = dimensionsUnitDetails.Code, Name = dimensionsUnitDetails.Name, SearchFields = (dimensionsUnitDetails.Code + "," + dimensionsUnitDetails.Name).ToLower() };
                dimensionsUnitRepository.Add(newDimensionsUnit);
            }
        }

        public static void AddDueTypes(DueTypeDetails dueTypeDetails, DueTypeRepository dueTypeRepository)
        {
            Dictionary<string, DueType> tenantDueTypes = dueTypeRepository.GetDueTypes().ToDictionary(d => d.Code, a => a);

            if (tenantDueTypes.Keys.Contains(dueTypeDetails.Code))
            {
                DueType dueType = dueTypeRepository.GetSingleDueTypeUpdate(dueTypeDetails.Code, 0);
                dueType.Name = dueTypeDetails.Name;
                dueType.SearchFields = (dueTypeDetails.Code + "," + dueTypeDetails.Name).ToLower();
                dueTypeRepository.Update(dueType);
            }
            else
            {
                DueType newDueType = new DueType() { Code = dueTypeDetails.Code, Name = dueTypeDetails.Name, SearchFields = (dueTypeDetails.Code + "," + dueTypeDetails.Name).ToLower() };
                dueTypeRepository.Add(newDueType);
            }
        }
        public static void AddQuoteGroupSection(QuoteGroupSectionDetails quoteGroupSectionDetails, QuoteGroupSectionRepository quoteGroupSectionRepository)
        {
            Dictionary<string, QuoteGroupSection> tenantQuoteGroupSections = quoteGroupSectionRepository.GetQuoteGroupSections().ToDictionary(d => d.Code, a => a);

            if (tenantQuoteGroupSections.Keys.Contains(quoteGroupSectionDetails.Code))
            {
                QuoteGroupSection quoteGroupSection = quoteGroupSectionRepository.GetSingleQuoteGroupSection(quoteGroupSectionDetails.Code, 0);
                quoteGroupSection.Name = quoteGroupSectionDetails.Name;
                quoteGroupSection.Code = quoteGroupSectionDetails.Code;
                quoteGroupSection.Searchfields = quoteGroupSectionDetails.Searchfields;
                quoteGroupSectionRepository.Update(quoteGroupSection);
            }
            else
            {
                QuoteGroupSection quoteGroupSection = new QuoteGroupSection() {  Code = quoteGroupSectionDetails.Code, Name = quoteGroupSectionDetails.Name, Searchfields = quoteGroupSectionDetails.Searchfields };
                quoteGroupSectionRepository.Add(quoteGroupSection);
            }
        }
        public static void AddTransportModes(TransportModeDetails transportModeDetails, TransportModeRepository transportModeRepository)
        {
            Dictionary<string, TransportMode> tenantTransportModes = transportModeRepository.GetTransportModes().ToDictionary(d => d.Id, a => a);

            if (tenantTransportModes.Keys.Contains(transportModeDetails.Id))
            {
                TransportMode transportMode = transportModeRepository.GetSingleTransportMode(transportModeDetails.Id);
                transportMode.Name = transportModeDetails.Name;
                transportMode.SearchFields = (transportModeDetails.Id + "," + transportModeDetails.Name).ToLower();
                transportModeRepository.Update(transportMode);
            }
            else
            {
                TransportMode newTransportMode = new TransportMode() { Id = transportModeDetails.Id, Name = transportModeDetails.Name, SearchFields = (transportModeDetails.Id + "," + transportModeDetails.Name).ToLower() };
                transportModeRepository.Add(newTransportMode);
            }
        }

        public static void AddDirections(DirectionDetails directionDetails, DirectionRepository directionRepository)
        {
            Dictionary<string, Direction> tenantDirections = directionRepository.GetDirections().ToDictionary(d => d.Id, a => a);

            if (tenantDirections.Keys.Contains(directionDetails.Id))
            {
                Direction direction = directionRepository.GetSingleDirection(directionDetails.Id);
                direction.Name = directionDetails.Name;
                direction.SearchFields = (directionDetails.Id + "," + directionDetails.Name).ToLower();
                directionRepository.Update(direction);
            }
            else
            {
                Direction newDirection = new Direction() { Id = directionDetails.Id, Name = directionDetails.Name, SearchFields = (directionDetails.Id + "," + directionDetails.Name).ToLower() };
                directionRepository.Add(newDirection);
            }
        }

        public static void AddPrepaidCollects(PrepaidCollectDetails prepaidCollectDetails, PrepaidCollectRepository prepaidCollectRepository)
        {
            Dictionary<string, PrepaidCollect> tenantPrepaidCollects = prepaidCollectRepository.GetPrepaidCollects().ToDictionary(d => d.Id, a => a);

            if (tenantPrepaidCollects.Keys.Contains(prepaidCollectDetails.Id))
            {
                PrepaidCollect prepaidCollect = prepaidCollectRepository.GetSinglePrepaidCollect(prepaidCollectDetails.Id);
                prepaidCollect.Name = prepaidCollectDetails.Name;
                prepaidCollect.DisplayInLOV = prepaidCollectDetails.DisplayInLOV;
                prepaidCollect.SearchFields = (prepaidCollectDetails.Name +","+ prepaidCollectDetails.Id).ToLower(); // Id = code 
                prepaidCollectRepository.Update(prepaidCollect);
            }
            else
            {
                PrepaidCollect newPrepaidCollect = new PrepaidCollect() { Id = prepaidCollectDetails.Id, Name = prepaidCollectDetails.Name, DisplayInLOV = prepaidCollectDetails.DisplayInLOV, SearchFields = (prepaidCollectDetails.Name + "," + prepaidCollectDetails.Id).ToLower() };
                prepaidCollectRepository.Add(newPrepaidCollect);
            }
        }

        public static void AddShipmentTypes(ShipmentTypeDetails shipmentTypeDetails, ShipmentTypeRepository shipmentTypeRepository)
        {
            IQueryable<ShipmentType> shpLIst = shipmentTypeRepository.GetShipmentTypes();
            Dictionary<string, ShipmentType> tenantShipmentTypes = shpLIst.ToDictionary(d => d.Id, a => a);//shipmentTypeRepository.GetShipmentTypes().ToDictionary(d => d.Id, a => a);
            //Dictionary<string, ShipmentType> tenantShipmentTypes = shipmentTypeRepository.GetShipmentTypes().ToDictionary(d => d.Id, a => a);

            if (tenantShipmentTypes.Keys.Contains(shipmentTypeDetails.Id))
            {
                ShipmentType shipmentType = shipmentTypeRepository.GetSingleShipmentType(shipmentTypeDetails.Id);
                shipmentType.Name = shipmentTypeDetails.Name;
                shipmentType.TransportModeId = shipmentTypeDetails.TransportModeId;
                shipmentType.SearchFields = (shipmentTypeDetails.Id +","+ shipmentTypeDetails.Name).ToLower();
                shipmentTypeRepository.Update(shipmentType);
            }
            else
            {
                ShipmentType newShipmentType = new ShipmentType() { Id = shipmentTypeDetails.Id, Name = shipmentTypeDetails.Name, TransportModeId = shipmentTypeDetails.TransportModeId, SearchFields = (shipmentTypeDetails.Id + "," + shipmentTypeDetails.Name).ToLower() };
                shipmentTypeRepository.Add(newShipmentType);
            }
        }

        public static void AddPartnerTypes(PartnerTypeDetails partnerTypeDetails, PartnerTypeRepository partnerTypeRepository)
        {
            Dictionary<string, PartnerType> tenantPartnerTypes = partnerTypeRepository.GetPartnerTypes().ToDictionary(d => d.Id, a => a);

            if (tenantPartnerTypes.Keys.Contains(partnerTypeDetails.Id))
            {
                PartnerType partnerType = partnerTypeRepository.GetSinglePartnerType(partnerTypeDetails.Id);
                partnerType.Name = partnerTypeDetails.Name;
                partnerType.SearchFields = (partnerTypeDetails.Id + "," + partnerTypeDetails.Name).ToLower();
                partnerTypeRepository.Update(partnerType);
            }
            else
            {
                PartnerType newPartnerType = new PartnerType() { Id = partnerTypeDetails.Id, Name = partnerTypeDetails.Name, SearchFields = (partnerTypeDetails.Id + "," + partnerTypeDetails.Name).ToLower() };
                partnerTypeRepository.Add(newPartnerType);
            }
        }

        public static void AddAddressTypes(AddressTypeDetails addressTypeDetails, Simplog.Data.CommonDataModel.Repositories.AddressTypeRepository addressTypeRepository)
        {
            Dictionary<string, Simplog.Data.CommonDataModel.EntityPOCOs.AddressType> tenantAddressTypes = addressTypeRepository.GetAddressTypes().ToDictionary(d => d.Id, a => a);

            if (tenantAddressTypes.Keys.Contains(addressTypeDetails.Id))
            {
                Simplog.Data.CommonDataModel.EntityPOCOs.AddressType addressType = addressTypeRepository.GetSingleAddressType(addressTypeDetails.Id);
                addressType.Name = addressTypeDetails.Name;
                addressType.SearchFields = (addressTypeDetails.Id + "," + addressTypeDetails.Name).ToLower();
                addressTypeRepository.Update(addressType);
            }
            else
            {
                Simplog.Data.CommonDataModel.EntityPOCOs.AddressType newAddressType = new Simplog.Data.CommonDataModel.EntityPOCOs.AddressType() { Id = addressTypeDetails.Id, Name = addressTypeDetails.Name, SearchFields = (addressTypeDetails.Id + "," + addressTypeDetails.Name).ToLower() };
                addressTypeRepository.Add(newAddressType);
            }
        }

        public static void AddPickUpDeliveryTypes(PickUpDeliveryTypeDetails pickUpDeliveryTypeDetails, PickUpDeliveryTypeRepository pickUpDeliveryTypeRepository)
        {
            Dictionary<string, PickUpDeliveryType> tenantFixedAmounts = pickUpDeliveryTypeRepository.GetPickUpDeliveryTypes().ToDictionary(d => d.Code, a => a);

            if (tenantFixedAmounts.Keys.Contains(pickUpDeliveryTypeDetails.Code))
            {
                PickUpDeliveryType pickUpDeliveryType = pickUpDeliveryTypeRepository.GetSinglePickUpDeliveryType(pickUpDeliveryTypeDetails.Code);
                pickUpDeliveryType.Name = pickUpDeliveryTypeDetails.Name;
                pickUpDeliveryTypeRepository.Update(pickUpDeliveryType);
            }
            else
            {
                PickUpDeliveryType newPickUpDeliveryType = new PickUpDeliveryType() { Code = pickUpDeliveryTypeDetails.Code, Name = pickUpDeliveryTypeDetails.Name, };
                pickUpDeliveryTypeRepository.Add(newPickUpDeliveryType);
            }
        }

        public static void AddPickUpDeliveryFromToTypes(PickUpDeliveryFromToTypeDetails pickUpDeliveryFromToTypeDetails, PickUpDeliveryFromToTypeRepository pickUpDeliveryFromToTypeRepository)
        {
            if (pickUpDeliveryFromToTypeRepository == null)
            {
                throw new ArgumentNullException("pickUpDeliveryFromToTypeRepository");
            }
            Dictionary<string, PickUpDeliveryFromToType> tenantFixedAmounts = pickUpDeliveryFromToTypeRepository.GetPickUpDeliveryFromToTypes().ToDictionary(d => d.Code, a => a);

            if (tenantFixedAmounts.Keys.Contains(pickUpDeliveryFromToTypeDetails.Code))
            {
                PickUpDeliveryFromToType pickUpDeliveryFromToType = pickUpDeliveryFromToTypeRepository.GetSinglePickUpDeliveryFromToType(pickUpDeliveryFromToTypeDetails.Code);
                pickUpDeliveryFromToType.Name = pickUpDeliveryFromToTypeDetails.Name;
                pickUpDeliveryFromToTypeRepository.Update(pickUpDeliveryFromToType);
            }
            else
            {
                PickUpDeliveryFromToType newPickUpDeliveryFromToType = new PickUpDeliveryFromToType() { Code = pickUpDeliveryFromToTypeDetails.Code, Name = pickUpDeliveryFromToTypeDetails.Name, };
                pickUpDeliveryFromToTypeRepository.Add(newPickUpDeliveryFromToType);
            }
        }

        public static void AddEntityDates(EntityDateDetails entityDateDetails, EntityDateRepository entityDateRepository)
        {
            Dictionary<string, EntityDate> tenantEntityDates = entityDateRepository.GetEntityDates().ToDictionary(d => d.Id, a => a);

            if (tenantEntityDates.Keys.Contains(entityDateDetails.Id))
            {
                EntityDate entityDate = entityDateRepository.GetSingleEntityDate(entityDateDetails.Id);
                entityDate.Name = entityDateDetails.Name;
                entityDate.SearchFields = (entityDateDetails.Id + "," + entityDateDetails.Name).ToLower();
                entityDateRepository.Update(entityDate);
            }
            else
            {
                EntityDate newEntityDate = new EntityDate() { Id = entityDateDetails.Id, Name = entityDateDetails.Name, SearchFields = (entityDateDetails.Id + "," + entityDateDetails.Name).ToLower() };
                entityDateRepository.Add(newEntityDate);
            }
        }

        public static void AddTextCodeTypes(TextCodeTypeDetails textCodeTypeDetails, TextCodeTypesRepository textCodeTypeRepository)
        {
            Dictionary<string, TextCodeType> tenantTextCodeTypes = textCodeTypeRepository.GetTextCodeTypes().ToDictionary(d => d.Code, a => a);

            if (tenantTextCodeTypes.Keys.Contains(textCodeTypeDetails.Code))
            {
                TextCodeType textCodeType = textCodeTypeRepository.GetSingleTextCodeType(textCodeTypeDetails.Code);
                textCodeType.Name = textCodeTypeDetails.Name;
                textCodeTypeRepository.Update(textCodeType);
            }
            else
            {
                TextCodeType newTextCodeType = new TextCodeType() { Code = textCodeTypeDetails.Code, Name = textCodeTypeDetails.Name, };
                textCodeTypeRepository.Add(newTextCodeType);
            }
        }

        public static void AddFieldDataTypes(FieldDataTypeDetails fieldDataTypeDetails, FieldDataTypesRepository fieldDataTypeRepository)
        {
            Dictionary<string, FieldDataType> tenantFieldDataTypes = fieldDataTypeRepository.GetFieldDataTypes().ToDictionary(d => d.Code, a => a);

            if (tenantFieldDataTypes.Keys.Contains(fieldDataTypeDetails.Code))
            {
                FieldDataType fieldDataType = fieldDataTypeRepository.GetSingleFieldDataType(fieldDataTypeDetails.Code);
                fieldDataType.Name = fieldDataTypeDetails.Name;
                fieldDataType.SearchFields = (fieldDataTypeDetails.Code + "," + fieldDataTypeDetails.Name).ToLower();
                fieldDataTypeRepository.Update(fieldDataType);
            }
            else
            {
                FieldDataType newFieldDataType = new FieldDataType() { Code = fieldDataTypeDetails.Code, Name = fieldDataTypeDetails.Name, SearchFields = (fieldDataTypeDetails.Code + "," + fieldDataTypeDetails.Name).ToLower() };
                fieldDataTypeRepository.Add(newFieldDataType);
            }
        }

        public static void AddChargesGroups(ChargesGroupDetails chargesGroupDetails, ChargesGroupRepository chargesGroupRepository)
        {
            //var allChargesGroups0 = chargesGroupRepository.GetChargesGroups(0).ToList();
            //Dictionary<string, ChargesGroup> tenantChargesGroups = allChargesGroups0.ToDictionary(d => d.Code, a => a);

            //if (tenantChargesGroups.Keys.Contains(chargesGroupDetails.Code))
            //{
            //    ChargesGroup chargesGroup = chargesGroupRepository.GetSingleChargesGroupByCode(chargesGroupDetails.Code, 0);
            //    chargesGroup.Name = chargesGroupDetails.Name;
            //    chargesGroup.ViewOrder = chargesGroupDetails.ViewOrder;
            //    chargesGroup.SearchFields = (chargesGroupDetails.Code + "," + chargesGroupDetails.Name).ToLower();
            //    chargesGroupRepository.Update(chargesGroup);
            //}
            //else
            //{
            //    ChargesGroup newChargesGroup = new ChargesGroup() { Code = chargesGroupDetails.Code, Name = chargesGroupDetails.Name,ViewOrder = chargesGroupDetails.ViewOrder, SearchFields = (chargesGroupDetails.Code + "," + chargesGroupDetails.Name).ToLower() };
            //    newChargesGroup.Id = IdCounter.GetNumber("ChargesGroup", 0);
            //    newChargesGroup.Tenant = 0;
            //    chargesGroupRepository.Add(newChargesGroup);
            //}
        }
        
        public static void AddVolumeUnits(VolumeUnitDetails volumeUnitDetails, VolumeUnitRepository volumeUnitRepository)
        {
            Dictionary<string, VolumeUnit> tenantVolumeUnits = volumeUnitRepository.GetVolumeUnits().ToDictionary(d => d.Code, a => a);

            if (tenantVolumeUnits.Keys.Contains(volumeUnitDetails.Code))
            {
                VolumeUnit volumeUnit = volumeUnitRepository.GetSingleVolumeUnit(volumeUnitDetails.Code);
                volumeUnit.Name = volumeUnitDetails.Name;
                volumeUnit.SearchFields = (volumeUnitDetails.Code + "," + volumeUnitDetails.Name).ToLower();
                volumeUnitRepository.Update(volumeUnit);
            }
            else
            {
                VolumeUnit newVolumeUnit = new VolumeUnit() { Code = volumeUnitDetails.Code, Name = volumeUnitDetails.Name, SearchFields = (volumeUnitDetails.Code + "," + volumeUnitDetails.Name).ToLower() };
                volumeUnitRepository.Add(newVolumeUnit);
            }
        }

        public static void AddShipmentReceivableLineStatus(ShipmentReceivableLineStatusDetails shipmentReceivableLineStatusDetails, ShipmentReceivableLineStatusRepository shipmentReceivableLineStatusRepository)
        {
            Dictionary<string, ShipmentReceivableLineStatus> tenantShipmentReceivableLineStatus = shipmentReceivableLineStatusRepository.GetShipmentReceivableLineStatus().ToDictionary(d => d.Code, a => a);

            if (tenantShipmentReceivableLineStatus.Keys.Contains(shipmentReceivableLineStatusDetails.Code))
            {
                ShipmentReceivableLineStatus shipmentReceivableLineStatus = shipmentReceivableLineStatusRepository.GetSingleShipmentReceivableLineStatusByCode(shipmentReceivableLineStatusDetails.Code);
                shipmentReceivableLineStatus.Name = shipmentReceivableLineStatusDetails.Name;
                shipmentReceivableLineStatus.SearchFields = (shipmentReceivableLineStatusDetails.Code + "," + shipmentReceivableLineStatusDetails.Name).ToLower();
                shipmentReceivableLineStatusRepository.Update(shipmentReceivableLineStatus);
            }
            else
            {
                ShipmentReceivableLineStatus newShipmentReceivableLineStatus = new ShipmentReceivableLineStatus() { Code = shipmentReceivableLineStatusDetails.Code, Name = shipmentReceivableLineStatusDetails.Name, SearchFields = (shipmentReceivableLineStatusDetails.Code + "," + shipmentReceivableLineStatusDetails.Name).ToLower() };
                shipmentReceivableLineStatusRepository.Add(newShipmentReceivableLineStatus);
            }
        }

        public static void AddShipmentPayableLineStatus(ShipmentPayableLineStatusDetails shipmentPayableLineStatusDetails, ShipmentPayableLineStatusRepository shipmentPayableLineStatusRepository)
        {
            Dictionary<string, ShipmentPayableLineStatus> tenantShipmentPayableLineStatus = shipmentPayableLineStatusRepository.GetPayableStatusTypes().ToDictionary(d => d.Code, a => a);

            if (tenantShipmentPayableLineStatus.Keys.Contains(shipmentPayableLineStatusDetails.Code))
            {
                ShipmentPayableLineStatus shipmentPayableLineStatus = shipmentPayableLineStatusRepository.GetSinglePayableStatusTypeByCode(shipmentPayableLineStatusDetails.Code);
                shipmentPayableLineStatus.Name = shipmentPayableLineStatusDetails.Name;
                shipmentPayableLineStatus.SearchFields = shipmentPayableLineStatusDetails.Code + "," + shipmentPayableLineStatusDetails.Name;
                shipmentPayableLineStatusRepository.Update(shipmentPayableLineStatus);
            }
            else
            {
                ShipmentPayableLineStatus newShipmentPayableLineStatus = new ShipmentPayableLineStatus() { Code = shipmentPayableLineStatusDetails.Code, Name = shipmentPayableLineStatusDetails.Name, SearchFields = (shipmentPayableLineStatusDetails.Code + "," + shipmentPayableLineStatusDetails.Name).ToLower() };
                shipmentPayableLineStatusRepository.Add(newShipmentPayableLineStatus);
            }
        }

        public static void AddShipmentPayableStatus(ShipmentPayableStatusDetails shipmentPayableStatusDetails, ShipmentPayableStatusRepository shipmentPayableStatusRepository)
        {
            Dictionary<string, ShipmentPayableStatus> tenantShipmentPayableStatus = shipmentPayableStatusRepository.GetStatusTypes().ToDictionary(d => d.Code, a => a);

            if (tenantShipmentPayableStatus.Keys.Contains(shipmentPayableStatusDetails.Code))
            {
                ShipmentPayableStatus shipmentPayableStatus = shipmentPayableStatusRepository.GetSingleShipmentPayableStatus(shipmentPayableStatusDetails.Code);
                shipmentPayableStatus.Name = shipmentPayableStatusDetails.Name;
                shipmentPayableStatus.SearchFields = shipmentPayableStatusDetails.Code + "," + shipmentPayableStatusDetails.Name;
                shipmentPayableStatusRepository.Update(shipmentPayableStatus);
            }
            else
            {
                ShipmentPayableStatus newShipmentPayableStatus = new ShipmentPayableStatus() { Code = shipmentPayableStatusDetails.Code, Name = shipmentPayableStatusDetails.Name, SearchFields = (shipmentPayableStatusDetails.Code + "," + shipmentPayableStatusDetails.Name).ToLower() };
                shipmentPayableStatusRepository.Add(newShipmentPayableStatus);
            }
        }

        public static void AddShipmentReceivableStatus(ShipmentReceivableStatusDetails shipmentReceivableStatusDetails, ShipmentReceivableStatusRepository shipmentReceivableStatusRepository)
        {
            Dictionary<string, ShipmentReceivableStatus> tenantShipmentReceivableStatus = shipmentReceivableStatusRepository.GetStatusTypes().ToDictionary(d => d.Code, a => a);

            if (tenantShipmentReceivableStatus.Keys.Contains(shipmentReceivableStatusDetails.Code))
            {
                ShipmentReceivableStatus shipmentReceivableStatus = shipmentReceivableStatusRepository.GetSingleShipmentReceivableStatus(shipmentReceivableStatusDetails.Code);
                shipmentReceivableStatus.Name = shipmentReceivableStatusDetails.Name;
                shipmentReceivableStatus.SearchFields = shipmentReceivableStatusDetails.Code + "," + shipmentReceivableStatusDetails.Name;
                shipmentReceivableStatusRepository.Update(shipmentReceivableStatus);
            }
            else
            {
                ShipmentReceivableStatus newShipmentReceivableStatus = new ShipmentReceivableStatus() { Code = shipmentReceivableStatusDetails.Code, Name = shipmentReceivableStatusDetails.Name, SearchFields = (shipmentReceivableStatusDetails.Code + "," + shipmentReceivableStatusDetails.Name).ToLower() };
                shipmentReceivableStatusRepository.Add(newShipmentReceivableStatus);
            }
        }

        public static void AddQuoteType(QuoteTypeDetails quoteTypeDetails, QuoteTypeRepository quoteTypeRepository)
        {
            Dictionary<string, QuoteType> tenantQuoteType = quoteTypeRepository.GetQuoteTypes().ToDictionary(d => d.Code, a => a);

            if (tenantQuoteType.Keys.Contains(quoteTypeDetails.Code))
            {
                QuoteType quoteType = quoteTypeRepository.GetSingleQuoteType(quoteTypeDetails.Code);
                quoteType.Name = quoteTypeDetails.Name;
                quoteType.SearchFields = quoteTypeDetails.Code + "," + quoteTypeDetails.Name;
                quoteTypeRepository.Update(quoteType);
            }
            else
            {
                QuoteType newQuoteType = new QuoteType() { Code = quoteTypeDetails.Code, Name = quoteTypeDetails.Name, SearchFields = (quoteTypeDetails.Code + "," + quoteTypeDetails.Name).ToLower() };
                quoteTypeRepository.Add(newQuoteType);
            }
        }

        public static void AddMarkUpType(MarkUpTypeDetails markUpTypeDetails, MarkUpTypeRepository markUpTypeRepository)
        {
            Dictionary<string, MarkUpType> tenantMarkUpType = markUpTypeRepository.GetMarkUpTypes().ToDictionary(d => d.Code, a => a);

            if (tenantMarkUpType.Keys.Contains(markUpTypeDetails.Code))
            {
                MarkUpType markUpType = markUpTypeRepository.GetSingleMarkUpType(markUpTypeDetails.Code);
                markUpType.Name = markUpTypeDetails.Name;
                markUpType.SearchFields = markUpTypeDetails.Code + "," + markUpTypeDetails.Name;
                markUpTypeRepository.Update(markUpType);
            }
            else
            {
                MarkUpType newMarkUpType = new MarkUpType() { Code = markUpTypeDetails.Code, Name = markUpTypeDetails.Name, SearchFields = (markUpTypeDetails.Code + "," + markUpTypeDetails.Name).ToLower() };
                markUpTypeRepository.Add(newMarkUpType);
            }
        }

        public static void AddInvoiceType(InvoiceTypeDetails invoiceTypeDetails, ARInvoiceTypeRepository invoiceTypeRepository)
        {
            Dictionary<string, ARInvoiceType> tenantInvoiceType = invoiceTypeRepository.GetARInvoiceTypes().ToDictionary(d => d.Code, a => a);

            if (tenantInvoiceType.Keys.Contains(invoiceTypeDetails.Code))
            {
                ARInvoiceType invoiceType = invoiceTypeRepository.GetSingleARInvoiceType(invoiceTypeDetails.Code);
                invoiceType.Name = invoiceTypeDetails.Name;
                invoiceType.SearchFields = invoiceTypeDetails.Code + "," + invoiceTypeDetails.Name;
                invoiceTypeRepository.Update(invoiceType);
            }
            else
            {
                ARInvoiceType newInvoiceType = new ARInvoiceType() { Code = invoiceTypeDetails.Code, Name = invoiceTypeDetails.Name, SearchFields = (invoiceTypeDetails.Code + "," + invoiceTypeDetails.Name).ToLower() };
                invoiceTypeRepository.Add(newInvoiceType);
            }
        }

        public static void AddInvoiceStatus(InvoiceStatusDetails invoiceStatusDetails, ARInvoiceStatusRepository invoiceStatusRepository)
        {
            Dictionary<string, ARInvoiceStatus> tenantInvoiceStatus = invoiceStatusRepository.GetARInvoiceStatus().ToDictionary(d => d.Code, a => a);

            if (tenantInvoiceStatus.Keys.Contains(invoiceStatusDetails.Code))
            {
                ARInvoiceStatus invoiceStatus = invoiceStatusRepository.GetSingleARInvoiceStatus(invoiceStatusDetails.Code);
                invoiceStatus.Name = invoiceStatusDetails.Name;
                invoiceStatus.SearchFields = invoiceStatusDetails.Code + "," + invoiceStatusDetails.Name;
                invoiceStatusRepository.Update(invoiceStatus);
            }
            else
            {
                ARInvoiceStatus newInvoiceStatus = new ARInvoiceStatus() { Code = invoiceStatusDetails.Code, Name = invoiceStatusDetails.Name, SearchFields = (invoiceStatusDetails.Code + "," + invoiceStatusDetails.Name).ToLower() };
                invoiceStatusRepository.Add(newInvoiceStatus);
            }
        }

        public static void AddARInvoiceTransferStatus(ARInvoiceTransferStatusDetails detailsClass, ARInvoiceTransferStatusRepository repository)
        {
            Dictionary<string, ARInvoiceTransferStatus> dictionary = repository.GetARInvoiceTransferStatus().ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(detailsClass.Code))
            {
                ARInvoiceTransferStatus entity = repository.GetSingleARInvoiceTransferStatus(detailsClass.Code);
                entity.Name = detailsClass.Name;
                entity.SearchFields = (detailsClass.Code + "," + detailsClass.Name).ToLower();
                repository.Update(entity);
            }

            else
            {
                ARInvoiceTransferStatus newEntity = new ARInvoiceTransferStatus() { Code = detailsClass.Code, Name = detailsClass.Name, SearchFields =( detailsClass.Code + "," + detailsClass.Name).ToLower() };
                repository.Add(newEntity);
            }
        }

        public static void AddAPInvoiceTransferStatus(APInvoiceTransferStatusDetails detailsClass, APInvoiceTransferStatusRepository repository)
        {
            Dictionary<string, APInvoiceTransferStatus> dictionary = repository.GetAPInvoiceTransferStatus().ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(detailsClass.Code))
            {
                APInvoiceTransferStatus entity = repository.GetSingleAPInvoiceTransferStatus(detailsClass.Code);
                entity.Name = detailsClass.Name;
                entity.SearchFields = (detailsClass.Code + "," + detailsClass.Name).ToLower();
                repository.Update(entity);
            }

            else
            {
                APInvoiceTransferStatus newEntity = new APInvoiceTransferStatus()
                {
                    Code = detailsClass.Code,
                    Name = detailsClass.Name,
                    SearchFields = (detailsClass.Code + "," + detailsClass.Name).ToLower()
                };

                repository.Add(newEntity);
            }
        }

        public static void AddMenuType(MenuTypeDetails menuTypeDetails, MenuTypeRepository menuTypeRepository)
        {
            Dictionary<string, MenuType> tenantMenuTypes = menuTypeRepository.GetMenuTypes().ToDictionary(d => d.Code, a => a);

            if (tenantMenuTypes.Keys.Contains(menuTypeDetails.Code))
            {
                MenuType menuType = menuTypeRepository.GetSingleMenuType(menuTypeDetails.Code);
                menuType.Name = menuTypeDetails.Name;
                menuTypeRepository.Update(menuType);
            }
            else
            {
                MenuType newMenuType = new MenuType() { Code = menuTypeDetails.Code, Name = menuTypeDetails.Name, };
                menuTypeRepository.Add(newMenuType);
            }
        }

        public static void AddCategoryType(CategoryTypeDetails categoryTypeDetails, CategoryTypeRepository categoryTypeRepository)
        {
            Dictionary<string, CategoryType> tenantCategoryTypes = categoryTypeRepository.GetCategoryTypes().ToDictionary(d => d.Code, a => a);

            if (tenantCategoryTypes.Keys.Contains(categoryTypeDetails.Code))
            {
                CategoryType categoryType = categoryTypeRepository.GetSingleCategoryType(categoryTypeDetails.Code);
                categoryType.Name = categoryTypeDetails.Name;
                categoryType.SearchFields = (categoryTypeDetails.Code + "," + categoryTypeDetails.Name).ToLower();
                categoryTypeRepository.Update(categoryType);
            }
            else
            {
                CategoryType newCategoryType = new CategoryType() { Code = categoryTypeDetails.Code, Name = categoryTypeDetails.Name, SearchFields = (categoryTypeDetails.Code + "," + categoryTypeDetails.Name).ToLower() };
                categoryTypeRepository.Add(newCategoryType);
            }
        }

        public static void AddTemplateFormat(TemplateFormatDetails templateFormatDetails, TemplateFormatRepository templateFormatRepository)
        {
            Dictionary<string, TemplateFormat> tenantTemplateFormats = templateFormatRepository.GetTemplateFormats().ToDictionary(d => d.Code, a => a);

            if (tenantTemplateFormats.Keys.Contains(templateFormatDetails.Code))
            {
                TemplateFormat templateFormat = templateFormatRepository.GetSingleTemplateFormat(templateFormatDetails.Code);
                templateFormat.Name = templateFormatDetails.Name;
                templateFormat.SearchFields = (templateFormatDetails.Code + "," + templateFormatDetails.Name).ToLower();
                templateFormatRepository.Update(templateFormat);
            }
            else
            {
                TemplateFormat newTemplateFormat = new TemplateFormat() { Code = templateFormatDetails.Code, Name = templateFormatDetails.Name, SearchFields = (templateFormatDetails.Code + "," + templateFormatDetails.Name).ToLower() };
                templateFormatRepository.Add(newTemplateFormat);
            }
        }

        public static void AddRuleType(RuleTypeDetails ruleTypeDetails, RuleTypeRepository ruleTypeRepository)
        {
            Dictionary<string, RuleType> tenantRuleTypes = ruleTypeRepository.GetRuleTypes().ToDictionary(d => d.Code, a => a);

            if (tenantRuleTypes.Keys.Contains(ruleTypeDetails.Code))
            {
                RuleType ruleType = ruleTypeRepository.GetRuleTypeByCode(ruleTypeDetails.Code);
                ruleType.Name = ruleTypeDetails.Name;
                ruleTypeRepository.Update(ruleType);
            }
            else
            {
                RuleType newRuleType = new RuleType() { Code = ruleTypeDetails.Code, Name = ruleTypeDetails.Name, };
                ruleTypeRepository.Add(newRuleType);
            }
        }

        public static void AddTriggerType(TriggerTypeDetails triggerTypeDetails, TriggerTypeRepository triggerTypeRepository)
        {
            Dictionary<string, TriggerType> tenantTriggerTypes = triggerTypeRepository.GetTriggerTypes().ToDictionary(d => d.Code, a => a);

            if (tenantTriggerTypes.Keys.Contains(triggerTypeDetails.Code))
            {
                TriggerType triggerType = triggerTypeRepository.GetTriggerTypeByCode(triggerTypeDetails.Code);
                triggerType.Name = triggerTypeDetails.Name;
                triggerType.SearchFields = (triggerTypeDetails.Code + "," + triggerTypeDetails.Name).ToLower();
                triggerTypeRepository.Update(triggerType);
            }
            else
            {
                TriggerType newTriggerType = new TriggerType() { Code = triggerTypeDetails.Code, Name = triggerTypeDetails.Name, SearchFields = triggerTypeDetails.Code + "," + triggerTypeDetails.Name };
                triggerTypeRepository.Add(newTriggerType);
            }
        }

        public static void AddRuleNotificationType(RuleNotificationTypeDetails ruleNotificationTypeDetails, RuleNotificationTypeRepository ruleNotificationTypeRepository)
        {
            Dictionary<string, RuleNotificationType> tenantRuleNotificationType = ruleNotificationTypeRepository.GetRuleNotificationTypes().ToDictionary(d => d.Code, a => a);

            if (tenantRuleNotificationType.Keys.Contains(ruleNotificationTypeDetails.Code))
            {
                RuleNotificationType ruleNotificationType = ruleNotificationTypeRepository.GetRuleNotificationTypeByCode(ruleNotificationTypeDetails.Code);
                ruleNotificationType.Name = ruleNotificationTypeDetails.Name;
                ruleNotificationType.SearchFields = (ruleNotificationTypeDetails.Code + "," + ruleNotificationTypeDetails.Name).ToLower();
                ruleNotificationTypeRepository.Update(ruleNotificationType);
            }
            else
            {
                RuleNotificationType newRuleNotificationType = new RuleNotificationType() { Code = ruleNotificationTypeDetails.Code, Name = ruleNotificationTypeDetails.Name, SearchFields = (ruleNotificationTypeDetails.Code + "," + ruleNotificationTypeDetails.Name).ToLower() };
                ruleNotificationTypeRepository.Add(newRuleNotificationType);
            }
        }

        public static void AddShipmentCustomerType(ShipmentCustomerTypeDetails shipmentCustomerTypeDetails, ShipmentCustomerTypeRepository shipmentCustomerTypeRepository)
        {
            Dictionary<string, ShipmentCustomerType> tenantShipmentCustomerType = shipmentCustomerTypeRepository.GetShipmentCustomerTypes().ToDictionary(d => d.Code, a => a);

            if (tenantShipmentCustomerType.Keys.Contains(shipmentCustomerTypeDetails.Code))
            {
                ShipmentCustomerType shipmentCustomerType = shipmentCustomerTypeRepository.GetSingleShipmentCustomerType(shipmentCustomerTypeDetails.Code);
                shipmentCustomerType.Name = shipmentCustomerTypeDetails.Name;
                shipmentCustomerType.ShowInLOV = shipmentCustomerTypeDetails.ShowInLOV;
                shipmentCustomerType.SearchFields = (shipmentCustomerTypeDetails.Code +","+ shipmentCustomerTypeDetails.Name).ToLower();
                shipmentCustomerTypeRepository.Update(shipmentCustomerType);
            }
            else
            {
                ShipmentCustomerType newShipmentCustomerType = new ShipmentCustomerType()
                {
                    Code = shipmentCustomerTypeDetails.Code,
                    Name = shipmentCustomerTypeDetails.Name,
                    SearchFields = (shipmentCustomerTypeDetails.Code + "," + shipmentCustomerTypeDetails.Name).ToLower(),
                    ShowInLOV = shipmentCustomerTypeDetails.ShowInLOV,
                };
                shipmentCustomerTypeRepository.Add(newShipmentCustomerType);
            }
        }

        public static void AddQuoteCustomerType(QuoteCustomerTypeDetails quoteCustomerTypeDetails, QuoteCustomerTypeRepository quoteCustomerTypeRepository)
        {
            Dictionary<string, QuoteCustomerType> tenantQuoteCustomerType = quoteCustomerTypeRepository.GetQuoteCustomerTypes().ToDictionary(d => d.Code, a => a);

            if (tenantQuoteCustomerType.Keys.Contains(quoteCustomerTypeDetails.Code))
            {
                QuoteCustomerType quoteCustomerType = quoteCustomerTypeRepository.GetSingleQuoteCustomerType(quoteCustomerTypeDetails.Code);
                quoteCustomerType.Name = quoteCustomerTypeDetails.Name;
                quoteCustomerType.ShowInLOV = quoteCustomerTypeDetails.ShowInLOV;
                quoteCustomerType.SearchFields = (quoteCustomerTypeDetails.Code + "," + quoteCustomerTypeDetails.Name).ToLower();
                quoteCustomerTypeRepository.Update(quoteCustomerType);
            }
            else
            {
                QuoteCustomerType newQuoteCustomerType = new QuoteCustomerType()
                {
                    Code = quoteCustomerTypeDetails.Code,
                    Name = quoteCustomerTypeDetails.Name,
                    SearchFields = (quoteCustomerTypeDetails.Code + "," + quoteCustomerTypeDetails.Name).ToLower(),
                    ShowInLOV = quoteCustomerTypeDetails.ShowInLOV,
                };
                quoteCustomerTypeRepository.Add(newQuoteCustomerType);
            }
        }

        public static void AddTarrifFromToType(TarrifFromToTypeDetails tarrifFromToTypeDetails, TarrifFromToTypeRepository tarrifFromToTypeRepository)
        {
            Dictionary<string, TarrifFromToType> tenantTarrifFromToType = tarrifFromToTypeRepository.GetTarrifFromToTypes().ToDictionary(d => d.Code, a => a);

            if (tenantTarrifFromToType.Keys.Contains(tarrifFromToTypeDetails.Code))
            {
                TarrifFromToType tarrifFromToType = tarrifFromToTypeRepository.GetSingleTarrifFromToType(tarrifFromToTypeDetails.Code);
                tarrifFromToType.Name = tarrifFromToTypeDetails.Name;
                tarrifFromToTypeRepository.Update(tarrifFromToType);
            }
            else
            {
                TarrifFromToType newTarrifFromToType = new TarrifFromToType() { Code = tarrifFromToTypeDetails.Code, Name = tarrifFromToTypeDetails.Name, };
                tarrifFromToTypeRepository.Add(newTarrifFromToType);
            }
        }

        public static void AddTarrifType(TarrifTypeDetails tarrifTypeDetails, TarrifTypeRepository tarrifTypeRepository)
        {
            Dictionary<string, TarrifType> tenantTarrifType = tarrifTypeRepository.GetTarrifTypes().ToDictionary(d => d.Code, a => a);

            if (tenantTarrifType.Keys.Contains(tarrifTypeDetails.Code))
            {
                TarrifType tarrifType = tarrifTypeRepository.GetSingleTarrifType(tarrifTypeDetails.Code);
                tarrifType.Name = tarrifTypeDetails.Name;
                tarrifTypeRepository.Update(tarrifType);
            }
            else
            {
                TarrifType newTarrifType = new TarrifType() { Code = tarrifTypeDetails.Code, Name = tarrifTypeDetails.Name, };
                tarrifTypeRepository.Add(newTarrifType);
            }
        }

        public static void AddNextLeg(NextLegDetails nextLegDetails, NextLegRepository nextLegRepository)
        {
            Dictionary<string, NextLeg> tenantNextLeg = nextLegRepository.GetNextLegs().ToDictionary(d => d.Code, a => a);

            if (tenantNextLeg.Keys.Contains(nextLegDetails.Code))
            {
                NextLeg nextLeg = nextLegRepository.GetSingleNextLeg(nextLegDetails.Code);
                nextLeg.Name = nextLegDetails.Name;
                nextLegRepository.Update(nextLeg);
            }
            else
            {
                NextLeg newNextLeg = new NextLeg() { Code = nextLegDetails.Code, Name = nextLegDetails.Name, };
                nextLegRepository.Add(newNextLeg);
            }
        }

        public static void AddPasswordPolicies(PasswordPolicyDetails passwordPolicyDetails, PasswordPolicyRepository passwordPolicyRepository)
        {
            Dictionary<string, PasswordPolicy> tenantSpecifications = passwordPolicyRepository.GetPasswordPolicies().ToDictionary(d => d.Code, a => a);

            if (tenantSpecifications.Keys.Contains(passwordPolicyDetails.Code))
            {
                PasswordPolicy passwordPolicy = passwordPolicyRepository.GetSinglePasswordPolicy(passwordPolicyDetails.Code);
                passwordPolicy.PasswordStrength = passwordPolicy.PasswordStrength;
                passwordPolicyRepository.Update(passwordPolicy);
            }
            else
            {
                PasswordPolicy newPasswordPolicy = new PasswordPolicy() { Code = passwordPolicyDetails.Code, PasswordStrength = passwordPolicyDetails.PasswordStrength, };
                passwordPolicyRepository.Add(newPasswordPolicy);
            }
        }

        public static void AddShipmentLevels(ShipmentLevelDetails shipmentLevelDetails, ShipmentLevelRepository shipmentLevelRepository)
        {
            Dictionary<string, ShipmentLevel> tenantShipmentLevel = shipmentLevelRepository.GetShipmentLevels().ToDictionary(d => d.Code, a => a);

            if (tenantShipmentLevel.Keys.Contains(shipmentLevelDetails.Code))
            {
                ShipmentLevel objShipmentLevel = shipmentLevelRepository.GetSingleShipmentLevel(shipmentLevelDetails.Code);
                objShipmentLevel.Name = shipmentLevelDetails.Name;
                objShipmentLevel.SearchFields = (shipmentLevelDetails.Code + "," + shipmentLevelDetails.Name).ToLower();
                shipmentLevelRepository.Update(objShipmentLevel);
            }
            else
            {
                ShipmentLevel newShipmentLevel = new ShipmentLevel() { Code = shipmentLevelDetails.Code, Name = shipmentLevelDetails.Name, SearchFields = (shipmentLevelDetails.Code+","+ shipmentLevelDetails.Name).ToLower() };
                shipmentLevelRepository.Add(newShipmentLevel);
            }
        }

        public static void AddQuoteTemplateSectionTypes(QuoteTemplateSectionTypeDetails quoteTemplateSectionTypeDetails, QuoteTemplateSectionTypeRepository quoteTemplateSectionTypeRepository)
        {
            Dictionary<string, QuoteTemplateSectionType> tenantQuoteTemplateSectionType = quoteTemplateSectionTypeRepository.GetQuoteTemplateSectionTypes().ToDictionary(d => d.Code, a => a);

            if (tenantQuoteTemplateSectionType.Keys.Contains(quoteTemplateSectionTypeDetails.Code))
            {
                QuoteTemplateSectionType objQuoteTemplateSectionType = quoteTemplateSectionTypeRepository.GetSingleQuoteTemplateSectionType(quoteTemplateSectionTypeDetails.Code,  false);
                objQuoteTemplateSectionType.Name = quoteTemplateSectionTypeDetails.Name;
                
                quoteTemplateSectionTypeRepository.Update(objQuoteTemplateSectionType);
            }
            else
            {
                QuoteTemplateSectionType newQuoteTemplateSectionType = new QuoteTemplateSectionType() { Code = quoteTemplateSectionTypeDetails.Code, Name = quoteTemplateSectionTypeDetails.Name};
                quoteTemplateSectionTypeRepository.Add(newQuoteTemplateSectionType);
            }
        }

        public static void AddBorderTypes(BorderTypeDetails bordertypedetails, BorderTypeRepository bordertyperepository)
        {
            Dictionary<string, BorderType> tenantbordertype = bordertyperepository.GetBorderTypes().ToDictionary(d => d.Code, a => a);

            if (tenantbordertype.Keys.Contains(bordertypedetails.Code))
            {
                BorderType objborderType = bordertyperepository.GetSingleBorderType(bordertypedetails.Code);
                objborderType.Name = bordertypedetails.Name;

                bordertyperepository.Update(objborderType);
            }
            else
            {
                BorderType newBorderType = new BorderType() { Code = bordertypedetails.Code, Name = bordertypedetails.Name };
                bordertyperepository.Add(newBorderType);
            }
        }

        public static void AddAccountTypes(AccountTypeDetails accountTypeDetails, AccountTypeRepository accountTypeRepository)
        {
            Dictionary<string, AccountType> tenantAccountTypes = accountTypeRepository.GetAccountTypes().ToDictionary(d => d.Code, a => a);

            if (tenantAccountTypes.Keys.Contains(accountTypeDetails.Code))
            {
                AccountType accountType = accountTypeRepository.GetSingleAccountType(accountTypeDetails.Code);
                accountType.Name = accountTypeDetails.Name;
                accountType.SearchFields = (accountTypeDetails.Code + "," + accountTypeDetails.Name).ToLower();
                accountTypeRepository.Update(accountType);
            }

            else
            {
                AccountType newaccountType = new AccountType() { Code = accountTypeDetails.Code, Name = accountTypeDetails.Name, SearchFields = (accountTypeDetails.Code + "," + accountTypeDetails.Name).ToLower() };
                accountTypeRepository.Add(newaccountType);
            }     
        }

        public static void AddAccountingTransferTypes(AccountingTransferTypeDetails detailsClass, AccountingTransferTypeRepository repository)
        {
            Dictionary<string, AccountingTransferType> dictionary = repository.GetAllEntities().ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(detailsClass.Code))
            {
                AccountingTransferType entity = repository.GetSingleEntity(detailsClass.Code);
                entity.Name = detailsClass.Name;
                entity.SearchFields = (detailsClass.Code + "," + detailsClass.Name).ToLower();
                repository.Update(entity);
            }

            else
            {
                AccountingTransferType newEntity = new AccountingTransferType()
                {
                    Code = detailsClass.Code,
                    Name = detailsClass.Name,
                    SearchFields = (detailsClass.Code + "," + detailsClass.Name).ToLower()
                };

                repository.Add(newEntity);
            }
        }
        
        public static void AddARPaymentstatus(ARPaymentStatusDetails paymentstatusDetails, ARPaymentStatusRepository paymentstatusRepository)
        {
            Dictionary<string, ARPaymentStatus> tenantPaymentstatuss = paymentstatusRepository.GetARPaymentStatus().ToDictionary(d => d.Code, a => a);

            if (tenantPaymentstatuss.Keys.Contains(paymentstatusDetails.Code))
            {
                ARPaymentStatus paymentstatus = paymentstatusRepository.GetSingleARPaymentStatus(paymentstatusDetails.Code);
                paymentstatus.Name = paymentstatusDetails.Name;
                paymentstatus.SearchFields = (paymentstatusDetails.Code + "," + paymentstatusDetails.Name).ToLower();
                paymentstatusRepository.Update(paymentstatus);
            }
            else
            {
                ARPaymentStatus newPaymentstatus = new ARPaymentStatus() { Code = paymentstatusDetails.Code, Name = paymentstatusDetails.Name, SearchFields = (paymentstatusDetails.Code + "," + paymentstatusDetails.Name).ToLower() };
                paymentstatusRepository.Add(newPaymentstatus);
            }
        }

      
        public static void AddAPInvoiceStatus(APInvoiceStatusDetails invoiceStatusDetails, APInvoiceStatusRepository invoiceStatusRepository)
        {
            Dictionary<string, APInvoiceStatus> tenantApInvoicetatus = invoiceStatusRepository.GetAPInvoiceStatus().ToDictionary(d => d.Code, a => a);

            if (tenantApInvoicetatus.Keys.Contains(invoiceStatusDetails.Code))
            {
                APInvoiceStatus invoiceStatus = invoiceStatusRepository.GetSingleAPInvoiceStatus(invoiceStatusDetails.Code);
                invoiceStatus.Name = invoiceStatusDetails.Name;
                invoiceStatus.SearchFields = (invoiceStatusDetails.Code + "," + invoiceStatusDetails.Name).ToLower();
                invoiceStatusRepository.Update(invoiceStatus);
            }
            else
            {
                APInvoiceStatus newInvoiceStatus = new APInvoiceStatus() { Code = invoiceStatusDetails.Code, Name = invoiceStatusDetails.Name, SearchFields = (invoiceStatusDetails.Code + "," + invoiceStatusDetails.Name).ToLower() };
                invoiceStatusRepository.Add(newInvoiceStatus);
            }
        }

        public static void AddAPInvoiceTypes(APInvoiceTypeDetails invoiceTypeDetails, APInvoiceTypeRepository invoiceTypeRepository)
        {
            Dictionary<string, APInvoiceType> tenantApInvoiceType = invoiceTypeRepository.GetAPInvoiceTypes().ToDictionary(d => d.Code, a => a);

            if (tenantApInvoiceType.Keys.Contains(invoiceTypeDetails.Code))
            {
                APInvoiceType invoiceType = invoiceTypeRepository.GetSingleAPInvoiceType(invoiceTypeDetails.Code);
                invoiceType.Name = invoiceTypeDetails.Name;
                invoiceType.SearchFields = (invoiceTypeDetails.Code + "," + invoiceTypeDetails.Name).ToLower();
                invoiceTypeRepository.Update(invoiceType);
            }
            else
            {
                APInvoiceType newInvoiceType = new APInvoiceType() { Code = invoiceTypeDetails.Code, Name = invoiceTypeDetails.Name, SearchFields = (invoiceTypeDetails.Code + "," + invoiceTypeDetails.Name).ToLower() };
                invoiceTypeRepository.Add(newInvoiceType);
            }
        }
        
        public static void AddAPPaymentstatus(APPaymentStatusDetails paymentstatusDetails, APPaymentStatusRepository paymentstatusRepository)
        {
            Dictionary<string, APPaymentStatus> tenantPaymentstatuss = paymentstatusRepository.GetAPPaymentStatus().ToDictionary(d => d.Code, a => a);

            if (tenantPaymentstatuss.Keys.Contains(paymentstatusDetails.Code))
            {
                APPaymentStatus paymentstatus = paymentstatusRepository.GetSingleAPPaymentStatus(paymentstatusDetails.Code);
                paymentstatus.Name = paymentstatusDetails.Name;
                paymentstatus.SearchFields = (paymentstatusDetails.Code + "," + paymentstatusDetails.Name).ToLower();
                paymentstatusRepository.Update(paymentstatus);
            }
            else
            {
                APPaymentStatus newPaymentstatus = new APPaymentStatus() { Code = paymentstatusDetails.Code, Name = paymentstatusDetails.Name, SearchFields = (paymentstatusDetails.Code + "," + paymentstatusDetails).ToLower() };
                paymentstatusRepository.Add(newPaymentstatus);
            }
        }

        public static void AddShipmentPayableAmountTypes(ShipmentPayableAmountTypeDetails paymentMethodDetails, ShipmentPayableAmountTypeRepository paymentMethodRepository)
        {
            Dictionary<string, ShipmentPayableAmountType> tenantPaymentMethods = paymentMethodRepository.GetShipmentPayableAmountTypes().ToDictionary(d => d.Code, a => a);

            if (tenantPaymentMethods.Keys.Contains(paymentMethodDetails.Code))
            {
                ShipmentPayableAmountType paymentMethod = paymentMethodRepository.GetSingleShipmentPayableAmountType(paymentMethodDetails.Code);
                paymentMethod.Name = paymentMethodDetails.Name;
               
                paymentMethodRepository.Update(paymentMethod);
            }
            else
            {
                ShipmentPayableAmountType newPaymentMethod = new ShipmentPayableAmountType() { Code = paymentMethodDetails.Code, Name = paymentMethodDetails.Name };
                paymentMethodRepository.Add(newPaymentMethod);
            }
        }

        //public static void AddDocTypes(DocTypeDetails docTypeDetails, DocTypeRepository docTypeRepository)
        //{
        //    Dictionary<string, DocType> tenantDocTypes = docTypeRepository.GetDocTypes().ToDictionary(d => d.Code, a => a);

        //    if (tenantDocTypes.Keys.Contains(docTypeDetails.Code))
        //    {
        //        DocType docType = docTypeRepository.GetSingleDocType(docTypeDetails.Code);
        //        docType.Name = docTypeDetails.Name;
        //        docType.SearchFields = docTypeDetails.Code + "," + docTypeDetails.Name;
        //        docTypeRepository.Update(docType);
        //    }
        //    else
        //    {
        //        DocType docType = new DocType() { Code = docTypeDetails.Code, Name = docTypeDetails.Name, SearchFields = docTypeDetails.Code + "," + docTypeDetails.Name };
        //        docTypeRepository.Add(docType);
        //    }
        //}

        //public static void AddBlobTypes(BlobTypeDetails blobTypeDetails, BlobTypeRepository blobTypeRepository)
        //{
        //    Dictionary<string, BlobType> tenantDocTypes = blobTypeRepository.GetBlobTypes().ToDictionary(d => d.Code, a => a);

        //    if (tenantDocTypes.Keys.Contains(blobTypeDetails.Code))
        //    {
        //        BlobType blobType = blobTypeRepository.GetSingleBlobType(blobTypeDetails.Code);
        //        blobType.Name = blobTypeDetails.Name;
        //        blobType.SearchFields = blobTypeDetails.Code + "," + blobTypeDetails.Name;
        //        blobTypeRepository.Update(blobType);
        //    }
        //    else
        //    {
        //        BlobType blobType = new BlobType() { Code = blobTypeDetails.Code, Name = blobTypeDetails.Name, SearchFields = blobTypeDetails.Code + "," + blobTypeDetails.Name };
        //        blobTypeRepository.Add(blobType);
        //    }
        //}

        public static void AddPermissionTypes(PermissionTypeDetails permissionTypeDetails, PermissionTypeRepository permissionTypeRepository)
        {
            Dictionary<string, PermissionType> tenantPermissionTypes = permissionTypeRepository.GetPermissionTypes().ToDictionary(d => d.Code, a => a);

            if (tenantPermissionTypes.Keys.Contains(permissionTypeDetails.Code))
            {
                PermissionType permissionType = permissionTypeRepository.GetSinglePermissionType(permissionTypeDetails.Code);
                permissionType.Name = permissionTypeDetails.Name;
                permissionType.SearchFields = permissionTypeDetails.Code + "," + permissionTypeDetails.Name;
                permissionTypeRepository.Update(permissionType);
            }
            else
            {
                PermissionType permissionType = new PermissionType() { Code = permissionTypeDetails.Code, Name = permissionTypeDetails.Name, SearchFields = permissionTypeDetails.Code + "," + permissionTypeDetails.Name };
                permissionTypeRepository.Add(permissionType);
            }
        }

        public static void AddRoleTypes(RoleTypeDetails roleTypeDetails, RoleTypeRepository roleTypeRepository)
        {
            Dictionary<string, RoleType> tenantRoleTypes = roleTypeRepository.GetRoleTypes().ToDictionary(d => d.Code, a => a);

            if (tenantRoleTypes.Keys.Contains(roleTypeDetails.Code))
            {
                RoleType roleType = roleTypeRepository.GetSingleRoleType(roleTypeDetails.Code);
                roleType.Name = roleTypeDetails.Name;
                roleType.SearchFields = roleTypeDetails.Code + "," + roleTypeDetails.Name;
                roleTypeRepository.Update(roleType);
            }
            else
            {
                RoleType roleType = new RoleType() { Code = roleTypeDetails.Code, Name = roleTypeDetails.Name, SearchFields = roleTypeDetails.Code + "," + roleTypeDetails.Name };
                roleTypeRepository.Add(roleType);
            }
        }

        public static void AddFeatureTypes(FeatureTypeDetails featureTypeDetails, FeatureTypeRepository featureTypeRepository)
        {
            Dictionary<string, FeatureType> tenantFeatureTypes = featureTypeRepository.GetFeatureTypes().ToDictionary(d => d.Code, a => a);

            if (tenantFeatureTypes.Keys.Contains(featureTypeDetails.Code))
            {
                FeatureType featureType = featureTypeRepository.GetSingleFeatureType(featureTypeDetails.Code);
                featureType.Name = featureTypeDetails.Name;
                featureType.SearchFields = featureTypeDetails.Code + "," + featureTypeDetails.Name;
                featureTypeRepository.Update(featureType);
            }
            else
            {
                FeatureType featureType = new FeatureType() { Code = featureTypeDetails.Code, Name = featureTypeDetails.Name, SearchFields = featureTypeDetails.Code + "," + featureTypeDetails.Name };
                featureTypeRepository.Add(featureType);
            }
        }

        public static void AddObjectTableTypes(ObjectTableTypeDetails objectTableTypeDetails, ObjectTableTypeRepository objectTableTypeRepository)
        {
            Dictionary<string, ObjectTableType> tenantObjectTableTypes = objectTableTypeRepository.GetObjectTableTypes().ToDictionary(d => d.Code, a => a);

            if (tenantObjectTableTypes.Keys.Contains(objectTableTypeDetails.Code))
            {
                ObjectTableType objectTableType = objectTableTypeRepository.GetSingleObjectTableType(objectTableTypeDetails.Code);
                objectTableType.Name = objectTableTypeDetails.Name;
                objectTableType.SearchFields = objectTableTypeDetails.Code + "," + objectTableTypeDetails.Name;
                objectTableTypeRepository.Update(objectTableType);
            }
            else
            {
                ObjectTableType objectTableType = new ObjectTableType() { Code = objectTableTypeDetails.Code, Name = objectTableTypeDetails.Name, SearchFields = objectTableTypeDetails.Code + "," + objectTableTypeDetails.Name };
                objectTableTypeRepository.Add(objectTableType);
            }
        }

        public static void AddPackages(PackageDetails packageDetails, PackageRepository packageRepository)
        {
            Dictionary<string, Package> tenantPackages = packageRepository.GetPackages().ToDictionary(d => d.Code, a => a);

            if (tenantPackages.Keys.Contains(packageDetails.Code))
            {
                //Package package = packageRepository.GetSinglePackage(packageDetails.Code);
                //package.Name = packageDetails.Name;
                //package.SearchFields = packageDetails.Code + "," + packageDetails.Name;
                //packageRepository.Update(package);
            }
            else
            {
                Package package = new Package() { Code = packageDetails.Code, Name = packageDetails.Name, SearchFields = packageDetails.Code + "," + packageDetails.Name , FeaturePackageTypeCode = "BS"};
                packageRepository.Add(package);
            }
        }

        public static void AddAWBChargesCodes(AWBChargesCodeDetails awbChargesCodeDetails, AWBChargesCodeRepository awbChargeCodesRepository)
        {
            Dictionary<string, AWBChargesCode> tenantPackages = awbChargeCodesRepository.GetAWBChargeCodes().ToDictionary(d => d.Code, a => a);

            if (tenantPackages.Keys.Contains(awbChargesCodeDetails.Code))
            {
                AWBChargesCode awbChargesCode = awbChargeCodesRepository.GetSingleAWBChargeCode(awbChargesCodeDetails.Code);
                awbChargesCode.Name = awbChargesCodeDetails.Name;
                awbChargesCode.SearchFields = awbChargesCodeDetails.Code + "," + awbChargesCodeDetails.Name;
                awbChargeCodesRepository.Update(awbChargesCode);
            }
            else
            {
                AWBChargesCode awbChargesCode = new AWBChargesCode() { Code = awbChargesCodeDetails.Code, Name = awbChargesCodeDetails.Name, SearchFields = awbChargesCodeDetails.Code + "," + awbChargesCodeDetails.Name };
                awbChargeCodesRepository.Add(awbChargesCode);
            }
        }

        //public static void AddAWBSpecialHandlingCodes(AWBSpecialHandlingCodeDetails awbSpecialHandlingCodeDetails, AWBSpecialHandlingCodeRepository awbSpecialHandlingCodeRepository)
        //{
        //    Dictionary<string, AWBSpecialHandlingCode> tenantPackages = awbSpecialHandlingCodeRepository.GetAWBHandlingCodes().ToDictionary(d => d.Code, a => a);

        //    if (tenantPackages.Keys.Contains(awbSpecialHandlingCodeDetails.Code))
        //    {
        //        AWBSpecialHandlingCode awbSpecialHandlingCode = awbSpecialHandlingCodeRepository.GetSingleAWBHandlingCode(awbSpecialHandlingCodeDetails.Code);
        //        awbSpecialHandlingCode.Name = awbSpecialHandlingCodeDetails.Name;
        //        awbSpecialHandlingCode.SearchFields = awbSpecialHandlingCodeDetails.Code + "," + awbSpecialHandlingCodeDetails.Name;
        //        awbSpecialHandlingCodeRepository.Update(awbSpecialHandlingCode);
        //    }
        //    else
        //    {
        //        AWBSpecialHandlingCode awbSpecialHandlingCode = new AWBSpecialHandlingCode() { Code = awbSpecialHandlingCodeDetails.Code, Name = awbSpecialHandlingCodeDetails.Name, SearchFields = awbSpecialHandlingCodeDetails.Code + "," + awbSpecialHandlingCodeDetails.Name };
        //        awbSpecialHandlingCodeRepository.Add(awbSpecialHandlingCode);
        //    }
        //}

        public static void AddSharedLogisticsUpdateStatus(SharedLogisticsUpdateStatusDetails sharedLogisticsUpdateStatusDetails, SharedLogisticsUpdateStatusRepository sharedLogisticsUpdateStatusRepository)
        {
            Dictionary<string, SharedLogisticsUpdateStatus> tenantSharedLogisticsUpdateStatus = sharedLogisticsUpdateStatusRepository.GetSharedLogisticsUpdateStatus().ToDictionary(d => d.Code, a => a);

            if (tenantSharedLogisticsUpdateStatus.Keys.Contains(sharedLogisticsUpdateStatusDetails.Code))
            {
                SharedLogisticsUpdateStatus sharedLogisticsUpdateStatus = sharedLogisticsUpdateStatusRepository.GetSingleSharedLogisticsUpdateStatus(sharedLogisticsUpdateStatusDetails.Code);
                sharedLogisticsUpdateStatus.Name = sharedLogisticsUpdateStatusDetails.Name;
                sharedLogisticsUpdateStatus.SearchFields = sharedLogisticsUpdateStatusDetails.Code + "," + sharedLogisticsUpdateStatusDetails.Name;
                sharedLogisticsUpdateStatusRepository.Update(sharedLogisticsUpdateStatus);
            }
            else
            {
                SharedLogisticsUpdateStatus sharedLogisticsUpdateStatus = new SharedLogisticsUpdateStatus() { Code = sharedLogisticsUpdateStatusDetails.Code, Name = sharedLogisticsUpdateStatusDetails.Name, SearchFields = sharedLogisticsUpdateStatusDetails.Code + "," + sharedLogisticsUpdateStatusDetails.Name };
                sharedLogisticsUpdateStatusRepository.Add(sharedLogisticsUpdateStatus);
            }
        }

        public static void AddFHLStatus(FHLStatusDetails FHLStatusDetails, FHLStatusRepository FHLStatusRepository)
        {
            Dictionary<string, FHLStatus> tenantFHLStatus = FHLStatusRepository.GetFHLStatus().ToDictionary(d => d.Code, a => a);

            if (tenantFHLStatus.Keys.Contains(FHLStatusDetails.Code))
            {
                FHLStatus FHLStatus = FHLStatusRepository.GetSingleFHLStatus(FHLStatusDetails.Code);
                FHLStatus.Name = FHLStatusDetails.Name;
                FHLStatus.SearchFields = FHLStatusDetails.Code + "," + FHLStatusDetails.Name;
                FHLStatusRepository.Update(FHLStatus);
            }
            else
            {
                FHLStatus FHLStatus = new FHLStatus() { Code = FHLStatusDetails.Code, Name = FHLStatusDetails.Name, SearchFields = FHLStatusDetails.Code + "," + FHLStatusDetails.Name };
                FHLStatusRepository.Add(FHLStatus);
            }
        }

        public static void AddFWBStatus(FWBStatusDetails FWBStatusDetails, FWBStatusRepository FWBStatusRepository)
        {
            Dictionary<string, FWBStatus> tenantFWBStatus = FWBStatusRepository.GetFWBStatus().ToDictionary(d => d.Code, a => a);

            if (tenantFWBStatus.Keys.Contains(FWBStatusDetails.Code))
            {
                FWBStatus FWBStatus = FWBStatusRepository.GetSingleFWBStatus(FWBStatusDetails.Code);
                FWBStatus.Name = FWBStatusDetails.Name;
                FWBStatus.SearchFields = FWBStatusDetails.Code + "," + FWBStatusDetails.Name;
                FWBStatusRepository.Update(FWBStatus);
            }
            else
            {
                FWBStatus FWBStatus = new FWBStatus() { Code = FWBStatusDetails.Code, Name = FWBStatusDetails.Name, SearchFields = FWBStatusDetails.Code + "," + FWBStatusDetails.Name };
                FWBStatusRepository.Add(FWBStatus);
            }
        }

        public static void AddAWBStatus(AWBStatusDetails AWBStatusDetails, AWBStatusRepository AWBStatusRepository)
        {
            Dictionary<string, AWBStatus> tenantAWBStatus = AWBStatusRepository.GetAWBStatus().ToDictionary(d => d.Code, a => a);

            if (tenantAWBStatus.Keys.Contains(AWBStatusDetails.Code))
            {
                AWBStatus AWBStatus = AWBStatusRepository.GetSingleAWBStatus(AWBStatusDetails.Code);
                AWBStatus.Name = AWBStatusDetails.Name;
                AWBStatus.SearchFields = AWBStatusDetails.Code + "," + AWBStatusDetails.Name;
                AWBStatusRepository.Update(AWBStatus);
            }
            else
            {
                AWBStatus AWBStatus = new AWBStatus() { Code = AWBStatusDetails.Code, Name = AWBStatusDetails.Name, SearchFields = AWBStatusDetails.Code + "," + AWBStatusDetails.Name };
                AWBStatusRepository.Add(AWBStatus);
            }
        }

        public static void AddRecurringPeriods(RecurringPeriodDetails recurringPeriodDetails, RecurringPeriodRepository recurringPeriodRepository)
        {
            Dictionary<string, RecurringPeriod> tenantRecurringPeriods = recurringPeriodRepository.GetRecurringPeriods().ToDictionary(d => d.Code, a => a);

            if (tenantRecurringPeriods.Keys.Contains(recurringPeriodDetails.Code))
            {
                RecurringPeriod recurringPeriod = recurringPeriodRepository.GetSingleRecurringPeriod(recurringPeriodDetails.Code);
                recurringPeriod.Name = recurringPeriodDetails.Name;
                recurringPeriod.SearchFields = recurringPeriodDetails.Code + "," + recurringPeriodDetails.Name;
                recurringPeriodRepository.Update(recurringPeriod);
            }
            else
            {
                RecurringPeriod recurringPeriod = new RecurringPeriod() { Code = recurringPeriodDetails.Code, Name = recurringPeriodDetails.Name, SearchFields = recurringPeriodDetails.Code + "," + recurringPeriodDetails.Name };
                recurringPeriodRepository.Add(recurringPeriod);
            }
        }


        public static void AddPaymentChannels(PaymentChannelDetails paymentChannelDetails, PaymentChannelRepository paymentChannelRepository)
        {
            Dictionary<string, PaymentChannel> tenantPaymentChannels = paymentChannelRepository.GetPaymentChannels().ToDictionary(d => d.Code, a => a);

            if (tenantPaymentChannels.Keys.Contains(paymentChannelDetails.Code))
            {
                PaymentChannel paymentChannel = paymentChannelRepository.GetSinglePaymentChannel(paymentChannelDetails.Code);
                paymentChannel.Name = paymentChannelDetails.Name;
                paymentChannel.SearchFields = paymentChannelDetails.Code + "," + paymentChannelDetails.Name;
                paymentChannelRepository.Update(paymentChannel);
            }
            else
            {
                PaymentChannel paymentChannel = new PaymentChannel() { Code = paymentChannelDetails.Code, Name = paymentChannelDetails.Name, SearchFields = paymentChannelDetails.Code + "," + paymentChannelDetails.Name };
                paymentChannelRepository.Add(paymentChannel);
            }   
        }

        public static void AddEntityLastActivityTypes(EntityLastActivityTypeDetails EntityLastActivityTypeDetails, EntityLastActivityTypeRepository EntityLastActivityTypeRepository)
        {
            Dictionary<string, EntityLastActivityType> tenantEntityLastActivityType = EntityLastActivityTypeRepository.GetEntityLastActivityTypes().ToDictionary(d => d.Code, a => a);

            if (tenantEntityLastActivityType.Keys.Contains(EntityLastActivityTypeDetails.Code))
            {
                EntityLastActivityType EntityLastActivityType = EntityLastActivityTypeRepository.GetSingleEntityLastActivityType(EntityLastActivityTypeDetails.Code);
                EntityLastActivityType.Name = EntityLastActivityTypeDetails.Name;
               
                EntityLastActivityTypeRepository.Update(EntityLastActivityType);
            }
            else
            {
                EntityLastActivityType EntityLastActivityType = new EntityLastActivityType() { Code = EntityLastActivityTypeDetails.Code, Name = EntityLastActivityTypeDetails.Name};
                EntityLastActivityTypeRepository.Add(EntityLastActivityType);
            }
        }

        public static void AddEventTypeCategories(EventTypeCategoryDetails eventTypeCategoryDetails, EventTypeCategoryRepository eventTypeCategoryRepository)
        {
            Dictionary<string, EventTypeCategory> tenantEventTypeCategories = eventTypeCategoryRepository.GetEventTypeCategories().ToDictionary(d => d.Code, a => a);

            if (tenantEventTypeCategories.Keys.Contains(eventTypeCategoryDetails.Code))
            {
                EventTypeCategory eventTypeCategory = eventTypeCategoryRepository.GetSingleEventTypeCategory(eventTypeCategoryDetails.Code);
                eventTypeCategory.Name = eventTypeCategoryDetails.Name;
                eventTypeCategory.SearchFields = eventTypeCategoryDetails.Code + "," + eventTypeCategoryDetails.Name;
                eventTypeCategoryRepository.Update(eventTypeCategory);
            }
            else
            {
                EventTypeCategory eventTypeCategory = new EventTypeCategory() { Code = eventTypeCategoryDetails.Code, Name = eventTypeCategoryDetails.Name, SearchFields = eventTypeCategoryDetails.Code + "," + eventTypeCategoryDetails.Name };
                eventTypeCategoryRepository.Add(eventTypeCategory);
            }
        }

        public static void AddAccountingSystems(AccountingSystemDetails accountingSystemDetails, AccountingSystemRepository accountingSystemRepository)
        {
            Dictionary<string, AccountingSystem> tenantAccountingSystems = accountingSystemRepository.GetAccountingSystems().ToDictionary(d => d.Code, a => a);

            if (tenantAccountingSystems.Keys.Contains(accountingSystemDetails.Code))
            {
                AccountingSystem accountingSystem = accountingSystemRepository.GetSingleAccountingSystem(accountingSystemDetails.Code);
                accountingSystem.Name = accountingSystemDetails.Name;
                accountingSystem.IsExternalCodesFromTable = accountingSystemDetails.IsExternalCodesFromTable;
                accountingSystem.AllowManuallyDueDate = accountingSystemDetails.AllowManuallyDueDate;
                accountingSystem.IsJournalMode = accountingSystemDetails.IsJournalMode;
                accountingSystem.IsSingleCurrencyAccount = accountingSystemDetails.IsSingleCurrencyAccount;
                accountingSystem.IsSingleTaxPerInvoice = accountingSystemDetails.IsSingleTaxPerInvoice;
                accountingSystem.IsTaxItemManaged = accountingSystemDetails.IsTaxItemManaged;
                accountingSystem.AllowMinusInvoiceLines = accountingSystemDetails.AllowMinusInvoiceLines;
                accountingSystem.SearchFields = accountingSystemDetails.Code + "," + accountingSystemDetails.Name;
                accountingSystem.ShowDownloadScreen = accountingSystemDetails.ShowDownloadScreen;
                accountingSystem.AllowPositiveAmountsInTheCreditNote = accountingSystemDetails.AllowPositiveAmountsInTheCreditNote;
                accountingSystem.AllowARInvoicesTransfer = accountingSystemDetails.AllowARInvoicesTransfer;
                accountingSystemRepository.Update(accountingSystem);
            }
            else
            {
                AccountingSystem accountingSystem = new AccountingSystem() { Code = accountingSystemDetails.Code, Name = accountingSystemDetails.Name, SearchFields = accountingSystemDetails.Code + "," + accountingSystemDetails.Name, AllowARInvoicesTransfer = accountingSystemDetails.AllowARInvoicesTransfer };
                accountingSystemRepository.Add(accountingSystem);
            }
        }

        public static void AddSharedLogisticsInvitationStatus(SharedLogisticsInvitationStatusDetails sharedLogisticsInvitationStatusDetails, SharedLogisticsInvitationStatusRepository sharedLogisticsInvitationStatusRepository)
        {
            Dictionary<int, SharedLogisticsInvitationStatus> tenantSharedLogisticsInvitationStatus = sharedLogisticsInvitationStatusRepository.GetSharedLogisticsInvitationStatus().ToDictionary(d => d.Code, a => a);

            if (tenantSharedLogisticsInvitationStatus.Keys.Contains(sharedLogisticsInvitationStatusDetails.Code))
            {
                SharedLogisticsInvitationStatus sharedLogisticsInvitationStatus = sharedLogisticsInvitationStatusRepository.GetSingleSharedLogisticsInvitationStatus(sharedLogisticsInvitationStatusDetails.Code);
                sharedLogisticsInvitationStatus.Name = sharedLogisticsInvitationStatusDetails.Name;
                sharedLogisticsInvitationStatus.SearchFields = sharedLogisticsInvitationStatusDetails.Code + "," + sharedLogisticsInvitationStatusDetails.Name;
                sharedLogisticsInvitationStatusRepository.Update(sharedLogisticsInvitationStatus);
            }
            else
            {
                SharedLogisticsInvitationStatus sharedLogisticsInvitationStatus = new SharedLogisticsInvitationStatus() { Code = sharedLogisticsInvitationStatusDetails.Code, Name = sharedLogisticsInvitationStatusDetails.Name, SearchFields = sharedLogisticsInvitationStatusDetails.Code + "," + sharedLogisticsInvitationStatusDetails.Name };
                sharedLogisticsInvitationStatusRepository.Add(sharedLogisticsInvitationStatus);
            }
            

            
        }



        public static void AddSharedManifestsStatus(SharedManifestsStatusDetails sharedManifestsStatusDetails, SharedManifestsStatusRepository sharedManifestsStatusRepository)
        {
          
            Dictionary<string, SharedManifestsStatus> sharedManifestsStatuses = sharedManifestsStatusRepository.GetSharedManifestsStatuses().ToDictionary(d => d.StatusCode, a => a);

            if (sharedManifestsStatuses.Keys.Contains(sharedManifestsStatusDetails.StatusCode))
            {
                SharedManifestsStatus sharedManifestsStatus = sharedManifestsStatusRepository.GetSingleSharedManifestsStatus(sharedManifestsStatusDetails.StatusCode);
                sharedManifestsStatus.StatusName = sharedManifestsStatusDetails.StatusName;
                sharedManifestsStatus.SearchFields = sharedManifestsStatusDetails.StatusCode + "," + sharedManifestsStatusDetails.StatusName;
                sharedManifestsStatusRepository.Update(sharedManifestsStatus);
            }
            else
            {
                SharedManifestsStatus sharedManifestsStatus = new SharedManifestsStatus() { StatusCode = sharedManifestsStatusDetails.StatusCode, StatusName = sharedManifestsStatusDetails.StatusName, SearchFields = sharedManifestsStatusDetails.StatusCode + "," + sharedManifestsStatusDetails.StatusName };
                sharedManifestsStatusRepository.Add(sharedManifestsStatus);
            }



        }



        public static void AddPhysicalCheckOperation(PhysicalCheckOperationDetails physicalCheckOperationDetails, PhysicalCheckOperationRepository physicalCheckOperationRepository)
        {
            Dictionary<string, PhysicalCheckOperation> physicalCheckOperations = physicalCheckOperationRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (physicalCheckOperations.Keys.Contains(physicalCheckOperationDetails.Code))
            {
                PhysicalCheckOperation physicalCheckOperation = physicalCheckOperationRepository.GetSingle(physicalCheckOperationDetails.Code);
                physicalCheckOperation.EnglishName = physicalCheckOperationDetails.EnglishName;
                physicalCheckOperation.LocalName = physicalCheckOperationDetails.LocalName;
                physicalCheckOperation.SearchFields =(physicalCheckOperationDetails.Code+','+ physicalCheckOperation.EnglishName+','+ physicalCheckOperationDetails.LocalName).ToLower();
                physicalCheckOperationRepository.Update(physicalCheckOperation);
            }
            else
            {
                PhysicalCheckOperation newPaymentMethod = new PhysicalCheckOperation() { Code = physicalCheckOperationDetails.Code, EnglishName = physicalCheckOperationDetails.EnglishName, LocalName = physicalCheckOperationDetails.LocalName, SearchFields =(physicalCheckOperationDetails.Code+','+ physicalCheckOperationDetails.EnglishName + ',' + physicalCheckOperationDetails.LocalName).ToLower() };
                physicalCheckOperationRepository.Add(newPaymentMethod);
            }
        }

        public static void AddPhysicalCheckStatusMessage(PhysicalCheckStatusMessageDetails physicalCheckStatusMessageDetails, PhysicalCheckStatusMessageRepository physicalCheckStatusMessageRepository)
        {
            Dictionary<string, PhysicalCheckStatusMessage> physicalCheckStatusMessages = physicalCheckStatusMessageRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (physicalCheckStatusMessages.Keys.Contains(physicalCheckStatusMessageDetails.Code))
            {
                PhysicalCheckStatusMessage physicalCheckStatusMessage = physicalCheckStatusMessageRepository.GetSingle(physicalCheckStatusMessageDetails.Code);
                physicalCheckStatusMessage.EnglishName = physicalCheckStatusMessageDetails.EnglishName;
                physicalCheckStatusMessage.LocalName = physicalCheckStatusMessageDetails.LocalName;
                physicalCheckStatusMessage.SearchFields = (physicalCheckStatusMessageDetails.Code + ',' + physicalCheckStatusMessage.EnglishName + ',' + physicalCheckStatusMessageDetails.LocalName).ToLower();
                physicalCheckStatusMessageRepository.Update(physicalCheckStatusMessage);
            }
            else
            {
                PhysicalCheckStatusMessage newPaymentMethod = new PhysicalCheckStatusMessage() { Code = physicalCheckStatusMessageDetails.Code, EnglishName = physicalCheckStatusMessageDetails.EnglishName, LocalName = physicalCheckStatusMessageDetails.LocalName, SearchFields = (physicalCheckStatusMessageDetails.Code + ',' + physicalCheckStatusMessageDetails.EnglishName + ',' + physicalCheckStatusMessageDetails.LocalName).ToLower() };
                physicalCheckStatusMessageRepository.Add(newPaymentMethod);
            }
        }

        public static void AddAWBCustomsInfo(AWBCustomsInfoDetails detailsClass, AWBCustomsInformationRepository repository)
        {
            Dictionary<string, AWBCustomsInformation> dictionary = repository.GetAWBCustomsInfos().ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(detailsClass.Code))
            {
                AWBCustomsInformation entity = repository.GetSingleAWBCustomsInfo(detailsClass.Code);
                entity.Name = detailsClass.Name;
                entity.SearchFields = detailsClass.Code + "," + detailsClass.Name;
                repository.Update(entity);
            }

            else
            {
                AWBCustomsInformation newEntity = new AWBCustomsInformation() { Code = detailsClass.Code, Name = detailsClass.Name, SearchFields = detailsClass.Code + "," + detailsClass.Name };
                repository.Add(newEntity);
            }            
        }

        public static void AddAWBInformation(AWBInformationDetails detailsClass, AWBInformationRepository repository)
        {
            Dictionary<string, AWBInformation> dictionary = repository.GetAWBInformations().ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(detailsClass.Code))
            {
                AWBInformation entity = repository.GetSingleAWBInformation(detailsClass.Code);
                entity.Name = detailsClass.Name;
                entity.SearchFields = detailsClass.Code + "," + detailsClass.Name;
                repository.Update(entity);
            }

            else
            {
                AWBInformation newEntity = new AWBInformation() { Code = detailsClass.Code, Name = detailsClass.Name, SearchFields = detailsClass.Code + "," + detailsClass.Name };
                repository.Add(newEntity);
            } 
        }

        public static void AddProductType(ProductTypeDetails detailsClass, ProductTypeRepository repository)
        {
            Dictionary<string, ProductType> dictionary = repository.GetProductTypes().ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(detailsClass.Code))
            {
                ProductType entity = repository.GetSingleProductType(detailsClass.Code);
                entity.Name = detailsClass.Name;
             
                entity.SearchFields = detailsClass.Code + "," + detailsClass.Name;
                repository.Update(entity);
            }
            else
            {
                ProductType newEntity = new ProductType() { Code = detailsClass.Code, Name = detailsClass.Name, SearchFields = detailsClass.Code + "," + detailsClass.Name };
                repository.Add(newEntity);
            }
        }

        public static void AddProductPeriod(ProductPeriodDetails detailsClass, ProductPeriodRepository repository)
        {
            Dictionary<string, ProductPeriod> dictionary = repository.GetProductPeriods().ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(detailsClass.Code))
            {
                ProductPeriod entity = repository.GetSingleProductPeriod(detailsClass.Code);
                entity.Name = detailsClass.Name;
                entity.SearchFields = detailsClass.Code + "," + detailsClass.Name;
                repository.Update(entity);
            }
            else
            {
                ProductPeriod newEntity = new ProductPeriod() { Code = detailsClass.Code, Name = detailsClass.Name, SearchFields = detailsClass.Code + "," + detailsClass.Name };
                repository.Add(newEntity);
            }
        }

        public static void AddContactDoneMethod(ContactDoneMethodDetails detailsClass, ContactDoneMethodRepository repository)
        {
            Dictionary<string, ContactDoneMethod> dictionary = repository.GetContactDoneMethods().ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(detailsClass.Code))
            {
                ContactDoneMethod entity = repository.GetSingleContactDoneMethod(detailsClass.Code);
                entity.Name = detailsClass.Name;
                entity.SearchFields = detailsClass.Code + "," + detailsClass.Name;
                repository.Update(entity);
            }
            else
            {
                ContactDoneMethod newEntity = new ContactDoneMethod() { Code = detailsClass.Code, Name = detailsClass.Name, SearchFields = detailsClass.Code + "," + detailsClass.Name };
                repository.Add(newEntity);
            }
        }

        //public static void AddQuoteClosingReason(QuoteClosingReasonDetails quoteClosingReasonDetails, QuoteClosingReasonRepository quoteClosingReasonRepository)
        //{
        //    Dictionary<string, QuoteClosingReason> tenantQuoteClosingReason = quoteClosingReasonRepository.GetQuoteClosingReasons().ToDictionary(d => d.Code, a => a);

        //    if (tenantQuoteClosingReason.Keys.Contains(quoteClosingReasonDetails.Code))
        //    {
        //        QuoteClosingReason quoteClosingReason = quoteClosingReasonRepository.GetSingleQuoteClosingReason(quoteClosingReasonDetails.Code);
        //        quoteClosingReason.Name = quoteClosingReasonDetails.Name;
        //        quoteClosingReason.SearchFields = quoteClosingReasonDetails.Code + "," + quoteClosingReasonDetails.Name;
        //        quoteClosingReasonRepository.Update(quoteClosingReason);
        //    }
        //    else
        //    {
        //        QuoteClosingReason newQuoteClosingReason = new QuoteClosingReason() { Code = quoteClosingReasonDetails.Code, Name = quoteClosingReasonDetails.Name, SearchFields = quoteClosingReasonDetails.Code + "," + quoteClosingReasonDetails.Name };
        //        quoteClosingReasonRepository.Add(newQuoteClosingReason);
        //    }
        //}

        public static void AddCustomerStatus(CustomerStatusDetails CustomerStatusDetails, CustomerStatusRepository CustomerStatusRepository)
        {
            Dictionary<string, CustomerStatus> tenantCustomerStatuss = CustomerStatusRepository.GetCustomerStatus().ToDictionary(d => d.Code, a => a);

            if (tenantCustomerStatuss.Keys.Contains(CustomerStatusDetails.Code))
            {
                CustomerStatus CustomerStatus = CustomerStatusRepository.GetSingleCustomerStatus(CustomerStatusDetails.Code);
                CustomerStatus.Name = CustomerStatusDetails.Name;
                CustomerStatus.SearchFields = CustomerStatusDetails.Code + "," + CustomerStatusDetails.Name;
                CustomerStatusRepository.Update(CustomerStatus);
            }
            else
            {
                CustomerStatus newCustomerStatus = new CustomerStatus() { Code = CustomerStatusDetails.Code, Name = CustomerStatusDetails.Name, SearchFields = CustomerStatusDetails.Code + "," + CustomerStatusDetails.Name };
                CustomerStatusRepository.Add(newCustomerStatus);
            }
        }

        public static void AddPaymentCurrencies(PaymentCurrencyDetails paymentCurrencyDetails, PaymentCurrencyRepository paymentCurrencyRepository)
        {
            Dictionary<string, PaymentCurrency> tenantPaymentCurrencies = paymentCurrencyRepository.GetPaymentCurrencies().ToDictionary(d => d.Code, a => a);

            if (tenantPaymentCurrencies.Keys.Contains(paymentCurrencyDetails.Code))
            {
                PaymentCurrency paymentCurrency = paymentCurrencyRepository.GetSinglePaymentCurrency(paymentCurrencyDetails.Code);
                paymentCurrency.Name = paymentCurrencyDetails.Name;
                paymentCurrency.SearchFields = paymentCurrencyDetails.Code + "," + paymentCurrencyDetails.Name;
                paymentCurrencyRepository.Update(paymentCurrency);
            }
            else
            {
                PaymentCurrency paymentCurrency = new PaymentCurrency() { Code = paymentCurrencyDetails.Code, Name = paymentCurrencyDetails.Name, SearchFields = paymentCurrencyDetails.Code + "," + paymentCurrencyDetails.Name };
                paymentCurrencyRepository.Add(paymentCurrency);
            }
        }

        public static void AddDocumentTypeCategories(DocumentTypeCategoryDetails documentTypeCategoryDetails, DocumentTypeCategoryRepository documentTypeCategoryRepository)
        {
            Dictionary<string, DocumentTypeCategory> tenantDocumentTypeCategories = documentTypeCategoryRepository.GetDocumentTypeCategories().ToDictionary(d => d.Code, a => a);

            if (tenantDocumentTypeCategories.Keys.Contains(documentTypeCategoryDetails.Code))
            {
                DocumentTypeCategory documentTypeCategory = documentTypeCategoryRepository.GetSingleDocumentTypeCategory(documentTypeCategoryDetails.Code);
                documentTypeCategory.Name = documentTypeCategoryDetails.Name;
                documentTypeCategory.SearchFields = documentTypeCategoryDetails.Code + "," + documentTypeCategoryDetails.Name;
                documentTypeCategoryRepository.Update(documentTypeCategory);
            }
            else
            {
                DocumentTypeCategory newDocumentTypeCategory = new DocumentTypeCategory() { Code = documentTypeCategoryDetails.Code, Name = documentTypeCategoryDetails.Name, SearchFields = documentTypeCategoryDetails.Code + "," + documentTypeCategoryDetails.Name };
                documentTypeCategoryRepository.Add(newDocumentTypeCategory);
            }
        }

        public static void AddCustomerTenantAccessStatusTypes(CustomerTenantAccessStatusTypeDetails customerTenantAccessStatusTypeDetails, CustomerTenantAccessStatusTypeRepository customerTenantAccessStatusTypeRepository)
        {
            Dictionary<string, CustomerTenantAccessStatusType> tenantCustomerTenantAccessStatusTypes = customerTenantAccessStatusTypeRepository.GetCustomerTenantAccessStatusTypes().ToDictionary(d => d.Code, a => a);

            if (tenantCustomerTenantAccessStatusTypes.Keys.Contains(customerTenantAccessStatusTypeDetails.Code))
            {
                CustomerTenantAccessStatusType customerTenantAccessStatusType = customerTenantAccessStatusTypeRepository.GetSingleCustomerTenantAccessStatusType(customerTenantAccessStatusTypeDetails.Code);
                customerTenantAccessStatusType.EnglishName = customerTenantAccessStatusTypeDetails.EnglishName;
                customerTenantAccessStatusType.LocalName = customerTenantAccessStatusTypeDetails.LocalName;
                customerTenantAccessStatusType.SearchFields = customerTenantAccessStatusTypeDetails.Code + "," + customerTenantAccessStatusTypeDetails.EnglishName + "," + customerTenantAccessStatusTypeDetails.LocalName;
                customerTenantAccessStatusTypeRepository.Update(customerTenantAccessStatusType);
            }
            else
            {
                CustomerTenantAccessStatusType newCustomerTenantAccessStatusType = new CustomerTenantAccessStatusType() { Code = customerTenantAccessStatusTypeDetails.Code, EnglishName = customerTenantAccessStatusTypeDetails.EnglishName, SearchFields = customerTenantAccessStatusTypeDetails.Code + "," + customerTenantAccessStatusTypeDetails.EnglishName + "," + customerTenantAccessStatusTypeDetails.LocalName };
                customerTenantAccessStatusTypeRepository.Add(newCustomerTenantAccessStatusType);
            }
        }

        public static void AddWarehouseTypes(WarehouseTypeDetails warehouseTypeDetails, WarehouseTypeRepository warehouseTypeRepository)
        {
            Dictionary<string, WarehouseType> tenantWarehouseTypes = warehouseTypeRepository.GetWarehouseTypes().ToDictionary(d => d.Code, a => a);

            if (tenantWarehouseTypes.Keys.Contains(warehouseTypeDetails.Code))
            {
                WarehouseType warehouseType = warehouseTypeRepository.GetSingleWarehouseType(warehouseTypeDetails.Code);
                warehouseType.Code = warehouseTypeDetails.Code;
                warehouseType.Name = warehouseTypeDetails.Name;
                warehouseType.SearchFields = warehouseTypeDetails.Code + "," + warehouseTypeDetails.Name;
                warehouseTypeRepository.Update(warehouseType);
            }
            else
            {
                WarehouseType newWarehouseType = new WarehouseType() { Code = warehouseTypeDetails.Code, Name = warehouseTypeDetails.Name, SearchFields = warehouseTypeDetails.Code + "," + warehouseTypeDetails.Name };
                warehouseTypeRepository.Add(newWarehouseType);
            }
        }
        // CRM
        public static void AddActivityTimeType(ActivityTimeTypeDetails detailsClass, ActivityTimeTypeRepository repository)
        {
            Dictionary<string, ActivityTimeType> dictionary = repository.GetAll().ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(detailsClass.Code))
            {
                ActivityTimeType entity = repository.GetSingle(detailsClass.Code);
                entity.Name = detailsClass.Name;
                entity.SearchFields = detailsClass.Code + "," + detailsClass.Name;
                repository.Update(entity);
            }

            else
            {
                ActivityTimeType newEntity = new ActivityTimeType() { Code = detailsClass.Code, Name = detailsClass.Name, SearchFields = detailsClass.Code + "," + detailsClass.Name };
                repository.Add(newEntity);
            }
        }

        public static void AddActivityType(ActivityTypeDetails detailsClass, ActivityTypeRepository repository)
        {
            Dictionary<string, ActivityType> dictionary = repository.GetAll().ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(detailsClass.Code))
            {
                ActivityType entity = repository.GetSingle(detailsClass.Code);
                entity.Name = detailsClass.Name;
                entity.SearchFields = detailsClass.Code + "," + detailsClass.Name;
                repository.Update(entity);
            }

            else
            {
                ActivityType newEntity = new ActivityType() { Code = detailsClass.Code, Name = detailsClass.Name, SearchFields = detailsClass.Code + "," + detailsClass.Name };
                repository.Add(newEntity);
            }
        }

        public static void AddActivityCallType(ActivityCallTypeDetails detailsClass, CallTypeRepository repository)
        {
            Dictionary<string, CallType> dictionary = repository.GetAll().ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(detailsClass.Code))
            {
                CallType entity = repository.GetSingle(detailsClass.Code);
                entity.Name = detailsClass.Name;
                entity.SearchFields = detailsClass.Code + "," + detailsClass.Name;
                repository.Update(entity);
            }

            else
            {
                CallType newEntity = new CallType() { Code = detailsClass.Code, Name = detailsClass.Name, SearchFields = detailsClass.Code + "," + detailsClass.Name };
                repository.Add(newEntity);
            }
        }

        //public static void AddActivityPriority(ActivityPriorityDetails detailsClass, ActivityPriorityRepository repository)
        //{
        //    Dictionary<string, ActivityPriority> dictionary = repository.GetAll().ToDictionary(d => d.Code, a => a);

        //    if (dictionary.Keys.Contains(detailsClass.Code))
        //    {
        //        ActivityPriority entity = repository.GetSingle(detailsClass.Code);
        //        entity.Name = detailsClass.Name;
        //        entity.SearchFields = detailsClass.Code + "," + detailsClass.Name;
        //        repository.Update(entity);
        //    }

        //    else
        //    {
        //        ActivityPriority newEntity = new ActivityPriority() { Code = detailsClass.Code, Name = detailsClass.Name, SearchFields = detailsClass.Code + "," + detailsClass.Name };
        //        repository.Add(newEntity);
        //    }
        //}

        public static void AddActivityStatus(ActivityStatusDetails detailsClass, ActivityStatusRepository repository)
        {
            Dictionary<string, ActivityStatus> dictionary = repository.GetAll().ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(detailsClass.Code))
            {
                ActivityStatus entity = repository.GetSingle(detailsClass.Code);
                entity.Name = detailsClass.Name;
                entity.SearchFields = detailsClass.Code + "," + detailsClass.Name;
                repository.Update(entity);
            }

            else
            {
                ActivityStatus newEntity = new ActivityStatus() { Code = detailsClass.Code, Name = detailsClass.Name, SearchFields = detailsClass.Code + "," + detailsClass.Name };
                repository.Add(newEntity);
            }
        }

        public static void AddRating(RatingDetails detailsClass, RatingRepository repository)
        {
            Dictionary<string, Rating> dictionary = repository.GetAll().ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(detailsClass.Code))
            {
                Rating entity = repository.GetSingle(detailsClass.Code);
                entity.Name = detailsClass.Name;
                entity.IndexOrder = detailsClass.IndexOrder;
                entity.SearchFields = detailsClass.Code + "," + detailsClass.Name;
                repository.Update(entity);
            }
            else
            {
                Rating newEntity = new Rating() { Code = detailsClass.Code, Name = detailsClass.Name, IndexOrder = detailsClass.IndexOrder, SearchFields = detailsClass.Code + "," + detailsClass.Name };
                repository.Add(newEntity);
            }
        }

        public static void AddVatUniqueTypeMethod(VatUniqueTypeDetails detailsClass, VatUniqueTypeRepository repository)
        {
            Dictionary<string, VatUniqueType> dictionary = repository.GetVatUniqueTypes().ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(detailsClass.Code))
            {
                VatUniqueType entity = repository.GetSingleVatUniqueType(detailsClass.Code);
                entity.Name = detailsClass.Name;
                entity.ViewOrder = detailsClass.ViewOrder;
                entity.SearchFields = detailsClass.Code + "," + detailsClass.Name;
                repository.Update(entity);
            }

            else
            {
                VatUniqueType newEntity = new VatUniqueType()
                {
                    Code = detailsClass.Code,
                    Name = detailsClass.Name,
                    ViewOrder = detailsClass.ViewOrder,
                    SearchFields = detailsClass.Code + "," + detailsClass.Name
                };

                repository.Add(newEntity);
            }
        }

        public static void AddVatMandatoryTypeMethod(VatMandatoryTypeDetails detailsClass, VatMandatoryTypeRepository repository)
        {
            Dictionary<string, VatMandatoryType> dictionary = repository.GetVatMandatoryTypes().ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(detailsClass.Code))
            {
                VatMandatoryType entity = repository.GetSingleVatMandatoryType(detailsClass.Code);
                entity.Name = detailsClass.Name;
                entity.ViewOrder = detailsClass.ViewOrder;
                entity.SearchFields = detailsClass.Code + "," + detailsClass.Name;
                repository.Update(entity);
            }

            else
            {
                VatMandatoryType newEntity = new VatMandatoryType()
                {
                    Code = detailsClass.Code,
                    Name = detailsClass.Name,
                    ViewOrder = detailsClass.ViewOrder,
                    SearchFields = detailsClass.Code + "," + detailsClass.Name
                };

                repository.Add(newEntity);
            }
        }

        public static void AddFeatureAccessLevel(FeatureAccessLevelDetails detailsClass, FeatureAccessLevelRepository repository)
        {
            Dictionary<string, FeatureAccessLevel> dictionary = repository.GetFeatureAccessLevels().ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(detailsClass.Code))
            {
                FeatureAccessLevel entity = repository.GetSingleFeatureAccessLevel(detailsClass.Code);
                entity.Name = detailsClass.Name;
                entity.SearchFields = detailsClass.Code + "," + detailsClass.Name;
                repository.Update(entity);
            }

            else
            {
                FeatureAccessLevel newEntity = new FeatureAccessLevel()
                {
                    Code = detailsClass.Code,
                    Name = detailsClass.Name,
                    SearchFields = detailsClass.Code + "," + detailsClass.Name
                };

                repository.Add(newEntity);
            }
        }

        public static void AddAccountingInformationIdentifier(AccountingInformationIdentifierDetails detailsClass, AccountingInformationIdentifierRepository repository)
        {
            Dictionary<string, AccountingInformationIdentifier> dictionary = repository.GetAccountingInformationIdentifiers().ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(detailsClass.Code))
            {
                AccountingInformationIdentifier entity = repository.GetSingleAccountingInformationIdentifier(detailsClass.Code);
                entity.Name = detailsClass.Name;
                entity.SearchFields = detailsClass.Code + "," + detailsClass.Name;
                repository.Update(entity);
            }

            else
            {
                AccountingInformationIdentifier newEntity = new AccountingInformationIdentifier()
                {
                    Code = detailsClass.Code,
                    Name = detailsClass.Name,
                    SearchFields = detailsClass.Code + "," + detailsClass.Name
                };

                repository.Add(newEntity);
            }
        }

        //Booking        


        public static void AddClosedTableStatus(ClosedTableStatusDetails closedTableStatusDetails, ClosedTableStatusRepository closedTableStatusRepository)
        {
            Dictionary<string, ClosedTableStatus> closedTableStatuses = closedTableStatusRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (closedTableStatuses.Keys.Contains(closedTableStatusDetails.Code))
            {
                ClosedTableStatus closedTableStatus = closedTableStatusRepository.GetSingle(closedTableStatusDetails.Code);
                closedTableStatus.EnglishName = closedTableStatusDetails.EnglishName;
                closedTableStatus.LocalName = closedTableStatusDetails.LocalName;
                closedTableStatus.SearchFields = (closedTableStatusDetails.Code + ',' + closedTableStatus.EnglishName + ',' + closedTableStatusDetails.LocalName).ToLower();
                closedTableStatusRepository.Update(closedTableStatus);
            }
            else
            {
                ClosedTableStatus newClosedTableStatus = new ClosedTableStatus() { Code = closedTableStatusDetails.Code, EnglishName = closedTableStatusDetails.EnglishName, LocalName = closedTableStatusDetails.LocalName, SearchFields = (closedTableStatusDetails.Code + ',' + closedTableStatusDetails.EnglishName + ',' + closedTableStatusDetails.LocalName).ToLower() };
                closedTableStatusRepository.Add(newClosedTableStatus);
            }
        }

        public static void AddCustomsTransportModes(CustomsTransportModeDetails customsTransportModeDetails, CustomsTransportModeRepository customsTransportModeRepository)
        {
            Dictionary<string, CustomsTransportMode> tenantCustomsTransportModes = customsTransportModeRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantCustomsTransportModes.Keys.Contains(customsTransportModeDetails.Code))
            {
                CustomsTransportMode customsTransportMode = customsTransportModeRepository.GetSingle(customsTransportModeDetails.Code);
                customsTransportMode.EnglishName = customsTransportModeDetails.EnglishName;
                customsTransportMode.LocalName = customsTransportModeDetails.LocalName;
                customsTransportMode.SearchFields = (customsTransportModeDetails.Code + "," + customsTransportModeDetails.EnglishName + "," + customsTransportModeDetails.LocalName).ToLower();
                customsTransportModeRepository.Update(customsTransportMode);
            }
            else
            {
                CustomsTransportMode newTransportMode = new CustomsTransportMode() { Code = customsTransportModeDetails.Code, LocalName = customsTransportModeDetails.LocalName, EnglishName = customsTransportModeDetails.EnglishName, SearchFields = (customsTransportModeDetails.Code + "," + customsTransportModeDetails.EnglishName + "," + customsTransportModeDetails.LocalName).ToLower() };
                customsTransportModeRepository.Add(newTransportMode);
            }
        }

        public static void AddCustomsRequestSheetStatuses(Logitude.Customs.Def.ClosedTable.CustomsRequestsSheetStatusDetails customsRequestSheetStatusDetails, CustomsRequestsSheetStatusRepository customsRequestsSheetStatusRepository)
        {
            Dictionary<string, CustomsRequestsSheetStatus> tenantCustomsRequestsSheetStatuses = customsRequestsSheetStatusRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantCustomsRequestsSheetStatuses.Keys.Contains(customsRequestSheetStatusDetails.Code))
            {
                CustomsRequestsSheetStatus customsRequestsSheetStatus = customsRequestsSheetStatusRepository.GetSingle(customsRequestSheetStatusDetails.Code);
                customsRequestsSheetStatus.EnglishName = customsRequestSheetStatusDetails.EnglishName;
                customsRequestsSheetStatus.LocalName = customsRequestSheetStatusDetails.LocalName;
                customsRequestsSheetStatus.SearchFields = (customsRequestSheetStatusDetails.Code + "," + customsRequestSheetStatusDetails.EnglishName + "," + customsRequestSheetStatusDetails.LocalName).ToLower();
                customsRequestsSheetStatusRepository.Update(customsRequestsSheetStatus);
            }
            else
            {
                CustomsRequestsSheetStatus newCustomsRequestsSheetStatus = new CustomsRequestsSheetStatus() { Code = customsRequestSheetStatusDetails.Code, LocalName = customsRequestSheetStatusDetails.LocalName, EnglishName = customsRequestSheetStatusDetails.EnglishName, SearchFields = (customsRequestSheetStatusDetails.Code + "," + customsRequestSheetStatusDetails.EnglishName + "," + customsRequestSheetStatusDetails.LocalName).ToLower() };
                customsRequestsSheetStatusRepository.Add(newCustomsRequestsSheetStatus);
            }
        }

        public static void AddTapagTypes(TapagTypeDetails tapagTypeDetails, TapagTypeRepository tapagTypeRepository)
        {
            Dictionary<string, TapagType> tenantTapagTypes = tapagTypeRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantTapagTypes.Keys.Contains(tapagTypeDetails.Code))
            {
                TapagType tapagType = tapagTypeRepository.GetSingle(tapagTypeDetails.Code);
                tapagType.EnglishName = tapagTypeDetails.EnglishName;
                tapagType.LocalName = tapagTypeDetails.LocalName;
                tapagType.SearchFields = (tapagTypeDetails.Code + "," + tapagTypeDetails.EnglishName + "," + tapagTypeDetails.LocalName).ToLower();
                tapagTypeRepository.Update(tapagType);
            }
            else
            {
                TapagType newTapagType = new TapagType() { Code = tapagTypeDetails.Code, LocalName = tapagTypeDetails.LocalName, EnglishName = tapagTypeDetails.EnglishName, SearchFields = (tapagTypeDetails.Code + "," + tapagTypeDetails.EnglishName + "," + tapagTypeDetails.LocalName).ToLower() };
                tapagTypeRepository.Add(newTapagType);
            }
        }

        public static void AddCustomsEnvoirmentType(CustomsEnvoirmentTypeDetails customsEnvoirmentTypeDetails, CustomsEnvoirmentTypeRepository customsEnvoirmentTypeRepository)
        {
            Dictionary<string, CustomsEnvoirmentType> customsEnvoirmentTypes = customsEnvoirmentTypeRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (customsEnvoirmentTypes.Keys.Contains(customsEnvoirmentTypeDetails.Code))
            {
                CustomsEnvoirmentType customsEnvoirmentType = customsEnvoirmentTypeRepository.GetSingle(customsEnvoirmentTypeDetails.Code);
                customsEnvoirmentType.EnglishName = customsEnvoirmentTypeDetails.EnglishName;
                customsEnvoirmentType.LocalName = customsEnvoirmentTypeDetails.LocalName;
                customsEnvoirmentType.SearchFields = (customsEnvoirmentTypeDetails.Code + ',' + customsEnvoirmentType.EnglishName + ',' + customsEnvoirmentTypeDetails.LocalName).ToLower();
                customsEnvoirmentTypeRepository.Update(customsEnvoirmentType);
            }
            else
            {
                CustomsEnvoirmentType newCustomsEnvoirmentType = new CustomsEnvoirmentType() { Code = customsEnvoirmentTypeDetails.Code, EnglishName = customsEnvoirmentTypeDetails.EnglishName, LocalName = customsEnvoirmentTypeDetails.LocalName, SearchFields = (customsEnvoirmentTypeDetails.Code + ',' + customsEnvoirmentTypeDetails.EnglishName + ',' + customsEnvoirmentTypeDetails.LocalName).ToLower() };
                customsEnvoirmentTypeRepository.Add(newCustomsEnvoirmentType);
            }
        }

        public static void AddNotificationDefinition(Logitude.Customs.Def.ClosedTable.NotificationDefinitionDetails itemDetails, NotificationDefinitionRepository repo)
        {
            var dic = repo.GetAll().ToDictionary(d => d.Code, a => a);

            if (dic.Keys.Contains(itemDetails.Code))
            {
                var updatePoco = repo.GetSingle(itemDetails.Code);
                itemDetails.MapPoco(updatePoco);
                //customsRequestsSheetStatus.SearchFields = customsRequestSheetStatusDetails.Code + "," + customsRequestSheetStatusDetails.EnglishName + "," + customsRequestSheetStatusDetails.LocalName;
                repo.Update(updatePoco);
            }
            else
            {
                var newPoco = new NotificationDefinition();
                itemDetails.MapPoco(newPoco);
                repo.Add(newPoco);
            }
        }

        public static void AddInterfaceSendOption(Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails itemDetails, InterfaceSendOptionRepository repo)
        {
            var dic = repo.GetAll().ToDictionary(d => d.Code, a => a);

            if (dic.Keys.Contains(itemDetails.Code))
            {
                var updatePoco = repo.GetSingle(itemDetails.Code);
                itemDetails.MapPoco(updatePoco);
                
                repo.Update(updatePoco);
            }
            else
            {
                var newPoco = new Logitude.Customs.Def.ClosedTable.InterfaceSendOptionsDetails();
                itemDetails.MapPoco(newPoco);
                repo.Add(newPoco);
        }
        }

        public static void AddInterfaceManagement(Logitude.Customs.Def.ClosedTable.InterfaceManagementDetails itemDetails, InterfaceManagementRepository repo)
        {
            var dic = repo.GetAll().ToDictionary(d => d.Code, a => a);

            if (dic.Keys.Contains(itemDetails.Code))
            {
                var updatePoco = repo.GetSingle(itemDetails.Code);
                itemDetails.MapPoco(updatePoco);

                repo.Update(updatePoco);
            }
            else
            {
                var newPoco = new Logitude.Customs.Def.ClosedTable.InterfaceManagementDetails();
                itemDetails.MapPoco(newPoco);
                repo.Add(newPoco);
            }
        }

        public static void AddAssigneeNotificationType(AssigneeNotificationTypeDetails assigneeNotificationTypeDetails, AssigneeNotificationTypeRepository assigneeNotificationTypeRepository)
        {
            Dictionary<string, AssigneeNotificationType> tenantAssigneeNotificationTypes = assigneeNotificationTypeRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantAssigneeNotificationTypes.Keys.Contains(assigneeNotificationTypeDetails.Code))
            {
                AssigneeNotificationType assigneeNotificationType = assigneeNotificationTypeRepository.GetSingle(assigneeNotificationTypeDetails.Code);
                assigneeNotificationType.EnglishName = assigneeNotificationTypeDetails.EnglishName;
                assigneeNotificationType.LocalName = assigneeNotificationTypeDetails.LocalName;
                assigneeNotificationType.SearchFields = (assigneeNotificationTypeDetails.Code + "," + assigneeNotificationTypeDetails.EnglishName + "," + assigneeNotificationTypeDetails.LocalName).ToLower();
                assigneeNotificationTypeRepository.Update(assigneeNotificationType);
            }
            else
            {
                AssigneeNotificationType newAssigneeNotificationType = new AssigneeNotificationType() { Code = assigneeNotificationTypeDetails.Code, LocalName = assigneeNotificationTypeDetails.LocalName, EnglishName = assigneeNotificationTypeDetails.EnglishName, SearchFields = (assigneeNotificationTypeDetails.Code + "," + assigneeNotificationTypeDetails.EnglishName + "," + assigneeNotificationTypeDetails.LocalName).ToLower() };
                assigneeNotificationTypeRepository.Add(newAssigneeNotificationType);
            }
        }

        public static void AddLastReleaseFromWarehouse(LastReleaseFromWarehouseDetails lastReleaseFromWarehouseDetails, LastReleaseFromWarehouseRepository lastReleaseFromWarehouseRepository)
        {
            Dictionary<string, LastReleaseFromWarehouse> tenantLastReleaseFromWarehouses = lastReleaseFromWarehouseRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantLastReleaseFromWarehouses.Keys.Contains(lastReleaseFromWarehouseDetails.Code))
            {
                LastReleaseFromWarehouse lastReleaseFromWarehouse = lastReleaseFromWarehouseRepository.GetSingle(lastReleaseFromWarehouseDetails.Code);
                lastReleaseFromWarehouse.EnglishName = lastReleaseFromWarehouseDetails.EnglishName;
                lastReleaseFromWarehouse.LocalName = lastReleaseFromWarehouseDetails.LocalName;
                lastReleaseFromWarehouse.SearchFields = (lastReleaseFromWarehouseDetails.Code + "," + lastReleaseFromWarehouseDetails.EnglishName + "," + lastReleaseFromWarehouseDetails.LocalName).ToLower();
                lastReleaseFromWarehouseRepository.Update(lastReleaseFromWarehouse);
            }
            else
            {
                LastReleaseFromWarehouse newLastReleaseFromWarehouse = new LastReleaseFromWarehouse() { Code = lastReleaseFromWarehouseDetails.Code, LocalName = lastReleaseFromWarehouseDetails.LocalName, EnglishName = lastReleaseFromWarehouseDetails.EnglishName, SearchFields = (lastReleaseFromWarehouseDetails.Code + "," + lastReleaseFromWarehouseDetails.EnglishName + "," + lastReleaseFromWarehouseDetails.LocalName).ToLower() };
                lastReleaseFromWarehouseRepository.Add(newLastReleaseFromWarehouse);
            }
            }

        public static void AddVehicleStatus(VehicleStatusDetails vehicleStatusDetails, VehicleStatusRepository vehicleStatusRepository)
        {
            Dictionary<string, VehicleStatus> tenantVehicleStatuses = vehicleStatusRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantVehicleStatuses.Keys.Contains(vehicleStatusDetails.Code))
            {
                VehicleStatus vehicleStatus = vehicleStatusRepository.GetSingle(vehicleStatusDetails.Code);
                vehicleStatus.EnglishName = vehicleStatusDetails.EnglishName;
                vehicleStatus.LocalName = vehicleStatusDetails.LocalName;
                vehicleStatus.SearchFields = (vehicleStatusDetails.Code + "," + vehicleStatusDetails.EnglishName + "," + vehicleStatusDetails.LocalName).ToLower();
                vehicleStatusRepository.Update(vehicleStatus);
            }
            else
            {
                VehicleStatus newVehicleStatus = new VehicleStatus() { Code = vehicleStatusDetails.Code, LocalName = vehicleStatusDetails.LocalName, EnglishName = vehicleStatusDetails.EnglishName, SearchFields = (vehicleStatusDetails.Code + "," + vehicleStatusDetails.EnglishName + "," + vehicleStatusDetails.LocalName).ToLower() };
                vehicleStatusRepository.Add(newVehicleStatus);
            }
        }

        public static void AddVehicleSafetyAccessoryInstallationType(VehicleSafeAccessoryInstlTypeDetails VehicleSafetyAccessoryInstallationTypeDetails, VehicleSafeAccessoryInstlTypeRepository VehicleSafeAccessoryInstlTypeRepository)
        {
            Dictionary<string, VehicleSafeAccessoryInstlType> tenantVehicleSafetyAccessoryInstallationTypees = VehicleSafeAccessoryInstlTypeRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantVehicleSafetyAccessoryInstallationTypees.Keys.Contains(VehicleSafetyAccessoryInstallationTypeDetails.Code))
            {
                VehicleSafeAccessoryInstlType vehicleSafetyAccessoryInstallationType = VehicleSafeAccessoryInstlTypeRepository.GetSingle(VehicleSafetyAccessoryInstallationTypeDetails.Code);
                vehicleSafetyAccessoryInstallationType.EnglishName = VehicleSafetyAccessoryInstallationTypeDetails.EnglishName;
                vehicleSafetyAccessoryInstallationType.LocalName = VehicleSafetyAccessoryInstallationTypeDetails.LocalName;
                vehicleSafetyAccessoryInstallationType.SearchFields = (VehicleSafetyAccessoryInstallationTypeDetails.Code + "," + VehicleSafetyAccessoryInstallationTypeDetails.EnglishName + "," + VehicleSafetyAccessoryInstallationTypeDetails.LocalName).ToLower();
                VehicleSafeAccessoryInstlTypeRepository.Update(vehicleSafetyAccessoryInstallationType);
            }
            else
            {
                VehicleSafeAccessoryInstlType newVehicleSafetyAccessoryInstallationType = new VehicleSafeAccessoryInstlType() { Code = VehicleSafetyAccessoryInstallationTypeDetails.Code, LocalName = VehicleSafetyAccessoryInstallationTypeDetails.LocalName, EnglishName = VehicleSafetyAccessoryInstallationTypeDetails.EnglishName, SearchFields = (VehicleSafetyAccessoryInstallationTypeDetails.Code + "," + VehicleSafetyAccessoryInstallationTypeDetails.EnglishName + "," + VehicleSafetyAccessoryInstallationTypeDetails.LocalName).ToLower() };
                VehicleSafeAccessoryInstlTypeRepository.Add(newVehicleSafetyAccessoryInstallationType);
            }
        }

        public static void AddCustomerIdentifyType(CustomerIdentifyTypeDetails CustomerIdentifyTypeDetails, CustomerIdentifyTypeRepository CustomerIdentifyTypeRepository)
        {
            Dictionary<string, CustomerIdentifyType> tenantCustomerIdentifyTypes = CustomerIdentifyTypeRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantCustomerIdentifyTypes.Keys.Contains(CustomerIdentifyTypeDetails.Code))
            {
                CustomerIdentifyType customerIdentifyType = CustomerIdentifyTypeRepository.GetSingle(CustomerIdentifyTypeDetails.Code);
                customerIdentifyType.EnglishName = CustomerIdentifyTypeDetails.EnglishName;
                customerIdentifyType.LocalName = CustomerIdentifyTypeDetails.LocalName;
                customerIdentifyType.SearchFields = (CustomerIdentifyTypeDetails.Code + "," + CustomerIdentifyTypeDetails.EnglishName + "," + CustomerIdentifyTypeDetails.LocalName).ToLower();
                CustomerIdentifyTypeRepository.Update(customerIdentifyType);
            }
            else
            {
                CustomerIdentifyType newCustomerIdentifyType = new CustomerIdentifyType() { Code = CustomerIdentifyTypeDetails.Code, LocalName = CustomerIdentifyTypeDetails.LocalName, EnglishName = CustomerIdentifyTypeDetails.EnglishName, SearchFields = (CustomerIdentifyTypeDetails.Code + "," + CustomerIdentifyTypeDetails.EnglishName + "," + CustomerIdentifyTypeDetails.LocalName).ToLower() };
                CustomerIdentifyTypeRepository.Add(newCustomerIdentifyType);
            }
            }

        public static void AddManifestStatus(ManifestStatusDetails myDetails, ManifestStatusRepository myRepository)
        {
            Dictionary<string, ManifestStatus> myDictionary = myRepository.GetManifestStatus().ToDictionary(d => d.Code, a => a);

            if (myDictionary.Keys.Contains(myDetails.Code))
            {
                ManifestStatus myPOCO = myRepository.GetSingleManifestStatus(myDetails.Code);
                myPOCO.Name = myDetails.Name;
                myPOCO.SearchFields = myDetails.Code + "," + myDetails.Name;
                myRepository.Update(myPOCO);
            }

            else
            {
                ManifestStatus myPOCO = new ManifestStatus()
                {
                    Code = myDetails.Code,
                    Name = myDetails.Name,
                    SearchFields = myDetails.Code + "," + myDetails.Name
                };

                myRepository.Add(myPOCO);
            }
        }



        public static void AddLoginPolicy(LoginPolicyDetails myDetails, LoginPolicyRepository myRepository)
        {
            Dictionary<string, LoginPolicy> myDictionary = myRepository.GetLoginPolicies().ToDictionary(d => d.Code, a => a);

            if (myDictionary.Keys.Contains(myDetails.Code))
            {
                LoginPolicy myPOCO = myRepository.GetSingleLoginPolicy(myDetails.Code);
                myPOCO.Name = myDetails.Name;
                myPOCO.SearchFields = myDetails.Code + "," + myDetails.Name;
                myRepository.Update(myPOCO);
            }

            else
            {
                LoginPolicy myPOCO = new LoginPolicy()
                {
                    Code = myDetails.Code,
                    Name = myDetails.Name,
                    SearchFields = myDetails.Code + "," + myDetails.Name
                };

                myRepository.Add(myPOCO);
            }
        }


        public static void AddMetodoPago(MetodoPagoDetails myDetails, MetodoPagoRepository myRepository)
        {
            Dictionary<string, MetodoPago> myDictionary = myRepository.GetMetodoPagos().ToDictionary(d => d.Code, a => a);

            if (myDictionary.Keys.Contains(myDetails.Code))
            {
                MetodoPago myPOCO = myRepository.GetSingleMetodoPago(myDetails.Code);
                myPOCO.Name = myDetails.Name;
                myPOCO.SearchFields = myDetails.Code + "," + myDetails.Name;
                myRepository.Update(myPOCO);
            }

            else
            {
                MetodoPago myPOCO = new MetodoPago()
                {
                    Code = myDetails.Code,
                    Name = myDetails.Name,
                    SearchFields = myDetails.Code + "," + myDetails.Name
                };

                myRepository.Add(myPOCO);
            }
        }


        public static void AddSATTransferStatus(SATTransferStatusDetails myDetails, SATTransferStatusRepository myRepository)
        {
            Dictionary<string, SATTransferStatus> myDictionary = myRepository.GetSATTransferStatus().ToDictionary(d => d.Code, a => a);

            if (myDictionary.Keys.Contains(myDetails.Code))
            {
                SATTransferStatus myPOCO = myRepository.GetSingleSATTransferStatus(myDetails.Code);
                myPOCO.Name = myDetails.Name;
                myPOCO.SearchFields = myDetails.Code + "," + myDetails.Name;
                myRepository.Update(myPOCO);
            }

            else
            {
                SATTransferStatus myPOCO = new SATTransferStatus()
                {
                    Code = myDetails.Code,
                    Name = myDetails.Name,
                    SearchFields = myDetails.Code + "," + myDetails.Name
                };

                myRepository.Add(myPOCO);
            }
        }



        public static void AddSATInvoiceStatus(SATInvoiceStatusDetails myDetails, SATInvoiceStatusRepository myRepository)
        {
            Dictionary<string, SATInvoiceStatus> myDictionary = myRepository.GetSATInvoiceStatus().ToDictionary(d => d.Code, a => a);

            if (myDictionary.Keys.Contains(myDetails.Code))
            {
                SATInvoiceStatus myPOCO = myRepository.GetSingleSATInvoiceStatus(myDetails.Code);
                myPOCO.Name = myDetails.Name;
                myPOCO.SearchFields = myDetails.Code + "," + myDetails.Name;
                myRepository.Update(myPOCO);
            }

            else
            {
                SATInvoiceStatus myPOCO = new SATInvoiceStatus()
                {
                    Code = myDetails.Code,
                    Name = myDetails.Name,
                    SearchFields = myDetails.Code + "," + myDetails.Name
                };

                myRepository.Add(myPOCO);
            }
        }


        public static void AddUsoCFDI(UsoCFDIDetails myDetails, UsoCFDIRepository myRepository)
        {
            Dictionary<string, UsoCFDI> myDictionary = myRepository.GetUsoCFDIs().ToDictionary(d => d.Code, a => a);

            if (myDictionary.Keys.Contains(myDetails.Code))
            {
                UsoCFDI myPOCO = myRepository.GetSingleUsoCFDI(myDetails.Code);
                myPOCO.Name = myDetails.Name;
                myPOCO.SearchFields = myDetails.Code + "," + myDetails.Name;
                myRepository.Update(myPOCO);
            }

            else
            {
                UsoCFDI myPOCO = new UsoCFDI()
                {
                    Code = myDetails.Code,
                    Name = myDetails.Name,
                    SearchFields = myDetails.Code + "," + myDetails.Name
                };

                myRepository.Add(myPOCO);
            }
        }


        public static void AddCustomsTransmissionsStatus(CustomsTransmissionsStatusDetails myDetails, CustomsTransmissionsStatusRepository myRepository)
        {
            Dictionary<string, CustomsTransmissionsStatus> myDictionary = myRepository.GetCustomsTransmissionsStatus().ToDictionary(d => d.Code, a => a);

            if (myDictionary.Keys.Contains(myDetails.Code))
            {
                CustomsTransmissionsStatus myPOCO = myRepository.GetSingleCustomsTransmissionsStatus(myDetails.Code);
                myPOCO.Name = myDetails.Name;
                myPOCO.SearchFields = myDetails.Code + "," + myDetails.Name;
                myRepository.Update(myPOCO);
            }

            else
            {
                CustomsTransmissionsStatus myPOCO = new CustomsTransmissionsStatus()
                {
                    Code = myDetails.Code,
                    Name = myDetails.Name,
                    SearchFields = myDetails.Code + "," + myDetails.Name
                };

                myRepository.Add(myPOCO);
            }
        }

        public static void AddSignatureType(SignatureTypeDetails SignatureTypeDetails, SignatureTypeRepository SignatureTypeRepository)
        {
            Dictionary<string, SignatureType> tenantSignatureTypes = SignatureTypeRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantSignatureTypes.Keys.Contains(SignatureTypeDetails.Code))
            {
                SignatureType signatureType = SignatureTypeRepository.GetSingle(SignatureTypeDetails.Code);
                signatureType.EnglishName = SignatureTypeDetails.EnglishName;
                signatureType.LocalName = SignatureTypeDetails.LocalName;
                signatureType.SearchFields = (SignatureTypeDetails.Code + "," + SignatureTypeDetails.EnglishName + "," + SignatureTypeDetails.LocalName).ToLower();
                SignatureTypeRepository.Update(signatureType);
            }
            else
            {
                SignatureType newSignatureType = new SignatureType() { Code = SignatureTypeDetails.Code, LocalName = SignatureTypeDetails.LocalName, EnglishName = SignatureTypeDetails.EnglishName, SearchFields = (SignatureTypeDetails.Code + "," + SignatureTypeDetails.EnglishName + "," + SignatureTypeDetails.LocalName).ToLower() };
                SignatureTypeRepository.Add(newSignatureType);
            }
        }


        // Accounting
        public static void AddJournalStatusType(JournalStatusTypeDetails JournalStatusTypeDetails, JournalStatusTypeRepository JournalStatusTypeRepository)
        {
            Dictionary<string, JournalStatusType> tenantJournalStatusTypes = JournalStatusTypeRepository.GetAll().ToDictionary(d => d.JournalStatusID, a => a);

            if (tenantJournalStatusTypes.Keys.Contains(JournalStatusTypeDetails.JournalStatusID))
            {
                JournalStatusType journalStatusType = JournalStatusTypeRepository.GetSingle(JournalStatusTypeDetails.JournalStatusID);
                journalStatusType.EnglishName = JournalStatusTypeDetails.EnglishName;
                journalStatusType.LocalName = JournalStatusTypeDetails.LocalName;
                journalStatusType.SearchFields = JournalStatusTypeDetails.JournalStatusID + "," + JournalStatusTypeDetails.EnglishName + "," + JournalStatusTypeDetails.LocalName;
                journalStatusType.Inactive = JournalStatusTypeDetails.Inactive;
                JournalStatusTypeRepository.Update(journalStatusType);
            }
            else
            {
                JournalStatusType newJournalStatusType = new JournalStatusType() { JournalStatusID = JournalStatusTypeDetails.JournalStatusID, LocalName = JournalStatusTypeDetails.LocalName, EnglishName = JournalStatusTypeDetails.EnglishName, SearchFields = JournalStatusTypeDetails.JournalStatusID + "," + JournalStatusTypeDetails.EnglishName + "," + JournalStatusTypeDetails.LocalName, Inactive = false };
                JournalStatusTypeRepository.Add(newJournalStatusType);
            }
        }
        
        public static void AddJournalType(JournalTypeDetails JournalTypeDetails, JournalTypeRepository JournalTypeRepository)
        {
            Dictionary<string, JournalType> tenantJournalTypes = JournalTypeRepository.GetAll().ToDictionary(d => d.JournalTypeID, a => a);

            if (tenantJournalTypes.Keys.Contains(JournalTypeDetails.JournalTypeID))
            {
                JournalType journalType = JournalTypeRepository.GetSingle(JournalTypeDetails.JournalTypeID);
                journalType.EnglishName = JournalTypeDetails.EnglishName;
                journalType.LocalName = JournalTypeDetails.LocalName;
                journalType.SearchFields = JournalTypeDetails.JournalTypeID + "," + JournalTypeDetails.EnglishName + "," + JournalTypeDetails.LocalName;
                journalType.Inactive = JournalTypeDetails.Inactive;
                JournalTypeRepository.Update(journalType);
            }
            else
            {
                JournalType newJournalType = new JournalType() { JournalTypeID = JournalTypeDetails.JournalTypeID, LocalName = JournalTypeDetails.LocalName, EnglishName = JournalTypeDetails.EnglishName, SearchFields = JournalTypeDetails.JournalTypeID + "," + JournalTypeDetails.EnglishName + "," + JournalTypeDetails.LocalName, Inactive = false };
                JournalTypeRepository.Add(newJournalType);
            }
        }

        public static void AddChartOfAccountsType(ChartOfAccountsTypeDetails chartOfAccountsTypeDetails, ChartOfAccountsTypeRepository chartOfAccountsTypeRepository)
        {
            Dictionary<string, ChartOfAccountsType> tenantChartOfAccountsType = chartOfAccountsTypeRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantChartOfAccountsType.Keys.Contains(chartOfAccountsTypeDetails.Code))
            {
                ChartOfAccountsType chartOfAccountsType = chartOfAccountsTypeRepository.GetSingle(chartOfAccountsTypeDetails.Code);
                chartOfAccountsType.EnglishName = chartOfAccountsTypeDetails.EnglishName;
                chartOfAccountsType.LocalName = chartOfAccountsTypeDetails.LocalName;
                chartOfAccountsType.Inactive = chartOfAccountsTypeDetails.Inactive;
                chartOfAccountsType.SearchFields = chartOfAccountsTypeDetails.Code + ',' + chartOfAccountsTypeDetails.EnglishName + chartOfAccountsTypeDetails.LocalName;
                chartOfAccountsTypeRepository.Update(chartOfAccountsType);
            }
            else
            {
                ChartOfAccountsType newChartOfAccountsType = new ChartOfAccountsType() { Code = chartOfAccountsTypeDetails.Code, LocalName = chartOfAccountsTypeDetails.LocalName, EnglishName = chartOfAccountsTypeDetails.EnglishName,  Inactive = false };
                chartOfAccountsTypeRepository.Add(newChartOfAccountsType);
            }
        }

        public static void AddJournalActionType(JournalActionTypeDetails journalActionTypeDetails, JournalActionTypeRepository journalActionTypeRepository)
        {
            Dictionary<string, JournalActionType> tenantJournalActionType = journalActionTypeRepository.GetAll(0).ToDictionary(d => d.Id, a => a);

            if (tenantJournalActionType.Keys.Contains(journalActionTypeDetails.Code))
            {
                JournalActionType journalActionType = journalActionTypeRepository.GetSingle(journalActionTypeDetails.JournalActionTypeID,0);
                journalActionType.EnglishName = journalActionTypeDetails.EnglishName;
                journalActionType.LocalName = journalActionTypeDetails.LocalName;
                journalActionType.Code = journalActionTypeDetails.Code;
                journalActionType.Tenant = journalActionTypeDetails.Tenant;
                journalActionType.SearchFields = journalActionTypeDetails.Code + "," + journalActionTypeDetails.EnglishName + "," + journalActionTypeDetails.LocalName;    
                journalActionType.Inactive = journalActionTypeDetails.Inactive;
                journalActionTypeRepository.Update(journalActionType);
            }
            else
            {
                JournalActionType newjournalActionType = new JournalActionType() { Code = journalActionTypeDetails.Code, Id = journalActionTypeDetails.JournalActionTypeID, LocalName = journalActionTypeDetails.LocalName, EnglishName = journalActionTypeDetails.EnglishName, Inactive = false };
                journalActionTypeRepository.Add(newjournalActionType);
            }
        }
        
        public static void AddAccountingEntity(AccountingEntityDetails accountingEntityDetails, AccountingEntityRepository accountingEntityRepository)
        {
            Dictionary<string, AccountingEntity> tenantAccountingEntity = accountingEntityRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantAccountingEntity.Keys.Contains(accountingEntityDetails.Code))
            {
                AccountingEntity accountingEntity = accountingEntityRepository.GetSingle(accountingEntityDetails.Code);
                accountingEntity.EnglishName = accountingEntityDetails.EnglishName;
                accountingEntity.LocalName = accountingEntityDetails.LocalName;
                accountingEntity.Code = accountingEntityDetails.Code;
               
            }
            else
            {
                AccountingEntity newAccountingEntity = new AccountingEntity() { Code = accountingEntityDetails.Code, LocalName = accountingEntityDetails.LocalName, EnglishName = accountingEntityDetails.EnglishName};
                accountingEntityRepository.Add(newAccountingEntity);
            }
        }
        
        public static void AddGLAccountType(GLAccountTypeDetails gLAccountTypeDetails, GLAccountTypeRepository gLAccountTypeRepository)
        {
            Dictionary<string, GLAccountType> tenantGLAccountTypes =  gLAccountTypeRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantGLAccountTypes.Keys.Contains(gLAccountTypeDetails.Code))
            {
                GLAccountType type = gLAccountTypeRepository.GetSingle(gLAccountTypeDetails.Code);
                type.EnglishName = gLAccountTypeDetails.EnglishName;
                type.LocalName = gLAccountTypeDetails.LocalName;
                //type.SearchFields = gLAccountTypeDetails.JournalTypeID + "," + gLAccountTypeDetails.EnglishName + "," + gLAccountTypeDetails.LocalName;
                type.Inactive = gLAccountTypeDetails.Inactive;
                gLAccountTypeRepository.Update(type);
            }
            else
            {
                GLAccountType newType = new GLAccountType() { Code = gLAccountTypeDetails.Code, LocalName = gLAccountTypeDetails.LocalName, EnglishName = gLAccountTypeDetails.EnglishName, Inactive = false };//, SearchFields = gLAccountTypeDetails.ode + "," + gLAccountTypeDetails.EnglishName + "," + gLAccountTypeDetails.LocalName };
                gLAccountTypeRepository.Add(newType);
            }
        }
        
        public static void AddRevenueExpenseType(RevenueExpenseTypeDetails revenueExpenseTypeDetails, RevenueExpenseTypeRepository revenueExpenseTypeRepository)
        {
            Dictionary<string, RevenueExpenseType> tenantRevenueExpenseTypes = revenueExpenseTypeRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantRevenueExpenseTypes.Keys.Contains(revenueExpenseTypeDetails.Code))
            {
                RevenueExpenseType type = revenueExpenseTypeRepository.GetSingle(revenueExpenseTypeDetails.Code);
                type.EnglishName = revenueExpenseTypeDetails.EnglishName;
                type.LocalName = revenueExpenseTypeDetails.LocalName;
                // type.SearchFields = revenueExpenseTypeDetails.JournalTypeID + "," + revenueExpenseTypeDetails.EnglishName + "," + revenueExpenseTypeDetails.LocalName;
                type.Inactive = revenueExpenseTypeDetails.Inactive;
                revenueExpenseTypeRepository.Update(type);
            }
            else
            {
                RevenueExpenseType newType = new RevenueExpenseType() { Code = revenueExpenseTypeDetails.Code, LocalName = revenueExpenseTypeDetails.LocalName, EnglishName = revenueExpenseTypeDetails.EnglishName, Inactive = false };//, SearchFields = revenueExpenseTypeDetails.ode + "," + revenueExpenseTypeDetails.EnglishName + "," + revenueExpenseTypeDetails.LocalName };
                revenueExpenseTypeRepository.Add(newType);
            }
        }

        public static void AddReconcileMethod(ReconcileMethodDetails reconcileMethodDetails, ReconcileMethodRepository reconcileMethodRepository)
        {
            Dictionary<string, ReconcileMethod> tenantReconcileMethods = reconcileMethodRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantReconcileMethods.Keys.Contains(reconcileMethodDetails.Code))
            {
                ReconcileMethod type = reconcileMethodRepository.GetSingle(reconcileMethodDetails.Code);
                type.EnglishName = reconcileMethodDetails.EnglishName;
                type.LocalName = reconcileMethodDetails.LocalName;
                type.SearchFields = reconcileMethodDetails.Code + "," + reconcileMethodDetails.EnglishName + "," + reconcileMethodDetails.LocalName;
                type.Inactive = reconcileMethodDetails.Inactive;
                reconcileMethodRepository.Update(type);
            }
            else
            {
                ReconcileMethod newType = new ReconcileMethod() { Code = reconcileMethodDetails.Code, LocalName = reconcileMethodDetails.LocalName, EnglishName = reconcileMethodDetails.EnglishName, Inactive = false, SearchFields = reconcileMethodDetails.Code + "," + reconcileMethodDetails.EnglishName + "," + reconcileMethodDetails.LocalName };
                reconcileMethodRepository.Add(newType);
            }
        }

        public static void AddPeriodType(PeriodTypeDetails periodTypeDetails, PeriodTypeRepository periodTypeRepository)
        {
            Dictionary<string, PeriodType> tenantPeriodTypes = periodTypeRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantPeriodTypes.Keys.Contains(periodTypeDetails.Code))
            {
                PeriodType type = periodTypeRepository.GetSingle(periodTypeDetails.Code);
                type.EnglishName = periodTypeDetails.EnglishName;
                type.LocalName = periodTypeDetails.LocalName;
                type.SearchFields = periodTypeDetails.Code + "," + periodTypeDetails.EnglishName + "," + periodTypeDetails.LocalName;
                type.Inactive = periodTypeDetails.Inactive;
                periodTypeRepository.Update(type);
            }
            else
            {
                PeriodType newType = new PeriodType() { Code = periodTypeDetails.Code, LocalName = periodTypeDetails.LocalName, EnglishName = periodTypeDetails.EnglishName, Inactive = false, SearchFields = periodTypeDetails.Code + "," + periodTypeDetails.EnglishName + "," + periodTypeDetails.LocalName };
                periodTypeRepository.Add(newType);
            }
        }
        
        public static void AddAutomaticReconcile(AutomaticReconcileDetails automaticReconcileDetails, AutomaticReconcileRepository automaticReconcileRepository)
        {
            Dictionary<string, AutomaticReconcile> tenantAutomaticReconciles = automaticReconcileRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantAutomaticReconciles.Keys.Contains(automaticReconcileDetails.Code))
            {
                AutomaticReconcile type = automaticReconcileRepository.GetSingle(automaticReconcileDetails.Code);
                type.EnglishName = automaticReconcileDetails.EnglishName;
                type.LocalName = automaticReconcileDetails.LocalName;
                type.SearchFields = automaticReconcileDetails.Code + "," + automaticReconcileDetails.EnglishName + "," + automaticReconcileDetails.LocalName;
                type.Inactive = automaticReconcileDetails.Inactive;
                automaticReconcileRepository.Update(type);
            }
            else
            {
                AutomaticReconcile newType = new AutomaticReconcile() { Code = automaticReconcileDetails.Code, LocalName = automaticReconcileDetails.LocalName, EnglishName = automaticReconcileDetails.EnglishName, Inactive = false, SearchFields = automaticReconcileDetails.Code + "," + automaticReconcileDetails.EnglishName + "," + automaticReconcileDetails.LocalName };
                automaticReconcileRepository.Add(newType);
            }
        }
        // end accounting
        
        public static void AddCertificatesStatus(CertificatesStatusDetails certificatesStatusDetails, CertificatesStatusRepository certificatesStatusRepository)
        {
            Dictionary<string, CertificatesStatus> tenantCertificatesStatuses = certificatesStatusRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantCertificatesStatuses.Keys.Contains(certificatesStatusDetails.Code))
            {
                CertificatesStatus certificatesStatus = certificatesStatusRepository.GetSingle(certificatesStatusDetails.Code);
                certificatesStatus.EnglishName = certificatesStatusDetails.EnglishName;
                certificatesStatus.LocalName = certificatesStatusDetails.LocalName;
                certificatesStatus.SearchFields = (certificatesStatusDetails.Code + "," + certificatesStatusDetails.EnglishName + "," + certificatesStatusDetails.LocalName).ToLower();
                certificatesStatusRepository.Update(certificatesStatus);
            }
            else
            {
                CertificatesStatus newCertificatesStatus = new CertificatesStatus() { Code = certificatesStatusDetails.Code, LocalName = certificatesStatusDetails.LocalName, EnglishName = certificatesStatusDetails.EnglishName, SearchFields = (certificatesStatusDetails.Code + "," + certificatesStatusDetails.EnglishName + "," + certificatesStatusDetails.LocalName).ToLower() };
                certificatesStatusRepository.Add(newCertificatesStatus);
            }
        }

        public static void AddTenantTypes(TenantTypeDetails tenantTypeDetails, TenantTypeRepository tenantTypeRepository)
        {
            Dictionary<string, TenantType> tenantTenantTypes = tenantTypeRepository.GetTenantTypes().ToDictionary(d => d.Code, a => a);

            if (tenantTenantTypes.Keys.Contains(tenantTypeDetails.Code))
            {
                TenantType tenantType = tenantTypeRepository.GetSingleTenantType(tenantTypeDetails.Code);
                tenantType.Name = tenantTypeDetails.Name;
                tenantType.SearchFields = tenantTypeDetails.Code + "," + tenantTypeDetails.Name;
                tenantTypeRepository.Update(tenantType);
            }
            else
            {
                TenantType tenantType = new TenantType() { Code = tenantTypeDetails.Code, Name = tenantTypeDetails.Name, SearchFields = tenantTypeDetails.Code + "," + tenantTypeDetails.Name };
                tenantTypeRepository.Add(tenantType);
            }
        }

        public static void AddVatFormatTypeMethod(VatFormatTypeDetails detailsClass, VatFormatTypeRepository repository)
        {
            Dictionary<string, VatFormatType> dictionary = repository.GetVatFormatTypes().ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(detailsClass.Code))
            {
                VatFormatType entity = repository.GetSingleVatFormatType(detailsClass.Code);
                entity.Name = detailsClass.Name;
                entity.ViewOrder = detailsClass.ViewOrder;
                entity.SearchFields = detailsClass.Code + "," + detailsClass.Name;
                repository.Update(entity);
            }

            else
            {
                VatFormatType newEntity = new VatFormatType()
                {
                    Code = detailsClass.Code,
                    Name = detailsClass.Name,
                    ViewOrder = detailsClass.ViewOrder,
                    SearchFields = detailsClass.Code + "," + detailsClass.Name
                };

                repository.Add(newEntity);
            }
        }

        public static void AddOtherParticipantId(CodeNameDetails myDetails, OtherParticipantIdRepository myRepository)
        {
            Dictionary<string, OtherParticipantId> myDictionary = myRepository.GetOtherParticipantIds().ToDictionary(d => d.Code, a => a);

            if (myDictionary.Keys.Contains(myDetails.Code))
            {
                OtherParticipantId myPOCO = myRepository.GetSingleOtherParticipantId(myDetails.Code);
                myPOCO.Name = myDetails.Name;
                myPOCO.SearchFields = myDetails.Code + "," + myDetails.Name;
                myRepository.Update(myPOCO);
            }

            else
            {
                OtherParticipantId myPOCO = new OtherParticipantId()
                {
                    Code = myDetails.Code,
                    Name = myDetails.Name,
                    SearchFields = myDetails.Code + "," + myDetails.Name
                };

                myRepository.Add(myPOCO);
            }
        }
        
        public static void AddARInvoiceLineAction(CodeNameDetails myDetails, ARInvoiceLineActionRepository myRepository)
        {
            Dictionary<string, ARInvoiceLineAction> myDictionary = myRepository.GetARInvoiceLineActions().ToDictionary(d => d.Code, a => a);

            if (myDictionary.Keys.Contains(myDetails.Code))
            {
                ARInvoiceLineAction myPOCO = myRepository.GetSingleARInvoiceLineAction(myDetails.Code);
                myPOCO.Name = myDetails.Name;
                myPOCO.LocalName = myDetails.LocalName;
                myPOCO.Inactive = myDetails.Inactive;
                myPOCO.SearchFields = myDetails.Code + "," + myDetails.Name + "," + myDetails.LocalName;
                myRepository.Update(myPOCO);
            }

            else
            {
                ARInvoiceLineAction myPOCO = new ARInvoiceLineAction()
                {
                    Code = myDetails.Code,
                    Name = myDetails.Name,
                    LocalName = myDetails.LocalName,
                    Inactive = myDetails.Inactive,
                    SearchFields = myDetails.Code + "," + myDetails.Name + "," + myDetails.LocalName,
                };

                myRepository.Add(myPOCO);
            }
        }

        public static void AddCustomsInterfaces(CustomsInterfaceDetails customsInterfaceDetails, CustomsInterfaceRepository customsInterfaceRepository)
        {
            Dictionary<string, CustomsInterface> tenantCustomsInterfaces = customsInterfaceRepository.GetCustomsInterfaces().ToDictionary(d => d.Code, a => a);

            if (tenantCustomsInterfaces.Keys.Contains(customsInterfaceDetails.Code))
            {
                CustomsInterface customsInterface = customsInterfaceRepository.GetSingleCustomsInterface(customsInterfaceDetails.Code);
                customsInterface.Name = customsInterfaceDetails.Name;
                customsInterface.SearchFields = customsInterfaceDetails.Code + "," + customsInterfaceDetails.Name;
                customsInterface.InterfaceType = customsInterfaceDetails.InterfaceType;
                customsInterfaceRepository.Update(customsInterface);
            }
            else
            {
                CustomsInterface customsInterface = new CustomsInterface()
                {
                    Code = customsInterfaceDetails.Code,
                    Name = customsInterfaceDetails.Name,
                    SearchFields = customsInterfaceDetails.Code + "," + customsInterfaceDetails.Name,
                    InterfaceType = customsInterfaceDetails.InterfaceType,
                };

                customsInterfaceRepository.Add(customsInterface);
            }
        }

        public static void AddSATInterface(SATInterfaceDetails sATInterfaceDetails, SATInterfaceRepository sATInterfaceRepository)
        {
            Dictionary<string, SATInterface> sATInterfaces = sATInterfaceRepository.GetSATInterfaces().ToDictionary(d => d.Code, a => a);

            if (sATInterfaces.Keys.Contains(sATInterfaceDetails.Code))
            {
                SATInterface sATInterface = sATInterfaceRepository.GetSingleSATInterface(sATInterfaceDetails.Code);
                sATInterface.Name = sATInterfaceDetails.Name;
                sATInterface.SearchFields = (sATInterfaceDetails.Code + "," + sATInterfaceDetails.Name).ToLower();
                sATInterfaceRepository.Update(sATInterface);
            }
            else
            {
                SATInterface sATInterface = new SATInterface() { Code = sATInterfaceDetails.Code, Name = sATInterfaceDetails.Name, SearchFields = (sATInterfaceDetails.Code + "," + sATInterfaceDetails.Name).ToLower() };
                sATInterfaceRepository.Add(sATInterface);
            }
        }


        public static void AddSATPaymentMethods(SATPaymentMethodDetails paymentMethodDetails, SATPaymentMethodRepository sATPaymentMethodRepository)
        {
            Dictionary<string, SATPaymentMethod> tenantPaymentMethods = sATPaymentMethodRepository.GetSATPaymentMethods().ToDictionary(d => d.Code, a => a);

            if (tenantPaymentMethods.Keys.Contains(paymentMethodDetails.Code))
            {
                SATPaymentMethod paymentMethod = sATPaymentMethodRepository.GetSingleSATPaymentMethod(paymentMethodDetails.Code);
                paymentMethod.Name = paymentMethodDetails.Name;
                paymentMethod.LocalName = paymentMethodDetails.LocalName;
                paymentMethod.SearchFields = (paymentMethodDetails.Code + "," + paymentMethodDetails.Name).ToLower();
                sATPaymentMethodRepository.Update(paymentMethod);
            }
            else
            {
                SATPaymentMethod newPaymentMethod = new SATPaymentMethod() { Code = paymentMethodDetails.Code, Name = paymentMethodDetails.Name, LocalName  = paymentMethodDetails.LocalName,SearchFields = (paymentMethodDetails.Code + "," + paymentMethodDetails.Name + "," + paymentMethodDetails.LocalName).ToLower() };
                sATPaymentMethodRepository.Add(newPaymentMethod);
            }
        }


        public static void AddAccumalationState(AccumalationStateDetails accumalationStateDetails, AccumalationStateRepository accumalationStateRepository)
        {
            Dictionary<string, AccumalationState> tenantAccumalationStates = accumalationStateRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantAccumalationStates.Keys.Contains(accumalationStateDetails.Code))
            {
                AccumalationState accumalationState = accumalationStateRepository.GetSingle(accumalationStateDetails.Code);
                accumalationState.Name = accumalationStateDetails.Name;
                accumalationState.LocalName = accumalationStateDetails.LocalName;
                accumalationState.SearchFields = (accumalationStateDetails.Code + "," + accumalationStateDetails.Name + "," + accumalationStateDetails.LocalName).ToLower();
                accumalationStateRepository.Update(accumalationState);
            }
            else
            {
                AccumalationState newAccumalationState = new AccumalationState() { Code = accumalationStateDetails.Code, LocalName = accumalationStateDetails.LocalName, Name = accumalationStateDetails.Name, SearchFields = (accumalationStateDetails.Code + "," + accumalationStateDetails.Name + "," + accumalationStateDetails.LocalName).ToLower() };
                accumalationStateRepository.Add(newAccumalationState);
            }
        }

        public static void AddStorageStatus(StorageStatusDetails storageStatusDetails, StorageStatusRepository storageStatusRepository)
        {
            Dictionary<string, StorageStatus> tenantStorageStatuses = storageStatusRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantStorageStatuses.Keys.Contains(storageStatusDetails.Code))
            {
                StorageStatus storageStatus = storageStatusRepository.GetSingle(storageStatusDetails.Code);
                storageStatus.Name = storageStatusDetails.Name;
                storageStatus.LocalName = storageStatusDetails.LocalName;
                storageStatus.SearchFields = (storageStatusDetails.Code + "," + storageStatusDetails.Name + "," + storageStatusDetails.LocalName).ToLower();
                storageStatusRepository.Update(storageStatus);
            }
            else
            {
                StorageStatus newStorageStatus = new StorageStatus() { Code = storageStatusDetails.Code, LocalName = storageStatusDetails.LocalName, Name = storageStatusDetails.Name, SearchFields = (storageStatusDetails.Code + "," + storageStatusDetails.Name + "," + storageStatusDetails.LocalName).ToLower() };
                storageStatusRepository.Add(newStorageStatus);
            }
        }

        public static void AddOBLType(CodeNameDetails myDetails, OBLTypeRepository myRepository)
        {
            Dictionary<string, OBLType> myDictionary = myRepository.GetOBLTypes().ToDictionary(d => d.Code, a => a);

            if (myDictionary.Keys.Contains(myDetails.Code))
            {
                OBLType myPOCO = myRepository.GetSingleOBLType(myDetails.Code);
                myPOCO.Name = myDetails.Name;
                myPOCO.SearchFields = myDetails.Code + "," + myDetails.Name;
                myRepository.Update(myPOCO);
            }

            else
            {
                OBLType myPOCO = new OBLType()
                {
                    Code = myDetails.Code,
                    Name = myDetails.Name,
                    SearchFields = myDetails.Code + "," + myDetails.Name,
                };

                myRepository.Add(myPOCO);
            }
        }
        public static void AddRegistryDateType(CodeNameDetails myDetails, RegistryDateTypeRepository myRepository)
        {
            Dictionary<string, RegistryDateType> myDictionary = myRepository.GetRegistryDateTypes().ToDictionary(d => d.Code, a => a);

            if (myDictionary.Keys.Contains(myDetails.Code))
            {
                RegistryDateType myPOCO = myRepository.GetSingleRegistryDateType(myDetails.Code);
                myPOCO.Name = myDetails.Name;
                myPOCO.SearchFields = myDetails.Code + "," + myDetails.Name;
                myRepository.Update(myPOCO);
            }

            else
            {
                RegistryDateType myPOCO = new RegistryDateType()
                {
                    Code = myDetails.Code,
                    Name = myDetails.Name,
                    SearchFields = myDetails.Code + "," + myDetails.Name,
                };

                myRepository.Add(myPOCO);
            }
        }

        public static void AddShipmentCustomsMessageType(CodeNameDetails myDetails, ShipmentCustomsMessageTypeRepository myRepository)
        {
            Dictionary<string, ShipmentCustomsMessageType> myDictionary = myRepository.GetShipmentCustomsMessageTypes().ToDictionary(d => d.Code, a => a);

            if (myDictionary.Keys.Contains(myDetails.Code))
            {
                ShipmentCustomsMessageType myPOCO = myRepository.GetSingleShipmentCustomsMessageType(myDetails.Code);
                myPOCO.Name = myDetails.Name;
                myPOCO.SearchFields = myDetails.Code + "," + myDetails.Name;
                myRepository.Update(myPOCO);
            }

            else
            {
                ShipmentCustomsMessageType myPOCO = new ShipmentCustomsMessageType()
                {
                    Code = myDetails.Code,
                    Name = myDetails.Name,
                    SearchFields = myDetails.Code + "," + myDetails.Name,
                };

                myRepository.Add(myPOCO);
            }
        }


        public static void AddMAWBType(MAWBTypeDetails MAWBTypeDetails, MAWBTypeRepository MAWBTypeRepository)
        {
            Dictionary<string, MAWBType> tenantMAWBTypees = MAWBTypeRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantMAWBTypees.Keys.Contains(MAWBTypeDetails.Code))
            {
                MAWBType MAWBType = MAWBTypeRepository.GetSingle(MAWBTypeDetails.Code);
                MAWBType.Name = MAWBTypeDetails.Name;
              
                MAWBType.SearchFields = (MAWBTypeDetails.Code + "," + MAWBTypeDetails.Name).ToLower();
                MAWBTypeRepository.Update(MAWBType);
            }
            else
            {
                MAWBType newMAWBType = new MAWBType() { Code = MAWBTypeDetails.Code,  Name = MAWBTypeDetails.Name, SearchFields = (MAWBTypeDetails.Code + "," + MAWBTypeDetails.Name ).ToLower() };
                MAWBTypeRepository.Add(newMAWBType);
            }
        }


        public static void AddCourierCustomStatus(CourierCustomStatusDetails courierCustomStatusDetails, CourierCustomStatusRepository courierCustomStatusRepository)
        {
            Dictionary<string, CourierCustomStatus> tenantCourierCustomStatuses = courierCustomStatusRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantCourierCustomStatuses.Keys.Contains(courierCustomStatusDetails.Code))
            {
                CourierCustomStatus courierCustomStatus = courierCustomStatusRepository.GetSingle(courierCustomStatusDetails.Code);
                courierCustomStatus.Name = courierCustomStatusDetails.Name;

                courierCustomStatus.SearchFields = (courierCustomStatusDetails.Code + "," + courierCustomStatusDetails.Name).ToLower();
                courierCustomStatusRepository.Update(courierCustomStatus);
            }
            else
            {
                CourierCustomStatus newCourierCustomStatus  = new CourierCustomStatus() { Code = courierCustomStatusDetails.Code, Name = courierCustomStatusDetails.Name, SearchFields = (courierCustomStatusDetails.Code + "," + courierCustomStatusDetails.Name).ToLower() };
                courierCustomStatusRepository.Add(newCourierCustomStatus);
            }
        }

        public static void AddManifestCargoStatus(ManifestCargoStatusDetails manifestCargoStatusDetails, ManifestCargoStatusRepository manifestCargoStatusRepository)
        {
            Dictionary<string, ManifestCargoStatus> tenantManifestCargoStatuses = manifestCargoStatusRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantManifestCargoStatuses.Keys.Contains(manifestCargoStatusDetails.Code))
            {
                ManifestCargoStatus manifestCargoStatus = manifestCargoStatusRepository.GetSingle(manifestCargoStatusDetails.Code);
                manifestCargoStatus.Name = manifestCargoStatusDetails.Name;

                manifestCargoStatus.SearchFields = (manifestCargoStatusDetails.Code + "," + manifestCargoStatusDetails.Name).ToLower();
                manifestCargoStatusRepository.Update(manifestCargoStatus);
            }
            else
            {
                ManifestCargoStatus newManifestCargoStatus = new ManifestCargoStatus() { Code =manifestCargoStatusDetails.Code, Name = manifestCargoStatusDetails.Name, SearchFields = (manifestCargoStatusDetails.Code + "," + manifestCargoStatusDetails.Name).ToLower() };
                manifestCargoStatusRepository.Add(newManifestCargoStatus);
            }
        }

        public static void AddCourierManifestStatus(CourierManifestStatusDetails courierManifestStatusDetails, CourierManifestStatusRepository courierManifestStatusRepository)
        {
            Dictionary<string, CourierManifestStatus> tenantCourierManifestStatuses = courierManifestStatusRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantCourierManifestStatuses.Keys.Contains(courierManifestStatusDetails.Code))
            {
                CourierManifestStatus courierManifestStatus = courierManifestStatusRepository.GetSingle(courierManifestStatusDetails.Code);
                courierManifestStatus.Name = courierManifestStatusDetails.Name;

                courierManifestStatus.SearchFields = (courierManifestStatusDetails.Code + "," + courierManifestStatusDetails.Name).ToLower();
                courierManifestStatusRepository.Update(courierManifestStatus);
            }
            else
            {
                CourierManifestStatus newCourierManifestStatus = new CourierManifestStatus() { Code = courierManifestStatusDetails.Code, Name = courierManifestStatusDetails.Name, SearchFields = (courierManifestStatusDetails.Code + "," + courierManifestStatusDetails.Name).ToLower() };
                courierManifestStatusRepository.Add(newCourierManifestStatus);
            }
        }

        public static void AddCourierDeclarationStatus(CourierDeclarationStatusDetails CourierDeclarationStatusDetails, CourierDeclarationStatusRepository CourierDeclarationStatusRepository)
        {
            Dictionary<string, CourierDeclarationStatus> tenantCourierDeclarationStatuses = CourierDeclarationStatusRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantCourierDeclarationStatuses.Keys.Contains(CourierDeclarationStatusDetails.Code))
            {
                CourierDeclarationStatus CourierDeclarationStatus = CourierDeclarationStatusRepository.GetSingle(CourierDeclarationStatusDetails.Code);
                CourierDeclarationStatus.Name = CourierDeclarationStatusDetails.Name;

                CourierDeclarationStatus.SearchFields = (CourierDeclarationStatusDetails.Code + "," + CourierDeclarationStatusDetails.Name).ToLower();
                CourierDeclarationStatusRepository.Update(CourierDeclarationStatus);
            }
            else
            {
                CourierDeclarationStatus newCourierDeclarationStatus = new CourierDeclarationStatus() { Code = CourierDeclarationStatusDetails.Code, Name = CourierDeclarationStatusDetails.Name, SearchFields = (CourierDeclarationStatusDetails.Code + "," + CourierDeclarationStatusDetails.Name).ToLower() };
                CourierDeclarationStatusRepository.Add(newCourierDeclarationStatus);
            }
        }

        public static void AddCourierPaymentStatus(CourierPaymentStatusDetails CourierPaymentStatusDetails, CourierPaymentStatusRepository CourierPaymentStatusRepository)
        {
            Dictionary<string, CourierPaymentStatus> tenantCourierPaymentStatuses = CourierPaymentStatusRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantCourierPaymentStatuses.Keys.Contains(CourierPaymentStatusDetails.Code))
            {
                CourierPaymentStatus CourierPaymentStatus = CourierPaymentStatusRepository.GetSingle(CourierPaymentStatusDetails.Code);
                CourierPaymentStatus.Name = CourierPaymentStatusDetails.Name;

                CourierPaymentStatus.SearchFields = (CourierPaymentStatusDetails.Code + "," + CourierPaymentStatusDetails.Name).ToLower();
                CourierPaymentStatusRepository.Update(CourierPaymentStatus);
            }
            else
            {
                CourierPaymentStatus newCourierPaymentStatus = new CourierPaymentStatus() { Code = CourierPaymentStatusDetails.Code, Name = CourierPaymentStatusDetails.Name, SearchFields = (CourierPaymentStatusDetails.Code + "," + CourierPaymentStatusDetails.Name).ToLower() };
                CourierPaymentStatusRepository.Add(newCourierPaymentStatus);
            }
        }

        public static void AddINTTRASettingMode(CodeNameDetails myDetails, INTTRASettingModeRepository myRepository)
        {
            Dictionary<string, INTTRASettingMode> myDictionary = myRepository.GetINTTRASettingModes().ToDictionary(d => d.Code, a => a);

            if (myDictionary.Keys.Contains(myDetails.Code))
            {
                INTTRASettingMode myPOCO = myRepository.GetSingleINTTRASettingMode(myDetails.Code);
                myPOCO.Name = myDetails.Name;
                myPOCO.SearchFields = myDetails.Code + "," + myDetails.Name;
                myRepository.Update(myPOCO);
            }

            else
            {
                INTTRASettingMode myPOCO = new INTTRASettingMode()
                {
                    Code = myDetails.Code,
                    Name = myDetails.Name,
                    SearchFields = myDetails.Code + "," + myDetails.Name,
                };

                myRepository.Add(myPOCO);
            }
        }


        public static void AddINTTRASIStatus(CodeNameDetails myDetails, INTTRASIStatusRepository myRepository)
        {
            Dictionary<string, INTTRASIStatus> myDictionary = myRepository.GetINTTRASIStatus().ToDictionary(d => d.Code, a => a);

            if (myDictionary.Keys.Contains(myDetails.Code))
            {
                INTTRASIStatus myPOCO = myRepository.GetSingleINTTRASIStatus(myDetails.Code);
                myPOCO.Name = myDetails.Name;
                myPOCO.SearchFields = myDetails.Code + "," + myDetails.Name;
                myRepository.Update(myPOCO);
            }

            else
            {
                INTTRASIStatus myPOCO = new INTTRASIStatus()
                {
                    Code = myDetails.Code,
                    Name = myDetails.Name,
                    SearchFields = myDetails.Code + "," + myDetails.Name,
                };

                myRepository.Add(myPOCO);
            }
        }

        public static void AddTaxWithholdingAssessingOfficeMode(TaxWithholdingAssessOfficeDetails details, TaxWithholdingAssessOfficeRepository repository)
        {
           
            Dictionary<string, TaxWithholdingAssessOffice> dictionary = repository.GetAll(0).ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(details.Code))
            {
                TaxWithholdingAssessOffice POCO = repository.GetSingleTaxWithholdingAssessOffice(details.Code,0);
                POCO.Name = details.Name;
                POCO.LocalName = details.LocalName;
                POCO.SearchFields = details.Code + "," + details.Name +"," + details.LocalName;
                repository.Update(POCO);
            }

            else
            {
                TaxWithholdingAssessOffice POCO = new TaxWithholdingAssessOffice()
                {
                    Code = details.Code,
                    Name = details.Name,
                    LocalName = details.LocalName,
                    Id = details.Id,
                    Tenant = details.Tenant,

                    SearchFields = details.Code + "," + details.Name+ "," + details.LocalName,
                };

                repository.Add(POCO);
            }
        }


        public static void AddAccountingCompanyType(AccountingCompanyTypeDetails details, AccountingCompanyTypeRepository repository)
        {

            Dictionary<string, AccountingCompanyType> dictionary = repository.GetAll(0).ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(details.Code))
            {
                AccountingCompanyType POCO = repository.GetSingleAccountingCompanyType(details.Code,0);
                POCO.EnglishName = details.EnglishName;
                POCO.LocalName = details.LocalName;
                POCO.SearchFields = details.Code + "," + details.EnglishName + "," + details.LocalName;
                repository.Update(POCO);
            }

            else
            {
                AccountingCompanyType POCO = new AccountingCompanyType()
                {
                    Code = details.Code,
                    EnglishName = details.EnglishName,
                    LocalName = details.LocalName,
                    Id = details.Id,
                    Tenant = details.Tenant,

                    SearchFields = details.Code + "," + details.EnglishName + "," + details.LocalName,
                };

                repository.Add(POCO);
            }
        }


        public static void AddWithholdingTaxDeductionType(WithholdingTaxDeductionTypeDetails details, WithholdingTaxDeductionTypeRepository repository)
        {

            Dictionary<string, WithholdingTaxDeductionType> dictionary = repository.GetAll(0).ToDictionary(d => d.Code, a => a);

            if (dictionary.Keys.Contains(details.Code))
            {
                WithholdingTaxDeductionType POCO = repository.GetSingleWithholdingTaxDeductionType(details.Code, 0);
                POCO.EnglishName = details.EnglishName;
                POCO.LocalName = details.LocalName;
                POCO.SearchFields = details.Code + "," + details.EnglishName + "," + details.LocalName;
                repository.Update(POCO);
            }

            else
            {
                WithholdingTaxDeductionType POCO = new WithholdingTaxDeductionType()
                {
                    Code = details.Code,
                    EnglishName = details.EnglishName,
                    LocalName = details.LocalName,
                    Id = details.Id,
                    Tenant = details.Tenant,

                    SearchFields = details.Code + "," + details.EnglishName + "," + details.LocalName,
                };

                repository.Add(POCO);
            }
        }

        public static void AddTemperatureUnit(CodeNameDetails myDetails, TemperatureUnitRepository myRepository)
        {
            Dictionary<string, TemperatureUnit> myDictionary = myRepository.GetTemperatureUnits().ToDictionary(d => d.Code, a => a);

            if (myDictionary.Keys.Contains(myDetails.Code))
            {
                TemperatureUnit myPOCO = myRepository.GetSingleTemperatureUnit(myDetails.Code);
                myPOCO.Name = myDetails.Name;
                myPOCO.SearchFields = myDetails.Code + "," + myDetails.Name;
                myRepository.Update(myPOCO);
            }

            else
            {
                TemperatureUnit myPOCO = new TemperatureUnit()
                {
                    Code = myDetails.Code,
                    Name = myDetails.Name,
                    SearchFields = myDetails.Code + "," + myDetails.Name,
                };

                myRepository.Add(myPOCO);
            }
        }

        public static void AddPickUpDeliveryTransportMode(CodeNameDetails myDetails, PickUpDeliveryTransportModeRepository myRepository)
        {
            Dictionary<string, PickUpDeliveryTransportMode> myDictionary = myRepository.GetPickUpDeliveryTransportModes().ToDictionary(d => d.Code, a => a);

            if (myDictionary.Keys.Contains(myDetails.Code))
            {
                PickUpDeliveryTransportMode myPOCO = myRepository.GetSinglePickUpDeliveryTransportMode(myDetails.Code);
                myPOCO.Name = myDetails.Name;
                myPOCO.SearchFields = myDetails.Code + "," + myDetails.Name;
                myRepository.Update(myPOCO);
            }

            else
            {
                PickUpDeliveryTransportMode myPOCO = new PickUpDeliveryTransportMode()
                {
                    Code = myDetails.Code,
                    Name = myDetails.Name,
                    SearchFields = myDetails.Code + "," + myDetails.Name,
                };

                myRepository.Add(myPOCO);
            }
        }

        public static void AddINTTRAStatus(CodeNameDetails myDetails, INTTRAStatusRepository myRepository)
        {
            Dictionary<string, INTTRAStatus> myDictionary = myRepository.GetINTTRAStatuses().ToDictionary(d => d.Code, a => a);

            if (myDictionary.Keys.Contains(myDetails.Code))
            {
                INTTRAStatus myPOCO = myRepository.GetSingleINTTRAStatus(myDetails.Code);
                myPOCO.Name = myDetails.Name;
                myPOCO.SearchFields = myDetails.Code + "," + myDetails.Name;
                myRepository.Update(myPOCO);
            }

            else
            {
                INTTRAStatus myPOCO = new INTTRAStatus()
                {
                    Code = myDetails.Code,
                    Name = myDetails.Name,
                    SearchFields = myDetails.Code + "," + myDetails.Name,
                };

                myRepository.Add(myPOCO);
            }
        }

        public static void AddINTTRADocumentType(CodeNameDetails myDetails, INTTRADocumentTypeRepository myRepository)
        {
            Dictionary<string, INTTRADocumentType> myDictionary = myRepository.GetINTTRADocumentTypes().ToDictionary(d => d.Code, a => a);

            if (myDictionary.Keys.Contains(myDetails.Code))
            {
                INTTRADocumentType myPOCO = myRepository.GetSingleINTTRADocumentType(myDetails.Code);
                myPOCO.Name = myDetails.Name;
                myPOCO.SearchFields = myDetails.Code + "," + myDetails.Name;
                myRepository.Update(myPOCO);
            }

            else
            {
                INTTRADocumentType myPOCO = new INTTRADocumentType()
                {
                    Code = myDetails.Code,
                    Name = myDetails.Name,
                    SearchFields = myDetails.Code + "," + myDetails.Name,
                };

                myRepository.Add(myPOCO);
            }
        }

        public static void AddAcceptanceStatus(AcceptanceStatusDetails acceptanceStatusDetails, AcceptanceStatusRepository acceptanceStatusRepository)
        {
            Dictionary<string, AcceptanceStatus> tenantAcceptanceStatuses = acceptanceStatusRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantAcceptanceStatuses.Keys.Contains(acceptanceStatusDetails.Code))
            {
                AcceptanceStatus acceptanceStatus = acceptanceStatusRepository.GetSingle(acceptanceStatusDetails.Code);
                acceptanceStatus.EnglishName = acceptanceStatusDetails.EnglishName;

                acceptanceStatus.SearchFields = (acceptanceStatusDetails.Code + "," + acceptanceStatusDetails.EnglishName).ToLower();
                acceptanceStatusRepository.Update(acceptanceStatus);
            }
            else
            {
                AcceptanceStatus newAcceptanceStatus = new AcceptanceStatus() { Code = acceptanceStatusDetails.Code, EnglishName = acceptanceStatusDetails.EnglishName, SearchFields = (acceptanceStatusDetails.Code + "," + acceptanceStatusDetails.EnglishName).ToLower() };
                acceptanceStatusRepository.Add(newAcceptanceStatus);
            }
        }

        public static void AddMamanStatus(MamanStatusDetails mamanStatusDetails, MamanStatusRepository mamanStatusRepository)
        {
            Dictionary<string, MamanStatus> tenantMamanStatuses = mamanStatusRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantMamanStatuses.Keys.Contains(mamanStatusDetails.Code))
            {
                MamanStatus mamanStatus = mamanStatusRepository.GetSingle(mamanStatusDetails.Code);
                mamanStatus.Name = mamanStatusDetails.Name;

                mamanStatus.SearchFields = (mamanStatusDetails.Code + "," + mamanStatusDetails.Name).ToLower();
                mamanStatusRepository.Update(mamanStatus);
            }
            else
            {
                MamanStatus newMamanStatus = new MamanStatus() { Code = mamanStatusDetails.Code, Name = mamanStatusDetails.Name, SearchFields = (mamanStatusDetails.Code + "," + mamanStatusDetails.Name).ToLower() };
                mamanStatusRepository.Add(newMamanStatus);
            }
        }

        public static void AddPendingErrorPlace(PendingErrorPlace pendingErrorPlaceDetails, PendingErrorPlaceRepository pendingErrorPlaceRepository)
        {
            Dictionary<string, PendingErrorPlace> tenantPendingErrorPlace = pendingErrorPlaceRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenantPendingErrorPlace.Keys.Contains(pendingErrorPlaceDetails.Code))
            {
                PendingErrorPlace pendingErrorPlace = pendingErrorPlaceRepository.GetSingle(pendingErrorPlaceDetails.Code);
                pendingErrorPlace.LocalName = pendingErrorPlaceDetails.LocalName;
                pendingErrorPlace.EnglishName = pendingErrorPlaceDetails.EnglishName;
                pendingErrorPlace.SearchFields = (pendingErrorPlaceDetails.Code + "," + pendingErrorPlaceDetails.LocalName).ToLower();
                pendingErrorPlaceRepository.Update(pendingErrorPlace);
            }
            else
            {
                PendingErrorPlace newPendingErrorPlace = new PendingErrorPlace() {
                    Code = pendingErrorPlaceDetails.Code,
                    LocalName = pendingErrorPlaceDetails.LocalName,
                    EnglishName = pendingErrorPlaceDetails.EnglishName,
                    SearchFields = (pendingErrorPlaceDetails.Code + "," + pendingErrorPlaceDetails.LocalName).ToLower()
                };
                pendingErrorPlaceRepository.Add(newPendingErrorPlace);
            }
        }

        public static void AddCourierPendingReason(CourierPendingReason courierPendingReasonDetails, CourierPendingReasonRepository courierPendingReasonRepository)
        {
            //Dictionary<string, CourierPendingReason> tenantCourierPendingReason = courierPendingReasonRepository.GetAll().ToDictionary(d => d.Code, a => a);

            //if (tenantCourierPendingReason.Keys.Contains(courierPendingReasonDetails.Code))
            //{
            //    CourierPendingReason courierPendingReason = courierPendingReasonRepository.GetSingle(courierPendingReasonDetails.Code, tenantCourierPendingReason.Keys.);
            //    courierPendingReason.LocalName = courierPendingReasonDetails.LocalName;
            //    courierPendingReason.EnglishName = courierPendingReasonDetails.EnglishName;
            //    courierPendingReason.SearchFields = (courierPendingReasonDetails.Code + "," + courierPendingReasonDetails.LocalName).ToLower();
            //    courierPendingReasonRepository.Update(courierPendingReason);
            //}
            //else
            //{
            //    CourierPendingReason newCourierPendingReason = new CourierPendingReason()
            //    {
            //        Code = courierPendingReasonDetails.Code,
            //        LocalName = courierPendingReasonDetails.LocalName,
            //        EnglishName = courierPendingReasonDetails.EnglishName,
            //        SearchFields = (courierPendingReasonDetails.Code + "," + courierPendingReasonDetails.LocalName).ToLower()
            //    };
            //    courierPendingReasonRepository.Add(newCourierPendingReason);
            //}
        }

        public static void AddMamanSpecialAction(MamanSpecialAction mamanSpecialActionDetails, MamanSpecialActionRepository mamanSpecialActionRepository)
        {
            Dictionary<string, MamanSpecialAction> tenant = mamanSpecialActionRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenant.Keys.Contains(mamanSpecialActionDetails.Code))
            {
                MamanSpecialAction mamanSpecialAction = mamanSpecialActionRepository.GetSingle(mamanSpecialActionDetails.Code);
                mamanSpecialAction.LocalName = mamanSpecialActionDetails.LocalName;
                mamanSpecialAction.EnglishName = mamanSpecialActionDetails.EnglishName;
                mamanSpecialAction.SearchFields = (mamanSpecialActionDetails.Code + "," + mamanSpecialActionDetails.LocalName).ToLower();
                mamanSpecialActionRepository.Update(mamanSpecialAction);
            }
            else
            {
                MamanSpecialAction newMamanSpecialAction = new MamanSpecialAction()
                {
                    Code = mamanSpecialActionDetails.Code,
                    LocalName = mamanSpecialActionDetails.LocalName,
                    EnglishName = mamanSpecialActionDetails.EnglishName,
                    SearchFields = (mamanSpecialActionDetails.Code + "," + mamanSpecialActionDetails.LocalName).ToLower()
                };
                mamanSpecialActionRepository.Add(newMamanSpecialAction);
            }
        }

        public static void AddMamanSpecialActionStatus(MamanSpecialActionStatus mamanSpecialActionStatusDetails, MamanSpecialActionStatusRepository mamanSpecialActionStatusRepository)
        {
            Dictionary<string, MamanSpecialActionStatus> tenant = mamanSpecialActionStatusRepository.GetAll().ToDictionary(d => d.Code, a => a);

            if (tenant.Keys.Contains(mamanSpecialActionStatusDetails.Code))
            {
                MamanSpecialActionStatus mamanSpecialActionStatus = mamanSpecialActionStatusRepository.GetSingle(mamanSpecialActionStatusDetails.Code);
                mamanSpecialActionStatus.LocalName = mamanSpecialActionStatusDetails.LocalName;
                mamanSpecialActionStatus.EnglishName = mamanSpecialActionStatusDetails.EnglishName;
                mamanSpecialActionStatus.SearchFields = (mamanSpecialActionStatusDetails.Code + "," + mamanSpecialActionStatusDetails.LocalName).ToLower();
                mamanSpecialActionStatusRepository.Update(mamanSpecialActionStatus);
            }
            else
            {
                MamanSpecialActionStatus newMamanSpecialActionStatus = new MamanSpecialActionStatus()
                {
                    Code = mamanSpecialActionStatusDetails.Code,
                    LocalName = mamanSpecialActionStatusDetails.LocalName,
                    EnglishName = mamanSpecialActionStatusDetails.EnglishName,
                    SearchFields = (mamanSpecialActionStatusDetails.Code + "," + mamanSpecialActionStatusDetails.LocalName).ToLower()
                };
                mamanSpecialActionStatusRepository.Add(newMamanSpecialActionStatus);
            }
        }
    }
}
