import * as Actions from "../../actions/Actions"
import { ShipmentSelectors } from "../../selectors/Selectors"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { PartnersDetails } from "cypress/models/PartnersDetails";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { ShipmentDetails } from "cypress/models/ShipmentDetails";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import * as Assists from "../../../../Base/cypress/assists/Assists";

let shipmentDetails: ShipmentDetails;

//#region Create direct export air shipment Given steps
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    Actions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
    shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(shipmentDetails);
});
//#endregion

//#region Update and delete partners tab given step
Given("the user add partners with following details", (dataTable) => {
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    const partnersDetails = Assists.CreateInstance<PartnersDetails>(dataTable, true);
    Actions.FillPartnersTab(shipmentDetails.Direction, shipmentDetails.TransportMode, partnersDetails)
});

Given("the user delete the below partners", (dataTable) => {
    const partnersDetails = Assists.CreateInstance<PartnersDetails>(dataTable, true);
    Actions.DeletePartnersInPartnersTab(partnersDetails)
});
//#endregion

//#region Actions steps
When("create shipment", () => {
    Actions.CreateShipment(shipmentDetails.ShipmentLevel);
});

When("update shipment", () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});
//#endregion

//#region Assert steps
Then("the direct should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        shipmentDetails.ShipmentNumber = interception.response.body.ShipmentNumber;
    })
});

Then("the direct should update successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion