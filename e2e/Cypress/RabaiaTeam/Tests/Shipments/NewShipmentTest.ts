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
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", null)
    })

    it("Create Direct Import Air Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","A")
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", "DIAShipment")
    })

    it("Create Direct Domestic Air Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","A")
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", "DDAShipment")
    })

    it("Create Direct Drop Air Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","A")
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", "DRAShipment")
    })

    it("Create Direct Export Ocean FCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("E","O","FCLD")
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", "DEOFCLShipment")
    })

    it("Create Direct Export Ocean LCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("E","O","LCLD")
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", "DEOLCLShipment")
    })

    it("Create Direct Import Ocean FCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","O","FCLD")
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", "DIOFCLShipment")
    })

    it("Create Direct Import Ocean LCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","O","LCLD")
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", "DIOLCLShipment")
    })

    it("Create Direct Domestic Ocean FCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","O","FCLD")
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", "DDOFCLShipment")
    })

    it("Create Direct Domestic Ocean LCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","O","LCLD")
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", "DDOLCLShipment")
    })

    it("Create Direct Drop Ocean FCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","O","FCLD")
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", "DROFCLShipment")
    })

    it("Create Direct Drop Ocean LCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","O","LCLD")
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", "DROLCLShipment")
    })

    it("Create Direct Export Inland FTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("E","I","FTL")
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", "DEIFTLShipment")
    })

    it("Create Direct Export Inland LTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("E","I","LTL")
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", "DEILTLShipment")
    })

    it("Create Direct Import Inland FTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","I","FTL")
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", "DIIFTLShipment")
    })

    it("Create Direct Import Inland LTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","I","LTL")
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", "DIILTLShipment")
    })

    it("Create Direct Domestic Inland FTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","I","FTL")
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", "DDIFTLShipment")
    })

    it("Create Direct Domestic Inland LTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","I","LTL")
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", "DDILTLShipment")
    })

    it("Create Direct Drop Inland FTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","I","FTL")
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", "DRIFTLShipment")
    })

    it("Create Direct Drop Inland LTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","I","LTL")
        cy.SaveClick("#ShipmentCreatebtn", null, "**/shipment", "DRILTLShipment")
    })

})