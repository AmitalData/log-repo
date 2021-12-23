import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { SignatureDetails } from "../../../cypress/models/SignatureDetails";
import { ShipmentDetails } from '../../../../Shipment/cypress/models/ShipmentDetails';
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";

let signatureDetails: SignatureDetails;
//#region Edit signature from maintenance
Given("the user logged in and navigate to {string} in maintenance menu", (Signature) => {
    cy.Login()
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(Signature, MaintenanceSelectors.MaintenanceItemSignature)
});

And("the user edits the HTML template as following", (dataTable) => {
    signatureDetails = Assists.CreateInstance<SignatureDetails>(dataTable, true);
    MaintenanceActions.AddDataFields(signatureDetails)
});

When("the user saves the new Signature", () => {
    MaintenanceActions.UpdateSignature();
});

Then("the Signature should update successfully", () => {
    MaintenanceActions.AssertUpdateSignature()
});
//#Endregion