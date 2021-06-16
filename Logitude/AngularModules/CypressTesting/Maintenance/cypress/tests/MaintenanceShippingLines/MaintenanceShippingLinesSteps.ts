import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import * as ShippingLineActions from "../../actions/ShippingLinesActions";
import { ShippingLineSelectors } from "../../../cypress/selectors/ShippingLineSelectors";
import * as MaintenanceActions from "../../actions/Actions";
import { ShippingLineDetails } from "../../../cypress/models/ShippingLineDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"

//#region Create new branch
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemShippingLine)
});

Given("a shippingLine with the following details", (dataTable) => {
    let shippingLineDetails = Assists.CreateInstance<ShippingLineDetails>(dataTable, true);
    ShippingLineActions.FillShippingLineDetails(shippingLineDetails)
});

When("create shipping Line", () => {
    ShippingLineActions.CreateShippingLine();
});

Then("the shipping Line should create successfully", () => {
    ShippingLineActions.AssertCreateShippingLine();
});
//#endregion


//#region Search for the branch by name
When("search shipping Line", () => {
    ShippingLineActions.SearchShippingLine()
});

Then("the shipping Line should appear successfully", () => {
    ShippingLineActions.AssertSearchShippingLine()
});
//#endregion


//#region Open the branch
When("open shipping Line", () => {
    ShippingLineActions.OpenShippingLine();
});

Then("the shipping Line should open successfully", () => {
    ShippingLineActions.AssertOpenShippingLine();
});
//#endregion


//#region Edit the branch
Given("check dim input in shipping line INTTRA", () => {
    cy.Navigate(ShippingLineSelectors.INTTRA);
    ShippingLineActions.CheckShippingLineINTTRA()
});

Given("fill the following Address in Addresses Shipping line", (dataTable) => {
    let shippingLineDetails = Assists.CreateInstance<ShippingLineDetails>(dataTable, true);
    cy.Navigate(ShippingLineSelectors.AddressesTab);
    ShippingLineActions.FillShippingLineAddresses(shippingLineDetails)
});

When("create shipping line address", () => {
    ShippingLineActions.CreateShippingLineAddress()
});

Then("the shipping line address should create successfully", () => {
    ShippingLineActions.AssertCreateShippingLineAddress()
});

Given("fill the following Area in Areas Shipping line", (dataTable) => {
    let shippingLineDetails = Assists.CreateInstance<ShippingLineDetails>(dataTable, true);
    cy.Navigate(ShippingLineSelectors.AreasTab);
    ShippingLineActions.FillShippingLineAreas(shippingLineDetails)
});

When("create shipping line area", () => {
    ShippingLineActions.CreateShippingLineArea()
});

Then("the shipping line area should create successfully", () => {
    ShippingLineActions.AssertCreateShippingLineArea()
});

Given("fill the following Tariff in Tariff Translations Tab", (dataTable) => {
    let shippingLineDetails = Assists.CreateInstance<ShippingLineDetails>(dataTable, true);
    cy.Navigate(ShippingLineSelectors.TariffTranslations);
    cy.Navigate("#addTranslation")
    ShippingLineActions.FillShippingLineTariffTranslations(shippingLineDetails)
});

When("create shipping line Tariff translation", () => {
    ShippingLineActions.CreateShippingLineTariffTranslations()
});

Then("the shipping line Tariff translation should create successfully", () => {
    ShippingLineActions.AssertCreateShippingLineTariffTranslations()
});

Given("the user activate shipping line", () => {
    cy.Navigate(ShippingLineSelectors.GeneralTab);
    MaintenanceActions.ChangeInactiveCheckBoxValue(ShippingLineSelectors.InActiveShippingLineCheckBox)
});

When("edit shipping line", () => {
    ShippingLineActions.EditShippingLine();
});

Then("the shipping line should update successfully", () => {
    ShippingLineActions.AssertEditShippingLine();
});

Then("following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, ShippingLineSelectors.ShippingLineEventsTab);
});

When("save and close shipping line", () => {
    ShippingLineActions.CloseSaveShippingLine();
});

Then("the shipping line should close successfully", () => {
    ShippingLineActions.AssertCloseSaveShippingLine();
});

//#endregion