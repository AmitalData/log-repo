import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import * as BranchActions from "../../actions/BranchActions";
import { BranchSelectors } from "../../../cypress/selectors/BranchSelectors";
import * as MaintenanceActions from "../../actions/Actions";
import { BranchDetails } from "../../../cypress/models/BranchDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { Constants } from "../../constants/Constants";


let branchDetails:BranchDetails;


//#region Add Branch
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemBranch)
});

Given("a branch with the following details", (dataTable) => {
    branchDetails = Assists.CreateInstance<BranchDetails>(dataTable, true);
    MaintenanceActions.OpenNewWizard(Constants.Branch);
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
 
//#region Edit the Branch
Given("a {string} as branchLocalName", (branchLocalName) => {
    BranchActions.FillBranchLocalName(branchLocalName)
});
 
Given("the user activate branch", () => {
    MaintenanceActions.ChangeInactiveCheckBoxValue(BranchSelectors.InActiveBranchCheckBox)
});
 
When("edit branch", () => {
    BranchActions.EditBranch(); 
});
 
Then("the branch should update successfully", () => {
    BranchActions.AssertEditBranch();  
});
 
Then("following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, BranchSelectors.BranchEventsTab);
});
 
//#endregion