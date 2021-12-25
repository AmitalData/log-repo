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

export function FillDeliveryRouting(DelivaryDeteails:DelivaryDeteails) {
    
}