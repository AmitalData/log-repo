import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import * as FBLStockActions from "../../actions/FBLStockActions";
import * as MaintenanceActions from "../../actions/Actions";
import { FBLStockDetails } from "../../../cypress/models/FBLStockDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";

//#region Create new fblStock by End Number
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemFBLStock)
});

Given("the user adds FBL stock with following details", (dataTable) => {
    let fblStockDetails = Assists.CreateInstance<FBLStockDetails>(dataTable, true);
    FBLStockActions.OpenAddWizard();
    FBLStockActions.FillFBLStockDetails(fblStockDetails)
});

When("create fblStock", () => {
    FBLStockActions.CreateFBLStock();
});

Then("the fblStock should create successfully", () => {
    FBLStockActions.AssertCreateFBLStock();
    FBLStockActions.AssertGetAllFBLStock();
});
//#endregion

//#region remove FBLStock 
When("user removes one entry", () => {
    FBLStockActions.RemoveFBLStock()
});

Then("the fblStock should Remove successfully", () => {
    FBLStockActions.AssertRemoveFBLStock()
    FBLStockActions.AssertGetAllFBLStock();
});
//#endregion

//#region create FBLStock by Amount
Given("user adds another FBL stock with the following details", (dataTable) => {
    let fblStockDetails = Assists.CreateInstance<FBLStockDetails>(dataTable, true);
    FBLStockActions.OpenAddWizard();
    FBLStockActions.FillFBLStockDetails(fblStockDetails)
});

When("create fblStock", () => {
    FBLStockActions.CreateFBLStock();
    FBLStockActions.AssertGetAllFBLStock()
});

Then("the fblStock should create successfully", () => {
    FBLStockActions.AssertCreateFBLStock();
});

//#region Remove FBLStock seires 
When("user removes a series of entries", () => {
    FBLStockActions.RemoveFBLStockSeries();
});

Then("the fblStock series should Remove successfully", () => {
    FBLStockActions.AssertRemoveFBLStock();
    FBLStockActions.AssertGetAllFBLStock();
});
//#endregion