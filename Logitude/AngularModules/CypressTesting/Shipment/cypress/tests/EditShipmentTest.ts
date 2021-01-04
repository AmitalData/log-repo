import * as sh from "../actions/ShipmentActions"
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors"


describe("Edit Direct Shipment Tests", () => {

    before(() => {
        cy.Login()
        cy.Click(BaseSelectors.OperationsMenu, null)
        cy.Click("#SHIP", null)
    })

    it("Edit Direct Export Air Shipment", () => {
        sh.OpenShipment("CreatedShipmentsData/DirectEA.json")
        sh.FillGeneralTab()
        sh.FillOrdersTab()
        sh.FillPartnersTab("E","A")
        sh.FillPackagesTab("A")
        
    })

})