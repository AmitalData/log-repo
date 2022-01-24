import * as Actions from "../../../actions/Actions";
import * as StandaloneAction from "../../../actions/StandaloneAction";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../../models/ShipmentDetails";
import { PickupDelivaryDetails } from "../../../models/PickupDelivaryDetails";
import * as BaseAssertion from "../../../../../Base/cypress/actions/Assertion";
import * as Assists from "../../../../../Base/cypress/assists/Assists";
import { RequestAliases } from "../../../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../../selectors/Selectors"
import { BaseSelectors } from '../../../../../Base/cypress/selectors/BaseSelectors';
import { ShipmentConstants } from "../../../constants/constants";
import { PackagesDetails } from "cypress/models/PackagesDetails";

let shipmentDetails: ShipmentDetails;
let pickupDelivarytData: PickupDelivaryDetails;
let shipmentNumber: string;
let containerDetailsList

//#region create direct shipment
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
//#endregion

//#region Add Packages
Given("the user open the shipment and navigate to packages workspace", () => {
Actions.OpenShipment(shipmentNumber);
cy.Navigate(ShipmentSelectors.PackagesTab);
});
  
Given("a container with the following details", (dataTable) => {
containerDetailsList = Assists.CreateSet<PackagesDetails>(dataTable);
Actions.FillPackageTab(shipmentDetails.TransportMode, containerDetailsList, shipmentDetails.ShipmentType);
});
  
When("save shipment", () => {
Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});
  
Then("the direct shipment should save successfully", () => {
BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion
  
//#region Add pickup
Given("the user in shipment routing tab", () => {
cy.Click(ShipmentSelectors.RoutingsTab, null)
cy.Click(ShipmentSelectors.AddPickUp, null)
cy.Click(BaseSelectors.Button, ShipmentConstants.AddPickUp)
})
  
Given("add a new pickup leg with the following details", (dataTable) => {
pickupDelivarytData = Assists.CreateInstance<PickupDelivaryDetails>(dataTable, true);
StandaloneAction.FillPickUpDelivaryDetails(pickupDelivarytData);
});

When("the user in the pickup packages",()=>{
cy.Click(ShipmentSelectors.PickUpDeliveryPackages, null)
cy.Click(ShipmentSelectors.AddContainerFromPickup, null, true)
}) 

Then("no container will appear in the window",()=>{
    BaseAssertion.AssertElementNotExist(BaseSelectors.RowsInPickUpPackages)
})
//#endregion
