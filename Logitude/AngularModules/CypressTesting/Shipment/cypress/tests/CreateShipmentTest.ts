import * as sh from "../actions/ShipmentActions"
import {Selectors} from "../selectors/Selectors"

describe("Create Direct Shipment Tests", () => {

    before(() => {
        cy.Login()
        cy.Click(Selectors.GeneralMHOperations, null)
        cy.Click("#SHIP", null)
    })

    it("Create Direct Export Air Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("E","A")
        sh.SaveShipment("ResponseData/DEAShipment.json")
    })

    it("Create Direct Import Air Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","A")
        sh.SaveShipment("ResponseData/DIAShipment.json")
    })

    it("Create Direct Domestic Air Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","A")
        sh.SaveShipment("ResponseData/DDAShipment.json")
    })

    it("Create Direct Drop Air Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","A")
        sh.SaveShipment("ResponseData/DRAShipment.json")
    })

    it("Create Direct Export Ocean FCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("E","O","FCLD")
        sh.SaveShipment("ResponseData/DEOFCLShipment.json")
    })

    it("Create Direct Export Ocean LCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("E","O","LCLD")
        sh.SaveShipment("ResponseData/DEOLCLShipment.json")
    })

    it("Create Direct Import Ocean FCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","O","FCLD")
        sh.SaveShipment("ResponseData/DIOFCLShipment.json")
    })

    it("Create Direct Import Ocean LCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","O","LCLD")
        sh.SaveShipment("ResponseData/DIOLCLShipment.json")
    })

    it("Create Direct Domestic Ocean FCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","O","FCLD")
        sh.SaveShipment("ResponseData/DDOFCLShipment.json")
    })

    it("Create Direct Domestic Ocean LCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","O","LCLD")
        sh.SaveShipment("ResponseData/DDOLCLShipment.json")
    })

    it("Create Direct Drop Ocean FCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","O","FCLD")
        sh.SaveShipment("ResponseData/DROFCLShipment.json")
    })

    it("Create Direct Drop Ocean LCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","O","LCLD")
        sh.SaveShipment("ResponseData/DROLCLShipment.json")
    })

    it("Create Direct Export Inland FTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("E","I","FTL")
        sh.SaveShipment("ResponseData/DEIFTLShipment.json")
    })

    it("Create Direct Export Inland LTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("E","I","LTL")
        sh.SaveShipment("ResponseData/DEILTLShipment.json")
    })

    it("Create Direct Import Inland FTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","I","FTL")
        sh.SaveShipment("ResponseData/DIIFTLShipment.json")
    })

    it("Create Direct Import Inland LTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","I","LTL")
        sh.SaveShipment("ResponseData/DIILTLShipment.json")
    })

    it("Create Direct Domestic Inland FTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","I","FTL")
        sh.SaveShipment("ResponseData/DDIFTLShipment.json")
    })

    it("Create Direct Domestic Inland LTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","I","LTL")
        sh.SaveShipment("ResponseData/DDILTLShipment.json")
    })

    it("Create Direct Drop Inland FTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","I","FTL")
        sh.SaveShipment("ResponseData/DRIFTLShipment.json")
    })

    it("Create Direct Drop Inland LTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","I","LTL")
        sh.SaveShipment("ResponseData/DRILTLShipment.json")
    })

})