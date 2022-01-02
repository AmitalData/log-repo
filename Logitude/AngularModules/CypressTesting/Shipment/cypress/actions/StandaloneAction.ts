import { ShipmentSelectors } from '../../../Shipment/cypress/selectors/Selectors';
import { BaseSelectors } from '../../../Base/cypress/selectors/BaseSelectors';
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { RequestAliases } from '../../../Base/cypress/constants/RequestAliases';
import { PickupDelivaryDeteails } from "../models/PickupDelivaryDeteails";
import { RegexSelectors } from "../selectors/RegexSelectors";
import { ShipmentConstants } from '../constants/constants'


export function FillALLPickUpDelivaryDetails(pickupdelivaryDeteails: PickupDelivaryDeteails) {
   FillPickupDeliveryRouting(pickupdelivaryDeteails)
   FillFromPickupDelivary(pickupdelivaryDeteails)
   FillToPickupDelivary(pickupdelivaryDeteails)
}

export function FillPickupDeliveryRouting(pickupdelivaryDeteails: PickupDelivaryDeteails) {
   let fromType = RegexSelectors.PickupDeliveryFromType(pickupdelivaryDeteails.From)
   cy.ClickRadio(fromType)
   let toType = RegexSelectors.PickupDeliveryToType(pickupdelivaryDeteails.To)
   cy.ClickRadio(toType)
}

export function FillFromPickupDelivary(pickupdelivaryDeteails: PickupDelivaryDeteails) {
   if (pickupdelivaryDeteails.From == ShipmentConstants.Partner) {
      cy.FillLogLov(ShipmentSelectors.DeliveryFromPartnerName, pickupdelivaryDeteails.FromPartner, true)

   }
   else if (pickupdelivaryDeteails.From == ShipmentConstants.Port) {
      cy.FillLogLov(ShipmentSelectors.PickUpDeliveryFromPort, pickupdelivaryDeteails.FromPort, true)
   }
   else {
      cy.FillLogLov(ShipmentSelectors.DeliveryFromCountryName, pickupdelivaryDeteails.FromCountry, true)
      cy.FillLogTextBox(ShipmentSelectors.DeliveryFromCityName, pickupdelivaryDeteails.FromCity)
   }
}

export function FillToPickupDelivary(pickupdelivaryDeteails: PickupDelivaryDeteails) {
   if (pickupdelivaryDeteails.To == ShipmentConstants.Partner) {
      cy.FillLogLov(ShipmentSelectors.DeliveryToPartnerName, pickupdelivaryDeteails.ToPartner, true)

   }
   else if (pickupdelivaryDeteails.To == ShipmentConstants.Port) {
      cy.FillLogLov(ShipmentSelectors.PickUpDeliveryToPort, pickupdelivaryDeteails.ToPort, true)
   }
   else {
      cy.FillLogTextBox(ShipmentSelectors.DeliveryToCityName, pickupdelivaryDeteails.ToCity)
      cy.FillLogLov(ShipmentSelectors.DeliveryToCountryName, pickupdelivaryDeteails.ToCountry, true)
   }
}

export function CreateStandaloneShipment() {
   cy.DefineRequestWait(RestAPI.POST, URLs.Shipment, RequestAliases.ShipmentStandaloneRequest)
   cy.Click(ShipmentSelectors.RedButton + BaseSelectors.LastElement, null)
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
export function AssertShipmenteDelivaryWindowDisabled(){
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryEmptyDeliveryContainer, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryEmptyDeliveryDepotReference, BaseSelectors.BeDisabled)
}



