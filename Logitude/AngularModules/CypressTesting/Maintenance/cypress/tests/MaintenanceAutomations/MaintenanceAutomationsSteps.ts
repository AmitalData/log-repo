
import { MaintenanceSelectors } from "../../selectors/Selectors";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import { Urls } from "../../constants/Urls";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../actions/Actions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import {AutomationsDetails} from  "../../models/AutomationsDetails"
import * as AutomationsActions from "../../actions/AutomationsActions";
import {AutomationsSelectors} from "../../selectors/AutomationsSelectors"

//#region Create new automation onCreate
Given("the user logged in",()=>{
    cy.Login()
});
Given("the user navigates to Automations menu", () => {        
    AutomationsActions.OpenAutomationMenu()
});
Given("the user select {string} entity",(Masters)=> {
    cy.contains(Masters).click()
});
Given("the user try to add automation onCreate with the following detailes",(dataTable)=> {
    let automationsDetails = Assists.CreateInstance<AutomationsDetails>(dataTable, true);
    AutomationsActions.checkAutomationvalidations(automationsDetails)
});
When(" the user save the new automatiion",()=> {
});
Then(" the new automation should createed successfully",()=> {
});

//#endregion 
//#region Scenario: Edit an automation
Given ("the user open an automation from master entity as following",()=> {
});
Given("edit it",()=> {
});
When(" update Automation",()=> {
});
Then(" the automation should updated successfully",()=> {
});
//#endregion