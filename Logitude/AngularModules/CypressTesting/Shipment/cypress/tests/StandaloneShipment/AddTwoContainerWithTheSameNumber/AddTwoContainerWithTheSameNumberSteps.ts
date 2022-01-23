import * as Actions from "../../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../../models/ShipmentDetails";
import { PickupDelivaryDetails } from "../../../models/PickupDelivaryDetails";
import * as BaseAssertion from "../../../../../Base/cypress/actions/Assertion";
import * as Assists from "../../../../../Base/cypress/assists/Assists";
import { RequestAliases } from "../../../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../../selectors/Selectors"
import { BaseSelectors } from '../../../../../Base/cypress/selectors/BaseSelectors';
import { PackagesDetails } from "cypress/models/PackagesDetails";

let shipmentDetails: ShipmentDetails;
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

//#region Add Two container
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

  Then("a validation message with {string} error should appear", (validationMessage) => {
    BaseAssertion.AssertElementContain(BaseSelectors.SingleError, validationMessage)
  });
  //#endregion
  









