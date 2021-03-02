import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../actions/Actions";
import { TariffDetails } from "../../models/TariffDetails";
import { ChargeTypeDetails } from "../../models/ChargeTypeDetails";
import { FreightCostTariffLineDetails } from "cypress/models/FreightCostTariffLineDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";

//#region Create Air freight cost
Given("the user logged in and navigate to tariff workspace", () => {
  Actions.LoginAndNavigateToTariffWorkspace();
});

Given("an air freight cost with the following details", (dataTable) => {
  let tariffDetails = dataTable.hashes()[0] as TariffDetails;
  Actions.FillNewFreightCost("Air", tariffDetails);
});

Given("the follwing All-In charges", (dataTable) => {
  let chargeTypeDetailsList = dataTable.hashes() as ChargeTypeDetails[];
  Actions.FillAllInCharges(chargeTypeDetailsList);
});

When("create freight cost", () => {
  Actions.CreateTariff();
});

Then("the freight cost should create successfully", () => {
  Actions.ValidateCreateFreightCost();
});
//#endregion

//#region Upload Excel File
Given("the user open the created air freight cost", () => {
  Actions.OpenLastCreatedTariff();
});

When("upload excel file", () => {
  Actions.UploadExcelFile();
});

Then("the file should load successfully with the following details", (dataTable) => {
  let freightCostTariffLineDetailsList = dataTable.hashes()[0] as FreightCostTariffLineDetails;
  Actions.ValidateUploadExcelFile();
  Actions.ValidateTariffLineRow(freightCostTariffLineDetailsList)

});
//#endregion