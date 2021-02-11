import * as sh from '../../Actions/EditShipmentActions'

describe("Edit Direct Shipment Tests", () => {

    before(() => {
        cy.Login()
        cy.Click("#GeneralMHOperations", null)
        cy.Click("#SHIP", null)
    })

    it("Edit Direct Export Air Shipment", () => {
        sh.OpenShipment("DEAShipment")
        sh.FillGeneralTab()
        sh.FillOrdersTab()
        sh.FillPartnersTab("E","A")
        sh.FillPackagesTab("A")
        
    })

})