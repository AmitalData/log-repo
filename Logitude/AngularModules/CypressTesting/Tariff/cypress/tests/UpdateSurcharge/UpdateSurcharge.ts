import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../actions/Actions";
import { TariffDetails } from "../../models/TariffDetails";
import { SurchargeDetails } from "../../models/SurchargeDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import { TariffSelectors } from "../../../../Tariff/cypress/selectors/Selectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { TariffLine } from "cypress/models/TariffLine";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";

var SellerCode;

//#region Create Shipping Line 
Given("the user logged in and navigate to Shipping Line in Maintenance workspace", () => {
    Actions.LoginAndNavigateToShippingLineWorkspace();
});

When("create new shipping line", () => {
    SellerCode = Actions.CreateSeller();
});

Then("the shipping line should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PostShippinglinesRequest, 200)
})
//#endregion

//#region  Create ocean FCL surcharge cost
Given("the user navigate to tariff workspace", () => {
    cy.Click(TariffSelectors.TariffMenu, null);
});

Given("an ocean FCL surcharge cost with the following details", (dataTable) => {
    let tariffDetails = dataTable.hashes()[0] as TariffDetails;
    tariffDetails.Seller = SellerCode;
    Actions.FillNewOceanFCLSurchargesCost("Ocean FCL", tariffDetails);
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
    Actions.OpenLastCreatedTariff();
});

Given("add the follwing tariff lines", (dataTable) => {
    let tariffDetails = dataTable.hashes()[0] as TariffLine;
    Actions.FillTariffLine(tariffDetails)
});

When("add the tariff line", () => {
    cy.Click(BaseSelectors.RedButton, "Ok");
    Actions.UpdateTariff();
});

Then("the surcharge cost should update successfully", () => {
    Actions.AssertPutTariff();
});
//#endregion

//#region  Update surcharges
Given("the follwing update details", (dataTable) => {
    let tariffDetails = dataTable.hashes()[0] as TariffLine;
    Actions.FillUpdateSurcharges(tariffDetails);
});

Given("the following price details", (dataTable) => {
    let tariffDetails = dataTable.hashes()[0] as TariffLine;
    Actions.FillUpdatePrice(tariffDetails);
});

When("update", () => {
    Actions.CreateUpdateTariff();
});

Then("the surcharge update should create successfully", () => {
    Actions.AssertPostUpdateTariff();
});
//#endregion