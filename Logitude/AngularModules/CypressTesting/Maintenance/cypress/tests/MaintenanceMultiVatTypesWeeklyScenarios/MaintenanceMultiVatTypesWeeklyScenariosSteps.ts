import * as Actions from "../../actions/Actions";
import * as vatTypeActions from "../../actions/VatTypeActions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { VatTypesSelectors } from "../../selectors/VatTypesSelectors";
import { VatTypeDetails } from "cypress/models/VatTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";

//#region Enable Multi-percentage
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, BaseSelectors.AccountingSettings)
});

Given("navigate AR advanced settings", () => {
    cy.Navigate(VatTypesSelectors.AccountingSetting_AdvancedARHyperlinkId)
});

When("enable multi-percentage VAT Types", () => {
    vatTypeActions.EnableMultiPercentage()
});

Then("the accounting settings should update successfully", () => {
    vatTypeActions.AssertEnableMultiPercentage()
});
//#endregion

//#region Add multi vat type with lenght more than 5
Given("the user open {string} in maintenance menu", (maintenanceItemName) => {
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, VatTypesSelectors.MaintenanceItem)
});

When("add {string} as multi vat type code", (VatTypeCode) => {
    Actions.OpenNewWizard("VatType");
    vatTypeActions.ClickMultiRadio()
    vatTypeActions.FillVatTypeCode(VatTypeCode)
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    Actions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion

//#region Create new multi vat type
Given("a multi vat type with the following details", (dataTable) => {
    let vatTypeDetails = Assists.CreateInstance<VatTypeDetails>(dataTable, true);
    vatTypeActions.FillVatTypeDetails(vatTypeDetails, 5);
});

When("create multi vat type", () => {
    vatTypeActions.CreateVatType();
});

Then("the multi vat type should create successfully", () => {
    vatTypeActions.AssertCreateVatType();
});
//#endregion

//#region Search for the single vat type
When("search multi vat type", () => {
    vatTypeActions.SearchVatType()
});

Then("the multi vat type should appear successfully", () => {
    vatTypeActions.AssertSearchVatType()
});
//#endregion

//#region Open the multi vat type
When("open multi vat type", () => {
    vatTypeActions.OpenVatType();
});

Then("the multi vat type should open successfully", () => {
    vatTypeActions.AssertOpenVatType();
});
//#endregion

//#region Edit the multi vat type
Given("the user fill the following multi vat type General details", (dataTable) => {
    let vatTypeDetails = Assists.CreateInstance<VatTypeDetails>(dataTable, true);
    vatTypeActions.EditVatTypeGeneralTab(vatTypeDetails)
});

Given("fill the following multi vat type Accounting details", (dataTable) => {
    let vatTypeDetails = Assists.CreateInstance<VatTypeDetails>(dataTable, true);
    cy.Navigate(VatTypesSelectors.AccountingTab);
    vatTypeActions.FillVatTypeAccountingTab(vatTypeDetails)
});

When("save multi vat type", () => {
    vatTypeActions.UpdateVatType()
});

Then("the multi vat type should update successfully", () => {
    vatTypeActions.AssertUpdateVatType()
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, VatTypesSelectors.EventsTab);
});
//#endregion

//#region Save and close the multi vat type
When("save and close multi vat type", () => {
    vatTypeActions.CloseSaveVatType();
});

Then("the multi vat type should close successfully", () => {
    vatTypeActions.AssertCloseSaveVatType();
});
 //#endregion