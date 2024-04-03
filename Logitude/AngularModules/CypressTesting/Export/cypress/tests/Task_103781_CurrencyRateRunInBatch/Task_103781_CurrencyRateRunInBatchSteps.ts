import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as CurrencyRateRunInBatchActions from '../../actions/CurrencyRateRunInBatchActions';
import * as Actions from '../../actions/Actions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { CurrencyRateRunInBatchDetails } from '../../models/CurrencyRateRunInBatchDetails';
import { CurrencyRateRunInBatchSelectors } from '../../selectors/CurrencyRateRunInBatchSelectors';


//#region Check Export Currency rates 
Given("the user logged in and navigates to Export workspace", () => {
    
    cy.Login();
    Actions.NavigatesCurrencyRatesWizerd()
});

Given("fill Export Currency rates with the following details", (dataTable) => {
    CurrencyRateRunInBatchActions.NavigatesCurrencyRatesWizerd()
    let CurrencyRateRunInBatchDetails = Assists.CreateInstance<CurrencyRateRunInBatchDetails>(dataTable, true);
    CurrencyRateRunInBatchActions.FillCurrencyRatesDetails(CurrencyRateRunInBatchDetails)
});

Given("the user logged in and fill the following details", (dataTable) => {
    let CurrencyRateRunInBatchDetails = Assists.CreateInstance<CurrencyRateRunInBatchDetails>(dataTable, true);
    CurrencyRateRunInBatchActions.FillRequestSheets(CurrencyRateRunInBatchDetails)
});

When("clicking the serche botton", () => {
    cy.Click(CurrencyRateRunInBatchSelectors.Search, null);

});


Then("the request status will be Answer was analyze", () => {
        CurrencyRateRunInBatchActions.CheckStatusRequest();
        
});



