import { ShipmentSelectors } from '../../../Shipment/cypress/selectors/Selectors';
import { BaseSelectors } from '../../../Base/cypress/selectors/BaseSelectors';
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { RequestAliases } from '../../../Base/cypress/constants/RequestAliases';
import { PickupDelivaryDetails } from "../models/PickupDelivaryDetails";
import { RegexSelectors } from "../selectors/RegexSelectors";
import { ShipmentConstants } from '../constants/constants'
import { PackagesDetails } from "cypress/models/PackagesDetails";

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

export function AssertShipmentPickupDelivaryWindowFields(condition) {
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryCarrier, condition)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryCarrierNumber, condition)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryDriver, condition)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryTruckNumber, condition)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryTrailerNumber, condition)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryByRail, condition)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryByTruck, condition)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryATDDate, condition)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryATDTime, condition)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryETDDate, condition)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryETDTime, condition)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryETATime, condition)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryETADate, condition)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryATADate, condition)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryATATime, condition)
}

export function AssertShipmenteDelivaryWindowDisabled() {
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryEmptyDeliveryContainer, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryEmptyDeliveryDepotReference, BaseSelectors.BeDisabled)
}

export function AddPcakagesInPickupDelivary() {
   cy.Click(ShipmentSelectors.PickUpDeliveryPackages, null)
   cy.Click(ShipmentSelectors.AddContainerFromPickup, null, true)
   cy.get(ShipmentSelectors.LogitudeCheckBox).eq(1).click();
   cy.get(ShipmentSelectors.LogitudeCheckBox).eq(2).click();
   cy.get(ShipmentSelectors.LogitudeCheckBox).eq(3).click();
   cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null)
}

export function AddPcakagesInStandaloneShipment(packagesDetails: PackagesDetails) {
   cy.FillLogLov(ShipmentSelectors.PackageType, packagesDetails.PackageType, true)
   cy.FillLogTextBox(ShipmentSelectors.ContainerNumber, packagesDetails.ContainerNumber)
   cy.FillLogTextBox(ShipmentSelectors.PackageWeight, packagesDetails.GrossWeight.toString())
   cy.Click(ShipmentSelectors.OceanPackageOKButton,null)
   cy.Click(BaseSelectors.RedButton, ShipmentConstants.Ok)
   
}
export function StandaloneShipmentOpenPackageTab(){
   cy.Click(ShipmentSelectors.PackagesTab_Number + BaseSelectors.LastElement, null)
   cy.Click(ShipmentSelectors.AddPackage, null)
   cy.Click(ShipmentSelectors.AddPackagesInStandalone, null)
}
export function OpenRoutingTabAddPickUp(){
  cy.Click(ShipmentSelectors.RoutingsTab, null)
  cy.Click(ShipmentSelectors.AddPickUp, null)
  cy.Click(BaseSelectors.Button, ShipmentConstants.AddPickUp)
}
export function FillStandaloneShipmentFromTypeDetails(standaloneDetails: PickupDelivaryDetails) {
   if (standaloneDetails.From == ShipmentConstants.Partner) {
      cy.FillLogLov(ShipmentSelectors.ShipmentMainCarriageFromPartner, standaloneDetails.FromPartner, true)
   }
   else if (standaloneDetails.From == ShipmentConstants.Port) {
      cy.FillLogLov(ShipmentSelectors.ShipmentMainCarriageFromPort, standaloneDetails.FromPort, true)
   }
   else {
      cy.FillLogLov(ShipmentSelectors.ShipmentInlandDomesticFromCountry, standaloneDetails.FromCountry, true)
      cy.FillLogTextBox(ShipmentSelectors.ShipmentInlandDomesticFromCity, standaloneDetails.FromCity)
   }
}

export function FillStandaloneShipmentToTypeDetails(standaloneDetails: PickupDelivaryDetails) {
   if (standaloneDetails.To == ShipmentConstants.Partner) {
      cy.FillLogLov(ShipmentSelectors.ShipmentMainCarriageToPartner, standaloneDetails.ToPartner, true)
   }
   else if (standaloneDetails.To == ShipmentConstants.Port) {
      cy.FillLogLov(ShipmentSelectors.ShipmentMainCarriageToPort, standaloneDetails.ToPort, true)
   }
   else {
      cy.FillLogLov(ShipmentSelectors.ShipmentInlandDomesticToCountry, standaloneDetails.ToCountry,true)
      cy.FillLogTextBox(ShipmentSelectors.ShipmentInlandDomesticToCity, standaloneDetails.ToCity)
   }
}

export function CreateStandaloneShipmentFromRoutingDetails(pickupdelivaryDetails: PickupDelivaryDetails) {
   FillPickupDeliveryRouting(pickupdelivaryDetails)
   FillStandaloneShipmentFromTypeDetails(pickupdelivaryDetails)
   FillStandaloneShipmentToTypeDetails(pickupdelivaryDetails)
}