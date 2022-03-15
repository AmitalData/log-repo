
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../../../../Base/cypress/assists/Assists";
import * as SharedLogisticsActions from "../../../actions/SharedLogisticsActions/SharedLogisticsActions";
import { SharedLogisticsSettingsDetails } from "cypress/models/SharedLogisticsDetails/SharedLogisticsSettingsDetails";
import { DocumentsPermissionsDetails } from "cypress/models/SharedLogisticsDetails/DocumentsPermissionsDetails";
import { PartnersPermissionsDetails } from "cypress/models/SharedLogisticsDetails/PartnersPermissionsDetails";
import { MoneyPermissionsDetails } from "cypress/models/SharedLogisticsDetails/MoneyPermissionsDetails";

//#region variables
let sharedLogisticsSettingsDetails:SharedLogisticsSettingsDetails
let documentsPermissionsDetails:DocumentsPermissionsDetails
//#endregion


Given("the user logged in", () => {
    cy.Login()
});
Given("the user Activate Shared Logistics", () => {
    SharedLogisticsActions.ChooseSharedLogistics()
    SharedLogisticsActions.ActivateSharedLogistics()
});
When("the user save changes", () => {
    SharedLogisticsActions.SaveActivateSharedLogistics()
});
Then("the Shared Logistics should Activated successfully", () => {
    SharedLogisticsActions.AssertActivateSharedLogistics()
});

Given("the user navigates to Shared Logistics", (dataTable) => {
    SharedLogisticsActions.ChooseSharedLogistics()
});

Given("update Shared Logistics Settings as following", (dataTable) => {

    sharedLogisticsSettingsDetails= Assists.CreateInstance<SharedLogisticsSettingsDetails>(dataTable, true); 
    SharedLogisticsActions.UpdateSharedLogisticsSetting(sharedLogisticsSettingsDetails)
});

Then("the Shared Logistics should updated successfully", () => {
    SharedLogisticsActions.AssertActivateSharedLogistics()
});
//Documents Permissions
Given("update Documents Permissions as following", (dataTable) => {
    documentsPermissionsDetails= Assists.CreateInstance<DocumentsPermissionsDetails>(dataTable, true); 
    SharedLogisticsActions.UpdatedocumentsPermissions(documentsPermissionsDetails)
});

When ("the user save Documents Permissions changes", () => {
    SharedLogisticsActions.SaveDocumentsPermissions()
});
Then ("the Documents Permissions should updated successfully", () => {
    SharedLogisticsActions.AssertDocumentsPermissions()
});
//Partners Permissions
Given("update Partners Permissions as following", (dataTable) => {
    let partnersPermissionsDetails= Assists.CreateInstance<PartnersPermissionsDetails>(dataTable, true); 
    SharedLogisticsActions.UpdatePartnersPermissions(partnersPermissionsDetails)
});

When("the user save Partners Permissions changes", () => {
    SharedLogisticsActions.SavePartnersPermissions()
});
Then("the Partners Permissions should updated successfully", () => {
    SharedLogisticsActions.AssertPartnersPermissions()
});
// Money Permissions
Given("update Money Permissions as following", (dataTable) => {
    let moneyPermissionsDetails= Assists.CreateInstance<MoneyPermissionsDetails>(dataTable, true); 
    SharedLogisticsActions.UpdateMoneyPermissions(moneyPermissionsDetails)
});

When("the user save Money Permissions changes", () => {
    SharedLogisticsActions.SaveMoneyPermissions()
});
Then("the Money Permissions should updated successfully", () => {
    SharedLogisticsActions.AssertMoneyPermissions()
});