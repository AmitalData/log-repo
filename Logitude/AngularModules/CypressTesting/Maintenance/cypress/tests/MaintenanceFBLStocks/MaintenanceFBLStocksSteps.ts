import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { MaintenanceSelectors } from "../../../cypress/selectors/Selectors";
import * as FBLStockActions from "../../actions/FBLStockActions";
import * as MaintenanceActions from "../../actions/Actions";
import { FBLStockDetails } from "../../../cypress/models/FBLStockDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";

let fblStockDetails:FBLStockDetails;

//#region Create new fblStock by End Number
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login();
    MaintenanceActions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemFBLStock)
});

Given("a fblStock with the following details", (dataTable) => {
    fblStockDetails = Assists.CreateInstance<FBLStockDetails>(dataTable, true);
    FBLStockActions.OpenAddWizard();
    FBLStockActions.FillFBLStockDetails(fblStockDetails) 
});

When("create fblStock", () => {
    FBLStockActions.CreateFBLStock();
});
 
Then("the fblStock should create successfully", () => {
    FBLStockActions.AssertCreateFBLStock();
});

//#endregion


//#region remove FBLStock 
When("Remove fblStock", () => {
    FBLStockActions.RemoveFBLStock()
});
 
Then("the fblStock should Remove successfully", () => {
    
    FBLStockActions.AssertRemoveFBLStock() 
});
//#endregion


//#region create FBLStock by Amount
Given("a fblStock with the following details", (dataTable) => {
    fblStockDetails = Assists.CreateInstance<FBLStockDetails>(dataTable, true);
    FBLStockActions.FillFBLStockDetails(fblStockDetails) 
});
 
When("create fblStock", () => {
    FBLStockActions.CreateFBLStock();
});
 
Then("the fblStock should create successfully", () => {
    FBLStockActions.AssertCreateFBLStock();
});



//#region Remove FBLStock seires 
When("Remove fblStock series", () => {
    FBLStockActions.RemoveFBLStockSeries();
});
 
Then("the fblStock series should Remove successfully", () => {
    FBLStockActions.AssertRemoveFBLStock(); 
});
//#endregion





