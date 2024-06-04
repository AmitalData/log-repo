import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { URLs } from "../constants/URLs";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { LinksGatewayPREQDetails } from "../models/LinksGatewayPREQDetails";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { LinksGatewayPREQsSelectors } from "../selectors/LinksGatewayPREQSelectors"
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { GenerateCurrentDatetimeString } from '../../../Base/cypress/actions/GenerateRandoms';



export function NavigatesToLinksGatewayPREQ(){
    cy.visit('https://test-accounting.amital.co.il/test/LinksGateway.aspx?Menu=PREQ&SecurityKey=d5e6d15f4cb24f12a8ac9c5e8c54a06d');
}


export function AssertLinksGatewayPREQ() {
    cy.DefineRequestWait(RestAPI.POST, URLs.qaIndicatorPREQ, RequestAliases.LinksGatewayPREQ)
    
}


