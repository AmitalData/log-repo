import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Selectors } from "../../selectors/Selectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { MainCarriageLeg } from "cypress/models/MainCarriageLeg";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";

let ShipmentData: ShipmentDetails;
let shipmentNumber: string;

Given("the user logged in and navigates to shipments workspace", () => {
  cy.Login()
  Actions.NavigatesToShipmentsWorkspace()
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
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        shipmentNumber = interception.response.body.ShipmentNumber;
    })
});

Given("the user in the direct's shipment rounting tab",()=>{
    Actions.OpenShipment(shipmentNumber);
    cy.Click(Selectors.RoutingsTab, null);
});

Given("edit main carriage leg with the follwing details",(dataTable)=>{
    let mainCarriageLeg = dataTable.hashes()[0] as MainCarriageLeg;
    Actions.EditMainCarriageLegs(mainCarriageLeg.Airline);
    cy.Click(Selectors.ShipmentSaveButton, null);
}); 

When("close shipment operationally",()=>{
    cy.Click(Selectors.ShipmentMoreList, null,true);
    cy.Click(Selectors.OperationalCloseButton, null);
    Actions.UpdateClosedShipment();
});

When("close shipment Accountly",()=>{
    cy.Click(Selectors.ShipmentMoreList, null,true);
    cy.Click(Selectors.AccountllyCloseButton, null);
    Actions.UpdateClosedShipment();
});

Then("the shipment should close successfully",()=>{
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
});

When("reopen shipment Accountly",()=>{
    cy.Click(Selectors.ShipmentMoreList, null,true);
    cy.Click(Selectors.AccountllyReopenButton, null);
    Actions.UpdateClosedShipment();
});

When("reopen shipment operationally",()=>{
    cy.Click(Selectors.ShipmentMoreList, null,true);
    cy.Click(Selectors.OperationalReopenButton, null);
    Actions.UpdateClosedShipment();
});

Then("the shipment should reopen successfully",()=>{
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
});