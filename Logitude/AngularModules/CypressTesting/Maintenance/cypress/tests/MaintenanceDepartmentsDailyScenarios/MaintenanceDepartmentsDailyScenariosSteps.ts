import * as Actions from "../../actions/Actions";
import * as GeneralActions from "../../actions/BaseActions";
import * as DepartmentActions from "../../actions/DepartmentActions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { DepartmentSelectors } from "../../selectors/DepartmentSelectors";
import { DepartmentDetails } from "cypress/models/DepartmentDetails";
import { ValidateEventsTab } from "../../../../Base/cypress/actions/Actions";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import { Urls } from "../../constants/Urls";

//#region Add payment term code with lenght more than 2
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, DepartmentSelectors.MaintenanceItem)
});

When("add {string} as department code", (departmentCode) => {
    Actions.OpenNewWizard("Department")
    DepartmentActions.FillCode(departmentCode)
});

Then("a validation message with {string} error should appear", (validationMessage) => {
    Actions.ValidateErrorPopUpMessage(validationMessage)
});
//#endregion

//#region Create new department
Given("a department with the following details", (dataTable) => {
    let departmentDetails = Assists.CreateInstance<DepartmentDetails>(dataTable, true);
    DepartmentActions.FillDepartmentDetails(departmentDetails, 10);
});

When("create department", () => {
    GeneralActions.MockCreate(Urls.Departments)
});

Then("the department should create successfully", () => {
    GeneralActions.AssertMockCreate()
});
//#endregion

//#region Search for the department
When("search for {string} department", (departmentName) => {
    GeneralActions.Search(departmentName)
});

Then("the {string} department should appear successfully", (departmentName) => {
    GeneralActions.AssertSearch(departmentName);
});
//#endregion

//#region Open the department
When("open department", () => {
    DepartmentActions.OpenDepartment();
});

Then("the department should open successfully", () => {
    DepartmentActions.AssertOpenDepartment();
});
//#endregion

//#region Edit the department
Given("the user edit the following department details", (dataTable) => {
    let paymentTermDetails = Assists.CreateInstance<DepartmentDetails>(dataTable, true);
    DepartmentActions.EditDepartmentGeneralTab(paymentTermDetails)
});

When("save department", () => {
    DepartmentActions.UpdateDepartment()
});

Then("the department should update successfully", () => {
    DepartmentActions.AssertUpdateDepartment()
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    ValidateEventsTab(eventDetailsList, DepartmentSelectors.EventsTab);
});
//#endregion

//#region Save and close the department
When("save and close department", () => {
    DepartmentActions.CloseSaveDepartment();
});

Then("the department should close successfully", () => {
    DepartmentActions.AssertCloseSaveDepartment();
});
 //#endregion