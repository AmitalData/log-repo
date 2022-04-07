import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import { BIReportSelectors } from "../selectors/BIReportSelectors";



export function NavigatesBIReportWorkspace() {
    cy.Click(BaseSelectors.Report, null)
    cy.get(BIReportSelectors.BI).contains("BI").click()
}