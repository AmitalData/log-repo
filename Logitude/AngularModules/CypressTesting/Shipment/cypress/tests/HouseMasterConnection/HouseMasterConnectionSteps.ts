import * as Actions from "../../actions/Actions";
import { Selectors } from "../../selectors/Selectors";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";

let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;

Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    cy.Click(BaseSelectors.OperationsMenu, null)
    cy.Click(Selectors.ShipmentTab, null)
});

Given("a master Shipment with the following details", (dataTable) => {
    shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(shipmentDetails);
});

Given("a house Shipment with the following details", (dataTable) => {
    shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(shipmentDetails);
});

Given("the user in the master's Shipment tab", () => {
    cy.Click(BaseSelectors.OperationsMenu, null)
    cy.Click(Selectors.ShipmentTab, null)
    cy.log("ShipmentNumberfinal")
    cy.log(shipmentNumber)
    Actions.OpenShipment(shipmentNumber);
    cy.Click(Selectors.ShipmentsTab, null);
});

When("create shipment", () => {
    Actions.CreateShipment(shipmentDetails.ShipmentLevel);
});

When("connect the house shipment", () => {
    cy.get('#EditComponentBusyIndicator_0').should('not.exist');
    cy.Click("#EditComponentCellId_0_0 > div.MediaFill > table > tr:nth-child(3) > td > div > div.MediaFillAbsolute.CurvedEditArea > table > tr > td:nth-child(2) > div > ng-component:nth-child(3) > div > scrollviewer > div > div > div > ng-component > table > tr:nth-child(1) > td > div > table > tr:nth-child(3) > td > div > div > div.SimpleGridViewBody > table > tr:nth-child(1) > td > table > tr:nth-child(2) > td:nth-child(2) > table > tr > td:nth-child(2) > button", null);
    Actions.ConnectShipment();
});

When("the user disconnect the house shipment", () => {
    cy.get('#EditComponentBusyIndicator_0').should('not.exist');
    cy.Click("#EditComponentCellId_0_0 > div.MediaFill > table > tr:nth-child(3) > td > div > div.MediaFillAbsolute.CurvedEditArea > table > tr > td:nth-child(2) > div > ng-component:nth-child(3) > div > scrollviewer > div > div > div > ng-component > table > tr:nth-child(1) > td > div > table > tr:nth-child(3) > td > div > div > div.SimpleGridViewBody > table > tr:nth-child(1) > td > table > tr:nth-child(2) > td:nth-child(2) > table > tr > td:nth-child(2) > button", null);
    Actions.DisconnectShipment();
});

Then("the master should create successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostShipmentRequest", 200).then((interception) => {
        shipmentNumber = interception.response.body.ShipmentNumber;
    })
});

Then("the house should create successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostShipmentRequest", 200)
});

Then("the shipment should connect successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutShipmentRequest", 200);
});

Then("the shipment should disconnect successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutShipmentRequest", 200);
});