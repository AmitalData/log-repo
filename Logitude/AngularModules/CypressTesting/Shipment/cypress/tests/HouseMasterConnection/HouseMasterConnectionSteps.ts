import * as Actions from "../../actions/Actions";
import { ShipmentSelectors } from "../../selectors/Selectors";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";

//#region variables
let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;
//#endregion

//#region Create master export air shipment steps
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    Actions.NavigatesToShipmentsWorkspace()
});

Given("a master Shipment with the following details", (dataTable) => {
    shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(shipmentDetails);
});

Then("the master should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        shipmentNumber = interception.response.body.ShipmentNumber;
    })
});
//#endregion

//#region Create shipment step
When("create shipment", () => {
    Actions.CreateShipment(shipmentDetails.ShipmentLevel);
});
//#endregion

//#region Create house export air shipment steps
Given("a house Shipment with the following details", (dataTable) => {
    shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(shipmentDetails);
});

Then("the house should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
});
//#endregion

//#region Connect the house shipment to the master 
Given("the user in the master's Shipment tab", () => {
    cy.Click(BaseSelectors.OperationsMenu, null)
    cy.Click(ShipmentSelectors.ShipmentTab, null)
    Actions.OpenShipment(shipmentNumber);
    cy.Click(ShipmentSelectors.ShipmentsTab, null);
});

When("connect the house shipment", () => {
    cy.get('#EditComponentBusyIndicator_0').should('not.exist');
    cy.Click("#ConnectAll", null);
    Actions.ConnectOrDisconnectShipment();
});

Then("the shipment should connect successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion

//#region Disconnect the house shipment
When("the user disconnect the house shipment", () => {
    cy.get('#EditComponentBusyIndicator_0').should('not.exist');
    cy.Click("#DisconnectALL", null);
    Actions.ConnectOrDisconnectShipment();
});

Then("the shipment should disconnect successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
    cy.Click("#ConnectAll", null);
    Actions.ConnectOrDisconnectShipment();
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion