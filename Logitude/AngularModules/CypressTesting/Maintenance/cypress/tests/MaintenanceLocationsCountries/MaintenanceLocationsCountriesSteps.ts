import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import { CountryDetails } from "../../../cypress/models/CountryDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"

//#region variable
let countryDetails:CountryDetails
//#endregion
//#region Add Country
Given("the user logged in and navigate to {string} in maintenance menu", (maintenanceItemName) => {
cy.Login();
MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemCountry)

});
 
Given("a country with the following details", (dataTable) => {
    countryDetails = Assists.CreateInstance<CountryDetails>(dataTable, true);
    MaintenanceActions.OpenNewWizard("Country");
    MaintenanceActions.FillCountryDetails(countryDetails)
});
 
When("add country", () => {
MaintenanceActions.CreateCountry();
});
 
Then("the country should add successfully", () => {
    MaintenanceActions.AssertCreateCountry(); 
});
 
//#endregion
 
//#region Search for the Country by name
When("search for {string} country", (country) => {
    MaintenanceActions.SearchCountry(country)
});
 
Then("the {string} country should appear successfully", (country) => {
    MaintenanceActions.AssertSearchCountry(country);
});
 
//#endregion
 
//#region Open the country
When("open country", () => {
    MaintenanceActions.OpenCountry();
});
 
Then("the country should open successfully", () => {
    MaintenanceActions.AssertOpenCountry() 
});
 
//#endregion
 
//#region Edit the country
Given("the user change InactiveCountry check box", () => {
    MaintenanceActions.ChangeInactiveCountryCheckBoxValue()
});
 
When("edit country", () => {
    MaintenanceActions.EditCountry();
});
 
Then("the country should update successfully", () => {
    MaintenanceActions.AssertPutCountry();
});
Then("following event should appear in events tab", (dataTable) => {
let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
eventDetailsList=MaintenanceActions.CountryConversionEventsMapping(eventDetailsList)
BaseActions.ValidateEventsTab(eventDetailsList,"#CountryTHEvents");
})
//#endregion