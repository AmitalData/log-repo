import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as PaymentBlockingActions from '../../actions/PaymentBlockingActions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { PaymentBlockingDetails } from '../../models/PaymentBlockingDetails';

//#region Payment Blocking on Declaration Changes

Given("the user logged in and navigates to Import workspace", () => {
    cy.Login();
    PaymentBlockingActions.NavigateToImportDeclarations();
});

Given("search for file number {string}", (fileNumber: string) => {
    PaymentBlockingActions.SearchByFileNumber(fileNumber);
});

Given("enter the file", () => {
    PaymentBlockingActions.EnterFile();
});

Given("navigate to {string} tab in declaration", (tabName: string) => {
    PaymentBlockingActions.NavigateToTab(tabName);
});

When("click {string} button", (buttonName: string) => {
    PaymentBlockingActions.ClickButton(buttonName);
});

Then("payment submission screen should open", () => {
    PaymentBlockingActions.VerifyPaymentScreenOpened();
});

Given("log all available buttons on the page", () => {
    PaymentBlockingActions.LogAllButtons();
});

When("try to find payment submission button", () => {
    PaymentBlockingActions.TryFindPaymentButton();
});

Then("payment submission functionality should be verified", () => {
    PaymentBlockingActions.VerifyPaymentFunctionality();
});

//#endregion
