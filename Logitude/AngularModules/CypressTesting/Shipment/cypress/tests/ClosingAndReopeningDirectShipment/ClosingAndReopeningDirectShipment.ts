import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Selectors } from "../../selectors/Selectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { MainCarriageLeg } from "cypress/models/MainCarriageLeg";

let ShipmentData: ShipmentDetails;
let shipmentNumber: string;

Given("the user logged in and navigates to shipments workspace", () => {
  cy.Login()
  cy.Click(BaseSelectors.OperationsMenu, null)
  cy.Click(Selectors.ShipmentTab, null)
});

Given("a direct shipment with the following details",
  (dataTable) => {
   let shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
   ShipmentData = shipmentDetails;
   Actions.OpenNewShipmentWizard(ShipmentData.ShipmentLevel);
   Actions.FillShipmentWizardsFields(ShipmentData);
});

When("create shipment", () => {
  Actions.CreateShipment(ShipmentData.ShipmentLevel);
});

Then("the shipment should create successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostShipmentRequest", 200).then((interception) => {
        shipmentNumber = interception.response.body.ShipmentNumber;
    })
});

Given("the user in the direct's shipment rounting tab",()=>{
    Actions.OpenShipment(shipmentNumber);
    cy.Click(Selectors.RoutingsTab, null);
});

Given("edit Main Carriage Leg with the follwing details",(dataTable)=>{
    let mainCarriageLeg = dataTable.hashes()[0] as MainCarriageLeg;
    Actions.EditMainCarriageLegs(mainCarriageLeg.Airline);
}); 

When("close shipment operationally",()=>{
    cy.Click(Selectors.ShipmentMoreList, null);
    cy.Click(Selectors.OperationalCloseButton, null);
    Actions.UpdateClosedShipment();
});

When("close shipment Accountly",()=>{
    cy.Click(Selectors.ShipmentMoreList, null);
    cy.Click(Selectors.AccountllyCloseButton, null);
    Actions.UpdateClosedShipment();
});

Then("the shipment should close successfully",()=>{
    BaseAssertion.AssertStatusCode("WaitPutShipmentRequest", 200)
});

When("reopen shipment Accountly",()=>{
    cy.Click(Selectors.ShipmentMoreList, null);
    cy.Click(Selectors.AccountllyReopenButton, null);
    Actions.UpdateClosedShipment();
});

When("reopen shipment operationally",()=>{
    cy.Click(Selectors.ShipmentMoreList, null);
    cy.Click(Selectors.OperationalReopenButton, null);
    Actions.UpdateClosedShipment();
});

Then("the shipment should Reopen successfully",()=>{
    BaseAssertion.AssertStatusCode("WaitPutShipmentRequest", 200)
});