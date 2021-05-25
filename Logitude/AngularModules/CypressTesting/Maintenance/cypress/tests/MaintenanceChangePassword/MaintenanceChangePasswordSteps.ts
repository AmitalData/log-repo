import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import { ChangePasswordsDetails } from '../../models/ChangePasswordsDetails'
import * as Assists from "../../../../Base/cypress/assists/Assists";
let changePasswordsDetails: ChangePasswordsDetails
//#region Change Password
Given("the user logged in and navigates to change password window from maintenance menu", () => {
  cy.Login();
  Actions.OpenChangeUserPasswordWindow()
});

Given("change user password with the following details", (dataTable) => {
  changePasswordsDetails = Assists.CreateInstance<ChangePasswordsDetails>(dataTable, true);
  Actions.FillChangePasswordWindow(changePasswordsDetails);
});

When("change password", () => {
  Actions.ChangePasswordMockChange();
});

Then("the password should change successfully", () => {
  BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow)
});
//#endregion