
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../../../../Base/cypress/assists/Assists";
import * as SharedLogisticsActions from "../../../actions/SharedLogisticsActions/SharedLogisticsActions";
import * as Actions from "../../../actions/Actions";
import { ContactDetails } from "../../../models/ContactDetails";
import { BaseSelectors } from "../../../../../Base/cypress/selectors/BaseSelectors";
import { MaintenanceSelectors } from "../../../selectors/Selectors";
//#region variables
let contactDetails: ContactDetails
let NewContactEmail: string
//#endregion


Given("the user logged in", () => {
    cy.Login()
});
Given("the user navigates to Shared Logistics", () => {
    SharedLogisticsActions.ChooseSharedLogistics()
});

Given("insert new contact to the fisrt customer from Invite customers tab", (dataTable) => {
    contactDetails = Assists.CreateInstance<ContactDetails>(dataTable, true);
    SharedLogisticsActions.GetFirstCustomer()
    Actions.FillContactDetails(contactDetails);
    GetNewContactEmail()
});
When("the user Add the new contact", () => {
    SharedLogisticsActions.CreateContact();
});
Then("the contact should created successfully", () => {
    SharedLogisticsActions.AssertPostContact();
});
When("the user invite the added contact", () => {
    SharedLogisticsActions.InviteAddedContact(NewContactEmail)
});
Then("the contact should invited successfully", () => {
    SharedLogisticsActions.AssertPutContactInvitation()
});
Then("the following confirmation messeage {string}+{string} appears", (part1, part2) => {
    SharedLogisticsActions.ConfirmInviting(contactDetails.EnglishName, part1, part2)
});

Then("the customer is now in the invited customers list", () => {
    SharedLogisticsActions.AssertcustomerInInvitedList();
});


Given("the user get the first Customer in the Not Invited List", () => {
    SharedLogisticsActions.GetFirstNotInvited()
});
When("search for it in Not Invited list", () => {
    SharedLogisticsActions.SearchInvitedNotCustomers()
});
Then("the search result should show the customer", () => {
    SharedLogisticsActions.AssertCustomerExist()
});


Given("the user get the first Customer in the invited customers list", () => {
    SharedLogisticsActions.GetFirstInvited()
});

When("search for it in Invited list", () => {
    SharedLogisticsActions.SearchInvitedCustomers()
});
Then("the search result shouldn't show the customer", () => {
    SharedLogisticsActions.AssertSearchIsNull()
});



function GetNewContactEmail() {
    cy.get('input' + MaintenanceSelectors.ContactEmail).invoke('val').then((Lable) => {
        cy.log(Lable.toString())
        NewContactEmail = (Lable.toString())
    })
}