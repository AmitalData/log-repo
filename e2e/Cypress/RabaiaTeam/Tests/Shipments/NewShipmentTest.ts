describe("Create Direct Shipment Tests", () => {

    before(() => {
        cy.Login()
        cy.NavigateToMainMenu("#GeneralMHOperations")
        cy.NavigateToWorkSpaceTab("#SHIP")
    })

    it("Create Direct Export Air Shipment", () => {

        cy.OpenNewShipmentWizard("Direct");
        cy.FillShipmentDefaultFields("E","A");   
        cy.ValidateValue("#Shipment_ShipmentCustomerTypeCode", "Shipper")
        
        cy.SaveClick("**/shipment", "#ShipmentCreatebtn", "Shipment Saved Successfully")

    })

    it("Create Direct Import Air Shipment", () => {

        cy.OpenNewShipmentWizard("Direct");
        cy.FillShipmentDefaultFields("I","A");  
        cy.ValidateValue("#Shipment_ShipmentCustomerTypeCode", "Consignee")
        
        cy.SaveClick("**/shipment", "#ShipmentCreatebtn", "Shipment Saved Successfully")

    })

    it("Create Direct Domestic Air Shipment", () => {
        
        cy.OpenNewShipmentWizard("Direct");
        cy.FillShipmentDefaultFields("D","A"); 
        cy.ValidateValue("#Shipment_ShipmentCustomerTypeCode", "Shipper")
   
        cy.SaveClick("**/shipment", "#ShipmentCreatebtn", "Shipment Saved Successfully")

    })

    it("Create Direct Drop Air Shipment", () => {

        cy.OpenNewShipmentWizard("Direct");
        cy.FillShipmentDefaultFields("R","A"); 
        cy.ValidateValue("#Shipment_ShipmentCustomerTypeCode", "Shipper")
 
        cy.SaveClick("**/shipment", "#ShipmentCreatebtn", "Shipment Saved Successfully")

    })

    it("Create Direct Export Ocean FCL Shipment", () => {

        cy.OpenNewShipmentWizard("Direct");
        cy.FillShipmentDefaultFields("E","O","FCLD"); 
        cy.ValidateValue("#Shipment_ShipmentCustomerTypeCode", "Shipper")

       
        cy.SaveClick("**/shipment", "#ShipmentCreatebtn", "Shipment Saved Successfully")

    })

    it("Create Direct Export Ocean LCL Shipment", () => {

        cy.OpenNewShipmentWizard("Direct");
        cy.FillShipmentDefaultFields("E","O","LCLD"); 
        cy.ValidateValue("#Shipment_ShipmentCustomerTypeCode", "Shipper")
        
        cy.SaveClick("**/shipment", "#ShipmentCreatebtn", "Shipment Saved Successfully")

    })

})

