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
        sh.SaveShipment("CreatedShipmentsData/DirectEA.json")
    })

    it("Create Direct Import Air Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","A")
        sh.SaveShipment("CreatedShipmentsData/DirectIA.json")
    })

    it("Create Direct Domestic Air Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","A")
        sh.SaveShipment("CreatedShipmentsData/DirectDA.json")
    })

    it("Create Direct Drop Air Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","A")
        sh.SaveShipment("CreatedShipmentsData/DirectRA.json")
    })

    it("Create Direct Export Ocean FCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("E","O","FCLD")
        sh.SaveShipment("CreatedShipmentsData/DirectEOFCLD.json")
    })

    it("Create Direct Export Ocean LCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("E","O","LCLD")
        sh.SaveShipment("CreatedShipmentsData/DirectEOLCLD.json")
    })

    it("Create Direct Import Ocean FCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","O","FCLD")
        sh.SaveShipment("CreatedShipmentsData/DirectIOFCLD.json")
    })

    it("Create Direct Import Ocean LCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","O","LCLD")
        sh.SaveShipment("CreatedShipmentsData/DirectIOLCLD.json")
    })

    it("Create Direct Domestic Ocean FCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","O","FCLD")
        sh.SaveShipment("CreatedShipmentsData/DirectDOFCLD.json")
    })

    it("Create Direct Domestic Ocean LCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","O","LCLD")
        sh.SaveShipment("CreatedShipmentsData/DirectDOLCLD.json")
    })

    it("Create Direct Drop Ocean FCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","O","FCLD")
        sh.SaveShipment("CreatedShipmentsData/DirectROFCLD.json")
    })

    it("Create Direct Drop Ocean LCL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","O","LCLD")
        sh.SaveShipment("CreatedShipmentsData/DirectROLCLD.json")
    })

    it("Create Direct Export Inland FTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("E","I","FTL")
        sh.SaveShipment("CreatedShipmentsData/DirectEIFTL.json")
    })

    it("Create Direct Export Inland LTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("E","I","LTL")
        sh.SaveShipment("CreatedShipmentsData/DirectEILTL.json")
    })

    it("Create Direct Import Inland FTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","I","FTL")
        sh.SaveShipment("CreatedShipmentsData/DirectIIFTL.json")
    })

    it("Create Direct Import Inland LTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("I","I","LTL")
        sh.SaveShipment("CreatedShipmentsData/DirectIILTL.json")
    })

    it("Create Direct Domestic Inland FTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","I","FTL")
        sh.SaveShipment("CreatedShipmentsData/DirectDIFTL.json")
    })

    it("Create Direct Domestic Inland LTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("D","I","LTL")
        sh.SaveShipment("CreatedShipmentsData/DirectDILTL.json")
    })

    it("Create Direct Drop Inland FTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","I","FTL")
        sh.SaveShipment("CreatedShipmentsData/DirectRIFTL.json")
    })

    it("Create Direct Drop Inland LTL Shipment", () => {
        sh.OpenNewShipmentWizard("Direct")
        sh.FillShipmentDefaultFields("R","I","LTL")
        sh.SaveShipment("CreatedShipmentsData/DirectRILTL.json")
    })

})