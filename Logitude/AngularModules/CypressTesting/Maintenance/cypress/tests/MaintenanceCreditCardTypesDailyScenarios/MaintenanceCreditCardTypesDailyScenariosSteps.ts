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
    MaintenanceActions.CreateCreditCardTypeMockCreate();
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
    MaintenanceActions.AssertCreateCreditCardTypeMockCreate();
});
//#endregion

//#region Search for the credit card type by name
When("search for {string} credit card type", (CreditCardTypeName) => {
    GeneralActions.Search(CreditCardTypeName)
});

Then("the {string} credit card type should appear successfully", (CreditCardTypeName) => {
    GeneralActions.AssertSearch(CreditCardTypeName)
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
Given("the user inactivate the credit card type", () => {
    MaintenanceActions.ChangeInactiveCheckBoxValue(MaintenanceSelectors.InActiveCreditCardTypeCheckBox)
});

When("update credit card type", () => {
    MaintenanceActions.UpdateCreditCardType()
});

Then("the credit card type should update successfully", () => {
    MaintenanceActions.AssertUpdateCreditCardType()
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    eventDetailsList = MaintenanceActions.CreditCardTypeConversionEventsMapping(eventDetailsList)
    BaseActions.ValidateEventsTab(eventDetailsList, MaintenanceSelectors.CreditCardTypeEventTab);
});
//#endregion