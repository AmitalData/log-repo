import * as Actions from "../../actions/Actions";
import * as vatTypeActions from "../../actions/VatTypeActions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { VatTypesSelectors } from "../../selectors/VatTypesSelectors";
import { VatTypeDetails } from "cypress/models/VatTypeDetails";

let vatTypeDetails: VatTypeDetails
let vatTypeCode = null
//#region Create new single vat type
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, VatTypesSelectors.MaintenanceItem)
});

Given("a single vat type with the following details", (dataTable) => {
    vatTypeDetails = Assists.CreateInstance<VatTypeDetails>(dataTable, true);
    Actions.OpenNewWizard("VatType");
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
    vatTypeCode = vatTypeActions.getVatTypeCode()
    vatTypeActions.SearchVatType(vatTypeCode)
});

Then("the single vat type should appear successfully", () => {
    vatTypeActions.AssertSearchVatType(vatTypeCode);
});
//#endregion

//#region Open the single vat type
When("open single vat type", () => {
    vatTypeActions.OpenVatType();
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
//#endregion

//#region Save and close the single vat type
When("save and close single vat type", () => {
    vatTypeActions.CloseSaveVatType();
});

Then("the single vat type should close successfully", () => {
    vatTypeActions.AssertCloseSaveVatType();
});
 //#endregion