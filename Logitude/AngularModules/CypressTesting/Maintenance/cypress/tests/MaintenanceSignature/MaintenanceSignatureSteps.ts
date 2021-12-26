import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { SignatureDetails } from "../../../cypress/models/SignatureDetails";
import { ShipmentDetails } from '../../../../Shipment/cypress/models/ShipmentDetails';
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as BaseActions from "../../actions/BaseActions";
import { Urls } from "../../../cypress/constants/Urls";

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
    BaseActions.MockSave(Urls.Signature)
  //  MaintenanceActions.UpdateSignature();
});

Then("the Signature should update successfully", () => {
    BaseActions.AssertMockSave()
    //MaintenanceActions.AssertUpdateSignature()
});
//#Endregion