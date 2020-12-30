import * as sh from "../actions/ShipmentActions"
import {Selectors} from "../selectors/Selectors"

describe("Edit Direct Shipment Tests", () => {

    before(() => {
        cy.Login()
        cy.Click(Selectors.GeneralMHOperations, null)
        cy.Click("#SHIP", null)
    })

    it("Edit Direct Export Air Shipment", () => {
        sh.OpenShipment("ResponseData/DEAShipment.json")
        sh.FillGeneralTab()
        sh.FillOrdersTab()
        sh.FillPartnersTab("E","A")
        sh.FillPackagesTab("A")
        
    })

})