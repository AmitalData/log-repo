import * as Actions from "../../actions/Actions";
import * as vatTypeActions from "../../actions/VatTypeActions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { VatTypesSelectors } from "../../selectors/VatTypesSelectors";
import { VatTypeDetails } from "cypress/models/VatTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";

//#region Add single vat type with lenght more than 5
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, VatTypesSelectors.MaintenanceItem)
});

When("add {string} as single vat type code", (VatTypeCode) => {
    Actions.OpenNewWizard("VatType");
    vatTypeActions.FillVatTypeCode(VatTypeCode)
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    Actions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion

//#region Create new single vat type
Given("a single vat type with the following details", (dataTable) => {
    let vatTypeDetails = Assists.CreateInstance<VatTypeDetails>(dataTable, true);
    vatTypeActions.FillVatTypeDetails(vatTypeDetails, 5);
});

When("create single vat type", () => {
    vatTypeActions.CreateVatType();
});

Then("the single vat type should create successfully", () => {
    vatTypeActions.AssertCreateVatType();
});
//#endregion

//#region Search for the single vat type
When("search single vat type", () => {
    vatTypeActions.SearchVatType()
});

Then("the single vat type should appear successfully", () => {
    vatTypeActions.AssertSearchVatType()
});
//#endregion

//#region Open the single vat type
When("open single vat type", () => {
    vatTypeActions.OpenVatType()
});

Then("the single vat type should open successfully", () => {
    vatTypeActions.AssertOpenVatType();
});
//#endregion

//#region Edit the single vat type
Given("the user fill the following single vat type General details", (dataTable) => {
    let vatTypeDetails = Assists.CreateInstance<VatTypeDetails>(dataTable, true);
    vatTypeActions.EditVatTypeGeneralTab(vatTypeDetails)
});

Given("add vat type percentage", (dataTable) => {
    let vatTypeDetails = Assists.CreateInstance<VatTypeDetails>(dataTable, true);
    cy.Navigate(VatTypesSelectors.PercentagesTab)
    vatTypeActions.NavigateEditPercentage()
    vatTypeActions.EditVatTypePercentagesTab(vatTypeDetails)
});

Given("fill the following single vat type Accounting details", (dataTable) => {
    let vatTypeDetails = Assists.CreateInstance<VatTypeDetails>(dataTable, true);
    cy.Navigate(VatTypesSelectors.AccountingTab)
    vatTypeActions.FillVatTypeAccountingTab(vatTypeDetails)
});

When("save single vat type", () => {
    vatTypeActions.UpdateVatType()
});

Then("the single vat type should update successfully", () => {
    vatTypeActions.AssertUpdateVatType()
});

Then("the following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, VatTypesSelectors.EventsTab);
});
//#endregion

//#region Save and close the single vat type
When("save and close single vat type", () => {
    vatTypeActions.CloseSaveVatType();
});

Then("the single vat type should close successfully", () => {
    vatTypeActions.AssertCloseSaveVatType();
});
 //#endregion