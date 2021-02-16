import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../actions/Actions";
import * as gr from '../../../../Base/cypress/actions/GenerateRandoms'
import { TariffDetails } from "../../models/TariffDetails";
import { SurchargeDetails } from "../../models/SurchargeDetails";
import { QuickSearchDetails } from "../../../../Base/cypress/models/QuickSearchDetails";
import { BaseURLs } from "../../../../Base/cypress/constants/URLs";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import { TariffSelectors } from "../../../../Tariff/cypress/selectors/Selectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"

var Code ;

Given("the user logged in and navigate to maintenance workspace",()=>{
cy.Login();
cy.Click("#GeneralMHMaintenance",null)
cy.Click("#MaintenanceItemMTSL",null);
cy.DefineRequestWait("GET","**/carrierviews/**", "get1")
cy.Click(BaseSelectors.Button , "Add");
});

When("create new shipping line",()=>{
 Code = Actions.createSeller();
 cy.DefineRequestWait("POST","**/shippinglines", "WaitPostShippinglinesRequest")
 cy.Click(BaseSelectors.RedButton,"Ok");
});

Then("the shipping line should create successfully",()=>{
    BaseAssertion.AssertStatusCode("WaitPostShippinglinesRequest", 200)
})

//#region  Create ocean FCL surcharge cost
Given("the user logged in and navigate to tariff workspace", () => {
    //  Actions.LoginAndNavigateToTariffWorkspace();
    cy.Click(TariffSelectors.TariffMenu, null);
});

Given("create new seller",()=>{
    Actions.OpenNewSurchargeCostWizard("Ocean FCL");
});

Given("an ocean FCL surcharge cost with new seller and the following details", (dataTable) => {
    let tariffDetails = dataTable.hashes()[0] as TariffDetails;
    //fill
    cy.FillLogTextBox(TariffSelectors.TariffName, tariffDetails.Name);
    cy.FillLogLov(TariffSelectors.TariffSeller, Code, false);
});

Given("the follwing surcharges details", (dataTable) => {
    let surchargeDetailsList = dataTable.hashes() as SurchargeDetails[];
    Actions.FillSurcharges(surchargeDetailsList);
});

When("create surcharge cost", () => {
    Actions.CreateTariff();
});

Then("the surcharge cost should create successfully", () => {
    Actions.ValidateCreateSurchargeCost();
});
//#endregion

//#region Add tariff lines
Given("the user open the created surcharge cost", () => {

});

Given("add the follwing tariff lines", () => {

});

When("add the tariff line", () => {

});

Then("the surcharge cost should update successfully", () => {

});
//#endregion

//#region  Update surcharges
Given("the follwing update details", () => {

});

Given("the following price details", () => {

});

When("update", () => {

});

Then("the surcharge update should create successfully", () => {

});
//#endregion