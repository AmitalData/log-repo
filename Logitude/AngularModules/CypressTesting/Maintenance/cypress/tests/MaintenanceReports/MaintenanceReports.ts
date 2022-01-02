import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import * as MaintenanceActions from "../../actions/Actions";
import * as Actions from "../../actions/ReportsActions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { ReportsDetails } from "../../models/ReportsDetails";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";

let reportsDetails: ReportsDetails

//#region Create new Report
Given("the user logged in and open {string} in maintenance menu", (Reports) => {
    cy.Login(true);
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(Reports, MaintenanceSelectors.MaintenanceItemReport);
});

Given("a Report with the following details", (dataTable) => {
    reportsDetails = Assists.CreateInstance<ReportsDetails>(dataTable, true);
    MaintenanceActions.OpenNewWizard("Report");
    Actions.FillReportstDetails(reportsDetails)
});

When("create Report", () => {
    Actions.CreateReport();
});

Then("the Report should create successfully", () => {
    Actions.AssertCreateReport()
});

When("search for Report", () => {
    Actions.SearchReport();
});

Then("the Reports should appear successfully", () => {
    Actions.AssertSearchReport()
    cy.get(BaseSelectors.RowClass).eq(0).click();
});

Given("The user create a new message template with the following details", (dataTable) => {
   reportsDetails = Assists.CreateInstance<ReportsDetails>(dataTable, true);
   Actions.FillReportTemplate(reportsDetails)

});

When("save Report", () => {
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainSave);
     
});

Then("the message template should update successfully", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, MaintenanceSelectors.ReportEventTab);
});
