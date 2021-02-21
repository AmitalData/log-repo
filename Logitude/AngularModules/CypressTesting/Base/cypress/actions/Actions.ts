import { BaseSelectors } from '../selectors/BaseSelectors';

export function NavigatesToMaintenanceMenu(){
    cy.Click(BaseSelectors.MaintenanceMenu,null)
}
export function NavigatesToCustomsSettings(){
    NavigatesToMaintenanceMenu();
    cy.Click(BaseSelectors.SystemSettings,null)
    cy.Click(BaseSelectors.CustomsSettings,null)
}
export function ActivateCustomsManagementInShipments(){
    NavigatesToCustomsSettings()
    cy.get(BaseSelectors.typeCheckbox).check({force: true})
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK)

}
