import * as Actions from '../../actions/Actions';
import * as MasterHouseConnectionRoutingActions from '../../actions/MasterHouseConnectionRoutingActions';
import { ShipmentSelectors } from '../../selectors/Selectors';
import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { ShipmentDetails } from '../../models/ShipmentDetails';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ShipmentContext } from '../../models/ShipmentContext';
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';

let MasterShipmentDetails: ShipmentDetails;

//#region Create master export air/Ocean shipment
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    Actions.NavigatesToShipmentsWorkspace()
});

Given("the user navigates to shipments workspace", () => {
    cy.Click(ShipmentSelectors.Backbutton, null)
});

Given("a master Shipment with following details", (dataTable) => {
    MasterShipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    Actions.OpenNewShipmentWizard(MasterShipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(MasterShipmentDetails);
});

When("create shipment", () => {
    Actions.CreateShipment(MasterShipmentDetails.ShipmentLevel);
});

Then("the master should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        ShipmentContext.MasterNumber = interception.response.body.ShipmentNumber;
    })
});
//#endregion

//#region add pre carriage and on carriage on master shipment
Given("open the shipment", () => {
    Actions.OpenShipment(ShipmentContext.MasterNumber);
})
Given("add pre carriage from port {string} to port {string}", (fromPort, toPort) => {
    Actions.FillPreCarriageRouting(MasterShipmentDetails.TransportMode, fromPort, toPort)
});

Given("add on carriage from port {string} to port {string}", (fromPort, toPort) => {
    Actions.FillOnCarriageRouting(MasterShipmentDetails.TransportMode, fromPort, toPort)
});
//#endregion

//#region Create house export air shipment inside the master
Given("the user in the master's Shipment tab", () => {
    cy.Navigate(ShipmentSelectors.ShipmentsTab);
});

When("create house with {string} as Shipper", (Shipper) => {
    Actions.CreateNewAttachedHouse(Shipper);
});

Then("the house should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        ShipmentContext.HouseNumber = interception.response.body.House;
    })
});

Then("the house should connect successfully", () => {
    Actions.CheckBusyIndicator()
    Actions.ValidateCheckHouseCheckBox();
    cy.wait(5000)
});
//#endregion

//#region shipment actions
When("update the shipment", () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton_Number + BaseSelectors.LastElement)
});

Then("the shipment should update successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion

//#region add pre forwarding and on forwarding on house shipment
Given("navigates routing tab in house shipment", () => {
    cy.get(ShipmentSelectors.HouseHyperLink).eq(0).click({ force: true })
    cy.Click(ShipmentSelectors.RoutingsTab_Number + BaseSelectors.LastElement, null)
});

Given("add pre forwarding from port {string}", (fromPort) => {
    MasterHouseConnectionRoutingActions.FillPreForwarding(MasterShipmentDetails.TransportMode, fromPort)
});

Given("add on forwarding to port {string}", (toPort) => {
    MasterHouseConnectionRoutingActions.FillOnForwarding(MasterShipmentDetails.TransportMode, toPort)
});
//#endregion

//#region Assert pre carriage and on carriage are dim in house shipment
When("open pre carriage edit screen", () => {
    cy.Click(ShipmentSelectors.EditPreCarriage_Number + BaseSelectors.LastElement, null)
});

When("open on carriage edit screen", () => {
    cy.Click(ShipmentSelectors.EditOnCarriage_Number + BaseSelectors.LastElement, null)
});

Then("this message {string} should be printed", (message) => {
    MasterHouseConnectionRoutingActions.AssertPreOnCarriageDisabled(message)
});
//#endregion