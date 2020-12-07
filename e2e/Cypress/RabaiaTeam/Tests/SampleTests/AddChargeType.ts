/// <reference types="cypress" />

describe("Add New Charge Type", () => {

    
    
    it('navigate to charge types', function () {
        cy.Login()    
        cy.Click("#GeneralMHMaintenance", null);
        cy.Click("#BIL", null);
        cy.Click("#MaintenanceItemMTCT", null);

    }); 

    it("add new charge type", () => {
  
        cy.Click("#NewButton_ChargesType", null);

        cy.FillRandomString("#ChargesType_Code", 4, true, true)
        cy.FillLogTextBox("#ChargesType_EnglishName", "RabaiaTest", true)
        cy.SelectLogLovFirstElement("#ChargesType_ChargesGroupId")
        cy.SelectLogLovFirstElement("#ChargesType_MeasurementId")

        cy.Click("button", "Next")
        cy.Click("button", "Next")
        //cy.SaveClick("**/chargestypes", "button", "Charge Type Saved Successfully", "Finish")

    })
})