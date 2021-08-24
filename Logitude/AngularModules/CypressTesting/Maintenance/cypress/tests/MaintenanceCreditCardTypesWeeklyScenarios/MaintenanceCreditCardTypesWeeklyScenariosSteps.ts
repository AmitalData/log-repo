import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import { CreditCardTypeDetails } from "../../../cypress/models/CreditCardTypeDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { Constants } from "../../constants/Constants";
import * as GeneralActions from "../../actions/BaseActions";

//#region variable
let creditCardTypeDetails: CreditCardTypeDetails
let creditCardTypeSearchValue = null
//#endregion

//#region Assert create trucker without code
Given("the user logged in and navigate to {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemCreditCardTypes)
});

Given("the user fill the required fields except the code", () => {
    MaintenanceActions.OpenNewWizard(Constants.CreditCardType);
    MaintenanceActions.FilllCreditCardTypeName("new name")
});

When("create credit card type", () => {
    MaintenanceActions.CreateCreditCardType();
});

Then("a validation single message with {string} error should appear", (validationMessage) => {
    GeneralActions.ValidateSingleErrorMessage(validationMessage)
});
//#endregion

//#region Add Credit Card Type Code with lenght more than 2
When("add {string} as credit card type code", (CreditCardTypeCode) => {
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

Then("the credit card type should create successfully", () => {
    MaintenanceActions.AssertCreateCreditCardType();
});
//#endregion

//#region Search for the credit card type by code
When("search for credit card type", () => {
    creditCardTypeSearchValue = MaintenanceActions.getCreditCardTypeSearchValue()
    GeneralActions.Search(creditCardTypeSearchValue)
});

Then("the credit card type should appear successfully", () => {
    GeneralActions.AssertSearch(creditCardTypeSearchValue)
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