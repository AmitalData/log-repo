import { ShipmentSelectors } from '../../../Shipment/cypress/selectors/Selectors';
import { ShipmentDetails } from '../models/ShipmentDetails';
import { PartnersDetails } from 'cypress/models/PartnersDetails';
import { BaseSelectors } from '../../../Base/cypress/selectors/BaseSelectors';
import { PayableDetails } from 'cypress/models/PayableDetails';
import { ReceivableDetails } from 'cypress/models/ReceivableDetails';
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import * as GenerateRandoms from '../../../Base/cypress/actions/GenerateRandoms';
import { PackagesDetails } from 'cypress/models/PackagesDetails';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { RequestAliases } from '../../../Base/cypress/constants/RequestAliases';
import * as Conditions from "../actions/Conditions";
import { BaseURLs } from '../../../Base/cypress/constants/URLs';
import { QuickSearchDetails } from '../../../Base/cypress/models/QuickSearchDetails';
import * as BaseActions from '../../../Base/cypress/actions/Actions';
import { EventDetails } from '../models/EventDetails';
import { EventTypeDetails } from '../models/EventTypeDetails';
import { WarehouseStorage } from 'cypress/models/WarehouseStorage';
import { ShipmentContext } from '../models/ShipmentContext';
import { DelivaryDeteails } from "../models/DelivaryDeteails";
import {RegexSelectors} from "../selectors/RegexSelectors";
import { ShipmentConstants } from '../constants/constants'




export function FillALL(delivaryDeteails:DelivaryDeteails) {
    FillDeliveryRouting(delivaryDeteails)
    FillFromPickupDelivary(delivaryDeteails)
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
        cy.FillLogLov(ShipmentSelectors.DeliveryFromCityName,delivaryDeteails.FromCity , true)
    }

     
 }
 