import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";

export function NavigatesFullAccounting() {
    cy.Click(BaseSelectors.FullAccountingTab, null)
}