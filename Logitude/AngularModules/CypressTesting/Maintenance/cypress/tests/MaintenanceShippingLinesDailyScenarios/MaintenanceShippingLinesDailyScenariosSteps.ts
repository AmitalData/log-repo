import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as ShippingLineActions from "../../actions/ShippingLinesActions";
import * as GeneralActions from "../../actions/BaseActions";
import { ShippingLineSelectors } from "../../selectors/ShippingLineSelectors";
import * as MaintenanceActions from "../../actions/Actions";
import { ShippingLineDetails } from "../../models/ShippingLineDetails";
import { AddressDetails } from "../../models/AddressDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { Urls } from "../../constants/Urls";

//#region import new shipping line
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, ShippingLineSelectors.MaintenanceItem)
});

When("fake import shipping Line", () => {
    cy.Navigate(ShippingLineSelectors.ImportShippingLine_AddButton)
    GeneralActions.MockImport();
});

Then("the shipping Line should import successfully", () => {
    GeneralActions.AssertMockImport();
});
//#endregion

//#region Add Shipping Line Code with lenght more than 4
Given("the user navigate shipping line wizerd", () => {
    cy.Navigate(ShippingLineSelectors.AddNewShippingLine)
});

When("add {string} as shipping line code", (code) => {
    ShippingLineActions.FillCode(code);
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    MaintenanceActions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion

//#region Add Shipping Line SCAC Code with lenght more than 4
When("add {string} as shipping line SCAC code", (SCACCode) => {
    ShippingLineActions.FillSCACCode(SCACCode);
});
//#endregion 

//#region Create new shipping line
Given("a shipping line with the following details", (dataTable) => {
    let shippingLineDetails = Assists.CreateInstance<ShippingLineDetails>(dataTable, true);
    ShippingLineActions.FillShippingLineDetails(shippingLineDetails)
});

When("create shipping Line", () => {
    GeneralActions.MockCreate(Urls.ShippingLines)
});

Then("the shipping Line should create successfully", () => {
    GeneralActions.AssertMockCreate()
});
//#endregion

//#region Search for the shipping line by code
When("search for {string} shipping Line", (shippingLineCode) => {
    GeneralActions.Search(shippingLineCode)
});

Then("the {string} shipping Line should appear successfully", (shippingLineCode) => {
    GeneralActions.AssertSearch(shippingLineCode)
});
//#endregion

//#region Open the shipping line
When("open shipping Line", () => {
    ShippingLineActions.OpenShippingLine();
});

Then("the shipping Line should open successfully", () => {
    ShippingLineActions.AssertOpenShippingLine();
});
//#endregion

//#region Check if the fields in INTTRA Tab are dim
Given("navigate INTTRA Tab", () => {
    cy.Navigate(ShippingLineSelectors.INTTRATab);
});

Then("the INTTRA input fields should be dim", () => {
    ShippingLineActions.CheckShippingLineINTTRA()
});
//#endregion

//#region Create shipping line address
Given("fill the following Address in Addresses Shipping line", (dataTable) => {
    let addressDetails = Assists.CreateInstance<AddressDetails>(dataTable, true);
    cy.Navigate(ShippingLineSelectors.AddressesTab);
    GeneralActions.NavigateAddressWizard()
    GeneralActions.FillAddressDetails(addressDetails)
});

When("create shipping line address", () => {
    GeneralActions.CreateAddress()
});

Then("the shipping line address should create successfully", () => {
    GeneralActions.AssertCreateAddress()
});
//#endregion

//#region Add Shipping Line Area Country Port
Given("add {string} as country port area in Areas Tab", (countryPortName) => {
    cy.Navigate(ShippingLineSelectors.AreasTab);
    cy.Navigate(ShippingLineSelectors.AddArea)
    cy.Navigate(ShippingLineSelectors.ChooseCountryPortButton)
    ShippingLineActions.FillAreaCountryPortName(countryPortName)
});

When("add country port area", () => {
    ShippingLineActions.AddAreaCountryPort()
});

Then("the country port area should add successfully", () => {
    ShippingLineActions.AssertAddAreaCountryPort()
});
//#endregion

//#region Add Shipping Line Area Port
Given("add {string} as port area in Areas Tab", (portName) => {
    cy.Navigate(ShippingLineSelectors.ChoosePortButton)
    ShippingLineActions.FillAreaPortName(portName)
});

When("add port area", () => {
    ShippingLineActions.AddAreaPort()
});

Then("the port area should add successfully", () => {
    ShippingLineActions.AssertAddAreaPort()
});
//#endregion

//#region Add Shipping Line Area
Given("fill the following Area details in Areas Tab", (dataTable) => {
    let shippingLineDetails = Assists.CreateInstance<ShippingLineDetails>(dataTable, true);
    ShippingLineActions.FillShippingLineAreaDetails(shippingLineDetails)
});

When("create shipping line area", () => {
    ShippingLineActions.CreateShippingLineArea()
});

Then("the shipping line area should create successfully", () => {
    ShippingLineActions.AssertCreateShippingLineArea()
});
//#endregion

//#region Add Shipping Line Tariff partner code with lenght more than 50
Given("the user navigate shipping line tariff wizerd", () => {
    cy.Navigate(ShippingLineSelectors.TariffTranslationsTab)
    cy.Navigate(ShippingLineSelectors.AddTranslation)
});

When("add {string} as shipping line tariff partner code", (partnerCode) => {
    ShippingLineActions.FillTariffPartnerCode(partnerCode);
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    MaintenanceActions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion

//#region Add Shipping Line Tariff
Given("fill the following Tariff Translation details", (dataTable) => {
    let shippingLineDetails = Assists.CreateInstance<ShippingLineDetails>(dataTable, true);
    ShippingLineActions.FillShippingLineTariffTranslations(shippingLineDetails)
});

When("create shipping line Tariff translation", () => {
    ShippingLineActions.CreateShippingLineTariffTranslations()
});

Then("the shipping line Tariff translation should create successfully", () => {
    ShippingLineActions.AssertCreateShippingLineTariffTranslations()
});
//#endregion

//#region Inactivate shipping line
Given("the user Inactivate shipping line", () => {
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
    BaseActions.ValidateEventsTab(eventDetailsList, ShippingLineSelectors.EventsTab);
});
//#endregion

//#region save and close shipping line 
When("save and close shipping line", () => {
    ShippingLineActions.CloseSaveShippingLine();
});

Then("the shipping line should close successfully", () => {
    ShippingLineActions.AssertCloseSaveShippingLine();
});
//#endregion