import * as Actions from "../../actions/Actions";
import * as GeneralActions from "../../actions/BaseActions";
import * as paymentTermActions from "../../actions/PaymentTermActions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { PaymentTermsSelectors } from "../../selectors/PaymentTermsSelectors";
import { PaymentTermDetails } from "cypress/models/PaymentTermDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import { Urls } from "../../constants/Urls";

//#region Add payment term code with lenght more than 2
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, PaymentTermsSelectors.MaintenanceItem)
});

When("add {string} as payment term code", (paymentTermCode) => {
    Actions.OpenNewWizard("PaymentTerm")
    paymentTermActions.FillCode(paymentTermCode)
});

Then("a validation message with {string} error should appear", (validationMessage) => {
    Actions.ValidateErrorPopUpMessage(validationMessage)
});
//#endregion

//#region add payment term code already exist
When("add another payment term code: {string}", (paymentTermCode) => {
    paymentTermActions.FillRequiredData(paymentTermCode)
    paymentTermActions.CreatePaymentTerm()
});

Then("this validation message error {string} should appear", (validationMessage) => {
    GeneralActions.ValidateSingleErrorMessage(validationMessage)
});
//#endregion

//#region Create new payment term
Given("a payment term with the following details", (dataTable) => {
    let paymentTermDetails = Assists.CreateInstance<PaymentTermDetails>(dataTable, true);
    paymentTermActions.FillPaymentTermDetails(paymentTermDetails, 4);
});

When("create payment term", () => {
    GeneralActions.MockCreate(Urls.PackageTypes)
});

Then("the payment term should create successfully", () => {
    GeneralActions.AssertMockCreate()
});
//#endregion

//#region Search for the payment term
When("search for {string} payment term", (searchFieldValue) => {
    GeneralActions.Search(searchFieldValue)
});

Then("the {string} payment term should appear successfully", (searchFieldValue) => {
    GeneralActions.AssertSearch(searchFieldValue)
});
//#endregion

//#region Open the payment term
When("open payment term", () => {
    paymentTermActions.OpenPaymentTerm();
});

Then("the payment term should open successfully", () => {
    paymentTermActions.AssertOpenPaymentTerm();
});
//#endregion

//#region Edit the payment term
Given("the user edit the following payment term details", (dataTable) => {
    let paymentTermDetails = Assists.CreateInstance<PaymentTermDetails>(dataTable, true);
    paymentTermActions.EditPaymentTermGeneralTab(paymentTermDetails)
});

Given("fill the following payment term Accounting External ID", (dataTable) => {
    let paymentTermDetails = Assists.CreateInstance<PaymentTermDetails>(dataTable, true);
    cy.Navigate(PaymentTermsSelectors.AccountingTab);
    paymentTermActions.FillPaymentTermAccountingTab(paymentTermDetails)
});

When("save payment term", () => {
    paymentTermActions.UpdatePaymentTerm()
});

Then("the payment term should update successfully", () => {
    paymentTermActions.AssertUpdatePaymentTerm()
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, PaymentTermsSelectors.EventsTab);
});
//#endregion

//#region Save and close the payment term
When("save and close payment term", () => {
    paymentTermActions.CloseSavePaymentTerm();
});

Then("the payment term should close successfully", () => {
    paymentTermActions.AssertCloseSavePaymentTerm();
});
 //#endregion