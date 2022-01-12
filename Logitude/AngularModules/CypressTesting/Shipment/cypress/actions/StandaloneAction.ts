import { ShipmentSelectors } from '../../../Shipment/cypress/selectors/Selectors';
import { BaseSelectors } from '../../../Base/cypress/selectors/BaseSelectors';
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { RequestAliases } from '../../../Base/cypress/constants/RequestAliases';
import { PickupDelivaryDetails } from "../models/PickupDelivaryDetails";
import { RegexSelectors } from "../selectors/RegexSelectors";
import { ShipmentConstants } from '../constants/constants'

let fromType = null
let toType = null

export function FillPickUpDelivaryDetails(pickupdelivaryDetails: PickupDelivaryDetails) {
   FillPickupDeliveryRouting(pickupdelivaryDetails)
   FillPickupDelivaryFromTypeDetails(pickupdelivaryDetails)
   FillPickupDelivaryToTypeDetails(pickupdelivaryDetails)
}

export function FillPickupDeliveryRouting(pickupdelivaryDetails: PickupDelivaryDetails) {
   fromType = RegexSelectors.PickupDeliveryFromType(pickupdelivaryDetails.From)
   cy.ClickRadio(fromType)
   toType = RegexSelectors.PickupDeliveryToType(pickupdelivaryDetails.To)
   cy.ClickRadio(toType)
}

export function FillPickupDelivaryFromTypeDetails(pickupdelivaryDetails: PickupDelivaryDetails) {
   if (pickupdelivaryDetails.From == ShipmentConstants.Partner) {
      cy.FillLogLov(ShipmentSelectors.DeliveryFromPartnerName, pickupdelivaryDetails.FromPartner, true)
   }
   else if (pickupdelivaryDetails.From == ShipmentConstants.Port) {
      cy.FillLogLov(ShipmentSelectors.PickUpDeliveryFromPort, pickupdelivaryDetails.FromPort, true)
   }
   else {
      cy.FillLogLov(ShipmentSelectors.DeliveryFromCountryName, pickupdelivaryDetails.FromCountry, true)
      cy.FillLogTextBox(ShipmentSelectors.DeliveryFromCityName, pickupdelivaryDetails.FromCity)
   }
}

export function FillPickupDelivaryToTypeDetails(pickupdelivaryDetails: PickupDelivaryDetails) {
   if (pickupdelivaryDetails.To == ShipmentConstants.Partner) {
      cy.FillLogLov(ShipmentSelectors.DeliveryToPartnerName, pickupdelivaryDetails.ToPartner, true)
   }
   else if (pickupdelivaryDetails.To == ShipmentConstants.Port) {
      cy.FillLogLov(ShipmentSelectors.PickUpDeliveryToPort, pickupdelivaryDetails.ToPort, true)
   }
   else {
      cy.FillLogTextBox(ShipmentSelectors.DeliveryToCityName, pickupdelivaryDetails.ToCity)
      cy.FillLogLov(ShipmentSelectors.DeliveryToCountryName, pickupdelivaryDetails.ToCountry, true)
   }
}

export function CreateStandaloneShipment() {
   cy.DefineRequestWait(RestAPI.POST, URLs.Shipment, RequestAliases.ShipmentStandaloneRequest)
   cy.Click(ShipmentSelectors.CreateShipmentButton + BaseSelectors.LastElement, null)
}

export function AssertShipmenteMenuButtonsEnabled() {
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBConvertToCustomFile + BaseSelectors.LastElement, BaseSelectors.NotBeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBOperationalClose + BaseSelectors.LastElement, BaseSelectors.NotBeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBSendResponse + BaseSelectors.LastElement, BaseSelectors.NotBeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBCancelShipment + BaseSelectors.LastElement, BaseSelectors.NotBeDisabled)
}

export function AssertShipmenteMenuButtonsDisabled() {
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBExceptionResolved + BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBAccountingClose + BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBOperationalReopen + BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBAccountedReopen + BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBConvertShipmentFromHouseToDirect + BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBConvertShipmentFromDirectToHouse + BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBConverttoLTL + BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBConvertShipmentDirection + BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBCopyShipment + BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBReactivateShipment + BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBSplitShipment + BaseSelectors.LastElement, BaseSelectors.BeDisabled)
}

export function AssertShipmentePickupDelivaryWindowDisabled() {
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryCarrier, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryCarrierNumber, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryDriver, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryTruckNumber, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryTrailerNumber, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryByRail, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryByTruck, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryATDDate, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryATDTime, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryETDDate, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryETDTime, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryETATime, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryETADate, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryATADate, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryATATime, BaseSelectors.BeDisabled)
}

export function AssertShipmenteDelivaryWindowDisabled() {
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryEmptyDeliveryContainer, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryEmptyDeliveryDepotReference, BaseSelectors.BeDisabled)
}