import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { URLs } from "../constants/URLs";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { LinksGatewayUIDDetails } from "../models/LinksGatewayUIDDetails";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { LinksGatewayUIDsSelectors } from "../selectors/LinksGatewayUIDSelectors"
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { GenerateCurrentDatetimeString } from '../../../Base/cypress/actions/GenerateRandoms';


export function NavigatesToLinksGatewayUDI(){
    cy.visit('https://accounting-staging.amital.co.il/accounting/LinksGateway.aspx?Menu=UID&SecurityKey=d5e6d15f4cb24f12a8ac9c5e8c54a06d&Tenant=');
}


export function AssertLinksGatewayUDI() {
    cy.DefineRequestWait(RestAPI.POST, URLs.qaIndicatorUID, RequestAliases.LinksGatewayUID)
    
}


