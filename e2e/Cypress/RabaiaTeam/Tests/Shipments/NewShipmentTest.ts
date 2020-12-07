describe("Create Direct Shipment Tests", () => {

    before(() => {
        cy.Login()
        cy.NavigateToMainMenu("#GeneralMHOperations")
        cy.NavigateToWorkSpaceTab("#SHIP")
    })

    it("Create Direct Export Air Shipment", () => {

        cy.Click("#HelperNotesButton_0_0")
        cy.Click(".LogitudeToggleButtonItem", "Direct")
        cy.ClickRadio("#DirectionRadio_0E")
        cy.ClickRadio("#TransportModeRadio_0A")
        cy.ValidateValue("#Shipment_ShipmentCustomerTypeCode", "Shipper")
        cy.SelectLogLovFirstElement("#Shipment_CustomerId")
        cy.SelectLogLovFirstElement("#Shipment_MainCarriageFromPortId")
        cy.SelectLogLovFirstElement("#Shipment_MainCarriageToPortId")
        cy.SaveClick("**/shipment", "#ShipmentCreatebtn", "Shipment Saved Successfully")

    })

    it("Create Direct Import Air Shipment", () => {

        cy.Click("#HelperNotesButton_0_0")
        cy.Click(".LogitudeToggleButtonItem", "Direct")
        cy.ClickRadio("#DirectionRadio_0I")
        cy.ClickRadio("#TransportModeRadio_0A")
        cy.ValidateValue("#Shipment_ShipmentCustomerTypeCode", "Consignee")
        cy.SelectLogLovFirstElement("#Shipment_CustomerId")
        cy.SelectLogLovFirstElement("#Shipment_MainCarriageFromPortId")
        cy.SelectLogLovFirstElement("#Shipment_MainCarriageToPortId")
        cy.SaveClick("**/shipment", "#ShipmentCreatebtn", "Shipment Saved Successfully")

    })

    it("Create Direct Domestic Air Shipment", () => {

        cy.Click("#HelperNotesButton_0_0")
        cy.Click(".LogitudeToggleButtonItem", "Direct")
        cy.ClickRadio("#DirectionRadio_0D")
        cy.ClickRadio("#TransportModeRadio_0A")
        cy.ValidateValue("#Shipment_ShipmentCustomerTypeCode", "Shipper")
        cy.SelectLogLovFirstElement("#Shipment_CustomerId")
        cy.SelectLogLovFirstElement("#Shipment_MainCarriageFromPortId")
        cy.SelectLogLovFirstElement("#Shipment_MainCarriageToPortId")
        cy.SaveClick("**/shipment", "#ShipmentCreatebtn", "Shipment Saved Successfully")

    })

    it("Create Direct Drop Air Shipment", () => {

        cy.Click("#HelperNotesButton_0_0")
        cy.Click(".LogitudeToggleButtonItem", "Direct")
        cy.ClickRadio("#DirectionRadio_0R")
        cy.ClickRadio("#TransportModeRadio_0A")
        cy.ValidateValue("#Shipment_ShipmentCustomerTypeCode", "Shipper")
        cy.SelectLogLovFirstElement("#Shipment_CustomerId")
        cy.SelectLogLovFirstElement("#Shipment_MainCarriageFromPortId")
        cy.SelectLogLovFirstElement("#Shipment_MainCarriageToPortId")
        cy.SaveClick("**/shipment", "#ShipmentCreatebtn", "Shipment Saved Successfully")

    })

    it("Create Direct Export Ocean FCL Shipment", () => {

        cy.Click("#HelperNotesButton_0_0")
        cy.Click(".LogitudeToggleButtonItem", "Direct")
        cy.ClickRadio("#DirectionRadio_0E")
        cy.ClickRadio("#TransportModeRadio_0O")
        cy.ClickRadio("#ShipmentTypeRadio_0FCLD")
        cy.ValidateValue("#Shipment_ShipmentCustomerTypeCode", "Shipper")
        cy.SelectLogLovFirstElement("#Shipment_CustomerId")
        cy.SelectLogLovFirstElement("#Shipment_MainCarriageFromPortId")
        cy.SelectLogLovFirstElement("#Shipment_MainCarriageToPortId")
        cy.SaveClick("**/shipment", "#ShipmentCreatebtn", "Shipment Saved Successfully")

    })

    it("Create Direct Export Ocean LCL Shipment", () => {

        cy.OpenNewShipmentWizard("Direct");
        cy.FillShipmentDefaultFields("E","O","LCLD"); 
        cy.ValidateValue("#Shipment_ShipmentCustomerTypeCode", "Shipper")
        
        cy.SaveClick("**/shipment", "#ShipmentCreatebtn", "Shipment Saved Successfully")

    })

})