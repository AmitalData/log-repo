import { ShipmentSelectors } from '../../../Shipment/cypress/selectors/Selectors';
import { BaseSelectors } from '../../../Base/cypress/selectors/BaseSelectors';
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { RequestAliases } from '../../../Base/cypress/constants/RequestAliases';
import { DelivaryDeteails } from "../models/DelivaryDeteails";
import {RegexSelectors} from "../selectors/RegexSelectors";
import { ShipmentConstants } from '../constants/constants'


export function FillALL(delivaryDeteails:DelivaryDeteails) {
    FillDeliveryRouting(delivaryDeteails)
    FillFromPickupDelivary(delivaryDeteails)
    FillToPickupDelivary(delivaryDeteails)
 }

export function FillDeliveryRouting(delivaryDeteails:DelivaryDeteails) {
   let fromType = RegexSelectors.PickupDeliveryFromType(delivaryDeteails.From)
    cy.ClickRadio(fromType)
    let toType = RegexSelectors.PickupDeliveryToType(delivaryDeteails.To)
    cy.ClickRadio(toType)
}

export function FillFromPickupDelivary(delivaryDeteails:DelivaryDeteails) {
     if(delivaryDeteails.From== ShipmentConstants.Partner) {
        cy.FillLogLov(ShipmentSelectors.DeliveryFromPartnerName,delivaryDeteails.FromPartner , true)
        
     }
     else if (delivaryDeteails.From== ShipmentConstants.Port) {
        cy.FillLogLov(ShipmentSelectors.PickUpDeliveryFromPort,delivaryDeteails.FromPort , true)
    }
    else{
        cy.FillLogLov(ShipmentSelectors.DeliveryFromCountryName,delivaryDeteails.FromCountry , true)
        cy.FillLogTextBox(ShipmentSelectors.DeliveryFromCityName,delivaryDeteails.FromCity)
    }

     
 }

 export function FillToPickupDelivary(delivaryDeteails:DelivaryDeteails) {
   if(delivaryDeteails.To== ShipmentConstants.Partner) {
      cy.FillLogLov(ShipmentSelectors.DeliveryToPartnerName,delivaryDeteails.ToPartner , true)
      
   }
   else if (delivaryDeteails.To== ShipmentConstants.Port) {
      cy.FillLogLov(ShipmentSelectors.PickUpDeliveryToPort,delivaryDeteails.ToPort , true)
  }
  else{
      cy.FillLogLov(ShipmentSelectors.DeliveryToCountryName,delivaryDeteails.ToCountry , true)
      cy.FillLogTextBox(ShipmentSelectors.DeliveryToCityName,delivaryDeteails.ToCity )
  }
   
}
export function CreateStandaloneShipment(){
  
   cy.DefineRequestWait(RestAPI.POST, URLs.Shipment, RequestAliases.ShipmentRequest)
   cy.get("#printbutton").contains(" Create Standalone Shipment ").click()
  // cy.DefineRequestWait(RestAPI.POST, URLs.Shipment, RequestAliases.ShipmentRequest)
   //cy.DefineRequestWait(RestAPI.POST, URLs.Shipment, RequestAliases.ShipmentRequest)
   cy.Navigate(ShipmentSelectors.CreateButtonStandalone,true)
   
}

export function AssertShipmenteMenuButtonsEnabled() {
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBConvertToCustomFile+BaseSelectors.LastElement, BaseSelectors.NotBeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBOperationalClose+BaseSelectors.LastElement, BaseSelectors.NotBeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBSendResponse+BaseSelectors.LastElement, BaseSelectors.NotBeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBCancelShipment+BaseSelectors.LastElement, BaseSelectors.NotBeDisabled)

}

export function AssertShipmenteMenuButtonsDisabled() {
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBExceptionResolved+BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBAccountingClose+BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBOperationalReopen+BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBAccountedReopen+BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBConvertShipmentFromHouseToDirect+BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBConvertShipmentFromDirectToHouse+BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBConverttoLTL+BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBConvertShipmentDirection+BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBCopyShipment+BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBReactivateShipment+BaseSelectors.LastElement, BaseSelectors.BeDisabled)
   BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentBSplitShipment+BaseSelectors.LastElement, BaseSelectors.BeDisabled)

}
export function AssertShipmenteDelivaryWindowDisabled() {
  BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryCarrier, BaseSelectors.BeDisabled)
  BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryCarrierNumber, BaseSelectors.BeDisabled)
  BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryDriver, BaseSelectors.BeDisabled)
  BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryTruckNumber, BaseSelectors.BeDisabled)
  BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryTrailerNumber, BaseSelectors.BeDisabled)
  BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryByRail, BaseSelectors.BeDisabled)
  BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryByTruck, BaseSelectors.BeDisabled)
  BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryEmptyDeliveryContainer, BaseSelectors.BeDisabled)
  BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentPickUpDeliveryEmptyDeliveryDepotReference, BaseSelectors.BeDisabled)
  BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryATDDate, BaseSelectors.BeDisabled)
  BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryATDTime, BaseSelectors.BeDisabled)
  BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryETDDate, BaseSelectors.BeDisabled)
  BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryETDTime, BaseSelectors.BeDisabled)
  BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryETATime, BaseSelectors.BeDisabled)
  BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryETADate, BaseSelectors.BeDisabled)
  BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryATADate, BaseSelectors.BeDisabled)
  BaseAssertion.AssertElementDisabled(ShipmentSelectors.PickUpDeliveryATATime, BaseSelectors.BeDisabled)
}



 