import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as AirLineActions from "../../actions/AirLineActions";
import { AirLineSelectors } from "../../selectors/AirLineSelectors";
import * as Actions from "../../actions/Actions";
import { AirLineDetails } from "../../models/AirLineDetails";
import { AddressDetails } from "../../models/AddressDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import * as GeneralActions from "../../actions/BaseActions";

//#region import new air line
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, AirLineSelectors.MaintenanceItem)
});

When("fake import air line", () => {
    cy.Navigate(AirLineSelectors.ImportAirLine_AddButton)
    GeneralActions.MockImport();
});

Then("the air line should import successfully", () => {
    GeneralActions.AssertMockImport();
});
//#endregion

//#region Add air Line Code with lenght more than 2
Given("the user navigate air line Wizard", () => {
    cy.Navigate(AirLineSelectors.AddNewAirLine)
});

When("add {string} as air line code", (code) => {
    AirLineActions.FillCode(code);
});

Then("a validation message with {string} error should appear", (validationMessage) => {
    Actions.ValidateErrorPopUpMessage(validationMessage)
});
//#endregion

//#region Add air Line ICAO with lenght 2
When("add {string} as air line ICAO", (ICAO) => {
    AirLineActions.FillICAO(ICAO);
});
//#endregion

//#region Add air Line ICAO already existes
When("add {string} as airline ICAO", (ICAO) => {
    AirLineActions.FillICAO(ICAO);
    AirLineActions.PressOnWindowHeader()
});

Then("a validation single error message with {string} should appear", (validationMessage) => {
    GeneralActions.ValidateSingleErrorMessage(validationMessage)
});
//#endregion

//#region Add Airline Prefix more than 3
When("add {string} as airline prefix", (prefix) => {
    AirLineActions.FillPrefix(prefix);
});
//#endregion

//#region Create new shipping line
Given("an air line with the following details", (dataTable) => {
    let airLineDetails = Assists.CreateInstance<AirLineDetails>(dataTable, true);
    AirLineActions.FillPrefix(airLineDetails.Prefix);
    AirLineActions.FillAirLineDetails(airLineDetails)
});

When("create air line", () => {
    AirLineActions.CreateAirLine();
});

Then("the air line should create successfully", () => {
    AirLineActions.AssertCreateAirLine();
});
//#endregion

//#region Search for the shipping line by code
When("search the air line", () => {
    AirLineActions.Search()
});

Then("the air line should appear successfully", () => {
    AirLineActions.AssertSearch()
});
//#endregion

//#region Open the shipping line
When("open air line", () => {
    AirLineActions.OpenAirLine();
});

Then("the air line should open successfully", () => {
    AirLineActions.AssertOpenAirLine();
});
//#endregion

//#region Create shipping line address
Given("fill the following Address details in Addresses air line tab", (dataTable) => {
    let addressDetails = Assists.CreateInstance<AddressDetails>(dataTable, true);
    cy.Navigate(AirLineSelectors.AddressesTab);
    GeneralActions.NavigateAddressWizard()
    GeneralActions.FillAddressDetails(addressDetails)
});

When("create air line address", () => {
    GeneralActions.CreateAddress()
});

Then("the air line address should create successfully", () => {
    GeneralActions.AssertCreateAddress()
});
//#endregion

//#region Add air Line Tariff partner code with lenght more than 50
Given("the user navigate air line tariff Wizard", () => {
    cy.Navigate(AirLineSelectors.TariffTranslationsTab)
    cy.Navigate(AirLineSelectors.AddTranslation)
});

When("add {string} as air line tariff partner code", (partnerCode) => {
    AirLineActions.FillTariffPartnerCode(partnerCode);
});
//#endregion

//#region Add air Line Tariff
Given("fill the following tariff translation details", (dataTable) => {
    let shippingLineDetails = Assists.CreateInstance<AirLineDetails>(dataTable, true);
    AirLineActions.FillAirLineTariffTranslations(shippingLineDetails)
});

When("create air line tariff translation", () => {
    AirLineActions.CreateAirLineTariffTranslations()
});

Then("the air line tariff translation should create successfully", () => {
    AirLineActions.AssertCreateAirLineTariffTranslations()
});
//#endregion

//#region Add air Line surcharge tariff
Given("fill the following surcharge tariff details", () => {
    AirLineActions.NavigateSurchargeTariffwizard()
    AirLineActions.FillAirLineSurchargeTariffDetails()
});

Given("add tariff charge", (dataTable) => {
    cy.Navigate(AirLineSelectors.AddTariffCharge)
    let shippingLineDetails = Assists.CreateInstance<AirLineDetails>(dataTable, true);
    AirLineActions.FillAirLineTariffCharge(shippingLineDetails)
});

When("create air line surcharge tariff", () => {
    AirLineActions.CreateAirLineSurchargeTariff()
});

Then("the air line surcharge tariff should create successfully", () => {
    AirLineActions.AssertCreateAirLineSurchargeTariff()
});
//#endregion

//#region Add Special Handling Code from Adaptations tab with lenght more than 4
Given("the user navigate Special Handling Codes Wizard", () => {
    cy.Navigate(AirLineSelectors.AdaptationsTab)
    cy.Navigate(AirLineSelectors.AddSpecialHandlingCode)
});

When("add {string} as Special Handling Code", (code) => {
    AirLineActions.FillSpecialHandlingCode(code);
});

Then("a validation message with {string} error should appear", (validationMessage) => {
    Actions.ValidateErrorPopUpMessage(validationMessage)
});
//#endregion

//#region Add Air Line AWB Special Handling Code
Given("add new special handling code from Adaptions Tab", (dataTable) => {
    let shippingLineDetails = Assists.CreateInstance<AirLineDetails>(dataTable, true);
    AirLineActions.FillSpecialHandlingCodesDetails(shippingLineDetails)
});

When("create air line awb special handling code", () => {
    AirLineActions.CreateSpecialHandlingCodes()
});

Then("the air line awb special handling code should create successfully", () => {
    AirLineActions.AssertCreateSpecialHandlingCodes()
});
//#endregion

//#region Inactivate shipping line
Given("the user Inactivate air line", () => {
    cy.Navigate(AirLineSelectors.GeneralTab);
    Actions.ChangeInactiveCheckBoxValue(AirLineSelectors.InactiveAirline)
});

When("save air line", () => {
    AirLineActions.EditAirLine();
});

Then("the air line should update successfully", () => {
    AirLineActions.AssertEditAirLine();
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, AirLineSelectors.EventsTab);
});
//#endregion

//#region save and close air line 
When("save and close air line", () => {
    AirLineActions.CloseSaveAirLine();
});

Then("the air line should close successfully", () => {
    AirLineActions.AssertCloseSaveAirLine();
});
//#endregion