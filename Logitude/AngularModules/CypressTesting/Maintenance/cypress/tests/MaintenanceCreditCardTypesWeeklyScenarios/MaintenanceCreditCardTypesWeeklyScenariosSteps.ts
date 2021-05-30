import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import { CreditCardTypeDetails } from "../../../cypress/models/CreditCardTypeDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { Constants } from "../../constants/Constants";

//#region variable
let creditCardTypeDetails: CreditCardTypeDetails
//#endregion

//#region Add Credit Card Type Code with lenght more than 2
Given("the user logged in and navigate to {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemCreditCardTypes)
});

When("add {string} as credit card type code", (CreditCardTypeCode) => {
    MaintenanceActions.OpenNewWizard(Constants.CreditCardType);
    MaintenanceActions.FillCreditCardTypeCode(CreditCardTypeCode);
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    MaintenanceActions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion
//#region  Create a new credit card type
Given("a credit card type with the following details", (dataTable) => {
    creditCardTypeDetails = Assists.CreateInstance<CreditCardTypeDetails>(dataTable, true);
    MaintenanceActions.FillCreditCardTypeDetails(creditCardTypeDetails) 
});
 
When("create credit card type", () => {
    MaintenanceActions.CreateCreditCardType();
});
 
Then("the credit card type should create successfully", () => {
    MaintenanceActions.AssertCreateCreditCardType();
});
 
//#endregion
//#region Search for the credit card type by code
When("search for credit card type", () => {
    MaintenanceActions.SearchCreditCardType()
});

Then("the credit card type should appear successfully", () => {
    MaintenanceActions.AssertSearchCreditCardTypeByFilter(creditCardTypeDetails.EnglishName)
});

//#endregion
//#region Open the credit card type
When("open credit card type", () => {
    MaintenanceActions.OpenCreditCardType()
});

Then("the credit card type should open successfully", () => {
    MaintenanceActions.AssertOpenCreditCardType()
});
//#endregion
//#region  Edit the credit card type
Given("{string} as credit card type name", (name) => {
    MaintenanceActions.FilllCreditCardTypeName(name)
});
 
 
When("update credit card type", () => {
    MaintenanceActions.UpdateCreditCardType()
});
 
Then("the credit card type should update successfully", () => {
    MaintenanceActions.AssertUpdateCreditCardType()
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, MaintenanceSelectors.CreditCardTypeEventTab);
});
//#endregion