import * as MaintenanceActions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import { ChangePasswordsDetails } from '../../models/ChangePasswordsDetails'
import { PasswordValidationMessagesDetails } from '../../models/PasswordValidationMessagesDetails'
import * as Assists from "../../../../Base/cypress/assists/Assists";
//#region variables
let changePasswordsDetails: ChangePasswordsDetails
let passwordValidationMessagesDetails: PasswordValidationMessagesDetails
//#endregion
//#region  scenarios
Given("the user logged in and navigates to change password window from maintenance menu", () => {
    cy.Login();
    MaintenanceActions.OpenChangeUserPasswordWindow()
});

Given("the user in change password window", () => {
BaseAssertion.AssertElementContain(BaseSelectors.WindowHeader,"Change User Password")
});

When("change user password with the following details", (dataTable) => {
    changePasswordsDetails = Assists.CreateInstance<ChangePasswordsDetails>(dataTable, true);
    MaintenanceActions.FillChangePasswordWindow(changePasswordsDetails);
});

When("change password", () => {
    MaintenanceActions.ChangePassword()
});

Then("the validation message with {string} message should appear", (message) => {
    BaseAssertion.AssertElementContain(BaseSelectors.SingleError,message)
});

Then("these validation should be with the following colors", (dataTable) => {
    passwordValidationMessagesDetails = Assists.CreateInstance<PasswordValidationMessagesDetails>(dataTable, true);
    MaintenanceActions.ValidatePasswordValidationMessagesColors(passwordValidationMessagesDetails);
});
//#endregion

