import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import { PackageTypeSelectors } from "../../selectors/PackageTypeSelectors";
import * as PackageTypeActions from "../../actions/PackageTypeActions";
import * as Actions from "../../actions/Actions";
import * as GeneralActions from "../../actions/GeneralActions";
import * as MaintenanceActions from "../../actions/Actions";
import { PackageTypeDetails } from "../../models/PackageTypeDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { Constants } from "../../constants/Constants";
import { Urls } from "../../constants/Urls";

//#region Add package type code with lenght more than 5
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemPackageType)
});

When("add {string} as package type code", (packageTypeCode) => {
    MaintenanceActions.OpenNewWizard(Constants.PackageType);
    PackageTypeActions.FillCode(packageTypeCode)
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    Actions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion

//#region Add package type PrintAs with lenght more than 20
When("add {string} as package type PrintAs", (printAs) => {
    PackageTypeActions.FillPrintAs(printAs)
});

Then("a validation message with {string} error should appear", (ValidationMessage) => {
    Actions.ValidateErrorPopUpMessage(ValidationMessage)
});
//#endregion

//#region Create new package type with code already exists
Given("a package type with the following required details", (dataTable) => {
    let packageTypeDetails = Assists.CreateInstance<PackageTypeDetails>(dataTable, true);
    PackageTypeActions.FillRequiredData(packageTypeDetails)
});

When("create", () => {
    PackageTypeActions.CreatePackageType();
});

Then("this validation message error {string} should appear", (validationMessage) => {
    GeneralActions.ValidateSingleErrorMessage(validationMessage)
});
//#endregion

//#region Create new package type
Given("a package type with the following details", (dataTable) => {
    let packageTypeDetails = Assists.CreateInstance<PackageTypeDetails>(dataTable, true);
    PackageTypeActions.FillPackageTypeDetails(packageTypeDetails)
});

When("create package type", () => {
    GeneralActions.MockCreate(Urls.PackageTypes)
});

Then("the package type should create successfully", () => {
    GeneralActions.AssertMockCreate()
});
//#endregion

//#region Search for the package type by code
When("search for {string} package type", (searchFieldValue) => {
    GeneralActions.Search(searchFieldValue)
});

Then("the {string} package type should appear successfully", (searchFieldValue) => {
    GeneralActions.AssertSearch(searchFieldValue)
});
//#endregion

//#region Open the package type
When("open package type", () => {
    PackageTypeActions.OpenPackageType();
});

Then("the package type should open successfully", () => {
    PackageTypeActions.AssertOpenPackageType();
});
//#endregion

//#region Edit the package type
Given("the user fill the following package type details", (dataTable) => {
    let packageTypeDetails = Assists.CreateInstance<PackageTypeDetails>(dataTable, true);
    PackageTypeActions.EditGeneralTab(packageTypeDetails)
});

When("edit package type", () => {
    PackageTypeActions.EditPackageType();
});

Then("the package type should update successfully", () => {
    PackageTypeActions.AssertEditPackageType();
});

Then("following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, PackageTypeSelectors.EventsTab);
});

When("save and close package type", () => {
    PackageTypeActions.CloseSavePackageType();
});

Then("the package type should close successfully", () => {
    PackageTypeActions.AssertCloseSavePackageType();
});
//#endregion