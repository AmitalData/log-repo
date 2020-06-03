/// <reference types="cypress" />

context('Actions', () => {
  /*beforeEach(() => {
    cy.visit('https://system.logitudeworld.com')
  })*/

 
    it('Redirect into logitude', () => { 
		//cy.visit('http://192.168.1.100/test') 
		//cy.visit('https://test.logitudeworld.com/staging') 
        //cy.request('http://localhost:9996/LinksGateway.aspx') 
          cy.visit('http://localhost:4200')
    }) 
    it('Login succeeded', () => {
	  //cy.get('#Email').type('protractor@test.com') 
	  //cy.get('#Password').type('!P123t456') 
      cy.get('#cmdLogin').click();
      cy.server();
      cy.route('/api/ObjectTableLastUpdate/GetLastTableUpdateDate/*').as('ChachedDataLoaded')
      cy.wait('@ChachedDataLoaded') 
    })

    it('Moving to Shipment Succeeded', () => {
        cy.get('#GeneralMHOperations').click()
        cy.get('#SHIP').click() 
    })

    it('Create Shipment', () => {
      
        cy.get('#HelperNotes_0_0').click()
        cy.get('#NEWDIRECT').click({ force: true }) 

    })
     

})
