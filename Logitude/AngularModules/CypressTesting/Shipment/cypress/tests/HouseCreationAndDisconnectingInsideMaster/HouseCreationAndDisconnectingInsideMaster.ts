import * as Actions from "../../actions/Actions"
import * as Assertions from "../../actions/Assertions"
import { Selectors } from "../../selectors/Selectors"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { PartnersDetails } from "cypress/models/PartnersDetails";
import { ShipmentDetails } from "../../models/ShipmentDetails";


let MasterShipmentDetails: ShipmentDetails;

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
  
Then("the shipment should create successfully", () => {
    let resultFile = "CreatedShipmentsData/" + MasterShipmentDetails.ShipmentLevel + MasterShipmentDetails.Direction +
    MasterShipmentDetails.TransportMode +
    ((typeof MasterShipmentDetails.ShipmentType) === "undefined" || MasterShipmentDetails.ShipmentType === null ? "" : MasterShipmentDetails.ShipmentType) + ".json";
    Assertions.ValidateCreatedShipment(resultFile);
});

Given("the user in the master's Shipment tab",()=>{
    Actions.OpenShipment("CreatedShipmentsData/MasterEA.json");
    cy.Click(Selectors.ShipmentsTab, null);
});

When("create house with {string} as Shipper",(Shipper)=>{
    cy.Click(Selectors.NewAttachedHouse,null);
    Actions.FillHouseInShipmentsTab(Shipper);
    Actions.CreateShipment("House");
});

Then("the shipment should create and connect successfully",()=>{
    let resultFile = "CreatedShipmentsData/" + "House" + MasterShipmentDetails.Direction +
    MasterShipmentDetails.TransportMode +
    ((typeof MasterShipmentDetails.ShipmentType) === "undefined" || MasterShipmentDetails.ShipmentType === null ? "" : MasterShipmentDetails.ShipmentType) + ".json";
    Assertions.ValidateCreatedShipment(resultFile);
   
    //Assertions.HouseConnectedToMaster(resultFile,"CreatedShipmentsData/MasterEA.json");
});

When("disconnect shipment",()=>{
    cy.Click(BaseSelectors.Button,"Disconnect All");
    Actions.DisconnectShipment()
});

Then("the shipment should disconnect successfully",()=>{
    Assertions.ValidateUpdatedShipment(null);
});



