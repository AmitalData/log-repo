import * as Actions from "../../actions/Actions";
import * as vatTypeActions from "../../actions/VatTypeActions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { VatTypesSelectors } from "../../selectors/VatTypesSelectors";
import { VatTypeDetails } from "cypress/models/VatTypeDetails";

let vatTypeDetails: VatTypeDetails
let vatTypeCode = null
//#region Create new multi vat type
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, VatTypesSelectors.MaintenanceItem)
});

Given("a multi vat type with the following details", (dataTable) => {
    vatTypeDetails = Assists.CreateInstance<VatTypeDetails>(dataTable, true);
    Actions.OpenNewWizard("VatType");
    vatTypeActions.FillVatTypeDetails(vatTypeDetails, 5);
});

When("create multi vat type", () => {
    vatTypeActions.CreateVatType();
});

Then("the multi vat type should create successfully", () => {
    vatTypeActions.AssertCreateVatType();
});
//#endregion

//#region Search for the multi vat type
When("search multi vat type", () => {
    vatTypeCode = vatTypeActions.getVatTypeCode()
    vatTypeActions.SearchVatType(vatTypeCode)
});

Then("the multi vat type should appear successfully", () => {
    vatTypeActions.AssertSearchVatType(vatTypeCode);
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
//#endregion

//#region Save and close the multi vat type
When("save and close multi vat type", () => {
    vatTypeActions.CloseSaveVatType();
});

Then("the multi vat type should close successfully", () => {
    vatTypeActions.AssertCloseSaveVatType();
});
 //#endregion