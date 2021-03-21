import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../actions/Actions";
import * as BaseActions from "../../../../Base/cypress/actions/Actions"
import { TariffDetails } from "../../models/TariffDetails";
import { FreightCostTariffLineDetails } from "cypress/models/FreightCostTariffLineDetails";
import { PriceCheckDetails } from "cypress/models/PriceCheckDetails";
import { priceCheck } from "cypress/models/priceCheck";
import { TariffSelectors } from "../../selectors/Selectors";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import * as Assists from "../../../../Base/cypress/assists/Assists";

var isSameDate : Boolean

//#region Air Create Freight Cost 
Given("the user logged in and navigate to tariff workspace", () => {
    Actions.LoginAndNavigateToTariffWorkspace();
});

Given("an air freight cost with the following details", (dataTable) => {
    let tariffDetails = Assists.CreateInstance<TariffDetails>(dataTable, true);
    Actions.FillNewFreightCost("Air", tariffDetails);
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

Given("add the follwing tariff line", (dataTable) => {
    let freightCostTariffLineDetailsList = Assists.CreateSet<FreightCostTariffLineDetails>(dataTable);
    Actions.AddFreightCostTariffLines("Air", freightCostTariffLineDetailsList);
});
When("approve version", () => {
    Actions.ApproveTariffVersion();
});

Then("the version should approve successfully", () => {
    Actions.ValidateApproveTariffVersion();
});
//#endregion

//#region Edit Surcharge
Given("the user in {string} surchage workspace", (SurchargeType) => {
    Actions.BackToTariffWorkspace()
    Actions.OpenSurchargeQueries(SurchargeType);
});

Given("open surchage with {string} as seller", (SellerName) => {
    Actions.SearchASurcharge(SellerName);
    cy.wait(3000)
    Actions.OpenTheFirstResult();
});

When("copy into new version if start date is not {string}", (startdate) => {
    let NowDate = BaseActions.GetTodayDate()
    cy.get(BaseSelectors.PackageGrid("4")).find(BaseSelectors.SpanElement).invoke(BaseSelectors.TextElement).then((text) => {
        if(text.trim()==NowDate){
            cy.log("Use Same Tariff Line")
            isSameDate = true
        }else{
           Actions.CopyIntoNewVersion(startdate)
        }
    })
});

Then("new version should approve successfully", () => {
    if(isSameDate){
        cy.log("Same Date")
    }else{
        Actions.ValidateApproveTariffVersion
    }
    Actions.SetTariffNumberFromTitle(TariffSelectors.TariffNumberShortTitle);
    cy.BackButton(TariffSelectors.ContainsBackButton("Air"))
    cy.Click(BaseSelectors.BackBottonBodyClass,"Tariff");
    
})
//#endregion

//#region Check Air Price 
Given("the user back into tariff workspace and open price check wizard", () => {
    Actions.OpenPriceCheckWizard("Air");
});

Given("fill the following price check details", (dataTable) => {
    let airPriceCheckDetails = Assists.CreateInstance<PriceCheckDetails>(dataTable, true);
    Actions.FillPriceCheckWizard("Air", airPriceCheckDetails)

});

When("search about prices", () => {
    Actions.PriceCheckSearch();
});

Then("air price should equal the following", (dataTable) => {
    let PriceCheckDetails = Assists.CreateInstance<priceCheck>(dataTable, true);
    Actions.ValidateTariffPriceCheck(PriceCheckDetails);
});
//#endregion