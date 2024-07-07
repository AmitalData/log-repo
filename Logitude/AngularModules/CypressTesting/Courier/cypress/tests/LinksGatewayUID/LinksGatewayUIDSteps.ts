import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as LinksGatewayUIDActions from "../../actions/LinksGatewayUIDActions";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { LinksGatewayPREQDetails } from "cypress/models/LinksGatewayPREQDetails";
import { LinksGatewayUIDsSelectors } from "../../selectors/LinksGatewayUIDSelectors"
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails"

//#region Logged in to LinksGatewayUID

Given("the user logged in to LinksGatewayUID", () => {
    cy.visit('https://test-accounting.amital.co.il/test/LinksGateway.aspx?Menu=UID&SecurityKey=d5e6d15f4cb24f12a8ac9c5e8c54a06d');
});


When("logged in", () => {

 });

Then("value = 1", () => {
     LinksGatewayUIDActions.AssertLinksGatewayUDI()

 });

//#endregion









