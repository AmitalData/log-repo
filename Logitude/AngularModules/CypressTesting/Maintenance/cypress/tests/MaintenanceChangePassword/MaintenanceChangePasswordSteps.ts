import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as BaseAssertion from  "../../../../Base/cypress/actions/Assertion"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
Given("the user logged in and navigates to change password window from maintenance menu", () => {
  cy.Login();
  Actions.OpenChangeUserPasswordWindow()
});

Given("{string} as a current password and {string} as a new paswword", (CurrentPassword,NewPassword) => {
    Actions.FillChangePasswordWindow(CurrentPassword,NewPassword,NewPassword); 
});

When("change password",()=>{
    Actions.ChangePasswordMockChange();
});

Then("the password should change successfully",()=>{
  BaseAssertion.AssertElementNotExist(BaseSelectors.LogitudeWindow)
});