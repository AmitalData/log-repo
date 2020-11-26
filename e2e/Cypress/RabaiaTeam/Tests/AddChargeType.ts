/// <reference types="cypress" />

describe("Charges Types", () => {

    it("Add Charge Type", () => {

        cy.Login()
        
        cy.Click("#GeneralMHMaintenance")
        cy.Click("#BIL")
        cy.Click("#MaintenanceItemMTCT")
        cy.Click("#NewButton_ChargesType")

        cy.FillRandomString("#ChargesType_Code", 4, true, true)
        cy.FillLogTextBox("#ChargesType_EnglishName", "AbedTest", true)
        cy.FillLogLov("#ChargesType_ChargesGroupId", "NONE", true)
        cy.FillLogLov("#ChargesType_MeasurementId", "FIXD", true)

        cy.Click("button", "Next")
        cy.ToggleCheckBox("#ChargesType_IsAutoDisplayInShipment")
        cy.ToggleCheckBox("#ChargesType_IsExport")
        cy.Click("button", "Next")
        cy.SaveClick("**/chargestypes", "button", "Finish")

    })

})