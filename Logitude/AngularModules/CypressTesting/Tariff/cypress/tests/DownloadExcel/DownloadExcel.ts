import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../actions/Actions";
import { TariffDetails } from "../../models/TariffDetails";
import { ChargeTypeDetails } from "../../models/ChargeTypeDetails";
import { FreightCostTariffLineDetails } from "cypress/models/FreightCostTariffLineDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";

//#region ocean FCL Create Freight Cost 
Given("the user logged in and navigate to tariff workspace", () => {
    Actions.LoginAndNavigateToTariffWorkspace();
});

Given("an {string} freight cost with the following details", (tariffType, dataTable) => {
    let tariffDetails = Assists.CreateInstance<TariffDetails>(dataTable, true);
    Actions.FillNewFreightCost(tariffType, tariffDetails);
});

Given("the following All-In charges", (dataTable) => {
    let chargeTypeDetailsList = Assists.CreateSet<ChargeTypeDetails>(dataTable);
    Actions.FillAllInCharges(chargeTypeDetailsList);
});

When("create freight cost", () => {
    Actions.CreateTariff();
});

Then("the freight cost should create successfully", () => {
    Actions.ValidateCreateFreightCost();
});
//#endregion

//#region Add Tariff Line 
Given("the user open the freight cost", () => {
    Actions.OpenLastCreatedTariff();
});

Given("add the following {string} tariff line", (tariffType, dataTable) => {
    let freightCostTariffLineDetailsList = Assists.CreateSet<FreightCostTariffLineDetails>(dataTable);
    Actions.AddFreightCostTariffLines(tariffType, freightCostTariffLineDetailsList);
});

When("approve version", () => {
    Actions.ApproveTariffVersion();
});

Then("the version should approve successfully", () => {
    Actions.ValidateApproveTariffVersion();
});
//#endregion

//#region Download Excel File
When("download excel file", () => {
    Actions.DownloadExcelFile()
});

Then("the file should download successfully", () => {
    Actions.ValidateDownloadFile()
});
//#endregion


