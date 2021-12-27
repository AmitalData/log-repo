import {AutomationsDetails} from  "./../models/AutomationsDetails"
import {AutomationsSelectors} from "./../selectors/AutomationsSelectors"
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";


export function checkAutomationvalidations(automationDetails: AutomationsDetails){
cy.get(AutomationsSelectors.add).click()

cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}