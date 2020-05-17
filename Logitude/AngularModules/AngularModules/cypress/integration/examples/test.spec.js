/// <reference types="cypress" />

context('Actions', () => {
  /*beforeEach(() => {
    cy.visit('https://system.logitudeworld.com')
  })*/

  // https://on.cypress.io/interacting-with-elements

/*

        
       

*/
  it('redirect into logitude', () => {
	   
		//cy.visit('http://localhost:4200') 
		cy.visit('https://test.logitudeworld.com/staging') 
		//cy.request('http://localhost:9996/LinksGateway.aspx') 
  })
  it('.type() - type into a DOM element', () => {
		cy.get('#Email').type('protractor@test.com') 
		cy.get('#Password').type('!P123t456') 
        cy.get('#cmdLogin').click().then(()=>{
           // cy.url().should('include', '/angular')

        })
  })
})
