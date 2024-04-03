import * as Actions from "../../actions/Actions";
import * as PaymentTermActions from "../../actions/PaymentTermActions";
import * as GeneralActions from "../../actions/BaseActions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { PaymentTermsSelectors } from "../../selectors/PaymentTermsSelectors";
import { PaymentTermDetails } from "cypress/models/PaymentTermDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";

//#region Add payment term code with lenght more than 2
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, PaymentTermsSelectors.MaintenanceItem)
});

When("add {string} as payment term code", (paymentTermCode) => {
    Actions.OpenNewWizard("PaymentTerm")
    PaymentTermActions.FillCode(paymentTermCode)
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    Actions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion

//#region add payment term code already exist
When("add another payment term code: {string}", (paymentTermCode) => {
    PaymentTermActions.FillRequiredData(paymentTermCode)
    PaymentTermActions.CreatePaymentTerm()
});

Then("this validation message error {string} should appear", (validationMessage) => {
    GeneralActions.ValidateSingleErrorMessage(validationMessage)
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsCancel);
});
//#endregion

//#region Create new payment term
Given("a payment term with the following details", (dataTable) => {
    cy.wait(1000)
    let paymentTermDetails = Assists.CreateInstance<PaymentTermDetails>(dataTable, true);
    PaymentTermActions.FillPaymentTermDetails(paymentTermDetails, 4);
});

When("create payment term", () => {
    PaymentTermActions.CreatePaymentTerm();
});

Then("the payment term should create successfully", () => {
    PaymentTermActions.AssertCreatePaymentTerm();
});
//#endregion

//#region Search for the payment term
When("search payment term", () => {
    PaymentTermActions.SearchPaymentTerm()
});

Then("the payment term should appear successfully", () => {
    PaymentTermActions.AssertSearchPaymentTerm();
});
//#endregion

//#region Open the payment term
When("open payment term", () => {
    PaymentTermActions.OpenPaymentTerm();
});

Then("the payment term should open successfully", () => {
    PaymentTermActions.AssertOpenPaymentTerm();
});
//#endregion

//#region Edit the payment term
Given("the user edit the following payment term details", (dataTable) => {
    let paymentTermDetails = Assists.CreateInstance<PaymentTermDetails>(dataTable, true);
    PaymentTermActions.EditPaymentTermGeneralTab(paymentTermDetails)
});

Given("fill the following payment term Accounting External ID", (dataTable) => {
    let paymentTermDetails = Assists.CreateInstance<PaymentTermDetails>(dataTable, true);
    cy.Navigate(PaymentTermsSelectors.AccountingTab);
    PaymentTermActions.FillPaymentTermAccountingTab(paymentTermDetails)
});

When("save payment term", () => {
    PaymentTermActions.UpdatePaymentTerm()
});

Then("the payment term should update successfully", () => {
    PaymentTermActions.AssertUpdatePaymentTerm()
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, PaymentTermsSelectors.EventsTab);
});
//#endregion

//#region Save and close the payment term
When("save and close payment term", () => {
    PaymentTermActions.CloseSavePaymentTerm();
});

Then("the payment term should close successfully", () => {
    PaymentTermActions.AssertCloseSavePaymentTerm();
});
 //#endregion