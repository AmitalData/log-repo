import * as Actions from "../../actions/Actions"
import { Selectors } from "../../selectors/Selectors"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { ShipmentDetails } from "../../models/ShipmentDetails";

let MasterShipmentDetails: ShipmentDetails;
let shipmentNumber: string;

Given("the user logged in", () => {
  cy.Login()
});

Given("navigate to shipments workspace", () => {
  cy.Click(BaseSelectors.OperationsMenu, null)
  cy.Click(Selectors.ShipmentTab, null)
});

Given("a master Shipment with following details",(dataTable)=>{
    const shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    MasterShipmentDetails = shipmentDetails;
    Actions.OpenNewShipmentWizard(MasterShipmentDetails.ShipmentLevel);
    Actions.FillShipmentDefaultFields(MasterShipmentDetails);
});

When("create shipment", () => {
    Actions.CreateShipment(MasterShipmentDetails.ShipmentLevel);
  });
  
Then("the master should create successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostShipmentRequest", 200).then((interception) => {
        shipmentNumber = interception.response.body.ShipmentNumber;
    })
});

Given("the user in the master's Shipment tab",()=>{
    Actions.OpenShipment(shipmentNumber);
    cy.Click(Selectors.ShipmentsTab, null);
});

When("create house with {string} as Shipper",(Shipper)=>{
    cy.Click(Selectors.NewAttachedHouse,null);
    Actions.FillHouseInShipmentsTab(Shipper);
    Actions.CreateShipment("House");
});

Then("the house should create and connect successfully",()=>{
    BaseAssertion.AssertStatusCode("WaitPostShipmentRequest", 200);
});

When("disconnect shipment",()=>{

    cy.Click(BaseSelectors.Button,"Disconnect All");
    cy.Click(BaseSelectors.RedButton,"Yes");
    cy.DefineRequestWait("PUT", "**/shipment", "WaitPutShipmentRequest")
});

Then("the shipment should disconnect successfully",()=>{
    BaseAssertion.AssertStatusCode("WaitPutShipmentRequest", 200)
});



