import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { Selectors } from "../../selectors/Selectors"
import * as Actions from "../../actions/Actions";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"

Given("{string} as a paswword without confirm password",(password)=>{
    cy.OpenChangePasswordPage();
    Actions.FillChangePasswordPage(password,password,null);
});

Given("{string} as a paswword and confirm password",(password)=>{
    cy.OpenChangePasswordPage();
    Actions.FillChangePasswordPage(password,password,password);
});

Given("{string} as a current ,paswword and confirm password",(password)=>{
    cy.OpenChangePasswordPage();
    Actions.FillChangePasswordPage(password,password,password);
});

Given("{string} as a new paswword and confirm password",(password)=>{
    cy.OpenChangePasswordPage();
    Actions.FillChangePasswordPage(password,password,password);
    cy.intercept(
        {
          method: 'POST',     
          url: '**/PostChangePassword/**',     
        },[true] 
      ) 
});

When("sumbit",()=>{
    cy.Click(Selectors.ConfirmPassword, null);
});

Then("validate message should appear successfully",()=>{
    cy.ValidateElementColor(Selectors.ErrorList,"rgb(255, 0, 0)"); //assertion for red color

});

Then("password should reset successfully",()=>{
     cy.RedirectToLogin();
});

