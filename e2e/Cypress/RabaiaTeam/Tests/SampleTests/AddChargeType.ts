/// <reference types="cypress" />

describe("Add New Charge Type", () => {

    
    
    it('navigate to charge types', function () {
        cy.Login()    
        cy.Click("#GeneralMHMaintenance");
        cy.Click("#BIL");
        cy.Click("#MaintenanceItemMTCT");

    }); 

    it("add new charge type", () => {
  
        cy.Click("#NewButton_ChargesType");

        cy.FillRandomString("#ChargesType_Code", 4, true, true)
        cy.FillLogTextBox("#ChargesType_EnglishName", "RabaiaTest", true)
        cy.SelectLogLovFirstElement("#ChargesType_ChargesGroupId")
        cy.SelectLogLovFirstElement("#ChargesType_MeasurementId")

        cy.Click("button", "Next")
        cy.Click("button", "Next")
        //cy.SaveClick("**/chargestypes", "button", "Charge Type Saved Successfully", "Finish")

    })
})