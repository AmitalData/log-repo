import * as sh from "../../Actions/NewShipmentActions"

describe("Create Direct Shipment Tests", () => {

    before(() => {
        cy.Login()
        cy.Click("#GeneralMHOperations", null)
        cy.Click("#SHIP", null)
    })

    it("Create Direct Export Air Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("E","A")
        sh.SaveShipment("DEAShipment")
    })

    it("Create Direct Import Air Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","A")
        sh.SaveShipment("DIAShipment")
    })

    it("Create Direct Domestic Air Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","A")
        sh.SaveShipment("DDAShipment")
    })

    it("Create Direct Drop Air Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","A")
        sh.SaveShipment("DRAShipment")
    })

    it("Create Direct Export Ocean FCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("E","O","FCLD")
        sh.SaveShipment("DEOFCLShipment")
    })

    it("Create Direct Export Ocean LCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("E","O","LCLD")
        sh.SaveShipment("DEOLCLShipment")
    })

    it("Create Direct Import Ocean FCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","O","FCLD")
        sh.SaveShipment("DIOFCLShipment")
    })

    it("Create Direct Import Ocean LCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","O","LCLD")
        sh.SaveShipment("DIOLCLShipment")
    })

    it("Create Direct Domestic Ocean FCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","O","FCLD")
        sh.SaveShipment("DDOFCLShipment")
    })

    it("Create Direct Domestic Ocean LCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","O","LCLD")
        sh.SaveShipment("DDOLCLShipment")
    })

    it("Create Direct Drop Ocean FCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","O","FCLD")
        sh.SaveShipment("DROFCLShipment")
    })

    it("Create Direct Drop Ocean LCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","O","LCLD")
        sh.SaveShipment("DROLCLShipment")
    })

    it("Create Direct Export Inland FTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("E","I","FTL")
        sh.SaveShipment("DEIFTLShipment")
    })

    it("Create Direct Export Inland LTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("E","I","LTL")
        sh.SaveShipment("DEILTLShipment")
    })

    it("Create Direct Import Inland FTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","I","FTL")
        sh.SaveShipment("DIIFTLShipment")
    })

    it("Create Direct Import Inland LTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","I","LTL")
        sh.SaveShipment("DIILTLShipment")
    })

    it("Create Direct Domestic Inland FTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","I","FTL")
        sh.SaveShipment("DDIFTLShipment")
    })

    it("Create Direct Domestic Inland LTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","I","LTL")
        sh.SaveShipment("DDILTLShipment")
    })

    it("Create Direct Drop Inland FTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","I","FTL")
        sh.SaveShipment("DRIFTLShipment")
    })

    it("Create Direct Drop Inland LTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","I","LTL")
        sh.SaveShipment("DRILTLShipment")
    })

})