/// <reference types="cypress" />
  

  //login
  it('Sucessfully Login ', () => {
    //  cy.visit('https://test.logitudeworld.com/test/')
    cy.visit('http://localhost:4200')
      cy.get('#Email').click()
      cy.get('#Email').clear()
      //cy.get('#Email').type('Raghad@protractor.com')
      cy.get('#Email').type('angular@fnarsoft.com')
      cy.get('#Password').click()
      cy.get('#Password').clear()
      //cy.get('#Password').type('!RS123Rs')
      cy.get('#Password').type('1')
      cy.get('#cmdLogin').click()
  
    })

it('Create New User', () => {
     // cy.wait(10000)
    //  cy.wait(100)
      cy.get("#GeneralMHMaintenance").click()
      cy.get('#null_Search').type('user')
      cy.get('#MaintenanceItemMTUS').click()
      cy.get('#NewUserId').click()
      let timeStamp = (new Date()).getTime()
      cy.get('#User_Email').type("Cypress14" + timeStamp + "@mail.com")
      cy.get('#PasswordId').type('123')
      cy.get('#retypePass').type('123')
      cy.get('#User_EnglishName').type("TestCypress" + timeStamp) 
      cy.get('#User_DepartmentId').type("Management") 
      cy.get(".DropDownListItem").eq(0).should('contain', 'Management').click();
      cy.get('#User_BranchId').type("main")
      cy.get(".DropDownListItem").eq(0).should('contain', 'Main Office').click();
      cy.get('#User_Notes').type("This is Test")
      cy.get('#row11').click()
      cy.get('#CheckBox_0_0_LBL').click()
      cy.get('#OKIdButton').click()

    })

it('Search For User', () => {
      cy.get('#User_Search').type('testcypress')
      cy.get(".ListBoxItem").eq(0).click()

    })
  
  
    it('Edit User', () => {
      cy.get('#User_Notes').type('Test cypress')
      cy.wait(1000)
      cy.get("#User-Save").click({ force: true })

    })
  



