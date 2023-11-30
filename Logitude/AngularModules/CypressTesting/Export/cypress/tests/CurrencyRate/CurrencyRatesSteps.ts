import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as CurrencyRatesAction from '../../actions/CurrencyRatesActions';
import * as Actions from '../../actions/Actions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { CurrencyRatesDetails } from '../../models/CurrencyRatesDetails';


//#region Check Export Currency rates 
Given("the user logged in and navigates to Export workspace", () => {
    
    cy.Login();
    Actions.NavigatesCurrencyRatesWizerd()
});

Given("fill Export Currency rates with the following details", (dataTable) => {
    CurrencyRatesAction.NavigatesCurrencyRatesWizerd()
    let CurrencyRatesDetails = Assists.CreateInstance<CurrencyRatesDetails>(dataTable, true);
    CurrencyRatesAction.FillCurrencyRatesDetails(CurrencyRatesDetails)
});

When("Check Export Currency rates", () => {
    CurrencyRatesAction.SendToCustom()
});

Then("the Rates should get successfully", () => {
    CurrencyRatesAction.FoundCurrencyRate()
    
});

