import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../actions/Actions";
import { TariffDetails } from "../../models/TariffDetails";
import { SurchargeDetails } from "../../models/SurchargeDetails";
import { SurchargeCostTariffLineDetails} from "cypress/models/SurchargeCostTariffLineDetails";

var SellerCode ;

//#region Create Shipping Line 
Given("the user logged in and navigate to Shipping Line in Maintenance workspace", () => {
    Actions.LoginAndNavigateToShippingLineWorkspace();
});

When("create new shipping line", () => {
    SellerCode = Actions.CreateNewShippingLine();
});

Then("the shipping line should create successfully", () => {
    Actions.ValidateShippingLine();
})
//#endregion

//#region  Create ocean FCL surcharge cost
Given("the user in tariff workspace", () => {
    Actions.NavigateToTariffWorkspace()
});

Given("an ocean FCL surcharge cost with the following details", (dataTable) => {
    let tariffDetails = dataTable.hashes()[0] as TariffDetails;
    tariffDetails.Seller = SellerCode ;
    Actions.FillNewSurchargeCost("Ocean FCL", tariffDetails);
    // Actions.FillNewSurchargeCostForUpdate("Ocean FCL", tariffDetails);
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
    let tariffDetails = dataTable.hashes() as SurchargeCostTariffLineDetails[];
    Actions.AddSurchargeCostTariffLines(tariffDetails)
});

When("add the tariff line", () => {
    Actions.UpdateTariff();
});

Then("the surcharge cost should update successfully", () => {
    Actions.ValidateUpdateTariff();
});
//#endregion

//#region  Update surcharges
Given("the user in update tab", () => {
    Actions.OpenUpdateTab();
});

Given("the follwing surcharge cost update details", (dataTable) => {
    let tariffDetails = dataTable.hashes()[0] as SurchargeCostTariffLineDetails;
    Actions.FillUpdateSurcharges(tariffDetails);
});

When("update", () => {
    Actions.CreateUpdateTariff();
});

Then("the surcharge update should create successfully", () => {
    Actions.ValidatePostUpdateTariff();
});
//#endregion