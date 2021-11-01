import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { ShipmentSelectors } from "../../selectors/Selectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { MainCarriageLeg } from "cypress/models/MainCarriageLeg";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";

let ShipmentData: ShipmentDetails;
let shipmentNumber: string;

//#region Create direct export air shipment
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    Actions.NavigatesToShipmentsWorkspace()
});

Given("a direct shipment with the following details", (dataTable) => {
    ShipmentData = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
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
//#endregion

//#region Close direct shipment operationally
Given("the user in the direct's shipment rounting tab", () => {
    Actions.OpenShipment(shipmentNumber);
    cy.Click(ShipmentSelectors.RoutingsTab, null);
});

Given("edit main carriage leg with the following details", (dataTable) => {
    let mainCarriageLeg = Assists.CreateInstance<MainCarriageLeg>(dataTable, true);
    Actions.EditMainCarriageLegs(mainCarriageLeg.Airline);
    cy.Click(ShipmentSelectors.ShipmentSaveButton, null);
});

When("close shipment operationally", () => {
    cy.Click(ShipmentSelectors.ShipmentMoreList, null, true);
    cy.Click(ShipmentSelectors.OperationalCloseButton, null);
    Actions.UpdateClosedShipment();
});
//#endregion

//#region Assert add/cannot add or edit orders
When("navigates to orders tab", () => {
    cy.Click(ShipmentSelectors.OrdersTab, null);
});

Then("the orders workspace should be dim", () => {
    Actions.AssertOrdersTabWorkSpaceFieldsDisable();
});

Then("the user should be able to add orders", () => {
    BaseAssertion.AssertElementDisabled(ShipmentSelectors.OrdersAddPackage, BaseSelectors.NotBeDisabled)
});
//#endregion

//#region Assert add/cannot add packages
When("navigates to packages tab", () => {
    cy.Click(ShipmentSelectors.PackagesTab, null);
});

Then("the packages workspace should be dim", () => {
    Actions.AssertPackagesTabWorkSpaceFieldsDisable();
});

Then("the user should be able to add package", () => {
    BaseAssertion.AssertElementDisabled(ShipmentSelectors.OrdersAddPackage, BaseSelectors.NotBeDisabled)
});
//#endregion

//#region Assert add/cannot add routing
When("navigates to routing tab", () => {
    cy.Click(ShipmentSelectors.RoutingsTab, null);
});

Then("the routing workspace should be dim", () => {
    Actions.AssertRoutingTabWorkSpaceButtonsDisable();
});

Then("all fields of the main carriage leg should be dim", () => {
    cy.get(ShipmentSelectors.EditRoutingMainCarriage).click({ force: true })
    Actions.AssertMainCarriageLegFieldsDisable();
});

Then("the routing workspace should not be dim", () => {
    Actions.AssertRoutingTabWorkSpaceButtonsEnable();
});
//#endregion

//#region Assert add/cannot add or edit partners
When("navigates to partners tab", () => {
    cy.Click(ShipmentSelectors.PartnersTab, null);
});

Then("the user should not be able to add or edit partners", () => {
    Actions.AssertCannotAddEditPartners();
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsCancel)
});

Then("the user should be able to add or edit partners", () => {
    Actions.AssertAddEditPartners();
});
//#endregion

//#region Close direct shipment accountly
When("close shipment Accountly", () => {
    cy.Click(ShipmentSelectors.ShipmentMoreList, null, true);
    cy.Click(ShipmentSelectors.AccountllyCloseButton, null);
    Actions.UpdateClosedShipment();
});

Then("the shipment should close successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
});
//#endregion

//#region Assert add/cannot add payable
When("navigates to payables tab", () => {
    cy.Click(ShipmentSelectors.PayablesTab, null);
});

Then("the user should not be able to add payable", () => {
    BaseAssertion.AssertElementDisabled(ShipmentSelectors.AddNewPayableLine, BaseSelectors.BeDisabled)
});

Then("the user should be able to add payable", () => {
    BaseAssertion.AssertElementDisabled(ShipmentSelectors.AddNewPayableLine, BaseSelectors.NotBeDisabled)
});
//#endregion

//#region Assert cannot add receivable
When("navigates to receivables tab", () => {
    cy.Click(ShipmentSelectors.ReceivablesTab, null);
});

Then("the user should not be able to add receivable", () => {
    BaseAssertion.AssertElementDisabled(ShipmentSelectors.AddNewReceivableLine, BaseSelectors.BeDisabled)
});

Then("the user should be able to add receivable", () => {
    BaseAssertion.AssertElementDisabled(ShipmentSelectors.AddNewReceivableLine, BaseSelectors.NotBeDisabled)
});
//#endregion

When("reopen shipment Accountly", () => {
    cy.Click(ShipmentSelectors.ShipmentMoreList, null, true);
    cy.Click(ShipmentSelectors.AccountllyReopenButton, null);
    Actions.UpdateClosedShipment();
});

When("reopen shipment operationally", () => {
    cy.Click(ShipmentSelectors.ShipmentMoreList, null, true);
    cy.Click(ShipmentSelectors.OperationalReopenButton, null);
    Actions.UpdateClosedShipment();
});

Then("the shipment should reopen successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
});