import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { URLs } from '../../constants/URLs';
import { LoginDetails } from 'cypress/models/LoginDetails';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as CommonSelectors from '../../../../Common/cypress/selectors/Selectors';
import * as Actions from "../../actions/Actions";
import * as BaseAssertion from '../../../../Base/cypress/actions/Assertion';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import { RestAPI } from '../../../../Base/cypress/constants/RestAPI'
let loginDetails: LoginDetails
//#region Login using wrong email/Login using wrong password
Given("the user in the Login page", () => {
cy.RedirectToLogin()
});

Given("login information with the following details", (dataTable) => {
    loginDetails = Assists.CreateInstance<LoginDetails>(dataTable, true);
    Actions.FillLoginDetails(loginDetails);
});

When("login", () => {
    Actions.Login();
});

Then("the login should fail", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.Authentication, 200).then((interception) => {
        assert.equal(interception.response.body.InValidMailOrPassword, true)
    })
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    Actions.ValidateErrorMessage(ValidationMessage)
});

//#endregion

//#region Send a password reset request
Given("the user in the Password Reset Request Page", () => {
    Actions.VisitPasswordResetRequestPage()
});
Given("{string} as email", (Email) => {
Actions.FillEmail(Email);
});

When("send a password reset request", () => {
Actions.SendPasswordResetRequest();
});

Then("the request should send successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ResetPassword,200)  
});
Then("the following messages should appear", (dataTable) => {
    Actions.ValidateMessagesForSendingPasswordResetRequest(dataTable)
}); 
//#endregion