import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import { CityDetails } from "../../../cypress/models/CityDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { Constants } from "../../constants/Constants";

//#region variable
let cityDetails: CityDetails
//#endregion

//#region Add CityCode with lenght more than 10
Given("the user logged in and navigate to {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemCities)
});

When("add {string} as city code", (cityCode) => {
    MaintenanceActions.OpenNewWizard(Constants.CountryCity);
    cy.FillLogTextBox(MaintenanceSelectors.CityCode, cityCode);
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    MaintenanceActions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion

//#region Add City
Given("a city with the following details", (dataTable) => {
    cityDetails = Assists.CreateInstance<CityDetails>(dataTable, true);
    MaintenanceActions.FillCityDetails(cityDetails)
});

When("add city", () => {
    MaintenanceActions.CreateCity();
});

Then("the city should add successfully", () => {
    MaintenanceActions.AssertCreateCity();
});
//#endregion

//#region Search for the City by name
When("search for city", () => {
    MaintenanceActions.SearchCity()
});

Then("the city should appear successfully", () => {
    MaintenanceActions.AssertSearchCity();
});
//#endregion

//#region Open the City
When("open city", () => {
    MaintenanceActions.OpenCity();
});

Then("the city should open successfully", () => {
    MaintenanceActions.AssertOpenCity()
});
//#endregion

//#region Edit the City
Given("a {string} as cityLocalName", (NewStateLocalName) => {
    MaintenanceActions.FillCityLocalName(NewStateLocalName)
});

Given("the user inactivate the city", () => {
    MaintenanceActions.ChangeInactiveCheckBoxValue(MaintenanceSelectors.InActiveCityCheckBox)
});

When("edit city", () => {
    MaintenanceActions.EditCity();
});

Then("the city should update successfully", () => {
    MaintenanceActions.AssertPutCity();
});

Then("following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, MaintenanceSelectors.CityEventTab);
});
//#endregion