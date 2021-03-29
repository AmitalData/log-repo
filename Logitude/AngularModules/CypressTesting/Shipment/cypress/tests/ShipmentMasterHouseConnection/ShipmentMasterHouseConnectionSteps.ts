import * as Actions from '../../actions/Actions';
import { ShipmentSelectors } from '../../selectors/Selectors';
import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { ShipmentDetails } from '../../models/ShipmentDetails';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { BaseSelectors } from '../../../../Base/cypress/selectors/BaseSelectors';

//#region variables
let MasterShipmentDetails: ShipmentDetails;
let shipmentNumber: string;
let shipmentDetails: ShipmentDetails;

//#endregion

//#region Create master export air shipment
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    Actions.NavigatesToShipmentsWorkspace()
});

Given("a master Shipment with following details",(dataTable)=>{
    const shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    MasterShipmentDetails = shipmentDetails;
    Actions.OpenNewShipmentWizard(MasterShipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(MasterShipmentDetails);
});

When("create shipment", () => {
    Actions.CreateShipment(MasterShipmentDetails.ShipmentLevel);
  });
  
Then("the master should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        shipmentNumber = interception.response.body.ShipmentNumber;
    })
});
//#endregion

//#region Create house export air shipment inside the master
Given("the user in the master's Shipment tab",()=>{
    Actions.OpenShipment(shipmentNumber);
    cy.Click(ShipmentSelectors.ShipmentsTab, null);
});

When("create house with {string} as Shipper",(Shipper)=>{
    cy.Click(ShipmentSelectors.NewAttachedHouse,null);
    Actions.FillHouseInShipmentsTab(Shipper);
    Actions.CreateShipment("House");
});

Then("the house should create and connect successfully",()=>{
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
});
//#endregion

//#region Disconnect the house shipment
When("disconnect shipment",()=>{
    cy.get('#EditComponentBusyIndicator_0').should('not.exist');
    cy.Click("#EditComponentCellId_0_0 > div.MediaFill > table > tr:nth-child(3) > td > div > div.MediaFillAbsolute.CurvedEditArea > table > tr > td:nth-child(2) > div > ng-component:nth-child(3) > div > scrollviewer > div > div > div > ng-component > table > tr:nth-child(1) > td > div > table > tr:nth-child(3) > td > div > div > div.SimpleGridViewBody > table > tr:nth-child(1) > td > table > tr:nth-child(2) > td:nth-child(2) > table > tr > td:nth-child(2) > button",null);
    Actions.ConnectOrDisconnectShipment();
});

Then("the shipment should disconnect successfully",()=>{
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
});
//#endregion
//#region Create house export air shipment steps
Given("a house Shipment with the following details", (dataTable) => {
    cy.BackButton(BaseSelectors.ContainsOperations)
    shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(shipmentDetails);
});
When("create house shipment", () => {
    Actions.CreateShipment(shipmentDetails.ShipmentLevel);
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