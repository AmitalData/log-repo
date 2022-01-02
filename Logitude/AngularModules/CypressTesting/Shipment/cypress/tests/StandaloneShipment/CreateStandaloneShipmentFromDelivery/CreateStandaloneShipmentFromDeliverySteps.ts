import * as Actions from "../../../actions/Actions";
import * as StandaloneAction from "../../../actions/StandaloneAction";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../../models/ShipmentDetails";
import { DelivaryDeteails } from "../../../models/DelivaryDeteails";
import * as BaseAssertion from "../../../../../Base/cypress/actions/Assertion";
import * as Assists from "../../../../../Base/cypress/assists/Assists";
import { RequestAliases } from "../../../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../../selectors/Selectors"
import { RestAPI } from '../../../../../Base/cypress/constants/RestAPI';
import { URLs } from '../../../constants/URLs';
import { BaseSelectors } from '../../../../../Base/cypress/selectors/BaseSelectors';

let shipmentDetails: ShipmentDetails;
let DelivarytData: DelivaryDeteails;
let shipmentNumber: string;

Given("the user logged in and navigates to shipments workspace", () => {
  cy.Login()
  Actions.NavigatesToShipmentsWorkspace()

});

Given("a direct shipment with the following details", (dataTable) => {
  shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
  Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
  Actions.FillShipmentWizardsFields(shipmentDetails);
  cy.FillLogLov(ShipmentSelectors.ShipmentShipper, shipmentDetails.Shipper, true)

});

When("create shipment", () => {
  Actions.CreateShipment(shipmentDetails.ShipmentLevel);

});

Then("the shipment should create successfully", () => {
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
    shipmentNumber = interception.response.body.ShipmentNumber

  })

});

Given("the user open the shipment and navigate to RoutingsTab workspace", () => {
  Actions.OpenShipment(shipmentNumber);
  cy.Navigate(ShipmentSelectors.RoutingsTab);
  cy.Navigate(ShipmentSelectors.AddDelivery);
  cy.get(".Button").contains("Add Delivery").click()
 
});

Given("unchecked the FullResponsibility",()=>{
  cy.get("#CheckBox_0_8_LBL").click()

})

Given("the user in the shipment's  routings tab",()=>{
  cy.Navigate(ShipmentSelectors.CloseBtn,true)
  cy.Navigate(ShipmentSelectors.RoutingToggle,true)
  cy.Navigate(ShipmentSelectors.Delivery,true)
  cy.get(".Button").contains("Add Delivery").click()

 })

Given("add a new Delivery leg with the following details", (dataTable) => {
  DelivarytData = Assists.CreateInstance<DelivaryDeteails>(dataTable, true);
  StandaloneAction.FillALL(DelivarytData);
  
});

Given("save the Delivery", () => {
  cy.DefineRequestWait(RestAPI.PUT, URLs.Shipment, RequestAliases.ShipmentRequest)
  cy.Navigate(ShipmentSelectors.SavePickupDelivery,true) 
  BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
  
});

When("click create Standalone Shipment",()=>{
  cy.get("#printbutton").contains(" Create Standalone Shipment ").click()

})

When("create standalone shipment",()=>{
 StandaloneAction.CreateStandaloneShipment()
 cy.wait(100)

})

Then("a domestic inland shipment should create", () => {
  //BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
  cy.wait(5000)
  
});

Then("the cancel ,operational close Shipment,convert to custom file and Send Response action in more button shouldn't be dim",()=>{
  cy.Click(BaseSelectors.ToggleButtonClass + BaseSelectors.LastElement, null)
  StandaloneAction.AssertShipmenteMenuButtonsEnabled()

})

Then("all other actions should be dim",()=>{
  StandaloneAction.AssertShipmenteMenuButtonsDisabled()

})

Then("all fields should be dim in Delivery window",()=>{
  cy.Navigate(ShipmentSelectors.Backbutton+BaseSelectors.LastElement,true)
  StandaloneAction.AssertShipmenteDelivaryWindowDisabled()

})

Then("the link of standalon should display",()=>{
  BaseAssertion.AssertElementExist(ShipmentSelectors.StandaloneShipmentHyperlink)

})

Then("a validation message with {string} error should appear", (validationMessage) => {
  BaseAssertion.AssertElementContain(BaseSelectors.SingleError, validationMessage)
  
});

Then("the Create Standalone Shipment button Should be dim",()=>{
  BaseAssertion.AssertElementNotVisible("#printbutton")
  
})







































