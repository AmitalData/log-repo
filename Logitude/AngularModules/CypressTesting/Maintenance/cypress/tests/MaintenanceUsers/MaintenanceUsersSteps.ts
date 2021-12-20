import * as Actions from "../../actions/Actions";
import * as UserActions from "../../actions/UserActions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { UserDetails } from "../../models/UserDetails";
import { UsersSelectors } from "../../selectors/UsersSelectors";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";

//#region Create new User
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, UsersSelectors.MaintenanceItem)
});

Given("a User with the following details", (dataTable) => {
    UserActions.NavigateUserWizerd()
    let userDetails = Assists.CreateInstance<UserDetails>(dataTable, true);
    UserActions.FillUserDetails(userDetails);
});

When("create User", () => {
    UserActions.CreateUser();
});

Then("the User should create successfully", () => {
    UserActions.AssertCreateUser();
});
//#endregion

//#region Search for the User by Email
When("search User", () => {
    UserActions.SearchUser()
});

Then("the User should appear successfully", () => {
    UserActions.AssertSearchUser();
});
//#endregion

//#region Open the User
When("open the User", () => {
    UserActions.OpenUser()
});

Then("the User should open successfully", () => {
    UserActions.AssertOpenUser()
});
//#endregion

//#region Edit the User
Given("a {string} as note", (notes) => {
    UserActions.FillNotes(notes)
});

When("save the User", () => {
    UserActions.UpdateUser()
});

Then("the User should update successfully", () => {
    UserActions.AssertUpdateUser()
});
//#endregion

//#region Inactivate the User
Given("inactive the user", () => {
    cy.ClickCheckBox(UsersSelectors.InActiveCheckBox)
});

Then("the following event should appear in events tab", (dataTable) => {
    cy.Click(UsersSelectors.EventsTab, null)
    cy.Click(BaseSelectors.RefreshImg + BaseSelectors.LastElement, null, true);
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, UsersSelectors.EventsTab);
});
//#endregion

//#region Log in with inactive user
Given("the user logged in with the inactive user", () => {
    UserActions.Login("123")
});

Given("change old password {string} to new password {string}", (oldPassword, newPassword) => {
    UserActions.ResetPassword(oldPassword, newPassword)
});

Given("re login", () => {
    cy.wait(5000)
    cy.FillLogTextBox(UsersSelectors.Password, "!Cypress1")
    cy.Click(UsersSelectors.cmdLogin, null)
});

Then("an error message with {string} should appear", (messageError) => {
    BaseAssertion.AssertElementContain(BaseSelectors.ErrorsList, messageError)
});
//#endregion