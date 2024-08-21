import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as LinksGatewayPREQActions from "../../actions/LinksGatewayPREQActions";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { LinksGatewayPREQDetails } from "cypress/models/LinksGatewayPREQDetails";
import { LinksGatewayPREQsSelectors } from "../../selectors/LinksGatewayPREQSelectors"
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails"


//#region Logged in to LinksGatewayPREQ

Given("the user logged in to LinksGatewayPREQ", () => {
        cy.visit('https://accounting-staging.amital.co.il/accounting/LinksGateway.aspx?Menu=PREQ&SecurityKey=d5e6d15f4cb24f12a8ac9c5e8c54a06d');
});


When("logged in", () => {

 });

Then("value = 1", () => {
    LinksGatewayPREQActions.AssertLinksGatewayPREQ()

});

//#endregion








