import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import { PackageTypeSelectors } from "../../../cypress/selectors/PackageTypeSelectors";
import * as PackageTypeActions from "../../actions/PackageTypeActions";
import * as MaintenanceActions from "../../actions/Actions";
import { PackageTypeDetails } from "../../../cypress/models/PackageTypeDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { EventTypeDetails } from "../../../../Base/cypress/models/EventTypeDetails";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { Constants } from "../../constants/Constants";



let packageTypeDetails: PackageTypeDetails;


//#region Create new package type
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemPackageType)
});

Given("a package type with the following details", (dataTable) => {
    packageTypeDetails = Assists.CreateInstance<PackageTypeDetails>(dataTable, true);
    MaintenanceActions.OpenNewWizard(Constants.PackageType);
    PackageTypeActions.FillPackageTypeDetails(packageTypeDetails)
});

When("create package type", () => {
    PackageTypeActions.CreatePackageType();
});

Then("the package type should create successfully", () => {
    PackageTypeActions.AssertCreatePackageType();
});
//#endregion


//#region Search for the package type by code
When("search package type", () => {
    PackageTypeActions.SearchPackageType()
});

Then("the package type should appear successfully", () => {
    PackageTypeActions.AssertSearchPackageType()
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
Given("a {string} as packageTypeLocalName", (packageTypeLocalName) => {
    PackageTypeActions.FillPackageTypeLocalName(packageTypeLocalName)
});

Given("the user activate package type", () => {
    MaintenanceActions.ChangeInactiveCheckBoxValue(PackageTypeSelectors.InActivePackageTypeCheckBox)
});

When("edit package type", () => {
    PackageTypeActions.EditPackageType();
});

Then("the package type should update successfully", () => {
    PackageTypeActions.AssertEditPackageType();
});

Then("following event should appear in events tab", (dataTable) => {
    let eventDetailsList = Assists.CreateSet<EventTypeDetails>(dataTable);
    BaseActions.ValidateEventsTab(eventDetailsList, PackageTypeSelectors.PackageTypeEventsTab);
});
//#endregion