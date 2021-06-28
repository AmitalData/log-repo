import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import * as BranchActions from "../../actions/BranchActions";
import * as Actions from "../../actions/Actions";
import { BranchSelectors } from "../../selectors/BranchSelectors";
import * as MaintenanceActions from "../../actions/Actions";
import { BranchDetails } from "../../models/BranchDetails";
import { AddressDetails } from "../../models/AddressDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { Constants } from "../../constants/Constants";
import * as GeneralActions from "../../actions/BaseActions";


//#region Add Branch code with lenght more than 10
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemBranch)
});

When("add {string} as branch code", (branchCode) => {
    MaintenanceActions.OpenNewWizard(Constants.Branch);
    BranchActions.FillBranchCode(branchCode)
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    Actions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion

//#region Add Branch counter code with lenght more than 5
When("add {string} as branch counter code", (counterCode) => {
    BranchActions.FillBranchCounterCode(counterCode)
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    Actions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion

//#region Create new branch
Given("a branch with the following details", (dataTable) => {
    let branchDetails = Assists.CreateInstance<BranchDetails>(dataTable, true);
    BranchActions.FillBranchDetails(branchDetails)
});

When("create branch", () => {
    BranchActions.CreateBranch();
});

Then("the branch should create successfully", () => {
    BranchActions.AssertCreateBranch();
});
//#endregion


//#region Search for the branch by name
When("search branch", () => {
    BranchActions.SearchBranch()
});

Then("the branch should appear successfully", () => {
    BranchActions.AssertSearchBranch()
});
//#endregion


//#region Open the branch
When("open branch", () => {
    BranchActions.OpenBranch();
});

Then("the branch should open successfully", () => {
    BranchActions.AssertOpenBranch();
});
//#endregion

//#region Edit the Branch by adding address
Given("the user create address with the following details", (dataTable) => {
    GeneralActions.NavigateAddressWizard()
    let addressDetails = Assists.CreateInstance<AddressDetails>(dataTable, true);
    GeneralActions.FillAddressDetails(addressDetails)
});

When("create address", () => {
    BranchActions.CreateAddress();
});

Then("the address should create successfully", () => {
    BranchActions.AssertCreateAddress();
});
//#endregion

//#region Edit the Branch
Given("add {string} to External ID in Accounting Tab", (accountingExternalID) => {
    cy.Navigate(BranchSelectors.AccountingTab);
    BranchActions.FillAccountingExternalID(accountingExternalID)
});

Given("the user Inactivate the Branch", () => {
    cy.Navigate(BranchSelectors.GeneralTab);
    BranchActions.CheckInactiveBox()
});

When("save branch", () => {
    BranchActions.EditBranch();
});

Then("the branch should update successfully", () => {
    BranchActions.AssertEditBranch();
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, BranchSelectors.EventsTab);
});
//#endregion

//#region Save and close the branch
When("save and close branch", () => {
    BranchActions.CloseSaveBranch();
});

Then("the branch should close successfully", () => {
    BranchActions.AssertCloseSaveBranch();
});
//#endregion