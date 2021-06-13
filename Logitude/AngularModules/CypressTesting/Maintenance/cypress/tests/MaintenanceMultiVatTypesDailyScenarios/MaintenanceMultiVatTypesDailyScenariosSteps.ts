import * as Actions from "../../actions/Actions";
import * as vatTypeActions from "../../actions/VatTypeActions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { VatTypesSelectors } from "../../selectors/VatTypesSelectors";
import { VatTypeDetails } from "cypress/models/VatTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as GeneralActions from "../../actions/GeneralActions";
import { Urls } from "../../constants/Urls";

//#region Add multi vat type with lenght more than 5
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
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
    GeneralActions.MockCreate(Urls.VatTypes)
});

Then("the multi vat type should create successfully", () => {
    GeneralActions.AssertMockCreate()
});
//#endregion

//#region Search for the single vat type
When("search for {string} multi vat type", (SearchFieldValue) => {
    GeneralActions.Search(SearchFieldValue)
});

Then("the {string} multi vat type should appear successfully", (SearchFieldValue) => {
    GeneralActions.AssertSearch(SearchFieldValue)
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